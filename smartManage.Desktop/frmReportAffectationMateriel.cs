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
    public partial class frmReportAffectationMateriel : Form
    {
        IDbConnection conn = null;
        private delegate void LoadSomeData(string ThreadName);
        private Thread tLoad = null;
        private ResourceManager stringManager = null;

        public frmReportAffectationMateriel()
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
            cboAC.DataSource = clsMetier.GetInstance().getAllClsAC();
            this.setMembersallcbo(cboAC, "Code_str", "Code_str");
            cboTypeLieuAffectation.DataSource = clsMetier.GetInstance().getAllClstype_lieu_affectation();
            this.setMembersallcbo(cboTypeLieuAffectation, "designation", "designation");
            cboLieuAffectation.DataSource = clsMetier.GetInstance().getAllClslieu_affectation();
            this.setMembersallcbo(cboLieuAffectation, "Designation", "Designation");
            cboCategorieMateriel.DataSource = clsMetier.GetInstance().getAllClscategorie_materiel1();
            this.setMembersallcbo(cboCategorieMateriel, "Designation", "Designation");

            List<ComboBox> lstCombo = new List<ComboBox>() { cboIdentifiant, cboAC, cboTypeLieuAffectation, cboLieuAffectation, cboCategorieMateriel };

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

        private void cmdView_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboCategorieItem.Items.Count > 0)
                {
                    int categorie = cboCategorieItem.SelectedIndex;
                    switch (cboCategorieItem.SelectedIndex)
                    {
                        case 0:
                            //***Par identifiant équipement
                            LoadReport(cboCategorieItem.SelectedIndex);
                            break;
                        case 1:
                            //***Par année académique
                            LoadReport(cboCategorieItem.SelectedIndex);
                            break;
                        case 2:
                            //***Par type de lieu d'affectation
                            LoadReport(cboCategorieItem.SelectedIndex);
                            break;
                        case 3:
                            //***Par lieu d'affectation
                            LoadReport(cboCategorieItem.SelectedIndex);
                            break;
                        case 4:
                            //***Par catégorie matériel
                            LoadReport(cboCategorieItem.SelectedIndex);
                            break;
                        case 5:
                            //***Par date d'affectation de matériel
                            LoadReport(cboCategorieItem.SelectedIndex);
                            break;
                    }
                }
                else
                    throw new CustomException("Impossible de charger les catégories d'item pour le rapport");
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

        private void setMembersallcbo(ComboBox cbo, string displayMember, string valueMember)
        {
            cbo.DisplayMember = displayMember;
            cbo.ValueMember = valueMember;
        }

        private void DesableItem()
        {
            cboIdentifiant.Enabled = false;
            cboAC.Enabled = false;
            cboTypeLieuAffectation.Enabled = false;
            cboLieuAffectation.Enabled = false;
            cboCategorieMateriel.Enabled = false;
            txtDateDebutAffectation.Enabled = false;
            txtDateFinAffectation.Enabled = false;
        }

        private void frmReportAffectationMateriel_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                this.UnloadThreadRessource(tLoad);
            }
            catch { }
        }

        private void frmReportAffectationMateriel_Load(object sender, EventArgs e)
        {
            //Deseable allt controls to allow make choice
            DesableItem();

            chkArchiver.Checked = false;

            cboCategorieItem.Items.Clear();
            cboCategorieItem.Items.Add("Par identifiant équipement");
            cboCategorieItem.Items.Add("Par année académique");
            cboCategorieItem.Items.Add("Par type de lieu d'affectation");
            cboCategorieItem.Items.Add("Par lieu d'affectation");
            cboCategorieItem.Items.Add("Par catégorie matériel");
            cboCategorieItem.Items.Add("Par date d'affectation de matériel");
            cboCategorieItem.Sorted = false;
            cboCategorieItem.SelectedIndex = 0;

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
        }

        private void cboCategorieItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboIdentifiant.Enabled = false;
            cboAC.Enabled = false;
            cboTypeLieuAffectation.Enabled = false;
            cboLieuAffectation.Enabled = false;
            cboCategorieMateriel.Enabled = false;
            txtDateDebutAffectation.Enabled = false;
            txtDateFinAffectation.Enabled = false;

            switch (cboCategorieItem.SelectedIndex)
            {
                case 0:
                    //***Par identifiant équipement
                    cboIdentifiant.Enabled = true;
                    break;
                case 1:
                    //***Par année académique
                    cboAC.Enabled = true;
                    break;
                case 2:
                    //***Par type de lieu d'affectation
                    cboTypeLieuAffectation.Enabled = true;
                    break;
                case 3:
                    //***Par lieu d'affectation
                    cboLieuAffectation.Enabled = true;
                    break;
                case 4:
                    //***Par catégorie matériel
                    cboCategorieMateriel.Enabled = true;
                    break;
                case 5:
                    //***Par date d'affectation de matériel
                    txtDateDebutAffectation.Enabled = true;
                    txtDateFinAffectation.Enabled = true;
                    break;
            }
        }

        private void LoadReport(int selectedIndex)
        {
            //Initialisation de la chaine de connexion
            conn = new SqlConnection(Model.Properties.Settings.Default.strChaineConnexion);

            if (conn.State == ConnectionState.Closed)
                conn.Open();

            switch (cboCategorieItem.SelectedIndex)
            {
                case 0:
                    //***Par identifiant équipement
                    #region Par identifiant équipement
                    using (IDbCommand cmd = conn.CreateCommand())
                    {
                        SqlDataAdapter adapter = null;
                        DataSet dataset = null;

                        cmd.CommandText = Queries.GetInstance().CommonQueryAffectationMateriel("where materiel.code_str=@code_str and materiel.archiver=@archiver");

                        SqlCommand sqlCmd = cmd as SqlCommand;
                        adapter = new SqlDataAdapter(sqlCmd);

                        cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@code_str", DbType.String, 10, cboIdentifiant.SelectedValue));
                        cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                        dataset = LoadRdlcReport.GetInstance().LoadReportWithSubReportSignataire(adapter, "DataSet1", "smartManage.Desktop.Reports.RptAffectationMateriel.rdlc", rpvReport);

                        if (dataset != null)
                            dataset.Dispose();
                        if (adapter != null)
                            adapter.Dispose();
                        if (conn != null)
                            conn.Close();
                    }
                    #endregion
                    break;
                case 1:
                    //***Par année académique
                    #region Par année académique
                    using (IDbCommand cmd = conn.CreateCommand())
                    {
                        SqlDataAdapter adapter = null;
                        DataSet dataset = null;

                        cmd.CommandText = Queries.GetInstance().CommonQueryAffectationMateriel("where AC.code_str=@code_str and materiel.archiver=@archiver");

                        SqlCommand sqlCmd = cmd as SqlCommand;
                        adapter = new SqlDataAdapter(sqlCmd);

                        cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@code_str", DbType.String, 50, cboAC.SelectedValue));
                        cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                        dataset = LoadRdlcReport.GetInstance().LoadReportWithSubReportSignataire(adapter, "DataSet1", "smartManage.Desktop.Reports.RptAffectationMateriel.rdlc", rpvReport);

                        if (dataset != null)
                            dataset.Dispose();
                        if (adapter != null)
                            adapter.Dispose();
                        if (conn != null)
                            conn.Close();
                    }
                    #endregion
                    break;
                case 2:
                    //***Par type de lieu d'affectation
                    #region Par type de lieu d'affectation
                    using (IDbCommand cmd = conn.CreateCommand())
                    {
                        SqlDataAdapter adapter = null;
                        DataSet dataset = null;

                        cmd.CommandText = Queries.GetInstance().CommonQueryAffectationMateriel("where type_lieu_affectation.designation=@designation and materiel.archiver=@archiver");

                        SqlCommand sqlCmd = cmd as SqlCommand;
                        adapter = new SqlDataAdapter(sqlCmd);

                        cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@designation", DbType.String, 50, cboTypeLieuAffectation.SelectedValue));
                        cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                        dataset = LoadRdlcReport.GetInstance().LoadReportWithSubReportSignataire(adapter, "DataSet1", "smartManage.Desktop.Reports.RptAffectationMateriel.rdlc", rpvReport);

                        if (dataset != null)
                            dataset.Dispose();
                        if (adapter != null)
                            adapter.Dispose();
                        if (conn != null)
                            conn.Close();
                    }
                    #endregion
                    break;
                case 3:
                    //***Par lieu d'affectation
                    #region Par lieu d'affectation
                    using (IDbCommand cmd = conn.CreateCommand())
                    {
                        SqlDataAdapter adapter = null;
                        DataSet dataset = null;

                        cmd.CommandText = Queries.GetInstance().CommonQueryAffectationMateriel("where lieu_affectation.designation=@designation and materiel.archiver=@archiver");

                        SqlCommand sqlCmd = cmd as SqlCommand;
                        adapter = new SqlDataAdapter(sqlCmd);

                        cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@designation", DbType.String, 50, cboLieuAffectation.SelectedValue));
                        cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                        dataset = LoadRdlcReport.GetInstance().LoadReportWithSubReportSignataire(adapter, "DataSet1", "smartManage.Desktop.Reports.RptAffectationMateriel.rdlc", rpvReport);

                        if (dataset != null)
                            dataset.Dispose();
                        if (adapter != null)
                            adapter.Dispose();
                        if (conn != null)
                            conn.Close();
                    }
                    #endregion
                    break;
                case 4:
                    //***Par catégorie matériel
                    #region Par catégorie matériel
                    using (IDbCommand cmd = conn.CreateCommand())
                    {
                        SqlDataAdapter adapter = null;
                        DataSet dataset = null;

                        cmd.CommandText = Queries.GetInstance().CommonQueryAffectationMateriel("where categorie_materiel.designation=@designation and materiel.archiver=@archiver");

                        SqlCommand sqlCmd = cmd as SqlCommand;
                        adapter = new SqlDataAdapter(sqlCmd);

                        cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@designation", DbType.String, 50, cboCategorieMateriel.SelectedValue));
                        cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                        dataset = LoadRdlcReport.GetInstance().LoadReportWithSubReportSignataire(adapter, "DataSet1", "smartManage.Desktop.Reports.RptAffectationMateriel.rdlc", rpvReport);

                        if (dataset != null)
                            dataset.Dispose();
                        if (adapter != null)
                            adapter.Dispose();
                        if (conn != null)
                            conn.Close();
                    }
                    #endregion
                    break;
                case 6:
                    //***Par date d'affectation de matériel
                    #region Par date d'affectation de matériel
                    using (IDbCommand cmd = conn.CreateCommand())
                    {
                        SqlDataAdapter adapter = null;
                        DataSet dataset = null;

                        cmd.CommandText = Queries.GetInstance().CommonQueryAffectationMateriel("where categorie_materiel.designation=@designation and materiel.archiver=@archiver");

                        SqlCommand sqlCmd = cmd as SqlCommand;
                        adapter = new SqlDataAdapter(sqlCmd);

                        cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@designation", DbType.String, 50, cboCategorieMateriel.SelectedValue));
                        cmd.Parameters.Add(clsMetier.GetInstance().getParameter(cmd, "@archiver", DbType.Boolean, 2, chkArchiver.Checked));

                        dataset = LoadRdlcReport.GetInstance().LoadReportWithSubReportSignataire(adapter, "DataSet1", "smartManage.Desktop.Reports.RptAffectationMateriel.rdlc", rpvReport);

                        if (dataset != null)
                            dataset.Dispose();
                        if (adapter != null)
                            adapter.Dispose();
                        if (conn != null)
                            conn.Close();
                    }
                    #endregion
                    break;
            }
        }
    }
}
