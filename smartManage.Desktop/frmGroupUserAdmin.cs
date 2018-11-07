using smartManage.RadiusAdminModel;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace smartManage.Desktop
{
    public partial class frmGroupUserAdmin : Form
    {
        BindingSource bdsrc = new BindingSource();
        bool blnModifie = false;
        private clsradgroupcheck materiel = new clsradgroupcheck();
        int? newID = null;

        public frmGroupUserAdmin()
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
            SetBindingControls(txtDesignation, "Text", materiel, "Groupname");
            SetBindingControls(cboAttribut, "SelectedValue", materiel, "Attribute");
            SetBindingControls(cboOption, "SelectedValue", materiel, "Op");
            SetBindingControls(txtValue, "Text", materiel, "Value");
        }

        private void BindingList()
        {
            SetBindingControls(txtId, "Text", bdsrc, "Id");
            SetBindingControls(txtDesignation, "Text", bdsrc, "Groupname");
            SetBindingControls(cboAttribut, "SelectedValue", bdsrc, "Attribute");
            SetBindingControls(cboOption, "SelectedValue", bdsrc, "Op");
            SetBindingControls(txtValue, "Text", bdsrc, "Value");
        }
        #endregion

        public void SetDataSource(BindingSource bdsrc)
        {
            this.bdNav.BindingSource = bdsrc;
        }

        private void RefreshData()
        {
            bdsrc.DataSource = clsMetier1.GetInstance().getAllClsradgroupcheck();
            this.SetDataSource(bdsrc);

            Dictionary<string, string> dicoAttribut = new Dictionary<string, string>();
            Dictionary<string, string> dicoOption = new Dictionary<string, string>();

            dicoAttribut.Add("Simultaneous-Use", "Simultaneous-Use");
            dicoAttribut.Add("Max-Daily-Session", "Max-Daily-Session");

            dicoOption.Add(":=", ":=");

            cboAttribut.DataSource = new BindingSource(dicoAttribut, null);
            this.setMembersallcbo(cboAttribut, "Value", "Key");
            cboOption.DataSource = new BindingSource(dicoOption, null);
            this.setMembersallcbo(cboOption, "Value", "Key");

            dgv.DataSource = bdsrc;

            if (bdsrc.Count == 0)
            {
                bdDelete.Enabled = false;
                bdSave.Enabled = false;
            }
        }

        private void New()
        {
            //Initialise object class
            materiel = new clsradgroupcheck();

            bdSave.Enabled = true;
            blnModifie = false;

            BindingCls();

            //Set the new ID
            if (newID == null)
                newID = clsMetier1.GetInstance().GenerateLastID("radgroupcheck");
            txtId.Text = newID.ToString();
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
                List<clsradgroupcheck> lstItemSearch = new List<clsradgroupcheck>();
                lstItemSearch = clsMetier1.GetInstance().getAllClsradgroupcheck(criteria);

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
            int record = materiel.update(((clsradgroupcheck)bdsrc.Current));
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
                    record = materiel.delete(((clsradgroupcheck)bdsrc.Current));
                    MessageBox.Show("Suppression éffectuée : " + record + " Supprimé", "Suppression enregistrement", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    newID = null;
                    smartManage.Desktop.Properties.Settings.Default.strFormModifieSubForm = this.Name;
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
                smartManage.Desktop.Properties.Settings.Default.strFormModifieSubForm = this.Name;
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

        private void frmGroupUserAdmin_Load(object sender, EventArgs e)
        {
            this.bdNav.Enabled = true;
            smartManage.Desktop.Properties.Settings.Default.strFormModifieSubForm = "";

            try
            {
                RefreshData();
            }
            catch (Exception)
            {
                MessageBox.Show("Erreur lors du chargement des données", "Erreur de chargement des données", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
