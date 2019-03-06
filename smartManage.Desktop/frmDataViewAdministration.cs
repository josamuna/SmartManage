using ManageUtilities;
using smartManage.RadiusAdminModel;
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
        BindingSource bdsrc_accounting = new BindingSource();
        BindingSource bdsrc_postauth = new BindingSource();

        bool blnModifie_nas = false;
        bool blnModifie_user = false;
        bool blnModifie_accounting = false;
        bool blnModifie_postauth = false;

        int? newID_nas = null;
        int? newID_user = null;

        //Delegate utilisation des threads
        private delegate void LoadSomeData(string threadName);

        //Timer pour chagement Datagrid et actualisation (Refresh)
        System.Timers.Timer tempsLoadDataGrid = null;

        //Timer for automatically set default cursor to form
        System.Timers.Timer tempsStopWaitCursor = null;

        //Thread pour chargement DataGrid
        Thread tLoadDataGrid = null;
        Thread tStopWaitCursor = null;

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
            gpReply.Enabled = status;
        }

        private void frmDataViewAdministration_Load(object sender, EventArgs e)
        {
            this.ActivateTabs(false);
            txtPwdBd.Focus();

            //Initialise timers
            tempsLoadDataGrid = new System.Timers.Timer();
            tempsLoadDataGrid.Interval = 100;

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
    }
}
