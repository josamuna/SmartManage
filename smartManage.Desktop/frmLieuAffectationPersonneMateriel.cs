﻿using ManageUtilities;
using smartManage.Model;
using smartManage.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace smartManage.Desktop
{
    public partial class frmLieuAffectationPersonneMateriel : Form, ICRUDGeneral, ICallMainForm
    {
        BindingSource bdsrc = new BindingSource();
        bool blnModifie = false;
        private clslieu_affectation materiel = new clslieu_affectation();
        int? newID = null;

        //Delegate utilisation des threads
        private delegate void LoadSomeData(string threadName);

        //Timer for automatically unload thread for update comboBox on DropDown event
        System.Timers.Timer tempsActualiseCombo = null;

        //Timer for automatically unload thread for SelectionChange DataGridView event
        System.Timers.Timer tempsSelectionChangeDataGrid = null;

        //To be deleted
        //System.Timers.Timer tempsRefreshData = null;

        //Timer for automatically unload for load form
        System.Timers.Timer tempsLoadForm;

        //Timer for automatically unload for load Datagrid
        System.Timers.Timer tempsLoadDataGrid;

        //Timer for automatically unload thread when FormActivated event occurs in form
        System.Timers.Timer tempsActivateForm = null;

        //Timer for automatically set default cursor to form
        System.Timers.Timer tempsStopWaitCursor = null;

        //All thread for loading values
        Thread tLoadForm = null;
        Thread tLoadDataGrid = null;
        Thread tSelectionChangeDataGrid = null;
        Thread tActualiseComb = null;
        Thread tStopWaitCursor = null;

        ////bool firstLoad = false;
        ResourceManager stringManager = null;

        public frmPrincipal Principal
        {
            get;
            set;
        }

        public frmLieuAffectationPersonneMateriel()
        {
            InitializeComponent();
            //Initialisation des Resources
            Assembly _assembly = Assembly.Load("ResourcesData");
            stringManager = new ResourceManager("ResourcesData.Resource", _assembly);
        }

        #region Methods For THREAD
        private void UnloadThreadRessource(Thread thread)
        {
            if (thread != null)
            {
                thread.Abort();
                thread = null;
            }
        }

        private void LoadForm(string threadName)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                List<clsAC> lstAC = new List<clsAC>();
                lstAC = clsMetier.GetInstance().getAllClsAC();
                cboACSearch.DataSource = lstAC;
                this.setMembersallcbo(cboACSearch, "Designation", "Code_str");
                cboAC.DataSource = lstAC;
                this.setMembersallcbo(cboAC, "Designation", "Code_str");
                cboPersonne.DataSource = clsMetier.GetInstance().getAllClspersonne();
                this.setMembersallcbo(cboPersonne, "NomComplet", "Id");
                cboFonction.DataSource = clsMetier.GetInstance().getAllClsfonction();
                this.setMembersallcbo(cboFonction, "Designation", "Id");
                cboTypeLieuAffect.DataSource = clsMetier.GetInstance().getAllClstype_lieu_affectation();
                this.setMembersallcbo(cboTypeLieuAffect, "Designation", "Id");
                cboDesignation.DataSource = clsMetier.GetInstance().getAllClslieu_affectation();
                this.setMembersallcbo(cboDesignation, "Designation", "Designation");

                List<ComboBox> lstCombo = new List<ComboBox>() { cboAC, cboACSearch, cboPersonne, cboFonction, cboTypeLieuAffect };

                SetSelectedIndexComboBox(lstCombo);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadComboBoxMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadComboBoxCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur de chargement des listes déroulantes de gauche : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadComboBoxMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadComboBoxCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur de chargement des listes déroulantes de gauche : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
        }

        private void ExecuteLoadForm()
        {
            try
            {
                LoadSomeData loadForm = new LoadSomeData(LoadForm);

                this.Invoke(loadForm, "tLoadForm");
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Erreur lors du chargement des listes déroulantes, {0}", ex.Message), "Chargement listes déroulantes", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void LoadDataGrid(string threadName)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                //Rechargement combo des designation
                cboDesignation.DataSource = clsMetier.GetInstance().getAllClslieu_affectation();
                this.setMembersallcbo(cboDesignation, "Designation", "Designation");

                if (chkAll.Checked)
                    bdsrc.DataSource = clsMetier.GetInstance().getAllClslieu_affectation();
                else
                    bdsrc.DataSource = clsMetier.GetInstance().getAllClslieu_affectation_AC(cboACSearch.SelectedValue.ToString());

                Principal.SetDataSource(bdsrc);

                dgv.DataSource = bdsrc;

                //Here we sotp waitCursor if there are not records in BindinSource
                if (bdsrc.Count == 0)
                {
                    ExecuteThreadStopWaitCursor();
                    MessageBox.Show(stringManager.GetString("StringNoDataLoadBindingMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringNoDataLoadBindingCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadDataMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadDataCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors du chargement des données : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadDataMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadDataCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors du chargement des données : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
        }

        private void ExecuteLoadDataGrid()
        {
            try
            {
                LoadSomeData datagrid = new LoadSomeData(LoadDataGrid);

                this.Invoke(datagrid, "tLoadDataGrid");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadDtgvMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadDataCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors du chargement du DataGrid : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadDtgvMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadDataCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors du chargement du DataGrid : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
        }

        private void DoExecuteSelectionDataGrid(string threadName)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                BindingList();
                blnModifie = true;
                Principal.ActivateOnNewSelectionChangeDgvCommandButtons(true);
                int id_personne = -1;

                if (bdsrc.Count > 0)
                {
                    pbPhoto.Image = null;

                    if (!string.IsNullOrEmpty(cboPersonne.Text))
                    {
                        id_personne = (int)((clslieu_affectation)bdsrc.Current).Id_personne;

                        //Load Picture of personne here                      
                        pbPhoto.Image = clsTools.Instance.GetImageFromByte(clsMetier.GetInstance().getClspersonne(id_personne).Photo);
                    }
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedSelectRecordDtgvMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedSelectRecordDtgvCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur de sélection dans le DataGrid : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
        }

        private void ExecuteSelectionDataGrid()
        {
            LoadSomeData selectDataGrid = new LoadSomeData(DoExecuteSelectionDataGrid);
            
            try
            {
                this.Invoke(selectDataGrid, "tSelectedItemDataGrid");
            }
            catch 
            {
                blnModifie = false;
                Principal.ActivateOnSelectionChangeDgvExceptionCommandButtons(false);
            }
        }

        private void DoSetDefaultCursor(string threadName)
        {
            this.Cursor = Cursors.Default;
        }

        private void ExecuteDefaultCursor()
        {
            LoadSomeData defaultCurs = new LoadSomeData(DoSetDefaultCursor);

            try
            {
                this.Invoke(defaultCurs, "DoSetDefaultCursor");
            }
            catch { }
        }

        #region Actualise value in ComboBox
        private void CallActualiseComboBoxModification(string threadName)
        {
            //Actualisation des combobox si modification
            try
            {
                if (!string.IsNullOrEmpty(Properties.Settings.Default.strFormModifieSubForm))
                {
                    if (Properties.Settings.Default.strFormModifieSubForm.Equals(FormActualisation.frmAC.ToString()))
                    {
                        cboAC.DataSource = clsMetier.GetInstance().getAllClsAC();
                        this.setMembersallcbo(cboAC, "Designation", "Code_str");
                    }
                    if (Properties.Settings.Default.strFormModifieSubForm.Equals(FormActualisation.frmFonction.ToString()))
                    {
                        cboFonction.DataSource = clsMetier.GetInstance().getAllClsfonction();
                        this.setMembersallcbo(cboFonction, "Designation", "Id");
                    }
                    if (Properties.Settings.Default.strFormModifieSubForm.Equals(FormActualisation.frmTypeLieuAffectation.ToString()))
                    {
                        cboTypeLieuAffect.DataSource = clsMetier.GetInstance().getAllClstype_lieu_affectation();
                        this.setMembersallcbo(cboTypeLieuAffect, "Designation", "Id");
                    }
                }

                Properties.Settings.Default.strFormModifieSubForm = "";
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedRefreshLoadComboMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedRefreshLoadComboCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec d'actualisation de la liste déroulante : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedRefreshLoadComboMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedRefreshLoadComboCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec d'actualisation de la liste déroulante : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
        }

        private void ActualiseComboBoxModification()
        {
            try
            {
                LoadSomeData actualiseCboModifie = new LoadSomeData(CallActualiseComboBoxModification);

                this.Invoke(actualiseCboModifie, "tActualiseComb");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadComboBoxMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadComboBoxCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors de l'actualisation de la liste deroulante : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadComboBoxMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadComboBoxCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors de l'actualisation de la liste deroulante : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
        }

        private void TempsActualiseCombo_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (tActualiseComb != null)
            {
                if (!tActualiseComb.IsAlive)
                {
                    tempsActualiseCombo.Enabled = false;
                    tActualiseComb.Abort();
                    tActualiseComb = null;

                    ExecuteThreadStopWaitCursor();
                }
            }
        }

        private void TempsSelectionChangeDataGrid_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (tSelectionChangeDataGrid != null)
            {
                if (!tSelectionChangeDataGrid.IsAlive)
                {
                    tempsSelectionChangeDataGrid.Enabled = false;
                    tSelectionChangeDataGrid.Abort();
                    tSelectionChangeDataGrid = null;

                    ExecuteThreadStopWaitCursor();

                    try
                    {
                        SafeNativeMethods.SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, (UIntPtr)(-1), (UIntPtr)(-1));
                    }
                    catch (DllNotFoundException ex)
                    {
                        ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Réduction mémoire utilisée : " + ex.GetType().ToString() + " : " + ex.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                    }
                    catch (System.ComponentModel.Win32Exception ex)
                    {
                        ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Réduction mémoire utilisée : " + ex.GetType().ToString() + " : " + ex.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                    }
                }
            }
            else
            {
                tempsStopWaitCursor.Enabled = false;
            }
        }

        private void TempsActivateForm_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (tLoadForm != null)
            {
                if (!tLoadForm.IsAlive)
                {
                    tempsActivateForm.Enabled = false;
                    tLoadForm.Abort();
                    tLoadForm = null;

                    ExecuteThreadStopWaitCursor();
                }
            }
        }

        private void TempsStopWaitCursor_Elapsed(object sender, ElapsedEventArgs e)
        {
            List<Thread> lstThread = new List<Thread>();

            if (tStopWaitCursor != null)
            {
                if (!tStopWaitCursor.IsAlive)
                {
                    if (tLoadDataGrid != null)
                        lstThread.Add(tLoadDataGrid);
                    else if (tSelectionChangeDataGrid != null)
                        lstThread.Add(tSelectionChangeDataGrid);
                    else if (tLoadForm != null)
                        lstThread.Add(tLoadForm);
                    else if (tActualiseComb != null)
                        lstThread.Add(tActualiseComb);

                    bool[] tb = { };
                    int count = 1;

                    foreach (Thread t in lstThread)
                    {
                        if (!t.IsAlive)
                            count++;
                    }

                    if (count == tb.Length)
                    {
                        tempsStopWaitCursor.Enabled = false;
                        tStopWaitCursor.Abort();
                        tStopWaitCursor = null;
                    }
                }
            }
        }

        private void DoActualiseDropDown()
        {
            try
            {
                tempsActualiseCombo.Enabled = true;
                tempsActualiseCombo.Elapsed += TempsActualiseCombo_Elapsed;

                if (tActualiseComb == null)
                {
                    tActualiseComb = new Thread(new ThreadStart(ActualiseComboBoxModification));
                    tActualiseComb.Start();
                }
            }
            catch { }
        }

        private void ExecuteThreadStopWaitCursor()
        {
            tempsStopWaitCursor.Enabled = true;
            tempsStopWaitCursor.Elapsed += TempsStopWaitCursor_Elapsed;

            try
            {
                if (tStopWaitCursor == null)
                {
                    tStopWaitCursor = new Thread(new ThreadStart(ExecuteDefaultCursor));
                    tStopWaitCursor.Start();
                }
                else
                {
                    tStopWaitCursor.Abort();
                    tStopWaitCursor = null;

                    tStopWaitCursor = new Thread(new ThreadStart(ExecuteDefaultCursor));
                    tStopWaitCursor.Start();
                }
            }
            catch { }
        }

        #endregion

        #endregion

        #region FOR BINDING
        private void setMembersallcbo(ComboBox cbo, string displayMember, string valueMember)
        {
            cbo.DisplayMember = displayMember;
            cbo.ValueMember = valueMember;
        }

        private void SetBindingControls(Control ctr, string ctr_prop, object objsrce, string obj_prop)
        {
            ctr.DataBindings.Clear();
            ctr.DataBindings.Add(ctr_prop, objsrce, obj_prop, true, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void BindingCls()
        {
            SetBindingControls(txtId, "Text", materiel, "Id");
            SetBindingControls(cboAC, "SelectedValue", materiel, "Code_ac");
            SetBindingControls(cboPersonne, "SelectedValue", materiel, "Id_personne");
            SetBindingControls(cboFonction, "SelectedValue", materiel, "Id_fonction");
            SetBindingControls(cboTypeLieuAffect, "SelectedValue", materiel, "Id_type_lieu_affectation");
            SetBindingControls(txtDateAffectation, "Text", materiel, "Date_affectation");
            SetBindingControls(cboDesignation, "Text", materiel, "Designation");
            
            SetBindingControls(txtCreateBy, "Text", materiel, "User_created");
            SetBindingControls(txtDateCreate, "Text", materiel, "Date_created");
            SetBindingControls(txtModifieBy, "Text", materiel, "User_modified");
            SetBindingControls(txtDateModifie, "Text", materiel, "Date_modified");                 
        }

        private void BindingList()
        {
            SetBindingControls(txtId, "Text", bdsrc, "Id");
            SetBindingControls(cboAC, "SelectedValue", bdsrc, "Code_ac");
            SetBindingControls(cboPersonne, "SelectedValue", bdsrc, "Id_personne");
            SetBindingControls(cboFonction, "SelectedValue", bdsrc, "Id_fonction");
            SetBindingControls(cboTypeLieuAffect, "SelectedValue", bdsrc, "Id_type_lieu_affectation");
            SetBindingControls(txtDateAffectation, "Text", bdsrc, "Date_affectation");
            SetBindingControls(cboDesignation, "Text", bdsrc, "Designation");

            SetBindingControls(txtCreateBy, "Text", bdsrc, "User_created");
            SetBindingControls(txtDateCreate, "Text", bdsrc, "Date_created");
            SetBindingControls(txtModifieBy, "Text", bdsrc, "User_modified");
            SetBindingControls(txtDateModifie, "Text", bdsrc, "Date_modified");
        }
        #endregion

        private void SetSelectedIndexComboBox(List<ComboBox> cbo)
        {
            foreach (ComboBox cmb in cbo)
                if (cmb.Items.Count > 0)
                    cmb.SelectedIndex = 0;
        }

        private void RefreshLoadDataGrid()
        {
            tempsLoadDataGrid.Enabled = true;
            tempsLoadDataGrid.Elapsed += TempsLoadDataGrid_Elapsed;

            try
            {
                if (tLoadDataGrid == null)
                {
                    tLoadDataGrid = new Thread(new ThreadStart(ExecuteLoadDataGrid));
                    tLoadDataGrid.Start();
                }
                else
                {
                    tLoadForm.Abort();
                    tLoadForm = null;
                }
            }
            catch { }
        }

        private void RefreshLoadForm()
        {
            tempsLoadForm.Enabled = true;
            tempsLoadForm.Elapsed += TempsLoadForm_Elapsed;

            try
            {
                if (tLoadForm == null)
                {
                    tLoadForm = new Thread(new ThreadStart(ExecuteLoadForm));
                    tLoadForm.Start();
                }
                else
                {
                    tLoadForm.Abort();
                    tLoadForm = null;
                }
            }
            catch { }
        }

        private void TempsLoadDataGrid_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (tLoadDataGrid != null)
            {
                if (!tLoadDataGrid.IsAlive)
                {
                    tempsLoadDataGrid.Enabled = false;

                    tLoadDataGrid.Abort();
                    tLoadDataGrid = null;

                    ExecuteThreadStopWaitCursor();

                    try
                    {
                        SafeNativeMethods.SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, (UIntPtr)(-1), (UIntPtr)(-1));
                    }
                    catch (DllNotFoundException ex)
                    {
                        ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Réduction mémoire utilisée : " + ex.GetType().ToString() + " : " + ex.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                    }
                    catch (System.ComponentModel.Win32Exception ex)
                    {
                        ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Réduction mémoire utilisée : " + ex.GetType().ToString() + " : " + ex.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                    }
                }
            }
        }

        private void TempsLoadForm_Elapsed(object sender, ElapsedEventArgs e)
        {
            if(tLoadForm != null)
            {
                if(!tLoadForm.IsAlive)
                {
                    tempsLoadForm.Enabled = false;

                    tLoadForm.Abort();
                    tLoadForm = null;

                    ExecuteThreadStopWaitCursor();
                }               
            }
        }

        public void New()
        {
            try
            {
                //Initialise object class
                materiel = new clslieu_affectation();

                Principal.ActivateOnNewCommandButtons(true);
                blnModifie = false;

                BindingCls();

                //Set the new ID
                if (newID == null)
                    newID = clsMetier.GetInstance().GenerateLastID("lieu_affectation");
                txtId.Text = newID.ToString();
                txtCreateBy.Text = Properties.Settings.Default.UserConnected;
                txtDateCreate.Text = DateTime.Now.ToString();
            }
            catch (ArgumentException ex)
            {
                Principal.ActivateOnNewCommandButtons(false);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors de la création d'un nouvel enregistrement : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (NullReferenceException ex)
            {
                Principal.ActivateOnNewCommandButtons(false);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors de la création d'un nouvel enregistrement : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                Principal.ActivateOnNewCommandButtons(false);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors de la création d'un nouvel enregistrement : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
        }

        public void Search(string criteria)
        {
            try
            {
                if (dgv.RowCount == 0)
                    return;
                else
                {
                    if (string.IsNullOrEmpty(criteria))
                    {
                        this.RefreshRec();
                        return;
                    }
                    else
                    {
                        List<clslieu_affectation> lstItemSearch = new List<clslieu_affectation>();
                        lstItemSearch = clsMetier.GetInstance().getAllClslieu_affectation(criteria);

                        dgv.DataSource = lstItemSearch;
                    }
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedSearchMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedSearchCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de la recherche : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
        }

        public void Save()
        {
            try
            {
                if (!blnModifie)
                {
                    int record = materiel.inserts();
                    if (record == 0)
                        throw new CustomException(stringManager.GetString("StringZeroRecordAffectedMessage", CultureInfo.CurrentUICulture));
                    else
                        MessageBox.Show(stringManager.GetString("StringSuccessSaveMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringSuccessSaveCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                }
                else
                {
                    UpdateRec();
                }

                newID = null;
                RefreshLoadDataGrid();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedSaveUpdateMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedSaveUpdateCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de la mise à jour : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (CustomException ex)
            {
                Properties.Settings.Default.StringLogFile = ex.Message;
                MessageBox.Show(Properties.Settings.Default.StringLogFile, stringManager.GetString("StringFailedSaveUpdateCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de la mise à jour  : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (System.Data.SqlTypes.SqlTypeException ex)
            {
                Properties.Settings.Default.StringLogFile = ex.Message;
                MessageBox.Show(Properties.Settings.Default.StringLogFile, stringManager.GetString("StringFailedSaveUpdateCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de la mise à jour  : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedSaveUpdateMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedSaveUpdateCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de la mise à jour : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
        }

        public void UpdateRec()
        {
            ((clslieu_affectation)bdsrc.Current).User_modified = Properties.Settings.Default.UserConnected;
            ((clslieu_affectation)bdsrc.Current).Date_modified = DateTime.Now;

            clslieu_affectation mat = new clslieu_affectation();
            mat = ((clslieu_affectation)bdsrc.Current);

            int record = mat.update(mat);
            if (record == 0)
                throw new CustomException(stringManager.GetString("StringZeroRecordAffectedMessage", CultureInfo.CurrentUICulture));
            else
                MessageBox.Show(stringManager.GetString("StringSuccessUpdateMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringSuccessUpdateCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
        }

        public void Delete()
        {
            try
            {
                if (blnModifie)
                {
                    DialogResult dr = MessageBox.Show(stringManager.GetString("StringPromptDeleteMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringPromptDeleteCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                    int record = 0;

                    if (dr == DialogResult.Yes)
                    {
                        record = materiel.delete(((clslieu_affectation)bdsrc.Current));
                        if (record == 0)
                            throw new CustomException(stringManager.GetString("StringZeroRecordAffectedMessage", CultureInfo.CurrentUICulture));
                        else
                            MessageBox.Show(stringManager.GetString("StringSuccessDeleteMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringSuccessDeleteCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly); newID = null;
                    }
                    else
                        MessageBox.Show(stringManager.GetString("StringCancelDeleteMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringCancelDeleteCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                }

                RefreshLoadDataGrid();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedDeleteMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedDeleteCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de la suppression : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (CustomException ex)
            {
                Properties.Settings.Default.StringLogFile = ex.Message;
                MessageBox.Show(Properties.Settings.Default.StringLogFile, stringManager.GetString("StringFailedDeleteCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de la suppression  : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedDeleteMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedDeleteCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de la suppression : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
        }

        public void Preview()
        {
            frmReportAffectationMateriel frm = new frmReportAffectationMateriel();
            frm.MdiParent = Principal;
            frm.Icon = this.Icon;
            frm.Show();
        }

        public void RefreshRec()
        {
            try
            {
                RefreshLoadDataGrid();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedRefreshMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedRefreshCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors de l'actualisation : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedRefreshMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedRefreshCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors de l'actualisation : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
        }

        private void dgv_SelectionChanged(object sender, EventArgs e)
        {
            tempsSelectionChangeDataGrid.Enabled = true;
            tempsSelectionChangeDataGrid.Elapsed += TempsSelectionChangeDataGrid_Elapsed;

            try
            {
                //Executed in thread
                if (tSelectionChangeDataGrid == null)
                {
                    tSelectionChangeDataGrid = new Thread(new ThreadStart(ExecuteSelectionDataGrid));
                    tSelectionChangeDataGrid.Start();
                }
            }
            catch { }
        }

        private void cboAC_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void lblAddAC_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAC frm = new frmAC();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddFonction_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmFonction frm = new frmFonction();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddTypeLieuAffect_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmTypeLieuAffectation frm = new frmTypeLieuAffectation();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void cboFonction_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void cboTypeLieuAffect_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAll.Checked)
                cboACSearch.Enabled = false;
            else
                cboACSearch.Enabled = true;
        }

        private void frmLieuAffectationPersonne_Load(object sender, EventArgs e)
        {
            chkAll.Checked = false;

            //Initialise timers
            tempsActualiseCombo = new System.Timers.Timer();
            tempsActualiseCombo.Interval = 100;

            tempsSelectionChangeDataGrid = new System.Timers.Timer();
            tempsSelectionChangeDataGrid.Interval = 100;

            tempsLoadForm = new System.Timers.Timer();
            tempsLoadForm.Interval = 100;

            tempsLoadDataGrid = new System.Timers.Timer();
            tempsLoadDataGrid.Interval = 100;

            tempsActivateForm = new System.Timers.Timer();
            tempsActivateForm.Interval = 100;

            tempsStopWaitCursor = new System.Timers.Timer();
            tempsStopWaitCursor.Interval = 50;

            //executed in independant thread
            RefreshLoadForm();
        }

        private void frmLieuAffectationPersonne_FormClosed(object sender, FormClosedEventArgs e)
        {
            Principal.SetValuesLabel(Properties.Settings.Default.UserConnected, "Attente d'une action de l'utilisateur");

            //Reinitialise all Thread
            try
            {
                this.UnloadThreadRessource(tLoadDataGrid);
                this.UnloadThreadRessource(tSelectionChangeDataGrid);
                this.UnloadThreadRessource(tLoadForm);
                this.UnloadThreadRessource(tActualiseComb);
                this.UnloadThreadRessource(tStopWaitCursor);
            }
            catch { }

            Principal.ApplyDefaultStatusBar(Principal, Properties.Settings.Default.UserConnected);
            //Affecte Activate BindingNavigator
            Principal.ActivateMainBindingSource(Principal);
        }

        private void frmLieuAffectationPersonne_Activated(object sender, EventArgs e)
        {
            Principal.SetValuesLabel(Properties.Settings.Default.UserConnected, "Lieu d'affectation d'une personne (Agent) ou d'un équipement");
            Principal.SetCurrentICRUDChildForm(this);

            ////try
            ////{
            ////    if (firstLoad)
            ////    {
            ////        tempsActivateForm.Enabled = true;
            ////        tempsActivateForm.Elapsed += TempsActivateForm_Elapsed;

            ////        if (tLoadForm == null)
            ////        {
            ////            tLoadForm = new Thread(new ThreadStart(ExecuteLoadForm));
            ////            tLoadForm.Start();
            ////        }
            ////    }
            ////}
            ////catch { }

            ////firstLoad = true;
            //Affecte Activate BindingNavigator
            Principal.ActivateMainBindingSource(Principal);
        }

        private void cmdAffiche_Click(object sender, EventArgs e)
        {
            RefreshLoadDataGrid();
        }
    }
}
