using ManageUtilities;
using smartManage.Model;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace smartManage.Desktop
{
    public partial class frmPrincipal : Form
    {
        private ICRUDGeneral frmCurrent = null;

        #region CODE FOR APPLIYING CHILD FORM AND MDIFORM

        private string UserConnect;
        private string ApplStatus;

        /// <summary>
        /// Allow to set values for label applying to Status bar
        /// </summary>
        /// <param name="userConnected">String user connected</param>
        /// <param name="statusApplication">string application status</param>
        public void SetValuesLabel(string userConnected, string statusApplication)
        {
            if (!string.IsNullOrEmpty(userConnected))
                UserConnect = userConnected;
            else
                UserConnect = "Aucun utilisateur n'est encore connecté";

            if (!string.IsNullOrEmpty(statusApplication))
                ApplStatus = statusApplication;
            else
                ApplStatus = "Attente d'une action de l'utilisateur";

            this.AfficherPropertySetForm(this.GetValuesLabel());
        }

        public void ActivateMainBindingSource(Form formMDI)
        {
            if (formMDI.IsMdiContainer)
            {
                if ((formMDI.MdiChildren.Length > 1))
                    this.bdNav.Enabled = true;
                else if ((formMDI.MdiChildren.Length == 0))
                    this.bdNav.Enabled = false;
                else
                    this.bdNav.Enabled = false;
            }
            else
                return;
        }

        public void ActivateMainBindingSource(bool value)
        {
            this.bdNav.Enabled = value;
        }

        /// <summary>
        /// Allow to set values for label applying to Status bar
        /// </summary>
        /// <param name="statusApplication">string application status</param>
        public void SetValuesLabel(string statusApplication)
        {
            if (!string.IsNullOrEmpty(statusApplication))
                ApplStatus = statusApplication;
            else
                ApplStatus = "Attente d'une action de l'utilisateur";

            this.AfficherPropertySetForm();
        }

        /// <summary>
        /// Allow to get values for label applying to Status bar
        /// </summary>
        /// <returns>List of values affected</returns>
        public List<string> GetValuesLabel()
        {
            List<string> lstValues = new List<string>();

            lstValues.Add(UserConnect);
            lstValues.Add(ApplStatus);

            return lstValues;
        }

        /// <summary>
        /// Allow to set Default value for label in Status Bar when passing a current MDI Form
        /// </summary>
        /// <param name="formMDI">Instance of MDI Form</param>
        public void ApplyDefaultStatusBar(Form formMDI, string connectedUser)
        {
            if (formMDI.IsMdiContainer)
            {
                if ((formMDI.MdiChildren.Length >= 0))
                {
                    SetValuesLabel(connectedUser, "");
                    this.UnloadDataSource();
                    this.bdNav.Enabled = false;
                }
            }
            else
                return;
        }

        public void CloseAllChildrensForms(Form formMDI)
        {
            if (formMDI.IsMdiContainer)
            {
                if ((formMDI.MdiChildren.Length > 1))
                {
                    foreach (Form frm in formMDI.MdiChildren)
                        frm.Close();
                    SetValuesLabel("");
                }
                else
                    return;
            }
        }

        public void SetCurrentICRUDChildForm(Form frmChildForm)
        {
            if (frmChildForm.IsMdiChild)
                frmCurrent = (ICRUDGeneral)frmChildForm;
        }

        /// <summary>
        /// Allow to set the DataSource on common BindingNavigator located on Main Form or MDIForm
        /// </summary>
        /// <param name="bdsrc">BindingSource variable</param>
        public void SetDataSource(BindingSource bdsrc)
        {
            this.bdNav.BindingSource = bdsrc;
        }

        public void ActivateOnLoadCommandButtons(bool blnStatus)
        {
            bdDelete.Enabled = blnStatus;
            bdSave.Enabled = blnStatus;
        }

        public void ActivateOnNewCommandButtons(bool blnStatus)
        {
            bdSave.Enabled = blnStatus;
        }

        public void ActivateOnNewSelectionChangeDgvCommandButtons(bool blnStatus)
        {
            bdDelete.Enabled = blnStatus;
            bdSave.Enabled = blnStatus;
        }

        public void ActivateOnNewSelectionChangeDgvCommandButtons1(bool blnStatus)
        {
            bdDelete.Enabled = blnStatus;
        }

        public void ActivateOnSelectionChangeDgvExceptionCommandButtons(bool blnStatus)
        {
            bdDelete.Enabled = blnStatus;
        }

        /// <summary>
        /// Allow to unload BindingNavigator Datasource for reinitialisation or when all Children's forms are closed
        /// </summary>
        private void UnloadDataSource()
        {
            this.bdNav.BindingSource = null;
        }

        public void LockMenu(bool val, string level)
        {
            if (string.IsNullOrEmpty(level))
            {
                //Desactivation de tous les menus
                //Sous Menus
                smConnection.Enabled = !val;
                smDeconnection.Enabled = val;
                smPerson.Enabled = val;
                smSignataire.Enabled = val;

                ssmAffectationPerson.Enabled = val;
                ssmAffectationMaterials.Enabled = val;

                smRptMateriels.Enabled = val;
                smRptPerson.Enabled = val;

                smManageUsers.Enabled = val;
                smRestoreBackup.Enabled = val;

                //Sous sous Menu
                ssmAmplifie.Enabled = val;
                ssmAP.Enabled = val;
                ssmEmetteur.Enabled = val;
                ssmPrinter.Enabled = val;
                ssmComputer.Enabled = val;
                ssmRouter.Enabled = val;
                ssmProjector.Enabled = val;
                ssmSwitch.Enabled = val;

                ssmAffectationPerson.Enabled = val;
                ssmAffectationMaterials.Enabled = val;

                ssmRptAffectationMateriels.Enabled = val;
                ssmRptAffectationPerson.Enabled = val;

                ssmRadiusServerAdmin.Enabled = val;
                ssmRadiusServerStudent.Enabled = val;
                ssmParamServer.Enabled = val;
            }
            else if (level.Equals("Administrateur"))
            {
                //Gestionniaire du system avec le full access
                //Sous Menus
                smConnection.Enabled = !val;
                smDeconnection.Enabled = val;
                smPerson.Enabled = val;
                smSignataire.Enabled = val;

                smRptMateriels.Enabled = val;
                smRptPerson.Enabled = val;

                smManageUsers.Enabled = val;
                smRestoreBackup.Enabled = val;

                //Sous sous Menu
                ssmAmplifie.Enabled = val;
                ssmAP.Enabled = val;
                ssmEmetteur.Enabled = val;
                ssmPrinter.Enabled = val;
                ssmComputer.Enabled = val;
                ssmRouter.Enabled = val;
                ssmProjector.Enabled = val;
                ssmSwitch.Enabled = val;

                ssmAffectationPerson.Enabled = val;
                ssmAffectationMaterials.Enabled = val;

                ssmRptAffectationMateriels.Enabled = val;
                ssmRptAffectationPerson.Enabled = val;

                ssmRadiusServerAdmin.Enabled = val;
                ssmRadiusServerStudent.Enabled = val;
                ssmParamServer.Enabled = val;
            }
            else if (level.Equals("Admin"))
            {
                //Utilisateur avec certains privileges superieurs au user normal
                //Sous Menus
                smConnection.Enabled = !val;
                smDeconnection.Enabled = val;
                smPerson.Enabled = val;
                smSignataire.Enabled = !val;

                ssmAffectationPerson.Enabled = val;
                ssmAffectationMaterials.Enabled = val;

                smRptMateriels.Enabled = val;
                smRptPerson.Enabled = val;

                smManageUsers.Enabled = !val;
                smRestoreBackup.Enabled = !val;

                //Sous sous Menu
                ssmAmplifie.Enabled = val;
                ssmAP.Enabled = val;
                ssmEmetteur.Enabled = val;
                ssmPrinter.Enabled = val;
                ssmComputer.Enabled = val;
                ssmRouter.Enabled = val;
                ssmProjector.Enabled = val;
                ssmSwitch.Enabled = val;

                ssmRptAffectationMateriels.Enabled = val;
                ssmRptAffectationPerson.Enabled = val;

                ssmRadiusServerAdmin.Enabled = !val;
                ssmRadiusServerStudent.Enabled = !val;
                ssmParamServer.Enabled = !val;
            }
            else if (level.Equals("User"))
            {
                //Utilisateur avec certains privileges superieurs au user normal
                //Sous Menus
                smConnection.Enabled = !val;
                smDeconnection.Enabled = val;
                smPerson.Enabled = val;
                smSignataire.Enabled = !val;

                ssmAffectationPerson.Enabled = val;
                ssmAffectationMaterials.Enabled = !val;

                smRptMateriels.Enabled = val;
                smRptPerson.Enabled = val;

                smManageUsers.Enabled = !val;
                smRestoreBackup.Enabled = !val;

                //Sous sous Menu
                ssmAmplifie.Enabled = val;
                ssmAP.Enabled = val;
                ssmEmetteur.Enabled = val;
                ssmPrinter.Enabled = val;
                ssmComputer.Enabled = val;
                ssmRouter.Enabled = val;
                ssmProjector.Enabled = val;
                ssmSwitch.Enabled = val;

                ssmRptAffectationMateriels.Enabled = !val;
                ssmRptAffectationPerson.Enabled = !val;

                ssmRadiusServerAdmin.Enabled = !val;
                ssmRadiusServerStudent.Enabled = !val;
                ssmParamServer.Enabled = !val;
            }
        }

        #endregion

        public frmPrincipal()
        {
            InitializeComponent();

            ImplementUtilities.Instance.MasterDirectoryConfiguration = "SmartManage";

            lblDate.Text = string.Format("{0} {1}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString());

            this.SetValuesLabel("", "");
            AfficherPropertySetForm(this.GetValuesLabel());
        }

        private void AfficherPropertySetForm()
        {
            this.lblStatusApplication.Text = UserConnect;
        }

        private void AfficherPropertySetForm(List<string> lst)
        {
            this.lblUserConnected.Text = lst[0];
            this.lblStatusApplication.Text = lst[1];
        }

        private void smConnection_Click(object sender, EventArgs e)
        {
            frmConnection frm = new frmConnection();
            frm.Principal = this;
            frm.MdiParent = this;
            frm.Icon = this.Icon;
            frm.Show();
        }

        private void ssmComputer_Click(object sender, EventArgs e)
        {
            frmOrdinateur frm = new frmOrdinateur();
            frm.Principal = this;
            frm.MdiParent = this;
            frm.Icon = this.Icon;
            frmCurrent = frm;
            frm.Show();
            this.bdNav.Enabled = true;
        }

        private void frmPrincipal_MdiChildActivate(object sender, EventArgs e)
        {
            List<string> lstProperty = this.GetValuesLabel();

            this.lblUserConnected.Text = lstProperty[0];
            this.lblStatusApplication.Text = lstProperty[1];
        }

        private void ssmPrinter_Click(object sender, EventArgs e)
        {
            //clsPropertyForm.SetChildForm(new frmImprimante(), this, frmCurrent);
            frmImprimante frm = new frmImprimante();
            frm.Principal = this;
            frm.MdiParent = this;
            frm.Icon = this.Icon;
            frmCurrent = frm;
            frm.Show();
            this.bdNav.Enabled = true;
        }

        private void bdSave_Click(object sender, EventArgs e)
        {
            if (frmCurrent != null)
                frmCurrent.Save();
        }

        private void bdRefresh_Click(object sender, EventArgs e)
        {
            if (frmCurrent != null)
                frmCurrent.RefreshRec();
        }

        private void bdPreview_Click(object sender, EventArgs e)
        {
            if (frmCurrent != null)
                frmCurrent.Preview();
        }

        private void bdSearch_Click(object sender, EventArgs e)
        {
            if (frmCurrent != null)
                frmCurrent.Search(txtSearch.Text);
        }

        private void smManageUsers_Click(object sender, EventArgs e)
        {
            frmUtilisateur frm = new frmUtilisateur();
            frm.Principal = this;
            frm.MdiParent = this;
            frm.Icon = this.Icon;
            frm.Show();
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            LockMenu(false, null);
        }

        private void frmPrincipal_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Fermeture de la connexion a la BD et on quitte
        }

        private void smCloseAllForms_Click(object sender, EventArgs e)
        {
            try
            {
                this.CloseAllChildrensForms(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Echec de fermeture des formulaires enfants, " + ex.Message, "Fermeture des formulaires enfants", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void smQuit_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Voulez-vous quitter ?", "Quitter l'application", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
                Application.Exit();
            else if (dr == DialogResult.No)
                return;
        }

        private void smDeconnection_Click(object sender, EventArgs e)
        {
            try
            {
                clsMetier.GetInstance().CloseConnection();
            }
            catch (Exception) { }

            this.LockMenu(false, null);
            this.ApplyDefaultStatusBar(this, "Aucun utilisateur n'est encore connecté");
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                if (frmCurrent != null)
                    frmCurrent.Search(txtSearch.Text);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtSearch.Text))
                    if (frmCurrent != null)
                        frmCurrent.RefreshRec();
            }
            catch (Exception) { }
        }

        private void bdNew_Click(object sender, EventArgs e)
        {
            if (frmCurrent != null)
                frmCurrent.New();
        }

        private void ssmEmetteur_Click(object sender, EventArgs e)
        {
            frmEmetteur frm = new frmEmetteur();
            frm.Principal = this;
            frm.MdiParent = this;
            frm.Icon = this.Icon;
            frmCurrent = frm;
            frm.Show();
            this.bdNav.Enabled = true;
        }

        private void bdDelete_Click(object sender, EventArgs e)
        {
            if (frmCurrent != null)
                frmCurrent.Delete();
        }

        private void ssmRouter_Click(object sender, EventArgs e)
        {
            frmRouteurAP frm = new frmRouteurAP();
            frm.Principal = this;
            frm.MdiParent = this;
            frm.Icon = this.Icon;
            frmCurrent = frm;
            frm.Show();
            this.bdNav.Enabled = true;
        }

        private void ssmAmplifie_Click(object sender, EventArgs e)
        {
            frmAmplificateur frm = new frmAmplificateur();
            frm.Principal = this;
            frm.MdiParent = this;
            frm.Icon = this.Icon;
            frmCurrent = frm;
            frm.Show();
            this.bdNav.Enabled = true;
        }

        private void ssmSwitch_Click(object sender, EventArgs e)
        {
            frmSwitch frm = new frmSwitch();
            frm.Principal = this;
            frm.MdiParent = this;
            frm.Icon = this.Icon;
            frmCurrent = frm;
            frm.Show();
            this.bdNav.Enabled = true;
        }

        private void ssmAP_Click(object sender, EventArgs e)
        {
            frmAccessPoint frm = new frmAccessPoint();
            frm.Principal = this;
            frm.MdiParent = this;
            frm.Icon = this.Icon;
            frmCurrent = frm;
            frm.Show();
            this.bdNav.Enabled = true;
        }

        private void ssmProjector_Click(object sender, EventArgs e)
        {
            frmRetroprojecteur frm = new frmRetroprojecteur();
            frm.Principal = this;
            frm.MdiParent = this;
            frm.Icon = this.Icon;
            frmCurrent = frm;
            frm.Show();
            this.bdNav.Enabled = true;
        }

        private void smSignataire_Click(object sender, EventArgs e)
        {
            frmSignataire frm = new frmSignataire();
            frm.Principal = this;
            frm.MdiParent = this;
            frm.Icon = this.Icon;
            frmCurrent = frm;
            frm.Show();
            this.bdNav.Enabled = true;
        }

        private void smPerson_Click(object sender, EventArgs e)
        {
            frmPersonne frm = new frmPersonne();
            frm.Principal = this;
            frm.MdiParent = this;
            frm.Icon = this.Icon;
            frmCurrent = frm;
            frm.Show();
            this.bdNav.Enabled = true;
        }

        private void ssmAffectationPerson_Click(object sender, EventArgs e)
        {
            frmLieuAffectationPersonneMateriel frm = new frmLieuAffectationPersonneMateriel();
            frm.Principal = this;
            frm.MdiParent = this;
            frm.Icon = this.Icon;
            frmCurrent = frm;
            frm.Show();
            this.bdNav.Enabled = true;
        }

        private void ssmAffectationMaterials_Click(object sender, EventArgs e)
        {
            frmAffectationMateriel frm = new frmAffectationMateriel();
            frm.Principal = this;
            frm.MdiParent = this;
            frm.Icon = this.Icon;
            frmCurrent = frm;
            frm.Show();
            this.bdNav.Enabled = true;
        }

        private void ssmParamServer_Click(object sender, EventArgs e)
        {
            frmParametersServeur frm = new frmParametersServeur();
            frm.Principal = this;
            frm.MdiParent = this;
            frm.Icon = this.Icon;
            frm.Show();
        }

        private void sssmVueDataAdministration_Click(object sender, EventArgs e)
        {
            frmDataViewAdministration frm = new frmDataViewAdministration();
            frm.Principal = this;
            frm.MdiParent = this;
            frm.Icon = this.Icon;
            frmCurrent = frm;
            frm.Show();
            //this.bdNav.Enabled = true;
        }

        private void sssmVueDataStudent_Click(object sender, EventArgs e)
        {
            frmDataViewEtudiant frm = new frmDataViewEtudiant();
            frm.Principal = this;
            frm.MdiParent = this;
            frm.Icon = this.Icon;
            frmCurrent = frm;
            frm.Show();
        }
    }
}
