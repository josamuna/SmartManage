namespace smartManage.Desktop
{
    partial class frmUtilisateur
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
            this.cmdCancel = new System.Windows.Forms.Button();
            this.tabMainManage = new System.Windows.Forms.TabControl();
            this.tabViewSearch = new System.Windows.Forms.TabPage();
            this.txtSeach = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.colDesignation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNomAgent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colActivation = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colNiveau = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSchema_user = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMotpass = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colId_personne = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabCreate = new System.Windows.Forms.TabPage();
            this.gpParamServeur = new System.Windows.Forms.GroupBox();
            this.txtVersion = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cmdLoadParam = new System.Windows.Forms.Button();
            this.cmdEnregistrer = new System.Windows.Forms.Button();
            this.txtBD = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtServeur = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.chkParamServeur = new System.Windows.Forms.CheckBox();
            this.cmdNouveauUser = new System.Windows.Forms.Button();
            this.cboAgent = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.cmdValiderUser = new System.Windows.Forms.Button();
            this.chkActivationUser = new System.Windows.Forms.CheckBox();
            this.txtMotPasse = new System.Windows.Forms.TextBox();
            this.txtNomUser = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabModiSup = new System.Windows.Forms.TabPage();
            this.tabManage = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdActivationUser = new System.Windows.Forms.RadioButton();
            this.rdPwdSeul = new System.Windows.Forms.RadioButton();
            this.rdUserEtPwd = new System.Windows.Forms.RadioButton();
            this.rdUserSeul = new System.Windows.Forms.RadioButton();
            this.txtNewUser = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lblCompte = new System.Windows.Forms.Label();
            this.chkActivationUserModi = new System.Windows.Forms.CheckBox();
            this.cmdModifierCompte = new System.Windows.Forms.Button();
            this.cboUtilisateur = new System.Windows.Forms.ComboBox();
            this.txtNewMotPasse = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtOldMotPasse = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.cmdDelete = new System.Windows.Forms.Button();
            this.CboUserSup = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.tabDroit = new System.Windows.Forms.TabPage();
            this.dgv1 = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSchema_user1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colnom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmdAfficherDroit = new System.Windows.Forms.Button();
            this.cmdRetirerDroit = new System.Windows.Forms.Button();
            this.cmdAccorderDroit = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkLevelAdmin = new System.Windows.Forms.CheckBox();
            this.chkLevelAdminstrateur = new System.Windows.Forms.CheckBox();
            this.chkLevelUser = new System.Windows.Forms.CheckBox();
            this.cboUtilisateur1 = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tabMainManage.SuspendLayout();
            this.tabViewSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.tabCreate.SuspendLayout();
            this.gpParamServeur.SuspendLayout();
            this.tabModiSup.SuspendLayout();
            this.tabManage.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabDroit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdCancel.ForeColor = System.Drawing.Color.Blue;
            this.cmdCancel.Location = new System.Drawing.Point(838, 286);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(61, 27);
            this.cmdCancel.TabIndex = 155;
            this.cmdCancel.Text = "&Fermer";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // tabMainManage
            // 
            this.tabMainManage.Controls.Add(this.tabViewSearch);
            this.tabMainManage.Controls.Add(this.tabCreate);
            this.tabMainManage.Controls.Add(this.tabModiSup);
            this.tabMainManage.Controls.Add(this.tabDroit);
            this.tabMainManage.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabMainManage.Location = new System.Drawing.Point(0, 0);
            this.tabMainManage.Name = "tabMainManage";
            this.tabMainManage.SelectedIndex = 0;
            this.tabMainManage.Size = new System.Drawing.Size(911, 279);
            this.tabMainManage.TabIndex = 154;
            this.tabMainManage.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabMainManage_Selecting);
            // 
            // tabViewSearch
            // 
            this.tabViewSearch.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabViewSearch.Controls.Add(this.txtSeach);
            this.tabViewSearch.Controls.Add(this.label14);
            this.tabViewSearch.Controls.Add(this.dgv);
            this.tabViewSearch.Location = new System.Drawing.Point(4, 22);
            this.tabViewSearch.Name = "tabViewSearch";
            this.tabViewSearch.Size = new System.Drawing.Size(903, 253);
            this.tabViewSearch.TabIndex = 2;
            this.tabViewSearch.Text = "Visualisation et recherche";
            // 
            // txtSeach
            // 
            this.txtSeach.Location = new System.Drawing.Point(132, 10);
            this.txtSeach.Name = "txtSeach";
            this.txtSeach.Size = new System.Drawing.Size(179, 20);
            this.txtSeach.TabIndex = 0;
            this.txtSeach.TextChanged += new System.EventHandler(this.txtSeach_TextChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label14.ForeColor = System.Drawing.Color.Red;
            this.label14.Location = new System.Drawing.Point(63, 13);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(69, 13);
            this.label14.TabIndex = 287;
            this.label14.Text = "Rechercher :";
            // 
            // dgv
            // 
            this.dgv.BackgroundColor = System.Drawing.Color.White;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDesignation,
            this.colNomAgent,
            this.colActivation,
            this.colNiveau,
            this.colSchema_user,
            this.colMotpass,
            this.colId_personne,
            this.dataGridViewTextBoxColumn1});
            this.dgv.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgv.GridColor = System.Drawing.SystemColors.AppWorkspace;
            this.dgv.Location = new System.Drawing.Point(0, 38);
            this.dgv.MultiSelect = false;
            this.dgv.Name = "dgv";
            this.dgv.ReadOnly = true;
            this.dgv.RowHeadersVisible = false;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.Size = new System.Drawing.Size(903, 215);
            this.dgv.TabIndex = 1;
            this.dgv.SelectionChanged += new System.EventHandler(this.dgv_SelectionChanged);
            this.dgv.DoubleClick += new System.EventHandler(this.dgv_DoubleClick);
            // 
            // colDesignation
            // 
            this.colDesignation.DataPropertyName = "Nomuser";
            this.colDesignation.HeaderText = "Username";
            this.colDesignation.Name = "colDesignation";
            this.colDesignation.ReadOnly = true;
            this.colDesignation.Width = 150;
            // 
            // colNomAgent
            // 
            this.colNomAgent.DataPropertyName = "Nom";
            this.colNomAgent.HeaderText = "Nom";
            this.colNomAgent.Name = "colNomAgent";
            this.colNomAgent.ReadOnly = true;
            this.colNomAgent.Width = 250;
            // 
            // colActivation
            // 
            this.colActivation.DataPropertyName = "Activation";
            this.colActivation.HeaderText = "Activation";
            this.colActivation.Name = "colActivation";
            this.colActivation.ReadOnly = true;
            // 
            // colNiveau
            // 
            this.colNiveau.DataPropertyName = "Droits";
            this.colNiveau.HeaderText = "Droits d\'accès";
            this.colNiveau.Name = "colNiveau";
            this.colNiveau.ReadOnly = true;
            this.colNiveau.Width = 395;
            // 
            // colSchema_user
            // 
            this.colSchema_user.DataPropertyName = "Schema_user";
            this.colSchema_user.HeaderText = "Schema_user";
            this.colSchema_user.Name = "colSchema_user";
            this.colSchema_user.ReadOnly = true;
            this.colSchema_user.Visible = false;
            // 
            // colMotpass
            // 
            this.colMotpass.DataPropertyName = "Motpass";
            this.colMotpass.HeaderText = "Mot de passe";
            this.colMotpass.Name = "colMotpass";
            this.colMotpass.ReadOnly = true;
            this.colMotpass.Visible = false;
            this.colMotpass.Width = 120;
            // 
            // colId_personne
            // 
            this.colId_personne.DataPropertyName = "Id_personne";
            this.colId_personne.HeaderText = "Id_personne";
            this.colId_personne.Name = "colId_personne";
            this.colId_personne.ReadOnly = true;
            this.colId_personne.Visible = false;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Id";
            this.dataGridViewTextBoxColumn1.HeaderText = "ID";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Visible = false;
            // 
            // tabCreate
            // 
            this.tabCreate.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabCreate.Controls.Add(this.gpParamServeur);
            this.tabCreate.Controls.Add(this.chkParamServeur);
            this.tabCreate.Controls.Add(this.cmdNouveauUser);
            this.tabCreate.Controls.Add(this.cboAgent);
            this.tabCreate.Controls.Add(this.label13);
            this.tabCreate.Controls.Add(this.cmdValiderUser);
            this.tabCreate.Controls.Add(this.chkActivationUser);
            this.tabCreate.Controls.Add(this.txtMotPasse);
            this.tabCreate.Controls.Add(this.txtNomUser);
            this.tabCreate.Controls.Add(this.label2);
            this.tabCreate.Controls.Add(this.label1);
            this.tabCreate.Location = new System.Drawing.Point(4, 22);
            this.tabCreate.Name = "tabCreate";
            this.tabCreate.Padding = new System.Windows.Forms.Padding(3);
            this.tabCreate.Size = new System.Drawing.Size(903, 253);
            this.tabCreate.TabIndex = 0;
            this.tabCreate.Text = "Création";
            // 
            // gpParamServeur
            // 
            this.gpParamServeur.Controls.Add(this.txtVersion);
            this.gpParamServeur.Controls.Add(this.label9);
            this.gpParamServeur.Controls.Add(this.cmdLoadParam);
            this.gpParamServeur.Controls.Add(this.cmdEnregistrer);
            this.gpParamServeur.Controls.Add(this.txtBD);
            this.gpParamServeur.Controls.Add(this.label3);
            this.gpParamServeur.Controls.Add(this.txtServeur);
            this.gpParamServeur.Controls.Add(this.label8);
            this.gpParamServeur.Location = new System.Drawing.Point(390, 44);
            this.gpParamServeur.Name = "gpParamServeur";
            this.gpParamServeur.Size = new System.Drawing.Size(303, 134);
            this.gpParamServeur.TabIndex = 19;
            this.gpParamServeur.TabStop = false;
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(113, 68);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.Size = new System.Drawing.Size(179, 20);
            this.txtVersion.TabIndex = 8;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 71);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(98, 13);
            this.label9.TabIndex = 10;
            this.label9.Text = "Type version SQL :";
            // 
            // cmdLoadParam
            // 
            this.cmdLoadParam.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdLoadParam.ForeColor = System.Drawing.Color.DarkOliveGreen;
            this.cmdLoadParam.Location = new System.Drawing.Point(113, 95);
            this.cmdLoadParam.Name = "cmdLoadParam";
            this.cmdLoadParam.Size = new System.Drawing.Size(75, 23);
            this.cmdLoadParam.TabIndex = 9;
            this.cmdLoadParam.Text = "C&harger";
            this.cmdLoadParam.UseVisualStyleBackColor = true;
            this.cmdLoadParam.Click += new System.EventHandler(this.cmdLoadParam_Click);
            // 
            // cmdEnregistrer
            // 
            this.cmdEnregistrer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdEnregistrer.ForeColor = System.Drawing.Color.Sienna;
            this.cmdEnregistrer.Location = new System.Drawing.Point(217, 94);
            this.cmdEnregistrer.Name = "cmdEnregistrer";
            this.cmdEnregistrer.Size = new System.Drawing.Size(75, 23);
            this.cmdEnregistrer.TabIndex = 10;
            this.cmdEnregistrer.Text = "&Enregistrer";
            this.cmdEnregistrer.UseVisualStyleBackColor = true;
            this.cmdEnregistrer.Click += new System.EventHandler(this.cmdEnregistrer_Click);
            // 
            // txtBD
            // 
            this.txtBD.Location = new System.Drawing.Point(113, 44);
            this.txtBD.Name = "txtBD";
            this.txtBD.Size = new System.Drawing.Size(179, 20);
            this.txtBD.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Base des données :";
            // 
            // txtServeur
            // 
            this.txtServeur.Location = new System.Drawing.Point(113, 20);
            this.txtServeur.Name = "txtServeur";
            this.txtServeur.Size = new System.Drawing.Size(179, 20);
            this.txtServeur.TabIndex = 6;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 23);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(83, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "Serveur de BD :";
            // 
            // chkParamServeur
            // 
            this.chkParamServeur.AutoSize = true;
            this.chkParamServeur.BackColor = System.Drawing.Color.SeaShell;
            this.chkParamServeur.ForeColor = System.Drawing.Color.Sienna;
            this.chkParamServeur.Location = new System.Drawing.Point(390, 14);
            this.chkParamServeur.Name = "chkParamServeur";
            this.chkParamServeur.Size = new System.Drawing.Size(243, 17);
            this.chkParamServeur.TabIndex = 18;
            this.chkParamServeur.Text = "Modification paramètres de connexion à la BD";
            this.chkParamServeur.UseVisualStyleBackColor = false;
            this.chkParamServeur.CheckedChanged += new System.EventHandler(this.chkParamServeur_CheckedChanged);
            // 
            // cmdNouveauUser
            // 
            this.cmdNouveauUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdNouveauUser.ForeColor = System.Drawing.Color.Green;
            this.cmdNouveauUser.Location = new System.Drawing.Point(246, 121);
            this.cmdNouveauUser.Name = "cmdNouveauUser";
            this.cmdNouveauUser.Size = new System.Drawing.Size(61, 26);
            this.cmdNouveauUser.TabIndex = 1;
            this.cmdNouveauUser.Text = "&Nouveau";
            this.cmdNouveauUser.UseVisualStyleBackColor = true;
            this.cmdNouveauUser.Click += new System.EventHandler(this.cmdNouveauUser_Click);
            // 
            // cboAgent
            // 
            this.cboAgent.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboAgent.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboAgent.FormattingEnabled = true;
            this.cboAgent.Location = new System.Drawing.Point(150, 13);
            this.cboAgent.Name = "cboAgent";
            this.cboAgent.Size = new System.Drawing.Size(223, 21);
            this.cboAgent.TabIndex = 0;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Font = new System.Drawing.Font("Franklin Gothic Book", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.Black;
            this.label13.Location = new System.Drawing.Point(17, 14);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(86, 16);
            this.label13.TabIndex = 17;
            this.label13.Text = "Choisir l\'agent :";
            // 
            // cmdValiderUser
            // 
            this.cmdValiderUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdValiderUser.ForeColor = System.Drawing.Color.Green;
            this.cmdValiderUser.Location = new System.Drawing.Point(315, 121);
            this.cmdValiderUser.Name = "cmdValiderUser";
            this.cmdValiderUser.Size = new System.Drawing.Size(57, 26);
            this.cmdValiderUser.TabIndex = 5;
            this.cmdValiderUser.Text = "&Valider";
            this.cmdValiderUser.UseVisualStyleBackColor = true;
            this.cmdValiderUser.Click += new System.EventHandler(this.cmdValiderUser_Click);
            // 
            // chkActivationUser
            // 
            this.chkActivationUser.AutoSize = true;
            this.chkActivationUser.BackColor = System.Drawing.Color.Transparent;
            this.chkActivationUser.ForeColor = System.Drawing.Color.Blue;
            this.chkActivationUser.Location = new System.Drawing.Point(152, 88);
            this.chkActivationUser.Name = "chkActivationUser";
            this.chkActivationUser.Size = new System.Drawing.Size(164, 17);
            this.chkActivationUser.TabIndex = 4;
            this.chkActivationUser.Text = "Activer/Désactiver Utilisateur";
            this.chkActivationUser.UseVisualStyleBackColor = false;
            this.chkActivationUser.CheckedChanged += new System.EventHandler(this.chkActivationUser_CheckedChanged);
            // 
            // txtMotPasse
            // 
            this.txtMotPasse.Location = new System.Drawing.Point(150, 61);
            this.txtMotPasse.Name = "txtMotPasse";
            this.txtMotPasse.PasswordChar = '*';
            this.txtMotPasse.Size = new System.Drawing.Size(223, 20);
            this.txtMotPasse.TabIndex = 3;
            // 
            // txtNomUser
            // 
            this.txtNomUser.Location = new System.Drawing.Point(150, 37);
            this.txtNomUser.Name = "txtNomUser";
            this.txtNomUser.Size = new System.Drawing.Size(223, 20);
            this.txtNomUser.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Franklin Gothic Book", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(16, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Mot de Passe :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Franklin Gothic Book", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(16, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nom Utilisateur :";
            // 
            // tabModiSup
            // 
            this.tabModiSup.Controls.Add(this.tabManage);
            this.tabModiSup.Location = new System.Drawing.Point(4, 22);
            this.tabModiSup.Name = "tabModiSup";
            this.tabModiSup.Padding = new System.Windows.Forms.Padding(3);
            this.tabModiSup.Size = new System.Drawing.Size(903, 253);
            this.tabModiSup.TabIndex = 1;
            this.tabModiSup.Text = "Modification et suppression";
            this.tabModiSup.UseVisualStyleBackColor = true;
            // 
            // tabManage
            // 
            this.tabManage.Controls.Add(this.tabPage3);
            this.tabManage.Controls.Add(this.tabPage4);
            this.tabManage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabManage.Location = new System.Drawing.Point(3, 3);
            this.tabManage.Name = "tabManage";
            this.tabManage.SelectedIndex = 0;
            this.tabManage.Size = new System.Drawing.Size(897, 247);
            this.tabManage.TabIndex = 0;
            this.tabManage.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabManage_Selecting);
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage3.Controls.Add(this.groupBox2);
            this.tabPage3.Controls.Add(this.txtNewUser);
            this.tabPage3.Controls.Add(this.label7);
            this.tabPage3.Controls.Add(this.lblCompte);
            this.tabPage3.Controls.Add(this.chkActivationUserModi);
            this.tabPage3.Controls.Add(this.cmdModifierCompte);
            this.tabPage3.Controls.Add(this.cboUtilisateur);
            this.tabPage3.Controls.Add(this.txtNewMotPasse);
            this.tabPage3.Controls.Add(this.label10);
            this.tabPage3.Controls.Add(this.txtOldMotPasse);
            this.tabPage3.Controls.Add(this.label4);
            this.tabPage3.Controls.Add(this.label6);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(889, 221);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Modifier un compte";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rdActivationUser);
            this.groupBox2.Controls.Add(this.rdPwdSeul);
            this.groupBox2.Controls.Add(this.rdUserEtPwd);
            this.groupBox2.Controls.Add(this.rdUserSeul);
            this.groupBox2.Location = new System.Drawing.Point(395, 8);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(377, 125);
            this.groupBox2.TabIndex = 103;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Modifier nom d\'utilisateur ou le mot de passe";
            // 
            // rdActivationUser
            // 
            this.rdActivationUser.AutoSize = true;
            this.rdActivationUser.ForeColor = System.Drawing.Color.DarkOliveGreen;
            this.rdActivationUser.Location = new System.Drawing.Point(37, 100);
            this.rdActivationUser.Name = "rdActivationUser";
            this.rdActivationUser.Size = new System.Drawing.Size(142, 17);
            this.rdActivationUser.TabIndex = 3;
            this.rdActivationUser.TabStop = true;
            this.rdActivationUser.Text = "Activation/Désactivation";
            this.rdActivationUser.UseVisualStyleBackColor = true;
            this.rdActivationUser.CheckedChanged += new System.EventHandler(this.rdActivationUser_CheckedChanged);
            // 
            // rdPwdSeul
            // 
            this.rdPwdSeul.AutoSize = true;
            this.rdPwdSeul.ForeColor = System.Drawing.Color.Brown;
            this.rdPwdSeul.Location = new System.Drawing.Point(37, 45);
            this.rdPwdSeul.Name = "rdPwdSeul";
            this.rdPwdSeul.Size = new System.Drawing.Size(289, 17);
            this.rdPwdSeul.TabIndex = 1;
            this.rdPwdSeul.TabStop = true;
            this.rdPwdSeul.Text = "Mot de passe seulement et/ou Activation/Désactivation";
            this.rdPwdSeul.UseVisualStyleBackColor = true;
            this.rdPwdSeul.CheckedChanged += new System.EventHandler(this.rdPwdSeul_CheckedChanged);
            // 
            // rdUserEtPwd
            // 
            this.rdUserEtPwd.AutoSize = true;
            this.rdUserEtPwd.ForeColor = System.Drawing.Color.SaddleBrown;
            this.rdUserEtPwd.Location = new System.Drawing.Point(37, 73);
            this.rdUserEtPwd.Name = "rdUserEtPwd";
            this.rdUserEtPwd.Size = new System.Drawing.Size(329, 17);
            this.rdUserEtPwd.TabIndex = 2;
            this.rdUserEtPwd.TabStop = true;
            this.rdUserEtPwd.Text = "Nom d\'utilisateur et mot de passe et/ou Activation/Désactivation";
            this.rdUserEtPwd.UseVisualStyleBackColor = true;
            this.rdUserEtPwd.CheckedChanged += new System.EventHandler(this.rdUserEtPwd_CheckedChanged);
            // 
            // rdUserSeul
            // 
            this.rdUserSeul.AutoSize = true;
            this.rdUserSeul.ForeColor = System.Drawing.Color.Indigo;
            this.rdUserSeul.Location = new System.Drawing.Point(37, 18);
            this.rdUserSeul.Name = "rdUserSeul";
            this.rdUserSeul.Size = new System.Drawing.Size(302, 17);
            this.rdUserSeul.TabIndex = 0;
            this.rdUserSeul.TabStop = true;
            this.rdUserSeul.Text = "Nom d\'utilisateur seulement et/ou Activation/Désactivation";
            this.rdUserSeul.UseVisualStyleBackColor = true;
            this.rdUserSeul.CheckedChanged += new System.EventHandler(this.rdUserSeul_CheckedChanged);
            // 
            // txtNewUser
            // 
            this.txtNewUser.Location = new System.Drawing.Point(167, 39);
            this.txtNewUser.Name = "txtNewUser";
            this.txtNewUser.Size = new System.Drawing.Size(221, 20);
            this.txtNewUser.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Franklin Gothic Book", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(6, 41);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(149, 16);
            this.label7.TabIndex = 101;
            this.label7.Text = "Nouveau nom d\'utilisateur :";
            // 
            // lblCompte
            // 
            this.lblCompte.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblCompte.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCompte.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblCompte.Location = new System.Drawing.Point(398, 105);
            this.lblCompte.Name = "lblCompte";
            this.lblCompte.Size = new System.Drawing.Size(53, 21);
            this.lblCompte.TabIndex = 16;
            this.lblCompte.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chkActivationUserModi
            // 
            this.chkActivationUserModi.AutoSize = true;
            this.chkActivationUserModi.ForeColor = System.Drawing.Color.Blue;
            this.chkActivationUserModi.Location = new System.Drawing.Point(166, 121);
            this.chkActivationUserModi.Name = "chkActivationUserModi";
            this.chkActivationUserModi.Size = new System.Drawing.Size(164, 17);
            this.chkActivationUserModi.TabIndex = 8;
            this.chkActivationUserModi.Text = "Activer/Désactiver Utilisateur";
            this.chkActivationUserModi.UseVisualStyleBackColor = true;
            // 
            // cmdModifierCompte
            // 
            this.cmdModifierCompte.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdModifierCompte.ForeColor = System.Drawing.Color.ForestGreen;
            this.cmdModifierCompte.Location = new System.Drawing.Point(327, 153);
            this.cmdModifierCompte.Name = "cmdModifierCompte";
            this.cmdModifierCompte.Size = new System.Drawing.Size(61, 26);
            this.cmdModifierCompte.TabIndex = 9;
            this.cmdModifierCompte.Text = "&Modifier";
            this.cmdModifierCompte.UseVisualStyleBackColor = true;
            this.cmdModifierCompte.Click += new System.EventHandler(this.cmdModifierCompte_Click);
            // 
            // cboUtilisateur
            // 
            this.cboUtilisateur.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboUtilisateur.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboUtilisateur.FormattingEnabled = true;
            this.cboUtilisateur.Location = new System.Drawing.Point(167, 13);
            this.cboUtilisateur.Name = "cboUtilisateur";
            this.cboUtilisateur.Size = new System.Drawing.Size(221, 21);
            this.cboUtilisateur.TabIndex = 4;
            // 
            // txtNewMotPasse
            // 
            this.txtNewMotPasse.Location = new System.Drawing.Point(167, 92);
            this.txtNewMotPasse.Name = "txtNewMotPasse";
            this.txtNewMotPasse.PasswordChar = '*';
            this.txtNewMotPasse.Size = new System.Drawing.Size(221, 20);
            this.txtNewMotPasse.TabIndex = 7;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Franklin Gothic Book", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Location = new System.Drawing.Point(7, 94);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(130, 16);
            this.label10.TabIndex = 12;
            this.label10.Text = "Nouveau Mot de passe :";
            // 
            // txtOldMotPasse
            // 
            this.txtOldMotPasse.Location = new System.Drawing.Point(167, 65);
            this.txtOldMotPasse.Name = "txtOldMotPasse";
            this.txtOldMotPasse.PasswordChar = '*';
            this.txtOldMotPasse.Size = new System.Drawing.Size(222, 20);
            this.txtOldMotPasse.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Franklin Gothic Book", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(7, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 16);
            this.label4.TabIndex = 8;
            this.label4.Text = "Ancien Mot de passe :";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Franklin Gothic Book", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(7, 14);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(159, 16);
            this.label6.TabIndex = 6;
            this.label6.Text = "Nom un utilisateur en cours :";
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage4.Controls.Add(this.cmdDelete);
            this.tabPage4.Controls.Add(this.CboUserSup);
            this.tabPage4.Controls.Add(this.label12);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(889, 221);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Supprimer un Compte";
            // 
            // cmdDelete
            // 
            this.cmdDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdDelete.ForeColor = System.Drawing.Color.Tomato;
            this.cmdDelete.Location = new System.Drawing.Point(134, 117);
            this.cmdDelete.Name = "cmdDelete";
            this.cmdDelete.Size = new System.Drawing.Size(65, 26);
            this.cmdDelete.TabIndex = 27;
            this.cmdDelete.Text = "&Supprimer";
            this.cmdDelete.UseVisualStyleBackColor = true;
            this.cmdDelete.Click += new System.EventHandler(this.cmdDelete_Click);
            // 
            // CboUserSup
            // 
            this.CboUserSup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CboUserSup.Enabled = false;
            this.CboUserSup.FormattingEnabled = true;
            this.CboUserSup.Location = new System.Drawing.Point(134, 81);
            this.CboUserSup.Name = "CboUserSup";
            this.CboUserSup.Size = new System.Drawing.Size(228, 21);
            this.CboUserSup.TabIndex = 13;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("Franklin Gothic Book", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.Black;
            this.label12.Location = new System.Drawing.Point(6, 82);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(122, 16);
            this.label12.TabIndex = 17;
            this.label12.Text = "Choisir un utilisateur :";
            // 
            // tabDroit
            // 
            this.tabDroit.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabDroit.Controls.Add(this.dgv1);
            this.tabDroit.Controls.Add(this.cmdAfficherDroit);
            this.tabDroit.Controls.Add(this.cmdRetirerDroit);
            this.tabDroit.Controls.Add(this.cmdAccorderDroit);
            this.tabDroit.Controls.Add(this.groupBox1);
            this.tabDroit.Controls.Add(this.cboUtilisateur1);
            this.tabDroit.Controls.Add(this.label5);
            this.tabDroit.Location = new System.Drawing.Point(4, 22);
            this.tabDroit.Name = "tabDroit";
            this.tabDroit.Size = new System.Drawing.Size(903, 253);
            this.tabDroit.TabIndex = 3;
            this.tabDroit.Text = "Droits utilisateur";
            // 
            // dgv1
            // 
            this.dgv1.BackgroundColor = System.Drawing.Color.White;
            this.dgv1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewCheckBoxColumn1,
            this.dataGridViewTextBoxColumn6,
            this.colSchema_user1,
            this.dataGridViewTextBoxColumn8,
            this.dataGridViewTextBoxColumn9,
            this.dataGridViewTextBoxColumn10,
            this.colnom});
            this.dgv1.GridColor = System.Drawing.SystemColors.AppWorkspace;
            this.dgv1.Location = new System.Drawing.Point(377, 15);
            this.dgv1.MultiSelect = false;
            this.dgv1.Name = "dgv1";
            this.dgv1.ReadOnly = true;
            this.dgv1.RowHeadersVisible = false;
            this.dgv1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv1.Size = new System.Drawing.Size(518, 235);
            this.dgv1.TabIndex = 22;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Nomuser";
            this.dataGridViewTextBoxColumn2.HeaderText = "Username";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "Droits";
            this.dataGridViewTextBoxColumn3.HeaderText = "Droits de l\'utilisateur sélectionné";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 340;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.DataPropertyName = "Activation";
            this.dataGridViewCheckBoxColumn1.HeaderText = "Activation";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.ReadOnly = true;
            this.dataGridViewCheckBoxColumn1.Width = 70;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "Motpass";
            this.dataGridViewTextBoxColumn6.HeaderText = "Mot de passe";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Visible = false;
            this.dataGridViewTextBoxColumn6.Width = 120;
            // 
            // colSchema_user1
            // 
            this.colSchema_user1.DataPropertyName = "Schema_user";
            this.colSchema_user1.HeaderText = "Schema_user";
            this.colSchema_user1.Name = "colSchema_user1";
            this.colSchema_user1.ReadOnly = true;
            this.colSchema_user1.Visible = false;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "id_agent";
            this.dataGridViewTextBoxColumn8.HeaderText = "id_agent";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            this.dataGridViewTextBoxColumn8.Visible = false;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.DataPropertyName = "Id_agentuser";
            this.dataGridViewTextBoxColumn9.HeaderText = "Id_agent";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            this.dataGridViewTextBoxColumn9.Visible = false;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.DataPropertyName = "Id_utilisateur";
            this.dataGridViewTextBoxColumn10.HeaderText = "ID";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.ReadOnly = true;
            this.dataGridViewTextBoxColumn10.Visible = false;
            // 
            // colnom
            // 
            this.colnom.DataPropertyName = "nom";
            this.colnom.HeaderText = "colnom";
            this.colnom.Name = "colnom";
            this.colnom.ReadOnly = true;
            this.colnom.Visible = false;
            // 
            // cmdAfficherDroit
            // 
            this.cmdAfficherDroit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdAfficherDroit.ForeColor = System.Drawing.Color.MidnightBlue;
            this.cmdAfficherDroit.Location = new System.Drawing.Point(305, 53);
            this.cmdAfficherDroit.Name = "cmdAfficherDroit";
            this.cmdAfficherDroit.Size = new System.Drawing.Size(65, 26);
            this.cmdAfficherDroit.TabIndex = 1;
            this.cmdAfficherDroit.Text = "Af&ficher";
            this.cmdAfficherDroit.UseVisualStyleBackColor = true;
            this.cmdAfficherDroit.Click += new System.EventHandler(this.cmdAfficherDroit_Click);
            // 
            // cmdRetirerDroit
            // 
            this.cmdRetirerDroit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdRetirerDroit.ForeColor = System.Drawing.Color.MediumVioletRed;
            this.cmdRetirerDroit.Location = new System.Drawing.Point(305, 118);
            this.cmdRetirerDroit.Name = "cmdRetirerDroit";
            this.cmdRetirerDroit.Size = new System.Drawing.Size(65, 26);
            this.cmdRetirerDroit.TabIndex = 11;
            this.cmdRetirerDroit.Text = "Ret&irer";
            this.cmdRetirerDroit.UseVisualStyleBackColor = true;
            this.cmdRetirerDroit.Click += new System.EventHandler(this.cmdRetirerDroit_Click);
            // 
            // cmdAccorderDroit
            // 
            this.cmdAccorderDroit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdAccorderDroit.ForeColor = System.Drawing.Color.Green;
            this.cmdAccorderDroit.Location = new System.Drawing.Point(305, 86);
            this.cmdAccorderDroit.Name = "cmdAccorderDroit";
            this.cmdAccorderDroit.Size = new System.Drawing.Size(65, 26);
            this.cmdAccorderDroit.TabIndex = 10;
            this.cmdAccorderDroit.Text = "Acco&rder";
            this.cmdAccorderDroit.UseVisualStyleBackColor = true;
            this.cmdAccorderDroit.Click += new System.EventHandler(this.cmdAccorderDroit_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkLevelAdmin);
            this.groupBox1.Controls.Add(this.chkLevelAdminstrateur);
            this.groupBox1.Controls.Add(this.chkLevelUser);
            this.groupBox1.Font = new System.Drawing.Font("Franklin Gothic Book", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.Black;
            this.groupBox1.Location = new System.Drawing.Point(8, 45);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(289, 99);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Attribuer les privilège pour l\'utilisateur";
            // 
            // chkLevelAdmin
            // 
            this.chkLevelAdmin.AutoSize = true;
            this.chkLevelAdmin.BackColor = System.Drawing.Color.Transparent;
            this.chkLevelAdmin.Location = new System.Drawing.Point(126, 41);
            this.chkLevelAdmin.Name = "chkLevelAdmin";
            this.chkLevelAdmin.Size = new System.Drawing.Size(59, 20);
            this.chkLevelAdmin.TabIndex = 3;
            this.chkLevelAdmin.Text = "Admin";
            this.chkLevelAdmin.UseVisualStyleBackColor = false;
            this.chkLevelAdmin.CheckedChanged += new System.EventHandler(this.chkLevelAdmin_CheckedChanged_1);
            // 
            // chkLevelAdminstrateur
            // 
            this.chkLevelAdminstrateur.AutoSize = true;
            this.chkLevelAdminstrateur.BackColor = System.Drawing.Color.Transparent;
            this.chkLevelAdminstrateur.Location = new System.Drawing.Point(11, 41);
            this.chkLevelAdminstrateur.Name = "chkLevelAdminstrateur";
            this.chkLevelAdminstrateur.Size = new System.Drawing.Size(103, 20);
            this.chkLevelAdminstrateur.TabIndex = 2;
            this.chkLevelAdminstrateur.Text = "Administrateur";
            this.chkLevelAdminstrateur.UseVisualStyleBackColor = false;
            this.chkLevelAdminstrateur.CheckedChanged += new System.EventHandler(this.chkLevelAdmin_CheckedChanged);
            // 
            // chkLevelUser
            // 
            this.chkLevelUser.AutoSize = true;
            this.chkLevelUser.BackColor = System.Drawing.Color.Transparent;
            this.chkLevelUser.Location = new System.Drawing.Point(203, 41);
            this.chkLevelUser.Name = "chkLevelUser";
            this.chkLevelUser.Size = new System.Drawing.Size(80, 20);
            this.chkLevelUser.TabIndex = 4;
            this.chkLevelUser.Text = "Utilisateur";
            this.chkLevelUser.UseVisualStyleBackColor = false;
            // 
            // cboUtilisateur1
            // 
            this.cboUtilisateur1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboUtilisateur1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboUtilisateur1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboUtilisateur1.FormattingEnabled = true;
            this.cboUtilisateur1.Location = new System.Drawing.Point(140, 15);
            this.cboUtilisateur1.Name = "cboUtilisateur1";
            this.cboUtilisateur1.Size = new System.Drawing.Size(231, 21);
            this.cboUtilisateur1.TabIndex = 0;
            this.cboUtilisateur1.SelectedIndexChanged += new System.EventHandler(this.cboUtilisateur1_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Franklin Gothic Book", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(8, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(122, 16);
            this.label5.TabIndex = 16;
            this.label5.Text = "Choisir un utilisateur :";
            // 
            // frmUtilisateur
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::smartManage.Desktop.Properties.Resources.img_home_player_background;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(911, 320);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.tabMainManage);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frmUtilisateur";
            this.Text = "Utilisateur";
            this.Activated += new System.EventHandler(this.frmUtilisateur_Activated);
            this.Load += new System.EventHandler(this.FrmUtilisateur_Load);
            this.tabMainManage.ResumeLayout(false);
            this.tabViewSearch.ResumeLayout(false);
            this.tabViewSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.tabCreate.ResumeLayout(false);
            this.tabCreate.PerformLayout();
            this.gpParamServeur.ResumeLayout(false);
            this.gpParamServeur.PerformLayout();
            this.tabModiSup.ResumeLayout(false);
            this.tabManage.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabDroit.ResumeLayout(false);
            this.tabDroit.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.TabControl tabMainManage;
        private System.Windows.Forms.TabPage tabViewSearch;
        private System.Windows.Forms.TextBox txtSeach;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.TabPage tabCreate;
        private System.Windows.Forms.Button cmdNouveauUser;
        private System.Windows.Forms.ComboBox cboAgent;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button cmdValiderUser;
        private System.Windows.Forms.CheckBox chkActivationUser;
        private System.Windows.Forms.TextBox txtMotPasse;
        private System.Windows.Forms.TextBox txtNomUser;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabModiSup;
        private System.Windows.Forms.TabControl tabManage;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label lblCompte;
        private System.Windows.Forms.CheckBox chkActivationUserModi;
        private System.Windows.Forms.Button cmdModifierCompte;
        private System.Windows.Forms.ComboBox cboUtilisateur;
        private System.Windows.Forms.TextBox txtNewMotPasse;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtOldMotPasse;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Button cmdDelete;
        private System.Windows.Forms.ComboBox CboUserSup;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TabPage tabDroit;
        private System.Windows.Forms.ComboBox cboUtilisateur1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button cmdRetirerDroit;
        private System.Windows.Forms.Button cmdAccorderDroit;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkLevelAdmin;
        private System.Windows.Forms.CheckBox chkLevelAdminstrateur;
        private System.Windows.Forms.CheckBox chkLevelUser;
        private System.Windows.Forms.Button cmdAfficherDroit;
        private System.Windows.Forms.DataGridView dgv1;
        private System.Windows.Forms.TextBox txtNewUser;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdPwdSeul;
        private System.Windows.Forms.RadioButton rdUserEtPwd;
        private System.Windows.Forms.RadioButton rdUserSeul;
        private System.Windows.Forms.GroupBox gpParamServeur;
        private System.Windows.Forms.CheckBox chkParamServeur;
        private System.Windows.Forms.Button cmdEnregistrer;
        private System.Windows.Forms.TextBox txtBD;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtServeur;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button cmdLoadParam;
        private System.Windows.Forms.RadioButton rdActivationUser;
        private System.Windows.Forms.TextBox txtVersion;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSchema_user1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn colnom;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDesignation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNomAgent;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colActivation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNiveau;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSchema_user;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMotpass;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId_personne;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    }
}