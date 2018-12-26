using ManageUtilities;
using smartManage.Model;
using smartManage.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace smartManage.Desktop
{
    public partial class frmUtilisateur : Form
    {
        ResourceManager stringManager = null;
        //Instance du formulaire principal
        public frmPrincipal Principal
        {
            get; set;
        }

        public frmUtilisateur()
        {
            InitializeComponent();
            //Initialisation des Resources
            Assembly _assembly = Assembly.Load("ResourcesData");
            stringManager = new ResourceManager("ResourcesData.Resource", _assembly);
        }

        #region Instance du frmPrincipal// ++++++++++++++++++++++++++++++++++++++
        // ++++++++++++++++++++++++++++++++++++++
        //private frmPrincipal mainform = new frmPrincipal();

        public frmPrincipal Mainform
        {
            get;
            set;
        }

        // ++++++++++++++++++++++++++++++++++++++++
        // ++++++++++++++++++++++++++++++++++++++++

        #endregion

        clsutilisateur utilisateur = new clsutilisateur();

        private BindingSource bdsrcCreate = new BindingSource();
        private BindingSource bdsrcDelete = new BindingSource();
        private BindingSource bdsrcAffiche = new BindingSource();
        private BindingSource bdsrcModifie = new BindingSource();
        private BindingSource bdsrcDroit = new BindingSource();

        private int id_utilisateur = 0, identifiant_user = 0;
        private bool okDoubleClicDgv = false;
        private bool bln1 = false;

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

        private void BindingLstCreate()
        {
            //SetBindingControls(txtNomUser, "Text", bdsrcCreate, "Nomuser");
            //SetBindingControls(txtMotPasse, "Text", bdsrcCreate, "Motpass");
            SetBindingControls(cboAgent, "SelectedValue", bdsrcCreate, "Id_personne");
            //SetBindingControls(chkActivationUser, "Checked", bdsrcCreate, "Activation");
        }

        private void BindingClsCreate()
        {
            SetBindingControls(txtNomUser, "Text", utilisateur, "Nomuser");
            SetBindingControls(txtMotPasse, "Text", utilisateur, "Motpass");
            SetBindingControls(cboAgent, "SelectedValue", utilisateur, "Id_personne");
            SetBindingControls(chkActivationUser, "Checked", utilisateur, "Activation");
        }

        private void New()
        {
            utilisateur = new clsutilisateur();
            bln1 = false;

            BindingClsCreate();
            if (cboAgent.Items.Count > 0) cboAgent.SelectedIndex = 0;
            utilisateur.Activation = chkActivationUser.Checked;
        }

        private void RefreshData()
        {
            List<clspersonne> lst = new List<clspersonne>();
            lst = clsMetier.GetInstance().getAllClspersonne();
            cboAgent.DataSource = lst;
            setMembersallcbo(cboAgent, "NomComplet", "Id");

            DataTable userListAgent = clsMetier.GetInstance().getAllClsutilisateur_Agent();
            cboUtilisateur.DataSource = userListAgent;
            setMembersallcbo(cboUtilisateur, "Nomuser", "Id");
            cboUtilisateur1.DataSource = userListAgent;
            setMembersallcbo(cboUtilisateur1, "Nomuser", "Id");
            CboUserSup.DataSource = userListAgent;
            setMembersallcbo(CboUserSup, "Nomuser", "Id");

            bdsrcAffiche.DataSource = userListAgent;
            bdsrcCreate.DataSource = userListAgent;
            bdsrcDelete.DataSource = userListAgent;
            bdsrcModifie.DataSource = userListAgent;

            chkLevelAdminstrateur.Checked = false;
            chkLevelAdmin.Checked = false;
            chkLevelUser.Checked = false;
        }

        private void RefreshDroits()
        {
            bdsrcDroit.DataSource = clsMetier.GetInstance().getAllClsutilisateur_Agent2(identifiant_user);
            dgv1.DataSource = bdsrcDroit;

            if (chkLevelAdminstrateur.Checked)
                chkLevelAdminstrateur.Checked = false;
            else if (chkLevelAdmin.Checked)
                chkLevelAdmin.Checked = false;
            else if (chkLevelUser.Checked)
                chkLevelUser.Checked = false;
            RefreshData();
        }

        private void FrmUtilisateur_Load(object sender, EventArgs e)
        {
            rdUserSeul.Checked = true;
            txtNewMotPasse.Enabled = false;
            cmdAfficherDroit.Enabled = false;
            cmdAccorderDroit.Enabled = false;
            cmdRetirerDroit.Enabled = false;
            activate_desactivetModifieUser(false);

            try
            {
                BindingLstCreate();
                RefreshData();
                dgv.DataSource = bdsrcAffiche;
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

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            Principal.SetValuesLabel(Properties.Settings.Default.UserConnected, "Attente d'une action de l'utilisateur");
            this.Close();
        }

        private void tabManage_Selecting(object sender, TabControlCancelEventArgs e)
        {
            //try
            //{
                //Se produit lorsque l'on change un onglet
                if (tabManage.SelectedIndex == 0) { }
                else if (tabManage.SelectedIndex == 1)
                {
                    activate_desactivetModifieUser(false);

                    if (bdsrcDelete.Count != 0) cmdDelete.Enabled = true;
                    if (!okDoubleClicDgv)
                    {
                        txtOldMotPasse.Clear();
                        txtNewMotPasse.Clear();
                        txtNewUser.Focus();
                    }
                }
            //}
            //catch { }
        }

        private void txtSeach_TextChanged(object sender, EventArgs e)
        {
            try
            {
                bdsrcAffiche.Filter = "Nomuser LIKE '%" + txtSeach.Text + "%' OR nom LIKE '%" + txtSeach.Text + "%'";
            }
            catch (ArgumentException ex)
            {
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de recherche dans un TextBox avec DataTable : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
        }

        private void dgv_DoubleClick(object sender, EventArgs e)
        {
            okDoubleClicDgv = true;
            if (dgv.SelectedRows.Count > 0)
            {
                tabMainManage.SelectedIndex = 2;
                //On met les valeurs correspondantes à l'Agent (Utilisateur) Selectionné
                try
                {
                    DataTable dtUserAgent = clsMetier.GetInstance().getAllClsutilisateur_Agent2(id_utilisateur);
                    bdsrcModifie.DataSource = dtUserAgent;

                    //Modification
                    if (bdsrcModifie.Count != 0)
                    {
                        //clsutilisateur s = new clsutilisateur();
                        DataRowView user = (DataRowView)bdsrcModifie.Current;

                        utilisateur.Nomuser = cboUtilisateur.Text = Convert.ToString(user["nomuser"], CultureInfo.InvariantCulture);
                        txtOldMotPasse.Text = ImplementChiffer.Instance.Decipher(Convert.ToString(user["motpass"], CultureInfo.InvariantCulture), "Jos@mRootP@ss");
                        utilisateur.Activation = chkActivationUserModi.Checked = Convert.ToBoolean(user["activation"], CultureInfo.InvariantCulture);
                        utilisateur.Droits = Convert.ToString(user["droits"], CultureInfo.InvariantCulture);
                        id_utilisateur = utilisateur.Id = Convert.ToInt32(user["id"], CultureInfo.InvariantCulture);
                        utilisateur.Id_personne = Convert.ToInt32(user["id_personne"], CultureInfo.InvariantCulture);
                        utilisateur.Schema_user = Convert.ToString(user["schema_user"], CultureInfo.InvariantCulture);
                    }
                    activate_desactivetModifieUser(true);
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    MessageBox.Show(stringManager.GetString("StringFailedSelectUserDataMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedSelectUserDataCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors de sélection des informations de l'utilisateur : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                    ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                }

                catch (System.Security.Cryptography.CryptographicException ex)
                {
                    MessageBox.Show(stringManager.GetString("StringFailedSelectUserDataMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedSelectUserDataCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors de sélection des informations de l'utilisateur : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                    ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                }
                txtOldMotPasse.Clear();
                txtNewMotPasse.Clear();
                txtNewUser.Focus();
            }
        }

        private void activate_desactivetModifieUser(bool state)
        {
            cboUtilisateur.Enabled = state;
            txtNewUser.Enabled = state;
            txtOldMotPasse.Enabled = state;
            //txtNewMotPasse.Enabled = state;
            chkActivationUserModi.Enabled = state;
            groupBox2.Enabled = state;
            cmdModifierCompte.Enabled = state;
        }

        private void dgv_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (bdsrcAffiche.Count != 0)
                {
                    DataRowView user = (DataRowView)bdsrcAffiche.Current;

                    id_utilisateur = Convert.ToInt32(user["id"], CultureInfo.InvariantCulture);
                    utilisateur.Id_personne = Convert.ToInt32(user["id_personne"], CultureInfo.InvariantCulture);

                    if (utilisateur.Activation.HasValue) chkActivationUser.Checked = (bool)utilisateur.Activation;
                    else chkActivationUser.Checked = false;
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedSelectUserDataMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedSelectUserDataCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur dans la zone d'affichage : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedSelectUserDataMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedSelectUserDataCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur dans la zone d'affichage : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
        }
        private void cmdNouveauUser_Click(object sender, EventArgs e)
        {
            try
            {
                New();
            }
            catch (ArgumentException ex)
            {
                cmdValiderUser.Enabled = false;
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors de la création d'un nouvel enregistrement : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
        }

        private void cmdValiderUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (!bln1)
                {
                    clsMetier.GetInstance().insertClsutilisateur(utilisateur);
                    MessageBox.Show(stringManager.GetString("StringSuccessSaveMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringSuccessSaveCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    chkActivationUser.Checked = false;
                }

                this.New();
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

        private void tabMainManage_Selecting(object sender, TabControlCancelEventArgs e)
        {
            //try
            //{
                //Se produit lorsque l'on change un onglet
                if (tabMainManage.SelectedIndex == 0) { activate_desactivetModifieUser(false); }
                else if (tabMainManage.SelectedIndex == 1)
                {
                    if (cboAgent.Items.Count > 0) cboAgent.SelectedIndex = 0;
                    gpParamServeur.Enabled = false;
                    chkParamServeur.Checked = false;
                    activate_desactivetModifieUser(false);
                }
                else if (tabMainManage.SelectedIndex == 2)
                {
                    rdUserSeul.Checked = true;
                    if (cboUtilisateur.Items.Count > 0) cboUtilisateur.SelectedIndex = 0;
                    if (CboUserSup.Items.Count > 0) CboUserSup.SelectedIndex = 0;
                }
                else if (tabMainManage.SelectedIndex == 3) { activate_desactivetModifieUser(false); }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Erreur lors de la sélection de l'onglet, " + ex.Message, "Sélection item onglet", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            //}
        }

        private void cmdDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show(stringManager.GetString("StringPromptDeleteMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringPromptDeleteCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                if (result == DialogResult.Yes)
                {
                    if (bdsrcDelete.DataSource != null)
                    {
                        clsMetier.GetInstance().deleteClsutilisateur((DataRowView)bdsrcDelete.Current);
                        //utilisateur.delete();

                        MessageBox.Show(stringManager.GetString("StringSuccessDeleteMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringSuccessDeleteCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        this.New();
                        RefreshData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Echec de la suppression, " + ex.Message, "Suppression utilisateur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void chkActivationUser_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ((clsutilisateur)bdsrcCreate.Current).Activation = chkActivationUser.Checked;
            }
            catch (Exception) { }
        }

        private void cmdModifierCompte_Click(object sender, EventArgs e)
        {
            try
            {
                if (bdsrcModifie.DataSource != null)
                {
                    DataRowView user = (DataRowView)bdsrcModifie.Current;

                    if (rdUserSeul.Checked)
                    {
                        //Modification de l'utilisateur seulement
                        clsTools.oldUser = Convert.ToString(user["nomuser"], CultureInfo.InvariantCulture);
                        clsTools.newUser = txtNewUser.Text;
                        user["nomuser"] = clsTools.newUser;

                        if (!string.IsNullOrEmpty(txtNewUser.Text))
                        {
                            clsTools.etat_modification_user = 1;
                            int record = new clsutilisateur().update(user);
                            if (record == 0)
                                throw new CustomException(stringManager.GetString("StringZeroRecordAffectedMessage", CultureInfo.CurrentUICulture));
                            else
                                MessageBox.Show(stringManager.GetString("StringSuccessUpdateMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringSuccessUpdateCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                            //this.New();
                            RefreshData();
                            activate_desactivetModifieUser(false);
                        }
                        else
                        {
                            MessageBox.Show(stringManager.GetString("StringEmptyUserNameMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringEmptyUserNameCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            txtNewUser.Focus();
                        }
                    }
                    else if (rdPwdSeul.Checked)
                    {
                        //Modification du mot de passe seulement
                        clsTools.oldUser = Convert.ToString(user["nomuser"], CultureInfo.InvariantCulture);

                        clsTools.oldPassword = ImplementChiffer.Instance.Decipher(Convert.ToString(user["motpass"], CultureInfo.InvariantCulture), "Jos@mRootP@ss");
                        clsTools.newPassword = ImplementChiffer.Instance.Cipher(txtNewMotPasse.Text, "Jos@mRootP@ss");
                        user["motpass"] = clsTools.newPassword;

                        user["nomuser"] = Convert.ToString(((DataRowView)cboUtilisateur1.SelectedItem).Row.ItemArray[2], CultureInfo.InvariantCulture);

                        if (!string.IsNullOrEmpty(txtOldMotPasse.Text))
                        {
                            if (txtOldMotPasse.Text.Equals(clsTools.oldPassword))
                            {
                                if (!string.IsNullOrEmpty(txtNewMotPasse.Text))
                                {
                                    clsTools.etat_modification_user = 2;
                                    int record = new clsutilisateur().update(user);

                                    if (record == 0)
                                        throw new CustomException(stringManager.GetString("StringZeroRecordAffectedMessage", CultureInfo.CurrentUICulture));
                                    else
                                        MessageBox.Show(stringManager.GetString("StringSuccessUpdateMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringSuccessUpdateCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                                    //this.New();
                                    RefreshData();
                                    activate_desactivetModifieUser(false);
                                }
                                else
                                {
                                    MessageBox.Show(stringManager.GetString("StringEmptyNewPasswordMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringEmptyNewPasswordCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                                    txtNewMotPasse.Focus();
                                }
                            }
                            else
                            {
                                MessageBox.Show(stringManager.GetString("StringNoMatchOldPasswordMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringNoMatchOldPasswordCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                                txtOldMotPasse.Focus();
                            }
                        }
                        else
                        {
                            MessageBox.Show(stringManager.GetString("StringEmptyOldPasswordMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringEmptyOldPasswordCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            txtOldMotPasse.Focus();
                        }
                    }
                    else if (rdUserEtPwd.Checked)
                    {
                        //Modification du nom d'utilisateur et du mot de passe
                        clsTools.oldUser = Convert.ToString(user["nomuser"], CultureInfo.InvariantCulture);
                        clsTools.newUser = txtNewUser.Text;
                        user["nomuser"] = clsTools.newUser;

                        clsTools.oldPassword = ImplementChiffer.Instance.Decipher(Convert.ToString(user["motpass"], CultureInfo.InvariantCulture), "Jos@mRootP@ss");
                        clsTools.newPassword = ImplementChiffer.Instance.Cipher(txtNewMotPasse.Text, "Jos@mRootP@ss");
                        user["motpass"] = clsTools.newPassword;

                        if (!string.IsNullOrEmpty(txtNewUser.Text))
                        {
                            if (!string.IsNullOrEmpty(txtOldMotPasse.Text))
                            {
                                if (txtOldMotPasse.Text.Equals(clsTools.oldPassword))
                                {
                                    if (!string.IsNullOrEmpty(txtNewMotPasse.Text))
                                    {
                                        clsTools.etat_modification_user = 3;

                                        int record = new clsutilisateur().update(user);

                                        if (record == 0)
                                            throw new CustomException(stringManager.GetString("StringZeroRecordAffectedMessage", CultureInfo.CurrentUICulture));
                                        else
                                            MessageBox.Show(stringManager.GetString("StringSuccessUpdateMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringSuccessUpdateCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                                        //this.New();
                                        RefreshData();
                                        activate_desactivetModifieUser(false);
                                    }
                                    else
                                    {
                                        MessageBox.Show(stringManager.GetString("StringEmptyNewPasswordMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringEmptyNewPasswordCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                                        txtNewMotPasse.Focus();
                                    }
                                }
                                else
                                {
                                    MessageBox.Show(stringManager.GetString("StringNoMatchOldPasswordMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringNoMatchOldPasswordCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                                    txtOldMotPasse.Focus();
                                }
                            }
                            else
                            {
                                MessageBox.Show(stringManager.GetString("StringEmptyOldPasswordMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringEmptyOldPasswordCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                                txtOldMotPasse.Focus();
                            }
                        }
                        else
                        {
                            MessageBox.Show(stringManager.GetString("StringEmptyUserNameMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringEmptyUserNameCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            txtNewUser.Focus();
                        }
                    }
                    else if (rdActivationUser.Checked)
                    {
                        //Activation/Desactivation de l'utilisateur

                        user["activation"] = chkActivationUserModi.Checked;
                        clsTools.activationUser = Convert.ToBoolean(user["activation"], CultureInfo.InvariantCulture);
                        clsTools.etat_modification_user = 4;

                        int record = new clsutilisateur().update(user);

                        if (record == 0)
                            throw new CustomException(stringManager.GetString("StringZeroRecordAffectedMessage", CultureInfo.CurrentUICulture));
                        else
                            MessageBox.Show(stringManager.GetString("StringSuccessUpdateMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringSuccessUpdateCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                        //this.New();
                        RefreshData();
                    }
                }
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedUpdateUserMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedUpdateUserCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de la modification de l'utilisateur : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedUpdateUserMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedUpdateUserCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de la modification de l'utilisateur : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (CustomException ex)
            {
                Properties.Settings.Default.StringLogFile = ex.Message;
                MessageBox.Show(Properties.Settings.Default.StringLogFile, stringManager.GetString("StringFailedUpdateUserCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de la modification de l'utilisateur : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
        }

        //private void cmdModifierCompte_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (bdsrcModifie.DataSource != null)
        //        {
        //            clsutilisateur user = new clsutilisateur();
        //            Object[] obj = ((DataRowView)bdsrcModifie.Current).Row.ItemArray;
        //            int i = 0;
        //            foreach (DataColumn dtc in ((DataRowView)bdsrcModifie.Current).Row.Table.Columns)
        //            {
        //                if (dtc.ToString().Equals("nomuser"))
        //                {
        //                    if (rdUserSeul.Checked)
        //                    {
        //                        clsTools.oldUser = ((string)obj[i]);
        //                        clsTools.newUser = txtNewUser.Text;
        //                        user.Nomuser = clsTools.newUser;
        //                    }
        //                    else if (rdPwdSeul.Checked)
        //                    {
        //                        clsTools.oldUser = ((string)obj[i]);
        //                    }
        //                    else if (rdUserEtPwd.Checked)
        //                    {
        //                        clsTools.oldUser = ((string)obj[i]);
        //                        clsTools.newUser = txtNewUser.Text;
        //                        user.Nomuser = clsTools.newUser;
        //                    }
        //                    else if (rdActivationUser.Checked)
        //                    {
        //                        user.Nomuser = ((string)obj[i]);
        //                    }
        //                }
        //                else if (dtc.ToString().Equals("motpass"))
        //                {
        //                    if (rdPwdSeul.Checked)
        //                    {
        //                        clsTools.oldPassword = ImplementChiffer.Instance.Decipher(((string)obj[i]), "Jos@mRootP@ss");
        //                        clsTools.newPassword = ImplementChiffer.Instance.Cipher(txtNewMotPasse.Text, "Jos@mRootP@ss");
        //                        user.Motpass = clsTools.newPassword;
        //                    }
        //                    else if (rdUserEtPwd.Checked)
        //                    {
        //                        clsTools.oldPassword = ImplementChiffer.Instance.Decipher(((string)obj[i]), "Jos@mRootP@ss");
        //                        clsTools.newPassword = ImplementChiffer.Instance.Cipher(txtNewMotPasse.Text, "Jos@mRootP@ss");
        //                        user.Motpass = clsTools.newPassword;
        //                    }
        //                    else if (rdActivationUser.Checked) { }
        //                }
        //                //else if (dtc.ToString().Equals("activation")) s.Activation = ((bool)obj[i]);
        //                else if (dtc.ToString().Equals("id")) user.Id = ((int)obj[i]);
        //                else if (dtc.ToString().Equals("id_personne")) user.Id_personne = ((int)obj[i]);
        //                else if (dtc.ToString().Equals("schema_user")) user.Schema_user = ((string)obj[i]);
        //                i++;
        //            }

        //            user.Activation = chkActivationUserModi.Checked;
        //            clsTools.activationUser = (bool)user.Activation;
        //            //Verification des valeurs 

        //            if (rdPwdSeul.Checked)
        //            {
        //                user.Nomuser = Convert.ToString(((DataRowView)cboUtilisateur1.SelectedItem).Row.ItemArray[2]);
        //            }

        //            //Recupération des anciennes valeurs

        //            if (rdUserSeul.Checked)
        //            {
        //                //Modification de l'utilisateur seulement
        //                if (!txtNewUser.Text.Equals(""))
        //                {
        //                    clsTools.etat_modification_user = 1;
        //                    clsMetier.GetInstance().updateClsutilisateur(user);
        //                    //new clsutilisateur().update(user);
        //                    MessageBox.Show("Modification effectuée!", "Modification utilisateur", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

        //                    //this.New();
        //                    RefreshData();
        //                    activate_desactivetModifieUser(false);
        //                }
        //                else
        //                {
        //                    MessageBox.Show("Le nom de l'utilisateur ne peut être vide", "Modification utilisateur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
        //                    txtNewUser.Focus();
        //                }
        //            }
        //            else if (rdPwdSeul.Checked)
        //            {
        //                //Modification du mot de passe seulement
        //                if (!txtOldMotPasse.Text.Equals(""))
        //                {
        //                    if (txtOldMotPasse.Text.Equals(clsTools.oldPassword))
        //                    {
        //                        if (!txtNewMotPasse.Text.Equals(""))
        //                        {
        //                            clsTools.etat_modification_user = 2;
        //                            clsMetier.GetInstance().updateClsutilisateur(user);
        //                            //new clsutilisateur().update(user);
        //                            MessageBox.Show("Modification effectuée!", "Modification utilisateur", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

        //                            //this.New();
        //                            RefreshData();
        //                            activate_desactivetModifieUser(false);
        //                        }
        //                        else
        //                        {
        //                            MessageBox.Show("Le nouveau mot de passe ne peut être vide", "Modification utilisateur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
        //                            txtNewMotPasse.Focus();
        //                        }
        //                    }
        //                    else
        //                    {
        //                        MessageBox.Show("L'ancien mot de passe ne correspond pas", "Modification utilisateur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
        //                        txtOldMotPasse.Focus();
        //                    }
        //                }
        //                else
        //                {
        //                    MessageBox.Show("L'ancien mot de passe ne peut être vide", "Modification utilisateur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
        //                    txtOldMotPasse.Focus();
        //                }
        //            }
        //            else if (rdUserEtPwd.Checked)
        //            {
        //                //Modification du nom d'utilisateur et du mot de passe
        //                if (!txtNewUser.Text.Equals(""))
        //                {
        //                    if (!txtOldMotPasse.Text.Equals(""))
        //                    {
        //                        if (txtOldMotPasse.Text.Equals(clsTools.oldPassword))
        //                        {
        //                            if (!txtNewMotPasse.Text.Equals(""))
        //                            {
        //                                clsTools.etat_modification_user = 3;
        //                                clsMetier.GetInstance().updateClsutilisateur(user);
        //                                //new clsutilisateur().update(user);
        //                                MessageBox.Show("Modification effectuée!", "Modification utilisateur", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

        //                                //this.New();
        //                                RefreshData();
        //                                activate_desactivetModifieUser(false);
        //                            }
        //                            else
        //                            {
        //                                MessageBox.Show("Le nouveau mot de passe ne peut être vide", "Modification utilisateur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
        //                                txtNewMotPasse.Focus();
        //                            }
        //                        }
        //                        else
        //                        {
        //                            MessageBox.Show("L'ancien mot de passe ne correspond pas", "Modification utilisateur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
        //                            txtOldMotPasse.Focus();
        //                        }
        //                    }
        //                    else
        //                    {
        //                        MessageBox.Show("L'ancien mot de passe ne peut être vide", "Modification utilisateur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
        //                        txtOldMotPasse.Focus();
        //                    }
        //                }
        //                else
        //                {
        //                    MessageBox.Show("Le nom de l'utilisateur ne peut être vide", "Modification utilisateur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
        //                    txtNewUser.Focus();
        //                }
        //            }
        //            else if (rdActivationUser.Checked)
        //            {
        //                //Modification du nom d'utilisateur et du mot de passe
        //                clsTools.etat_modification_user = 4;
        //                clsMetier.GetInstance().updateClsutilisateur(user);
        //                //new clsutilisateur().update(user);
        //                MessageBox.Show("Modification effectuée!", "Modification utilisateur", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

        //                //this.New();
        //                RefreshData();
        //            }
        //        }
        //        //activate_desactivetModifieUser(false);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Echec de la modification de l'utilisateur, " + ex.Message, "Modification utilisateur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
        //    }
        //}

        private void rdUserSeul_CheckedChanged(object sender, EventArgs e)
        {
            clsTools.etat_modification_user = 1;
            txtNewMotPasse.Enabled = false;
            txtOldMotPasse.Enabled = false;
            txtNewUser.Enabled = true;
            txtNewMotPasse.Clear();
            txtMotPasse.Clear();
            txtNewUser.Focus();
        }

        private void rdPwdSeul_CheckedChanged(object sender, EventArgs e)
        {
            clsTools.etat_modification_user = 2;
            txtOldMotPasse.Enabled = true;
            txtNewUser.Enabled = false;
            txtNewMotPasse.Enabled = true;
            txtNewUser.Clear();
            txtOldMotPasse.Focus();
        }

        private void rdUserEtPwd_CheckedChanged(object sender, EventArgs e)
        {
            clsTools.etat_modification_user = 3;
            txtOldMotPasse.Enabled = true;
            txtNewUser.Enabled = true;
            txtNewMotPasse.Enabled = true;
            txtNewUser.Clear();
            txtNewMotPasse.Clear();
            txtMotPasse.Clear();
            txtNewUser.Focus();
        }

        private void cboUtilisateur1_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmdAfficherDroit.Enabled = true;

            try
            {
                identifiant_user = Convert.ToInt32(((DataRowView)cboUtilisateur1.SelectedItem).Row.ItemArray[0]);
            }
            catch (ArgumentException ex)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, stringManager.GetString(DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors de la sélection de l'Id de l'utilisateur : " + this.Name + ex.GetType().ToString() + " : " + ex.Message, CultureInfo.CurrentUICulture), Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (NullReferenceException ex)
            {
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors de la sélection de l'Id de l'utilisateur : " + this.Name + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
        }

        private void cmdAfficherDroit_Click(object sender, EventArgs e)
        {
            try
            {
                bdsrcDroit.DataSource = clsMetier.GetInstance().getAllClsutilisateur_Agent2(identifiant_user);
                dgv1.DataSource = bdsrcDroit;

                if (chkLevelAdminstrateur.Checked)
                    chkLevelAdminstrateur.Checked = false;
                else if (chkLevelAdmin.Checked)
                    chkLevelAdmin.Checked = false;
                else if (chkLevelUser.Checked)
                    chkLevelUser.Checked = false;
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadRightAccessMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadRightAccessCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors du chargement des droits : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadRightAccessMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadRightAccessCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors du chargement des droits : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }

            cmdAccorderDroit.Enabled = true;
            cmdRetirerDroit.Enabled = true;
        }

        private void chkLevelAdmin_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLevelAdminstrateur.Checked)
            {
                chkLevelAdmin.Enabled = false;
                chkLevelUser.Enabled = false;

                chkLevelAdmin.Checked = false;
                chkLevelUser.Checked = false;
            }
            else
            {
                chkLevelAdmin.Enabled = true;
                chkLevelUser.Enabled = true;
            }
        }

        private void cmdAccorderDroit_Click(object sender, EventArgs e)
        {
            List<int> liste = new List<int>();

            if (chkLevelAdminstrateur.Checked)
                liste.Add(0);
            else if (chkLevelAdmin.Checked)
                liste.Add(1);
            else if (chkLevelUser.Checked)
                liste.Add(2);

            if (chkLevelAdminstrateur.Checked || chkLevelAdmin.Checked || chkLevelUser.Checked)
            {
                try
                {
                    ExecuteDroitsGrant(liste);

                    //On met à jour les droits de l'utilisateur
                    MessageBox.Show(stringManager.GetString("StringSuccessGrantRightAccessMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringSuccessGrantRightAccessCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    RefreshDroits();
                }
                catch (NullReferenceException ex)
                {
                    MessageBox.Show(stringManager.GetString("StringFailedGrantRightAccessMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedGrantRightAccessCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors de la validation des droits : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                    ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    MessageBox.Show(stringManager.GetString("StringFailedGrantRightAccessMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedGrantRightAccessCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors de la validation des droits : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                    ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                }
            }
            else
                MessageBox.Show(stringManager.GetString("StringWarningSelectGrantRightAccessMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringWarningSelectGrantRightAccessCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

            //try
            //{
            //    cboUtilisateur1.SelectedIndex = 0;
            //}
            //catch(ArgumentOutOfRangeException ex)
        }

        private void ExecuteDroitsGrant(List<int> liste)
        {
            string[] items = clsMetier.GetInstance().getLogin_SchemaUser(identifiant_user);
            clsMetier.GetInstance().grantPermission(liste, items[0], items[1]);

            //Recupération des tous les droit du user pour mis à jour
            int increment = 0;
            int taille = liste.Count;
            string droit = "";

            foreach (int str in liste)
            {
                increment++;
                if (increment == taille)
                {
                    if (str == 0) droit = droit + "Administrateur";
                    else if (str == 1) droit = droit + "Admin";
                    else if (str == 2) droit = droit + "User";
                }
                else
                {
                    if (str == 0) droit = droit + "Administrateur,";
                    else if (str == 1) droit = droit + "Admin";
                    else if (str == 2) droit = droit + "User";
                }
            }

            clsMetier.GetInstance().updateClsutilisateur_droit(identifiant_user, droit);
        }

        private void cmdRetirerDroit_Click(object sender, EventArgs e)
        {
            List<int> liste_droit_user = new List<int>();
            List<int> listeDelete = new List<int>();
            List<int> listeDroit = new List<int>();
            List<int> listeCheckBox = new List<int>();

            //On récuperer l'etat des droit que le user a cocher
            if (chkLevelAdminstrateur.Checked)
                listeCheckBox.Add(0);
            if (chkLevelAdmin.Checked)
                listeCheckBox.Add(1);
            if (chkLevelUser.Checked)
                listeCheckBox.Add(2);

            //On récuperer les anciens droits de l'utilisateur choisi
            try
            {
                liste_droit_user = clsMetier.GetInstance().getDroitsUser(identifiant_user);

                if (liste_droit_user.Count == 0)
                    throw new CustomException("Il n'y a aucun droit à retirer à l'utilisateur !!");

                int item = 0;

                foreach (int str in listeCheckBox)
                {
                    foreach (int str1 in liste_droit_user)
                    {
                        if (str == str1)
                        {
                            //Correspondance avec droit existant pour revocation 
                            listeDelete.Add(str);
                        }
                        else
                        {
                            //Aucune correspondance trouvee entre le droit et celui cocher
                            item++;
                        }
                    }
                    if (item == liste_droit_user.Count)
                    {
                        //On genere une erreur car le droit a revoker n'appartient pas au user
                        throw new CustomException("Vous essayez de retirer un droit d'accès que l'utilisateur n'a pas !!!");
                    }
                    item = 0;
                }

                if (chkLevelAdminstrateur.Checked || chkLevelAdmin.Checked || chkLevelUser.Checked)
                {
                    ExecuteDroitsRevoke(liste_droit_user, listeDelete, listeDroit);

                    //On met à jour les droits de l'utilisateur
                    MessageBox.Show(stringManager.GetString("StringSuccessDenyRightAccessMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringSuccessDenyRightAccessCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                    RefreshDroits();
                }
                else
                    MessageBox.Show(stringManager.GetString("StringWarningSelectDenyRightAccessMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringWarningSelectDenyRightAccessCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedDenyRightAccessMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedDenyRightAccessCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors du retrait des droits d'accès : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedDenyRightAccessMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedDenyRightAccessCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors du retrait des droits d'accès : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (CustomException ex)
            {
                Properties.Settings.Default.StringLogFile = ex.Message;
                MessageBox.Show(Properties.Settings.Default.StringLogFile, stringManager.GetString("StringFailedDenyRightAccessCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors du retrait des droits d'accès : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }

            //cboUtilisateur1.SelectedIndex = 0;
        }

        private void ExecuteDroitsRevoke(List<int> liste_droit_user, List<int> listeDelete, List<int> listeDroit)
        {
            //string nom_utilisateur = clsMetier.GetInstance().getSchemaUser(((clsutilisateur)cboUtilisateur1.SelectedItem).Id);
            string[] items = clsMetier.GetInstance().getLogin_SchemaUser(identifiant_user);
            clsMetier.GetInstance().revokePermission(liste_droit_user, items[0], items[1]);

            //Mise a jour des droits non revoke qui seront ceux ne se trouvant pas dans la liste liste
            int tailleDelete = 0;
            foreach (int str in liste_droit_user)
            {
                foreach (int str1 in listeDelete)
                {
                    if (str == str1)
                    {
                        //Droit revoke et on ne fait rien 
                    }
                    else
                    {
                        //Droit a ne pas revoke mais a garder
                        tailleDelete++;
                    }
                }
                if (tailleDelete == listeDelete.Count) listeDroit.Add(str);
                tailleDelete = 0;
            }

            //Recupération des tous les droit du user pour mis à jour
            int increment = 0;
            int taille = listeDroit.Count;
            string droit = "";
            if (taille == 0) droit = "Aucun";
            else
            {
                foreach (int str in listeDroit)
                {
                    increment++;
                    if (increment == taille)
                    {
                        if (str == 0) droit = droit + "Administrateur";
                        else if (str == 1) droit = droit + "Admin";
                        else if (str == 2) droit = droit + "User";
                    }
                    else
                    {
                        if (str == 0) droit = droit + "Administrateur,";
                        else if (str == 1) droit = droit + "Admin";
                        else if (str == 2) droit = droit + "User";
                    }
                }
            }

            clsMetier.GetInstance().updateClsutilisateur_droit(identifiant_user, droit);
        }

        private void cmdLoadParam_Click(object sender, EventArgs e)
        {
            try
            {
                //Chargement des parametres de connexion 
                //Ici si le fichier est vide ou qu'il n'existe pas,on charge les paramètres par défaut
                List<string> lstValues = ImplementUtilities.Instance.LoadDatabaseParameters(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.DirectoryUtilConn, Properties.Settings.Default.FileSQLServer, '\n'); 
                txtServeur.Text = lstValues[0];
                txtBD.Text = lstValues[1];
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadDataBaseParamsMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadDataBaseParamsCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors du chargement des paramètres de connexion à la Base de Données : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (System.IO.DirectoryNotFoundException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadDataBaseParamsMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadDataBaseParamsCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors du chargement des paramètres de connexion à la Base de Données : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (System.IO.FileNotFoundException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadDataBaseParamsMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadDataBaseParamsCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors du chargement des paramètres de connexion à la Base de Données : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (System.IO.IOException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedLoadDataBaseParamsMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedLoadDataBaseParamsCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors du chargement des paramètres de connexion à la Base de Données : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
        }

        private void chkLevelAdmin_CheckedChanged_1(object sender, EventArgs e)
        {
            if (chkLevelAdmin.Checked)
            {
                chkLevelAdminstrateur.Enabled = false;
                chkLevelUser.Enabled = false;

                chkLevelAdminstrateur.Checked = false;
                chkLevelUser.Checked = false;
            }
            else
            {
                chkLevelAdminstrateur.Enabled = true;
                chkLevelUser.Enabled = true;
            }
        }

        private void frmUtilisateur_Activated(object sender, EventArgs e)
        {
            Principal.SetValuesLabel(Properties.Settings.Default.UserConnected, "Gestion des utilisateurs");
        }

        private void cmdEnregistrer_Click(object sender, EventArgs e)
        {
            try
            {
                //Enregistrement des parametres de connexion
                clsConnexion connection = new clsConnexion();

                connection.Serveur = txtServeur.Text;
                connection.DB = txtBD.Text;

                ImplementUtilities.Instance.SaveParameters(Properties.Settings.Default.MasterDirectory,
                    string.Format("Serveur={0}\nDataBase={1}\nUserBD={2}\nPassword={3}", txtServeur.Text, txtBD.Text, string.Empty, string.Empty),
                    Properties.Settings.Default.DirectoryUtilConn, Properties.Settings.Default.FileSQLServer);

                MessageBox.Show(stringManager.GetString("StringSuccessSaveDataBaseParamsMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringSuccessSaveDataBaseParamsCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                txtServeur.Clear();
                txtBD.Clear();
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedSaveDataBaseParamsMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedSaveDataBaseParamsCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors de l'enregistrement des paramètres de connexion : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (System.IO.DirectoryNotFoundException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedSaveDataBaseParamsMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedSaveDataBaseParamsCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors de l'enregistrement des paramètres de connexion : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName); 
            }
            catch (System.IO.FileNotFoundException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedSaveDataBaseParamsMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedSaveDataBaseParamsCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors de l'enregistrement des paramètres de connexion : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
            catch (System.IO.IOException ex)
            {
                MessageBox.Show(stringManager.GetString("StringFailedSaveDataBaseParamsMessage", CultureInfo.CurrentUICulture), stringManager.GetString("StringFailedSaveDataBaseParamsCaption", CultureInfo.CurrentUICulture), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Properties.Settings.Default.StringLogFile = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Erreur lors de l'enregistrement des paramètres de connexion : " + this.Name + " : " + ex.GetType().ToString() + " : " + ex.Message;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, Properties.Settings.Default.StringLogFile, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
            }
        }

        private void chkParamServeur_CheckedChanged(object sender, EventArgs e)
        {
            if (chkParamServeur.Checked)
            {
                gpParamServeur.Enabled = true;
                txtVersion.Enabled = false;
            }
            else gpParamServeur.Enabled = false;
        }

        private void rdActivationUser_CheckedChanged(object sender, EventArgs e)
        {
            chkActivationUserModi.Enabled = true;
            txtOldMotPasse.Enabled = false;
            txtNewMotPasse.Enabled = false;
            txtNewUser.Enabled = false;
            txtNewMotPasse.Enabled = false;
            txtNewUser.Clear();
            txtNewMotPasse.Focus();
        }
    }
}