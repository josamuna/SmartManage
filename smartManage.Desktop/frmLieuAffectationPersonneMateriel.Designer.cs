namespace smartManage.Desktop
{
    partial class frmLieuAffectationPersonneMateriel
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCode_AC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDate_affectation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colId_personne = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colId_type_lieu_affectation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colId_fonction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDesignation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUser_created = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDate_created = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUser_modified = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDate_modified = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmdAffiche = new System.Windows.Forms.Button();
            this.chkAll = new System.Windows.Forms.CheckBox();
            this.cboACSearch = new System.Windows.Forms.ComboBox();
            this.cboDesignation = new System.Windows.Forms.ComboBox();
            this.lblAddAC = new System.Windows.Forms.LinkLabel();
            this.cboTypeLieuAffect = new System.Windows.Forms.ComboBox();
            this.lblAddTypeLieuAffect = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDateAffectation = new System.Windows.Forms.DateTimePicker();
            this.cboPersonne = new System.Windows.Forms.ComboBox();
            this.cboFonction = new System.Windows.Forms.ComboBox();
            this.txtDateModifie = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtModifieBy = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.txtDateCreate = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.txtCreateBy = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.pbPhoto = new System.Windows.Forms.PictureBox();
            this.lblAddFonction = new System.Windows.Forms.LinkLabel();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cboAC = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtId = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPhoto)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgv.BackgroundColor = System.Drawing.Color.White;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colId,
            this.colCode_AC,
            this.colDate_affectation,
            this.colId_personne,
            this.colId_type_lieu_affectation,
            this.colId_fonction,
            this.colDesignation,
            this.colUser_created,
            this.colDate_created,
            this.colUser_modified,
            this.colDate_modified});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Thistle;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgv.Location = new System.Drawing.Point(3, 19);
            this.dgv.MultiSelect = false;
            this.dgv.Name = "dgv";
            this.dgv.RowHeadersVisible = false;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.Size = new System.Drawing.Size(823, 257);
            this.dgv.TabIndex = 200;
            this.dgv.TabStop = false;
            this.dgv.SelectionChanged += new System.EventHandler(this.dgv_SelectionChanged);
            // 
            // colId
            // 
            this.colId.DataPropertyName = "Id";
            this.colId.HeaderText = "Code";
            this.colId.Name = "colId";
            this.colId.Width = 70;
            // 
            // colCode_AC
            // 
            this.colCode_AC.DataPropertyName = "Code_AC";
            this.colCode_AC.HeaderText = "Année Ac.";
            this.colCode_AC.Name = "colCode_AC";
            // 
            // colDate_affectation
            // 
            this.colDate_affectation.DataPropertyName = "Date_affectation";
            this.colDate_affectation.HeaderText = "Date";
            this.colDate_affectation.Name = "colDate_affectation";
            // 
            // colId_personne
            // 
            this.colId_personne.DataPropertyName = "Id_personne";
            this.colId_personne.HeaderText = "Id_personne";
            this.colId_personne.Name = "colId_personne";
            this.colId_personne.Visible = false;
            this.colId_personne.Width = 110;
            // 
            // colId_type_lieu_affectation
            // 
            this.colId_type_lieu_affectation.DataPropertyName = "Id_type_lieu_affectation";
            this.colId_type_lieu_affectation.HeaderText = "Id_type_lieu_affectation";
            this.colId_type_lieu_affectation.Name = "colId_type_lieu_affectation";
            this.colId_type_lieu_affectation.Visible = false;
            // 
            // colId_fonction
            // 
            this.colId_fonction.DataPropertyName = "Id_fonction";
            this.colId_fonction.HeaderText = "Id_fonction";
            this.colId_fonction.Name = "colId_fonction";
            this.colId_fonction.Visible = false;
            this.colId_fonction.Width = 50;
            // 
            // colDesignation
            // 
            this.colDesignation.DataPropertyName = "Designation";
            this.colDesignation.HeaderText = "Désignation";
            this.colDesignation.Name = "colDesignation";
            this.colDesignation.Width = 250;
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
            this.colDate_modified.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.dgv);
            this.groupBox1.Location = new System.Drawing.Point(8, 179);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(829, 279);
            this.groupBox1.TabIndex = 540;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Affichage des données manipulées";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.cmdAffiche);
            this.groupBox2.Controls.Add(this.chkAll);
            this.groupBox2.Controls.Add(this.cboACSearch);
            this.groupBox2.Controls.Add(this.cboDesignation);
            this.groupBox2.Controls.Add(this.lblAddAC);
            this.groupBox2.Controls.Add(this.cboTypeLieuAffect);
            this.groupBox2.Controls.Add(this.lblAddTypeLieuAffect);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtDateAffectation);
            this.groupBox2.Controls.Add(this.cboPersonne);
            this.groupBox2.Controls.Add(this.cboFonction);
            this.groupBox2.Controls.Add(this.txtDateModifie);
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Controls.Add(this.txtModifieBy);
            this.groupBox2.Controls.Add(this.label18);
            this.groupBox2.Controls.Add(this.txtDateCreate);
            this.groupBox2.Controls.Add(this.label19);
            this.groupBox2.Controls.Add(this.txtCreateBy);
            this.groupBox2.Controls.Add(this.label20);
            this.groupBox2.Controls.Add(this.groupBox5);
            this.groupBox2.Controls.Add(this.lblAddFonction);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.cboAC);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtId);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(8, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(828, 171);
            this.groupBox2.TabIndex = 510;
            this.groupBox2.TabStop = false;
            // 
            // cmdAffiche
            // 
            this.cmdAffiche.BackColor = System.Drawing.Color.LavenderBlush;
            this.cmdAffiche.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cmdAffiche.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdAffiche.ForeColor = System.Drawing.Color.Brown;
            this.cmdAffiche.Location = new System.Drawing.Point(552, 14);
            this.cmdAffiche.Name = "cmdAffiche";
            this.cmdAffiche.Size = new System.Drawing.Size(84, 22);
            this.cmdAffiche.TabIndex = 11;
            this.cmdAffiche.Text = "Affiche&r";
            this.cmdAffiche.UseVisualStyleBackColor = false;
            this.cmdAffiche.Click += new System.EventHandler(this.cmdAffiche_Click);
            // 
            // chkAll
            // 
            this.chkAll.AutoSize = true;
            this.chkAll.ForeColor = System.Drawing.Color.Purple;
            this.chkAll.Location = new System.Drawing.Point(311, 18);
            this.chkAll.Name = "chkAll";
            this.chkAll.Size = new System.Drawing.Size(86, 17);
            this.chkAll.TabIndex = 9;
            this.chkAll.TabStop = false;
            this.chkAll.Text = "Tout afficher";
            this.chkAll.UseVisualStyleBackColor = true;
            this.chkAll.CheckedChanged += new System.EventHandler(this.chkAll_CheckedChanged);
            // 
            // cboACSearch
            // 
            this.cboACSearch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboACSearch.DropDownWidth = 109;
            this.cboACSearch.FormattingEnabled = true;
            this.cboACSearch.Location = new System.Drawing.Point(432, 15);
            this.cboACSearch.Name = "cboACSearch";
            this.cboACSearch.Size = new System.Drawing.Size(114, 21);
            this.cboACSearch.TabIndex = 10;
            // 
            // cboDesignation
            // 
            this.cboDesignation.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboDesignation.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboDesignation.DropDownWidth = 130;
            this.cboDesignation.FormattingEnabled = true;
            this.cboDesignation.Location = new System.Drawing.Point(432, 40);
            this.cboDesignation.Name = "cboDesignation";
            this.cboDesignation.Size = new System.Drawing.Size(204, 21);
            this.cboDesignation.TabIndex = 8;
            // 
            // lblAddAC
            // 
            this.lblAddAC.AutoSize = true;
            this.lblAddAC.Location = new System.Drawing.Point(308, 45);
            this.lblAddAC.Name = "lblAddAC";
            this.lblAddAC.Size = new System.Drawing.Size(29, 13);
            this.lblAddAC.TabIndex = 1;
            this.lblAddAC.TabStop = true;
            this.lblAddAC.Text = "New";
            this.lblAddAC.VisitedLinkColor = System.Drawing.Color.Blue;
            this.lblAddAC.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblAddAC_LinkClicked);
            // 
            // cboTypeLieuAffect
            // 
            this.cboTypeLieuAffect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTypeLieuAffect.DropDownWidth = 130;
            this.cboTypeLieuAffect.FormattingEnabled = true;
            this.cboTypeLieuAffect.Location = new System.Drawing.Point(102, 115);
            this.cboTypeLieuAffect.Name = "cboTypeLieuAffect";
            this.cboTypeLieuAffect.Size = new System.Drawing.Size(204, 21);
            this.cboTypeLieuAffect.TabIndex = 5;
            this.cboTypeLieuAffect.DropDown += new System.EventHandler(this.cboTypeLieuAffect_DropDown);
            // 
            // lblAddTypeLieuAffect
            // 
            this.lblAddTypeLieuAffect.AutoSize = true;
            this.lblAddTypeLieuAffect.Location = new System.Drawing.Point(308, 120);
            this.lblAddTypeLieuAffect.Name = "lblAddTypeLieuAffect";
            this.lblAddTypeLieuAffect.Size = new System.Drawing.Size(29, 13);
            this.lblAddTypeLieuAffect.TabIndex = 6;
            this.lblAddTypeLieuAffect.TabStop = true;
            this.lblAddTypeLieuAffect.Text = "New";
            this.lblAddTypeLieuAffect.VisitedLinkColor = System.Drawing.Color.Blue;
            this.lblAddTypeLieuAffect.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblAddTypeLieuAffect_LinkClicked);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 119);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 164;
            this.label2.Text = "Type lieu Affect. : ";
            // 
            // txtDateAffectation
            // 
            this.txtDateAffectation.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtDateAffectation.Location = new System.Drawing.Point(102, 140);
            this.txtDateAffectation.Name = "txtDateAffectation";
            this.txtDateAffectation.Size = new System.Drawing.Size(204, 20);
            this.txtDateAffectation.TabIndex = 7;
            // 
            // cboPersonne
            // 
            this.cboPersonne.DropDownWidth = 130;
            this.cboPersonne.FormattingEnabled = true;
            this.cboPersonne.Location = new System.Drawing.Point(102, 65);
            this.cboPersonne.Name = "cboPersonne";
            this.cboPersonne.Size = new System.Drawing.Size(204, 21);
            this.cboPersonne.TabIndex = 2;
            // 
            // cboFonction
            // 
            this.cboFonction.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboFonction.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboFonction.DropDownWidth = 150;
            this.cboFonction.FormattingEnabled = true;
            this.cboFonction.Location = new System.Drawing.Point(102, 90);
            this.cboFonction.Name = "cboFonction";
            this.cboFonction.Size = new System.Drawing.Size(204, 21);
            this.cboFonction.TabIndex = 3;
            this.cboFonction.DropDown += new System.EventHandler(this.cboFonction_DropDown);
            // 
            // txtDateModifie
            // 
            this.txtDateModifie.Location = new System.Drawing.Point(432, 140);
            this.txtDateModifie.Name = "txtDateModifie";
            this.txtDateModifie.ReadOnly = true;
            this.txtDateModifie.Size = new System.Drawing.Size(204, 20);
            this.txtDateModifie.TabIndex = 156;
            this.txtDateModifie.TabStop = false;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(337, 144);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(98, 13);
            this.label17.TabIndex = 48;
            this.label17.Text = "Date modification : ";
            // 
            // txtModifieBy
            // 
            this.txtModifieBy.Location = new System.Drawing.Point(432, 115);
            this.txtModifieBy.Name = "txtModifieBy";
            this.txtModifieBy.ReadOnly = true;
            this.txtModifieBy.Size = new System.Drawing.Size(204, 20);
            this.txtModifieBy.TabIndex = 155;
            this.txtModifieBy.TabStop = false;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(337, 119);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(68, 13);
            this.label18.TabIndex = 46;
            this.label18.Text = "Modifier par :";
            // 
            // txtDateCreate
            // 
            this.txtDateCreate.Location = new System.Drawing.Point(432, 90);
            this.txtDateCreate.Name = "txtDateCreate";
            this.txtDateCreate.ReadOnly = true;
            this.txtDateCreate.Size = new System.Drawing.Size(204, 20);
            this.txtDateCreate.TabIndex = 154;
            this.txtDateCreate.TabStop = false;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(337, 94);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(80, 13);
            this.label19.TabIndex = 44;
            this.label19.Text = "Date création : ";
            // 
            // txtCreateBy
            // 
            this.txtCreateBy.Location = new System.Drawing.Point(432, 65);
            this.txtCreateBy.Name = "txtCreateBy";
            this.txtCreateBy.ReadOnly = true;
            this.txtCreateBy.Size = new System.Drawing.Size(204, 20);
            this.txtCreateBy.TabIndex = 153;
            this.txtCreateBy.TabStop = false;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(337, 69);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(53, 13);
            this.label20.TabIndex = 42;
            this.label20.Text = "Créé par :";
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.pbPhoto);
            this.groupBox5.Location = new System.Drawing.Point(643, 9);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(177, 152);
            this.groupBox5.TabIndex = 5;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Profil de la personne";
            // 
            // pbPhoto
            // 
            this.pbPhoto.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbPhoto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbPhoto.Location = new System.Drawing.Point(8, 18);
            this.pbPhoto.Name = "pbPhoto";
            this.pbPhoto.Size = new System.Drawing.Size(160, 125);
            this.pbPhoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbPhoto.TabIndex = 2;
            this.pbPhoto.TabStop = false;
            // 
            // lblAddFonction
            // 
            this.lblAddFonction.AutoSize = true;
            this.lblAddFonction.Location = new System.Drawing.Point(308, 95);
            this.lblAddFonction.Name = "lblAddFonction";
            this.lblAddFonction.Size = new System.Drawing.Size(29, 13);
            this.lblAddFonction.TabIndex = 4;
            this.lblAddFonction.TabStop = true;
            this.lblAddFonction.Text = "New";
            this.lblAddFonction.VisitedLinkColor = System.Drawing.Color.Blue;
            this.lblAddFonction.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblAddFonction_LinkClicked);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 94);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(57, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "Fonction : ";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 143);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(92, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Date affectation : ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 69);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Personne : ";
            // 
            // cboAC
            // 
            this.cboAC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAC.DropDownWidth = 109;
            this.cboAC.FormattingEnabled = true;
            this.cboAC.Location = new System.Drawing.Point(102, 40);
            this.cboAC.Name = "cboAC";
            this.cboAC.Size = new System.Drawing.Size(204, 21);
            this.cboAC.TabIndex = 0;
            this.cboAC.DropDown += new System.EventHandler(this.cboAC_DropDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Année Acad. : ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(337, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Désignation : ";
            // 
            // txtId
            // 
            this.txtId.Location = new System.Drawing.Point(102, 16);
            this.txtId.Name = "txtId";
            this.txtId.ReadOnly = true;
            this.txtId.Size = new System.Drawing.Size(204, 20);
            this.txtId.TabIndex = 150;
            this.txtId.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Code :";
            // 
            // frmLieuAffectation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.ClientSize = new System.Drawing.Size(844, 465);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmLieuAffectation";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Text = "Lieu d\'affectation d\'une personne (Agent) ou d\'un équipement";
            this.Activated += new System.EventHandler(this.frmLieuAffectationPersonne_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmLieuAffectationPersonne_FormClosed);
            this.Load += new System.EventHandler(this.frmLieuAffectationPersonne_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbPhoto)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cboAC;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.LinkLabel lblAddFonction;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.PictureBox pbPhoto;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtModifieBy;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtDateCreate;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txtCreateBy;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox txtDateModifie;
        private System.Windows.Forms.ComboBox cboFonction;
        private System.Windows.Forms.ComboBox cboPersonne;
        private System.Windows.Forms.DateTimePicker txtDateAffectation;
        private System.Windows.Forms.ComboBox cboTypeLieuAffect;
        private System.Windows.Forms.LinkLabel lblAddTypeLieuAffect;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel lblAddAC;
        private System.Windows.Forms.ComboBox cboDesignation;
        private System.Windows.Forms.ComboBox cboACSearch;
        private System.Windows.Forms.CheckBox chkAll;
        private System.Windows.Forms.Button cmdAffiche;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCode_AC;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate_affectation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId_personne;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId_type_lieu_affectation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId_fonction;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDesignation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUser_created;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate_created;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUser_modified;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate_modified;
    }
}