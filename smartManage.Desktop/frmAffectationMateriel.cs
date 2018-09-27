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
    public partial class frmAffectationMateriel : Form, ICRUDGeneral, ICallMainForm
    {
        public frmAffectationMateriel()
        {
            InitializeComponent();
        }

        public frmPrincipal Principal
        {
            get; set;
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public void New()
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

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Search(string criteria)
        {
            throw new NotImplementedException();
        }

        public void UpdateRec()
        {
            throw new NotImplementedException();
        }
    }
}
