using ManageUtilities;
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
    public partial class frmSignataire : Form, ICRUDGeneral, ICallMainForm
    {
        BindingSource bdsrc = new BindingSource();
        bool blnModifie = false;
        private clssignataire materiel = new clssignataire();
        int? newID = null;
        ResourceManager stringManager = null;

        //Delegate utilisation des threads
        private delegate void LoadSomeData(string threadName);

        //Timer for automatically unload thread for update comboBox on DropDown event
        System.Timers.Timer tempsActualiseCombo = null;

        //Timer for automatically unload thread for SelectionChange DataGridView event
        System.Timers.Timer tempsSelectionChangeDataGrid = null;

        //Timer for automatically unload thread for RefreshData method
        System.Timers.Timer tempsRefreshData = null;

        //Timer for automatically unload thread when FormActivated event occurs in form
        System.Timers.Timer tempsActivateForm = null;

        //Timer for automatically set default cursor to form
        System.Timers.Timer tempsStopWaitCursor = null;

        //All thread for loading values
        Thread tDataGrid = null;
        Thread tSelectionChangeDataGrid = null;
        Thread tLeftCombo = null;
        Thread tActualiseComb = null;
        Thread tStopWaitCursor = null;

        //Boolean variables for photo
        bool blnPhoto = false;

        ////bool firstLoad = false;

        public frmPrincipal Principal
        {
            get;
            set;
        }

        public frmSignataire()
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

        private void LoadLeftCombo(string threadName)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                cboPersonne.DataSource = clsMetier.GetInstance().getAllClspersonne();
                this.setMembersallcbo(cboPersonne, "NomComplet", "Id");
                cboAC.DataSource = clsMetier.GetInstance().getAllClsAC();
                this.setMembersallcbo(cboAC, "Designation", "Code_str");

                List<ComboBox> lstCombo = new List<ComboBox>() { cboPersonne, cboPersonne };

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

        private void ExecuteLeftCombo()
        {
            try
            {
                LoadSomeData leftCbo = new LoadSomeData(LoadLeftCombo);

                this.Invoke(leftCbo, "tLeftCombo");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadComboBoxMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadComboBoxCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur de chargement des listes déroulantes de gauche dans le thread : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadComboBoxMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadComboBoxCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur de chargement des listes déroulantes de gauche dans le thread : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
        }

        private void LoadDataGrid(string threadName)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                bdsrc.DataSource = clsMetier.GetInstance().getAllClssignataire();
                Principal.SetDataSource(bdsrc);

                dgv.DataSource = bdsrc;

                //Here we sotp waitCursor if there are not records in BindinSource
                if (bdsrc.Count == 0)
                {
                    ExecuteThreadStopWaitCursor();
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

        private void ExecuteDataGrid()
        {
            try
            {
                LoadSomeData datagrid = new LoadSomeData(LoadDataGrid);

                this.Invoke(datagrid, "tDataGrid");
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
        }

        private void TempsActivateForm_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (tDataGrid != null)
            {
                if (!tDataGrid.IsAlive)
                {
                    tempsActivateForm.Enabled = false;
                    tDataGrid.Abort();
                    tDataGrid = null;

                    ExecuteThreadStopWaitCursor();
                }
            }
        }

        private void TempsRefreshData_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (tLeftCombo != null || tDataGrid != null)
            {
                if (!tDataGrid.IsAlive)
                {
                    if (!tLeftCombo.IsAlive)
                    {
                        tempsRefreshData.Enabled = false;

                        tLeftCombo.Abort();
                        tLeftCombo = null;

                        tDataGrid.Abort();
                        tDataGrid = null;

                        tempsStopWaitCursor.Enabled = true;
                        tempsStopWaitCursor.Elapsed += TempsStopWaitCursor_Elapsed;

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
        }

        private void TempsStopWaitCursor_Elapsed(object sender, ElapsedEventArgs e)
        {
            List<Thread> lstThread = new List<Thread>();

            if (tStopWaitCursor != null)
            {
                if (!tStopWaitCursor.IsAlive)
                {
                    if (tDataGrid != null)
                        lstThread.Add(tDataGrid);
                    else if (tSelectionChangeDataGrid != null)
                        lstThread.Add(tSelectionChangeDataGrid);
                    else if (tLeftCombo != null)
                        lstThread.Add(tLeftCombo);
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

        private void Bs_Parse(object sender, ConvertEventArgs e)
        {
            try
            {
                if (e.Value == null) e = null;
                else e.Value = (clsTools.Instance.GetBytesFromImage(pbPhoto.Image));
            }
            catch (NullReferenceException)
            {
                //Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de conversion de la photo : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                //ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (ArgumentException)
            {
                //Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de conversion de la photo : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                //ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
        }

        void binding_Format(object sender, ConvertEventArgs e)
        {
            try
            {
                pbPhoto.Tag = null;
                if (e.DesiredType != typeof(System.Drawing.Image) || e.Value.ToString() == e.DesiredType.FullName || e.Value.ToString() == e.DesiredType.Name) return;
                if (e.Value.ToString() == "System.Drawing.Bitmap") return;
                if (e.Value == null || e.Value.ToString() == "")
                {
                    pbPhoto.Tag = "1";
                    pbPhoto.Image = null;
                }
                else
                {
                    string imagestr = e.Value.ToString();
                    e.Value = (clsTools.Instance.GetImageFromByte((Byte[])e.Value));
                }
            }
            catch (NullReferenceException ex)
            {
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors du binding de la photo : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (InvalidOperationException ex)
            {
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors du binding de la photo : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
        }

        private void bingImg(PictureBox pb, Object src, string ctrProp, string obj)
        {
            pb.DataBindings.Clear();
            Binding binding = new Binding(ctrProp, src, obj, true, DataSourceUpdateMode.OnPropertyChanged);
            binding.Parse += new ConvertEventHandler(Bs_Parse);
            binding.Format += new ConvertEventHandler(binding_Format);
            pb.DataBindings.Add(binding);
        }

        private void BindingCls()
        {
            SetBindingControls(txtId, "Text", materiel, "Id");
            SetBindingControls(cboPersonne, "SelectedValue", materiel, "Id_personne");
            SetBindingControls(cboAC, "SelectedValue", materiel, "Code_ac");
            bingImg(pbPhoto, materiel, "Image", "Signature_specimen");

            SetBindingControls(txtCreateBy, "Text", materiel, "User_created");
            SetBindingControls(txtDateCreate, "Text", materiel, "Date_created");
            SetBindingControls(txtModifieBy, "Text", materiel, "User_modified");
            SetBindingControls(txtDateModifie, "Text", materiel, "Date_modified");                 
        }

        private void BindingList()
        {
            SetBindingControls(txtId, "Text", bdsrc, "Id");
            SetBindingControls(cboPersonne, "SelectedValue", bdsrc, "Id_personne");
            SetBindingControls(cboAC, "SelectedValue", bdsrc, "Code_ac");
            bingImg(pbPhoto, bdsrc, "Image", "Signature_specimen");

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

        private void RefreshData()
        {
            tempsRefreshData.Enabled = true;
            tempsRefreshData.Elapsed += TempsRefreshData_Elapsed;

            try
            {
                if (tLeftCombo == null)
                {
                    tLeftCombo = new Thread(new ThreadStart(ExecuteLeftCombo));
                    tLeftCombo.Start();

                    tDataGrid = new Thread(new ThreadStart(ExecuteDataGrid));
                    tDataGrid.Start();
                }
                else
                {
                    tLeftCombo.Abort();
                    tLeftCombo = null;
                }
            }
            catch { }

            if (bdsrc.Count == 0)
            {
                Principal.ActivateOnLoadCommandButtons(false);
            }
        }

        public void New()
        {
            try
            {
                //Initialise object class
                materiel = new clssignataire();

                Principal.ActivateOnNewCommandButtons(true);
                blnModifie = false;

                BindingCls();

                //Set the new ID
                if (newID == null)
                    newID = clsMetier.GetInstance().GenerateLastID("signataire");
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
                        List<clssignataire> lstItemSearch = new List<clssignataire>();
                        lstItemSearch = clsMetier.GetInstance().getAllClssignataire(criteria);

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
                RefreshData();
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
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedSaveUpdateMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedSaveUpdateCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de la mise à jour : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
        }

        public void UpdateRec()
        {
            ((clssignataire)bdsrc.Current).User_modified = Properties.Settings.Default.UserConnected;
            ((clssignataire)bdsrc.Current).Date_modified = DateTime.Now;

            clssignataire mat = new clssignataire();
            mat = ((clssignataire)bdsrc.Current);
            if (blnPhoto)
            {
                mat.Signature_specimen = materiel.Signature_specimen;
                blnPhoto = false;
            }

            int record = materiel.update(mat);
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
                        record = materiel.delete(((clssignataire)bdsrc.Current));
                        if (record == 0)
                            throw new CustomException(stringManager.GetString("StringZeroRecordAffectedMessage", CultureInfo.CurrentUICulture));
                        else
                            MessageBox.Show(stringManager.GetString("StringSuccessDeleteMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringSuccessDeleteCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly); newID = null;
                    }
                    else
                        MessageBox.Show(stringManager.GetString("StringCancelDeleteMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringCancelDeleteCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                }

                RefreshData();
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
            //frmReportSignataire frm = new frmReportSignataire();
            //frm.MdiParent = Principal;
            //frm.Icon = this.Icon;
            //frm.Show();
            Properties.Settings.Default.StringLogFile = "Rapport non encore ajouté";
            MessageBox.Show(Properties.Settings.Default.StringLogFile, stringManager.GetString("StringFailedLoadReportCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
        }

        public void RefreshRec()
        {
            try
            {
                RefreshData();
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

        private void LoadPicture()
        {
            OpenFileDialog open;
            DialogResult result;
            LoadPicture(out open, out result);

            if (result == DialogResult.OK)
            {
                string strChaine = clsTools.Instance.LimiteImageSize1(open.FileName, 1024000, 1000, 1000);

                if (string.IsNullOrEmpty(strChaine))
                {
                    pbPhoto.Load(open.FileName);
                    blnPhoto = true;
                    materiel.Signature_specimen = clsTools.Instance.GetByteFromFile(open.FileName);
                }
                else
                {
                    Properties.Settings.Default.StringLogFile = strChaine;
                    MessageBox.Show(Properties.Settings.Default.StringLogFile, stringManager.GetString("StringFailedLoadPictureCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                    Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de chargement de la photo  : " + this.Name + " : " + strChaine;
                    ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                }
            }
        }

        private void LoadPicture(out OpenFileDialog open, out DialogResult result)
        {
            open = new OpenFileDialog();
            open.Title = stringManager.GetString("StringLoadPictureCaption1", CultureInfo.InvariantCulture);
            open.Filter = stringManager.GetString("StringPictureFilter2", CultureInfo.InvariantCulture);
            result = open.ShowDialog();
        }

        private void lblPhoto_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                LoadPicture();
            }
            catch (System.IO.FileNotFoundException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadPictureMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadPictureCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur de chargement de la photo1 : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (System.IO.IOException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadPictureMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadPictureCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur de chargement de la photo1 : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
        }

        private void pbPhoto_MouseHover(object sender, EventArgs e)
        {
            pbPhoto.Cursor = Cursors.Hand;
        }

        private void pbPhoto_MouseLeave(object sender, EventArgs e)
        {
            pbPhoto.Cursor = Cursors.Default;
        }

        private void frmPersonne_Activated(object sender, EventArgs e)
        {
            Principal.SetValuesLabel(Properties.Settings.Default.UserConnected, "Identification des signataires");
            Principal.SetCurrentICRUDChildForm(this);

            ////try
            ////{
            ////    if (firstLoad)
            ////    {
            ////        tempsActivateForm.Enabled = true;
            ////        tempsActivateForm.Elapsed += TempsActivateForm_Elapsed;

            ////        if (tDataGrid == null)
            ////        {
            ////            tDataGrid = new Thread(new ThreadStart(ExecuteDataGrid));
            ////            tDataGrid.Start();
            ////        }
            ////    }
            ////}
            ////catch { }

            ////firstLoad = true;
        }

        private void smnCtxPhoto_Click(object sender, EventArgs e)
        {
            try
            {
                LoadPicture();
            }
            catch (System.IO.FileNotFoundException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadPictureMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadPictureCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur de chargement de la photo1 : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (System.IO.IOException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadPictureMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadPictureCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur de chargement de la photo1 : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
        }

        private void cboPersonne_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void lblAddAC_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAC frm = new frmAC();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void frmSignataire_FormClosed(object sender, FormClosedEventArgs e)
        {
            Principal.SetValuesLabel(Properties.Settings.Default.UserConnected, "Attente d'une action de l'utilisateur");

            //Reinitialise all Thread
            try
            {
                this.UnloadThreadRessource(tDataGrid);
                this.UnloadThreadRessource(tSelectionChangeDataGrid);
                this.UnloadThreadRessource(tLeftCombo);
                this.UnloadThreadRessource(tActualiseComb);
                this.UnloadThreadRessource(tStopWaitCursor);
            }
            catch { }

            Principal.ApplyDefaultStatusBar(Principal, Properties.Settings.Default.UserConnected);
            //Affecte Activate BindingNavigator
            Principal.ActivateMainBindingSource(Principal);
        }

        private void frmSignataire_Load(object sender, EventArgs e)
        {
            //Initialise timers
            tempsActualiseCombo = new System.Timers.Timer();
            tempsActualiseCombo.Interval = 100;

            tempsSelectionChangeDataGrid = new System.Timers.Timer();
            tempsSelectionChangeDataGrid.Interval = 100;

            tempsRefreshData = new System.Timers.Timer();
            tempsRefreshData.Interval = 100;

            tempsActivateForm = new System.Timers.Timer();
            tempsActivateForm.Interval = 100;

            tempsStopWaitCursor = new System.Timers.Timer();
            tempsStopWaitCursor.Interval = 50;

            //Affecte MenuStrip
            pbPhoto.ContextMenuStrip = ctxMenuPhoto;
            //executed in many independant thread
            RefreshData();
        }

        private void cboAC_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }
    }
}
