using ManageUtilities;
using smartManage.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows.Forms;

namespace smartManage.Desktop
{
    public partial class frmReportMateriel : Form
    {
        IDbConnection conn = null;
        private delegate void LoadSomeData(string ThreadName);
        private Thread tLoad = null;
        private ResourceManager stringManager = null;

        public frmReportMateriel()
        {
            InitializeComponent();
            //Initialisation du ressourceManager
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
        private void SetSelectedIndexComboBox(List<ComboBox> cbo)
        {
            foreach (ComboBox cmb in cbo)
                if (cmb.Items.Count > 0)
                    cmb.SelectedIndex = 0;
        }

        private void DoExecuteLoadData(string threadName)
        {
            cboIdentifiant.DataSource = clsMetier.GetInstance().getAllClsmateriel();
            this.setMembersallcbo(cboIdentifiant, "Code_str", "Code_str");
            cboEtat.DataSource = clsMetier.GetInstance().getAllClsetat_materiel();
            this.setMembersallcbo(cboEtat, "Designation", "Designation");
            cboDelais.DataSource = clsMetier.GetInstance().getAllClsgarantie();
            this.setMembersallcbo(cboDelais, "Valeur", "Valeur");
            cboMacWifi.DataSource = clsMetier.GetInstance().getAllClsmateriel_MAC1();
            this.setMembersallcbo(cboMacWifi, "Mac_adresse1", "Mac_adresse1");
            cboMacLAN.DataSource = clsMetier.GetInstance().getAllClsmateriel_MAC2();
            this.setMembersallcbo(cboMacLAN, "Mac_adresse2", "Mac_adresse2");
            cboCategorieMat.DataSource = clsMetier.GetInstance().getAllClscategorie_materiel1();
            this.setMembersallcbo(cboCategorieMat, "Designation", "Id");
            cboPortee.DataSource = clsMetier.GetInstance().getAllClsportee(); 
            this.setMembersallcbo(cboPortee, "Valeur", "Valeur");
            cboFrequence.DataSource = clsMetier.GetInstance().getAllClsfrequence();
            this.setMembersallcbo(cboFrequence, "Designation", "Designation");
            cboNetete.DataSource = clsMetier.GetInstance().getAllClsnetette();
            this.setMembersallcbo(cboNetete, "Designation", "Designation");
            cboPPM.DataSource = clsMetier.GetInstance().getAllClspage_par_minute();
            this.setMembersallcbo(cboPPM, "Valeur", "Valeur");
            cboMACAdresse.DataSource = clsMetier.GetInstance().getAllClsmateriel_MAC1();
            this.setMembersallcbo(cboMACAdresse, "Mac_adresse1", "Mac_adresse1");

            List<ComboBox> lstCombo = new List<ComboBox>() { cboIdentifiant, cboEtat, cboDelais, cboMacWifi, cboMacLAN,
                cboCategorieMat, cboPortee, cboFrequence, cboNetete, cboPPM, cboMACAdresse };

            SetSelectedIndexComboBox(lstCombo);
        }

        private void ExecuteLoadData()
        {
            LoadSomeData loadDt = new LoadSomeData(DoExecuteLoadData);

            try
            {
                this.Invoke(loadDt, "tLoad");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadComboBoxMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadComboBoxCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur de chargement des listes déroulantes de gauche dans le thread : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadComboBoxMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadComboBoxCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur de chargement des listes déroulantes de gauche dans le thread : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
        }
        #endregion

        private void LoadReport(int cboIndex, int categorie, Control radiobutton)
        {
            //Initialisation de la chaine de connexion
            conn = new SqlConnection(Model.Properties.Settings.Default.strChaineConnexion);

            if (conn.State == ConnectionState.Closed)
                conn.Open();

            switch(categorie)
            {
                case 0:
                    //*****Choix pour Ordinateur
                    #region Choix pour Ordinateur
                    using (IDbCommand cmd = conn.CreateCommand())
                    {
                        try
                        {
                            SqlDataAdapter adapter = null;
                            DataSet dataset = null;

                            RadioButton rd = radiobutton as RadioButton;

                            if (cboIndex == 0 && rd.Name.Equals(rdLstIdentifiant.Name))
                            {
                                #region Ordinateur par identifiant equipement
                                cmd.CommandText = Queries.GetInstance().CommonQueryMateriel(@"where  categorie_materiel.designation='Ordinateur' and materiel.code_str=@code_str and materiel.archiver=@archiver");

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@code_str", DbType.String, 10, cboIdentifiant.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = LoadRdlcReport.GetInstance().LoadReportWithSubReportSignataire(adapter, "DataSet1", "smartManage.Desktop.Reports.RptOrdinateur.rdlc", rpvReport);
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstEtat.Name))
                            {
                                #region Ordinateur par etat de l'equipement
                                cmd.CommandText = Queries.GetInstance().CommonQueryMateriel("where categorie_materiel.designation='Ordinateur' and etat_materiel.designation=@designation and archiver=@archiver");

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@designation", DbType.String, 50, cboEtat.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = LoadRdlcReport.GetInstance().LoadReportWithSubReportSignataire(adapter, "DataSet1", "smartManage.Desktop.Reports.RptOrdinateur.rdlc", rpvReport);
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstEndGarantie.Name))
                            {
                                #region Ordinateur par délais de garantie de l'equipement
                                cmd.CommandText = Queries.GetInstance().CommonQueryMateriel("where categorie_materiel.designation='Ordinateur' and garantie.valeur=@valeur and archiver=@archiver");

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@valeur", DbType.Int32, 4, Convert.ToInt32(cboDelais.SelectedValue, CultureInfo.CurrentCulture)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = LoadRdlcReport.GetInstance().LoadReportWithSubReportSignataire(adapter, "DataSet1", "smartManage.Desktop.Reports.RptOrdinateur.rdlc", rpvReport);
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstMAC.Name))
                            {
                                #region Ordinateur par MAC Wifi de l'equipement
                                cmd.CommandText = Queries.GetInstance().CommonQueryMateriel("where categorie_materiel.designation='Ordinateur' and materiel.mac_adresse1 LIKE @mac_adresse1 and archiver=@archiver");

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@mac_adresse1", DbType.String, 20, cboMacWifi.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = LoadRdlcReport.GetInstance().LoadReportWithSubReportSignataire(adapter, "DataSet1", "smartManage.Desktop.Reports.RptOrdinateur.rdlc", rpvReport);
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstDateAcquisition.Name))
                            {
                                #region Ordinateur par date d'acquisition de l'equipement
                                cmd.CommandText = Queries.GetInstance().CommonQueryMateriel("where categorie_materiel.designation='Ordinateur' and (convert(date,materiel.date_acquisition,100) between @date_acquisition1 and @date_acquisition2) and archiver=@archiver");

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);
                                //Console.WriteLine(txtDateAcquisitionDebut.Text);
                                //Console.WriteLine(txtDateAcquisitionFin.Text);
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@date_acquisition1", DbType.String, 10, Convert.ToString(txtDateAcquisitionDebut.Text)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@date_acquisition2", DbType.String, 10, Convert.ToString(txtDateAcquisitionFin.Text)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = LoadRdlcReport.GetInstance().LoadReportWithSubReportSignataire(adapter, "DataSet1", "smartManage.Desktop.Reports.RptOrdinateur.rdlc", rpvReport);
                                #endregion
                            }
                            else if (cboIndex == 1 && rd.Name.Equals(rdLstMAC.Name))
                            {
                                #region par MAC LAN de l'equipement
                                cmd.CommandText = Queries.GetInstance().CommonQueryMateriel("where categorie_materiel.designation='Ordinateur' and materiel.mac_adresse2 LIKE @mac_adresse2 and archiver=@archiver");

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@mac_adresse2", DbType.String, 20, cboMacLAN.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = LoadRdlcReport.GetInstance().LoadReportWithSubReportSignataire(adapter, "DataSet1", "smartManage.Desktop.Reports.RptOrdinateur.rdlc", rpvReport);
                                #endregion
                            }
                            else if (cboIndex == 2 && rd.Name.Equals(rdLstMAC.Name))
                            {
                                #region par MAC Wifi et LAN de l'equipement 
                                cmd.CommandText = Queries.GetInstance().CommonQueryMateriel("where categorie_materiel.designation='Ordinateur' and (materiel.mac_adresse1 LIKE @mac_adresse1 or materiel.mac_adresse2 LIKE @mac_adresse2) and archiver=@archiver");

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@mac_adresse1", DbType.String, 20, cboMacWifi.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@mac_adresse2", DbType.String, 20, cboMacLAN.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = LoadRdlcReport.GetInstance().LoadReportWithSubReportSignataire(adapter, "DataSet1", "smartManage.Desktop.Reports.RptOrdinateur.rdlc", rpvReport);
                                #endregion
                            }
                            else
                                throw new CustomException("Sélection invalide, recommencez !!!");

                            if (dataset != null)
                                dataset.Dispose();
                            if (adapter != null)
                                adapter.Dispose();
                            if (conn != null)
                                conn.Close();
                        }
                        catch (InvalidOperationException ex)
                        {
                            MessageBox.Show(stringManager.GetString("StringFailedLoadReportMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadReportCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de chargement du rapport : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                            ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                        }
                    }
                    #endregion
                    break;
                case 1:
                    //*****Choix pour Switch
                    #region Choix pour Switch
                    using (IDbCommand cmd = conn.CreateCommand())
                    {
                        try
                        {
                            SqlDataAdapter adapter = null;
                            DataSet dataset = null;

                            RadioButton rd = radiobutton as RadioButton;

                            if (cboIndex == 0 && rd.Name.Equals(rdLstIdentifiant.Name))
                            {
                                #region Switch par identifiant equipement
                                cmd.CommandText = Queries.GetInstance().CommonQueryMateriel("where  categorie_materiel.designation='Switch' and materiel.code_str=@code_str and materiel.archiver=@archiver");

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@code_str", DbType.String, 10, cboIdentifiant.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = LoadRdlcReport.GetInstance().LoadReportWithSubReportSignataire(adapter, "DataSet1", "smartManage.Desktop.Reports.RptSwitch.rdlc", rpvReport);
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstEtat.Name))
                            {
                                #region Switch par etat de l'equipement
                                cmd.CommandText = Queries.GetInstance().CommonQueryMateriel("where categorie_materiel.designation='Switch' and etat_materiel.designation=@designation and archiver=@archiver");

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@designation", DbType.String, 50, cboEtat.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = LoadRdlcReport.GetInstance().LoadReportWithSubReportSignataire(adapter, "DataSet1", "smartManage.Desktop.Reports.RptSwitch.rdlc", rpvReport);
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstEndGarantie.Name))
                            {
                                #region Switch par délais de garantie de l'equipement
                                cmd.CommandText = Queries.GetInstance().CommonQueryMateriel("where categorie_materiel.designation='Switch' and garantie.valeur=@valeur and archiver=@archiver");

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@valeur", DbType.Int32, 4, Convert.ToInt32(cboDelais.SelectedValue, CultureInfo.CurrentCulture)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = LoadRdlcReport.GetInstance().LoadReportWithSubReportSignataire(adapter, "DataSet1", "smartManage.Desktop.Reports.RptSwitch.rdlc", rpvReport);
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstMAC_Adresse.Name))
                            {
                                #region Switch par MAC l'equipement
                                cmd.CommandText = Queries.GetInstance().CommonQueryMateriel("where categorie_materiel.designation='Switch' and materiel.mac_adresse1 LIKE @mac_adresse1 and archiver=@archiver");

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@mac_adresse1", DbType.String, 20, cboMACAdresse.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = LoadRdlcReport.GetInstance().LoadReportWithSubReportSignataire(adapter, "DataSet1", "smartManage.Desktop.Reports.RptSwitch.rdlc", rpvReport);
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstDateAcquisition.Name))
                            {
                                #region Switch par date d'acquisition de l'equipement
                                cmd.CommandText = Queries.GetInstance().CommonQueryMateriel("where categorie_materiel.designation='Switch' and (convert(date,materiel.date_acquisition,100) between @date_acquisition1 and @date_acquisition2) and archiver=@archiver");

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@date_acquisition1", DbType.String, 10, Convert.ToString(txtDateAcquisitionDebut.Text)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@date_acquisition2", DbType.String, 10, Convert.ToString(txtDateAcquisitionFin.Text)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = LoadRdlcReport.GetInstance().LoadReportWithSubReportSignataire(adapter, "DataSet1", "smartManage.Desktop.Reports.RptSwitch.rdlc", rpvReport);
                                #endregion
                            }
                            else
                                throw new CustomException("Sélection invalide, recommencez !!!");

