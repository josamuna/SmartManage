using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace smartManage.Desktop
{
    public partial class frmReportLieuAffectation : Form
    {
        ResourceManager stringManager = null;
        public frmReportLieuAffectation()
        {
            InitializeComponent();
            //Initialisation des Resources
            Assembly _assembly = Assembly.Load("ResourcesData");
            stringManager = new ResourceManager("ResourcesData.Resource", _assembly);
        }
    }
}
