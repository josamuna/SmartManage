using ManageUtilities;
using System;
using System.Collections.Generic;
using smartManage.Model;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace smartManage.Desktop
{
    public partial class frmDataViewAdministration : Form, ICallMainForm, ICRUDGeneral
    {
        //Repertoire pour le Log
        private const string MasterDirectory = "SmartManage";
        //Nom du repertoire qui contiendra la chaine de connexion a la BD
        private const string DirectoryUtilConn = "ConnectionStringRaduis";
        //Nom du fichier qui contiendra la chaine de connexion connexion a la MySql pour Adminitration
        private const string FileRadAdmin = "UserRadAdmin.txt";

        clsConnexion connection = new clsConnexion();

        public frmDataViewAdministration()
        {
            InitializeComponent();
        }

        public frmPrincipal Principal
        {
            get;
            set;
        }

        #region Partie pour Login
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

                    paramServeur = ImplementUtilities.Instance.LoadDatabaseParameters(MasterDirectory, DirectoryUtilConn, FileRadAdmin, '\n', txtChipherKey.Text, true);

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
                        MessageBox.Show("Connexion réussie", "Connexion à la base de données", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Echec de l'authentification de l'utilisateur, {0}", ex.Message), "Connexion à la base de données", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
        }

        private void frmDataViewAdministration_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                clsMetier1.GetInstance().CloseConnection();
            }
            catch { }

            Principal.SetValuesLabel(Properties.Settings.Default.UserConnected, "Attente d'une action de l'utilisateur");
            Principal.ApplyDefaultStatusBar(Principal, Properties.Settings.Default.UserConnected);
        }

        public void New()
        {
            throw new NotImplementedException();
        }

        public void Search(string criteria)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void UpdateRec()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public void RefreshRec()
        {
            throw new NotImplementedException();
        }

        public void Preview()
        {
            MessageBox.Show("The Reports has not been set, please ask the Administrator", "Reports", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