                            if (dataset != null)
                                dataset.Dispose();
                            if (adapter != null)
                                adapter.Dispose();
                            if (conn != null)
                                conn.Close();
                        }
                        catch (InvalidOperationException ex)
                        {
                            MessageBox.Show(stringManager.GetString("StringFailedLoadReportMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadReportCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de chargement du rapport : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                            ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                        }
                    }
                    #endregion
                    break;
                case 2:
                    //*****Choix pour Imprimante
                    #region Choix pour Imprimante
                    using (IDbCommand cmd = conn.CreateCommand())
                    {
                        try
                        {
                            SqlDataAdapter adapter = null;
                            DataSet dataset = null;

                            RadioButton rd = radiobutton as RadioButton;

                            if (cboIndex == 0 && rd.Name.Equals(rdLstIdentifiant.Name))
                            {
                                #region Imprimante par identifiant equipement
                                cmd.CommandText = Queries.GetInstance().CommonQueryMateriel("where  categorie_materiel.designation='Imprimante' and materiel.code_str=@code_str and materiel.archiver=@archiver");

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@code_str", DbType.String, 10, cboIdentifiant.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = LoadRdlcReport.GetInstance().LoadReportWithSubReportSignataire(adapter, "DataSet1", "smartManage.Desktop.Reports.RptImprimante.rdlc", rpvReport);
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstEtat.Name))
                            {
                                #region Imprimante par etat de l'equipement
                                cmd.CommandText = Queries.GetInstance().CommonQueryMateriel("where categorie_materiel.designation='Imprimante' and etat_materiel.designation=@designation and archiver=@archiver");

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@designation", DbType.String, 50, cboEtat.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = LoadRdlcReport.GetInstance().LoadReportWithSubReportSignataire(adapter, "DataSet1", "smartManage.Desktop.Reports.RptImprimante.rdlc", rpvReport);
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstEndGarantie.Name))
                            {
                                #region Imprimante par délais de garantie de l'equipement
                                cmd.CommandText = Queries.GetInstance().CommonQueryMateriel("where categorie_materiel.designation='Imprimante' and garantie.valeur=@valeur and archiver=@archiver");

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@valeur", DbType.Int32, 4, Convert.ToInt32(cboDelais.SelectedValue, CultureInfo.CurrentCulture)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = LoadRdlcReport.GetInstance().LoadReportWithSubReportSignataire(adapter, "DataSet1", "smartManage.Desktop.Reports.RptImprimante.rdlc", rpvReport);
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstPPM.Name))
                            {
                                #region Imprimante par PPM l'equipement
                                cmd.CommandText = Queries.GetInstance().CommonQueryMateriel("where categorie_materiel.designation='Imprimante' and page_par_minute.valeur=@page_par_minute and archiver=@archiver");

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@page_par_minute", DbType.Int32, 4, Convert.ToInt32(cboPPM.SelectedValue, CultureInfo.InvariantCulture)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = LoadRdlcReport.GetInstance().LoadReportWithSubReportSignataire(adapter, "DataSet1", "smartManage.Desktop.Reports.RptImprimante.rdlc", rpvReport);
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstDateAcquisition.Name))
                            {
                                #region Imprimante par date d'acquisition de l'equipement
                                cmd.CommandText = Queries.GetInstance().CommonQueryMateriel("where categorie_materiel.designation='Imprimante' and (convert(date,materiel.date_acquisition,100) between @date_acquisition1 and @date_acquisition2) and archiver=@archiver");

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@date_acquisition1", DbType.String, 10, Convert.ToString(txtDateAcquisitionDebut.Text)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@date_acquisition2", DbType.String, 10, Convert.ToString(txtDateAcquisitionFin.Text)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = LoadRdlcReport.GetInstance().LoadReportWithSubReportSignataire(adapter, "DataSet1", "smartManage.Desktop.Reports.RptImprimante.rdlc", rpvReport);
                                #endregion
                            }
                            else
                                throw new CustomException("Sélection invalide, recommencez !!!");

