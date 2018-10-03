namespace smartManage.Desktop
{
    partial class frmOrdinateur
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCode_str = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colId_categorie_materiel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colId_compte = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQrcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDate_acquisition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGuarantie = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colId_marque = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colId_modele = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colId_couleur = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colId_poids = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colId_etat_materiel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPhoto1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPhoto2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPhoto3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLabel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMac_adresse1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMac_adresse2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCommentaire = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUser_created = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDate_created = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUser_modified = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDate_modified = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colId_type_ordinateur = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colId_type_clavier = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colId_os = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRam = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProcesseur = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNombre_coeur_processeur = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNombre_hdd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCapacite_hdd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIndice_performance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPouce = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNombre_usb2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNombre_usb3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNombre_hdmi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNombre_vga = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTension_batterie = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTension_adaptateur = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPuissance_adaptateur = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIntensite_adaptateur = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNumero_cle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colId_type_imprimante = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPuissance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIntensite = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNombre_page_par_minute = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colId_type_amplificateur = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTension_alimentation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNombre_usb = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNombre_memoire = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNombre_sorties_audio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNombre_microphone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGain = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colId_type_routeur_ap = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colId_version_ios = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNombre_gbe = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNombre_fe = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNombre_fo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNombre_serial = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCapable_usb = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMotpasse_defaut = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDefault_ip = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNombre_console = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNombre_auxiliaire = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colId_type_ap = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colId_type_switch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFrequence = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAlimentation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNombre_antenne = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colId_netette = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCompatible_wifi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cboMarque = new System.Windows.Forms.ComboBox();
            this.txtDateModifie = new System.Windows.Forms.TextBox();
            this.txtQRCode = new System.Windows.Forms.RichTextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtModifieBy = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.txtDateCreate = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.txtCreateBy = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.txtCommentaire = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtMAC2 = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.lblPhoto3 = new System.Windows.Forms.LinkLabel();
            this.lblPhoto2 = new System.Windows.Forms.LinkLabel();
            this.pbPhoto3 = new System.Windows.Forms.PictureBox();
            this.pbPhoto2 = new System.Windows.Forms.PictureBox();
            this.lblPhoto1 = new System.Windows.Forms.LinkLabel();
            this.pbPhoto1 = new System.Windows.Forms.PictureBox();
            this.txtMAC1 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.lblEtatMatriel = new System.Windows.Forms.LinkLabel();
            this.cboEtat = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.lblAddPoids = new System.Windows.Forms.LinkLabel();
            this.cboPoids = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.lblAddCouleur = new System.Windows.Forms.LinkLabel();
            this.cboCouleur = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.lblAddModele = new System.Windows.Forms.LinkLabel();
            this.cboModele = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.lblAddMarque = new System.Windows.Forms.LinkLabel();
            this.label9 = new System.Windows.Forms.Label();
            this.cboGuarantie = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtDateAcquisition = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblAddNumCompte = new System.Windows.Forms.LinkLabel();
            this.cboNumCompte = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtIdentidiant = new System.Windows.Forms.TextBox();
            this.lblAddCategorieMat = new System.Windows.Forms.LinkLabel();
            this.cboCatMateriel = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtId = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.pbQRCode = new System.Windows.Forms.PictureBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtNumeroCle = new System.Windows.Forms.TextBox();
            this.cboTensionAdap = new System.Windows.Forms.ComboBox();
            this.cboPuissanceAdap = new System.Windows.Forms.ComboBox();
            this.cboNbrHDMI = new System.Windows.Forms.ComboBox();
            this.cboIntensiteAdap = new System.Windows.Forms.ComboBox();
            this.cboNbrVGA = new System.Windows.Forms.ComboBox();
            this.label46 = new System.Windows.Forms.Label();
            this.label47 = new System.Windows.Forms.Label();
            this.label48 = new System.Windows.Forms.Label();
            this.label49 = new System.Windows.Forms.Label();
            this.cboTensionBatt = new System.Windows.Forms.ComboBox();
            this.label50 = new System.Windows.Forms.Label();
            this.label51 = new System.Windows.Forms.Label();
            this.label52 = new System.Windows.Forms.Label();
            this.cboTypeHDD = new System.Windows.Forms.ComboBox();
            this.cboNbrCoeurProcesseur = new System.Windows.Forms.ComboBox();
            this.lblAddTypeClavier = new System.Windows.Forms.LinkLabel();
            this.cboTypeClavier = new System.Windows.Forms.ComboBox();
            this.lblAddPC = new System.Windows.Forms.LinkLabel();
            this.cboTypeOrdi = new System.Windows.Forms.ComboBox();
            this.cboUSB3 = new System.Windows.Forms.ComboBox();
            this.label21 = new System.Windows.Forms.Label();
            this.cboUSB2 = new System.Windows.Forms.ComboBox();
            this.label22 = new System.Windows.Forms.Label();
            this.cboNbrHDD = new System.Windows.Forms.ComboBox();
            this.label24 = new System.Windows.Forms.Label();
            this.cboCapaciteHDD = new System.Windows.Forms.ComboBox();
            this.label25 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.lblAddOS = new System.Windows.Forms.LinkLabel();
            this.cboTypeOS = new System.Windows.Forms.ComboBox();
            this.label30 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.cboRAM = new System.Windows.Forms.ComboBox();
            this.cboProcesseur = new System.Windows.Forms.ComboBox();
            this.lblAddRAM = new System.Windows.Forms.LinkLabel();
            this.lblAddProcessor = new System.Windows.Forms.LinkLabel();
            this.lblAddTypeHDD = new System.Windows.Forms.LinkLabel();
            this.lblAddCorProcessor = new System.Windows.Forms.LinkLabel();
            this.lblAddNbrHDD = new System.Windows.Forms.LinkLabel();
            this.lblAddCapacityHDD = new System.Windows.Forms.LinkLabel();
            this.cboTailleEcran = new System.Windows.Forms.ComboBox();
            this.lblAddUSB2 = new System.Windows.Forms.LinkLabel();
            this.lblAddScreen = new System.Windows.Forms.LinkLabel();
            this.lblAddUSB3 = new System.Windows.Forms.LinkLabel();
            this.lblAddIAdapt = new System.Windows.Forms.LinkLabel();
            this.lblAddPAdapt = new System.Windows.Forms.LinkLabel();
            this.lblAddUAdapt = new System.Windows.Forms.LinkLabel();
            this.lblAddVGA = new System.Windows.Forms.LinkLabel();
            this.lblAddHDMI = new System.Windows.Forms.LinkLabel();
            this.lblAddUBatterie = new System.Windows.Forms.LinkLabel();
            this.lblAddGuaratie = new System.Windows.Forms.LinkLabel();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label23 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPhoto3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPhoto2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPhoto1)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbQRCode)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgv.BackgroundColor = System.Drawing.Color.White;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colId,
            this.colCode_str,
            this.colId_categorie_materiel,
            this.colId_compte,
            this.colQrcode,
            this.colDate_acquisition,
            this.colGuarantie,
            this.colId_marque,
            this.colId_modele,
            this.colId_couleur,
            this.colId_poids,
            this.colId_etat_materiel,
            this.colPhoto1,
            this.colPhoto2,
            this.colPhoto3,
            this.colLabel,
            this.colMac_adresse1,
            this.colMac_adresse2,
            this.colCommentaire,
            this.colUser_created,
            this.colDate_created,
            this.colUser_modified,
            this.colDate_modified,
            this.colId_type_ordinateur,
            this.colId_type_clavier,
            this.colId_os,
            this.colRam,
            this.colProcesseur,
            this.colNombre_coeur_processeur,
            this.colNombre_hdd,
            this.colCapacite_hdd,
            this.colIndice_performance,
            this.colPouce,
            this.colNombre_usb2,
            this.colNombre_usb3,
            this.colNombre_hdmi,
            this.colNombre_vga,
            this.colTension_batterie,
            this.colTension_adaptateur,
            this.colPuissance_adaptateur,
            this.colIntensite_adaptateur,
            this.colNumero_cle,
            this.colId_type_imprimante,
            this.colPuissance,
            this.colIntensite,
            this.colNombre_page_par_minute,
            this.colId_type_amplificateur,
            this.colTension_alimentation,
            this.colNombre_usb,
            this.colNombre_memoire,
            this.colNombre_sorties_audio,
            this.colNombre_microphone,
            this.colGain,
            this.colId_type_routeur_ap,
            this.colId_version_ios,
            this.colNombre_gbe,
            this.colNombre_fe,
            this.colNombre_fo,
            this.colNombre_serial,
            this.colCapable_usb,
            this.colMotpasse_defaut,
            this.colDefault_ip,
            this.colNombre_console,
            this.colNombre_auxiliaire,
            this.colId_type_ap,
            this.colId_type_switch,
            this.colFrequence,
            this.colAlimentation,
            this.colNombre_antenne,
            this.colId_netette,
            this.colCompatible_wifi});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Thistle;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgv.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgv.Location = new System.Drawing.Point(3, 19);
            this.dgv.MultiSelect = false;
            this.dgv.Name = "dgv";
            this.dgv.RowHeadersVisible = false;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.Size = new System.Drawing.Size(1185, 204);
            this.dgv.TabIndex = 200;
            this.dgv.SelectionChanged += new System.EventHandler(this.dgv_SelectionChanged);
            // 
            // colId
            // 
            this.colId.DataPropertyName = "Id";
            this.colId.HeaderText = "Id";
            this.colId.Name = "colId";
            this.colId.Visible = false;
            // 
            // colCode_str
            // 
            this.colCode_str.DataPropertyName = "Code_str";
            this.colCode_str.HeaderText = "Code texte";
            this.colCode_str.Name = "colCode_str";
            // 
            // colId_categorie_materiel
            // 
            this.colId_categorie_materiel.DataPropertyName = "Id_categorie_materiel";
            this.colId_categorie_materiel.HeaderText = "Id_categorie_materiel";
            this.colId_categorie_materiel.Name = "colId_categorie_materiel";
            this.colId_categorie_materiel.Visible = false;
            // 
            // colId_compte
            // 
            this.colId_compte.DataPropertyName = "Id_compte";
            this.colId_compte.HeaderText = "Id_compte";
            this.colId_compte.Name = "colId_compte";
            this.colId_compte.Visible = false;
            // 
            // colQrcode
            // 
            this.colQrcode.DataPropertyName = "Qrcode";
            this.colQrcode.HeaderText = "Qrcode";
            this.colQrcode.Name = "colQrcode";
            this.colQrcode.Visible = false;
            // 
            // colDate_acquisition
            // 
            this.colDate_acquisition.DataPropertyName = "Date_acquisition";
            this.colDate_acquisition.HeaderText = "Date acquisition";
            this.colDate_acquisition.Name = "colDate_acquisition";
            this.colDate_acquisition.Width = 110;
            // 
            // colGuarantie
            // 
            this.colGuarantie.DataPropertyName = "Guarantie";
            this.colGuarantie.HeaderText = "Guarantie";
            this.colGuarantie.Name = "colGuarantie";
            // 
            // colId_marque
            // 
            this.colId_marque.DataPropertyName = "Id_marque";
            this.colId_marque.HeaderText = "Id_marque";
            this.colId_marque.Name = "colId_marque";
            this.colId_marque.Visible = false;
            // 
            // colId_modele
            // 
            this.colId_modele.DataPropertyName = "Id_modele";
            this.colId_modele.HeaderText = "Id_modele";
            this.colId_modele.Name = "colId_modele";
            this.colId_modele.Visible = false;
            // 
            // colId_couleur
            // 
            this.colId_couleur.DataPropertyName = "Id_couleur";
            this.colId_couleur.HeaderText = "Id_couleur";
            this.colId_couleur.Name = "colId_couleur";
            this.colId_couleur.Visible = false;
            // 
            // colId_poids
            // 
            this.colId_poids.DataPropertyName = "Id_poids";
            this.colId_poids.HeaderText = "Id_poids";
            this.colId_poids.Name = "colId_poids";
            this.colId_poids.Visible = false;
            // 
            // colId_etat_materiel
            // 
            this.colId_etat_materiel.DataPropertyName = "Id_etat_materiel";
            this.colId_etat_materiel.HeaderText = "Id_etat_materiel";
            this.colId_etat_materiel.Name = "colId_etat_materiel";
            this.colId_etat_materiel.Visible = false;
            // 
            // colPhoto1
            // 
            this.colPhoto1.DataPropertyName = "Photo1";
            this.colPhoto1.HeaderText = "Photo1";
            this.colPhoto1.Name = "colPhoto1";
            this.colPhoto1.Visible = false;
            // 
            // colPhoto2
            // 
            this.colPhoto2.DataPropertyName = "Photo2";
            this.colPhoto2.HeaderText = "Photo2";
            this.colPhoto2.Name = "colPhoto2";
            this.colPhoto2.Visible = false;
            // 
            // colPhoto3
            // 
            this.colPhoto3.DataPropertyName = "Photo3";
            this.colPhoto3.HeaderText = "Photo3";
            this.colPhoto3.Name = "colPhoto3";
            this.colPhoto3.Visible = false;
            // 
            // colLabel
            // 
            this.colLabel.DataPropertyName = "Label";
            this.colLabel.HeaderText = "Etiquette";
            this.colLabel.Name = "colLabel";
            // 
            // colMac_adresse1
            // 
            this.colMac_adresse1.DataPropertyName = "Mac_adresse1";
            this.colMac_adresse1.HeaderText = "Mac Wifi";
            this.colMac_adresse1.Name = "colMac_adresse1";
            // 
            // colMac_adresse2
            // 
            this.colMac_adresse2.DataPropertyName = "Mac_adresse2";
            this.colMac_adresse2.HeaderText = "Mac LAN";
            this.colMac_adresse2.Name = "colMac_adresse2";
            // 
            // colCommentaire
            // 
            this.colCommentaire.DataPropertyName = "Commentaire";
            this.colCommentaire.HeaderText = "Commentaire";
            this.colCommentaire.Name = "colCommentaire";
            // 
            // colUser_created
            // 
            this.colUser_created.DataPropertyName = "User_created";
            this.colUser_created.HeaderText = "User created";
            this.colUser_created.Name = "colUser_created";
            // 
            // colDate_created
            // 
            this.colDate_created.DataPropertyName = "Date_created";
            this.colDate_created.HeaderText = "Date created";
            this.colDate_created.Name = "colDate_created";
            // 
            // colUser_modified
            // 
            this.colUser_modified.DataPropertyName = "User_modified";
            this.colUser_modified.HeaderText = "User modified";
            this.colUser_modified.Name = "colUser_modified";
            // 
            // colDate_modified
            // 
            this.colDate_modified.DataPropertyName = "Date_modified";
            this.colDate_modified.HeaderText = "Date modified";
            this.colDate_modified.Name = "colDate_modified";
            // 
            // colId_type_ordinateur
            // 
            this.colId_type_ordinateur.DataPropertyName = "Id_type_ordinateur";
            this.colId_type_ordinateur.HeaderText = "Id_type_ordinateur";
            this.colId_type_ordinateur.Name = "colId_type_ordinateur";
            this.colId_type_ordinateur.Visible = false;
            // 
            // colId_type_clavier
            // 
            this.colId_type_clavier.DataPropertyName = "Id_type_clavier";
            this.colId_type_clavier.HeaderText = "Id_type_clavier";
            this.colId_type_clavier.Name = "colId_type_clavier";
            this.colId_type_clavier.Visible = false;
            // 
            // colId_os
            // 
            this.colId_os.DataPropertyName = "Id_os";
            this.colId_os.HeaderText = "Id_os";
            this.colId_os.Name = "colId_os";
            this.colId_os.Visible = false;
            // 
            // colRam
            // 
            this.colRam.DataPropertyName = "Ram";
            this.colRam.HeaderText = "Ram (Ghz)";
            this.colRam.Name = "colRam";
            // 
            // colProcesseur
            // 
            this.colProcesseur.DataPropertyName = "Ram";
            this.colProcesseur.HeaderText = "Processeur (Ghz)";
            this.colProcesseur.Name = "colProcesseur";
            this.colProcesseur.Width = 130;
            // 
            // colNombre_coeur_processeur
            // 
            this.colNombre_coeur_processeur.DataPropertyName = "Nombre_coeur_processeur";
            this.colNombre_coeur_processeur.HeaderText = "Nbr coeur processeur";
            this.colNombre_coeur_processeur.Name = "colNombre_coeur_processeur";
            this.colNombre_coeur_processeur.Width = 150;
            // 
            // colNombre_hdd
            // 
            this.colNombre_hdd.DataPropertyName = "Nombre_hdd";
            this.colNombre_hdd.HeaderText = "Nbr HDD";
            this.colNombre_hdd.Name = "colNombre_hdd";
            // 
            // colCapacite_hdd
            // 
            this.colCapacite_hdd.DataPropertyName = "Capacite_hdd";
            this.colCapacite_hdd.HeaderText = "Capacite HDD";
            this.colCapacite_hdd.Name = "colCapacite_hdd";
            this.colCapacite_hdd.Width = 120;
            // 
            // colIndice_performance
            // 
            this.colIndice_performance.DataPropertyName = "Indice_performance";
            this.colIndice_performance.HeaderText = "Indice perfo.";
            this.colIndice_performance.Name = "colIndice_performance";
            // 
            // colPouce
            // 
            this.colPouce.DataPropertyName = "Pouce";
            this.colPouce.HeaderText = "Taille écran (Pouce)";
            this.colPouce.Name = "colPouce";
            this.colPouce.Width = 150;
            // 
            // colNombre_usb2
            // 
            this.colNombre_usb2.DataPropertyName = "Nombre_usb2";
            this.colNombre_usb2.HeaderText = "USB 2.0";
            this.colNombre_usb2.Name = "colNombre_usb2";
            // 
            // colNombre_usb3
            // 
            this.colNombre_usb3.DataPropertyName = "Nombre_usb3";
            this.colNombre_usb3.HeaderText = "USB 3.0";
            this.colNombre_usb3.Name = "colNombre_usb3";
            // 
            // colNombre_hdmi
            // 
            this.colNombre_hdmi.DataPropertyName = "Nombre_hdmi";
            this.colNombre_hdmi.HeaderText = "HDMI";
            this.colNombre_hdmi.Name = "colNombre_hdmi";
            // 
            // colNombre_vga
            // 
            this.colNombre_vga.DataPropertyName = "Nombre_vga";
            this.colNombre_vga.HeaderText = "VGA";
            this.colNombre_vga.Name = "colNombre_vga";
            // 
            // colTension_batterie
            // 
            this.colTension_batterie.DataPropertyName = "Tension_batterie";
            this.colTension_batterie.HeaderText = "Tension batterie (V)";
            this.colTension_batterie.Name = "colTension_batterie";
            this.colTension_batterie.Width = 130;
            // 
            // colTension_adaptateur
            // 
            this.colTension_adaptateur.DataPropertyName = "Tension_adaptateur";
            this.colTension_adaptateur.HeaderText = "Tension adapt. (V)";
            this.colTension_adaptateur.Name = "colTension_adaptateur";
            this.colTension_adaptateur.Width = 130;
            // 
            // colPuissance_adaptateur
            // 
            this.colPuissance_adaptateur.DataPropertyName = "Puissance_adaptateur";
            this.colPuissance_adaptateur.HeaderText = "Puissance adapt. (W)";
            this.colPuissance_adaptateur.Name = "colPuissance_adaptateur";
            this.colPuissance_adaptateur.Width = 150;
            // 
            // colIntensite_adaptateur
            // 
            this.colIntensite_adaptateur.DataPropertyName = "Intensite_adaptateur";
            this.colIntensite_adaptateur.HeaderText = "Intensité adapt. (A)";
            this.colIntensite_adaptateur.Name = "colIntensite_adaptateur";
            this.colIntensite_adaptateur.Width = 130;
            // 
            // colNumero_cle
            // 
            this.colNumero_cle.DataPropertyName = "Numero_cle";
            this.colNumero_cle.HeaderText = "Num clé";
            this.colNumero_cle.Name = "colNumero_cle";
            // 
            // colId_type_imprimante
            // 
            this.colId_type_imprimante.DataPropertyName = "Id_type_imprimante";
            this.colId_type_imprimante.HeaderText = "Id_type_imprimante";
            this.colId_type_imprimante.Name = "colId_type_imprimante";
            this.colId_type_imprimante.Visible = false;
            // 
            // colPuissance
            // 
            this.colPuissance.DataPropertyName = "Puissance";
            this.colPuissance.HeaderText = "Puissance (W)";
            this.colPuissance.Name = "colPuissance";
            this.colPuissance.Visible = false;
            // 
            // colIntensite
            // 
            this.colIntensite.DataPropertyName = "Intensite";
            this.colIntensite.HeaderText = "Intensité (A)";
            this.colIntensite.Name = "colIntensite";
            this.colIntensite.Visible = false;
            // 
            // colNombre_page_par_minute
            // 
            this.colNombre_page_par_minute.DataPropertyName = "Nombre_page_par_minute";
            this.colNombre_page_par_minute.HeaderText = "Page/minute";
            this.colNombre_page_par_minute.Name = "colNombre_page_par_minute";
            this.colNombre_page_par_minute.Visible = false;
            // 
            // colId_type_amplificateur
            // 
            this.colId_type_amplificateur.DataPropertyName = "Id_type_amplificateur";
            this.colId_type_amplificateur.HeaderText = "Id_type_amplificateur";
            this.colId_type_amplificateur.Name = "colId_type_amplificateur";
            this.colId_type_amplificateur.Visible = false;
            // 
            // colTension_alimentation
            // 
            this.colTension_alimentation.DataPropertyName = "Tension_alimentation";
            this.colTension_alimentation.HeaderText = "Tension alimentation";
            this.colTension_alimentation.Name = "colTension_alimentation";
            this.colTension_alimentation.Visible = false;
            this.colTension_alimentation.Width = 110;
            // 
            // colNombre_usb
            // 
            this.colNombre_usb.DataPropertyName = "Nombre_usb";
            this.colNombre_usb.HeaderText = "Nbr USB";
            this.colNombre_usb.Name = "colNombre_usb";
            this.colNombre_usb.Visible = false;
            // 
            // colNombre_memoire
            // 
            this.colNombre_memoire.DataPropertyName = "Nombre_memoire";
            this.colNombre_memoire.HeaderText = "Nbr mémoire";
            this.colNombre_memoire.Name = "colNombre_memoire";
            this.colNombre_memoire.Visible = false;
            // 
            // colNombre_sorties_audio
            // 
            this.colNombre_sorties_audio.DataPropertyName = "Nombre_sorties_audio";
            this.colNombre_sorties_audio.HeaderText = "Nbr sorties audio";
            this.colNombre_sorties_audio.Name = "colNombre_sorties_audio";
            this.colNombre_sorties_audio.Visible = false;
            // 
            // colNombre_microphone
            // 
            this.colNombre_microphone.DataPropertyName = "Nombre_microphone";
            this.colNombre_microphone.HeaderText = "Nbr microphone";
            this.colNombre_microphone.Name = "colNombre_microphone";
            this.colNombre_microphone.Visible = false;
            // 
            // colGain
            // 
            this.colGain.DataPropertyName = "Gain";
            this.colGain.HeaderText = "Gain (dB)";
            this.colGain.Name = "colGain";
            this.colGain.Visible = false;
            // 
            // colId_type_routeur_ap
            // 
            this.colId_type_routeur_ap.DataPropertyName = "Id_type_routeur_ap";
            this.colId_type_routeur_ap.HeaderText = "Id_type_routeur_ap";
            this.colId_type_routeur_ap.Name = "colId_type_routeur_ap";
            this.colId_type_routeur_ap.Visible = false;
            // 
            // colId_version_ios
            // 
            this.colId_version_ios.DataPropertyName = "Id_version_ios";
            this.colId_version_ios.HeaderText = "Id_version_ios";
            this.colId_version_ios.Name = "colId_version_ios";
            this.colId_version_ios.Visible = false;
            // 
            // colNombre_gbe
            // 
            this.colNombre_gbe.DataPropertyName = "Nombre_gbe";
            this.colNombre_gbe.HeaderText = "GB Eth";
            this.colNombre_gbe.Name = "colNombre_gbe";
            this.colNombre_gbe.Visible = false;
            // 
            // colNombre_fe
            // 
            this.colNombre_fe.DataPropertyName = "Nombre_fe";
            this.colNombre_fe.HeaderText = "Fast Eth";
            this.colNombre_fe.Name = "colNombre_fe";
            this.colNombre_fe.Visible = false;
            // 
            // colNombre_fo
            // 
            this.colNombre_fo.DataPropertyName = "Nombre_fo";
            this.colNombre_fo.HeaderText = "Fibre Optique";
            this.colNombre_fo.Name = "colNombre_fo";
            this.colNombre_fo.Visible = false;
            // 
            // colNombre_serial
            // 
            this.colNombre_serial.DataPropertyName = "Nombre_serial";
            this.colNombre_serial.HeaderText = "Serial";
            this.colNombre_serial.Name = "colNombre_serial";
            this.colNombre_serial.Visible = false;
            // 
            // colCapable_usb
            // 
            this.colCapable_usb.DataPropertyName = "Capable_usb";
            this.colCapable_usb.HeaderText = "Support USB";
            this.colCapable_usb.Name = "colCapable_usb";
            this.colCapable_usb.Visible = false;
            // 
            // colMotpasse_defaut
            // 
            this.colMotpasse_defaut.DataPropertyName = "Motpasse_defaut";
            this.colMotpasse_defaut.HeaderText = "Default pwd";
            this.colMotpasse_defaut.Name = "colMotpasse_defaut";
            this.colMotpasse_defaut.Visible = false;
            // 
            // colDefault_ip
            // 
            this.colDefault_ip.DataPropertyName = "Default_ip";
            this.colDefault_ip.HeaderText = "Default IP";
            this.colDefault_ip.Name = "colDefault_ip";
            this.colDefault_ip.Visible = false;
            // 
            // colNombre_console
            // 
            this.colNombre_console.DataPropertyName = "Nombre_console";
            this.colNombre_console.HeaderText = "Port Console";
            this.colNombre_console.Name = "colNombre_console";
            this.colNombre_console.Visible = false;
            // 
            // colNombre_auxiliaire
            // 
            this.colNombre_auxiliaire.DataPropertyName = "Nombre_auxiliaire";
            this.colNombre_auxiliaire.HeaderText = "Nombre_auxiliaire";
            this.colNombre_auxiliaire.Name = "colNombre_auxiliaire";
            this.colNombre_auxiliaire.Visible = false;
            // 
            // colId_type_ap
            // 
            this.colId_type_ap.DataPropertyName = "Id_type_ap";
            this.colId_type_ap.HeaderText = "Id_type_ap";
            this.colId_type_ap.Name = "colId_type_ap";
            this.colId_type_ap.Visible = false;
            // 
            // colId_type_switch
            // 
            this.colId_type_switch.DataPropertyName = "Id_type_switch";
            this.colId_type_switch.HeaderText = "Id_type_switch";
            this.colId_type_switch.Name = "colId_type_switch";
            this.colId_type_switch.Visible = false;
            // 
            // colFrequence
            // 
            this.colFrequence.DataPropertyName = "Frequence";
            this.colFrequence.HeaderText = "Fréquence";
            this.colFrequence.Name = "colFrequence";
            this.colFrequence.Visible = false;
            // 
            // colAlimentation
            // 
            this.colAlimentation.DataPropertyName = "Alimentation";
            this.colAlimentation.HeaderText = "Alimentation (Mhz)";
            this.colAlimentation.Name = "colAlimentation";
            this.colAlimentation.Visible = false;
            // 
            // colNombre_antenne
            // 
            this.colNombre_antenne.DataPropertyName = "Nombre_antenne";
            this.colNombre_antenne.HeaderText = "Nbr antenne";
            this.colNombre_antenne.Name = "colNombre_antenne";
            this.colNombre_antenne.Visible = false;
            // 
            // colId_netette
            // 
            this.colId_netette.DataPropertyName = "Id_netette";
            this.colId_netette.HeaderText = "Id_netette";
            this.colId_netette.Name = "colId_netette";
            this.colId_netette.Visible = false;
            // 
            // colCompatible_wifi
            // 
            this.colCompatible_wifi.DataPropertyName = "Compatible_wifi";
            this.colCompatible_wifi.HeaderText = "Compatible Wifi";
            this.colCompatible_wifi.Name = "colCompatible_wifi";
            this.colCompatible_wifi.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.dgv);
            this.groupBox1.Location = new System.Drawing.Point(8, 326);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1191, 226);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Affichage des données manipulées";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblAddGuaratie);
            this.groupBox2.Controls.Add(this.cboMarque);
            this.groupBox2.Controls.Add(this.txtDateModifie);
            this.groupBox2.Controls.Add(this.txtQRCode);
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Controls.Add(this.txtModifieBy);
            this.groupBox2.Controls.Add(this.label18);
            this.groupBox2.Controls.Add(this.txtDateCreate);
            this.groupBox2.Controls.Add(this.label19);
            this.groupBox2.Controls.Add(this.txtCreateBy);
            this.groupBox2.Controls.Add(this.label20);
            this.groupBox2.Controls.Add(this.txtCommentaire);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.txtMAC2);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.groupBox5);
            this.groupBox2.Controls.Add(this.txtMAC1);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.lblEtatMatriel);
            this.groupBox2.Controls.Add(this.cboEtat);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.lblAddPoids);
            this.groupBox2.Controls.Add(this.cboPoids);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.lblAddCouleur);
            this.groupBox2.Controls.Add(this.cboCouleur);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.lblAddModele);
            this.groupBox2.Controls.Add(this.cboModele);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.lblAddMarque);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.cboGuarantie);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.txtDateAcquisition);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.lblAddNumCompte);
            this.groupBox2.Controls.Add(this.cboNumCompte);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtIdentidiant);
            this.groupBox2.Controls.Add(this.lblAddCategorieMat);
            this.groupBox2.Controls.Add(this.cboCatMateriel);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtId);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(8, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(671, 315);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            // 
            // cboMarque
            // 
            this.cboMarque.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboMarque.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboMarque.DropDownWidth = 130;
            this.cboMarque.FormattingEnabled = true;
            this.cboMarque.Location = new System.Drawing.Point(101, 185);
            this.cboMarque.Name = "cboMarque";
            this.cboMarque.Size = new System.Drawing.Size(109, 21);
            this.cboMarque.TabIndex = 6;
            this.cboMarque.DropDown += new System.EventHandler(this.cboMarque_DropDown);
            // 
            // txtDateModifie
            // 
            this.txtDateModifie.Location = new System.Drawing.Point(548, 84);
            this.txtDateModifie.Name = "txtDateModifie";
            this.txtDateModifie.ReadOnly = true;
            this.txtDateModifie.Size = new System.Drawing.Size(109, 20);
            this.txtDateModifie.TabIndex = 156;
            // 
            // txtQRCode
            // 
            this.txtQRCode.Location = new System.Drawing.Point(101, 112);
            this.txtQRCode.Name = "txtQRCode";
            this.txtQRCode.ReadOnly = true;
            this.txtQRCode.Size = new System.Drawing.Size(109, 22);
            this.txtQRCode.TabIndex = 152;
            this.txtQRCode.Text = "";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(453, 88);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(98, 13);
            this.label17.TabIndex = 48;
            this.label17.Text = "Date modification : ";
            // 
            // txtModifieBy
            // 
            this.txtModifieBy.Location = new System.Drawing.Point(548, 60);
            this.txtModifieBy.Name = "txtModifieBy";
            this.txtModifieBy.ReadOnly = true;
            this.txtModifieBy.Size = new System.Drawing.Size(109, 20);
            this.txtModifieBy.TabIndex = 155;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(453, 64);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(68, 13);
            this.label18.TabIndex = 46;
            this.label18.Text = "Modifier par :";
            // 
            // txtDateCreate
            // 
            this.txtDateCreate.Location = new System.Drawing.Point(548, 37);
            this.txtDateCreate.Name = "txtDateCreate";
            this.txtDateCreate.ReadOnly = true;
            this.txtDateCreate.Size = new System.Drawing.Size(109, 20);
            this.txtDateCreate.TabIndex = 154;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(453, 40);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(80, 13);
            this.label19.TabIndex = 44;
            this.label19.Text = "Date création : ";
            // 
            // txtCreateBy
            // 
            this.txtCreateBy.Location = new System.Drawing.Point(548, 13);
            this.txtCreateBy.Name = "txtCreateBy";
            this.txtCreateBy.ReadOnly = true;
            this.txtCreateBy.Size = new System.Drawing.Size(109, 20);
            this.txtCreateBy.TabIndex = 153;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(453, 17);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(53, 13);
            this.label20.TabIndex = 42;
            this.label20.Text = "Créé par :";
            // 
            // txtCommentaire
            // 
            this.txtCommentaire.Location = new System.Drawing.Point(339, 61);
            this.txtCommentaire.Multiline = true;
            this.txtCommentaire.Name = "txtCommentaire";
            this.txtCommentaire.Size = new System.Drawing.Size(109, 73);
            this.txtCommentaire.TabIndex = 18;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(244, 64);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(77, 13);
            this.label15.TabIndex = 40;
            this.label15.Text = "Commentaire : ";
            // 
            // txtMAC2
            // 
            this.txtMAC2.Location = new System.Drawing.Point(339, 37);
            this.txtMAC2.Name = "txtMAC2";
            this.txtMAC2.Size = new System.Drawing.Size(109, 20);
            this.txtMAC2.TabIndex = 17;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(244, 40);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(83, 13);
            this.label16.TabIndex = 38;
            this.label16.Text = "Adresse MAC2 :";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.lblPhoto3);
            this.groupBox5.Controls.Add(this.lblPhoto2);
            this.groupBox5.Controls.Add(this.pbPhoto3);
            this.groupBox5.Controls.Add(this.pbPhoto2);
            this.groupBox5.Controls.Add(this.lblPhoto1);
            this.groupBox5.Controls.Add(this.pbPhoto1);
            this.groupBox5.Location = new System.Drawing.Point(247, 137);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(417, 171);
            this.groupBox5.TabIndex = 5;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Vue de profil de l\'équipement";
            // 
            // lblPhoto3
            // 
            this.lblPhoto3.AutoSize = true;
            this.lblPhoto3.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.lblPhoto3.Location = new System.Drawing.Point(305, 152);
            this.lblPhoto3.Name = "lblPhoto3";
            this.lblPhoto3.Size = new System.Drawing.Size(80, 13);
            this.lblPhoto3.TabIndex = 44;
            this.lblPhoto3.TabStop = true;
            this.lblPhoto3.Text = "Charger photo3";
            this.lblPhoto3.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.lblPhoto3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblPhoto3_LinkClicked);
            // 
            // lblPhoto2
            // 
            this.lblPhoto2.AutoSize = true;
            this.lblPhoto2.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.lblPhoto2.Location = new System.Drawing.Point(168, 153);
            this.lblPhoto2.Name = "lblPhoto2";
            this.lblPhoto2.Size = new System.Drawing.Size(80, 13);
            this.lblPhoto2.TabIndex = 43;
            this.lblPhoto2.TabStop = true;
            this.lblPhoto2.Text = "Charger photo2";
            this.lblPhoto2.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.lblPhoto2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblPhoto2_LinkClicked);
            // 
            // pbPhoto3
            // 
            this.pbPhoto3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbPhoto3.Location = new System.Drawing.Point(278, 20);
            this.pbPhoto3.Name = "pbPhoto3";
            this.pbPhoto3.Size = new System.Drawing.Size(131, 131);
            this.pbPhoto3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbPhoto3.TabIndex = 44;
            this.pbPhoto3.TabStop = false;
            // 
            // pbPhoto2
            // 
            this.pbPhoto2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbPhoto2.Location = new System.Drawing.Point(142, 20);
            this.pbPhoto2.Name = "pbPhoto2";
            this.pbPhoto2.Size = new System.Drawing.Size(131, 131);
            this.pbPhoto2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbPhoto2.TabIndex = 43;
            this.pbPhoto2.TabStop = false;
            // 
            // lblPhoto1
            // 
            this.lblPhoto1.AutoSize = true;
            this.lblPhoto1.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.lblPhoto1.Location = new System.Drawing.Point(34, 153);
            this.lblPhoto1.Name = "lblPhoto1";
            this.lblPhoto1.Size = new System.Drawing.Size(80, 13);
            this.lblPhoto1.TabIndex = 42;
            this.lblPhoto1.TabStop = true;
            this.lblPhoto1.Text = "Charger photo1";
            this.lblPhoto1.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.lblPhoto1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblPhoto1_LinkClicked);
            // 
            // pbPhoto1
            // 
            this.pbPhoto1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbPhoto1.Location = new System.Drawing.Point(7, 20);
            this.pbPhoto1.Name = "pbPhoto1";
            this.pbPhoto1.Size = new System.Drawing.Size(131, 131);
            this.pbPhoto1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbPhoto1.TabIndex = 2;
            this.pbPhoto1.TabStop = false;
            // 
            // txtMAC1
            // 
            this.txtMAC1.Location = new System.Drawing.Point(339, 13);
            this.txtMAC1.Name = "txtMAC1";
            this.txtMAC1.Size = new System.Drawing.Size(109, 20);
            this.txtMAC1.TabIndex = 16;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(244, 16);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(86, 13);
            this.label12.TabIndex = 36;
            this.label12.Text = "Adresse MAC1 : ";
            // 
            // lblEtatMatriel
            // 
            this.lblEtatMatriel.AutoSize = true;
            this.lblEtatMatriel.Location = new System.Drawing.Point(213, 288);
            this.lblEtatMatriel.Name = "lblEtatMatriel";
            this.lblEtatMatriel.Size = new System.Drawing.Size(29, 13);
            this.lblEtatMatriel.TabIndex = 15;
            this.lblEtatMatriel.TabStop = true;
            this.lblEtatMatriel.Text = "New";
            this.lblEtatMatriel.VisitedLinkColor = System.Drawing.Color.Blue;
            this.lblEtatMatriel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblEtatMatriel_LinkClicked);
            // 
            // cboEtat
            // 
            this.cboEtat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEtat.FormattingEnabled = true;
            this.cboEtat.Location = new System.Drawing.Point(101, 285);
            this.cboEtat.Name = "cboEtat";
            this.cboEtat.Size = new System.Drawing.Size(109, 21);
            this.cboEtat.TabIndex = 14;
            this.cboEtat.DropDown += new System.EventHandler(this.cboEtat_DropDown);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 289);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(67, 13);
            this.label13.TabIndex = 31;
            this.label13.Text = "Etat actuel : ";
            // 
            // lblAddPoids
            // 
            this.lblAddPoids.AutoSize = true;
            this.lblAddPoids.Location = new System.Drawing.Point(213, 263);
            this.lblAddPoids.Name = "lblAddPoids";
            this.lblAddPoids.Size = new System.Drawing.Size(29, 13);
            this.lblAddPoids.TabIndex = 13;
            this.lblAddPoids.TabStop = true;
            this.lblAddPoids.Text = "New";
            this.lblAddPoids.VisitedLinkColor = System.Drawing.Color.Blue;
            this.lblAddPoids.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblAddPoids_LinkClicked);
            // 
            // cboPoids
            // 
            this.cboPoids.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPoids.FormattingEnabled = true;
            this.cboPoids.Location = new System.Drawing.Point(101, 260);
            this.cboPoids.Name = "cboPoids";
            this.cboPoids.Size = new System.Drawing.Size(109, 21);
            this.cboPoids.TabIndex = 12;
            this.cboPoids.DropDown += new System.EventHandler(this.cboPoids_DropDown);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 264);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(42, 13);
            this.label10.TabIndex = 28;
            this.label10.Text = "Poids : ";
            // 
            // lblAddCouleur
            // 
            this.lblAddCouleur.AutoSize = true;
            this.lblAddCouleur.Location = new System.Drawing.Point(213, 239);
            this.lblAddCouleur.Name = "lblAddCouleur";
            this.lblAddCouleur.Size = new System.Drawing.Size(29, 13);
            this.lblAddCouleur.TabIndex = 11;
            this.lblAddCouleur.TabStop = true;
            this.lblAddCouleur.Text = "New";
            this.lblAddCouleur.VisitedLinkColor = System.Drawing.Color.Blue;
            this.lblAddCouleur.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblAddCouleur_LinkClicked);
            // 
            // cboCouleur
            // 
            this.cboCouleur.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboCouleur.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboCouleur.FormattingEnabled = true;
            this.cboCouleur.Location = new System.Drawing.Point(101, 235);
            this.cboCouleur.Name = "cboCouleur";
            this.cboCouleur.Size = new System.Drawing.Size(109, 21);
            this.cboCouleur.TabIndex = 10;
            this.cboCouleur.DropDown += new System.EventHandler(this.cboCouleur_DropDown);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 239);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(52, 13);
            this.label11.TabIndex = 25;
            this.label11.Text = "Couleur : ";
            // 
            // lblAddModele
            // 
            this.lblAddModele.AutoSize = true;
            this.lblAddModele.Location = new System.Drawing.Point(213, 213);
            this.lblAddModele.Name = "lblAddModele";
            this.lblAddModele.Size = new System.Drawing.Size(29, 13);
            this.lblAddModele.TabIndex = 9;
            this.lblAddModele.TabStop = true;
            this.lblAddModele.Text = "New";
            this.lblAddModele.VisitedLinkColor = System.Drawing.Color.Blue;
            this.lblAddModele.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblAddModele_LinkClicked);
            // 
            // cboModele
            // 
            this.cboModele.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboModele.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboModele.DropDownWidth = 150;
            this.cboModele.FormattingEnabled = true;
            this.cboModele.Location = new System.Drawing.Point(101, 210);
            this.cboModele.Name = "cboModele";
            this.cboModele.Size = new System.Drawing.Size(109, 21);
            this.cboModele.TabIndex = 8;
            this.cboModele.DropDown += new System.EventHandler(this.cboModele_DropDown);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 214);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(51, 13);
            this.label8.TabIndex = 22;
            this.label8.Text = "Modèle : ";
            // 
            // lblAddMarque
            // 
            this.lblAddMarque.AutoSize = true;
            this.lblAddMarque.Location = new System.Drawing.Point(213, 188);
            this.lblAddMarque.Name = "lblAddMarque";
            this.lblAddMarque.Size = new System.Drawing.Size(29, 13);
            this.lblAddMarque.TabIndex = 7;
            this.lblAddMarque.TabStop = true;
            this.lblAddMarque.Text = "New";
            this.lblAddMarque.VisitedLinkColor = System.Drawing.Color.Blue;
            this.lblAddMarque.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblAddMarque_LinkClicked);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 189);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(52, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "Marque : ";
            // 
            // cboGuarantie
            // 
            this.cboGuarantie.Location = new System.Drawing.Point(101, 161);
            this.cboGuarantie.Name = "cboGuarantie";
            this.cboGuarantie.Size = new System.Drawing.Size(109, 20);
            this.cboGuarantie.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 164);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(102, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Guarantie (Année) : ";
            // 
            // txtDateAcquisition
            // 
            this.txtDateAcquisition.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtDateAcquisition.Location = new System.Drawing.Point(101, 137);
            this.txtDateAcquisition.Name = "txtDateAcquisition";
            this.txtDateAcquisition.Size = new System.Drawing.Size(109, 20);
            this.txtDateAcquisition.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 140);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(92, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Date acquisition : ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 115);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "QR Code Texte : ";
            // 
            // lblAddNumCompte
            // 
            this.lblAddNumCompte.AutoSize = true;
            this.lblAddNumCompte.Location = new System.Drawing.Point(213, 90);
            this.lblAddNumCompte.Name = "lblAddNumCompte";
            this.lblAddNumCompte.Size = new System.Drawing.Size(29, 13);
            this.lblAddNumCompte.TabIndex = 3;
            this.lblAddNumCompte.TabStop = true;
            this.lblAddNumCompte.Text = "New";
            this.lblAddNumCompte.VisitedLinkColor = System.Drawing.Color.Blue;
            this.lblAddNumCompte.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblAddNumCompte_LinkClicked);
            // 
            // cboNumCompte
            // 
            this.cboNumCompte.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNumCompte.DropDownWidth = 109;
            this.cboNumCompte.FormattingEnabled = true;
            this.cboNumCompte.Location = new System.Drawing.Point(101, 87);
            this.cboNumCompte.Name = "cboNumCompte";
            this.cboNumCompte.Size = new System.Drawing.Size(109, 21);
            this.cboNumCompte.TabIndex = 2;
            this.cboNumCompte.DropDown += new System.EventHandler(this.cboNumCompte_DropDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Numéro Compte : ";
            // 
            // txtIdentidiant
            // 
            this.txtIdentidiant.Location = new System.Drawing.Point(101, 38);
            this.txtIdentidiant.Name = "txtIdentidiant";
            this.txtIdentidiant.ReadOnly = true;
            this.txtIdentidiant.Size = new System.Drawing.Size(109, 20);
            this.txtIdentidiant.TabIndex = 151;
            // 
            // lblAddCategorieMat
            // 
            this.lblAddCategorieMat.AutoSize = true;
            this.lblAddCategorieMat.Location = new System.Drawing.Point(213, 65);
            this.lblAddCategorieMat.Name = "lblAddCategorieMat";
            this.lblAddCategorieMat.Size = new System.Drawing.Size(29, 13);
            this.lblAddCategorieMat.TabIndex = 1;
            this.lblAddCategorieMat.TabStop = true;
            this.lblAddCategorieMat.Text = "New";
            this.lblAddCategorieMat.VisitedLinkColor = System.Drawing.Color.Blue;
            this.lblAddCategorieMat.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblAddCategorieMat_LinkClicked);
            // 
            // cboCatMateriel
            // 
            this.cboCatMateriel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCatMateriel.DropDownWidth = 150;
            this.cboCatMateriel.FormattingEnabled = true;
            this.cboCatMateriel.Location = new System.Drawing.Point(101, 62);
            this.cboCatMateriel.Name = "cboCatMateriel";
            this.cboCatMateriel.Size = new System.Drawing.Size(109, 21);
            this.cboCatMateriel.TabIndex = 0;
            this.cboCatMateriel.DropDown += new System.EventHandler(this.cboCatMateriel_DropDown);
            this.cboCatMateriel.Leave += new System.EventHandler(this.cboCatMateriel_Leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Catégorie matériel : ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Identifiant : ";
            // 
            // txtId
            // 
            this.txtId.Location = new System.Drawing.Point(101, 13);
            this.txtId.Name = "txtId";
            this.txtId.ReadOnly = true;
            this.txtId.Size = new System.Drawing.Size(109, 20);
            this.txtId.TabIndex = 150;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Code :";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.pbQRCode);
            this.groupBox3.Location = new System.Drawing.Point(377, 178);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(124, 130);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "QR Code";
            // 
            // pbQRCode
            // 
            this.pbQRCode.Location = new System.Drawing.Point(6, 14);
            this.pbQRCode.Name = "pbQRCode";
            this.pbQRCode.Size = new System.Drawing.Size(112, 112);
            this.pbQRCode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbQRCode.TabIndex = 2;
            this.pbQRCode.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.groupBox6);
            this.groupBox4.Controls.Add(this.lblAddIAdapt);
            this.groupBox4.Controls.Add(this.lblAddPAdapt);
            this.groupBox4.Controls.Add(this.lblAddUAdapt);
            this.groupBox4.Controls.Add(this.lblAddVGA);
            this.groupBox4.Controls.Add(this.lblAddHDMI);
            this.groupBox4.Controls.Add(this.lblAddUBatterie);
            this.groupBox4.Controls.Add(this.lblAddUSB2);
            this.groupBox4.Controls.Add(this.lblAddScreen);
            this.groupBox4.Controls.Add(this.lblAddUSB3);
            this.groupBox4.Controls.Add(this.cboTailleEcran);
            this.groupBox4.Controls.Add(this.lblAddNbrHDD);
            this.groupBox4.Controls.Add(this.lblAddCapacityHDD);
            this.groupBox4.Controls.Add(this.lblAddTypeHDD);
            this.groupBox4.Controls.Add(this.lblAddCorProcessor);
            this.groupBox4.Controls.Add(this.lblAddProcessor);
            this.groupBox4.Controls.Add(this.lblAddRAM);
            this.groupBox4.Controls.Add(this.cboProcesseur);
            this.groupBox4.Controls.Add(this.cboRAM);
            this.groupBox4.Controls.Add(this.label14);
            this.groupBox4.Controls.Add(this.txtNumeroCle);
            this.groupBox4.Controls.Add(this.groupBox3);
            this.groupBox4.Controls.Add(this.cboTensionAdap);
            this.groupBox4.Controls.Add(this.cboPuissanceAdap);
            this.groupBox4.Controls.Add(this.cboNbrHDMI);
            this.groupBox4.Controls.Add(this.cboIntensiteAdap);
            this.groupBox4.Controls.Add(this.cboNbrVGA);
            this.groupBox4.Controls.Add(this.label46);
            this.groupBox4.Controls.Add(this.label47);
            this.groupBox4.Controls.Add(this.label48);
            this.groupBox4.Controls.Add(this.label49);
            this.groupBox4.Controls.Add(this.cboTensionBatt);
            this.groupBox4.Controls.Add(this.label50);
            this.groupBox4.Controls.Add(this.label51);
            this.groupBox4.Controls.Add(this.label52);
            this.groupBox4.Controls.Add(this.cboTypeHDD);
            this.groupBox4.Controls.Add(this.cboNbrCoeurProcesseur);
            this.groupBox4.Controls.Add(this.lblAddTypeClavier);
            this.groupBox4.Controls.Add(this.cboTypeClavier);
            this.groupBox4.Controls.Add(this.lblAddPC);
            this.groupBox4.Controls.Add(this.cboTypeOrdi);
            this.groupBox4.Controls.Add(this.cboUSB3);
            this.groupBox4.Controls.Add(this.label21);
            this.groupBox4.Controls.Add(this.cboUSB2);
            this.groupBox4.Controls.Add(this.label22);
            this.groupBox4.Controls.Add(this.cboNbrHDD);
            this.groupBox4.Controls.Add(this.label24);
            this.groupBox4.Controls.Add(this.cboCapaciteHDD);
            this.groupBox4.Controls.Add(this.label25);
            this.groupBox4.Controls.Add(this.label26);
            this.groupBox4.Controls.Add(this.label27);
            this.groupBox4.Controls.Add(this.label28);
            this.groupBox4.Controls.Add(this.label29);
            this.groupBox4.Controls.Add(this.lblAddOS);
            this.groupBox4.Controls.Add(this.cboTypeOS);
            this.groupBox4.Controls.Add(this.label30);
            this.groupBox4.Controls.Add(this.label31);
            this.groupBox4.Controls.Add(this.label32);
            this.groupBox4.Location = new System.Drawing.Point(686, 5);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(513, 315);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 237);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(71, 13);
            this.label14.TabIndex = 113;
            this.label14.Text = "Taille écran : ";
            // 
            // txtNumeroCle
            // 
            this.txtNumeroCle.Location = new System.Drawing.Point(366, 159);
            this.txtNumeroCle.Name = "txtNumeroCle";
            this.txtNumeroCle.Size = new System.Drawing.Size(109, 20);
            this.txtNumeroCle.TabIndex = 41;
            // 
            // cboTensionAdap
            // 
            this.cboTensionAdap.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboTensionAdap.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboTensionAdap.FormattingEnabled = true;
            this.cboTensionAdap.Location = new System.Drawing.Point(366, 85);
            this.cboTensionAdap.Name = "cboTensionAdap";
            this.cboTensionAdap.Size = new System.Drawing.Size(109, 21);
            this.cboTensionAdap.TabIndex = 38;
            this.cboTensionAdap.DropDown += new System.EventHandler(this.cboTensionAdap_DropDown);
            // 
            // cboPuissanceAdap
            // 
            this.cboPuissanceAdap.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboPuissanceAdap.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboPuissanceAdap.FormattingEnabled = true;
            this.cboPuissanceAdap.Location = new System.Drawing.Point(366, 109);
            this.cboPuissanceAdap.Name = "cboPuissanceAdap";
            this.cboPuissanceAdap.Size = new System.Drawing.Size(109, 21);
            this.cboPuissanceAdap.TabIndex = 39;
            this.cboPuissanceAdap.DropDown += new System.EventHandler(this.cboPuissanceAdap_DropDown);
            // 
            // cboNbrHDMI
            // 
            this.cboNbrHDMI.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboNbrHDMI.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboNbrHDMI.FormattingEnabled = true;
            this.cboNbrHDMI.Location = new System.Drawing.Point(366, 11);
            this.cboNbrHDMI.Name = "cboNbrHDMI";
            this.cboNbrHDMI.Size = new System.Drawing.Size(109, 21);
            this.cboNbrHDMI.TabIndex = 35;
            this.cboNbrHDMI.DropDown += new System.EventHandler(this.cboNbrHDMI_DropDown);
            // 
            // cboIntensiteAdap
            // 
            this.cboIntensiteAdap.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboIntensiteAdap.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboIntensiteAdap.FormattingEnabled = true;
            this.cboIntensiteAdap.Location = new System.Drawing.Point(366, 134);
            this.cboIntensiteAdap.Name = "cboIntensiteAdap";
            this.cboIntensiteAdap.Size = new System.Drawing.Size(109, 21);
            this.cboIntensiteAdap.TabIndex = 40;
            this.cboIntensiteAdap.DropDown += new System.EventHandler(this.cboIntensiteAdap_DropDown);
            // 
            // cboNbrVGA
            // 
            this.cboNbrVGA.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboNbrVGA.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboNbrVGA.FormattingEnabled = true;
            this.cboNbrVGA.Location = new System.Drawing.Point(366, 36);
            this.cboNbrVGA.Name = "cboNbrVGA";
            this.cboNbrVGA.Size = new System.Drawing.Size(109, 21);
            this.cboNbrVGA.TabIndex = 36;
            this.cboNbrVGA.DropDown += new System.EventHandler(this.cboNbrVGA_DropDown);
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Location = new System.Drawing.Point(252, 162);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(70, 13);
            this.label46.TabIndex = 86;
            this.label46.Text = "Numéro clé : ";
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Location = new System.Drawing.Point(252, 138);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(105, 13);
            this.label47.TabIndex = 85;
            this.label47.Text = "Intensité adapt. (A) : ";
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Location = new System.Drawing.Point(252, 113);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(118, 13);
            this.label48.TabIndex = 83;
            this.label48.Text = "Puissance adapt. (W) : ";
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Location = new System.Drawing.Point(252, 89);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(103, 13);
            this.label49.TabIndex = 82;
            this.label49.Text = "Tension adapt. (V) : ";
            // 
            // cboTensionBatt
            // 
            this.cboTensionBatt.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboTensionBatt.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboTensionBatt.FormattingEnabled = true;
            this.cboTensionBatt.Location = new System.Drawing.Point(366, 60);
            this.cboTensionBatt.Name = "cboTensionBatt";
            this.cboTensionBatt.Size = new System.Drawing.Size(109, 21);
            this.cboTensionBatt.TabIndex = 37;
            this.cboTensionBatt.DropDown += new System.EventHandler(this.cboTensionBatt_DropDown);
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.Location = new System.Drawing.Point(252, 64);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(108, 13);
            this.label50.TabIndex = 79;
            this.label50.Text = "Tension batterie (V) : ";
            // 
            // label51
            // 
            this.label51.AutoSize = true;
            this.label51.Location = new System.Drawing.Point(252, 39);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(61, 13);
            this.label51.TabIndex = 78;
            this.label51.Text = "Nbr. VGA : ";
            // 
            // label52
            // 
            this.label52.AutoSize = true;
            this.label52.Location = new System.Drawing.Point(252, 16);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(64, 13);
            this.label52.TabIndex = 77;
            this.label52.Text = "Nbr. HDMI :";
            // 
            // cboTypeHDD
            // 
            this.cboTypeHDD.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboTypeHDD.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboTypeHDD.FormattingEnabled = true;
            this.cboTypeHDD.Location = new System.Drawing.Point(109, 159);
            this.cboTypeHDD.Name = "cboTypeHDD";
            this.cboTypeHDD.Size = new System.Drawing.Size(109, 21);
            this.cboTypeHDD.TabIndex = 29;
            this.cboTypeHDD.DropDown += new System.EventHandler(this.cboNbrHDD_DropDown);
            // 
            // cboNbrCoeurProcesseur
            // 
            this.cboNbrCoeurProcesseur.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboNbrCoeurProcesseur.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboNbrCoeurProcesseur.FormattingEnabled = true;
            this.cboNbrCoeurProcesseur.Location = new System.Drawing.Point(109, 134);
            this.cboNbrCoeurProcesseur.Name = "cboNbrCoeurProcesseur";
            this.cboNbrCoeurProcesseur.Size = new System.Drawing.Size(109, 21);
            this.cboNbrCoeurProcesseur.TabIndex = 28;
            this.cboNbrCoeurProcesseur.DropDown += new System.EventHandler(this.cboNbrCoeurProcesseur_DropDown);
            // 
            // lblAddTypeClavier
            // 
            this.lblAddTypeClavier.AutoSize = true;
            this.lblAddTypeClavier.Location = new System.Drawing.Point(221, 39);
            this.lblAddTypeClavier.Name = "lblAddTypeClavier";
            this.lblAddTypeClavier.Size = new System.Drawing.Size(29, 13);
            this.lblAddTypeClavier.TabIndex = 23;
            this.lblAddTypeClavier.TabStop = true;
            this.lblAddTypeClavier.Text = "New";
            this.lblAddTypeClavier.VisitedLinkColor = System.Drawing.Color.Blue;
            this.lblAddTypeClavier.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblAddTypeClavier_LinkClicked);
            // 
            // cboTypeClavier
            // 
            this.cboTypeClavier.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTypeClavier.FormattingEnabled = true;
            this.cboTypeClavier.Location = new System.Drawing.Point(109, 36);
            this.cboTypeClavier.Name = "cboTypeClavier";
            this.cboTypeClavier.Size = new System.Drawing.Size(109, 21);
            this.cboTypeClavier.TabIndex = 22;
            this.cboTypeClavier.DropDown += new System.EventHandler(this.cboTypeClavier_DropDown);
            // 
            // lblAddPC
            // 
            this.lblAddPC.AutoSize = true;
            this.lblAddPC.Location = new System.Drawing.Point(221, 14);
            this.lblAddPC.Name = "lblAddPC";
            this.lblAddPC.Size = new System.Drawing.Size(29, 13);
            this.lblAddPC.TabIndex = 21;
            this.lblAddPC.TabStop = true;
            this.lblAddPC.Text = "New";
            this.lblAddPC.VisitedLinkColor = System.Drawing.Color.Blue;
            this.lblAddPC.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblAddPC_LinkClicked);
            // 
            // cboTypeOrdi
            // 
            this.cboTypeOrdi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTypeOrdi.FormattingEnabled = true;
            this.cboTypeOrdi.Location = new System.Drawing.Point(109, 11);
            this.cboTypeOrdi.Name = "cboTypeOrdi";
            this.cboTypeOrdi.Size = new System.Drawing.Size(109, 21);
            this.cboTypeOrdi.TabIndex = 20;
            this.cboTypeOrdi.DropDown += new System.EventHandler(this.cboTypeOrdi_DropDown);
            // 
            // cboUSB3
            // 
            this.cboUSB3.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboUSB3.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboUSB3.FormattingEnabled = true;
            this.cboUSB3.Location = new System.Drawing.Point(109, 283);
            this.cboUSB3.Name = "cboUSB3";
            this.cboUSB3.Size = new System.Drawing.Size(109, 21);
            this.cboUSB3.TabIndex = 34;
            this.cboUSB3.DropDown += new System.EventHandler(this.cboUSB3_DropDown);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(4, 287);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(79, 13);
            this.label21.TabIndex = 62;
            this.label21.Text = "Nbr. USB 3.0 : ";
            // 
            // cboUSB2
            // 
            this.cboUSB2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboUSB2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboUSB2.FormattingEnabled = true;
            this.cboUSB2.Location = new System.Drawing.Point(109, 258);
            this.cboUSB2.Name = "cboUSB2";
            this.cboUSB2.Size = new System.Drawing.Size(109, 21);
            this.cboUSB2.TabIndex = 33;
            this.cboUSB2.DropDown += new System.EventHandler(this.cboUSB2_DropDown);
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(4, 262);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(79, 13);
            this.label22.TabIndex = 59;
            this.label22.Text = "Nbr. USB 2.0 : ";
            // 
            // cboNbrHDD
            // 
            this.cboNbrHDD.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboNbrHDD.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboNbrHDD.FormattingEnabled = true;
            this.cboNbrHDD.Location = new System.Drawing.Point(109, 208);
            this.cboNbrHDD.Name = "cboNbrHDD";
            this.cboNbrHDD.Size = new System.Drawing.Size(109, 21);
            this.cboNbrHDD.TabIndex = 31;
            this.cboNbrHDD.DropDown += new System.EventHandler(this.cboIndicePC_DropDown);
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(4, 212);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(80, 13);
            this.label24.TabIndex = 53;
            this.label24.Text = "Nombre HDD : ";
            // 
            // cboCapaciteHDD
            // 
            this.cboCapaciteHDD.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboCapaciteHDD.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboCapaciteHDD.FormattingEnabled = true;
            this.cboCapaciteHDD.Location = new System.Drawing.Point(109, 184);
            this.cboCapaciteHDD.Name = "cboCapaciteHDD";
            this.cboCapaciteHDD.Size = new System.Drawing.Size(109, 21);
            this.cboCapaciteHDD.TabIndex = 30;
            this.cboCapaciteHDD.DropDown += new System.EventHandler(this.cboCapaciteHDD_DropDown);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(4, 187);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(108, 13);
            this.label25.TabIndex = 50;
            this.label25.Text = "Capacité HDD (Gb) : ";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(4, 162);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(67, 13);
            this.label26.TabIndex = 48;
            this.label26.Text = "Type HDD : ";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(4, 138);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(91, 13);
            this.label27.TabIndex = 46;
            this.label27.Text = "Nbr. des coeurs : ";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(4, 113);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(97, 13);
            this.label28.TabIndex = 44;
            this.label28.Text = "Processeur (Ghz) : ";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(4, 89);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(103, 13);
            this.label29.TabIndex = 41;
            this.label29.Text = "Mémoire RAM (Gb): ";
            // 
            // lblAddOS
            // 
            this.lblAddOS.AutoSize = true;
            this.lblAddOS.Location = new System.Drawing.Point(221, 63);
            this.lblAddOS.Name = "lblAddOS";
            this.lblAddOS.Size = new System.Drawing.Size(29, 13);
            this.lblAddOS.TabIndex = 25;
            this.lblAddOS.TabStop = true;
            this.lblAddOS.Text = "New";
            this.lblAddOS.VisitedLinkColor = System.Drawing.Color.Blue;
            this.lblAddOS.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblAddOS_LinkClicked);
            // 
            // cboTypeOS
            // 
            this.cboTypeOS.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboTypeOS.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboTypeOS.FormattingEnabled = true;
            this.cboTypeOS.Location = new System.Drawing.Point(109, 60);
            this.cboTypeOS.Name = "cboTypeOS";
            this.cboTypeOS.Size = new System.Drawing.Size(109, 21);
            this.cboTypeOS.TabIndex = 24;
            this.cboTypeOS.DropDown += new System.EventHandler(this.cboTypeOS_DropDown);
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(4, 64);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(89, 13);
            this.label30.TabIndex = 37;
            this.label30.Text = "Type Sys. Expl. : ";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(4, 39);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(74, 13);
            this.label31.TabIndex = 36;
            this.label31.Text = "Type clavier : ";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(4, 15);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(87, 13);
            this.label32.TabIndex = 34;
            this.label32.Text = "Type ordinateur :";
            // 
            // cboRAM
            // 
            this.cboRAM.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboRAM.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboRAM.FormattingEnabled = true;
            this.cboRAM.Location = new System.Drawing.Point(109, 85);
            this.cboRAM.Name = "cboRAM";
            this.cboRAM.Size = new System.Drawing.Size(109, 21);
            this.cboRAM.TabIndex = 26;
            // 
            // cboProcesseur
            // 
            this.cboProcesseur.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboProcesseur.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboProcesseur.FormattingEnabled = true;
            this.cboProcesseur.Location = new System.Drawing.Point(109, 109);
            this.cboProcesseur.Name = "cboProcesseur";
            this.cboProcesseur.Size = new System.Drawing.Size(109, 21);
            this.cboProcesseur.TabIndex = 27;
            // 
            // lblAddRAM
            // 
            this.lblAddRAM.AutoSize = true;
            this.lblAddRAM.Location = new System.Drawing.Point(221, 88);
            this.lblAddRAM.Name = "lblAddRAM";
            this.lblAddRAM.Size = new System.Drawing.Size(29, 13);
            this.lblAddRAM.TabIndex = 114;
            this.lblAddRAM.TabStop = true;
            this.lblAddRAM.Text = "New";
            this.lblAddRAM.VisitedLinkColor = System.Drawing.Color.Blue;
            // 
            // lblAddProcessor
            // 
            this.lblAddProcessor.AutoSize = true;
            this.lblAddProcessor.Location = new System.Drawing.Point(222, 112);
            this.lblAddProcessor.Name = "lblAddProcessor";
            this.lblAddProcessor.Size = new System.Drawing.Size(29, 13);
            this.lblAddProcessor.TabIndex = 115;
            this.lblAddProcessor.TabStop = true;
            this.lblAddProcessor.Text = "New";
            this.lblAddProcessor.VisitedLinkColor = System.Drawing.Color.Blue;
            // 
            // lblAddTypeHDD
            // 
            this.lblAddTypeHDD.AutoSize = true;
            this.lblAddTypeHDD.Location = new System.Drawing.Point(222, 164);
            this.lblAddTypeHDD.Name = "lblAddTypeHDD";
            this.lblAddTypeHDD.Size = new System.Drawing.Size(29, 13);
            this.lblAddTypeHDD.TabIndex = 117;
            this.lblAddTypeHDD.TabStop = true;
            this.lblAddTypeHDD.Text = "New";
            this.lblAddTypeHDD.VisitedLinkColor = System.Drawing.Color.Blue;
            // 
            // lblAddCorProcessor
            // 
            this.lblAddCorProcessor.AutoSize = true;
            this.lblAddCorProcessor.Location = new System.Drawing.Point(221, 139);
            this.lblAddCorProcessor.Name = "lblAddCorProcessor";
            this.lblAddCorProcessor.Size = new System.Drawing.Size(29, 13);
            this.lblAddCorProcessor.TabIndex = 116;
            this.lblAddCorProcessor.TabStop = true;
            this.lblAddCorProcessor.Text = "New";
            this.lblAddCorProcessor.VisitedLinkColor = System.Drawing.Color.Blue;
            // 
            // lblAddNbrHDD
            // 
            this.lblAddNbrHDD.AutoSize = true;
            this.lblAddNbrHDD.Location = new System.Drawing.Point(222, 212);
            this.lblAddNbrHDD.Name = "lblAddNbrHDD";
            this.lblAddNbrHDD.Size = new System.Drawing.Size(29, 13);
            this.lblAddNbrHDD.TabIndex = 119;
            this.lblAddNbrHDD.TabStop = true;
            this.lblAddNbrHDD.Text = "New";
            this.lblAddNbrHDD.VisitedLinkColor = System.Drawing.Color.Blue;
            // 
            // lblAddCapacityHDD
            // 
            this.lblAddCapacityHDD.AutoSize = true;
            this.lblAddCapacityHDD.Location = new System.Drawing.Point(222, 187);
            this.lblAddCapacityHDD.Name = "lblAddCapacityHDD";
            this.lblAddCapacityHDD.Size = new System.Drawing.Size(29, 13);
            this.lblAddCapacityHDD.TabIndex = 118;
            this.lblAddCapacityHDD.TabStop = true;
            this.lblAddCapacityHDD.Text = "New";
            this.lblAddCapacityHDD.VisitedLinkColor = System.Drawing.Color.Blue;
            // 
            // cboTailleEcran
            // 
            this.cboTailleEcran.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboTailleEcran.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboTailleEcran.FormattingEnabled = true;
            this.cboTailleEcran.Location = new System.Drawing.Point(109, 233);
            this.cboTailleEcran.Name = "cboTailleEcran";
            this.cboTailleEcran.Size = new System.Drawing.Size(109, 21);
            this.cboTailleEcran.TabIndex = 32;
            // 
            // lblAddUSB2
            // 
            this.lblAddUSB2.AutoSize = true;
            this.lblAddUSB2.Location = new System.Drawing.Point(221, 261);
            this.lblAddUSB2.Name = "lblAddUSB2";
            this.lblAddUSB2.Size = new System.Drawing.Size(29, 13);
            this.lblAddUSB2.TabIndex = 121;
            this.lblAddUSB2.TabStop = true;
            this.lblAddUSB2.Text = "New";
            this.lblAddUSB2.VisitedLinkColor = System.Drawing.Color.Blue;
            // 
            // lblAddScreen
            // 
            this.lblAddScreen.AutoSize = true;
            this.lblAddScreen.Location = new System.Drawing.Point(221, 236);
            this.lblAddScreen.Name = "lblAddScreen";
            this.lblAddScreen.Size = new System.Drawing.Size(29, 13);
            this.lblAddScreen.TabIndex = 120;
            this.lblAddScreen.TabStop = true;
            this.lblAddScreen.Text = "New";
            this.lblAddScreen.VisitedLinkColor = System.Drawing.Color.Blue;
            // 
            // lblAddUSB3
            // 
            this.lblAddUSB3.AutoSize = true;
            this.lblAddUSB3.Location = new System.Drawing.Point(221, 287);
            this.lblAddUSB3.Name = "lblAddUSB3";
            this.lblAddUSB3.Size = new System.Drawing.Size(29, 13);
            this.lblAddUSB3.TabIndex = 122;
            this.lblAddUSB3.TabStop = true;
            this.lblAddUSB3.Text = "New";
            this.lblAddUSB3.VisitedLinkColor = System.Drawing.Color.Blue;
            // 
            // lblAddIAdapt
            // 
            this.lblAddIAdapt.AutoSize = true;
            this.lblAddIAdapt.Location = new System.Drawing.Point(477, 141);
            this.lblAddIAdapt.Name = "lblAddIAdapt";
            this.lblAddIAdapt.Size = new System.Drawing.Size(29, 13);
            this.lblAddIAdapt.TabIndex = 128;
            this.lblAddIAdapt.TabStop = true;
            this.lblAddIAdapt.Text = "New";
            this.lblAddIAdapt.VisitedLinkColor = System.Drawing.Color.Blue;
            // 
            // lblAddPAdapt
            // 
            this.lblAddPAdapt.AutoSize = true;
            this.lblAddPAdapt.Location = new System.Drawing.Point(478, 113);
            this.lblAddPAdapt.Name = "lblAddPAdapt";
            this.lblAddPAdapt.Size = new System.Drawing.Size(29, 13);
            this.lblAddPAdapt.TabIndex = 127;
            this.lblAddPAdapt.TabStop = true;
            this.lblAddPAdapt.Text = "New";
            this.lblAddPAdapt.VisitedLinkColor = System.Drawing.Color.Blue;
            // 
            // lblAddUAdapt
            // 
            this.lblAddUAdapt.AutoSize = true;
            this.lblAddUAdapt.Location = new System.Drawing.Point(477, 89);
            this.lblAddUAdapt.Name = "lblAddUAdapt";
            this.lblAddUAdapt.Size = new System.Drawing.Size(29, 13);
            this.lblAddUAdapt.TabIndex = 126;
            this.lblAddUAdapt.TabStop = true;
            this.lblAddUAdapt.Text = "New";
            this.lblAddUAdapt.VisitedLinkColor = System.Drawing.Color.Blue;
            // 
            // lblAddVGA
            // 
            this.lblAddVGA.AutoSize = true;
            this.lblAddVGA.Location = new System.Drawing.Point(477, 40);
            this.lblAddVGA.Name = "lblAddVGA";
            this.lblAddVGA.Size = new System.Drawing.Size(29, 13);
            this.lblAddVGA.TabIndex = 124;
            this.lblAddVGA.TabStop = true;
            this.lblAddVGA.Text = "New";
            this.lblAddVGA.VisitedLinkColor = System.Drawing.Color.Blue;
            // 
            // lblAddHDMI
            // 
            this.lblAddHDMI.AutoSize = true;
            this.lblAddHDMI.Location = new System.Drawing.Point(477, 15);
            this.lblAddHDMI.Name = "lblAddHDMI";
            this.lblAddHDMI.Size = new System.Drawing.Size(29, 13);
            this.lblAddHDMI.TabIndex = 123;
            this.lblAddHDMI.TabStop = true;
            this.lblAddHDMI.Text = "New";
            this.lblAddHDMI.VisitedLinkColor = System.Drawing.Color.Blue;
            // 
            // lblAddUBatterie
            // 
            this.lblAddUBatterie.AutoSize = true;
            this.lblAddUBatterie.Location = new System.Drawing.Point(477, 64);
            this.lblAddUBatterie.Name = "lblAddUBatterie";
            this.lblAddUBatterie.Size = new System.Drawing.Size(29, 13);
            this.lblAddUBatterie.TabIndex = 125;
            this.lblAddUBatterie.TabStop = true;
            this.lblAddUBatterie.Text = "New";
            this.lblAddUBatterie.VisitedLinkColor = System.Drawing.Color.Blue;
            // 
            // lblAddGuaratie
            // 
            this.lblAddGuaratie.AutoSize = true;
            this.lblAddGuaratie.Location = new System.Drawing.Point(214, 165);
            this.lblAddGuaratie.Name = "lblAddGuaratie";
            this.lblAddGuaratie.Size = new System.Drawing.Size(29, 13);
            this.lblAddGuaratie.TabIndex = 157;
            this.lblAddGuaratie.TabStop = true;
            this.lblAddGuaratie.Text = "New";
            this.lblAddGuaratie.VisitedLinkColor = System.Drawing.Color.Blue;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label23);
            this.groupBox6.ForeColor = System.Drawing.Color.Crimson;
            this.groupBox6.Location = new System.Drawing.Point(255, 178);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(115, 131);
            this.groupBox6.TabIndex = 129;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Duréé restante avant fin garantie : ";
            // 
            // label23
            // 
            this.label23.BackColor = System.Drawing.Color.White;
            this.label23.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label23.ForeColor = System.Drawing.Color.OrangeRed;
            this.label23.Location = new System.Drawing.Point(4, 36);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(105, 90);
            this.label23.TabIndex = 130;
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmOrdinateur
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.ClientSize = new System.Drawing.Size(1207, 560);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmOrdinateur";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Text = "Identification d\'un ordinateur";
            this.Activated += new System.EventHandler(this.frmOrdinateur_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmOrdinateur_FormClosed);
            this.Load += new System.EventHandler(this.frmOrdinateur_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPhoto3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPhoto2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPhoto1)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbQRCode)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.PictureBox pbQRCode;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.LinkLabel lblAddNumCompte;
        private System.Windows.Forms.ComboBox cboNumCompte;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtIdentidiant;
        private System.Windows.Forms.LinkLabel lblAddCategorieMat;
        private System.Windows.Forms.ComboBox cboCatMateriel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker txtDateAcquisition;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.LinkLabel lblEtatMatriel;
        private System.Windows.Forms.ComboBox cboEtat;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.LinkLabel lblAddPoids;
        private System.Windows.Forms.ComboBox cboPoids;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.LinkLabel lblAddCouleur;
        private System.Windows.Forms.ComboBox cboCouleur;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.LinkLabel lblAddModele;
        private System.Windows.Forms.ComboBox cboModele;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.LinkLabel lblAddMarque;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox cboGuarantie;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtCommentaire;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtMAC2;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.LinkLabel lblPhoto1;
        private System.Windows.Forms.PictureBox pbPhoto1;
        private System.Windows.Forms.TextBox txtMAC1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtModifieBy;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtDateCreate;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txtCreateBy;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.LinkLabel lblPhoto3;
        private System.Windows.Forms.LinkLabel lblPhoto2;
        private System.Windows.Forms.PictureBox pbPhoto3;
        private System.Windows.Forms.PictureBox pbPhoto2;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.ComboBox cboNbrHDD;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.LinkLabel lblAddOS;
        private System.Windows.Forms.ComboBox cboTypeOS;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.LinkLabel lblAddPC;
        private System.Windows.Forms.ComboBox cboTypeOrdi;
        private System.Windows.Forms.ComboBox cboTypeHDD;
        private System.Windows.Forms.ComboBox cboNbrCoeurProcesseur;
        private System.Windows.Forms.LinkLabel lblAddTypeClavier;
        private System.Windows.Forms.ComboBox cboTypeClavier;
        private System.Windows.Forms.ComboBox cboCapaciteHDD;
        private System.Windows.Forms.RichTextBox txtQRCode;
        private System.Windows.Forms.ComboBox cboUSB3;
        private System.Windows.Forms.ComboBox cboUSB2;
        private System.Windows.Forms.ComboBox cboNbrHDMI;
        private System.Windows.Forms.ComboBox cboIntensiteAdap;
        private System.Windows.Forms.ComboBox cboNbrVGA;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.ComboBox cboTensionBatt;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.Label label52;
        private System.Windows.Forms.ComboBox cboTensionAdap;
        private System.Windows.Forms.ComboBox cboPuissanceAdap;
        private System.Windows.Forms.TextBox txtNumeroCle;
        private System.Windows.Forms.TextBox txtDateModifie;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCode_str;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId_categorie_materiel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId_compte;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQrcode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate_acquisition;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGuarantie;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId_marque;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId_modele;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId_couleur;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId_poids;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId_etat_materiel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPhoto1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPhoto2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPhoto3;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMac_adresse1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMac_adresse2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCommentaire;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUser_created;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate_created;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUser_modified;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate_modified;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId_type_ordinateur;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId_type_clavier;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId_os;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRam;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProcesseur;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNombre_coeur_processeur;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNombre_hdd;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCapacite_hdd;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIndice_performance;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPouce;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNombre_usb2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNombre_usb3;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNombre_hdmi;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNombre_vga;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTension_batterie;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTension_adaptateur;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPuissance_adaptateur;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIntensite_adaptateur;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNumero_cle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId_type_imprimante;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPuissance;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIntensite;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNombre_page_par_minute;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId_type_amplificateur;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTension_alimentation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNombre_usb;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNombre_memoire;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNombre_sorties_audio;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNombre_microphone;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGain;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId_type_routeur_ap;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId_version_ios;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNombre_gbe;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNombre_fe;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNombre_fo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNombre_serial;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCapable_usb;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMotpasse_defaut;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDefault_ip;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNombre_console;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNombre_auxiliaire;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId_type_ap;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId_type_switch;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFrequence;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAlimentation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNombre_antenne;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId_netette;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCompatible_wifi;
        private System.Windows.Forms.ComboBox cboMarque;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox cboProcesseur;
        private System.Windows.Forms.ComboBox cboRAM;
        private System.Windows.Forms.LinkLabel lblAddNbrHDD;
        private System.Windows.Forms.LinkLabel lblAddCapacityHDD;
        private System.Windows.Forms.LinkLabel lblAddTypeHDD;
        private System.Windows.Forms.LinkLabel lblAddCorProcessor;
        private System.Windows.Forms.LinkLabel lblAddProcessor;
        private System.Windows.Forms.LinkLabel lblAddRAM;
        private System.Windows.Forms.LinkLabel lblAddIAdapt;
        private System.Windows.Forms.LinkLabel lblAddPAdapt;
        private System.Windows.Forms.LinkLabel lblAddUAdapt;
        private System.Windows.Forms.LinkLabel lblAddVGA;
        private System.Windows.Forms.LinkLabel lblAddHDMI;
        private System.Windows.Forms.LinkLabel lblAddUBatterie;
        private System.Windows.Forms.LinkLabel lblAddUSB2;
        private System.Windows.Forms.LinkLabel lblAddScreen;
        private System.Windows.Forms.LinkLabel lblAddUSB3;
        private System.Windows.Forms.ComboBox cboTailleEcran;
        private System.Windows.Forms.LinkLabel lblAddGuaratie;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label23;
    }
}