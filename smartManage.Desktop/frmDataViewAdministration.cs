using ManageUtilities;
using smartManage.RadiusAdminModel;
using smartManage.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace smartManage.Desktop
{
    public partial class frmDataViewAdministration : Form, ICallMainForm, ICRUDGeneral
    {
        clsConnexion1 connection = new clsConnexion1();

        //Object des classes
        private clsnas nas = new clsnas();
        private clsradcheck radcheck = new clsradcheck();
        private clsradacct accounting = new clsradacct();
        private clsradpostauth postauth = new clsradpostauth();

        //Declaration des BindingSources
        BindingSource bdsrc_nas = new BindingSource();
        BindingSource bdsrc_user = new BindingSource();
        BindingSource bdsrc_user_multi = new BindingSource();
        BindingSource bdsrc_accounting = new BindingSource();
        BindingSource bdsrc_postauth = new BindingSource();

        bool blnModifie_nas = false;
        bool blnModifie_user = false;
        bool blnModifie_accounting = false;
        bool blnModifie_postauth = false;

        bool useCipherKey = false;

        int? newID_nas = null;
        int? newID_user = null;

        //Delegate utilisation des threads
        private delegate void LoadSomeData(string threadName);

        //Timer pour chagement Datagrid et actualisation (Refresh)
        System.Timers.Timer tempsLoadDataGrid = null;

        //Timer pour chagement Datagrid de generation
        System.Timers.Timer tempsLoadDataGridMulti = null;

        //Timer pour chagement Datagrid de generation avec fichier
        System.Timers.Timer tempsLoadDataGridMultiFile = null;

        //Timer pour chagement Datagrid de generation
        System.Timers.Timer tempsGenerateKey = null;

        //Timer pour enregistrer les data Générés
        System.Timers.Timer tempsSaveKey = null;

        //Timer pour exporter les data Générés
        System.Timers.Timer tempsExportKey = null;

        //Timer for automatically set default cursor to form
        System.Timers.Timer tempsStopWaitCursor = null;

        //Thread pour chargement DataGrid
        Thread tLoadDataGrid = null;
        Thread tStopWaitCursor = null;

        //Thread pour generation des cles
        Thread tSaveKey = null;
        Thread tExportKey = null;
        Thread tGenerateKey = null;
        Thread tLoadDataGridMulti = null;
        Thread tLoadDataGridMultiFile = null;

        ResourceManager stringManager = null;

        public frmDataViewAdministration()
        {
            InitializeComponent();
            //Initialisation des Resources
            Assembly _assembly = Assembly.Load("ResourcesData");
            stringManager = new ResourceManager("ResourcesData.Resource", _assembly);
        }

        public frmPrincipal Principal
        {
            get;
            set;
        }

        #region BINDING FOR GENERATE PASSWORD
        private void BindingList_User_multi()
        {
            //SetBindingControls(txtCodeUser, "Text", bdsrc_user, "id");
            //SetBindingControls(txtNomUser, "Text", bdsrc_user, "username");
            //SetBindingControls(cboAttributUser, "SelectedValue", bdsrc_user, "attribute");
            //SetBindingControls(cboOpUser, "SelectedValue", bdsrc_user, "op");
            //SetBindingControls(txtPasswordUser, "Text", bdsrc_user, "value");
            SetBindingControls(lblRecord, "Text", bdsrc_user_multi, "nbr_enreg");
            //SetBindingControls(txtPriorityUser, "Text", bdsrc_user, "priority");
            SetBindingControls(cboGroupe, "SelectedValue", bdsrc_user_multi, "groupname");
        }
        #endregion

        #region Methodes for BINDING
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

        #region GESTION LOAD
        private void BindingCls_Nas()
        {
            SetBindingControls(txtCodeNas, "Text", nas, "Id");
            SetBindingControls(txtIPNas1, "Text", nas, "Nasname");
            SetBindingControls(txtNomNas, "Text", nas, "Shortname");
            SetBindingControls(txtTypeNas1, "Text", nas, "Type");
            SetBindingControls(txtNumPortRadius, "Text", nas, "Ports");
            SetBindingControls(txtPasswordNas, "Text", nas, "Secret");
            SetBindingControls(txtServeur, "Text", nas, "Server");
            SetBindingControls(txtCaumunaute, "Text", nas, "Community");
            SetBindingControls(txtDecriptionNas, "Text", nas, "Description");
        }

        private void BindingList_Nas()
        {
            SetBindingControls(txtCodeNas, "Text", bdsrc_nas, "Id");
            SetBindingControls(txtIPNas1, "Text", bdsrc_nas, "Nasname");
            SetBindingControls(txtNomNas, "Text", bdsrc_nas, "Shortname");
            SetBindingControls(txtTypeNas1, "Text", bdsrc_nas, "Type");
            SetBindingControls(txtNumPortRadius, "Text", bdsrc_nas, "Ports");
            SetBindingControls(txtPasswordNas, "Text", bdsrc_nas, "Secret");
            SetBindingControls(txtServeur, "Text", bdsrc_nas, "Server");
            SetBindingControls(txtCaumunaute, "Text", bdsrc_nas, "Community");
            SetBindingControls(txtDecriptionNas, "Text", bdsrc_nas, "Description");
        }
        #endregion

        #region GENERATING KEY
        private void UnloadThreadRessource(Thread thread)
        {
            if (thread != null)
            {
                thread.Abort();
                thread = null;
            }
        }

        private void CallGenerateKey()
        {
            try
            {
                if (tGenerateKey == null)
                {
                    tGenerateKey = new Thread(new ThreadStart(ExecuteGenerateKey));
                    tGenerateKey.Start();
                }
                else
                {
                    tGenerateKey.Abort();
                    tGenerateKey = null;
                }
            }
            catch { }
        }

        private void CallSaveKey()
        {
            try
            {
                if (tSaveKey == null)
                {
                    tSaveKey = new Thread(new ThreadStart(ExecuteSaveKey));
                    tSaveKey.Start();
                }
                else
                {
                    tSaveKey.Abort();
                    tSaveKey = null;
                }
            }
            catch { }
        }

        private void CallExportKey()
        {
            try
            {
                if (tExportKey == null)
                {
                    tExportKey = new Thread(new ThreadStart(ExecuteExportKey));
                    tExportKey.Start();
                }
                else
                {
                    tExportKey.Abort();
                    tExportKey = null;
                }
            }
            catch { }
        }

        private void ExecuteGenerateKey()
        {
            try
            {
                LoadSomeData generate = new LoadSomeData(GenerateKey);

                this.Invoke(generate, "tGenerateKey");
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Erreur lors de la génération des mots de passe, {0}", ex.Message), "Génération des mots de passe", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void GenerateKey(string threadName)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                tempsGenerateKey.Enabled = true;
                tempsGenerateKey.Elapsed += TempsGenerateKey_Elapsed;

                if (bdsrc_user_multi.Count > 0)
                {
                    foreach (DataRow dtr in ((DataRowView)bdsrc_user_multi.Current).Row.Table.Rows)
                    {
                        string strUser = dtr["username"].ToString();
                        //Generate key
                        if(useCipherKey)
                            dtr["value"] = DoKey.Crypte(strUser, txtChipherKeyMultiple.Text).Substring(0, 4);
                        else
                            dtr["value"] = clsMetier1.GetInstance().GeneratePassword(strUser);

                        dtr.EndEdit();
                        ((DataRowView)bdsrc_user_multi.Current).Row.Table.AcceptChanges();
                    }
                    dgvUserMulti.DataSource = bdsrc_user_multi;
                }
                else
                    MessageBox.Show("Il n'a rien a généré !!!", "Génération mot de passe", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Erreur lors de la génération des mots de passe, {0}", ex.Message), "Génération des mots de passe", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }

            //Disable use cipherkey
            useCipherKey = false;
        }

        private void AfficheDataMulti()
        {
            try
            {
                if (tLoadDataGridMulti == null)
                {
                    tLoadDataGridMulti = new Thread(new ThreadStart(ExecuteLoadDataGridMulti));
                    tLoadDataGridMulti.Start();
                }
                else
                {
                    tLoadDataGridMulti.Abort();
                    tLoadDataGridMulti = null;
                }
            }
            catch { }
        }

        private void AfficheDataMultiFile()
        {
            try
            {
                if (tLoadDataGridMultiFile == null)
                {
                    tLoadDataGridMultiFile = new Thread(new ThreadStart(ExecuteLoadDataGridMultiFile));
                    tLoadDataGridMultiFile.Start();
                }
                else
                {
                    tLoadDataGridMultiFile.Abort();
                    tLoadDataGridMultiFile = null;
                }
            }
            catch { }
        }

        private void TempsGenerateKey_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (tGenerateKey != null)
            {
                if (!tGenerateKey.IsAlive)
                {
                    tempsGenerateKey.Enabled = false;
                    tGenerateKey.Abort();
                    tGenerateKey = null;

                    ExecuteThreadStopWaitCursor();

                    try
                    {
                        SafeNativeMethods.SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, (UIntPtr)(-1), (UIntPtr)(-1));
                    }
                    catch (DllNotFoundException ex)
                    {
                        ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Réduction mémoire utilisée : " + ex.GetType().ToString() + " : " + ex.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileNameRadiusStudent);
                    }
                    catch (System.ComponentModel.Win32Exception ex)
                    {
                        ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Réduction mémoire utilisée : " + ex.GetType().ToString() + " : " + ex.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileNameRadiusStudent);
                    }
                }
            }
        }

        private void ExecuteSaveKey()
        {
            try
            {
                LoadSomeData save = new LoadSomeData(SaveKey);

                this.Invoke(save, "tSaveKey");
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Erreur lors de l'enregistrement des données générées, {0}", ex.Message), "Enregistrement des données générées", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void SaveKey(string threadName)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                tempsSaveKey.Enabled = true;
                tempsSaveKey.Elapsed += TempsSaveKey_Elapsed;

                clsMetier1.GetInstance().insertClsradcheck_multiple_dtrowv((DataRowView)bdsrc_user_multi.Current);

                MessageBox.Show("Enregistrement effectué avec succès", "Enregistrement des données générées", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Erreur lors de l'enregistrement des données générées, {0}", ex.Message), "Enregistrement des données générées", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void TempsSaveKey_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (tSaveKey != null)
            {
                if (!tSaveKey.IsAlive)
                {
                    tempsSaveKey.Enabled = false;
                    tSaveKey.Abort();
                    tSaveKey = null;

                    ExecuteThreadStopWaitCursor();

                    try
                    {
                        SafeNativeMethods.SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, (UIntPtr)(-1), (UIntPtr)(-1));
                    }
                    catch (DllNotFoundException ex)
                    {
                        ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Réduction mémoire utilisée : " + ex.GetType().ToString() + " : " + ex.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileNameRadiusStudent);
                    }
                    catch (System.ComponentModel.Win32Exception ex)
                    {
                        ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Réduction mémoire utilisée : " + ex.GetType().ToString() + " : " + ex.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileNameRadiusStudent);
                    }
                }
            }
        }

        private void ExecuteExportKey()
        {
            try
            {
                LoadSomeData export = new LoadSomeData(ExportKey);

                this.Invoke(export, "tExportKey");
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Erreur lors de l'exportation des données générées, {0}", ex.Message), "Exportation des données générées", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void ExportKey(string threadName)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                tempsExportKey.Enabled = true;
                tempsExportKey.Elapsed += TempsExportKey_Elapsed;

                FolderBrowserDialog folder = new FolderBrowserDialog();
                folder.Description = "Sélection d'un emplacement";

                if (folder.ShowDialog() == DialogResult.OK)
                {
                    if (System.IO.Directory.Exists(folder.SelectedPath))
                    {
                        clsMetier1.GenerateFiles(bdsrc_user_multi, folder.SelectedPath);
                        MessageBox.Show("Enregistrement effectué avec succès", "Enregistrement des données générées", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    }
                    else throw new Exception("L'emplacement choisi n'existe pas !!!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Erreur lors de l'enregistrement des données générées, {0}", ex.Message), "Enregistrement des données générées", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void TempsExportKey_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (tExportKey != null)
            {
                if (!tExportKey.IsAlive)
                {
                    tempsExportKey.Enabled = false;
                    tExportKey.Abort();
                    tExportKey = null;

                    ExecuteThreadStopWaitCursor();

                    try
                    {
                        SafeNativeMethods.SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, (UIntPtr)(-1), (UIntPtr)(-1));
                    }
                    catch (DllNotFoundException ex)
                    {
                        ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Réduction mémoire utilisée : " + ex.GetType().ToString() + " : " + ex.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileNameRadiusStudent);
                    }
                    catch (System.ComponentModel.Win32Exception ex)
                    {
                        ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Réduction mémoire utilisée : " + ex.GetType().ToString() + " : " + ex.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileNameRadiusStudent);
                    }
                }
            }
        }

        private void ExecuteLoadDataGridMulti()
        {
            try
            {
                LoadSomeData loadDtMulti = new LoadSomeData(LoadDataGridMulti);

                this.Invoke(loadDtMulti, "tLoadDataGridMulti");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadDtgvMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadDataCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors du chargement du DataGrid : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileNameRadiusStudent);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadDtgvMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadDataCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors du chargement du DataGrid : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileNameRadiusStudent);
            }
        }

        private void LoadDataGridMulti(string threadName)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                tempsLoadDataGridMulti.Enabled = true;
                tempsLoadDataGridMulti.Elapsed += TempsLoadDataGridMulti_Elapsed;

                bdsrc_user_multi.DataSource = clsMetier1.GetInstance().getAllClsradcheck_dt();
                Principal.SetDataSource(bdsrc_user_multi);
                dgvUserMulti.DataSource = bdsrc_user_multi;

                cmdGenerate.Enabled = true;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadDtgvMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadDataCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors du chargement du DataGrid : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileNameRadiusStudent);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadDtgvMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadDataCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors du chargement du DataGrid : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileNameRadiusStudent);
            }
        }

        private void TempsLoadDataGridMulti_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (tLoadDataGridMulti != null)
            {
                if (!tLoadDataGridMulti.IsAlive)
                {
                    tempsLoadDataGridMulti.Enabled = false;
                    tLoadDataGridMulti.Abort();
                    tLoadDataGridMulti = null;

                    ExecuteThreadStopWaitCursor();

                    try
                    {
                        SafeNativeMethods.SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, (UIntPtr)(-1), (UIntPtr)(-1));
                    }
                    catch (DllNotFoundException ex)
                    {
                        ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Réduction mémoire utilisée : " + ex.GetType().ToString() + " : " + ex.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileNameRadiusStudent);
                    }
                    catch (System.ComponentModel.Win32Exception ex)
                    {
                        ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Réduction mémoire utilisée : " + ex.GetType().ToString() + " : " + ex.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileNameRadiusStudent);
                    }
                }
            }
        }

        private void ExecuteLoadDataGridMultiFile()
        {
            try
            {
                LoadSomeData loadDtMultiFile = new LoadSomeData(LoadDataGridMultiFile);

                this.Invoke(loadDtMultiFile, "tLoadDataGridMultiFile");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadDtgvMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadDataCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors du chargement du DataGrid : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileNameRadiusStudent);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadDtgvMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadDataCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors du chargement du DataGrid : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileNameRadiusStudent);
            }
        }

        private void LoadDataGridMultiFile(string threadName)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                tempsLoadDataGridMultiFile.Enabled = true;
                tempsLoadDataGridMultiFile.Elapsed += TempsLoadDataGridMultiFile_Elapsed;

                bdsrc_user_multi.DataSource = clsMetier1.GetInstance().getAllClsradcheck_dt_file(txtFile.Text);
                Principal.SetDataSource(bdsrc_user_multi);
                dgvUserMulti.DataSource = bdsrc_user_multi;

                cmdGenerate.Enabled = true;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadDtgvMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadDataCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors du chargement du DataGrid : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileNameRadiusStudent);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadDtgvMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadDataCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors du chargement du DataGrid : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileNameRadiusStudent);
            }
        }

        private void TempsLoadDataGridMultiFile_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (tLoadDataGridMultiFile != null)
            {
                if (!tLoadDataGridMultiFile.IsAlive)
                {
                    tempsLoadDataGridMultiFile.Enabled = false;
                    tLoadDataGridMultiFile.Abort();
                    tLoadDataGridMultiFile = null;

                    ExecuteThreadStopWaitCursor();

                    try
                    {
                        SafeNativeMethods.SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, (UIntPtr)(-1), (UIntPtr)(-1));
                    }
                    catch (DllNotFoundException ex)
                    {
                        ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Réduction mémoire utilisée : " + ex.GetType().ToString() + " : " + ex.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileNameRadiusStudent);
                    }
                    catch (System.ComponentModel.Win32Exception ex)
                    {
                        ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Réduction mémoire utilisée : " + ex.GetType().ToString() + " : " + ex.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileNameRadiusStudent);
                    }
                }
            }
        }
        #endregion

        #region GESTION USERS
        private void BindingCls_User()
        {
            SetBindingControls(txtCodeUser, "Text", radcheck, "id");
            SetBindingControls(txtNomUser, "Text", radcheck, "username");
            SetBindingControls(cboAttributUser, "SelectedValue", radcheck, "attribute");
            SetBindingControls(cboOpUser, "SelectedValue", radcheck, "op");
            SetBindingControls(txtPasswordUser, "Text", radcheck, "value");
            //Propriete en lecture seul
            //SetBindingControls(lblNbrRecordUser, "Text", radcheck, "nbr_enreg");
            SetBindingControls(txtPriorityUser, "Text", radcheck, "priority");
            SetBindingControls(cboGroupeUser, "SelectedValue", radcheck, "groupname");
        }

        private void BindingList_User()
        {
            SetBindingControls(txtCodeUser, "Text", bdsrc_user, "id");
            SetBindingControls(txtNomUser, "Text", bdsrc_user, "username");
            SetBindingControls(cboAttributUser, "SelectedValue", bdsrc_user, "attribute");
            SetBindingControls(cboOpUser, "SelectedValue", bdsrc_user, "op");
            SetBindingControls(txtPasswordUser, "Text", bdsrc_user, "value");
            SetBindingControls(lblNbrRecordUser, "Text", bdsrc_user, "nbr_enreg");
            SetBindingControls(txtPriorityUser, "Text", bdsrc_user, "priority");
            SetBindingControls(cboGroupeUser, "SelectedValue", bdsrc_user, "groupname");
        }
        #endregion

        #region VIEW ACCOUNTING
        private void BindingList_Accounting()
        {
            SetBindingControls(txtCodeAccount, "Text", bdsrc_accounting, "Radacctid");
            SetBindingControls(txtIdSession, "Text", bdsrc_accounting, "Acctsessionid");
            SetBindingControls(txtIdAccount, "Text", bdsrc_accounting, "Acctuniqueid");
            SetBindingControls(txtUsername, "Text", bdsrc_accounting, "Username");
            SetBindingControls(txtGroupeName, "Text", bdsrc_accounting, "Groupname");
            SetBindingControls(txtRealm, "Text", bdsrc_accounting, "Realm");
            SetBindingControls(txtIPNas, "Text", bdsrc_accounting, "Nasipaddress");
            SetBindingControls(txtIdPort, "Text", bdsrc_accounting, "Nasportid");
            SetBindingControls(txtTypeNas, "Text", bdsrc_accounting, "Nasporttype");
            SetBindingControls(txtHeureDebut, "Text", bdsrc_accounting, "Acctstarttime");
            SetBindingControls(txtHeureFin, "Text", bdsrc_accounting, "Acctstoptime");
            SetBindingControls(txtDureeSession, "Text", bdsrc_accounting, "Acctsessiontime");
            SetBindingControls(txtTypeAuthentification, "Text", bdsrc_accounting, "Acctauthentic");
            SetBindingControls(txtInfoStart, "Text", bdsrc_accounting, "Connectinfo_start");
            SetBindingControls(txtInfoStop, "Text", bdsrc_accounting, "Connectinfo_stop");
            SetBindingControls(txtOctetIn, "Text", bdsrc_accounting, "Acctinputoctets");
            SetBindingControls(txtOctetOut, "Text", bdsrc_accounting, "Acctoutputoctets");
            SetBindingControls(txtMacNas, "Text", bdsrc_accounting, "Calledstationid");
            SetBindingControls(txtMacHote, "Text", bdsrc_accounting, "Callingstationid");
            SetBindingControls(txtCauseArret, "Text", bdsrc_accounting, "Acctterminatecause");
            SetBindingControls(txtTypeService, "Text", bdsrc_accounting, "Servicetype");
            SetBindingControls(txtFramedProtocol, "Text", bdsrc_accounting, "Framedprotocol");
            SetBindingControls(txtFramedIP, "Text", bdsrc_accounting, "Framedipaddress");
            SetBindingControls(txtStartDelay, "Text", bdsrc_accounting, "Acctstartdelay");
            SetBindingControls(txtStopDelay, "Text", bdsrc_accounting, "Acctstopdelay");
            SetBindingControls(txtCleSession, "Text", bdsrc_accounting, "Xascendsessionsvrkey");
            SetBindingControls(lblNbrRecordCompt, "Text", bdsrc_accounting, "Nombre_enregistrement");
        }
        #endregion

        #region VIEW POST AUTHENTICATION
        private void BindingList_Postauth()
        {
            SetBindingControls(txtCodePostAuth, "Text", bdsrc_postauth, "Id");
            SetBindingControls(txtUsernamePostAuth, "Text", bdsrc_postauth, "Username");
            SetBindingControls(txtPass, "Text", bdsrc_postauth, "Pass");
            SetBindingControls(txtResponsePacket, "Text", bdsrc_postauth, "Reply");
            SetBindingControls(txtDateAuth, "Text", bdsrc_postauth, "Authdate");
            SetBindingControls(lblNbrRecordPostAuth, "Text", bdsrc_postauth, "Nombre_enregistrement");
        }
        #endregion

        #endregion

        #region THREAD TREATMENTS
        #region GESTION NAS
        private void ExecuteLoadDataGrid()
        {
            try
            {
                LoadSomeData loadDt = new LoadSomeData(LoadDataGrid);

                this.Invoke(loadDt, "tLoadDataGrid");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadDtgvMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadDataCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors du chargement du DataGrid : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileNameRadiusAdmin);
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadDtgvMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadDataCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors du chargement du DataGrid : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileNameRadiusAdmin);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadDtgvMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadDataCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors du chargement du DataGrid : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileNameRadiusAdmin);
            }
        }

        private void LoadDataGrid(string threadName)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                tempsLoadDataGrid.Enabled = true;
                tempsLoadDataGrid.Elapsed += TempsLoadDataGrid_Elapsed;

                //Chargement Data suivant l'onglet choisi
                switch (tblMain.SelectedIndex)
                {
                    case 1://Manage NAS
                        bdsrc_nas.DataSource = clsMetier1.GetInstance().getAllClsnas();
                        Principal.SetDataSource(bdsrc_nas);

                        dgvNAS.DataSource = bdsrc_nas;
                        break;
                    case 2://Gestion Users
                        tbModem.Enabled = false;
                        tbSMS.Enabled = false;
                        cmdConnect.Enabled = true;
                        cmdDisconnect.Enabled = false;

                        Dictionary<string, string> dicoAttribut = new Dictionary<string, string>();
                        Dictionary<string, string> dicoOption = new Dictionary<string, string>();

                        dicoAttribut.Add("Cleartext-Password", "Cleartext-Password");

                        dicoOption.Add(":=", ":=");

                        cboAttributUser.DataSource = new BindingSource(dicoAttribut, null);
                        this.setMembersallcbo(cboAttributUser, "Value", "Key");
                        cboOpUser.DataSource = new BindingSource(dicoOption, null);
                        this.setMembersallcbo(cboOpUser, "Value", "Key");
                        cboGroupeUser.DataSource = clsMetier1.GetInstance().getAllClsradgroupcheck_dt();
                        this.setMembersallcbo(cboGroupeUser, "groupname", "groupname");

                        bdsrc_user.DataSource = clsMetier1.GetInstance().getAllClsradcheck_dt();
                        Principal.SetDataSource(bdsrc_user);
                        dgvUser.DataSource = bdsrc_user;
                        break;
                    case 3://Gestion Accounting
                        bdsrc_accounting.DataSource = clsMetier1.GetInstance().getAllClsradacct();
                        Principal.SetDataSource(bdsrc_accounting);

                        dgvAccounting.DataSource = bdsrc_accounting;
                        break;
                    case 4://Gestion post authentication
                        bdsrc_postauth.DataSource = clsMetier1.GetInstance().getAllClsradpostauth();
                        Principal.SetDataSource(bdsrc_postauth);

                        dgvPostAuth.DataSource = bdsrc_postauth;
                        break;
                    case 5://Generation multiple de mot de passe
                        gpFile.Enabled = false;
                        rdBD.Checked = true;

                        cmdGenerate.Enabled = false;
                        cmdSave.Enabled = false;
                        cmdExport.Enabled = false;

                        cboGroupe.DataSource = clsMetier1.GetInstance().getAllClsradgroupcheck_dt();
                        this.setMembersallcbo(cboGroupe, "groupname", "groupname");
                        break;
                }

                //Here we sotp waitCursor if there are not records in BindinSource
                if (bdsrc_nas.Count == 0 || bdsrc_nas.Count == 0 || bdsrc_user.Count == 0
                    || bdsrc_accounting.Count == 0 || bdsrc_postauth.Count == 0)
                {
                    ExecuteThreadStopWaitCursor();
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadDtgvMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadDataCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors du chargement du DataGrid : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileNameRadiusAdmin);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadDtgvMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadDataCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors du chargement du DataGrid : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileNameRadiusAdmin);
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

        private void TempsLoadDataGrid_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
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
                        ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Réduction mémoire utilisée : " + ex.GetType().ToString() + " : " + ex.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileNameRadiusAdmin);
                    }
                    catch (System.ComponentModel.Win32Exception ex)
                    {
                        ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Réduction mémoire utilisée : " + ex.GetType().ToString() + " : " + ex.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileNameRadiusAdmin);
                    }
                }
            }
        }

        #endregion

        #region METHODS CALL
        private void RefreshData()
        {
            //Common Part
            try
            {
                if (tLoadDataGrid == null)
                {
                    tLoadDataGrid = new Thread(new ThreadStart(ExecuteLoadDataGrid));
                    tLoadDataGrid.Start();
                }
                else
                {
                    tLoadDataGrid.Abort();
                    tLoadDataGrid = null;
                }
            }
            catch { }

            switch (tblMain.SelectedIndex)
            {
                case 1:
                    if (bdsrc_nas.Count == 0)
                    {
                        Principal.ActivateOnLoadCommandButtons(false);
                    }
                    break;
                case 2:
                    if (bdsrc_user.Count == 0)
                    {
                        Principal.ActivateOnLoadCommandButtons(false);

                        txtDestinataire.Clear();
                        txtMsgSend.Clear();
                        lstPersonneTel.Items.Clear();
                    }
                    break;
                case 3:
                    if (bdsrc_accounting.Count == 0)
                    {
                        Principal.ActivateOnLoadCommandButtons(false);
                    }
                    break;
                case 4:
                    if (bdsrc_postauth.Count == 0)
                    {
                        Principal.ActivateOnLoadCommandButtons(false);
                    }
                    break;
            }
        }

        private void TempsStopWaitCursor_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            List<Thread> lstThread = new List<Thread>();

            if (tStopWaitCursor != null)
            {
                if (!tStopWaitCursor.IsAlive)
                {
                    if (tLoadDataGrid != null)
                        lstThread.Add(tLoadDataGrid);

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

        #endregion

        private void ActivateTabs(bool status)
        {
            gpNas.Enabled = status;
            gpUser.Enabled = status;
            gpAccounting.Enabled = status;
            gpPostAuthentication.Enabled = status;
            gpGeneratePassword.Enabled = status;
        }

        private void frmDataViewAdministration_Load(object sender, EventArgs e)
        {
            this.ActivateTabs(false);
            txtPwdBd.Focus();

            //Initialise timers
            tempsLoadDataGrid = new System.Timers.Timer();
            tempsLoadDataGrid.Interval = 100;

            tempsLoadDataGridMulti = new System.Timers.Timer();
            tempsLoadDataGridMulti.Interval = 100;

            tempsLoadDataGridMultiFile = new System.Timers.Timer();
            tempsLoadDataGridMultiFile.Interval = 100;

            tempsGenerateKey = new System.Timers.Timer();
            tempsGenerateKey.Interval = 100;

            tempsSaveKey = new System.Timers.Timer();
            tempsSaveKey.Interval = 100;

            tempsExportKey = new System.Timers.Timer();
            tempsExportKey.Interval = 100;

            tempsStopWaitCursor = new System.Timers.Timer();
            tempsStopWaitCursor.Interval = 100;
        }

        private void cmdValidate_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtPwdBd.Text))
                {
                    txtPwdBd.Focus();
                    throw new Exception("Veuillez spécifier un mot de passe valide svp !!!");
                }
                else if (string.IsNullOrEmpty(txtChipherKey.Text))
                {
                    txtChipherKey.Focus();
                    throw new Exception("Veuillez spécifier une clé de chiffrement valide svp !!!");
                }
                else
                {
                    MySql.Data.MySqlClient.MySqlConnection con = new MySql.Data.MySqlClient.MySqlConnection();
                    List<string> paramServeur = new List<string>();

                    paramServeur = ImplementUtilities.Instance.LoadDatabaseParameters(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.DirectoryUtilConn, Properties.Settings.Default.FileRadAdmin, '\n', txtChipherKey.Text, true);

                    if (paramServeur.Count > 0)
                    {
                        connection.Serveur = paramServeur[0];
                        connection.DB = paramServeur[1];
                        connection.User = paramServeur[2];
                        connection.Pwd = txtPwdBd.Text;
                    }

                    //Ouverture de la connexion a la BD avec les parametres fournis
                    clsMetier1.GetInstance().Initialize(connection);

                    if (clsMetier1.GetInstance().isConnect())
                    {
                        this.ActivateTabs(true);
                        MessageBox.Show("Connexion réussie", "Connexion à la base de données", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                        txtPwdBd.Clear();
                        txtChipherKey.Clear();
                        this.ActivateTabs(true);
                        Principal.ActivateMainBindingSource(true);
                        tblMain.SelectedIndex = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                txtPwdBd.Focus();
                MessageBox.Show(string.Format("Echec de l'authentification de l'utilisateur, {0}", ex.Message), "Connexion à la base de données", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void txtChipherKey_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cmdValidate_Click(sender, e);
            }
        }

        private void frmDataViewAdministration_Activated(object sender, EventArgs e)
        {
            Principal.SetValuesLabel(Properties.Settings.Default.UserConnected, "Gestion des utilisateurs Radius de l'Administration");
            Principal.SetCurrentICRUDChildForm(this);

            //RefreshData();
        }

        private void frmDataViewAdministration_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                clsMetier1.GetInstance().CloseConnection();
            }
            catch { }

            try
            {
                clsMetier1.GetInstance().CloseModemConnection();
            }
            catch { }

            //Reinitialise all Threads
            try
            {
                this.UnloadThreadRessource(tLoadDataGrid);
                this.UnloadThreadRessource(tLoadDataGridMulti);
                this.UnloadThreadRessource(tLoadDataGridMultiFile);
                this.UnloadThreadRessource(tGenerateKey);
                this.UnloadThreadRessource(tSaveKey);
                this.UnloadThreadRessource(tExportKey);
            }
            catch { }

            Principal.SetValuesLabel(Properties.Settings.Default.UserConnected, "Attente d'une action de l'utilisateur");
            Principal.ApplyDefaultStatusBar(Principal, Properties.Settings.Default.UserConnected);
            //Affecte Activate BindingNavigator
            Principal.ActivateMainBindingSource(Principal);
        }

        public void New()
        {
            try
            {
                Principal.ActivateOnNewCommandButtons(true);

                switch (tblMain.SelectedIndex)
                {
                    case 1:
                        //Initialise object class
                        nas = new clsnas();

                        blnModifie_nas = false;

                        BindingCls_Nas();

                        //Set the new ID
                        if (newID_nas == null)
                            newID_nas = clsMetier1.GetInstance().GenerateLastID("nas");
                        txtCodeNas.Text = newID_nas.ToString();
                        break;
                    case 2:
                        //Initialise object class
                        radcheck = new clsradcheck();

                        blnModifie_user = false;

                        BindingCls_User();

                        //Set the new ID
                        if (newID_user == null)
                            newID_user = clsMetier1.GetInstance().GenerateLastID("radcheck");
                        txtCodeUser.Text = newID_user.ToString();
                        break;
                    case 3:
                        MessageBox.Show("Ajout pas nécessaire", "Nouvel enregistrement", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        break;
                    case 4:
                        MessageBox.Show("Ajout pas nécessaire", "Nouvel enregistrement", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        break;
                    case 5:
                        MessageBox.Show("Ajout pas nécessaire", "Nouvel enregistrement", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        break;
                }
            }
            catch (ArgumentException ex)
            {
                Principal.ActivateOnNewCommandButtons(false);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors de la création d'un nouvel enregistrement : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileNameRadiusAdmin);
            }
            catch (NullReferenceException ex)
            {
                Principal.ActivateOnNewCommandButtons(false);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors de la création d'un nouvel enregistrement : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileNameRadiusAdmin); 
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                Principal.ActivateOnNewCommandButtons(false);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors de la création d'un nouvel enregistrement : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileNameRadiusAdmin);
            }
        }

        public void Search(string criteria)
        {
            try
            {
                switch (tblMain.SelectedIndex)
                {
                    case 1:
                        if (dgvNAS.RowCount == 0)
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
                                List<clsnas> lstItemSearch = new List<clsnas>();
                                lstItemSearch = clsMetier1.GetInstance().getAllClsnas(criteria);

                                dgvNAS.DataSource = lstItemSearch;
                            }
                        }
                        break;
                    case 2:
                        if (dgvUser.RowCount == 0)
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
                                //List<clsradcheck> lstItemSearch = new List<clsradcheck>();
                                //lstItemSearch = clsMetier1.GetInstance().getAllClsradcheck(criteria);
                                DataTable lstItemSearch = new DataTable();
                                lstItemSearch = clsMetier1.GetInstance().getAllClsradcheck_dt(criteria);

                                dgvUser.DataSource = lstItemSearch;
                            }
                        }
                        break;
                    case 3:
                        if (dgvAccounting.RowCount == 0)
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
                                List<clsradacct> lstItemSearch = new List<clsradacct>();
                                lstItemSearch = clsMetier1.GetInstance().getAllClsradacct(criteria);

                                dgvAccounting.DataSource = lstItemSearch;
                            }
                        }
                        break;
                    case 4:
                        if (dgvPostAuth.RowCount == 0)
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
                                List<clsradpostauth> lstItemSearch = new List<clsradpostauth>();
                                lstItemSearch = clsMetier1.GetInstance().getAllClsradpostauth(criteria);

                                dgvPostAuth.DataSource = lstItemSearch;
                            }
                        }
                        break;
                    case 5:
                        if (dgvUserMulti.RowCount == 0)
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
                                DataTable lstItemSearch = new DataTable();
                                lstItemSearch = clsMetier1.GetInstance().getAllClsradcheck_dt(criteria);
                                dgvUserMulti.DataSource = lstItemSearch;
                            }
                        }
                        break;
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedSearchMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedSearchCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de la recherche : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileNameRadiusAdmin);
            }
        }

        public void Save()
        {
            try
            {
                int numTab = tblMain.SelectedIndex;
                switch (numTab)
                {
                    case 1:
                        if (!blnModifie_nas)
                        {
                            int record = nas.inserts();
                            if (record == 0)
                                throw new CustomException(stringManager.GetString("StringZeroRecordAffectedMessage", CultureInfo.CurrentUICulture));
                            else
                                MessageBox.Show(stringManager.GetString("StringSuccessSaveMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringSuccessSaveCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        }
                        else
                        {
                            UpdateRec(ref numTab);
                        }
                        newID_nas = null;
                        break;
                    case 2:
                        if (!blnModifie_user)
                        {
                            int record = radcheck.inserts();
                            if (record == 0)
                                throw new CustomException(stringManager.GetString("StringZeroRecordAffectedMessage", CultureInfo.CurrentUICulture));
                            else
                                MessageBox.Show(stringManager.GetString("StringSuccessSaveMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringSuccessSaveCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        }
                        else
                        {
                            UpdateRec(ref numTab);
                        }
                        newID_user = null;
                        break;
                    case 3:
                        MessageBox.Show("Aucune modification n;est requise ici", "Enregistrement-Modification", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        break;
                    case 4:
                        MessageBox.Show("Aucune modification n;est requise ici", "Enregistrement-Modification", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        break;
                    case 5:
                        MessageBox.Show("Utiliser le bouton enregistrer de cette interface (Après génération des nouveaux mots de passe)", "Enregistrement-Modification", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        return;
                }

                RefreshData();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedSaveUpdateMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedSaveUpdateCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de la mise à jour : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileNameRadiusAdmin);
            }
            catch (CustomException ex)
            {
                Properties.Settings.Default.StringLogFile = ex.Message;
                MessageBox.Show(Properties.Settings.Default.StringLogFile, stringManager.GetString("StringFailedSaveUpdateCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de la mise à jour  : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileNameRadiusAdmin);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedSaveUpdateMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedSaveUpdateCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de la mise à jour : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileNameRadiusAdmin);
            }
        }

        public void UpdateRec(ref int numTab)
        {
            switch (numTab)
            {
                case 1:
                    int record1 = nas.update(((clsnas)bdsrc_nas.Current));
                    MessageBox.Show("Modification éffectuée : " + record1 + " Modifié", "Modification", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    break;
                case 2:
                    int record2 = radcheck.update(((DataRowView)bdsrc_user.Current));
                    MessageBox.Show("Modification éffectuée : " + record2 + " Modifié", "Modification", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    break;
                case 3:
                    MessageBox.Show("Aucune modification n;est requise ici", "Enregistrement-Modification", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    break;
                case 4:
                    MessageBox.Show("Aucune modification n;est requise ici", "Enregistrement-Modification", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    break;
            }
        }

        public void Delete()
        {
            try
            {
                DialogResult dr = MessageBox.Show(stringManager.GetString("StringPromptDeleteMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringPromptDeleteCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                switch (tblMain.SelectedIndex)
                {
                    case 1:
                        if (blnModifie_nas)
                        {
                            int record = 0;

                            if (dr == DialogResult.Yes)
                            {
                                record = nas.delete(((clsnas)bdsrc_nas.Current));
                                if (record == 0)
                                    throw new CustomException(stringManager.GetString("StringZeroRecordAffectedMessage", CultureInfo.CurrentUICulture));
                                else
                                    MessageBox.Show(stringManager.GetString("StringSuccessDeleteMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringSuccessDeleteCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly); newID_nas = null;
                            }
                            else
                                MessageBox.Show(stringManager.GetString("StringCancelDeleteMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringCancelDeleteCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        }
                        break;
                    case 2:
                        if (blnModifie_user)
                        {
                            int record = 0;

                            if (dr == DialogResult.Yes)
                            {
                                record = radcheck.delete(((DataRowView)bdsrc_user.Current));
                                if (record == 0)
                                    throw new CustomException(stringManager.GetString("StringZeroRecordAffectedMessage", CultureInfo.CurrentUICulture));
                                else
                                    MessageBox.Show(stringManager.GetString("StringSuccessDeleteMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringSuccessDeleteCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly); newID_user = null;
                            }
                            else
                                MessageBox.Show(stringManager.GetString("StringCancelDeleteMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringCancelDeleteCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        }
                        break;
                    case 3:
                        if (blnModifie_accounting)
                        {
                            int record = 0;

                            if (dr == DialogResult.Yes)
                            {
                                if (chkDeleteAllAccounting.Checked)
                                    record = accounting.delete_all();
                                else
                                    record = accounting.delete(((clsradacct)bdsrc_accounting.Current));
                                if (record == 0)
                                    throw new CustomException(stringManager.GetString("StringZeroRecordAffectedMessage", CultureInfo.CurrentUICulture));
                                else
                                    MessageBox.Show(stringManager.GetString("StringSuccessDeleteMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringSuccessDeleteCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            }
                            else
                                MessageBox.Show(stringManager.GetString("StringCancelDeleteMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringCancelDeleteCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        }
                        chkDeleteAllAccounting.Checked = false;
                        break;
                    case 4:
                        if (blnModifie_postauth)
                        {
                            int record = 0;

                            if (dr == DialogResult.Yes)
                            {
                                if (chkDeleteAllPostAuth.Checked)
                                    record = postauth.delete_all();
                                else
                                    record = postauth.delete(((clsradpostauth)bdsrc_postauth.Current));
                                if (record == 0)
                                    throw new CustomException(stringManager.GetString("StringZeroRecordAffectedMessage", CultureInfo.CurrentUICulture));
                                else
                                    MessageBox.Show(stringManager.GetString("StringSuccessDeleteMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringSuccessDeleteCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            }
                            else
                                MessageBox.Show(stringManager.GetString("StringCancelDeleteMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringCancelDeleteCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        }
                        chkDeleteAllPostAuth.Checked = false;
                        break;
                    case 5:
                        MessageBox.Show("Aucune action requise", "Suppression enregistrement", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        return;
                }

                RefreshData();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedDeleteMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedDeleteCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de la suppression : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileNameRadiusAdmin);
            }
            catch (CustomException ex)
            {
                Properties.Settings.Default.StringLogFile = ex.Message;
                MessageBox.Show(Properties.Settings.Default.StringLogFile, stringManager.GetString("StringFailedDeleteCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de la suppression  : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileNameRadiusAdmin);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedDeleteMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedDeleteCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de la suppression : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileNameRadiusAdmin);
            }
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
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileNameRadiusAdmin);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedRefreshMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedRefreshCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors de l'actualisation : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileNameRadiusAdmin);
            }
        }

        public void Preview()
        {
            MessageBox.Show("The Reports has not been set, please ask the Administrator", "Reports", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
        }

        private void tblMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tblMain.SelectedIndex != 0)
            {
                RefreshData();
            }
        }

        private void dgvNAS_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                BindingList_Nas();
                blnModifie_nas = true;
                Principal.ActivateOnNewSelectionChangeDgvCommandButtons(true);
            }
            catch (ArgumentException ex)
            {
                blnModifie_nas = false;
                Principal.ActivateOnSelectionChangeDgvExceptionCommandButtons(false);

                MessageBox.Show(stringManager.GetString("StringFailedSelectRecordDtgvMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedSelectRecordDtgvCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur de sélection dans le DataGrid : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileNameRadiusAdmin);
            }
        }

        private void dgvUser_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                BindingList_User();
                blnModifie_user = true;
                Principal.ActivateOnNewSelectionChangeDgvCommandButtons(true);

                if (chkSMS.Checked)
                {
                    txtMsgSend.Text = string.Format("Votre login pour accès au réseau de l'ISIG \nUsername:{0} \net mot de passe:{1}", txtNomUser.Text, txtPasswordUser.Text);
                }
            }
            catch (ArgumentException ex)
            {
                blnModifie_user = false;
                Principal.ActivateOnSelectionChangeDgvExceptionCommandButtons(false);

                MessageBox.Show(stringManager.GetString("StringFailedSelectRecordDtgvMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedSelectRecordDtgvCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur de sélection dans le DataGrid : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileNameRadiusAdmin);
            }
        }

        private void dgvAccounting_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                BindingList_Accounting();
                blnModifie_accounting = true;
                Principal.ActivateOnNewSelectionChangeDgvCommandButtons1(true);
            }
            catch (ArgumentException ex)
            {
                blnModifie_accounting = false;
                Principal.ActivateOnSelectionChangeDgvExceptionCommandButtons(false);

                MessageBox.Show(stringManager.GetString("StringFailedSelectRecordDtgvMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedSelectRecordDtgvCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur de sélection dans le DataGrid : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileNameRadiusAdmin);
            }
        }

        private void dgvPostAuth_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                BindingList_Postauth();
                blnModifie_postauth = true;
                Principal.ActivateOnNewSelectionChangeDgvCommandButtons1(true);
            }
            catch (ArgumentException ex)
            {
                blnModifie_postauth = false;
                Principal.ActivateOnSelectionChangeDgvExceptionCommandButtons(false);

                MessageBox.Show(stringManager.GetString("StringFailedSelectRecordDtgvMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedSelectRecordDtgvCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur de sélection dans le DataGrid : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileNameRadiusAdmin);
            }
        }

        public void UpdateRec()
        {
            throw new NotImplementedException();
        }

        private void lblAddGroupUser_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmGroupUserAdmin frm = new frmGroupUserAdmin();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void cboGroupeUser_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void DoActualiseDropDown()
        {
            //Actualisation des combobox si modification
            try
            {
                if (!string.IsNullOrEmpty(Properties.Settings.Default.strFormModifieSubForm))
                {
                    if (Properties.Settings.Default.strFormModifieSubForm.Equals(FormActualisation.frmGroupUserAdmin.ToString()))
                    {
                        cboGroupeUser.DataSource = clsMetier1.GetInstance().getAllClsradgroupcheck_dt();
                        this.setMembersallcbo(cboGroupeUser, "groupname", "groupname");
                    }
                }

                Properties.Settings.Default.strFormModifieSubForm = "";
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedRefreshLoadComboMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedRefreshLoadComboCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec d'actualisation de la liste déroulante : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileNameRadiusAdmin);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedRefreshLoadComboMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedRefreshLoadComboCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec d'actualisation de la liste déroulante : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileNameRadiusAdmin);
            }
        }

        private void chkSMS_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSMS.Checked)
            {
                tbModem.Enabled = true;
                tbSMS.Enabled = true;

                cboPort.DataSource = clsMetier1.GetInstance().GetAllports();
                cboBaud.DataSource = clsMetier1.GetInstance().LoadBaudPorts();
                cboTimeOut.DataSource = clsMetier1.GetInstance().LoadTimeOut();

                cboBaud.SelectedIndex = 5;
                cboTimeOut.SelectedIndex = 1;

                txtDestinataire.Clear();
                txtMsgSend.Clear();
                lstPersonneTel.Items.Clear();

                //Load Phone number and personnes
                try
                {
                    lstPersonneTel.Items.Clear();
                    List<string> lst = new List<string>();
                    lst = smartManage.Model.clsMetier.GetInstance().getTelephonePersonne();
                    foreach (string str in lst)
                        lstPersonneTel.Items.Add(str);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Echec de chargement des destinataires, {0}", ex.Message), "Chargement destinataires", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                }
            }
            else
            {
                tbModem.Enabled = false;
                tbSMS.Enabled = false;
            }
        }

        private void cmdDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                clsMetier1.GetInstance().CloseModemConnection();

                cmdDisconnect.Enabled = false;
                cmdConnect.Enabled = true;
                MessageBox.Show("Port du Modem déconnecté", "Déconnexion", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Echec de la déconnexion, {0}", ex.Message), "Déconnexion Modem", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void cmdConnect_Click(object sender, EventArgs e)
        {
            try
            {
                clsMetier1.GetInstance().OpenConnection(clsMetier1.GetInstance().RecupNumeroPort(cboPort.Text), Convert.ToInt32(cboBaud.SelectedValue.ToString()), Convert.ToInt32(cboTimeOut.SelectedValue.ToString()));

                cmdConnect.Enabled = false;
                cmdDisconnect.Enabled = true;

                MessageBox.Show("Connexion réussie", "Connexion Modem", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Echec de la connexion, {0}", ex.Message), "Connexion Modem", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void cmdSendMsg_Click(object sender, EventArgs e)
        {
            List<string> liste = new List<string>();

            try
            {
                if (txtDestinataire.Text.Split(';').Length > 1)
                    //Envoie multiple
                    clsMetier1.GetInstance().SendManySMS(txtMsgSend.Text, txtDestinataire.Text);
                else
                    clsMetier1.GetInstance().SendOneSMS(txtMsgSend.Text, txtDestinataire.Text);

                MessageBox.Show("Message envoyé", "Envoie SMS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
            catch (Exception)
            {
                MessageBox.Show("Echec de l'envoie du SMS", "Envoie SMS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void lstPersonneTel_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(txtDestinataire.Text);
                if (string.IsNullOrEmpty(txtDestinataire.Text))
                {
                    sb.Append(lstPersonneTel.SelectedItem.ToString().Split('>')[1]);
                }
                else
                {
                    sb.Append(";");
                    sb.Append(lstPersonneTel.SelectedItem.ToString().Split('>')[1]);
                }

                txtDestinataire.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Echec de sélection, {0}", ex.Message), "Sélection destinataire", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void lblGeneratePassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                txtPasswordUser.Text = clsMetier1.GetInstance().GeneratePassword(txtNomUser.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Echec de génération du mot de passe, {0}", ex.Message), "Génération mot de passe", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void rdBD_CheckedChanged(object sender, EventArgs e)
        {
            if (rdBD.Checked)
                gpFile.Enabled = false;
        }

        private void rdFile_CheckedChanged(object sender, EventArgs e)
        {
            if (rdFile.Checked)
                gpFile.Enabled = true;
            else
                gpFile.Enabled = false;
        }

        private void cmdLoadFile_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dlgOpen = new OpenFileDialog();
                dlgOpen.Title = "Choisir un fichier";
                dlgOpen.Filter = "Fichier texte (*.txt)|*.txt";

                if (dlgOpen.ShowDialog() == DialogResult.OK)
                {
                    txtFile.Text = dlgOpen.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Echec de l'ouverture du fichier, " + ex.Message, "Ouvrir un fichier", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void cmdView_Click(object sender, EventArgs e)
        {
            try
            {
                if (rdBD.Checked)
                {
                    AfficheDataMulti();
                }
                else if (rdFile.Checked)
                {
                    AfficheDataMultiFile();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Echec de l'afichage des données, {0}", ex.Message), "Affichage données", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void lblAddGroup_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmGroupUserAdmin frm = new frmGroupUserAdmin();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void cmdGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtChipherKeyMultiple.Text))
                    useCipherKey = true;

                CallGenerateKey();
                cmdSave.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Echec de la génération des mots de passe, " + ex.Message, "Génération mot de passe", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {
                CallSaveKey();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Echec de l'enregistrement des données générées, " + ex.Message, "Génération des données générées", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void cmdExport_Click(object sender, EventArgs e)
        {
            try
            {
                CallExportKey();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Echec de l'enregistrement des données générées, " + ex.Message, "Génération des données générées", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }
    }
}
