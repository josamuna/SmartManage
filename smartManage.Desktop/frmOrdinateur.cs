using ManageQRCode;
using smartManage.Model;
using smartManage.Tools;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace smartManage.Desktop
{
    public partial class frmOrdinateur : Form, ICRUDGeneral, ICallMainForm
    {
        BindingSource bdsrc = new BindingSource();
        bool blnModifie = false;
        private clsmateriel materiel = new clsmateriel();
        int? newID = null;

        public frmPrincipal Principal
        {
            get;
            set;
        }

        public frmOrdinateur()
        {
            InitializeComponent();
        }

        #region FOR GENERATE QRCode
        public string SaveTempImage(System.Windows.Forms.PictureBox pbox)
        {
            string filename = Environment.GetEnvironmentVariables()["TEMP"].ToString() + @"\" + "fTmp" + DateTime.Now.Millisecond.ToString() + ".png";

            using (System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite))
            {
                System.Drawing.Imaging.ImageFormat imageFormat = System.Drawing.Imaging.ImageFormat.Png;
                pbox.Image.Save(fs, imageFormat);
                fs.Close();
            }

            return filename;
        }

        public void RemoveTempImage(string fileName)
        {
            if (System.IO.File.Exists(fileName))
            {
                System.IO.File.Delete(fileName);
            }
        }
        #endregion

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

        private void Bs_Parse1(object sender, ConvertEventArgs e)
        {
            try
            {
                if (e.Value == null) e = null;
                else e.Value = (new clsTools().ImageToString64(pbPhoto1.Image));
            }
            catch { }
        }

        private void Bs_Parse2(object sender, ConvertEventArgs e)
        {
            try
            {
                if (e.Value == null) e = null;
                else e.Value = (new clsTools().ImageToString64(pbPhoto2.Image));
            }
            catch { }
        }

        private void Bs_Parse3(object sender, ConvertEventArgs e)
        {
            try
            {
                if (e.Value == null) e = null;
                else e.Value = (new clsTools().ImageToString64(pbPhoto3.Image));
            }
            catch { }
        }
        void binding_Format1(object sender, ConvertEventArgs e)
        {
            try
            {
                pbPhoto1.Tag = null;
                if (e.DesiredType != typeof(System.Drawing.Image) || e.Value.ToString() == e.DesiredType.FullName || e.Value.ToString() == e.DesiredType.Name) return;
                if (e.Value.ToString() == "System.Drawing.Bitmap") return;
                if (e.Value == null || e.Value.ToString() == "")
                {
                    pbPhoto1.Tag = "1";
                }
                else
                {
                    string imagestr = e.Value.ToString();
                    e.Value = (new clsTools().LoadImage(e.Value.ToString()));
                }
            }
            catch { }
        }

        void binding_Format2(object sender, ConvertEventArgs e)
        {
            try
            {
                pbPhoto2.Tag = null;
                if (e.DesiredType != typeof(System.Drawing.Image) || e.Value.ToString() == e.DesiredType.FullName || e.Value.ToString() == e.DesiredType.Name) return;
                if (e.Value.ToString() == "System.Drawing.Bitmap") return;
                if (e.Value == null || e.Value.ToString() == "")
                {
                    pbPhoto2.Tag = "1";
                }
                else
                {
                    string imagestr = e.Value.ToString();
                    e.Value = (new clsTools().LoadImage(e.Value.ToString()));
                }
            }
            catch { }
        }

        void binding_Format3(object sender, ConvertEventArgs e)
        {
            try
            {
                pbPhoto3.Tag = null;
                if (e.DesiredType != typeof(System.Drawing.Image) || e.Value.ToString() == e.DesiredType.FullName || e.Value.ToString() == e.DesiredType.Name) return;
                if (e.Value.ToString() == "System.Drawing.Bitmap") return;
                if (e.Value == null || e.Value.ToString() == "")
                {
                    pbPhoto3.Tag = "1";
                }
                else
                {
                    string imagestr = e.Value.ToString();
                    e.Value = (new clsTools().LoadImage(e.Value.ToString()));
                }
            }
            catch { }
        }

        private void bingImg1(PictureBox pb, Object src, string ctrProp, string obj)
        {
            pb.DataBindings.Clear();
            Binding binding = new Binding(ctrProp, src, obj, true, DataSourceUpdateMode.OnPropertyChanged);
            binding.Parse += new ConvertEventHandler(Bs_Parse1);
            binding.Format += new ConvertEventHandler(binding_Format1);
            pb.DataBindings.Add(binding);
        }

        private void bingImg2(PictureBox pb, Object src, string ctrProp, string obj)
        {
            pb.DataBindings.Clear();
            Binding binding = new Binding(ctrProp, src, obj, true, DataSourceUpdateMode.OnPropertyChanged);
            binding.Parse += new ConvertEventHandler(Bs_Parse2);
            binding.Format += new ConvertEventHandler(binding_Format2);
            pb.DataBindings.Add(binding);
        }

        private void bingImg3(PictureBox pb, Object src, string ctrProp, string obj)
        {
            pb.DataBindings.Clear();
            Binding binding = new Binding(ctrProp, src, obj, true, DataSourceUpdateMode.OnPropertyChanged);
            binding.Parse += new ConvertEventHandler(Bs_Parse3);
            binding.Format += new ConvertEventHandler(binding_Format3);
            pb.DataBindings.Add(binding);
        }

        private void BindingCls()
        {
            SetBindingControls(txtId, "Text", materiel, "Id");
            SetBindingControls(txtIdentidiant, "Text", materiel, "Code_str");
            SetBindingControls(cboCatMateriel, "SelectedValue", materiel, "Id_categorie_materiel");
            SetBindingControls(cboNumCompte, "SelectedValue", materiel, "Id_compte");
            SetBindingControls(txtQRCode, "Text", materiel, "Qrcode");
            SetBindingControls(txtDateAcquisition, "Text", materiel, "Date_acquisition");
            SetBindingControls(cboGuarantie, "Text", materiel, "Guarantie");
            SetBindingControls(cboMarque, "SelectedValue", materiel, "Id_marque");
            SetBindingControls(cboModele, "SelectedValue", materiel, "Id_modele");
            SetBindingControls(cboCouleur, "SelectedValue", materiel, "Id_couleur");
            SetBindingControls(cboPoids, "SelectedValue", materiel, "Id_poids");
            SetBindingControls(cboEtat, "SelectedValue", materiel, "Id_etat_materiel");
            bingImg1(pbPhoto1, materiel, "Image", "Photo1");
            bingImg2(pbPhoto2, materiel, "Image", "Photo2");
            bingImg3(pbPhoto3, materiel, "Image", "Photo3");
            SetBindingControls(txtMAC1, "Text", materiel, "Mac_adresse1");
            SetBindingControls(txtMAC2, "Text", materiel, "Mac_adresse2");
            SetBindingControls(txtCommentaire, "Text", materiel, "Commentaire");
            SetBindingControls(txtCreateBy, "Text", materiel, "User_created");
            SetBindingControls(txtDateCreate, "Text", materiel, "Date_created");
            SetBindingControls(txtModifieBy, "Text", materiel, "User_modified");
            SetBindingControls(txtDateModifie, "Text", materiel, "Date_modified");
            
            //Partie pour ordinateur
            SetBindingControls(cboTypeOrdi, "SelectedValue", materiel, "Id_type_ordinateur");
            SetBindingControls(cboTypeClavier, "SelectedValue", materiel, "Id_type_clavier");
            SetBindingControls(cboTypeOS, "SelectedValue", materiel, "Id_os");
            SetBindingControls(cboRAM, "Text", materiel, "Ram");
            SetBindingControls(cboProcesseur, "Text", materiel, "Processeur");
            SetBindingControls(cboNbrCoeurProcesseur, "SelectedValue", materiel, "Nombre_coeur_processeur");
            SetBindingControls(cboTypeHDD, "SelectedValue", materiel, "Nombre_hdd");
            SetBindingControls(cboCapaciteHDD, "SelectedValue", materiel, "Capacite_hdd");
            SetBindingControls(cboNbrHDD, "SelectedValue", materiel, "Indice_performance");
            SetBindingControls(cboTailleEcran, "Text", materiel, "Pouce");
            SetBindingControls(cboUSB2, "SelectedValue", materiel, "Nombre_usb2");
            SetBindingControls(cboUSB3, "SelectedValue", materiel, "Nombre_usb3");
            SetBindingControls(cboNbrHDMI, "SelectedValue", materiel, "Nombre_hdmi");
            SetBindingControls(cboNbrVGA, "SelectedValue", materiel, "Nombre_vga");
            SetBindingControls(cboTensionBatt, "SelectedValue", materiel, "Tension_batterie");
            SetBindingControls(cboTensionAdap, "SelectedValue", materiel, "Tension_adaptateur");
            SetBindingControls(cboPuissanceAdap, "SelectedValue", materiel, "Puissance_adaptateur");
            SetBindingControls(cboIntensiteAdap, "SelectedValue", materiel, "Intensite_adaptateur");
            SetBindingControls(txtNumeroCle, "Text", materiel, "Numero_cle");
        }

        private void BindingList()
        {
            SetBindingControls(txtId, "Text", bdsrc, "Id");
            SetBindingControls(txtIdentidiant, "Text", bdsrc, "Code_str");
            SetBindingControls(cboCatMateriel, "SelectedValue", bdsrc, "Id_categorie_materiel");
            SetBindingControls(cboNumCompte, "SelectedValue", bdsrc, "Id_compte");
            SetBindingControls(txtQRCode, "Text", bdsrc, "Qrcode");
            SetBindingControls(txtDateAcquisition, "Text", bdsrc, "Date_acquisition");
            SetBindingControls(cboGuarantie, "Text", bdsrc, "Guarantie");
            SetBindingControls(cboMarque, "SelectedValue", bdsrc, "Id_marque");
            SetBindingControls(cboModele, "SelectedValue", bdsrc, "Id_modele");
            SetBindingControls(cboCouleur, "SelectedValue", bdsrc, "Id_couleur");
            SetBindingControls(cboPoids, "SelectedValue", bdsrc, "Id_poids");
            SetBindingControls(cboEtat, "SelectedValue", bdsrc, "Id_etat_materiel");
            bingImg1(pbPhoto1, bdsrc, "Image", "Photo1");
            bingImg2(pbPhoto2, bdsrc, "Image", "Photo2");
            bingImg3(pbPhoto3, bdsrc, "Image", "Photo3");
            SetBindingControls(txtMAC1, "Text", bdsrc, "Mac_adresse1");
            SetBindingControls(txtMAC2, "Text", bdsrc, "Mac_adresse2");
            SetBindingControls(txtCommentaire, "Text", bdsrc, "Commentaire");
            SetBindingControls(txtCreateBy, "Text", bdsrc, "User_created");
            SetBindingControls(txtDateCreate, "Text", bdsrc, "Date_created");
            SetBindingControls(txtModifieBy, "Text", bdsrc, "User_modified");
            SetBindingControls(txtDateModifie, "Text", bdsrc, "Date_modified");

            //Partie pour ordinateur
            SetBindingControls(cboTypeOrdi, "SelectedValue", bdsrc, "Id_type_ordinateur");
            SetBindingControls(cboTypeClavier, "SelectedValue", bdsrc, "Id_type_clavier");
            SetBindingControls(cboTypeOS, "SelectedValue", bdsrc, "Id_os");
            SetBindingControls(cboRAM, "Text", bdsrc, "Ram");
            SetBindingControls(cboProcesseur, "Text", bdsrc, "Processeur");
            SetBindingControls(cboNbrCoeurProcesseur, "SelectedValue", bdsrc, "Nombre_coeur_processeur");
            SetBindingControls(cboTypeHDD, "SelectedValue", bdsrc, "Nombre_hdd");
            SetBindingControls(cboCapaciteHDD, "SelectedValue", bdsrc, "Capacite_hdd");
            SetBindingControls(cboNbrHDD, "SelectedValue", bdsrc, "Indice_performance");
            SetBindingControls(cboTailleEcran, "Text", bdsrc, "Pouce");
            SetBindingControls(cboUSB2, "SelectedValue", bdsrc, "Nombre_usb2");
            SetBindingControls(cboUSB3, "SelectedValue", bdsrc, "Nombre_usb3");
            SetBindingControls(cboNbrHDMI, "SelectedValue", bdsrc, "Nombre_hdmi");
            SetBindingControls(cboNbrVGA, "SelectedValue", bdsrc, "Nombre_vga");
            SetBindingControls(cboTensionBatt, "SelectedValue", bdsrc, "Tension_batterie");
            SetBindingControls(cboTensionAdap, "SelectedValue", bdsrc, "Tension_adaptateur");
            SetBindingControls(cboPuissanceAdap, "SelectedValue", bdsrc, "Puissance_adaptateur");
            SetBindingControls(cboIntensiteAdap, "SelectedValue", bdsrc, "Intensite_adaptateur");
            SetBindingControls(txtNumeroCle, "Text", bdsrc, "Numero_cle");
        }
        #endregion

        private void frmOrdinateur_Load(object sender, EventArgs e)
        {
            //try
            //{
                RefreshData();

                List<ComboBox> lstCombo = new List<ComboBox>();
                ComboBox[] tbCbo = { cboCatMateriel, cboNumCompte, cboMarque, cboCouleur, cboPoids, cboEtat,
                    cboTypeOrdi, cboTypeClavier, cboTypeOS, cboNbrCoeurProcesseur, cboTypeHDD, cboCapaciteHDD,
                    cboNbrHDD , cboUSB2, cboUSB3, cboNbrHDMI, cboNbrVGA, cboTensionBatt, cboPuissanceAdap,
                    cboTensionAdap, cboIntensiteAdap};
                lstCombo.Add(cboCatMateriel);
                lstCombo.Add(cboNumCompte);
                lstCombo.Add(cboMarque);
                lstCombo.Add(cboModele);
                lstCombo.Add(cboCouleur);
                lstCombo.Add(cboPoids);
                lstCombo.Add(cboEtat);
                lstCombo.Add(cboTypeOrdi);
                lstCombo.Add(cboTypeClavier);
                lstCombo.Add(cboTypeOS);
                lstCombo.Add(cboNbrCoeurProcesseur);
                lstCombo.Add(cboTypeHDD);
                lstCombo.Add(cboCapaciteHDD);
                lstCombo.Add(cboNbrHDD);
                lstCombo.Add(cboUSB2);
                lstCombo.Add(cboUSB3);
                lstCombo.Add(cboNbrHDMI);
                lstCombo.Add(cboNbrVGA);
                lstCombo.Add(cboTensionBatt);
                lstCombo.Add(cboPuissanceAdap);
                lstCombo.Add(cboTensionAdap);
                lstCombo.Add(cboIntensiteAdap);

                SetSelectedIndexComboBox(lstCombo);
            //}
            //catch (Exception)
            //{
            //    MessageBox.Show("Erreur lors du chargement des données", "Erreur de chargement des données", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //}
        }

        private void SetSelectedIndexComboBox(List<ComboBox> cbo)
        {
            foreach (ComboBox cmb in cbo)
                if (cmb.Items.Count > 0)
                    cmb.SelectedIndex = 0;
        }

        private void RefreshData()
        {
            bdsrc.DataSource = clsMetier.GetInstance().getAllClsmateriel();
            Principal.SetDataSource(bdsrc);

            dgv.DataSource = bdsrc;

            cboCatMateriel.DataSource = clsMetier.GetInstance().getAllClscategorie_materiel();
            this.setMembersallcbo(cboCatMateriel, "Designation", "Id");
            cboNumCompte.DataSource = clsMetier.GetInstance().getAllClscompte();
            this.setMembersallcbo(cboNumCompte, "Numero", "Id");
            cboMarque.DataSource = clsMetier.GetInstance().getAllClsmarque();
            this.setMembersallcbo(cboMarque, "Designation", "Id");
            cboModele.DataSource = clsMetier.GetInstance().getAllClsmodele();
            this.setMembersallcbo(cboModele, "Designation", "Id");
            cboCouleur.DataSource = clsMetier.GetInstance().getAllClscouleur();
            this.setMembersallcbo(cboCouleur, "Designation", "Id");
            cboPoids.DataSource = clsMetier.GetInstance().getAllClspoids();
            this.setMembersallcbo(cboPoids, "Valeur", "Id");
            cboEtat.DataSource = clsMetier.GetInstance().getAllClsetat_materiel();
            this.setMembersallcbo(cboEtat, "Designation", "Id");
            cboTypeOrdi.DataSource = clsMetier.GetInstance().getAllClstype_ordinateur();
            this.setMembersallcbo(cboTypeOrdi, "Designation", "Id");
            cboTypeClavier.DataSource = clsMetier.GetInstance().getAllClstype_clavier();
            this.setMembersallcbo(cboTypeClavier, "Designation", "Id");
            cboTypeOS.DataSource = clsMetier.GetInstance().getAllClstype_OS();
            this.setMembersallcbo(cboTypeOS, "Designation", "Id");

            List<string> lstNbrCoeurProcesseur = new List<string>();
            lstNbrCoeurProcesseur.Add("1");
            lstNbrCoeurProcesseur.Add("2");
            lstNbrCoeurProcesseur.Add("3");
            lstNbrCoeurProcesseur.Add("4");
            lstNbrCoeurProcesseur.Add("5");
            lstNbrCoeurProcesseur.Add("6");
            lstNbrCoeurProcesseur.Add("7");

            cboNbrCoeurProcesseur.DataSource = lstNbrCoeurProcesseur;
            this.setMembersallcbo(cboNbrCoeurProcesseur, "Nombre_coeur_processeur", "Nombre_coeur_processeur");

            List<string> lstNbrHDD = new List<string>();
            lstNbrHDD.Add("1");
            lstNbrHDD.Add("2");
            lstNbrHDD.Add("3");
            cboTypeHDD.DataSource = lstNbrHDD;
            this.setMembersallcbo(cboNbrCoeurProcesseur, "Nombre_hdd", "Nombre_hdd");

            List<string> lstCapaciteHDD = new List<string>(); 
            lstCapaciteHDD.Add("38");
            lstCapaciteHDD.Add("74");
            lstCapaciteHDD.Add("233");
            lstCapaciteHDD.Add("300");
            lstCapaciteHDD.Add("450");
            lstCapaciteHDD.Add("465");
            lstCapaciteHDD.Add("500");
            cboCapaciteHDD.DataSource = lstCapaciteHDD;
            this.setMembersallcbo(cboNbrCoeurProcesseur, "Capacite_hdd", "Capacite_hdd");

            List<string> lstIndicePC = new List<string>();
            lstIndicePC.Add("1");
            lstIndicePC.Add("3,1");
            lstIndicePC.Add("3,2");
            lstIndicePC.Add("3,3");
            lstIndicePC.Add("4,4");
            lstIndicePC.Add("4,5");
            cboNbrHDD.DataSource = lstIndicePC;
            this.setMembersallcbo(cboNbrCoeurProcesseur, "Indice_performance", "Indice_performance");

            List<string> lstUSB2 = new List<string>();
            lstUSB2.Add("0");
            lstUSB2.Add("1");
            lstUSB2.Add("2");
            lstUSB2.Add("3");
            lstUSB2.Add("4");
            cboUSB2.DataSource = lstUSB2;
            this.setMembersallcbo(cboNbrCoeurProcesseur, "Nombre_usb2", "Nombre_usb2");

            List<string> lstUSB3 = new List<string>();
            lstUSB3.Add("0");
            lstUSB3.Add("1");
            lstUSB3.Add("2");
            lstUSB3.Add("3");
            cboUSB3.DataSource = lstUSB3;
            this.setMembersallcbo(cboNbrCoeurProcesseur, "Nombre_usb3", "Nombre_usb3");

            List<string> lstNbrHDMI = new List<string>();
            lstNbrHDMI.Add("0");
            lstNbrHDMI.Add("1");
            lstNbrHDMI.Add("2");
            lstNbrHDMI.Add("3");
            cboNbrHDMI.DataSource = lstNbrHDMI;
            this.setMembersallcbo(cboNbrCoeurProcesseur, "Nombre_hdmi", "Nombre_hdmi");

            List<string> lstNbrVGA = new List<string>();
            lstNbrVGA.Add("1");
            lstNbrVGA.Add("2");
            lstNbrVGA.Add("3");
            cboNbrVGA.DataSource = lstNbrVGA;
            this.setMembersallcbo(cboNbrCoeurProcesseur, "Nombre_vga", "Nombre_vga");

            List<string> lstTensionBatt = new List<string>();
            lstTensionBatt.Add("10,8");
            lstTensionBatt.Add("12");
            cboTensionBatt.DataSource = lstTensionBatt;
            this.setMembersallcbo(cboNbrCoeurProcesseur, "Tension_batterie", "Tension_batterie");

            List<string> lstPuissanceAdap = new List<string>();
            lstPuissanceAdap.Add("10,8");
            lstPuissanceAdap.Add("12");
            cboPuissanceAdap.DataSource = lstPuissanceAdap;
            this.setMembersallcbo(cboNbrCoeurProcesseur, "Puissance_adaptateur", "Puissance_adaptateur");

            List<string> lstTensionAdap = new List<string>();
            lstTensionAdap.Add("15");
            lstTensionAdap.Add("18,5");
            lstTensionAdap.Add("19");
            lstTensionAdap.Add("19,5");
            cboTensionAdap.DataSource = lstTensionAdap;
            this.setMembersallcbo(cboNbrCoeurProcesseur, "Tension_adaptateur", "Tension_adaptateur");

            List<string> lstIntensiteAdap = new List<string>();
            lstIntensiteAdap.Add("3,33");
            lstIntensiteAdap.Add("3,5");
            lstIntensiteAdap.Add("5");
            cboIntensiteAdap.DataSource = lstIntensiteAdap;
            this.setMembersallcbo(cboNbrCoeurProcesseur, "Intensite_adaptateur", "Intensite_adaptateur");

            if (bdsrc.Count == 0)
            {
                Principal.ActivateOnLoadCommandButtons(false);
            }
        }

        private void frmOrdinateur_Activated(object sender, EventArgs e)
        {
            Principal.SetValuesLabel(Properties.Settings.Default.UserConnected, "Gestion des ordinateurs");
            Principal.SetCurrentICRUDChildForm(this);
            frmOrdinateur_Load(sender, e);
        }

        private void ActualiseComboBoxModification()
        {
            //Actualisation des combobox si modification
            
            if (!string.IsNullOrEmpty(smartManage.Desktop.Properties.Settings.Default.strFormModifie))
            {
                if (smartManage.Desktop.Properties.Settings.Default.strFormModifie.Equals(FormActualisation.frmCategorieMateriel.ToString()))
                {
                    cboCatMateriel.DataSource = clsMetier.GetInstance().getAllClscategorie_materiel();
                    this.setMembersallcbo(cboCatMateriel, "Designation", "Id");
                }
                else if (smartManage.Desktop.Properties.Settings.Default.strFormModifie.Equals(FormActualisation.frmNumeroCompte.ToString()))
                {
                    cboNumCompte.DataSource = clsMetier.GetInstance().getAllClscompte();
                    this.setMembersallcbo(cboNumCompte, "Numero", "Id");
                }
                else if (smartManage.Desktop.Properties.Settings.Default.strFormModifie.Equals(FormActualisation.frmMarque.ToString()))
                {
                    cboMarque.DataSource = clsMetier.GetInstance().getAllClsmarque();
                    this.setMembersallcbo(cboMarque, "Designation", "Id");
                }
                else if (smartManage.Desktop.Properties.Settings.Default.strFormModifie.Equals(FormActualisation.frmModele.ToString()))
                {
                    cboModele.DataSource = clsMetier.GetInstance().getAllClsmodele();
                    this.setMembersallcbo(cboModele, "Designation", "Id");
                }
                else if (smartManage.Desktop.Properties.Settings.Default.strFormModifie.Equals(FormActualisation.frmCouleur.ToString()))
                {
                    cboCouleur.DataSource = clsMetier.GetInstance().getAllClscouleur();
                    this.setMembersallcbo(cboCouleur, "Designation", "Id");
                }
                else if (smartManage.Desktop.Properties.Settings.Default.strFormModifie.Equals(FormActualisation.frmPoids.ToString()))
                {
                    cboPoids.DataSource = clsMetier.GetInstance().getAllClspoids();
                    this.setMembersallcbo(cboPoids, "Valeur", "Id");
                }
                else if (smartManage.Desktop.Properties.Settings.Default.strFormModifie.Equals(FormActualisation.frmEtatMateriel.ToString()))
                {
                    cboEtat.DataSource = clsMetier.GetInstance().getAllClsetat_materiel();
                    this.setMembersallcbo(cboEtat, "Designation", "Id");
                }
                else if (smartManage.Desktop.Properties.Settings.Default.strFormModifie.Equals(FormActualisation.frmTypeOrdinateur.ToString()))
                {
                    cboTypeOrdi.DataSource = clsMetier.GetInstance().getAllClstype_ordinateur();
                    this.setMembersallcbo(cboTypeOrdi, "Designation", "Id");
                }
                else if (smartManage.Desktop.Properties.Settings.Default.strFormModifie.Equals(FormActualisation.frmTypeClavier.ToString()))
                {
                    cboTypeClavier.DataSource = clsMetier.GetInstance().getAllClstype_clavier();
                    this.setMembersallcbo(cboTypeClavier, "Designation", "Id");
                }
                else if (smartManage.Desktop.Properties.Settings.Default.strFormModifie.Equals(FormActualisation.frmTypeOS.ToString()))
                {
                    cboTypeOS.DataSource = clsMetier.GetInstance().getAllClstype_OS();
                    this.setMembersallcbo(cboTypeOS, "Designation", "Id");
                }
            }

            smartManage.Desktop.Properties.Settings.Default.strFormModifie = "";
        }

        private void frmOrdinateur_FormClosed(object sender, FormClosedEventArgs e)
        {
            Principal.SetValuesLabel(Properties.Settings.Default.UserConnected, "Attente d'une action de l'utilisateur");
        }

        public void New()
        {
            try
            {
                //Initialise object class
                materiel = new clsmateriel();

                Principal.ActivateOnNewCommandButtons(true);
                blnModifie = false;

                BindingCls();

                //Set the new ID
                if (newID == null)
                    newID = clsMetier.GetInstance().GenerateLastID("materiel");
                txtId.Text = newID.ToString();
                txtCreateBy.Text = smartManage.Desktop.Properties.Settings.Default.UserConnected;
                txtDateCreate.Text = DateTime.Now.ToString();
            }
            catch (Exception)
            {
                Principal.ActivateOnNewCommandButtons(false);
            }
        }

        public void Search(string criteria)
        {
            try
            {
                if (string.IsNullOrEmpty(criteria))
                {
                    this.RefreshRec();
                    return;
                }
                else
                {
                    List<clsmateriel> lstItemSearch = new List<clsmateriel>();
                    lstItemSearch = clsMetier.GetInstance().getAllClsmateriel(criteria);

                    dgv.DataSource = lstItemSearch;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la recherche, " + ex.Message, "Recherche élément", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public void Save()
        {
            try
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

                newID = null;
                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Echec de la mise à jour, " + ex.Message, "Mise à jour", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public void UpdateRec()
        {
            ((clsmateriel)bdsrc.Current).User_modified = smartManage.Desktop.Properties.Settings.Default.UserConnected;
            ((clsmateriel)bdsrc.Current).Date_modified = DateTime.Now;

            int record = materiel.update(((clsmateriel)bdsrc.Current));
            MessageBox.Show("Modification éffectuée : " + record + " Modifié", "Modification", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void Delete()
        {
            try
            {
                if (blnModifie)
                {
                    DialogResult dr = MessageBox.Show("Voulez-vous supprimer cet enrgistrement ?", "Suppression enregistrement", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    int record = 0;

                    if (dr == DialogResult.Yes)
                    {
                        record = materiel.delete(((clsmateriel)bdsrc.Current));
                        MessageBox.Show("Suppression éffectuée : " + record + " Supprimé", "Suppression enregistrement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        newID = null;
                    }
                    else
                        MessageBox.Show("Aucune suppression éffectuée : " + record + " Supprimé", "Suppression enregistrement", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Echec de la suppression, " + ex.Message, "Suppression enregistrement", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public void Preview()
        {
            try
            {
                frmReport frm = new frmReport();
                frm.Text = "Rapports pour ordinateurs";
                frm.MdiParent = Principal;
                //frm.setData(factory.getAllSexe_Dt(), @"D:\appStockMS\appStock.Desktop\reports\rptListSexe.rdlc");
                frm.Icon = this.Icon;
                frm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Echec de la mise à jour, " + ex.Message, "Mise à jour", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public void RefreshRec()
        {
            try
            {
                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'actualisation, " + ex.Message, "Actualisation des données", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void dgv_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                BindingList();
                blnModifie = true;
                Principal.ActivateOnNewSelectionChangeDgvCommandButtons(true);
            }
            catch (Exception)
            {
                blnModifie = false;
                Principal.ActivateOnSelectionChangeDgvExceptionCommandButtons(false);
            }
        }

        private void lblAddCategorieMat_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmCategorieMateriel frm = new frmCategorieMateriel();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddNumCompte_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmNumeroCompte frm = new frmNumeroCompte();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void cboCatMateriel_DropDown(object sender, EventArgs e)
        {
            try
            {
                ActualiseComboBoxModification();
            }
            catch (Exception) { }
        }

        private void cboNumCompte_DropDown(object sender, EventArgs e)
        {
            try
            {
                ActualiseComboBoxModification();
            }
            catch (Exception) { }
        }

        private void lblAddMarque_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmMarque frm = new frmMarque();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddModele_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmModele frm = new frmModele();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void cboModele_DropDown(object sender, EventArgs e)
        {
            try
            {
                ActualiseComboBoxModification();
            }
            catch (Exception) { }
        }

        private void cboCouleur_DropDown(object sender, EventArgs e)
        {
            try
            {
                ActualiseComboBoxModification();
            }
            catch (Exception) { }
        }

        private void cboPoids_DropDown(object sender, EventArgs e)
        {
            try
            {
                ActualiseComboBoxModification();
            }
            catch (Exception) { }
        }

        private void cboEtat_DropDown(object sender, EventArgs e)
        {
            try
            {
                ActualiseComboBoxModification();
            }
            catch (Exception) { }
        }

        private void cboTypeOrdi_DropDown(object sender, EventArgs e)
        {
            try
            {
                ActualiseComboBoxModification();
            }
            catch (Exception) { }
        }

        private void cboTypeClavier_DropDown(object sender, EventArgs e)
        {
            try
            {
                ActualiseComboBoxModification();
            }
            catch (Exception) { }
        }

        private void cboTypeOS_DropDown(object sender, EventArgs e)
        {
            try
            {
                ActualiseComboBoxModification();
            }
            catch (Exception) { }
        }

        private void cboNbrCoeurProcesseur_DropDown(object sender, EventArgs e)
        {
            try
            {
                ActualiseComboBoxModification();
            }
            catch (Exception) { }
        }

        private void cboNbrHDD_DropDown(object sender, EventArgs e)
        {
            try
            {
                ActualiseComboBoxModification();
            }
            catch (Exception) { }
        }

        private void cboCapaciteHDD_DropDown(object sender, EventArgs e)
        {
            try
            {
                ActualiseComboBoxModification();
            }
            catch (Exception) { }
        }

        private void cboIndicePC_DropDown(object sender, EventArgs e)
        {
            try
            {
                ActualiseComboBoxModification();
            }
            catch (Exception) { }
        }

        private void cboUSB2_DropDown(object sender, EventArgs e)
        {
            try
            {
                ActualiseComboBoxModification();
            }
            catch (Exception) { }
        }

        private void cboUSB3_DropDown(object sender, EventArgs e)
        {
            try
            {
                ActualiseComboBoxModification();
            }
            catch (Exception) { }
        }

        private void cboNbrHDMI_DropDown(object sender, EventArgs e)
        {
            try
            {
                ActualiseComboBoxModification();
            }
            catch (Exception) { }
        }

        private void cboNbrVGA_DropDown(object sender, EventArgs e)
        {
            try
            {
                ActualiseComboBoxModification();
            }
            catch (Exception) { }
        }

        private void cboTensionBatt_DropDown(object sender, EventArgs e)
        {
            try
            {
                ActualiseComboBoxModification();
            }
            catch (Exception) { }
        }

        private void cboTensionAdap_DropDown(object sender, EventArgs e)
        {
            try
            {
                ActualiseComboBoxModification();
            }
            catch (Exception) { }
        }

        private void cboPuissanceAdap_DropDown(object sender, EventArgs e)
        {
            try
            {
                ActualiseComboBoxModification();
            }
            catch (Exception) { }
        }

        private void cboIntensiteAdap_DropDown(object sender, EventArgs e)
        {
            try
            {
                ActualiseComboBoxModification();
            }
            catch (Exception) { }
        }

        private void lblAddCouleur_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmCouleur frm = new frmCouleur();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddPoids_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmPoids frm = new frmPoids();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblEtatMatriel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmEtatMateriel frm = new frmEtatMateriel();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddPC_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmModele frm = new frmModele();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddTypeClavier_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmModele frm = new frmModele();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddOS_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmModele frm = new frmModele();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void cboCatMateriel_Leave(object sender, EventArgs e)
        {
            try
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(string.Format("{0}-{1}", txtId.Text, ((clscategorie_materiel)cboCatMateriel.SelectedItem).Id.ToString()));
                txtIdentidiant.Text = sb.ToString();

                //Creating QrCode 
                clsTools tool = new clsTools();
                System.Drawing.Image img = QRCodeImage.GetGenerateQRCode(txtIdentidiant.Text, "L", "", 0);//L, M ou Q
                pbQRCode.Image = img;

                //Convert PictureBox image to Base64 text
                //Save a temp image file
                string fileName = tool.SaveTempImage(pbQRCode);
                txtQRCode.Text = tool.ImageToString64(tool.GetImageFromByte(fileName));

                //Remove the temp image created
                tool.RemoveTempImage(fileName);
            }
            catch (Exception) { }
        }

        private void cboMarque_DropDown(object sender, EventArgs e)
        {
            try
            {
                ActualiseComboBoxModification();
            }
            catch (Exception) { }
        }

        private void lblPhoto1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                OpenFileDialog open;
                DialogResult result;
                LoadPicture(out open, out result);

                if (result == DialogResult.OK)
                {
                    pbPhoto1.Load(open.FileName);
                    materiel.Photo1 = new clsTools().ImageToString64(open.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Echec de chargement de la photo, " + ex.Message, "Chargement de la photo1", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private static void LoadPicture(out OpenFileDialog open, out DialogResult result)
        {
            open = new OpenFileDialog();
            open.Title = "Sélection d'une photo";
            open.Filter = "PNG Files(*.png)|*.png|JPG Files(*.jpg)|*.jpg|JPEG Files(*.jpeg)|*.jpeg|TIFF Files(*.tiff)|*.tiff";
            result = open.ShowDialog();
        }

        private void lblPhoto2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                OpenFileDialog open;
                DialogResult result;
                LoadPicture(out open, out result);

                if (result == DialogResult.OK)
                {
                    pbPhoto2.Load(open.FileName);
                    materiel.Photo2 = new clsTools().ImageToString64(open.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Echec de chargement de la photo, " + ex.Message, "Chargement de la photo2", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void lblPhoto3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                OpenFileDialog open;
                DialogResult result;
                LoadPicture(out open, out result);

                if (result == DialogResult.OK)
                {
                    pbPhoto3.Load(open.FileName);
                    materiel.Photo3 = new clsTools().ImageToString64(open.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Echec de chargement de la photo, " + ex.Message, "Chargement de la photo3", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
