using System;
using System.Windows.Forms;

namespace smartManage.Desktop
{
    public partial class frmSplash : Form
    {
        public frmSplash()
        {
            InitializeComponent();
            this.Cursor = Cursors.WaitCursor;
        }

        private void StartTimer(object sender, EventArgs e)
        {
            timerTemp.Enabled = false;
            this.Hide();

            frmPrincipal frm = new frmPrincipal();
            frm.Show();
        }

        private void timerTemp_Tick(object sender, EventArgs e)
        {
            timerTemp.Interval = 8000;
            timerTemp.Tick += new EventHandler(StartTimer);
        }

        private void frmSplash_Load(object sender, EventArgs e)
        {
            timerTemp.Enabled = true;
        }
    }
}
