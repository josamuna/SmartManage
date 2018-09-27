using ManageUtilities;
using smartManage.Model;
using smartManage.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace smartManage.Desktop
{
    public partial class frmUtilisateur : Form
    {
        //Repertoire pour le Log
        private const string MasterDirectory = "SmartManage";
        //Nom du repertoire qui contiendra la chaine de connexion a la BD
        private const string DirectoryUtilConn = "ConnectionString";
        //Nom du fichier qui contiendra la chaine de connexion connexion a la BD SQLServer
        private const string FileSQLServer = "UserSQLSever.txt";
        //Repertoire pour le Log
        const string DirectoryUtilLog = "Log";

        //Instance du formulaire principal
        public frmPrincipal Principal
        {
            get; set;
        }

        public frmUtilisateur()
        {
            InitializeComponent();
            ImplementUtilities.Instance.MasterDirectoryConfiguration = MasterDirectory;
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
        private BindingSource bsrc1 = new BindingSource();

        private BindingSource bdsrcCreate = new BindingSource();
        private BindingSource bdsrcDelete = new BindingSource();
        private BindingSource bdsrcAffiche = new BindingSource();
        private BindingSource bdsrcModifie = new BindingSource();
        private BindingSource bdsrcDroit = new BindingSource();

        private string userName = "", olPwd = "";
        private int id_utilisateur = 0, identifiant_user = 0, id_agentuser = 0;
        private bool okDoubleClicDgv = false;
        private string schema_user = "";
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
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement, " + ex.Message, "Erreur de chargement des données", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            Principal.SetValuesLabel(Properties.Settings.Default.UserConnected, "Attente d'une action de l'utilisateur");
            this.Close();
        }

        private void tabManage_Selecting(object sender, TabControlCancelEventArgs e)
        {
            try
            {
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
            }
            catch { }
        }

        private void txtSeach_TextChanged(object sender, EventArgs e)
        {
            try
            {
                bdsrcAffiche.Filter = "Nomuser LIKE '%" + txtSeach.Text + "%' OR nom LIKE '%" + txtSeach.Text + "%'";
            }
            catch { }
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
                        Object[] obj = ((DataRowView)bdsrcModifie.Current).Row.ItemArray;
                        int i = 0;
                        foreach (DataColumn dtc in ((DataRowView)bdsrcModifie.Current).Row.Table.Columns)
                        {
                            if (dtc.ToString().Equals("nomuser")) { cboUtilisateur.Text = ((string)obj[i]); userName = ((string)obj[i]); utilisateur.Nomuser = ((string)obj[i]); }
                            else if (dtc.ToString().Equals("motpass")) { txtOldMotPasse.Text = ImplementChiffer.Instance.Decipher(((string)obj[i]), "Jos@mRootP@ss"); olPwd = ImplementChiffer.Instance.Cipher(((string)obj[i]), "rootP@ss"); }
                            else if (dtc.ToString().Equals("activation")) { chkActivationUserModi.Checked = ((bool)obj[i]); utilisateur.Activation = ((bool)obj[i]); }
                            else if (dtc.ToString().Equals("droits")) { utilisateur.Droits = ((string)obj[i]); }
                            else if (dtc.ToString().Equals("id")) { id_utilisateur = ((int)obj[i]); utilisateur.Id = ((int)obj[i]); }
                            else if (dtc.ToString().Equals("id_personne")) { id_agentuser = ((int)obj[i]); utilisateur.Id_personne = ((int)obj[i]); }
                            else if (dtc.ToString().Equals("schema_user")) { schema_user = ((string)obj[i]); utilisateur.Schema_user = ((string)obj[i]); }

                            i++;
                        }
                    }
                    activate_desactivetModifieUser(true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de sélection des informations de l'utilisateur", "Erreur " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                    Object[] obj = ((DataRowView)bdsrcAffiche.Current).Row.ItemArray;
                    int i = 0;
                    foreach (DataColumn dtc in ((DataRowView)bdsrcAffiche.Current).Row.Table.Columns)
                    {
                        if (dtc.ToString().Equals("id")) id_utilisateur = ((int)obj[i]);
                        else if (dtc.ToString().Equals("id_personne")) id_agentuser = ((int)obj[i]);
                        i++;
                    }
                    if (utilisateur.Activation.HasValue) chkActivationUser.Checked = (bool)utilisateur.Activation;
                    else chkActivationUser.Checked = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Erreur dans la zone d'affichage", "Erreur d'affichage");
            }
        }
        private void cmdNouveauUser_Click(object sender, EventArgs e)
        {
            try
            {
                New();
            }
            catch (Exception) { cmdValiderUser.Enabled = false; }
        }

        private void cmdValiderUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (!bln1)
                {
                    clsMetier.GetInstance().insertClsutilisateur(utilisateur);
                    MessageBox.Show("Enregistrement éffectué", "Enregistrement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    chkActivationUser.Checked = false;
                }

                this.New();
                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Echec de la mise à jour, " + ex.Message, "Mise à jour", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void tabMainManage_Selecting(object sender, TabControlCancelEventArgs e)
        {
            try
            {
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la sélection de l'onglet, " + ex.Message, "Sélection item onglet", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void cmdDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Voulez - vous vraiment supprimer cet enregistrement ?", "Suppression", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    if (bdsrcDelete.DataSource != null)
                    {
                        clsMetier.GetInstance().deleteClsutilisateur(utilisateur);
                        //utilisateur.delete();

                        MessageBox.Show("Suppression éffectuée", "Suppression utilisateur", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.New();
                        RefreshData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Echec de la suppression, " + ex.Message, "Suppression utilisateur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                    clsutilisateur user = new clsutilisateur();
                    Object[] obj = ((DataRowView)bdsrcModifie.Current).Row.ItemArray;
                    int i = 0;
                    foreach (DataColumn dtc in ((DataRowView)bdsrcModifie.Current).Row.Table.Columns)
                    {
                        if (dtc.ToString().Equals("nomuser"))
                        {
                            if (rdUserSeul.Checked)
                            {
                                clsTools.oldUser = ((string)obj[i]);
                                clsTools.newUser = txtNewUser.Text;
                                user.Nomuser = clsTools.newUser;
                            }
                            else if (rdPwdSeul.Checked)
                            {
                                clsTools.oldUser = ((string)obj[i]);
                            }
                            else if (rdUserEtPwd.Checked)
                            {
                                clsTools.oldUser = ((string)obj[i]);
                                clsTools.newUser = txtNewUser.Text;
                                user.Nomuser = clsTools.newUser;
                            }
                            else if (rdActivationUser.Checked)
                            {
                                user.Nomuser = ((string)obj[i]);
                            }
                        }
                        else if (dtc.ToString().Equals("motpass"))
                        {
                            if (rdPwdSeul.Checked)
                            {
                                clsTools.oldPassword = ImplementChiffer.Instance.Decipher(((string)obj[i]), "rootP@ss");
                                clsTools.newPassword = ImplementChiffer.Instance.Cipher(txtNewMotPasse.Text, "rootP@ss");
                                user.Motpass = clsTools.newPassword;
                            }
                            else if (rdUserEtPwd.Checked)
                            {
                                clsTools.oldPassword = ImplementChiffer.Instance.Decipher(((string)obj[i]), "rootP@ss");
                                clsTools.newPassword = ImplementChiffer.Instance.Cipher(txtNewMotPasse.Text, "rootP@ss");
                                user.Motpass = clsTools.newPassword;
                            }
                            else if (rdActivationUser.Checked) { }
                        }
                        //else if (dtc.ToString().Equals("activation")) s.Activation = ((bool)obj[i]);
                        else if (dtc.ToString().Equals("id")) user.Id = ((int)obj[i]);
                        else if (dtc.ToString().Equals("id_personne")) user.Id_personne = ((int)obj[i]);
                        else if (dtc.ToString().Equals("schema_user")) user.Schema_user = ((string)obj[i]);
                        i++;
                    }

                    user.Activation = chkActivationUserModi.Checked;
                    clsTools.activationUser = (bool)user.Activation;
                    //Verification des valeurs 

                    if (rdPwdSeul.Checked)
                    {
                        user.Nomuser = Convert.ToString(((DataRowView)cboUtilisateur1.SelectedItem).Row.ItemArray[2]);
                    }

                    //Recupération des anciennes valeurs

                    if (rdUserSeul.Checked)
                    {
                        //Modification de l'utilisateur seulement
                        if (!txtNewUser.Text.Equals(""))
                        {
                            clsTools.etat_modification_user = 1;
                            clsMetier.GetInstance().updateClsutilisateur(user);
                            //new clsutilisateur().update(user);
                            MessageBox.Show("Modification effectuée!", "Modification utilisateur", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            //this.New();
                            RefreshData();
                            activate_desactivetModifieUser(false);
                        }
                        else
                        {
                            MessageBox.Show("Le nom de l'utilisateur ne peut être vide", "Modification utilisateur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            txtNewUser.Focus();
                        }
                    }
                    else if (rdPwdSeul.Checked)
                    {
                        //Modification du mot de passe seulement
                        if (!txtOldMotPasse.Text.Equals(""))
                        {
                            if (txtOldMotPasse.Text.Equals(clsTools.oldPassword))
                            {
                                if (!txtNewMotPasse.Text.Equals(""))
                                {
                                    clsTools.etat_modification_user = 2;
                                    clsMetier.GetInstance().updateClsutilisateur(user);
                                    //new clsutilisateur().update(user);
                                    MessageBox.Show("Modification effectuée!", "Modification utilisateur", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    //this.New();
                                    RefreshData();
                                    activate_desactivetModifieUser(false);
                                }
                                else
                                {
                                    MessageBox.Show("Le nouveau mot de passe ne peut être vide", "Modification utilisateur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    txtNewMotPasse.Focus();
                                }
                            }
                            else
                            {
                                MessageBox.Show("L'ancien mot de passe ne correspond pas", "Modification utilisateur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                txtOldMotPasse.Focus();
                            }
                        }
                        else
                        {
                            MessageBox.Show("L'ancien mot de passe ne peut être vide", "Modification utilisateur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            txtOldMotPasse.Focus();
                        }
                    }
                    else if (rdUserEtPwd.Checked)
                    {
                        //Modification du nom d'utilisateur et du mot de passe
                        if (!txtNewUser.Text.Equals(""))
                        {
                            if (!txtOldMotPasse.Text.Equals(""))
                            {
                                if (txtOldMotPasse.Text.Equals(clsTools.oldPassword))
                                {
                                    if (!txtNewMotPasse.Text.Equals(""))
                                    {
                                        clsTools.etat_modification_user = 3;
                                        clsMetier.GetInstance().updateClsutilisateur(user);
                                        //new clsutilisateur().update(user);
                                        MessageBox.Show("Modification effectuée!", "Modification utilisateur", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                        //this.New();
                                        RefreshData();
                                        activate_desactivetModifieUser(false);
                                    }
                                    else
                                    {
                                        MessageBox.Show("Le nouveau mot de passe ne peut être vide", "Modification utilisateur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                        txtNewMotPasse.Focus();
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("L'ancien mot de passe ne correspond pas", "Modification utilisateur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    txtOldMotPasse.Focus();
                                }
                            }
                            else
                            {
                                MessageBox.Show("L'ancien mot de passe ne peut être vide", "Modification utilisateur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                txtOldMotPasse.Focus();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Le nom de l'utilisateur ne peut être vide", "Modification utilisateur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            txtNewUser.Focus();
                        }
                    }
                    else if (rdActivationUser.Checked)
                    {
                        //Modification du nom d'utilisateur et du mot de passe
                        clsTools.etat_modification_user = 4;
                        clsMetier.GetInstance().updateClsutilisateur(user);
                        //new clsutilisateur().update(user);
                        MessageBox.Show("Modification effectuée!", "Modification utilisateur", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        //this.New();
                        RefreshData();
                    }
                }
                //activate_desactivetModifieUser(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Echec de la modification de l'utilisateur, " + ex.Message, "Modification utilisateur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

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
            catch (Exception) { }
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
            catch (Exception)
            {
                MessageBox.Show("Erreur lors du chargement", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                    MessageBox.Show("Droits attribué avec succès", "Attributions droits d'accès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshDroits();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de l'attribution des droits d'accès, " + ex.Message, "Attributions droits d'accès", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
                MessageBox.Show("Veuillez choisir une catégorie des droits à attribuer à l'utilisateur svp !!", "Attributions droits d'accès", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            cboUtilisateur1.SelectedIndex = 0;
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
                    throw new Exception("Il n'y a aucun droit à retirer à l'utilisateur !!");

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
                        throw new Exception("Vous essayez de retirer un droit d'accès que l'utilisateur n'a pas !!!");
                    }
                    item = 0;
                }

                if (chkLevelAdminstrateur.Checked || chkLevelAdmin.Checked || chkLevelUser.Checked)
                {
                    ExecuteDroitsRevoke(liste_droit_user, listeDelete, listeDroit);

                    //On met à jour les droits de l'utilisateur
                    MessageBox.Show("Droits d'accès retirés avec succès", "Retrait droits d'accès", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    RefreshDroits();
                }
                else
                    MessageBox.Show("Veuillez choisir une catégorie des droits d'accès à retirer à l'utilisateur svp !!", "Retrait droits d'accès", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du retrait des droits d'accès, " + ex.Message, "Retrait droits d'accès", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            cboUtilisateur1.SelectedIndex = 0;
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
                List<string> lstValues = ImplementUtilities.Instance.LoadDatabaseParameters(MasterDirectory, DirectoryUtilConn, FileSQLServer, '\n');
                txtServeur.Text = lstValues[0];
                txtBD.Text = lstValues[1];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des paramètres de connexion à la Base de Données, " + ex.Message, "Paramètres de connexion à la Base de Données", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

                ImplementUtilities.Instance.SaveParameters(MasterDirectory,
                    string.Format("Serveur={0}\nDataBase={1}\nUserBD={2}\nPassword={3}", txtServeur.Text, txtBD.Text, string.Empty, string.Empty),
                    DirectoryUtilConn, FileSQLServer);

                MessageBox.Show("Enregistrement effectué avec succès", "Enregistrement paramètres de connexion", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtServeur.Clear();
                txtBD.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'enregistrement des paramètres de connexion, " + ex.Message, "Enregistrement paramètres de connexion", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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