                            if (dataset != null)
                                dataset.Dispose();
                            if (adapter != null)
                                adapter.Dispose();
                            if (conn != null)
                                conn.Close();
                        }
                        catch (InvalidOperationException ex)
                        {
                            MessageBox.Show(stringManager.GetString("StringFailedLoadReportMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadReportCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de chargement du rapport : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                            ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                        }
                    }
                    #endregion
                    break;
                case 3:
                    //*****Choix pour Emetteur
                    #region Choix pour Emetteur
                    using (IDbCommand cmd = conn.CreateCommand())
                    {
                        try
                        {
                            SqlDataAdapter adapter = null;
                            DataSet dataset = null;

                            RadioButton rd = radiobutton as RadioButton;

                            if (cboIndex == 0 && rd.Name.Equals(rdLstIdentifiant.Name))
                            {
                                #region Emetteur par identifiant equipement
                                cmd.CommandText = Queries.GetInstance().CommonQueryMateriel("where  categorie_materiel.designation='Emetteur' and materiel.code_str=@code_str and materiel.archiver=@archiver");

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@code_str", DbType.String, 10, cboIdentifiant.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = LoadRdlcReport.GetInstance().LoadReportWithSubReportSignataire(adapter, "DataSet1", "smartManage.Desktop.Reports.RptEmetteur.rdlc", rpvReport);
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstEtat.Name))
                            {
                                #region Emetteur par etat de l'equipement
                                cmd.CommandText = Queries.GetInstance().CommonQueryMateriel("where categorie_materiel.designation='Emetteur' and etat_materiel.designation=@designation and archiver=@archiver");

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@designation", DbType.String, 50, cboEtat.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = LoadRdlcReport.GetInstance().LoadReportWithSubReportSignataire(adapter, "DataSet1", "smartManage.Desktop.Reports.RptEmetteur.rdlc", rpvReport);
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstEndGarantie.Name))
                            {
                                #region Emetteur par délais de garantie de l'equipement
                                cmd.CommandText = Queries.GetInstance().CommonQueryMateriel("where categorie_materiel.designation='Emetteur' and garantie.valeur=@valeur and archiver=@archiver");

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@valeur", DbType.Int32, 4, Convert.ToInt32(cboDelais.SelectedValue, CultureInfo.CurrentCulture)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = LoadRdlcReport.GetInstance().LoadReportWithSubReportSignataire(adapter, "DataSet1", "smartManage.Desktop.Reports.RptEmetteur.rdlc", rpvReport);
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstFrequence.Name))
                            {
                                #region Emetteur par frequence de fonctionnement l'equipement
                                cmd.CommandText = Queries.GetInstance().CommonQueryMateriel("where categorie_materiel.designation='Emetteur' and frequence.designation=@frequence and archiver=@archiver");

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@frequence", DbType.String, 20, cboFrequence.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = LoadRdlcReport.GetInstance().LoadReportWithSubReportSignataire(adapter, "DataSet1", "smartManage.Desktop.Reports.RptEmetteur.rdlc", rpvReport);
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstDateAcquisition.Name))
                            {
                                #region Emetteur par date d'acquisition de l'equipement
                                cmd.CommandText = Queries.GetInstance().CommonQueryMateriel("where categorie_materiel.designation='Emetteur' and (convert(date,materiel.date_acquisition,100) between @date_acquisition1 and @date_acquisition2) and archiver=@archiver");

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@date_acquisition1", DbType.String, 10, Convert.ToString(txtDateAcquisitionDebut.Text)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@date_acquisition2", DbType.String, 10, Convert.ToString(txtDateAcquisitionFin.Text)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = LoadRdlcReport.GetInstance().LoadReportWithSubReportSignataire(adapter, "DataSet1", "smartManage.Desktop.Reports.RptEmetteur.rdlc", rpvReport);
                                #endregion
                            }
                            else
                                throw new CustomException("Sélection invalide, recommencez !!!");

                            if (dataset != null)
                                dataset.Dispose();
                            if (adapter != null)
                                adapter.Dispose();
                            if (conn != null)
                                conn.Close();
                        }
                        catch (InvalidOperationException ex)
                        {
                            MessageBox.Show(stringManager.GetString("StringFailedLoadReportMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadReportCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de chargement du rapport : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                            ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                        }
                    }
                    #endregion
                    break;
                case 4:
                    //*****Choix pour Amplificateur
                    #region Choix pour Amplificateur
                    using (IDbCommand cmd = conn.CreateCommand())
                    {
                        try
                        {
                            SqlDataAdapter adapter = null;
                            DataSet dataset = null;

                            RadioButton rd = radiobutton as RadioButton;

                            if (cboIndex == 0 && rd.Name.Equals(rdLstIdentifiant.Name))
                            {
                                #region Amplificateur par identifiant equipement
                                cmd.CommandText = Queries.GetInstance().CommonQueryMateriel("where  categorie_materiel.designation='Emetteur' and materiel.code_str=@code_str and materiel.archiver=@archiver");

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@code_str", DbType.String, 10, cboIdentifiant.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = LoadRdlcReport.GetInstance().LoadReportWithSubReportSignataire(adapter, "DataSet1", "smartManage.Desktop.Reports.RptAmplificateur.rdlc", rpvReport);
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstEtat.Name))
                            {
                                #region Amplificateur par etat de l'equipement
                                cmd.CommandText = Queries.GetInstance().CommonQueryMateriel("where categorie_materiel.designation='Amplificateur' and etat_materiel.designation=@designation and archiver=@archiver");

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@designation", DbType.String, 50, cboEtat.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = LoadRdlcReport.GetInstance().LoadReportWithSubReportSignataire(adapter, "DataSet1", "smartManage.Desktop.Reports.RptAmplificateur.rdlc", rpvReport);
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstEndGarantie.Name))
                            {
                                #region Amplificateur par délais de garantie de l'equipement
                                cmd.CommandText = Queries.GetInstance().CommonQueryMateriel("where categorie_materiel.designation='Amplificateur' and garantie.valeur=@valeur and archiver=@archiver");

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@valeur", DbType.Int32, 4, Convert.ToInt32(cboDelais.SelectedValue, CultureInfo.CurrentCulture)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = LoadRdlcReport.GetInstance().LoadReportWithSubReportSignataire(adapter, "DataSet1", "smartManage.Desktop.Reports.RptAmplificateur.rdlc", rpvReport);
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstDateAcquisition.Name))
                            {
                                #region Amplificateur par date d'acquisition de l'equipement
                                cmd.CommandText = Queries.GetInstance().CommonQueryMateriel("where categorie_materiel.designation='Amplificateur' and (convert(date,materiel.date_acquisition,100) between @date_acquisition1 and @date_acquisition2) and archiver=@archiver");

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@date_acquisition1", DbType.String, 10, Convert.ToString(txtDateAcquisitionDebut.Text)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@date_acquisition2", DbType.String, 10, Convert.ToString(txtDateAcquisitionFin.Text)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = LoadRdlcReport.GetInstance().LoadReportWithSubReportSignataire(adapter, "DataSet1", "smartManage.Desktop.Reports.RptAmplificateur.rdlc", rpvReport);
                                #endregion
                            }
                            else
                                throw new CustomException("Sélection invalide, recommencez !!!");

                            if (dataset != null)
                                dataset.Dispose();
                            if (adapter != null)
                                adapter.Dispose();
                            if (conn != null)
                                conn.Close();
                        }
                        catch (InvalidOperationException ex)
                        {
                            MessageBox.Show(stringManager.GetString("StringFailedLoadReportMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadReportCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de chargement du rapport : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                            ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                        }
                    }
                    #endregion
                    break;
                case 5:
                    //*****Choix pour Retroprojecteur
                    #region Choix pour Retroprojecteur
                    using (IDbCommand cmd = conn.CreateCommand())
                    {
                        try
                        {
                            SqlDataAdapter adapter = null;
                            DataSet dataset = null;

                            RadioButton rd = radiobutton as RadioButton;

                            if (cboIndex == 0 && rd.Name.Equals(rdLstIdentifiant.Name))
                            {
                                #region Retroprojecteur par identifiant equipement
                                cmd.CommandText = Queries.GetInstance().CommonQueryMateriel("where  categorie_materiel.designation='Retroprojecteur' and materiel.code_str=@code_str and materiel.archiver=@archiver");

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@code_str", DbType.String, 10, cboIdentifiant.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = LoadRdlcReport.GetInstance().LoadReportWithSubReportSignataire(adapter, "DataSet1", "smartManage.Desktop.Reports.RptRetroprojecteur.rdlc", rpvReport);
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstEtat.Name))
                            {
                                #region Retroprojecteur par etat de l'equipement
                                cmd.CommandText = Queries.GetInstance().CommonQueryMateriel("where categorie_materiel.designation='Retroprojecteur' and etat_materiel.designation=@designation and archiver=@archiver");

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@designation", DbType.String, 50, cboEtat.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = LoadRdlcReport.GetInstance().LoadReportWithSubReportSignataire(adapter, "DataSet1", "smartManage.Desktop.Reports.RptRetroprojecteur.rdlc", rpvReport);
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstEndGarantie.Name))
                            {
                                #region Retroprojecteur par délais de garantie de l'equipement
                                cmd.CommandText = Queries.GetInstance().CommonQueryMateriel("where categorie_materiel.designation='Retroprojecteur' and garantie.valeur=@valeur and archiver=@archiver");

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@valeur", DbType.Int32, 4, Convert.ToInt32(cboDelais.SelectedValue, CultureInfo.CurrentCulture)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = LoadRdlcReport.GetInstance().LoadReportWithSubReportSignataire(adapter, "DataSet1", "smartManage.Desktop.Reports.RptRetroprojecteur.rdlc", rpvReport);
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstNetete.Name))
                            {
                                #region Retroprojecteur par netete de l'equipement
                                cmd.CommandText = Queries.GetInstance().CommonQueryMateriel("where categorie_materiel.designation='Retroprojecteur' and netette.designation=@netette and archiver=@archiver");

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@netette", DbType.String, 20, cboNetete.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = LoadRdlcReport.GetInstance().LoadReportWithSubReportSignataire(adapter, "DataSet1", "smartManage.Desktop.Reports.RptRetroprojecteur.rdlc", rpvReport);
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstDateAcquisition.Name))
                            {
                                #region Retroprojecteur par date d'acquisition de l'equipement
                                cmd.CommandText = Queries.GetInstance().CommonQueryMateriel("where categorie_materiel.designation='Retroprojecteur' and (convert(date,materiel.date_acquisition,100) between @date_acquisition1 and @date_acquisition2) and archiver=@archiver");

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@date_acquisition1", DbType.String, 10, Convert.ToString(txtDateAcquisitionDebut.Text)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@date_acquisition2", DbType.String, 10, Convert.ToString(txtDateAcquisitionFin.Text)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = LoadRdlcReport.GetInstance().LoadReportWithSubReportSignataire(adapter, "DataSet1", "smartManage.Desktop.Reports.RptRetroprojecteur.rdlc", rpvReport);
                                #endregion
                            }
                            else
                                throw new CustomException("Sélection invalide, recommencez !!!");

                            if (dataset != null)
                                dataset.Dispose();
                            if (adapter != null)
                                adapter.Dispose();
                            if (conn != null)
                                conn.Close();
                        }
                        catch (InvalidOperationException ex)
                        {
                            MessageBox.Show(stringManager.GetString("StringFailedLoadReportMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadReportCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de chargement du rapport : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                            ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                        }
                    }
                    #endregion
                    break;
                case 6:
                    //*****Choix pour Routeur_Access Point
                    #region Choix pour Routeur_Access Point
                    using (IDbCommand cmd = conn.CreateCommand())
                    {
                        try
                        {
                            SqlDataAdapter adapter = null;
                            DataSet dataset = null;

                            RadioButton rd = radiobutton as RadioButton;

                            if (cboIndex == 0 && rd.Name.Equals(rdLstIdentifiant.Name))
                            {
                                #region Routeur_Access Point par identifiant equipement
                                cmd.CommandText = Queries.GetInstance().CommonQueryMateriel("where  categorie_materiel.designation='Routeur_Access Point' and materiel.code_str=@code_str and materiel.archiver=@archiver");

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@code_str", DbType.String, 10, cboIdentifiant.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = LoadRdlcReport.GetInstance().LoadReportWithSubReportSignataire(adapter, "DataSet1", "smartManage.Desktop.Reports.RptRouteurAP.rdlc", rpvReport);
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstEtat.Name))
                            {
                                #region Routeur_Access Point par etat de l'equipement
                                cmd.CommandText = Queries.GetInstance().CommonQueryMateriel("where categorie_materiel.designation='Routeur_Access Point' and etat_materiel.designation=@designation and archiver=@archiver");

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@designation", DbType.String, 50, cboEtat.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = LoadRdlcReport.GetInstance().LoadReportWithSubReportSignataire(adapter, "DataSet1", "smartManage.Desktop.Reports.RptRouteurAP.rdlc", rpvReport);
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstEndGarantie.Name))
                            {
                                #region Routeur_Access Point par délais de garantie de l'equipement
                                cmd.CommandText = Queries.GetInstance().CommonQueryMateriel("where categorie_materiel.designation='Routeur_Access Point' and garantie.valeur=@valeur and archiver=@archiver");

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@valeur", DbType.Int32, 4, Convert.ToInt32(cboDelais.SelectedValue, CultureInfo.CurrentCulture)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = LoadRdlcReport.GetInstance().LoadReportWithSubReportSignataire(adapter, "DataSet1", "smartManage.Desktop.Reports.RptRouteurAP.rdlc", rpvReport);
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstMAC_Adresse.Name))
                            {
                                #region Routeur_Access Point par MAC l'equipement
                                cmd.CommandText = Queries.GetInstance().CommonQueryMateriel("where categorie_materiel.designation='Routeur_Access Point' and materiel.mac_adresse1 LIKE @mac_adresse1 and archiver=@archiver");

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@mac_adresse1", DbType.String, 20, cboMACAdresse.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = LoadRdlcReport.GetInstance().LoadReportWithSubReportSignataire(adapter, "DataSet1", "smartManage.Desktop.Reports.RptRouteurAP.rdlc", rpvReport);
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstDateAcquisition.Name))
                            {
                                #region Routeur_Access Point par date d'acquisition de l'equipement
                                cmd.CommandText = Queries.GetInstance().CommonQueryMateriel("where categorie_materiel.designation='Routeur_Access Point' and (convert(date,materiel.date_acquisition,100) between @date_acquisition1 and @date_acquisition2) and archiver=@archiver");

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@date_acquisition1", DbType.String, 10, Convert.ToString(txtDateAcquisitionDebut.Text)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@date_acquisition2", DbType.String, 10, Convert.ToString(txtDateAcquisitionFin.Text)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = LoadRdlcReport.GetInstance().LoadReportWithSubReportSignataire(adapter, "DataSet1", "smartManage.Desktop.Reports.RptRouteurAP.rdlc", rpvReport);
                                #endregion
                            }
                            else
                                throw new CustomException("Sélection invalide, recommencez !!!");

                            if (dataset != null)
                                dataset.Dispose();
                            if (adapter != null)
                                adapter.Dispose();
                            if (conn != null)
                                conn.Close();
                        }
                        catch (InvalidOperationException ex)
                        {
                            MessageBox.Show(stringManager.GetString("StringFailedLoadReportMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadReportCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de chargement du rapport : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                            ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                        }
                    }
                    #endregion
                    break;
                case 7:
                    //*****Choix pour Access Point
                    #region Choix pour Access Point
                    using (IDbCommand cmd = conn.CreateCommand())
                    {
                        try
                        {
                            SqlDataAdapter adapter = null;
                            DataSet dataset = null;

                            RadioButton rd = radiobutton as RadioButton;

                            if (cboIndex == 0 && rd.Name.Equals(rdLstIdentifiant.Name))
                            {
                                #region Access Point par identifiant equipement
                                cmd.CommandText = Queries.GetInstance().CommonQueryMateriel("where  categorie_materiel.designation='Access Point' and materiel.code_str=@code_str and materiel.archiver=@archive");

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@code_str", DbType.String, 10, cboIdentifiant.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = LoadRdlcReport.GetInstance().LoadReportWithSubReportSignataire(adapter, "DataSet1", "smartManage.Desktop.Reports.RptAccessPoint.rdlc", rpvReport);
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstEtat.Name))
                            {
                                #region Access Point par etat de l'equipement
                                cmd.CommandText = Queries.GetInstance().CommonQueryMateriel("where categorie_materiel.designation='Access Point' and etat_materiel.designation=@designation and archiver=@archiver");

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@designation", DbType.String, 50, cboEtat.SelectedValue));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = LoadRdlcReport.GetInstance().LoadReportWithSubReportSignataire(adapter, "DataSet1", "smartManage.Desktop.Reports.RptAccessPoint.rdlc", rpvReport);
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstEndGarantie.Name))
                            {
                                #region Access Point par délais de garantie de l'equipement
                                cmd.CommandText = Queries.GetInstance().CommonQueryMateriel("where categorie_materiel.designation='Access Point' and garantie.valeur=@valeur and archiver=@archiver");

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@valeur", DbType.Int32, 4, Convert.ToInt32(cboDelais.SelectedValue, CultureInfo.CurrentCulture)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = LoadRdlcReport.GetInstance().LoadReportWithSubReportSignataire(adapter, "DataSet1", "smartManage.Desktop.Reports.RptAccessPoint.rdlc", rpvReport);
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstPortee.Name))
                            {
                                #region Access Point par portee l'equipement
                                cmd.CommandText = Queries.GetInstance().CommonQueryMateriel("where categorie_materiel.designation='Access Point' and portee.valeur=@portee and archiver=@archiver");

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@portee", DbType.Int32, 4, Convert.ToInt32(cboPortee.SelectedValue, CultureInfo.InvariantCulture)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = LoadRdlcReport.GetInstance().LoadReportWithSubReportSignataire(adapter, "DataSet1", "smartManage.Desktop.Reports.RptAccessPoint.rdlc", rpvReport);
                                #endregion
                            }
                            else if (cboIndex == 0 && rd.Name.Equals(rdLstDateAcquisition.Name))
                            {
                                #region Access Point par date d'acquisition de l'equipement
                                cmd.CommandText = Queries.GetInstance().CommonQueryMateriel("where categorie_materiel.designation='Access Point' and (convert(date,materiel.date_acquisition,100) between @date_acquisition1 and @date_acquisition2) and archiver=@archiver");

                                SqlCommand sqlCmd = cmd as SqlCommand;
                                adapter = new SqlDataAdapter(sqlCmd);

                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@date_acquisition1", DbType.String, 10, Convert.ToString(txtDateAcquisitionDebut.Text)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@date_acquisition2", DbType.String, 10, Convert.ToString(txtDateAcquisitionFin.Text)));
                                cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                                dataset = LoadRdlcReport.GetInstance().LoadReportWithSubReportSignataire(adapter, "DataSet1", "smartManage.Desktop.Reports.RptAccessPoint.rdlc", rpvReport);
                                #endregion
                            }
                            else
                                throw new CustomException("Sélection invalide, recommencez !!!");

                            if (dataset != null)
                                dataset.Dispose();
                            if (adapter != null)
                                adapter.Dispose();
                            if (conn != null)
                                conn.Close();
                        }
                        catch (InvalidOperationException ex)
                        {
                            MessageBox.Show(stringManager.GetString("StringFailedLoadReportMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadReportCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de chargement du rapport : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                            ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                        }
                    }
                    #endregion
                    break;
            }
        }

        private void cmdView_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboCategorieMat.Items.Count > 0 && cboItems.Items.Count > 0)
                {
                    int categorie = cboCategorieMat.SelectedIndex;
                    int index = cboItems.SelectedIndex;

                    switch(categorie)
                    {
                        case 0:
                            //*****Choix pour Ordinateur
                            ValidateReport(categorie, index);
                            break;
                        case 1:
                            //*****Choix pour Switch
                            ValidateReport(categorie, index);
                            break;
                        case 2:
                            //*****Choix pour Imprimante
                            ValidateReport(categorie, index);
                            break;
                        case 3:
                            //*****Choix pour Emetteur
                            ValidateReport(categorie, index);
                            break;
                        case 4:
                            //*****Choix pour Amplificateur
                            ValidateReport(categorie, index);
                            break;
                        case 5:
                            //*****Choix pour Retroprojecteur
                            ValidateReport(categorie, index);
                            break;
                        case 6:
                            //*****Choix pour Routeur_Access Point
                            ValidateReport(categorie, index);
                            break;
                        case 7:
                            //*****Choix pour Access Point
                            ValidateReport(categorie, index);
                            break;
                    }
                }
                else
                    throw new CustomException("Impossible de charger les catégories de matériel ou la liste des items pour le rapport, veuillez réactualiser le formulaire");
            }
            catch (CustomException ex)
            {
                Properties.Settings.Default.StringLogFile = ex.Message;
                MessageBox.Show(Properties.Settings.Default.StringLogFile, stringManager.GetString("StringFailedLoadReportCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de chargement du rapport : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadReportMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadReportCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de chargement du rapport : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
        }

        private void ValidateReport(int categorie, int index)
        {
            if (rdLstIdentifiant.Checked)
            {
                if (!string.IsNullOrEmpty(cboIdentifiant.SelectedItem.ToString()))
                {
                    LoadReport(index, categorie, rdLstIdentifiant);
                }
                else
                    throw new CustomException("Veuillez vérifier que la liste déroulante des codes des matériels n'est pas vide");
            }
            else if (rdLstEtat.Checked)
            {
                if (!string.IsNullOrEmpty(cboEtat.Text))
                {
                    LoadReport(index, categorie, rdLstEtat);
                }
                else
                    throw new CustomException("Veuillez vérifier que la liste déroulante des état des matériels n'est pas vide");
            }
            else if (rdLstEndGarantie.Checked)
            {
                if (!string.IsNullOrEmpty(cboDelais.Text))
                {
                    LoadReport(index, categorie, rdLstEndGarantie);
                }
                else
                    throw new CustomException("Veuillez vérifier que la liste déroulante des délais n'est pas vide");
            }
            else if (rdLstMAC.Checked)
            {
                if (!string.IsNullOrEmpty(cboMacWifi.Text) || !string.IsNullOrEmpty(cboMacLAN.Text))
                {
                    LoadReport(index, categorie, rdLstMAC);
                }
                else
                    throw new CustomException("Veuillez vérifier que toutes les listes déroulantes des adresses MAC ne sont pas vides suivant la catégorie choisie");
            }
            else if (rdLstDateAcquisition.Checked)
            {
                if (!string.IsNullOrEmpty(txtDateAcquisitionDebut.Text) && !string.IsNullOrEmpty(txtDateAcquisitionFin.Text))
                {
                    LoadReport(index, categorie, rdLstDateAcquisition);
                }
                else
                    throw new CustomException("Veuillez vérifier que toutes les listes déroulantes ne sont pas vides");
            }
            else if (rdLstPortee.Checked)   
            {
                if (!string.IsNullOrEmpty(cboPortee.Text))
                {
                    LoadReport(index, categorie, rdLstPortee);
                }
                else
                    throw new CustomException("Veuillez vérifier que la liste déroulante des portées n'est pas vide");
            }
            else if (rdLstMAC_Adresse.Checked)
            {
                if (!string.IsNullOrEmpty(cboMACAdresse.Text))
                {
                    LoadReport(index, categorie, rdLstMAC_Adresse);
                }
                else
                    throw new CustomException("Veuillez vérifier que toutes la liste déroulante des adresses MAC n'est pas vide");
            }
            else if (rdLstFrequence.Checked)
            {
                if (!string.IsNullOrEmpty(cboFrequence.Text))
                {
                    LoadReport(index, categorie, rdLstFrequence);
                }
                else
                    throw new CustomException("Veuillez vérifier que la liste déroulante des fréquences n'est pas vide");
            }
            else if (rdLstNetete.Checked)
            {
                if (!string.IsNullOrEmpty(cboNetete.Text))
                {
                    LoadReport(index, categorie, rdLstNetete);
                }
                else
                    throw new CustomException("Veuillez vérifier que la liste déroulante des netété n'est pas vide");
            }
            else if (rdLstPPM.Checked)
            {
                if (!string.IsNullOrEmpty(cboPPM.Text))
                {
                    LoadReport(index, categorie, rdLstPPM);
                }
                else
                    throw new CustomException("Veuillez vérifier que la liste déroulante des pages par minute n'est pas vide");
            }
        }

        private void setMembersallcbo(ComboBox cbo, string displayMember, string valueMember)
        {
            cbo.DisplayMember = displayMember;
            cbo.ValueMember = valueMember;
        }

        private void rdLstEtat_CheckedChanged(object sender, EventArgs e)
        {
            if (cboCategorieMat.Items.Count > 0)
                ActivateControls(cboCategorieMat.SelectedIndex, rdLstEtat);
            else
            {
                Properties.Settings.Default.StringLogFile = "Veuillez d'abord choisir une catégorie de matériel valide !!!";
                MessageBox.Show(Properties.Settings.Default.StringLogFile, null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void rdLstEndGarantie_CheckedChanged(object sender, EventArgs e)
        {
            if (cboCategorieMat.Items.Count > 0)
                ActivateControls(cboCategorieMat.SelectedIndex, rdLstEndGarantie);
            else
            {
                Properties.Settings.Default.StringLogFile = "Veuillez d'abord choisir une catégorie de matériel valide !!!";
                MessageBox.Show(Properties.Settings.Default.StringLogFile, null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }            
        }

        private void rdLstMAC_CheckedChanged(object sender, EventArgs e)
        {
            if (cboCategorieMat.Items.Count > 0)
                ActivateControls(cboCategorieMat.SelectedIndex, rdLstMAC);
            else
            {
                Properties.Settings.Default.StringLogFile = "Veuillez d'abord choisir une catégorie de matériel valide !!!";
                MessageBox.Show(Properties.Settings.Default.StringLogFile, null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void frmReportOrdinateur_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void DesableItem()
        {
            cboIdentifiant.Enabled = false;
            cboEtat.Enabled = false;
            cboDelais.Enabled = false;
            cboMacWifi.Enabled = false;
            cboMacLAN.Enabled = false;
            txtDateAcquisitionDebut.Enabled = false;
            txtDateAcquisitionFin.Enabled = false;

            cboPortee.Enabled = false;
            cboFrequence.Enabled = false;
            cboNetete.Enabled = false;
            cboPPM.Enabled = false;
            cboMACAdresse.Enabled = false;

            chkArchiver.Checked = false;

            rdLstIdentifiant.Checked = false;
            rdLstEtat.Checked = false;
            rdLstEndGarantie.Checked = false;
            rdLstMAC.Checked = false;
            rdLstDateAcquisition.Checked = false;

            rdLstPortee.Checked = false;
            rdLstMAC_Adresse.Checked = false;
            rdLstFrequence.Checked = false;
            rdLstNetete.Checked = false;
            rdLstPPM.Checked = false;
        }

        private void frmReportOrdinateur_Load(object sender, EventArgs e)
        {
            
        }

        private void ActivateControls(int categorie, Control control)
        {
            //Comon Desable
            cboIdentifiant.Enabled = false;
            cboEtat.Enabled = false;
            cboDelais.Enabled = false;
            cboMacWifi.Enabled = false;
            cboMacLAN.Enabled = false;
            txtDateAcquisitionDebut.Enabled = false;
            txtDateAcquisitionFin.Enabled = false;

            cboPortee.Enabled = false;
            cboFrequence.Enabled = false;
            cboNetete.Enabled = false;
            cboPPM.Enabled = false;
            cboMACAdresse.Enabled = false;

            chkArchiver.Checked = false;

            switch (categorie)
            {
                case 0:
                    //*****Choix pour Ordinateur
                    ActivateControlOrdinateur(control);
                    break;
                case 1:
                    //*****Choix pour Switch
                    ActivateControlSwitch(control);
                    break;
                case 2:
                    //*****Choix pour Imprimante
                    ActivateControlImprimante(control);
                    break;
                case 3:
                    //*****Choix pour Emetteur
                    ActivateControlEmetteur(control);
                    break;
                case 4:
                    //*****Choix pour Amplificateur
                    ActivateControlAmplificateur(control);
                    break;
                case 5:
                    //*****Choix pour Retroprojecteur
                    ActivateControlRetroprojecteur(control);
                    break;
                case 6:
                    //*****Choix pour Routeur_Access Point
                    ActivateControlRouteurAP(control);
                    break;
                case 7:
                    //*****Choix pour Access Point
                    ActivateControlAP(control);
                    break;
            }
        }

        private void ActivateControlOrdinateur(Control control)
        {
            RadioButton rd = control as RadioButton;

            if (rd.Name.Equals(rdLstIdentifiant.Name))
            {
                //Activation pour Identifiant
                cboItems.Items.Clear();
                cboItems.Items.Add("Par identifiant équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboIdentifiant.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstEtat.Name))
            {
                //Activation pour Etat
                cboItems.Items.Clear();
                cboItems.Items.Add("Par etat de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboEtat.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstEndGarantie.Name))
            {
                //Activation pour Delais
                cboItems.Items.Clear();
                cboItems.Items.Add("Par délais de garantie de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboDelais.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstMAC.Name))
            {
                //Activation pour MAC adresse, Wifi ou LAN
                cboItems.Items.Clear();
                cboItems.Items.Add("Par MAC Adresse Wifi");
                cboItems.Items.Add("Par MAC Adresse LAN");
                cboItems.Items.Add("Par MAC Adresse Wifi ou LAN");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboMacWifi.Enabled = true;
                cboMacLAN.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstDateAcquisition.Name))
            {
                //Activation pour Date echeance
                cboItems.Items.Clear();
                cboItems.Items.Add("Par date aquisition de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                txtDateAcquisitionDebut.Enabled = true;
                txtDateAcquisitionFin.Enabled = true;
            }
        }

        private void ActivateControlSwitch(Control control)
        {
            RadioButton rd = control as RadioButton;

            if (rd.Name.Equals(rdLstIdentifiant.Name))
            {
                //Activation pour Identifiant
                cboItems.Items.Clear();
                cboItems.Items.Add("Par identifiant équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboIdentifiant.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstEtat.Name))
            {
                //Activation pour Etat
                cboItems.Items.Clear();
                cboItems.Items.Add("Par etat de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboEtat.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstEndGarantie.Name))
            {
                //Activation pour Delais
                cboItems.Items.Clear();
                cboItems.Items.Add("Par délais de garantie de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboDelais.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstMAC_Adresse.Name))
            {
                //Activation pour MAC adresse
                cboItems.Items.Clear();
                cboItems.Items.Add("Par MAC Adresse");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboMACAdresse.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstDateAcquisition.Name))
            {
                //Activation pour Date echeance
                cboItems.Items.Clear();
                cboItems.Items.Add("Par date aquisition de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                txtDateAcquisitionDebut.Enabled = true;
                txtDateAcquisitionFin.Enabled = true;
            }
        }

        private void ActivateControlImprimante(Control control)
        {
            RadioButton rd = control as RadioButton;

            if (rd.Name.Equals(rdLstIdentifiant.Name))
            {
                //Activation pour Identifiant
                cboItems.Items.Clear();
                cboItems.Items.Add("Par identifiant équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboIdentifiant.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstEtat.Name))
            {
                //Activation pour Etat
                cboItems.Items.Clear();
                cboItems.Items.Add("Par etat de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboEtat.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstEndGarantie.Name))
            {
                //Activation pour Delais
                cboItems.Items.Clear();
                cboItems.Items.Add("Par délais de garantie de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboDelais.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstPPM.Name))
            {
                //Activation pour Nombre des page par minute
                cboItems.Items.Clear();
                cboItems.Items.Add("Par nombre des pages par minute");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboPPM.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstDateAcquisition.Name))
            {
                //Activation pour Date echeance
                cboItems.Items.Clear();
                cboItems.Items.Add("Par date aquisition de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                txtDateAcquisitionDebut.Enabled = true;
                txtDateAcquisitionFin.Enabled = true;
            }
        }

        private void ActivateControlEmetteur(Control control)
        {
            RadioButton rd = control as RadioButton;

            if (rd.Name.Equals(rdLstIdentifiant.Name))
            {
                //Activation pour Identifiant
                cboItems.Items.Clear();
                cboItems.Items.Add("Par identifiant équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboIdentifiant.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstEtat.Name))
            {
                //Activation pour Etat
                cboItems.Items.Clear();
                cboItems.Items.Add("Par etat de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboEtat.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstEndGarantie.Name))
            {
                //Activation pour Delais
                cboItems.Items.Clear();
                cboItems.Items.Add("Par délais de garantie de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboDelais.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstFrequence.Name))
            {
                //Activation pour Frequence
                cboItems.Items.Clear();
                cboItems.Items.Add("Par Fréquence de fonctionnement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboFrequence.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstDateAcquisition.Name))
            {
                //Activation pour Date echeance
                cboItems.Items.Clear();
                cboItems.Items.Add("Par date aquisition de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                txtDateAcquisitionDebut.Enabled = true;
                txtDateAcquisitionFin.Enabled = true;
            }
        }

        private void ActivateControlAmplificateur(Control control)
        {
            RadioButton rd = control as RadioButton;

            if (rd.Name.Equals(rdLstIdentifiant.Name))
            {
                //Activation pour Identifiant
                cboItems.Items.Clear();
                cboItems.Items.Add("Par identifiant équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboIdentifiant.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstEtat.Name))
            {
                //Activation pour Etat
                cboItems.Items.Clear();
                cboItems.Items.Add("Par etat de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboEtat.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstEndGarantie.Name))
            {
                //Activation pour Delais
                cboItems.Items.Clear();
                cboItems.Items.Add("Par délais de garantie de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboDelais.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstDateAcquisition.Name))
            {
                //Activation pour Date echeance
                cboItems.Items.Clear();
                cboItems.Items.Add("Par date aquisition de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                txtDateAcquisitionDebut.Enabled = true;
                txtDateAcquisitionFin.Enabled = true;
            }
        }

        private void ActivateControlRetroprojecteur(Control control)
        {
            RadioButton rd = control as RadioButton;

            if (rd.Name.Equals(rdLstIdentifiant.Name))
            {
                //Activation pour Identifiant
                cboItems.Items.Clear();
                cboItems.Items.Add("Par identifiant équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboIdentifiant.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstEtat.Name))
            {
                //Activation pour Etat
                cboItems.Items.Clear();
                cboItems.Items.Add("Par etat de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboEtat.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstEndGarantie.Name))
            {
                //Activation pour Delais
                cboItems.Items.Clear();
                cboItems.Items.Add("Par délais de garantie de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboDelais.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstNetete.Name))
            {
                //Activation pour Netete
                cboItems.Items.Clear();
                cboItems.Items.Add("Par Netété de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboNetete.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstDateAcquisition.Name))
            {
                //Activation pour Date echeance
                cboItems.Items.Clear();
                cboItems.Items.Add("Par date aquisition de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                txtDateAcquisitionDebut.Enabled = true;
                txtDateAcquisitionFin.Enabled = true;
            }
        }

        private void ActivateControlRouteurAP(Control control)
        {
            RadioButton rd = control as RadioButton;

            if (rd.Name.Equals(rdLstIdentifiant.Name))
            {
                //Activation pour Identifiant
                cboItems.Items.Clear();
                cboItems.Items.Add("Par identifiant équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboIdentifiant.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstEtat.Name))
            {
                //Activation pour Etat
                cboItems.Items.Clear();
                cboItems.Items.Add("Par etat de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboEtat.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstEndGarantie.Name))
            {
                //Activation pour Delais
                cboItems.Items.Clear();
                cboItems.Items.Add("Par délais de garantie de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboDelais.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstMAC_Adresse.Name))
            {
                //Activation pour Mac Adresse
                cboItems.Items.Clear();
                cboItems.Items.Add("Par MAC Adresse de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboMACAdresse.Enabled = true;
            }
            //else if (rd.Name.Equals(rdLstPortee.Name))
            //{
            //    //Activation pour Portee
            //    cboItems.Items.Clear();
            //    cboItems.Items.Add("Par portée de l'équipement");
            //    cboItems.Sorted = false;
            //    cboItems.SelectedIndex = 0;

            //    cboPortee.Enabled = true;
            //}
            else if (rd.Name.Equals(rdLstDateAcquisition.Name))
            {
                //Activation pour Date echeance
                cboItems.Items.Clear();
                cboItems.Items.Add("Par date aquisition de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                txtDateAcquisitionDebut.Enabled = true;
                txtDateAcquisitionFin.Enabled = true;
            }
        }

        private void ActivateControlAP(Control control)
        {
            RadioButton rd = control as RadioButton;

            if (rd.Name.Equals(rdLstIdentifiant.Name))
            {
                //Activation pour Identifiant
                cboItems.Items.Clear();
                cboItems.Items.Add("Par identifiant équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboIdentifiant.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstEtat.Name))
            {
                //Activation pour Etat
                cboItems.Items.Clear();
                cboItems.Items.Add("Par etat de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboEtat.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstEndGarantie.Name))
            {
                //Activation pour Delais
                cboItems.Items.Clear();
                cboItems.Items.Add("Par délais de garantie de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboDelais.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstMAC_Adresse.Name))
            {
                //Activation pour Mac Adresse
                cboItems.Items.Clear();
                cboItems.Items.Add("Par MAC Adresse de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboMACAdresse.Enabled = true;
            }
            else if (rd.Name.Equals(rdLstPortee.Name))
            {
                //Activation pour Portee
                cboItems.Items.Clear();
                cboItems.Items.Add("Par portée de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                cboPortee.Enabled = true;
            }

            else if (rd.Name.Equals(rdLstDateAcquisition.Name))
            {
                //Activation pour Date echeance
                cboItems.Items.Clear();
                cboItems.Items.Add("Par date aquisition de l'équipement");
                cboItems.Sorted = false;
                cboItems.SelectedIndex = 0;

                txtDateAcquisitionDebut.Enabled = true;
                txtDateAcquisitionFin.Enabled = true;
            }
        }

        private void rdLstIdentifiant_CheckedChanged(object sender, EventArgs e)
        {
            if (cboCategorieMat.Items.Count > 0)
                ActivateControls(cboCategorieMat.SelectedIndex, rdLstIdentifiant);
            else
            {
                Properties.Settings.Default.StringLogFile = "Veuillez d'abord choisir une catégorie de matériel valide !!!";
                MessageBox.Show(Properties.Settings.Default.StringLogFile, null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void rdLstDateAcquisition_CheckedChanged(object sender, EventArgs e)
        {
            if (cboCategorieMat.Items.Count > 0)
                ActivateControls(cboCategorieMat.SelectedIndex, rdLstDateAcquisition);
            else
            {
                Properties.Settings.Default.StringLogFile = "Veuillez d'abord choisir une catégorie de matériel valide !!!";
                MessageBox.Show(Properties.Settings.Default.StringLogFile, null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void rdLstPortee_CheckedChanged(object sender, EventArgs e)
        {
            if (cboCategorieMat.Items.Count > 0)
                ActivateControls(cboCategorieMat.SelectedIndex, rdLstPortee);
            else
            {
                Properties.Settings.Default.StringLogFile = "Veuillez d'abord choisir une catégorie de matériel valide !!!";
                MessageBox.Show(Properties.Settings.Default.StringLogFile, null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void rdLstMAC_Adresse_CheckedChanged(object sender, EventArgs e)
        {
            if (cboCategorieMat.Items.Count > 0)
                ActivateControls(cboCategorieMat.SelectedIndex, rdLstMAC_Adresse);
            else
            {
                Properties.Settings.Default.StringLogFile = "Veuillez d'abord choisir une catégorie de matériel valide !!!";
                MessageBox.Show(Properties.Settings.Default.StringLogFile, null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void rdLstFrequence_CheckedChanged(object sender, EventArgs e)
        {
            if (cboCategorieMat.Items.Count > 0)
                ActivateControls(cboCategorieMat.SelectedIndex, rdLstFrequence);
            else
            {
                Properties.Settings.Default.StringLogFile = "Veuillez d'abord choisir une catégorie de matériel valide !!!";
                MessageBox.Show(Properties.Settings.Default.StringLogFile, null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void rdLstNetete_CheckedChanged(object sender, EventArgs e)
        {
            if (cboCategorieMat.Items.Count > 0)
                ActivateControls(cboCategorieMat.SelectedIndex, rdLstNetete);
            else
            {
                Properties.Settings.Default.StringLogFile = "Veuillez d'abord choisir une catégorie de matériel valide !!!";
                MessageBox.Show(Properties.Settings.Default.StringLogFile, null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void rdLstPPM_CheckedChanged(object sender, EventArgs e)
        {
            if (cboCategorieMat.Items.Count > 0)
                ActivateControls(cboCategorieMat.SelectedIndex, rdLstPPM);
            else
            {
                Properties.Settings.Default.StringLogFile = "Veuillez d'abord choisir une catégorie de matériel valide !!!";
                MessageBox.Show(Properties.Settings.Default.StringLogFile, null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void frmReportMateriel_Load(object sender, EventArgs e)
        {
            //Deseable allt controls to allow make choice
            DesableItem();

            try
            {
                if (tLoad == null)
                {
                    tLoad = new Thread(new ThreadStart(ExecuteLoadData));
                    tLoad.Start();
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadComboBoxMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadComboBoxCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur de chargement des listes déroulantes de gauche : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadComboBoxMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadComboBoxCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur de chargement des listes déroulantes de gauche : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }

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
            this.rpvReport.RefreshReport();
        }

        private void frmReportMateriel_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                this.UnloadThreadRessource(tLoad);
            }
            catch { }
        }
    }
}
