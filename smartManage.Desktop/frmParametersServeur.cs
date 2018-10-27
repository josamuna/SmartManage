using ManageUtilities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace smartManage.Desktop
{
    public partial class frmParametersServeur : Form, ICRUDGeneral, ICallMainForm
    {
        //Repertoire pour le Log
        private const string MasterDirectory = "SmartManage";
        //Nom du repertoire qui contiendra la chaine de connexion a la BD
        private const string DirectoryUtilConn = "ConnectionStringRaduis";
        //Nom du fichier qui contiendra la chaine de connexion connexion a la MySql pour Adminitration
        private const string FileRadAdmin = "UserRadAdmin.txt";
        //Nom du fichier qui contiendra la chaine de connexion connexion a la MySql pour Etudiant
        private const string FileRadEtu = "UserRadEtu.txt";

        public frmParametersServeur()
        {
            InitializeComponent();
        }

        public frmPrincipal Principal
        {
            get;
            set;
        }

        private void ClearText()
        {
            txtHost.Clear();
            txtDatabase.Clear();
            txtUser.Clear();
            //txtPwd.Clear();
            txtChipherKey.Clear();
            txtHost.Focus();
        }

        private void frmParametersServeur_Load(object sender, EventArgs e)
        {
            cboServerType.DataSource = Enum.GetNames(typeof(TypeServeur));
            txtHost.Focus();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmParametersServeur_Activated(object sender, EventArgs e)
        {
            Principal.SetValuesLabel(Properties.Settings.Default.UserConnected, "Paramétrage des serveurs Radius");
            ClearText();
            Principal.SetCurrentICRUDChildForm(this);
        }

        private void frmParametersServeur_FormClosed(object sender, FormClosedEventArgs e)
        {
            Principal.SetValuesLabel(Properties.Settings.Default.UserConnected, "Attente d'une action de l'utilisateur");
            Principal.ApplyDefaultStatusBar(Principal, Properties.Settings.Default.UserConnected);
        }

        private void cmdLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtChipherKey.Text))
                {
                    ClearText();
                    throw new Exception("Veuillez spécifier la clé de chiffrement svp !!!");
                }
                else
                {
                    List<string> paramServeur = new List<string>();

                    if (cboServerType.SelectedValue.ToString().Equals(TypeServeur.Administration.ToString()))
                        paramServeur = ImplementUtilities.Instance.LoadDatabaseParameters(MasterDirectory, DirectoryUtilConn, FileRadAdmin, '\n', txtChipherKey.Text, true);
                    else
                        paramServeur = ImplementUtilities.Instance.LoadDatabaseParameters(MasterDirectory, DirectoryUtilConn, FileRadEtu, '\n', txtChipherKey.Text, true);

                    if (paramServeur.Count > 0)
                    {
                        string key = txtChipherKey.Text;

                        txtHost.Text = paramServeur[0];
                        txtDatabase.Text = paramServeur[1];
                        txtUser.Text = paramServeur[2];
                        //txtPwd.Text = paramServeur[3];
                    }
                    else
                        ClearText();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(string.Format("Echec de chargement des données, {0}", ex.Message), "Chargement des données", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {
                string host = ImplementChiffer.Instance.Cipher(txtHost.Text, txtChipherKey.Text);
                string dataBase = ImplementChiffer.Instance.Cipher(txtDatabase.Text, txtChipherKey.Text);
                string user = ImplementChiffer.Instance.Cipher(txtUser.Text, txtChipherKey.Text);
                //string cipherPwd = ImplementChiffer.Instance.Cipher(txtPwd.Text, txtChipherKey.Text);

                if (cboServerType.SelectedValue.ToString().Equals(TypeServeur.Administration.ToString()))
                    ImplementUtilities.Instance.SaveParameters(MasterDirectory, string.Format("Serveur={0}\nDataBase={1}\nUser={2}", host, dataBase, user), DirectoryUtilConn, FileRadAdmin);
                else
                    ImplementUtilities.Instance.SaveParameters(MasterDirectory, string.Format("Serveur={0}\nDataBase={1}\nUser={2}", host, dataBase, user), DirectoryUtilConn, FileRadEtu);

                MessageBox.Show("Enregistrement effectué avec succès", "Enregistrement des données", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Echec de l'enregistrement des données, {0}", ex.Message), "Enregistrement des données", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public void New()
        {
        }

        public void Search(string criteria)
        {
        }

        public void Save()
        {
        }

        public void UpdateRec()
        {
        }

        public void Delete()
        {
        }

        public void RefreshRec()
        {
        }

        public void Preview()
        {
        }
    }
}
