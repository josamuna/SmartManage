using smartManage.Model;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace smartManage.Desktop
{
    public partial class frmOS : Form
    {
        BindingSource bdsrc = new BindingSource();
        bool blnModifie = false;
        private clsOS materiel = new clsOS();
        int? newID = null;

        public frmOS()
        {
            InitializeComponent();
        }

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

        private void BindingCls()
        {
            SetBindingControls(txtId, "Text", materiel, "Id");
            SetBindingControls(txtDesignation, "Text", materiel, "Designation");
            SetBindingControls(cboTypeOS, "SelectedValue", materiel, "Id_type_OS");
            SetBindingControls(cboArchitectureOS, "SelectedValue", materiel, "Id_architecture_OS");
            SetBindingControls(txtCreateBy, "Text", materiel, "User_created");
            SetBindingControls(txtDateCreate, "Text", materiel, "Date_created");
            SetBindingControls(txtModifieBy, "Text", materiel, "User_modified");
            SetBindingControls(txtDateModifie, "Text", materiel, "Date_modified");
        }

        private void BindingList()
        {
            SetBindingControls(txtId, "Text", bdsrc, "Id");
            SetBindingControls(txtDesignation, "Text", bdsrc, "Designation");
            SetBindingControls(cboTypeOS, "SelectedValue", bdsrc, "Id_type_OS");
            SetBindingControls(cboArchitectureOS, "SelectedValue", bdsrc, "Id_architecture_OS");
            SetBindingControls(txtCreateBy, "Text", bdsrc, "User_created");
            SetBindingControls(txtDateCreate, "Text", bdsrc, "Date_created");
            SetBindingControls(txtModifieBy, "Text", bdsrc, "User_modified");
            SetBindingControls(txtDateModifie, "Text", bdsrc, "Date_modified");
        }
        #endregion

        public void SetDataSource(BindingSource bdsrc)
        {
            this.bdNav.BindingSource = bdsrc;
        }

        private void frmCategorieMateriel_Load(object sender, EventArgs e)
        {
            this.bdNav.Enabled = true;
            smartManage.Desktop.Properties.Settings.Default.strFormModifieOrdinateur = "";

            try
            {
                RefreshData();
            }
            catch (Exception)
            {
                MessageBox.Show("Erreur lors du chargement des données", "Erreur de chargement des données", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void SetSelectedIndexComboBox(List<ComboBox> cbo)
        {
            foreach (ComboBox cmb in cbo)
                if (cmb.Items.Count > 0)
                    cmb.SelectedIndex = 0;
        }

        private void CallActualiseComboBoxModification()
        {
            //Actualisation des combobox si modification
            if (!string.IsNullOrEmpty(smartManage.Desktop.Properties.Settings.Default.strFormModifieOS))
            {
                if (smartManage.Desktop.Properties.Settings.Default.strFormModifieOS.Equals(FormActualisation.frmTypeOS.ToString()))
                {
                    cboTypeOS.DataSource = clsMetier.GetInstance().getAllClstype_OS();
                    this.setMembersallcbo(cboTypeOS, "Designation", "Id");
                }
                else if (smartManage.Desktop.Properties.Settings.Default.strFormModifieOS.Equals(FormActualisation.frmArchitectureOS.ToString()))
                {
                    cboArchitectureOS.DataSource = clsMetier.GetInstance().getAllClsarchitecture_OS();
                    this.setMembersallcbo(cboArchitectureOS, "Designation", "Id");
                }

                smartManage.Desktop.Properties.Settings.Default.strFormModifieOS = "";
            }
        }

        private void RefreshData()
        {
            bdsrc.DataSource = clsMetier.GetInstance().getAllClsOS();
            this.SetDataSource(bdsrc);
            dgv.DataSource = bdsrc;

            cboTypeOS.DataSource = clsMetier.GetInstance().getAllClstype_OS();
            this.setMembersallcbo(cboTypeOS, "Designation", "Id");
            cboArchitectureOS.DataSource = clsMetier.GetInstance().getAllClsarchitecture_OS();
            this.setMembersallcbo(cboArchitectureOS, "Designation", "Id");

            List<ComboBox> lstCombo = new List<ComboBox>() { cboTypeOS, cboArchitectureOS };

            SetSelectedIndexComboBox(lstCombo);

            if (bdsrc.Count == 0)
            {
                bdDelete.Enabled = false;
                bdSave.Enabled = false;
            }
        }

        private void New()
        {
            //Initialise object class
            materiel = new clsOS();

            bdSave.Enabled = true;
            blnModifie = false;

            BindingCls();

            //Set the new ID
            if (newID == null)
                newID = clsMetier.GetInstance().GenerateLastID("OS");
            txtId.Text = newID.ToString();
            txtCreateBy.Text = smartManage.Desktop.Properties.Settings.Default.UserConnected;
            txtDateCreate.Text = DateTime.Now.ToString();
        }

        private void Search(string criteria)
        {
            if (string.IsNullOrEmpty(criteria))
            {
                this.RefreshRec();
                return;
            }                
            else
            {
                List<clsOS> lstItemSearch = new List<clsOS>();
                lstItemSearch = clsMetier.GetInstance().getAllClsOS(criteria);

                dgv.DataSource = lstItemSearch;
            }
        }

        private void Save()
        {
            if (!blnModifie)
            {
                int record = materiel.inserts();
                MessageBox.Show("Enregistrement éffectué : " + record + " Affecté", "Enregistrement", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                UpdateRec();
            }
        }

        private void UpdateRec()
        {
            ((clsOS)bdsrc.Current).User_modified = smartManage.Desktop.Properties.Settings.Default.UserConnected;
            ((clsOS)bdsrc.Current).Date_modified = DateTime.Now;

            int record = materiel.update(((clsOS)bdsrc.Current));
            MessageBox.Show("Modification éffectuée : " + record + " Modifié", "Modification", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Delete()
        {
            if (blnModifie)
            {
                DialogResult dr = MessageBox.Show("Voulez-vous supprimer cet enrgistrement ?", "Suppression enregistrement", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                int record = 0;

                if (dr == DialogResult.Yes)
                {
                    record = materiel.delete(((clsOS)bdsrc.Current));
                    MessageBox.Show("Suppression éffectuée : " + record + " Supprimé", "Suppression enregistrement", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    newID = null;
                    smartManage.Desktop.Properties.Settings.Default.strFormModifieOrdinateur = this.Name;
                    RefreshData();
                }
                else
                    MessageBox.Show("Aucune suppression éffectuée : " + record + " Supprimé", "Suppression enregistrement", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void RefreshRec()
        {
            RefreshData();
        }

        private void dgv_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                BindingList();

                blnModifie = true;
                bdDelete.Enabled = true;
            }
            catch (Exception)
            {
                blnModifie = false;
                bdDelete.Enabled = false;
            }
        }

        private void bdRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                this.RefreshRec();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'actualisation, " + ex.Message, "Actualisation des données", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                try
                {
                    this.Search(txtSearch.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de la recherche, " + ex.Message, "Recherche élément", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtSearch.Text))
                    this.RefreshRec();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur inattendu, " + ex.Message + ", veuillez actualiser les formulaire svp", "Besoin d'actualisation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void bdSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.Search(txtSearch.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la recherche, " + ex.Message, "Recherche élément", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void bdNew_Click(object sender, EventArgs e)
        {
            try
            {
                this.New();
            }
            catch (Exception)
            {
                bdSave.Enabled = false;
            }
        }

        private void bdSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.Save();
                smartManage.Desktop.Properties.Settings.Default.strFormModifieOrdinateur = this.Name;
                newID = null;
                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Echec de la mise à jour, " + ex.Message, "Mise à jour", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void bdDelete_Click(object sender, EventArgs e)
        {
            try
            {
                this.Delete();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Echec de la suppression, " + ex.Message, "Suppression enregistrement", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void lblAddTypeOS_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmTypeOS frm = new frmTypeOS();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddArchitectureOS_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmArchitectureOS frm = new frmArchitectureOS();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void cboTypeOS_DropDown(object sender, EventArgs e)
        {            
            try
            {
                CallActualiseComboBoxModification();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Echec d'actualisation de la liste déroulante, {0}", ex.Message), "Actualisation liste déroulante", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void cboArchitectureOS_DropDown(object sender, EventArgs e)
        {
            try
            {
                CallActualiseComboBoxModification();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Echec d'actualisation de la liste déroulante, {0}", ex.Message), "Actualisation liste déroulante", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
