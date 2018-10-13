using smartManage.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace smartManage.Desktop
{
    public partial class frmReportOrdinateur : Form
    {
        IDbConnection conn = null;
        private delegate void LoadSomeData(string ThreadName);
        private Thread tLoad = null;

        public frmReportOrdinateur()
        {
            InitializeComponent();
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

            List<ComboBox> lstCombo = new List<ComboBox>() { cboIdentifiant, cboEtat, cboDelais, cboMacWifi, cboMacLAN };

            SetSelectedIndexComboBox(lstCombo);
        }

        private void ExecuteLoadData()
        {
            LoadSomeData loadDt = new LoadSomeData(DoExecuteLoadData);

            try
            {
                this.Invoke(loadDt, "tLoad");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Echec de chargement des listes déroulantes, " + ex.Message, "Chargement listes déroulantes", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        #endregion

        private string SetQueryExecute(ComboBox cboItems)
        {
            string query = null;

            switch (cboItems.SelectedIndex)
            {
                case 0:
                    if (rdLstIdentifiant.Checked)
                    {
                        //par identifiant equipement
                        query = string.Format(@"select tbl_fiche_pr.uuid as 'Identifiant unique',ISNULL(tbl_fiche_pr.nom,'') + '' + ISNULL(tbl_fiche_pr.post_nom,'') + ' ' + ISNULL(tbl_fiche_pr.prenom,'') AS 'Noms planteur',tbl_fiche_pr.association as 'Association',
                        tbl_fiche_pr.superficie as 'Hectares réalisé',tbl_fiche_pr.saison as 'Saison',tbl_fiche_pr.essence_principale as 'Essence principale',tbl_fiche_pr.essence_principale_autre as 'Autre essence',tbl_fiche_pr.ecartement_dim_1 as 'Ecartement 1',
                        tbl_fiche_pr.ecartement_dim_2 as 'Ecartement2',tbl_fiche_pr.regarnissage as 'Regarnissage',tbl_fiche_pr.entretien as 'Entretien',tbl_fiche_pr.etat as 'Etat plantation',tbl_fiche_pr.croissance_arbres as 'Croissance arbre',
                        tbl_fiche_pr.localisation as 'Coordonnées géographiques',tbl_fiche_pr.bailleur as 'Bailleur'
                        from tbl_fiche_pr 
                        where tbl_fiche_pr.saison='{0}'", cboIdentifiant.SelectedValue);
                    }
                    else if (rdLstEtat.Checked)
                    {
                        //par etat de l'equipement
                        query = string.Format(@"select tbl_fiche_pr.uuid as 'Identifiant unique',ISNULL(tbl_fiche_pr.nom,'') + '' + ISNULL(tbl_fiche_pr.post_nom,'') + ' ' + ISNULL(tbl_fiche_pr.prenom,'') AS 'Noms planteur',tbl_fiche_pr.association as 'Association',
                        tbl_fiche_pr.superficie as 'Hectares réalisé',tbl_fiche_pr.saison as 'Saison',tbl_fiche_pr.essence_principale as 'Essence principale',tbl_fiche_pr.essence_principale_autre as 'Autre essence',tbl_fiche_pr.ecartement_dim_1 as 'Ecartement 1',
                        tbl_fiche_pr.ecartement_dim_2 as 'Ecartement2',tbl_fiche_pr.regarnissage as 'Regarnissage',tbl_fiche_pr.entretien as 'Entretien',tbl_fiche_pr.etat as 'Etat plantation',tbl_fiche_pr.croissance_arbres as 'Croissance arbre',
                        tbl_fiche_pr.localisation as 'Coordonnées géographiques',tbl_fiche_pr.bailleur as 'Bailleur' 
                        from tbl_fiche_pr 
                        where tbl_fiche_pr.saison='{0}'", cboEtat.SelectedValue);
                    }
                    else if (rdLstEndGarantie.Checked)
                    {
                        //par délais de garantie de l'equipement
                        query = string.Format(@"select tbl_fiche_pr.uuid as 'Identifiant unique',ISNULL(tbl_fiche_pr.nom,'') + '' + ISNULL(tbl_fiche_pr.post_nom,'') + ' ' + ISNULL(tbl_fiche_pr.prenom,'') AS 'Noms planteur',tbl_fiche_pr.association as 'Association',tbl_fiche_pr.n_plantation as 'Nombre plantation',
                        tbl_fiche_pr.superficie as 'Hectares réalisé',tbl_fiche_pr.saison as 'Saison',tbl_fiche_pr.essence_principale as 'Essence principale',tbl_fiche_pr.ecartement_dim_1 as 'Ecartement 1',
                        tbl_fiche_pr.ecartement_dim_2 as 'Ecartement2',tbl_fiche_pr.regarnissage as 'Regarnissage',tbl_fiche_pr.entretien as 'Entretien',tbl_fiche_pr.etat as 'Etat plantation',tbl_fiche_pr.croissance_arbres as 'Croissance arbre',
                        tbl_fiche_pr.localisation as 'Coordonnées géographiques', (select n_visite as n_visite from tbl_fiche_pr union select n_visite_2 as n_visite from tbl_fiche_pr union select n_viste_3 as n_visite from tbl_fiche_pr) as n_visite, tbl_fiche_pr.nom_agent as 'Agent' 
                        from tbl_fiche_pr 
                        where (tbl_fiche_pr.n_visite='{0}'", cboDelais.SelectedValue);
                    }
                    else if (rdLstMAC.Checked)
                    {
                        //par MAC Wifi de l'equipement
                        query = string.Format(@"select tbl_fiche_pr.uuid as 'Identifiant unique',ISNULL(tbl_fiche_pr.nom,'') + '' + ISNULL(tbl_fiche_pr.post_nom,'') + ' ' + ISNULL(tbl_fiche_pr.prenom,'') AS 'Noms planteur',tbl_fiche_pr.association as 'Association',tbl_fiche_pr.n_plantation as 'Nombre plantation',
                        tbl_fiche_pr.superficie as 'Hectares réalisé',tbl_fiche_pr.saison as 'Saison',tbl_fiche_pr.essence_principale as 'Essence principale',tbl_fiche_pr.ecartement_dim_1 as 'Ecartement 1',
                        tbl_fiche_pr.ecartement_dim_2 as 'Ecartement2',tbl_fiche_pr.regarnissage as 'Regarnissage',tbl_fiche_pr.entretien as 'Entretien',tbl_fiche_pr.etat as 'Etat plantation',tbl_fiche_pr.croissance_arbres as 'Croissance arbre',
                        tbl_fiche_pr.localisation as 'Coordonnées géographiques', (select n_visite as n_visite from tbl_fiche_pr union select n_visite_2 as n_visite from tbl_fiche_pr union select n_viste_3 as n_visite from tbl_fiche_pr) as n_visite, tbl_fiche_pr.nom_agent as 'Agent' 
                        from tbl_fiche_pr 
                        where (tbl_fiche_pr.n_visite='{0}'", cboMacWifi.SelectedValue);
                    }

                    break;
                case 1:
                    if (rdLstMAC.Checked)
                    {
                        //par MAC LAN de l'equipement
                        query = string.Format(@"select tbl_fiche_pr.uuid as 'Identifiant unique',ISNULL(tbl_fiche_pr.nom,'') + '' + ISNULL(tbl_fiche_pr.post_nom,'') + ' ' + ISNULL(tbl_fiche_pr.prenom,'') AS 'Noms planteur',tbl_fiche_pr.association as 'Association',tbl_fiche_pr.n_plantation as 'Nombre plantation',
                        tbl_fiche_pr.superficie as 'Hectares réalisé',tbl_fiche_pr.saison as 'Saison',tbl_fiche_pr.essence_principale as 'Essence principale',tbl_fiche_pr.ecartement_dim_1 as 'Ecartement 1',
                        tbl_fiche_pr.ecartement_dim_2 as 'Ecartement2',tbl_fiche_pr.regarnissage as 'Regarnissage',tbl_fiche_pr.entretien as 'Entretien',tbl_fiche_pr.etat as 'Etat plantation',tbl_fiche_pr.croissance_arbres as 'Croissance arbre',
                        tbl_fiche_pr.localisation as 'Coordonnées géographiques', (select n_visite as n_visite from tbl_fiche_pr union select n_visite_2 as n_visite from tbl_fiche_pr union select n_viste_3 as n_visite from tbl_fiche_pr) as n_visite, tbl_fiche_pr.nom_agent as 'Agent' 
                        from tbl_fiche_pr 
                        where (tbl_fiche_pr.n_visite='{0}'", cboMacLAN.SelectedValue);
                    }

                    break;
                case 2:
                    if (rdLstMAC.Checked)
                    {
                        //par MAC Wifi ou LAN de l'equipement
                        query = string.Format(@"select tbl_fiche_pr.uuid as 'Identifiant unique',ISNULL(tbl_fiche_pr.nom,'') + '' + ISNULL(tbl_fiche_pr.post_nom,'') + ' ' + ISNULL(tbl_fiche_pr.prenom,'') AS 'Noms planteur',tbl_fiche_pr.association as 'Association',tbl_fiche_pr.n_plantation as 'Nombre plantation',
                            tbl_fiche_pr.superficie as 'Hectares réalisé',tbl_fiche_pr.saison as 'Saison',tbl_fiche_pr.essence_principale as 'Essence principale',tbl_fiche_pr.ecartement_dim_1 as 'Ecartement 1',
                            tbl_fiche_pr.ecartement_dim_2 as 'Ecartement2',tbl_fiche_pr.regarnissage as 'Regarnissage',tbl_fiche_pr.entretien as 'Entretien',tbl_fiche_pr.etat as 'Etat plantation',tbl_fiche_pr.croissance_arbres as 'Croissance arbre',
                            tbl_fiche_pr.localisation as 'Coordonnées géographiques', (select n_visite as n_visite from tbl_fiche_pr union select n_visite_2 as n_visite from tbl_fiche_pr union select n_viste_3 as n_visite from tbl_fiche_pr) as n_visite, tbl_fiche_pr.nom_agent as 'Agent' 
                            from tbl_fiche_pr 
                            where (tbl_fiche_pr.n_visite='{0}' or tbl_fiche_pr.n_visite_2='{1}'", cboMacWifi.SelectedValue, cboMacLAN.SelectedValue);
                    }

                    break;
            }
            return query;
        }

        private void LoadReport(string query, int cboIndex)
        {
            //Initialisation de la chaine de connexion
            conn = new SqlConnection(smartManage.Model.Properties.Settings.Default.strChaineConnexion);

            if (conn.State == ConnectionState.Closed)
                conn.Open();

            using (IDbCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = query;
                IDbDataAdapter adapter = new SqlDataAdapter((SqlCommand)cmd);
                DataSet dataset = new DataSet();
                adapter.Fill(dataset);

                switch (cboIndex)
                {
                    case 0:
                        if (rdLstIdentifiant.Checked)
                        {
                            //par identifiant equipement
                            Reports.LstOrdinateurIdentifiant rpt = new Reports.LstOrdinateurIdentifiant();
                            rpt.SetDataSource(dataset.Tables["lstTable"]);
                            crvReport.ReportSource = rpt;
                            crvReport.Refresh();
                            dataset.Dispose();
                        }
                        else if (rdLstEtat.Checked)
                        {
                            //par etat de l'equipement
                            Reports.LstOrdinateurIdentifiant rpt = new Reports.LstOrdinateurIdentifiant();
                            rpt.SetDataSource(dataset.Tables["lstTable"]);
                            crvReport.ReportSource = rpt;
                            crvReport.Refresh();
                            dataset.Dispose();
                        }
                        else if (rdLstEndGarantie.Checked)
                        {
                            //par délais de garantie de l'equipement
                            Reports.LstOrdinateurIdentifiant rpt = new Reports.LstOrdinateurIdentifiant();
                            rpt.SetDataSource(dataset.Tables["lstTable"]);
                            crvReport.ReportSource = rpt;
                            crvReport.Refresh();
                            dataset.Dispose(); ;
                        }
                        else if (rdLstMAC.Checked)
                        {
                            //par MAC Wifi de l'equipement
                            Reports.LstOrdinateurIdentifiant rpt = new Reports.LstOrdinateurIdentifiant();
                            rpt.SetDataSource(dataset.Tables["lstTable"]);
                            crvReport.ReportSource = rpt;
                            crvReport.Refresh();
                            dataset.Dispose();
                        }
                        else if (rdLstDateAcquisition.Checked)
                        {
                            //par date d'acuisition de l'equipement
                            Reports.LstOrdinateurIdentifiant rpt = new Reports.LstOrdinateurIdentifiant();
                            rpt.SetDataSource(dataset.Tables["lstTable"]);
                            crvReport.ReportSource = rpt;
                            crvReport.Refresh();
                            dataset.Dispose(); ;
                        }

                        break;

                    case 1:
                        if (rdLstMAC.Checked)
                        {
                            //par MAC LAN de l'equipement
                            Reports.LstOrdinateurIdentifiant rpt = new Reports.LstOrdinateurIdentifiant();
                            rpt.SetDataSource(dataset.Tables["lstTable"]);
                            crvReport.ReportSource = rpt;
                            crvReport.Refresh();
                            dataset.Dispose();
                        }

                        break;

                    case 2:
                        if (rdLstMAC.Checked)
                        {
                            //par MAC Wifi ou LAN de l'equipement
                            Reports.LstOrdinateurIdentifiant rpt = new Reports.LstOrdinateurIdentifiant();
                            rpt.SetDataSource(dataset.Tables["lstTable"]);
                            crvReport.ReportSource = rpt;
                            crvReport.Refresh();
                            dataset.Dispose();
                        }

                        break; 
                }
            }
        }

        private void cmdView_Click(object sender, EventArgs e)
        {
            try
            {
                LoadReport(SetQueryExecute(cboItems), cboItems.SelectedIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Echec de chargement du rapport, " + ex.Message, "Chargement rapport", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                conn.Close();
            }
        }

        private void setMembersallcbo(ComboBox cbo, string displayMember, string valueMember)
        {
            cbo.DisplayMember = displayMember;
            cbo.ValueMember = valueMember;
        }

        private void rdLstEtat_CheckedChanged(object sender, EventArgs e)
        {
            cboItems.Items.Clear();
            cboItems.Items.Add("Par etat de l'équipement");
            cboItems.Sorted = false;
            cboItems.SelectedIndex = 0;

            cboEtat.Enabled = true;
            cboIdentifiant.Enabled = false;
            cboDelais.Enabled = false;
            cboMacWifi.Enabled = false;
            cboMacLAN.Enabled = false;
            txtDateAcquisitionDebut.Enabled = false;
            txtDateAcquisitionFin.Enabled = false;
        }

        private void rdLstEndGarantie_CheckedChanged(object sender, EventArgs e)
        {
            cboItems.Items.Clear();
            cboItems.Items.Add("Par délais de garantie de l'équipement");
            cboItems.Sorted = false;
            cboItems.SelectedIndex = 0;

            cboIdentifiant.Enabled = false;
            cboEtat.Enabled = false;
            cboDelais.Enabled = true;
            cboMacWifi.Enabled = false;
            cboMacLAN.Enabled = false;
            txtDateAcquisitionDebut.Enabled = false;
            txtDateAcquisitionFin.Enabled = false;
        }

        private void rdLstMAC_CheckedChanged(object sender, EventArgs e)
        {
            cboItems.Items.Clear();
            cboItems.Items.Add("Par MAC Adresse Wifi");
            cboItems.Items.Add("Par MAC Adresse LAN");
            cboItems.Items.Add("Par MAC Adresse Wifi ou LAN");
            cboItems.Sorted = false;
            cboItems.SelectedIndex = 0;

            cboIdentifiant.Enabled = false;
            cboEtat.Enabled = false;
            cboDelais.Enabled = false;
            cboMacWifi.Enabled = true;
            cboMacLAN.Enabled = true;
            txtDateAcquisitionDebut.Enabled = false;
            txtDateAcquisitionFin.Enabled = false;
        }

        private void frmReportOrdinateur_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                this.UnloadThreadRessource(tLoad);
            }
            catch { }
        }

        private void frmReportOrdinateur_Load(object sender, EventArgs e)
        {
            try
            {
                if (tLoad == null)
                {
                    tLoad = new Thread(new ThreadStart(ExecuteLoadData));
                    tLoad.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Erreur lor du chargement des listes déroulantes, {0}", ex.Message), "Chargement listes déroulantes", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            try
            {
                smartManage.Tools.clsTools.SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
            }
            catch { }
        }

        private void rdLstIdentifiant_CheckedChanged(object sender, EventArgs e)
        {
            cboItems.Items.Clear();
            cboItems.Items.Add("Par identifiant équipement");
            cboItems.Sorted = false;
            cboItems.SelectedIndex = 0;

            cboIdentifiant.Enabled = true;
            cboEtat.Enabled = false;
            cboDelais.Enabled = false;
            cboMacWifi.Enabled = false;
            cboMacLAN.Enabled = false;
            txtDateAcquisitionDebut.Enabled = false;
            txtDateAcquisitionFin.Enabled = false;
        }

        private void rdLstDateAcquisition_CheckedChanged(object sender, EventArgs e)
        {
            cboItems.Items.Clear();
            cboItems.Items.Add("Par date aquisition de l'équipement");
            cboItems.Sorted = false;
            cboItems.SelectedIndex = 0;

            cboIdentifiant.Enabled = false;
            cboEtat.Enabled = false;
            cboDelais.Enabled = false;
            cboMacWifi.Enabled = false;
            cboMacLAN.Enabled = false;
            txtDateAcquisitionDebut.Enabled = true;
            txtDateAcquisitionFin.Enabled = true;
        }
    }
}
