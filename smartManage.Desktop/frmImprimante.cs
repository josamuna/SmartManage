using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace smartManage.Desktop
{
    public partial class frmImprimante : Form, ICRUDGeneral, ICallMainForm
    {
        BindingSource bdsrc = new BindingSource();

        public frmPrincipal Principal
        {
            get; set;
        }

        public frmImprimante()
        {
            InitializeComponent();
        }

        private void frmImprimante_Load(object sender, EventArgs e)
        {
            try
            {
                RefreshData();
                dgv.DataSource = bdsrc;
            }
            catch (Exception)
            {
                MessageBox.Show("Erreur lors du chargement", "Erreur de chargement des données", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private DataTable LoadDataSource()
        {
            DataTable dt = new DataTable();

            DataColumn colNom = dt.Columns.Add("Nom");

            DataRow row1 = dt.Rows.Add("Josue ISAMUNA");
            DataRow row2 = dt.Rows.Add("Erman KOKO");
            DataRow row3 = dt.Rows.Add("Potopoto SAMUNU");
            DataRow row4 = dt.Rows.Add("Niclette HELONI");
            DataRow row5 = dt.Rows.Add("Kasereka KASOLE");
            DataRow row6 = dt.Rows.Add("HP DeskJet 5020");
            DataRow row7 = dt.Rows.Add("Epson L830");
            DataRow row8 = dt.Rows.Add("Epson L220");
            DataRow row9 = dt.Rows.Add("HP 4059");
            DataRow row10 = dt.Rows.Add("HP Pro 20");

            return dt;
        }

        public void RefreshData()
        {
            bdsrc.DataSource = this.LoadDataSource();
            Principal.SetDataSource(bdsrc);

            //if (bdsrc.Count == 0)
            //{
            //    bdSave.Enabled = false;
            //    bdDelete.Enabled = false;
            //}
        }

        private void frmImprimante_Activated(object sender, EventArgs e)
        {
            Principal.SetCurrentICRUDChildForm(this);
            Principal.SetValuesLabel(Properties.Settings.Default.UserConnected, "Connected Appl Printer");
            frmImprimante_Load(sender, e);
        }

        private void frmImprimante_FormClosed(object sender, FormClosedEventArgs e)
        {
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
            MessageBox.Show("Save Printer", "Mis à jour", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void UpdateRec()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public void Preview()
        {
            throw new NotImplementedException();
        }

        public void RefreshRec()
        {
            throw new NotImplementedException();
        }
    }
}
