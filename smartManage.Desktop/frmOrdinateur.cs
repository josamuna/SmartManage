using ManageQRCode;
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
    public partial class frmOrdinateur : Form, ICRUDGeneral, ICallMainForm
    {
        BindingSource bdsrc = new BindingSource();
        bool blnModifie = false;
        private clsmateriel materiel = new clsmateriel();
        int? newID = null;

        //Delegate utilisation des threads
        private delegate void LoadSomeData(string threadName);

        //Timer for automatically unload thread for update comboBox on DropDown event
        System.Timers.Timer tempsActualiseCombo = null;

        //Timer for automatically unload thread for generate QrCode
        System.Timers.Timer tempsGenerateQrCode = null;

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
        Thread tLeftCombo = null, tMiddleCombo = null, tRightCombo = null;
        Thread tActualiseComb = null;
        Thread tGenerateQrCode = null;
        Thread tStopWaitCursor = null;

        //ariable that contain Byte image for QrCode
        byte[] tmpQrCode = null;

        //Boolean variables for photo
        bool blnPhoto1 = false;
        bool blnPhoto2 = false;
        bool blnPhoto3 = false;

        bool firstLoad = false;

        ResourceManager stringManager = null;

        public frmPrincipal Principal
        {
            get;
            set;
        }

        public frmOrdinateur()
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

                cboCatMateriel.DataSource = clsMetier.GetInstance().getAllClscategorie_materiel();
                this.setMembersallcbo(cboCatMateriel, "Designation", "Id");
                cboNumCompte.DataSource = clsMetier.GetInstance().getAllClscompte();
                this.setMembersallcbo(cboNumCompte, "Numero", "Id");
                cboGarantie.DataSource = clsMetier.GetInstance().getAllClsgarantie();
                this.setMembersallcbo(cboGarantie, "Valeur", "Id");
                cboMarque.DataSource = clsMetier.GetInstance().getAllClsmarque();
                this.setMembersallcbo(cboMarque, "Designation", "Id");
                cboModele.DataSource = clsMetier.GetInstance().getAllClsmodele();
                this.setMembersallcbo(cboModele, "Designation", "Id");
                cboCouleur.DataSource = clsMetier.GetInstance().getAllClscouleur();
                this.setMembersallcbo(cboCouleur, "Designation", "Id");
                cboPoids.DataSource = clsMetier.GetInstance().getAllClspoids();
                this.setMembersallcbo(cboPoids, "Valeur", "Id");
                cboEtat.DataSource = clsMetier.GetInstance().getAllClsetat_materiel();
                this.setMembersallcbo(cboEtat, "Designation", "Id");

                List<ComboBox> lstCombo = new List<ComboBox>() { cboCatMateriel, cboNumCompte, cboGarantie, cboMarque, cboModele, cboCouleur, cboPoids, cboEtat };

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

        private void LoadMiddleCombo(string threadName)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                cboTypeOrdi.DataSource = clsMetier.GetInstance().getAllClstype_ordinateur();
                this.setMembersallcbo(cboTypeOrdi, "Designation", "Id");
                cboTypeClavier.DataSource = clsMetier.GetInstance().getAllClstype_clavier();
                this.setMembersallcbo(cboTypeClavier, "Designation", "Id");
                cboOS.DataSource = clsMetier.GetInstance().getAllClsOS();
                this.setMembersallcbo(cboOS, "Designation", "Id");
                cboRAM.DataSource = clsMetier.GetInstance().getAllClsram();
                this.setMembersallcbo(cboRAM, "Valeur", "Id");
                cboProcesseur.DataSource = clsMetier.GetInstance().getAllClsprocesseur();
                this.setMembersallcbo(cboProcesseur, "Valeur", "Id");
                cboNbrCoeurProcesseur.DataSource = clsMetier.GetInstance().getAllClsnombre_coeur_processeur();
                this.setMembersallcbo(cboNbrCoeurProcesseur, "Valeur", "Id");
                cboTypeHDD.DataSource = clsMetier.GetInstance().getAllClstype_hdd();
                this.setMembersallcbo(cboTypeHDD, "Designation", "Id");
                cboCapaciteHDD.DataSource = clsMetier.GetInstance().getAllClscapacite_hdd();
                this.setMembersallcbo(cboCapaciteHDD, "Valeur", "Id");
                cboNbrHDD.DataSource = clsMetier.GetInstance().getAllClsnombre_hdd();
                this.setMembersallcbo(cboNbrHDD, "Valeur", "Id");
                cboTailleEcran.DataSource = clsMetier.GetInstance().getAllClstaille_ecran();
                this.setMembersallcbo(cboTailleEcran, "Valeur", "Id");
                cboUSB2.DataSource = clsMetier.GetInstance().getAllClsusb2();
                this.setMembersallcbo(cboUSB2, "Valeur", "Id");
                cboUSB3.DataSource = clsMetier.GetInstance().getAllClsusb3();
                this.setMembersallcbo(cboUSB3, "Valeur", "Id");

                List<ComboBox> lstCombo = new List<ComboBox>() { cboTypeOrdi, cboTypeClavier, cboOS, cboRAM, cboProcesseur, cboNbrCoeurProcesseur,
                cboTypeHDD, cboCapaciteHDD, cboNbrHDD, cboTailleEcran, cboUSB2, cboUSB3 };

                SetSelectedIndexComboBox(lstCombo);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadComboBoxMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadComboBoxCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur de chargement des listes déroulantes du milieu : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadComboBoxMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadComboBoxCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur de chargement des listes déroulantes du milieu : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
        }

        private void ExecuteMiddleCombo()
        {
            try
            {
                LoadSomeData middleCbo = new LoadSomeData(LoadMiddleCombo);

                this.Invoke(middleCbo, "tMiddleCombo");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadComboBoxMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadComboBoxCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur de chargement des listes déroulantes du milieu dans le thread : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadComboBoxMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadComboBoxCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur de chargement des listes déroulantes du milieu dans le thread : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
        }

        private void LoadRightCombo(string threadName)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                cboNbrHDMI.DataSource = clsMetier.GetInstance().getAllClshdmi();
                this.setMembersallcbo(cboNbrHDMI, "Valeur", "Id");
                cboNbrVGA.DataSource = clsMetier.GetInstance().getAllClsvga();
                this.setMembersallcbo(cboNbrVGA, "Valeur", "Id");
                cboTensionBatt.DataSource = clsMetier.GetInstance().getAllClstension_batterie();
                this.setMembersallcbo(cboTensionBatt, "Valeur", "Id");
                cboTensionAdap.DataSource = clsMetier.GetInstance().getAllClstension_adaptateur();
                this.setMembersallcbo(cboTensionAdap, "Valeur", "Id");
                cboPuissanceAdap.DataSource = clsMetier.GetInstance().getAllClspuissance_adaptateur();
                this.setMembersallcbo(cboPuissanceAdap, "Valeur", "Id");
                cboIntensiteAdap.DataSource = clsMetier.GetInstance().getAllClsintensite_adaptateur();
                this.setMembersallcbo(cboIntensiteAdap, "Valeur", "Id");

                List<ComboBox> lstCombo = new List<ComboBox>() { cboNbrHDMI, cboNbrVGA, cboTensionBatt, cboTensionAdap, cboPuissanceAdap, cboIntensiteAdap };

                SetSelectedIndexComboBox(lstCombo);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadComboBoxMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadComboBoxCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur de chargement des listes déroulantes de droite : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadComboBoxMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadComboBoxCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur de chargement des listes déroulantes de droite : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
        }

        private void ExecuteRightCombo()
        {
            try
            {
                LoadSomeData rightCbo = new LoadSomeData(LoadRightCombo);

                this.Invoke(rightCbo, "tRightCombo");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadComboBoxMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadComboBoxCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur de chargement des listes déroulantes de droite dans le thread : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadComboBoxMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadComboBoxCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur de chargement des listes déroulantes de droite dans le thread : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
        }

        private void LoadDataGrid(string threadName)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                bdsrc.DataSource = clsMetier.GetInstance().getAllClsmateriel_Ordinateur();
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
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadDtgvMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadDataCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors du chargement du DataGrid : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
        }

        private void GenerateQrCode(string threadName)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(string.Format("{0}-{1}", txtId.Text, ((clscategorie_materiel)cboCatMateriel.SelectedItem).Id.ToString()));
                txtIdentidiant.Text = sb.ToString();

                //Generate label Equipement
                txtLabel.Text = clsMetier.GetInstance().GenerateLabelMateriel(Convert.ToInt32(txtId.Text), "ILPC");

                //Creating QrCode 
                System.Drawing.Image img = QRCodeImage.GetGenerateQRCode(string.Format("{0}\n{1}", txtIdentidiant.Text, txtLabel.Text), "L", "", 0);//L, M ou Q
                pbQRCode.Image = img;

                //Convert PictureBox image to Byte[]
                //Save a temp image file
                string fileName = clsTools.Instance.SaveTempImage(pbQRCode);
                tmpQrCode = clsTools.Instance.GetByteFromFile(fileName);
                //txtQRCode.Text = Convert.ToString(tmpQrCode);
                //Remove the temp image created
                clsTools.Instance.RemoveTempImage(fileName);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedGenerateQrCodeMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedGenerateQrCodeCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors de la génération du QrCode : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedGenerateQrCodeMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedGenerateQrCodeCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors de la génération du QrCode : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
        }

        private void ExecuteGenerateQrCode()
        {
            try
            {
                LoadSomeData codeQr = new LoadSomeData(GenerateQrCode);

                this.Invoke(codeQr, "tQrCode");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedGenerateQrCodeMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedGenerateQrCodeCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors de la génération du QrCode : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedGenerateQrCodeMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedGenerateQrCodeCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors de la génération du QrCode : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
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

                if (bdsrc.Count > 0)
                {
                    if ((bool)((clsmateriel)bdsrc.Current).Archiver)
                        cmdArchiver.Enabled = false;
                    else
                        cmdArchiver.Enabled = true;
                }
                else
                    cmdArchiver.Enabled = false;

                //Affectation de la duree restante par rapport a la garantie de l'equipement
                int? duree = null;

                if (cboGarantie.SelectedValue != null)
                    duree = int.Parse(cboGarantie.SelectedValue.ToString());

                lblStatusGuaraty.Text = clsMetier.GetInstance().CalculateEndGuarany(duree, DateTime.Parse(txtDateAcquisition.Text));

                //Affiche QrCode from Byte[]
                pbQRCode.Image = null;
                pbQRCode.Image = clsTools.Instance.GetImageFromByte(((clsmateriel)bdsrc.Current).Qrcode);
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
                    if (Properties.Settings.Default.strFormModifieSubForm.Equals(FormActualisation.frmCategorieMateriel.ToString()))
                    {
                        cboCatMateriel.DataSource = clsMetier.GetInstance().getAllClscategorie_materiel();
                        this.setMembersallcbo(cboCatMateriel, "Designation", "Id");
                    }
                    else if (Properties.Settings.Default.strFormModifieSubForm.Equals(FormActualisation.frmNumeroCompte.ToString()))
                    {
                        cboNumCompte.DataSource = clsMetier.GetInstance().getAllClscompte();
                        this.setMembersallcbo(cboNumCompte, "Numero", "Id");
                    }
                    else if (Properties.Settings.Default.strFormModifieSubForm.Equals(FormActualisation.frmMarque.ToString()))
                    {
                        cboMarque.DataSource = clsMetier.GetInstance().getAllClsmarque();
                        this.setMembersallcbo(cboMarque, "Designation", "Id");
                    }
                    else if (Properties.Settings.Default.strFormModifieSubForm.Equals(FormActualisation.frmModele.ToString()))
                    {
                        cboModele.DataSource = clsMetier.GetInstance().getAllClsmodele();
                        this.setMembersallcbo(cboModele, "Designation", "Id");
                    }
                    else if (Properties.Settings.Default.strFormModifieSubForm.Equals(FormActualisation.frmCouleur.ToString()))
                    {
                        cboCouleur.DataSource = clsMetier.GetInstance().getAllClscouleur();
                        this.setMembersallcbo(cboCouleur, "Designation", "Id");
                    }
                    else if (Properties.Settings.Default.strFormModifieSubForm.Equals(FormActualisation.frmPoids.ToString()))
                    {
                        cboPoids.DataSource = clsMetier.GetInstance().getAllClspoids();
                        this.setMembersallcbo(cboPoids, "Valeur", "Id");
                    }
                    else if (Properties.Settings.Default.strFormModifieSubForm.Equals(FormActualisation.frmEtatMateriel.ToString()))
                    {
                        cboEtat.DataSource = clsMetier.GetInstance().getAllClsetat_materiel();
                        this.setMembersallcbo(cboEtat, "Designation", "Id");
                    }
                    else if (Properties.Settings.Default.strFormModifieSubForm.Equals(FormActualisation.frmTypeOrdinateur.ToString()))
                    {
                        cboTypeOrdi.DataSource = clsMetier.GetInstance().getAllClstype_ordinateur();
                        this.setMembersallcbo(cboTypeOrdi, "Designation", "Id");
                    }
                    else if (Properties.Settings.Default.strFormModifieSubForm.Equals(FormActualisation.frmTypeClavier.ToString()))
                    {
                        cboTypeClavier.DataSource = clsMetier.GetInstance().getAllClstype_clavier();
                        this.setMembersallcbo(cboTypeClavier, "Designation", "Id");
                    }
                    else if (Properties.Settings.Default.strFormModifieSubForm.Equals(FormActualisation.frmOS.ToString()))
                    {
                        cboOS.DataSource = clsMetier.GetInstance().getAllClsOS();
                        this.setMembersallcbo(cboOS, "Designation", "Id");
                    }
                    else if (Properties.Settings.Default.strFormModifieSubForm.Equals(FormActualisation.frmGarantie.ToString()))
                    {
                        cboGarantie.DataSource = clsMetier.GetInstance().getAllClsgarantie();
                        this.setMembersallcbo(cboGarantie, "Valeur", "Id");
                    }
                    else if (Properties.Settings.Default.strFormModifieSubForm.Equals(FormActualisation.frmRAM.ToString()))
                    {
                        cboRAM.DataSource = clsMetier.GetInstance().getAllClsram();
                        this.setMembersallcbo(cboRAM, "Valeur", "Id");
                    }
                    else if (Properties.Settings.Default.strFormModifieSubForm.Equals(FormActualisation.frmProcesseur.ToString()))
                    {
                        cboProcesseur.DataSource = clsMetier.GetInstance().getAllClsprocesseur();
                        this.setMembersallcbo(cboProcesseur, "Valeur", "Id");
                    }
                    else if (Properties.Settings.Default.strFormModifieSubForm.Equals(FormActualisation.frmNbrCoeurProcesseur.ToString()))
                    {
                        cboNbrCoeurProcesseur.DataSource = clsMetier.GetInstance().getAllClsnombre_coeur_processeur();
                        this.setMembersallcbo(cboNbrCoeurProcesseur, "Valeur", "Id");
                    }
                    else if (Properties.Settings.Default.strFormModifieSubForm.Equals(FormActualisation.frmTypeHDD.ToString()))
                    {
                        cboTypeHDD.DataSource = clsMetier.GetInstance().getAllClstype_hdd();
                        this.setMembersallcbo(cboTypeHDD, "Designation", "Id");
                    }
                    else if (Properties.Settings.Default.strFormModifieSubForm.Equals(FormActualisation.frmCapaciteHDD.ToString()))
                    {
                        cboCapaciteHDD.DataSource = clsMetier.GetInstance().getAllClscapacite_hdd();
                        this.setMembersallcbo(cboCapaciteHDD, "Valeur", "Id");
                    }
                    else if (Properties.Settings.Default.strFormModifieSubForm.Equals(FormActualisation.frmNbrHDD.ToString()))
                    {
                        cboNbrHDD.DataSource = clsMetier.GetInstance().getAllClsnombre_hdd();
                        this.setMembersallcbo(cboNbrHDD, "Valeur", "Id");
                    }
                    else if (Properties.Settings.Default.strFormModifieSubForm.Equals(FormActualisation.frmTailleEcran.ToString()))
                    {
                        cboTailleEcran.DataSource = clsMetier.GetInstance().getAllClstaille_ecran();
                        this.setMembersallcbo(cboTailleEcran, "Valeur", "Id");
                    }
                    else if (Properties.Settings.Default.strFormModifieSubForm.Equals(FormActualisation.frmNbrUSB2.ToString()))
                    {
                        cboUSB2.DataSource = clsMetier.GetInstance().getAllClsusb2();
                        this.setMembersallcbo(cboUSB2, "Valeur", "Id");
                    }
                    else if (Properties.Settings.Default.strFormModifieSubForm.Equals(FormActualisation.frmNbrUSB3.ToString()))
                    {
                        cboUSB3.DataSource = clsMetier.GetInstance().getAllClsusb3();
                        this.setMembersallcbo(cboUSB3, "Valeur", "Id");
                    }
                    else if (Properties.Settings.Default.strFormModifieSubForm.Equals(FormActualisation.frmNbrHDMI.ToString()))
                    {
                        cboNbrHDMI.DataSource = clsMetier.GetInstance().getAllClshdmi();
                        this.setMembersallcbo(cboNbrHDMI, "Valeur", "Id");
                    }
                    else if (Properties.Settings.Default.strFormModifieSubForm.Equals(FormActualisation.frmNbrVGA.ToString()))
                    {
                        cboNbrVGA.DataSource = clsMetier.GetInstance().getAllClsvga();
                        this.setMembersallcbo(cboNbrVGA, "Valeur", "Id");
                    }
                    else if (Properties.Settings.Default.strFormModifieSubForm.Equals(FormActualisation.frmTensionBatterie.ToString()))
                    {
                        cboTensionBatt.DataSource = clsMetier.GetInstance().getAllClstension_batterie();
                        this.setMembersallcbo(cboTensionBatt, "Valeur", "Id");
                    }
                    else if (Properties.Settings.Default.strFormModifieSubForm.Equals(FormActualisation.frmTensionAdaptateur.ToString()))
                    {
                        cboTensionAdap.DataSource = clsMetier.GetInstance().getAllClstension_adaptateur();
                        this.setMembersallcbo(cboTensionAdap, "Valeur", "Id");
                    }
                    else if (Properties.Settings.Default.strFormModifieSubForm.Equals(FormActualisation.frmPuissanceAdaptateur.ToString()))
                    {
                        cboPuissanceAdap.DataSource = clsMetier.GetInstance().getAllClspuissance_adaptateur();
                        this.setMembersallcbo(cboPuissanceAdap, "Valeur", "Id");
                    }
                    else if (Properties.Settings.Default.strFormModifieSubForm.Equals(FormActualisation.frmIntensiteAdaptateur.ToString()))
                    {
                        cboIntensiteAdap.DataSource = clsMetier.GetInstance().getAllClsintensite_adaptateur();
                        this.setMembersallcbo(cboIntensiteAdap, "Valeur", "Id");
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

        private void TempsGenerateQrCode_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (tGenerateQrCode != null)
            {
                if (!tGenerateQrCode.IsAlive)
                {
                    tempsGenerateQrCode.Enabled = false;
                    tGenerateQrCode.Abort();
                    tGenerateQrCode = null;

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
            if (tLeftCombo != null || tMiddleCombo != null || tRightCombo != null || tDataGrid != null)
            {
                if (!tRightCombo.IsAlive)
                {
                    if (!tMiddleCombo.IsAlive)
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

                                tMiddleCombo.Abort();
                                tMiddleCombo = null;

                                tRightCombo.Abort();
                                tRightCombo = null;

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
                    else if (tGenerateQrCode != null)
                        lstThread.Add(tGenerateQrCode);
                    else if (tLeftCombo != null)
                        lstThread.Add(tLeftCombo);
                    else if (tRightCombo != null)
                        lstThread.Add(tRightCombo);
                    else if (tMiddleCombo != null)
                        lstThread.Add(tMiddleCombo);
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

        #region FOR GENERATE QRCode
        public string SaveTempImage(System.Windows.Forms.PictureBox pbox)
        {
            string filename = Environment.GetEnvironmentVariables()["TEMP"].ToString() + @"\" + "fTmp" + DateTime.Now.Millisecond.ToString() + ".png";

            using (System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite))
            {
                System.Drawing.Imaging.ImageFormat imageFormat = System.Drawing.Imaging.ImageFormat.Png;
                pbox.Image.Save(fs, imageFormat);
            }

            return filename;
        }

        public void RemoveTempImage(string fileName)
        {
            if (System.IO.File.Exists(fileName))
            {
                System.IO.File.Delete(fileName);
            }
        }
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

        private void Bs_Parse1(object sender, ConvertEventArgs e)
        {
            try
            {
                if (e.Value == null) e = null;
                else e.Value = (clsTools.Instance.GetBytesFromImage(pbPhoto1.Image));
            }
            catch (NullReferenceException ex)
            {
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de conversion de la photo : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (ArgumentException ex)
            {
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de conversion de la photo : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
        }

        private void Bs_Parse2(object sender, ConvertEventArgs e)
        {
            try
            {
                if (e.Value == null) e = null;
                else e.Value = (clsTools.Instance.GetBytesFromImage(pbPhoto2.Image));
            }
            catch (NullReferenceException ex)
            {
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de conversion de la photo : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            //catch (ArgumentException ex)
            //{
            //    Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de conversion de la photo : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
            //    ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            //}
        }

        private void Bs_Parse3(object sender, ConvertEventArgs e)
        {
            try
            {
                if (e.Value == null) e = null;
                else e.Value = (clsTools.Instance.GetBytesFromImage(pbPhoto3.Image));
            }
            catch (NullReferenceException ex)
            {
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de conversion de la photo : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (ArgumentException ex)
            {
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de conversion de la photo : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
        }
        void binding_Format1(object sender, ConvertEventArgs e)
        {
            try
            {
                pbPhoto1.Tag = null;
                if (e.DesiredType != typeof(System.Drawing.Image) || e.Value.ToString() == e.DesiredType.FullName || e.Value.ToString() == e.DesiredType.Name) return;
                if (e.Value.ToString() == "System.Drawing.Bitmap") return;
                if (e.Value == null || e.Value.ToString() == "")
                {
                    pbPhoto1.Tag = "1";
                    pbPhoto1.Image = null;
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

        void binding_Format2(object sender, ConvertEventArgs e)
        {
            try
            {
                pbPhoto2.Tag = null;
                if (e.DesiredType != typeof(System.Drawing.Image) || e.Value.ToString() == e.DesiredType.FullName || e.Value.ToString() == e.DesiredType.Name) return;
                if (e.Value.ToString() == "System.Drawing.Bitmap") return;
                if (e.Value == null || e.Value.ToString() == "")
                {
                    pbPhoto2.Tag = "1";
                    pbPhoto2.Image = null;
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

        void binding_Format3(object sender, ConvertEventArgs e)
        {
            try
            {
                pbPhoto3.Tag = null;
                if (e.DesiredType != typeof(System.Drawing.Image) || e.Value.ToString() == e.DesiredType.FullName || e.Value.ToString() == e.DesiredType.Name) return;
                if (e.Value.ToString() == "System.Drawing.Bitmap") return;
                if (e.Value == null || e.Value.ToString() == "")
                {
                    pbPhoto3.Tag = "1";
                    pbPhoto3.Image = null;
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

        private void bingImg1(PictureBox pb, Object src, string ctrProp, string obj)
        {
            pb.DataBindings.Clear();
            Binding binding = new Binding(ctrProp, src, obj, true, DataSourceUpdateMode.OnPropertyChanged);
            binding.Parse += new ConvertEventHandler(Bs_Parse1);
            binding.Format += new ConvertEventHandler(binding_Format1);
            pb.DataBindings.Add(binding);
        }

        private void bingImg2(PictureBox pb, Object src, string ctrProp, string obj)
        {
            pb.DataBindings.Clear();
            Binding binding = new Binding(ctrProp, src, obj, true, DataSourceUpdateMode.OnPropertyChanged);
            binding.Parse += new ConvertEventHandler(Bs_Parse2);
            binding.Format += new ConvertEventHandler(binding_Format2);
            pb.DataBindings.Add(binding);
        }

        private void bingImg3(PictureBox pb, Object src, string ctrProp, string obj)
        {
            pb.DataBindings.Clear();
            Binding binding = new Binding(ctrProp, src, obj, true, DataSourceUpdateMode.OnPropertyChanged);
            binding.Parse += new ConvertEventHandler(Bs_Parse3);
            binding.Format += new ConvertEventHandler(binding_Format3);
            pb.DataBindings.Add(binding);
        }

        private void BindingCls()
        {
            SetBindingControls(txtId, "Text", materiel, "Id");
            SetBindingControls(txtIdentidiant, "Text", materiel, "Code_str");
            SetBindingControls(cboCatMateriel, "SelectedValue", materiel, "Id_categorie_materiel");
            SetBindingControls(cboNumCompte, "SelectedValue", materiel, "Id_compte");
            SetBindingControls(txtLabel, "Text", materiel, "Label");
            SetBindingControls(txtDateAcquisition, "Text", materiel, "Date_acquisition");
            SetBindingControls(cboGarantie, "SelectedValue", materiel, "Id_garantie");
            SetBindingControls(cboMarque, "SelectedValue", materiel, "Id_marque");
            SetBindingControls(cboModele, "SelectedValue", materiel, "Id_modele");
            SetBindingControls(cboCouleur, "SelectedValue", materiel, "Id_couleur");
            SetBindingControls(cboPoids, "SelectedValue", materiel, "Id_poids");
            SetBindingControls(cboEtat, "SelectedValue", materiel, "Id_etat_materiel");
            bingImg1(pbPhoto1, materiel, "Image", "Photo1");
            bingImg2(pbPhoto2, materiel, "Image", "Photo2");
            bingImg3(pbPhoto3, materiel, "Image", "Photo3");
            SetBindingControls(txtMAC1, "Text", materiel, "Mac_adresse1");
            SetBindingControls(txtMAC2, "Text", materiel, "Mac_adresse2");
            SetBindingControls(txtCommentaire, "Text", materiel, "Commentaire");
            SetBindingControls(txtCreateBy, "Text", materiel, "User_created");
            SetBindingControls(txtDateCreate, "Text", materiel, "Date_created");
            SetBindingControls(txtModifieBy, "Text", materiel, "User_modified");
            SetBindingControls(txtDateModifie, "Text", materiel, "Date_modified");
            SetBindingControls(chkArchiver, "Checked", materiel, "Archiver");

            //Partie pour ordinateur
            SetBindingControls(cboTypeOrdi, "SelectedValue", materiel, "Id_type_ordinateur");
            SetBindingControls(cboTypeClavier, "SelectedValue", materiel, "Id_type_clavier");
            SetBindingControls(cboOS, "SelectedValue", materiel, "Id_os");
            SetBindingControls(cboRAM, "SelectedValue", materiel, "Id_ram");
            SetBindingControls(cboProcesseur, "SelectedValue", materiel, "Id_processeur");
            SetBindingControls(cboNbrCoeurProcesseur, "SelectedValue", materiel, "Id_nombre_coeur_processeur");
            SetBindingControls(cboTypeHDD, "SelectedValue", materiel, "Id_type_hdd");
            SetBindingControls(cboCapaciteHDD, "SelectedValue", materiel, "Id_capacite_hdd");
            SetBindingControls(cboNbrHDD, "SelectedValue", materiel, "Id_nombre_hdd");
            SetBindingControls(cboTailleEcran, "SelectedValue", materiel, "Id_taille_ecran");
            SetBindingControls(cboUSB2, "SelectedValue", materiel, "Id_usb2");
            SetBindingControls(cboUSB3, "SelectedValue", materiel, "Id_usb3");
            SetBindingControls(cboNbrHDMI, "SelectedValue", materiel, "Id_hdmi");
            SetBindingControls(cboNbrVGA, "SelectedValue", materiel, "Id_vga");
            SetBindingControls(cboTensionBatt, "SelectedValue", materiel, "Id_tension_batterie");
            SetBindingControls(cboTensionAdap, "SelectedValue", materiel, "Id_tension_adaptateur");
            SetBindingControls(cboPuissanceAdap, "SelectedValue", materiel, "Id_puissance_adaptateur");
            SetBindingControls(cboIntensiteAdap, "SelectedValue", materiel, "Id_intensite_adaptateur");
            SetBindingControls(txtNumeroCle, "Text", materiel, "Numero_cle");            
        }

        private void BindingList()
        {
            SetBindingControls(txtId, "Text", bdsrc, "Id");
            SetBindingControls(txtIdentidiant, "Text", bdsrc, "Code_str");
            SetBindingControls(cboCatMateriel, "SelectedValue", bdsrc, "Id_categorie_materiel");
            SetBindingControls(cboNumCompte, "SelectedValue", bdsrc, "Id_compte");
            SetBindingControls(txtLabel, "Text", bdsrc, "Label");
            SetBindingControls(txtDateAcquisition, "Text", bdsrc, "Date_acquisition");
            SetBindingControls(cboGarantie, "SelectedValue", bdsrc, "Id_garantie");
            SetBindingControls(cboMarque, "SelectedValue", bdsrc, "Id_marque");
            SetBindingControls(cboModele, "SelectedValue", bdsrc, "Id_modele");
            SetBindingControls(cboCouleur, "SelectedValue", bdsrc, "Id_couleur");
            SetBindingControls(cboPoids, "SelectedValue", bdsrc, "Id_poids");
            SetBindingControls(cboEtat, "SelectedValue", bdsrc, "Id_etat_materiel");
            bingImg1(pbPhoto1, bdsrc, "Image", "Photo1");
            bingImg2(pbPhoto2, bdsrc, "Image", "Photo2");
            bingImg3(pbPhoto3, bdsrc, "Image", "Photo3");
            SetBindingControls(txtMAC1, "Text", bdsrc, "Mac_adresse1");
            SetBindingControls(txtMAC2, "Text", bdsrc, "Mac_adresse2");
            SetBindingControls(txtCommentaire, "Text", bdsrc, "Commentaire");
            SetBindingControls(txtCreateBy, "Text", bdsrc, "User_created");
            SetBindingControls(txtDateCreate, "Text", bdsrc, "Date_created");
            SetBindingControls(txtModifieBy, "Text", bdsrc, "User_modified");
            SetBindingControls(txtDateModifie, "Text", bdsrc, "Date_modified");
            SetBindingControls(chkArchiver, "Checked", bdsrc, "Archiver");

            //Partie pour ordinateur
            SetBindingControls(cboTypeOrdi, "SelectedValue", bdsrc, "Id_type_ordinateur");
            SetBindingControls(cboTypeClavier, "SelectedValue", bdsrc, "Id_type_clavier");
            SetBindingControls(cboOS, "SelectedValue", bdsrc, "Id_os");
            SetBindingControls(cboRAM, "SelectedValue", bdsrc, "Id_ram");
            SetBindingControls(cboProcesseur, "SelectedValue", bdsrc, "Id_processeur");
            SetBindingControls(cboNbrCoeurProcesseur, "SelectedValue", bdsrc, "Id_nombre_coeur_processeur");
            SetBindingControls(cboTypeHDD, "SelectedValue", bdsrc, "Id_type_hdd");
            SetBindingControls(cboCapaciteHDD, "SelectedValue", bdsrc, "Id_capacite_hdd");
            SetBindingControls(cboNbrHDD, "SelectedValue", bdsrc, "Id_nombre_hdd");
            SetBindingControls(cboTailleEcran, "SelectedValue", bdsrc, "Id_taille_ecran");
            SetBindingControls(cboUSB2, "SelectedValue", bdsrc, "Id_usb2");
            SetBindingControls(cboUSB3, "SelectedValue", bdsrc, "Id_usb3");
            SetBindingControls(cboNbrHDMI, "SelectedValue", bdsrc, "Id_hdmi");
            SetBindingControls(cboNbrVGA, "SelectedValue", bdsrc, "Id_vga");
            SetBindingControls(cboTensionBatt, "SelectedValue", bdsrc, "Id_tension_batterie");
            SetBindingControls(cboTensionAdap, "SelectedValue", bdsrc, "Id_tension_adaptateur");
            SetBindingControls(cboPuissanceAdap, "SelectedValue", bdsrc, "Id_puissance_adaptateur");
            SetBindingControls(cboIntensiteAdap, "SelectedValue", bdsrc, "Id_intensite_adaptateur");
            SetBindingControls(txtNumeroCle, "Text", bdsrc, "Numero_cle");
        }
        #endregion

        private void frmOrdinateur_Load(object sender, EventArgs e)
        {
            cmdArchiver.Enabled = false;
            cboCatMateriel.Enabled = false;

            //Initialise timers
            tempsActualiseCombo = new System.Timers.Timer();
            tempsActualiseCombo.Interval = 100;

            tempsGenerateQrCode = new System.Timers.Timer();
            tempsGenerateQrCode.Interval = 100;

            tempsSelectionChangeDataGrid = new System.Timers.Timer();
            tempsSelectionChangeDataGrid.Interval = 100;

            tempsRefreshData = new System.Timers.Timer();
            tempsRefreshData.Interval = 100;

            tempsActivateForm = new System.Timers.Timer();
            tempsActivateForm.Interval = 100;

            tempsStopWaitCursor = new System.Timers.Timer();
            tempsStopWaitCursor.Interval = 50;

            //Affecte MenuStrip
            pbPhoto1.ContextMenuStrip = ctxMenuPhoto1;
            pbPhoto2.ContextMenuStrip = ctxMenuPhoto2;
            pbPhoto3.ContextMenuStrip = ctxMenuPhoto3;

            //executed in many independant thread
            RefreshData();
        }

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

                    tMiddleCombo = new Thread(new ThreadStart(ExecuteMiddleCombo));
                    tMiddleCombo.Start();

                    tRightCombo = new Thread(new ThreadStart(ExecuteRightCombo));
                    tRightCombo.Start();

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

        private void frmOrdinateur_Activated(object sender, EventArgs e)
        {
            Principal.SetValuesLabel(Properties.Settings.Default.UserConnected, "Gestion des ordinateurs");
            Principal.SetCurrentICRUDChildForm(this);

            try
            {
                if (firstLoad)
                {
                    tempsActivateForm.Enabled = true;
                    tempsActivateForm.Elapsed += TempsActivateForm_Elapsed;

                    if (tDataGrid == null)
                    {
                        tDataGrid = new Thread(new ThreadStart(ExecuteDataGrid));
                        tDataGrid.Start();
                    }
                }
            }
            catch { }

            firstLoad = true;
        }

        private void frmOrdinateur_FormClosed(object sender, FormClosedEventArgs e)
        {
            Principal.SetValuesLabel(Properties.Settings.Default.UserConnected, "Attente d'une action de l'utilisateur");

            //Reinitialise all Thread
            try
            {
                this.UnloadThreadRessource(tDataGrid);
                this.UnloadThreadRessource(tSelectionChangeDataGrid);
                this.UnloadThreadRessource(tGenerateQrCode);
                this.UnloadThreadRessource(tLeftCombo);
                this.UnloadThreadRessource(tRightCombo);
                this.UnloadThreadRessource(tMiddleCombo);
                this.UnloadThreadRessource(tActualiseComb);
                this.UnloadThreadRessource(tStopWaitCursor);
            }
            catch { }

            Principal.ApplyDefaultStatusBar(Principal, Properties.Settings.Default.UserConnected);
            //Affecte Activate BindingNavigator
            Principal.ActivateMainBindingSource(Principal);
        }

        public void New()
        {
            try
            {
                //Initialise object class
                materiel = new clsmateriel();

                Principal.ActivateOnNewCommandButtons(true);
                blnModifie = false;

                BindingCls();

                //Set the new ID
                if (newID == null)
                    newID = clsMetier.GetInstance().GenerateLastID("materiel");
                txtId.Text = newID.ToString();
                txtCreateBy.Text = Properties.Settings.Default.UserConnected;
                txtDateCreate.Text = DateTime.Now.ToString();
                lblStatusGuaraty.Text = "";
                pbQRCode.Image = null;
                cmdArchiver.Enabled = false;

                //Selection automatique de l'item du combo
                cboCatMateriel.Text = clsMetier.GetInstance().getClscategorie_materiel("1").Designation.ToString();
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
                        List<clsmateriel> lstItemSearch = new List<clsmateriel>();
                        lstItemSearch = clsMetier.GetInstance().getAllClsmateriel(criteria);

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
                    if (chkArchiver.Checked)
                        throw new CustomException("Vous ne pouvez archiver un enregistrement non encore sauvegardé !!! Réessayer svp !!!");

                    materiel.Qrcode = tmpQrCode;
                    int record = materiel.inserts();
                    tmpQrCode = null;
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
            ((clsmateriel)bdsrc.Current).User_modified = Properties.Settings.Default.UserConnected;
            ((clsmateriel)bdsrc.Current).Date_modified = DateTime.Now;

            clsmateriel mat = new clsmateriel();
            mat = ((clsmateriel)bdsrc.Current);
            if (blnPhoto1)
            {
                mat.Photo1 = materiel.Photo1;
                blnPhoto1 = false;
            }
            if (blnPhoto2)
            {
                mat.Photo2 = materiel.Photo2;
                blnPhoto2 = false;
            }
            if (blnPhoto3)
            {
                mat.Photo3 = materiel.Photo3;
                blnPhoto3 = false;
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
                        record = materiel.delete(((clsmateriel)bdsrc.Current));
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
            frmReportMateriel frm = new frmReportMateriel();
            frm.MdiParent = Principal;
            //frm.setData(factory.getAllSexe_Dt(), @"D:\appStockMS\appStock.Desktop\reports\rptListSexe.rdlc");
            frm.Icon = this.Icon;
            frm.Show();
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
            catch (ArgumentException ex)
            {
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors de lexécution du thread : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
        }

        private void lblAddCategorieMat_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmCategorieMateriel frm = new frmCategorieMateriel();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddNumCompte_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmNumeroCompte frm = new frmNumeroCompte();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void cboCatMateriel_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void cboNumCompte_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void lblAddMarque_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmMarque frm = new frmMarque();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddModele_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmModele frm = new frmModele();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void cboModele_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void cboCouleur_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void cboPoids_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void cboEtat_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void cboTypeOrdi_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void cboTypeClavier_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void cboNbrCoeurProcesseur_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void cboCapaciteHDD_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void cboUSB2_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void cboUSB3_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void cboNbrHDMI_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void cboNbrVGA_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void cboTensionBatt_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void cboTensionAdap_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void cboPuissanceAdap_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void cboIntensiteAdap_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void lblAddCouleur_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmCouleur frm = new frmCouleur();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddPoids_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmPoids frm = new frmPoids();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblEtatMatriel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmEtatMateriel frm = new frmEtatMateriel();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddPC_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmTypeOrdinateur frm = new frmTypeOrdinateur();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddTypeClavier_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmTypeClavier frm = new frmTypeClavier();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddOS_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmOS frm = new frmOS();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void cboMarque_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void lblPhoto1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                LoadPicture1();
            }
            catch (CustomException ex)
            {
                Properties.Settings.Default.StringLogFile = ex.Message;
                MessageBox.Show(Properties.Settings.Default.StringLogFile, stringManager.GetString("StringFailedLoadPictureCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de chargement de la photo  : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
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

        private void LoadPicture1()
        {
            OpenFileDialog open;
            DialogResult result;
            LoadPicture(out open, out result);

            if (result == DialogResult.OK)
            {
                string strChaine = clsTools.Instance.LimiteImageSize1(open.FileName, 1024000, 1000, 1000);

                if (string.IsNullOrEmpty(strChaine))
                {
                    pbPhoto1.Load(open.FileName);
                    blnPhoto1 = true;
                    materiel.Photo1 = clsTools.Instance.GetByteFromFile(open.FileName);
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

        private void lblPhoto2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                LoadPicture2();
            }
            catch (CustomException ex)
            {
                Properties.Settings.Default.StringLogFile = ex.Message;
                MessageBox.Show(Properties.Settings.Default.StringLogFile, stringManager.GetString("StringFailedLoadPictureCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de chargement de la photo  : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (System.IO.FileNotFoundException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadPictureMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadPictureCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur de chargement de la photo2 : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (System.IO.IOException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadPictureMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadPictureCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur de chargement de la photo2 : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
        }

        private void LoadPicture2()
        {
            OpenFileDialog open;
            DialogResult result;
            LoadPicture(out open, out result);

            if (result == DialogResult.OK)
            {
                string strChaine = clsTools.Instance.LimiteImageSize1(open.FileName, 1024000, 1000, 1000);

                if (string.IsNullOrEmpty(strChaine))
                {
                    pbPhoto2.Load(open.FileName);
                    blnPhoto2 = true;
                    materiel.Photo2 = clsTools.Instance.GetByteFromFile(open.FileName);
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

        private void lblPhoto3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                LoadPicture3();
            }
            catch (CustomException ex)
            {
                Properties.Settings.Default.StringLogFile = ex.Message;
                MessageBox.Show(Properties.Settings.Default.StringLogFile, stringManager.GetString("StringFailedLoadPictureCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de chargement de la photo  : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (System.IO.FileNotFoundException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadPictureMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadPictureCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur de chargement de la photo3 : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (System.IO.IOException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadPictureMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadPictureCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur de chargement de la photo3 : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
        }

        private void LoadPicture3()
        {
            OpenFileDialog open;
            DialogResult result;
            LoadPicture(out open, out result);

            if (result == DialogResult.OK)
            {
                string strChaine = clsTools.Instance.LimiteImageSize1(open.FileName, 1024000, 1000, 1000);

                if (string.IsNullOrEmpty(strChaine))
                {
                    pbPhoto3.Load(open.FileName);
                    blnPhoto3 = true;
                    materiel.Photo3 = clsTools.Instance.GetByteFromFile(open.FileName);
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

        private void lblAddGuaratie_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmGarantie frm = new frmGarantie();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void cboGarantie_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void cboRAM_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void cboProcesseur_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void cboTailleEcran_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void lblAddRAM_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmRAM frm = new frmRAM();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddProcessor_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmProcesseur frm = new frmProcesseur();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddCorProcessor_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmNbrCoeurProcesseur frm = new frmNbrCoeurProcesseur();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddTypeHDD_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmTypeHDD frm = new frmTypeHDD();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddCapacityHDD_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmCapaciteHDD frm = new frmCapaciteHDD();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddNbrHDD_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmNbrHDD frm = new frmNbrHDD();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddScreen_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmTailleEcran frm = new frmTailleEcran();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddUSB2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmNbrUSB2 frm = new frmNbrUSB2();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddUSB3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmNbrUSB3 frm = new frmNbrUSB3();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddHDMI_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmNbrHDMI frm = new frmNbrHDMI();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddVGA_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmNbrVGA frm = new frmNbrVGA();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddUBatterie_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmTensionBatterie frm = new frmTensionBatterie();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddUAdapt_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmTensionAdaptateur frm = new frmTensionAdaptateur();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddPAdapt_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmPuissanceAdaptateur frm = new frmPuissanceAdaptateur();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddIAdapt_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmIntensiteAdaptateur frm = new frmIntensiteAdaptateur();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void smnCtxPhoto3_Click(object sender, EventArgs e)
        {
            try
            {
                LoadPicture3();
            }
            catch (CustomException ex)
            {
                Properties.Settings.Default.StringLogFile = ex.Message;
                MessageBox.Show(Properties.Settings.Default.StringLogFile, stringManager.GetString("StringFailedLoadPictureCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de chargement de la photo  : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (System.IO.FileNotFoundException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadPictureMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadPictureCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur de chargement de la photo3 : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (System.IO.IOException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadPictureMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadPictureCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur de chargement de la photo3 : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
        }

        private void pbPhoto3_MouseHover(object sender, EventArgs e)
        {
            pbPhoto3.Cursor = Cursors.Hand;
        }

        private void pbPhoto2_MouseHover(object sender, EventArgs e)
        {
            pbPhoto2.Cursor = Cursors.Hand;
        }

        private void pbPhoto1_MouseHover(object sender, EventArgs e)
        {
            pbPhoto1.Cursor = Cursors.Hand;
        }

        private void pbPhoto1_MouseLeave(object sender, EventArgs e)
        {
            pbPhoto1.Cursor = Cursors.Default;
        }

        private void pbPhoto2_MouseLeave(object sender, EventArgs e)
        {
            pbPhoto2.Cursor = Cursors.Default;
        }

        private void pbPhoto3_MouseLeave(object sender, EventArgs e)
        {
            pbPhoto3.Cursor = Cursors.Default;
        }

        private void cmdArchiver_Click(object sender, EventArgs e)
        {
            try
            {
                clsmateriel mat = new clsmateriel();
                mat = ((clsmateriel)bdsrc.Current);
                int record = clsMetier.GetInstance().ArchiverMateriel(mat);

                MessageBox.Show(stringManager.GetString("StringSuccessArchiveMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringSuccessArchiveCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                RefreshData();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedArchiveMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedArchiveCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de l'archivage de l'enregistrement : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
        }

        private void cboOS_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void cboTypeHDD_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void cboNbrHDD_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void cboNumCompte_Leave(object sender, EventArgs e)
        {
            //try
            //{
                if (!string.IsNullOrEmpty(txtId.Text))
                {
                    tempsGenerateQrCode.Enabled = true;
                    tempsGenerateQrCode.Elapsed += TempsGenerateQrCode_Elapsed;

                    //Executed in new thread
                    if (tGenerateQrCode == null)
                    {
                        tGenerateQrCode = new Thread(new ThreadStart(ExecuteGenerateQrCode));
                        tGenerateQrCode.Start();
                    }
                }
            //}
            //catch { }
        }

        private void smnCtxPhoto1_Click(object sender, EventArgs e)
        {
            try
            {
                LoadPicture1();
            }
            catch (CustomException ex)
            {
                Properties.Settings.Default.StringLogFile = ex.Message;
                MessageBox.Show(Properties.Settings.Default.StringLogFile, stringManager.GetString("StringFailedLoadPictureCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de chargement de la photo  : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
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

        private void smnCtxPhoto2_Click(object sender, EventArgs e)
        {
            try
            {
                LoadPicture2();
            }
            catch (CustomException ex)
            {
                Properties.Settings.Default.StringLogFile = ex.Message;
                MessageBox.Show(Properties.Settings.Default.StringLogFile, stringManager.GetString("StringFailedLoadPictureCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de chargement de la photo  : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (System.IO.FileNotFoundException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadPictureMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadPictureCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur de chargement de la photo2 : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (System.IO.IOException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadPictureMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadPictureCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur de chargement de la photo2 : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
        }
    }
}
