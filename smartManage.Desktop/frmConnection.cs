using ManageUtilities;
using smartManage.Model;
using smartManage.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace smartManage.Desktop
{
    public partial class frmConnection : Form, ICallMainForm
    {
        clsConnexion connection = new clsConnexion();

        //Repertoire pour le Log
        private const string MasterDirectory = "SmartManage";
        //Nom du repertoire qui contiendra la chaine de connexion a la BD
        private const string DirectoryUtilConn = "ConnectionString";
        //Nom du fichier qui contiendra la chaine de connexion connexion a la BD SQLServer
        private const string FileSQLServer = "UserSQLSever.txt";
        //Repertoire pour le Log
        const string DirectoryUtilLog = "Log";

        public static string bdEnCours = "";

        public frmPrincipal Principal
        {
            get;
            set;
        }
        
        private void LoadValues()
        {
            clsTools.valueUser.Clear();

            //Chardement des parametres de connexion 
            //Ici si le fichier est vide ou qu'il n'existe pas,on charge les paramètres par défaut
            List<string> lstValues = ImplementUtilities.Instance.LoadDatabaseParameters(MasterDirectory, DirectoryUtilConn, FileSQLServer, '\n');

            if (lstValues.Count > 0)
            {
                connection.Serveur = lstValues[0];//Nom du serveur
                connection.DB = lstValues[1];//Nom de la Base de Donnees
                //connection.User = lstValues[2];//User de la Base de Donnees
                connection.User = txtNomUser.Text;
                connection.Pwd = txtPwd.Text;

                //Iniialisation de la chaine de connexion
                clsMetier.GetInstance().Initialize(connection, 1);
            }
            else
            {
                //Si le fichier des parametres de la BD ne contient rien on y mets des  parametres par defaut qu'on pourra modifier after
                //Le nom correct du serveur doit être change s'il arrive que vous devez utiliser une autre machine
                connection.Serveur = @"JOSAM\SQLEXPRESS";
                connection.DB = "gestion_labo_DB";
                connection.User = txtNomUser.Text;
                connection.Pwd = txtPwd.Text;
                //connection.User = "sa";

                //Iniialisation de la chaine de connexion
                clsMetier.GetInstance().Initialize(connection, 1);
            }
        }
        public frmConnection()
        {
            InitializeComponent();
            ImplementUtilities.Instance.MasterDirectoryConfiguration = MasterDirectory;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Principal.SetValuesLabel(Properties.Settings.Default.UserConnected, "Attente d'une action de l'utilisateur");
            this.Close();
        }

        private void VerifieUser()
        {
            if (clsMetier.GetInstance().isConnect())
            {
                ArrayList lstValues = clsMetier.GetInstance().verifieLoginUser(connection.User, connection.Pwd);

                if (Convert.ToBoolean(lstValues[3]))
                {
                    //Ajout des droits du user dans la variable
                    foreach (string str in lstValues[2].ToString().Split(','))
                        clsTools.valueUser.Add(str);
                }

                if (Convert.ToBoolean(lstValues[3]))
                {
                    MessageBox.Show("Connexion réussie", "Connexion à la base de données", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //Enregistrement des parametres de connexion
                    ImplementUtilities.Instance.SaveParameters(MasterDirectory,
                        string.Format("Serveur={0}\nDataBase={1}\nUserBD={2}\nPassword={3}", connection.Serveur, connection.DB, string.Empty, string.Empty),
                        DirectoryUtilConn, FileSQLServer);
                }
                else
                {
                    MessageBox.Show("Echec de l'authentification de l'utilisateur", "Authentification de l'utilisateur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtNomUser.Clear();
                    txtPwd.Clear();
                    txtNomUser.Focus();
                }
            }
        }

        private void btnxConn_Click(object sender, EventArgs e)
        {
            try
            {
                //Appel de l'initialisation de la chaine de connexion avant de l'ouvrir
                LoadValues();

                //Execution des verification de l'utilisateur
                VerifieUser();

                //Recupération bd connectée
                try
                {
                    clsMetier.bdEnCours = clsMetier.GetInstance().getCurrentDataBase();
                }
                catch (Exception) { }

                //Activation/Desactivation items des menus
                Properties.Settings.Default.UserConnected = connection.User;
                Principal.SetValuesLabel(Properties.Settings.Default.UserConnected, "Utilisateur authentifié avec succès");

                Principal.LockMenu(true, clsTools.valueUser[0]);
                this.Cursor = Cursors.Default;
                this.Close();
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;

                MessageBox.Show("Echec de l'ouverture de la connexion à la Base de données, " + ex.Message, "connexion à la Base de données", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //On garde chaque fois une trace de l'erreur generee
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de l'ouverture de la connexion à la Base de données : " + ex.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
            }
        }

        private void txtnomserveur_Enter(object sender, EventArgs e)
        {
            ((Control)sender).BackColor = Color.Khaki;
        }

        private void txtnomserveur_Leave(object sender, EventArgs e)
        {
            ((Control)sender).BackColor = Color.White;
        }

        private void txtpwd_TextChanged(object sender, EventArgs e)
        {
            btnxConn.Enabled = true;
        }

        private void txtpwd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnxConn_Click(sender, e);
            }
        }

        private void frmxConn_Load(object sender, EventArgs e)
        {
            txtNomUser.Focus();
        }

        private void frmConnection_Activated(object sender, EventArgs e)
        {
            Principal.SetValuesLabel(Properties.Settings.Default.UserConnected, "Authentification de l'utilisateur");
        }
    }
}
