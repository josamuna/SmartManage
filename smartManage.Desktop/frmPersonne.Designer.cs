namespace smartManage.Desktop
{
    partial class frmPersonne
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPostnom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrenom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSexe = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEtatcivil = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDatenaissance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIsenseignant = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colIsagent = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colIsetudiant = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colPhoto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colId_grade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUser_created = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDate_created = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUser_modified = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDate_modified = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNomComplet = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmdNewAddress = new System.Windows.Forms.Button();
            this.lblRefreshAllSubDgv = new System.Windows.Forms.LinkLabel();
            this.cmdNewEmail = new System.Windows.Forms.Button();
            this.cmdNewPhone = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.dgvAdresse = new System.Windows.Forms.DataGridView();
            this.colId_adresse = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDesignation2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colId_personne3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dgvEmail = new System.Windows.Forms.DataGridView();
            this.colIdTel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDesignation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colId_personne2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chkEtudiant = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.dgvTelephone = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNumeroComplet = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNumero = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colId_personne = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chkAgent = new System.Windows.Forms.CheckBox();
            this.txtDateNaissance = new System.Windows.Forms.MaskedTextBox();
            this.txtPrenom = new System.Windows.Forms.TextBox();
            this.txtPostNom = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.chkEnseignant = new System.Windows.Forms.CheckBox();
            this.cboEtatCivil = new System.Windows.Forms.ComboBox();
            this.cboGrade = new System.Windows.Forms.ComboBox();
            this.txtDateModifie = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtModifieBy = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.txtDateCreate = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.txtCreateBy = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.lblPhoto = new System.Windows.Forms.LinkLabel();
            this.pbPhoto = new System.Windows.Forms.PictureBox();
            this.lblAddGrade = new System.Windows.Forms.LinkLabel();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cboSexe = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtNom = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtId = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ctxMenuPhoto = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.smnCtxPhoto = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAdresse)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEmail)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTelephone)).BeginInit();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPhoto)).BeginInit();
            this.ctxMenuPhoto.SuspendLayout();
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
            this.colNom,
            this.colPostnom,
            this.colPrenom,
            this.colSexe,
            this.colEtatcivil,
            this.colDatenaissance,
            this.colIsenseignant,
            this.colIsagent,
            this.colIsetudiant,
            this.colPhoto,
            this.colId_grade,
            this.colUser_created,
            this.colDate_created,
            this.colUser_modified,
            this.colDate_modified,
            this.colNomComplet});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Thistle;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgv.Location = new System.Drawing.Point(3, 19);
            this.dgv.MultiSelect = false;
            this.dgv.Name = "dgv";
            this.dgv.RowHeadersVisible = false;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.Size = new System.Drawing.Size(941, 210);
            this.dgv.TabIndex = 200;
            this.dgv.TabStop = false;
            this.dgv.SelectionChanged += new System.EventHandler(this.dgv_SelectionChanged);
            // 
            // colId
            // 
            this.colId.DataPropertyName = "Id";
            this.colId.HeaderText = "Id";
            this.colId.Name = "colId";
            this.colId.Visible = false;
            // 
            // colNom
            // 
            this.colNom.DataPropertyName = "Nom";
            this.colNom.HeaderText = "Nom";
            this.colNom.Name = "colNom";
            // 
            // colPostnom
            // 
            this.colPostnom.DataPropertyName = "Postnom";
            this.colPostnom.HeaderText = "Postnom";
            this.colPostnom.Name = "colPostnom";
            // 
            // colPrenom
            // 
            this.colPrenom.DataPropertyName = "Prenom";
            this.colPrenom.HeaderText = "Prénom";
            this.colPrenom.Name = "colPrenom";
            this.colPrenom.Width = 110;
            // 
            // colSexe
            // 
            this.colSexe.DataPropertyName = "Sexe";
            this.colSexe.HeaderText = "Sexe";
            this.colSexe.Name = "colSexe";
            this.colSexe.Width = 50;
            // 
            // colEtatcivil
            // 
            this.colEtatcivil.DataPropertyName = "Etatcivil";
            this.colEtatcivil.HeaderText = "Etatcivil";
            this.colEtatcivil.Name = "colEtatcivil";
            // 
            // colDatenaissance
            // 
            this.colDatenaissance.DataPropertyName = "Datenaissance";
            this.colDatenaissance.HeaderText = "Date de naissance";
            this.colDatenaissance.Name = "colDatenaissance";
            this.colDatenaissance.Width = 120;
            // 
            // colIsenseignant
            // 
            this.colIsenseignant.DataPropertyName = "Isenseignant";
            this.colIsenseignant.HeaderText = "Enseignant";
            this.colIsenseignant.Name = "colIsenseignant";
            this.colIsenseignant.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colIsenseignant.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // colIsagent
            // 
            this.colIsagent.DataPropertyName = "Isagent";
            this.colIsagent.HeaderText = "Agent";
            this.colIsagent.Name = "colIsagent";
            this.colIsagent.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colIsagent.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // colIsetudiant
            // 
            this.colIsetudiant.DataPropertyName = "Isetudiant";
            this.colIsetudiant.HeaderText = "Etudiant";
            this.colIsetudiant.Name = "colIsetudiant";
            this.colIsetudiant.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colIsetudiant.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // colPhoto
            // 
            this.colPhoto.DataPropertyName = "Photo";
            this.colPhoto.HeaderText = "Photo";
            this.colPhoto.Name = "colPhoto";
            this.colPhoto.Visible = false;
            // 
            // colId_grade
            // 
            this.colId_grade.DataPropertyName = "Id_grade";
            this.colId_grade.HeaderText = "Id_grade";
            this.colId_grade.Name = "colId_grade";
            this.colId_grade.Visible = false;
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
            // colNomComplet
            // 
            this.colNomComplet.DataPropertyName = "NomComplet";
            this.colNomComplet.HeaderText = "NomComplet";
            this.colNomComplet.Name = "colNomComplet";
            this.colNomComplet.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.dgv);
            this.groupBox1.Location = new System.Drawing.Point(8, 336);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(947, 232);
            this.groupBox1.TabIndex = 540;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Affichage des données manipulées";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.cmdNewAddress);
            this.groupBox2.Controls.Add(this.lblRefreshAllSubDgv);
            this.groupBox2.Controls.Add(this.cmdNewEmail);
            this.groupBox2.Controls.Add(this.cmdNewPhone);
            this.groupBox2.Controls.Add(this.groupBox6);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.chkEtudiant);
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Controls.Add(this.chkAgent);
            this.groupBox2.Controls.Add(this.txtDateNaissance);
            this.groupBox2.Controls.Add(this.txtPrenom);
            this.groupBox2.Controls.Add(this.txtPostNom);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.chkEnseignant);
            this.groupBox2.Controls.Add(this.cboEtatCivil);
            this.groupBox2.Controls.Add(this.cboGrade);
            this.groupBox2.Controls.Add(this.txtDateModifie);
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Controls.Add(this.txtModifieBy);
            this.groupBox2.Controls.Add(this.label18);
            this.groupBox2.Controls.Add(this.txtDateCreate);
            this.groupBox2.Controls.Add(this.label19);
            this.groupBox2.Controls.Add(this.txtCreateBy);
            this.groupBox2.Controls.Add(this.label20);
            this.groupBox2.Controls.Add(this.groupBox5);
            this.groupBox2.Controls.Add(this.lblAddGrade);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.cboSexe);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtNom);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtId);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(8, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(947, 329);
            this.groupBox2.TabIndex = 510;
            this.groupBox2.TabStop = false;
            // 
            // cmdNewAddress
            // 
            this.cmdNewAddress.BackColor = System.Drawing.Color.SeaShell;
            this.cmdNewAddress.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cmdNewAddress.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdNewAddress.ForeColor = System.Drawing.Color.Sienna;
            this.cmdNewAddress.Location = new System.Drawing.Point(185, 294);
            this.cmdNewAddress.Name = "cmdNewAddress";
            this.cmdNewAddress.Size = new System.Drawing.Size(80, 26);
            this.cmdNewAddress.TabIndex = 14;
            this.cmdNewAddress.Text = "New a&dresse";
            this.cmdNewAddress.UseVisualStyleBackColor = false;
            this.cmdNewAddress.Click += new System.EventHandler(this.cmdNewAddress_Click);
            // 
            // lblRefreshAllSubDgv
            // 
            this.lblRefreshAllSubDgv.AutoSize = true;
            this.lblRefreshAllSubDgv.LinkColor = System.Drawing.Color.Green;
            this.lblRefreshAllSubDgv.Location = new System.Drawing.Point(188, 250);
            this.lblRefreshAllSubDgv.Name = "lblRefreshAllSubDgv";
            this.lblRefreshAllSubDgv.Size = new System.Drawing.Size(84, 13);
            this.lblRefreshAllSubDgv.TabIndex = 15;
            this.lblRefreshAllSubDgv.TabStop = true;
            this.lblRefreshAllSubDgv.Text = "Refresh subform";
            this.lblRefreshAllSubDgv.VisitedLinkColor = System.Drawing.Color.Green;
            this.lblRefreshAllSubDgv.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblRefreshAllSubDgv_LinkClicked);
            // 
            // cmdNewEmail
            // 
            this.cmdNewEmail.BackColor = System.Drawing.Color.Linen;
            this.cmdNewEmail.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cmdNewEmail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdNewEmail.ForeColor = System.Drawing.Color.Olive;
            this.cmdNewEmail.Location = new System.Drawing.Point(99, 294);
            this.cmdNewEmail.Name = "cmdNewEmail";
            this.cmdNewEmail.Size = new System.Drawing.Size(80, 26);
            this.cmdNewEmail.TabIndex = 13;
            this.cmdNewEmail.Text = "New e-&mail";
            this.cmdNewEmail.UseVisualStyleBackColor = false;
            this.cmdNewEmail.Click += new System.EventHandler(this.cmdNewEmail_Click);
            // 
            // cmdNewPhone
            // 
            this.cmdNewPhone.BackColor = System.Drawing.Color.LavenderBlush;
            this.cmdNewPhone.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cmdNewPhone.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdNewPhone.ForeColor = System.Drawing.Color.Brown;
            this.cmdNewPhone.Location = new System.Drawing.Point(13, 294);
            this.cmdNewPhone.Name = "cmdNewPhone";
            this.cmdNewPhone.Size = new System.Drawing.Size(80, 26);
            this.cmdNewPhone.TabIndex = 12;
            this.cmdNewPhone.Text = "New p&hone";
            this.cmdNewPhone.UseVisualStyleBackColor = false;
            this.cmdNewPhone.Click += new System.EventHandler(this.cmdNewPhone_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox6.Controls.Add(this.dgvAdresse);
            this.groupBox6.Location = new System.Drawing.Point(487, 219);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(452, 102);
            this.groupBox6.TabIndex = 502;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Adresses de résidence";
            // 
            // dgvAdresse
            // 
            this.dgvAdresse.AllowUserToAddRows = false;
            this.dgvAdresse.AllowUserToDeleteRows = false;
            this.dgvAdresse.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvAdresse.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvAdresse.BackgroundColor = System.Drawing.Color.White;
            this.dgvAdresse.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAdresse.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colId_adresse,
            this.colDesignation2,
            this.colId_personne3});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Thistle;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvAdresse.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvAdresse.Location = new System.Drawing.Point(8, 15);
            this.dgvAdresse.MultiSelect = false;
            this.dgvAdresse.Name = "dgvAdresse";
            this.dgvAdresse.RowHeadersVisible = false;
            this.dgvAdresse.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAdresse.Size = new System.Drawing.Size(437, 78);
            this.dgvAdresse.TabIndex = 203;
            this.dgvAdresse.TabStop = false;
            // 
            // colId_adresse
            // 
            this.colId_adresse.DataPropertyName = "Id";
            this.colId_adresse.HeaderText = "Id";
            this.colId_adresse.Name = "colId_adresse";
            this.colId_adresse.Visible = false;
            // 
            // colDesignation2
            // 
            this.colDesignation2.DataPropertyName = "Designation";
            this.colDesignation2.HeaderText = "Adresse de résidence";
            this.colDesignation2.Name = "colDesignation2";
            this.colDesignation2.Width = 430;
            // 
            // colId_personne3
            // 
            this.colId_personne3.DataPropertyName = "Id_personne";
            this.colId_personne3.HeaderText = "Id_personne";
            this.colId_personne3.Name = "colId_personne3";
            this.colId_personne3.Visible = false;
            this.colId_personne3.Width = 430;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.dgvEmail);
            this.groupBox3.Location = new System.Drawing.Point(488, 115);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(452, 102);
            this.groupBox3.TabIndex = 501;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Adresses E-mail";
            // 
            // dgvEmail
            // 
            this.dgvEmail.AllowUserToAddRows = false;
            this.dgvEmail.AllowUserToDeleteRows = false;
            this.dgvEmail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvEmail.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvEmail.BackgroundColor = System.Drawing.Color.White;
            this.dgvEmail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEmail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colIdTel,
            this.colDesignation,
            this.colId_personne2});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Thistle;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvEmail.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvEmail.Location = new System.Drawing.Point(7, 18);
            this.dgvEmail.MultiSelect = false;
            this.dgvEmail.Name = "dgvEmail";
            this.dgvEmail.RowHeadersVisible = false;
            this.dgvEmail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEmail.Size = new System.Drawing.Size(439, 78);
            this.dgvEmail.TabIndex = 202;
            this.dgvEmail.TabStop = false;
            // 
            // colIdTel
            // 
            this.colIdTel.DataPropertyName = "Id";
            this.colIdTel.HeaderText = "Id";
            this.colIdTel.Name = "colIdTel";
            this.colIdTel.Visible = false;
            // 
            // colDesignation
            // 
            this.colDesignation.DataPropertyName = "Designation";
            this.colDesignation.HeaderText = "E-mail";
            this.colDesignation.Name = "colDesignation";
            this.colDesignation.Width = 430;
            // 
            // colId_personne2
            // 
            this.colId_personne2.DataPropertyName = "Id_personne";
            this.colId_personne2.HeaderText = "Id_personne";
            this.colId_personne2.Name = "colId_personne2";
            this.colId_personne2.Visible = false;
            // 
            // chkEtudiant
            // 
            this.chkEtudiant.AutoSize = true;
            this.chkEtudiant.ForeColor = System.Drawing.Color.Teal;
            this.chkEtudiant.Location = new System.Drawing.Point(102, 271);
            this.chkEtudiant.Name = "chkEtudiant";
            this.chkEtudiant.Size = new System.Drawing.Size(82, 17);
            this.chkEtudiant.TabIndex = 10;
            this.chkEtudiant.Text = "Est étudiant";
            this.chkEtudiant.UseVisualStyleBackColor = true;
            this.chkEtudiant.CheckedChanged += new System.EventHandler(this.chkEtudiant_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.dgvTelephone);
            this.groupBox4.Location = new System.Drawing.Point(488, 8);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(451, 103);
            this.groupBox4.TabIndex = 500;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Numéro de téléphones";
            // 
            // dgvTelephone
            // 
            this.dgvTelephone.AllowUserToAddRows = false;
            this.dgvTelephone.AllowUserToDeleteRows = false;
            this.dgvTelephone.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTelephone.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvTelephone.BackgroundColor = System.Drawing.Color.White;
            this.dgvTelephone.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTelephone.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.colCode,
            this.colNumeroComplet,
            this.colNumero,
            this.colId_personne});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.Thistle;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTelephone.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvTelephone.Location = new System.Drawing.Point(7, 18);
            this.dgvTelephone.MultiSelect = false;
            this.dgvTelephone.Name = "dgvTelephone";
            this.dgvTelephone.RowHeadersVisible = false;
            this.dgvTelephone.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTelephone.Size = new System.Drawing.Size(437, 78);
            this.dgvTelephone.TabIndex = 201;
            this.dgvTelephone.TabStop = false;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Id";
            this.dataGridViewTextBoxColumn1.HeaderText = "Id";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Visible = false;
            // 
            // colCode
            // 
            this.colCode.DataPropertyName = "Code";
            this.colCode.HeaderText = "Code";
            this.colCode.Name = "colCode";
            this.colCode.Visible = false;
            // 
            // colNumeroComplet
            // 
            this.colNumeroComplet.DataPropertyName = "NumeroComplet";
            this.colNumeroComplet.HeaderText = "Numéro";
            this.colNumeroComplet.Name = "colNumeroComplet";
            this.colNumeroComplet.Width = 430;
            // 
            // colNumero
            // 
            this.colNumero.DataPropertyName = "Numero";
            this.colNumero.HeaderText = "Numero";
            this.colNumero.Name = "colNumero";
            this.colNumero.Visible = false;
            this.colNumero.Width = 110;
            // 
            // colId_personne
            // 
            this.colId_personne.DataPropertyName = "Id_personne";
            this.colId_personne.HeaderText = "Id_personne";
            this.colId_personne.Name = "colId_personne";
            this.colId_personne.Visible = false;
            // 
            // chkAgent
            // 
            this.chkAgent.AutoSize = true;
            this.chkAgent.ForeColor = System.Drawing.Color.RoyalBlue;
            this.chkAgent.Location = new System.Drawing.Point(102, 248);
            this.chkAgent.Name = "chkAgent";
            this.chkAgent.Size = new System.Drawing.Size(71, 17);
            this.chkAgent.TabIndex = 9;
            this.chkAgent.TabStop = false;
            this.chkAgent.Text = "Est agent";
            this.chkAgent.UseVisualStyleBackColor = true;
            this.chkAgent.CheckedChanged += new System.EventHandler(this.chkAgent_CheckedChanged);
            // 
            // txtDateNaissance
            // 
            this.txtDateNaissance.Location = new System.Drawing.Point(102, 174);
            this.txtDateNaissance.Mask = "00/00/0000";
            this.txtDateNaissance.Name = "txtDateNaissance";
            this.txtDateNaissance.Size = new System.Drawing.Size(139, 20);
            this.txtDateNaissance.TabIndex = 5;
            this.txtDateNaissance.ValidatingType = typeof(System.DateTime);
            // 
            // txtPrenom
            // 
            this.txtPrenom.Location = new System.Drawing.Point(102, 93);
            this.txtPrenom.Name = "txtPrenom";
            this.txtPrenom.Size = new System.Drawing.Size(139, 20);
            this.txtPrenom.TabIndex = 2;
            // 
            // txtPostNom
            // 
            this.txtPostNom.Location = new System.Drawing.Point(102, 68);
            this.txtPostNom.Name = "txtPostNom";
            this.txtPostNom.Size = new System.Drawing.Size(139, 20);
            this.txtPostNom.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 73);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 160;
            this.label5.Text = "Postnom : ";
            // 
            // chkEnseignant
            // 
            this.chkEnseignant.AutoSize = true;
            this.chkEnseignant.ForeColor = System.Drawing.Color.Purple;
            this.chkEnseignant.Location = new System.Drawing.Point(102, 227);
            this.chkEnseignant.Name = "chkEnseignant";
            this.chkEnseignant.Size = new System.Drawing.Size(96, 17);
            this.chkEnseignant.TabIndex = 8;
            this.chkEnseignant.TabStop = false;
            this.chkEnseignant.Text = "Est enseignant";
            this.chkEnseignant.UseVisualStyleBackColor = true;
            this.chkEnseignant.CheckedChanged += new System.EventHandler(this.chkEnseignant_CheckedChanged);
            // 
            // cboEtatCivil
            // 
            this.cboEtatCivil.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEtatCivil.DropDownWidth = 130;
            this.cboEtatCivil.FormattingEnabled = true;
            this.cboEtatCivil.Location = new System.Drawing.Point(102, 146);
            this.cboEtatCivil.Name = "cboEtatCivil";
            this.cboEtatCivil.Size = new System.Drawing.Size(139, 21);
            this.cboEtatCivil.TabIndex = 4;
            // 
            // cboGrade
            // 
            this.cboGrade.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGrade.DropDownWidth = 130;
            this.cboGrade.FormattingEnabled = true;
            this.cboGrade.Location = new System.Drawing.Point(102, 201);
            this.cboGrade.Name = "cboGrade";
            this.cboGrade.Size = new System.Drawing.Size(139, 21);
            this.cboGrade.TabIndex = 6;
            this.cboGrade.DropDown += new System.EventHandler(this.cboGrade_DropDown);
            // 
            // txtDateModifie
            // 
            this.txtDateModifie.Location = new System.Drawing.Point(342, 91);
            this.txtDateModifie.Name = "txtDateModifie";
            this.txtDateModifie.ReadOnly = true;
            this.txtDateModifie.Size = new System.Drawing.Size(139, 20);
            this.txtDateModifie.TabIndex = 156;
            this.txtDateModifie.TabStop = false;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(247, 95);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(98, 13);
            this.label17.TabIndex = 48;
            this.label17.Text = "Date modification : ";
            // 
            // txtModifieBy
            // 
            this.txtModifieBy.Location = new System.Drawing.Point(342, 66);
            this.txtModifieBy.Name = "txtModifieBy";
            this.txtModifieBy.ReadOnly = true;
            this.txtModifieBy.Size = new System.Drawing.Size(139, 20);
            this.txtModifieBy.TabIndex = 155;
            this.txtModifieBy.TabStop = false;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(247, 70);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(68, 13);
            this.label18.TabIndex = 46;
            this.label18.Text = "Modifier par :";
            // 
            // txtDateCreate
            // 
            this.txtDateCreate.Location = new System.Drawing.Point(342, 41);
            this.txtDateCreate.Name = "txtDateCreate";
            this.txtDateCreate.ReadOnly = true;
            this.txtDateCreate.Size = new System.Drawing.Size(139, 20);
            this.txtDateCreate.TabIndex = 154;
            this.txtDateCreate.TabStop = false;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(247, 45);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(80, 13);
            this.label19.TabIndex = 44;
            this.label19.Text = "Date création : ";
            // 
            // txtCreateBy
            // 
            this.txtCreateBy.Location = new System.Drawing.Point(342, 15);
            this.txtCreateBy.Name = "txtCreateBy";
            this.txtCreateBy.ReadOnly = true;
            this.txtCreateBy.Size = new System.Drawing.Size(139, 20);
            this.txtCreateBy.TabIndex = 153;
            this.txtCreateBy.TabStop = false;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(247, 19);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(53, 13);
            this.label20.TabIndex = 42;
            this.label20.Text = "Créé par :";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.lblPhoto);
            this.groupBox5.Controls.Add(this.pbPhoto);
            this.groupBox5.Location = new System.Drawing.Point(278, 115);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(203, 206);
            this.groupBox5.TabIndex = 5;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Vue de profil de la personne";
            // 
            // lblPhoto
            // 
            this.lblPhoto.AutoSize = true;
            this.lblPhoto.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.lblPhoto.Location = new System.Drawing.Point(52, 184);
            this.lblPhoto.Name = "lblPhoto";
            this.lblPhoto.Size = new System.Drawing.Size(95, 13);
            this.lblPhoto.TabIndex = 11;
            this.lblPhoto.TabStop = true;
            this.lblPhoto.Text = "Charger une photo";
            this.lblPhoto.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.lblPhoto.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblPhoto_LinkClicked);
            // 
            // pbPhoto
            // 
            this.pbPhoto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbPhoto.Location = new System.Drawing.Point(9, 20);
            this.pbPhoto.Name = "pbPhoto";
            this.pbPhoto.Size = new System.Drawing.Size(185, 161);
            this.pbPhoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbPhoto.TabIndex = 2;
            this.pbPhoto.TabStop = false;
            this.pbPhoto.MouseLeave += new System.EventHandler(this.pbPhoto_MouseLeave);
            this.pbPhoto.MouseHover += new System.EventHandler(this.pbPhoto_MouseHover);
            // 
            // lblAddGrade
            // 
            this.lblAddGrade.AutoSize = true;
            this.lblAddGrade.Location = new System.Drawing.Point(243, 206);
            this.lblAddGrade.Name = "lblAddGrade";
            this.lblAddGrade.Size = new System.Drawing.Size(29, 13);
            this.lblAddGrade.TabIndex = 7;
            this.lblAddGrade.TabStop = true;
            this.lblAddGrade.Text = "New";
            this.lblAddGrade.VisitedLinkColor = System.Drawing.Color.Blue;
            this.lblAddGrade.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblAddGrade_LinkClicked);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 206);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(75, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "Grade acad. : ";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 177);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(105, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Date de naissance : ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 150);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Etat civil : ";
            // 
            // cboSexe
            // 
            this.cboSexe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSexe.DropDownWidth = 109;
            this.cboSexe.FormattingEnabled = true;
            this.cboSexe.Location = new System.Drawing.Point(102, 119);
            this.cboSexe.Name = "cboSexe";
            this.cboSexe.Size = new System.Drawing.Size(139, 21);
            this.cboSexe.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 123);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Sexe : ";
            // 
            // txtNom
            // 
            this.txtNom.Location = new System.Drawing.Point(102, 42);
            this.txtNom.Name = "txtNom";
            this.txtNom.Size = new System.Drawing.Size(139, 20);
            this.txtNom.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Prénom : ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Nom : ";
            // 
            // txtId
            // 
            this.txtId.Location = new System.Drawing.Point(102, 15);
            this.txtId.Name = "txtId";
            this.txtId.ReadOnly = true;
            this.txtId.Size = new System.Drawing.Size(139, 20);
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
            // ctxMenuPhoto
            // 
            this.ctxMenuPhoto.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.smnCtxPhoto});
            this.ctxMenuPhoto.Name = "ctxMenuPhoto1";
            this.ctxMenuPhoto.Size = new System.Drawing.Size(175, 26);
            // 
            // smnCtxPhoto
            // 
            this.smnCtxPhoto.ForeColor = System.Drawing.Color.SaddleBrown;
            this.smnCtxPhoto.Name = "smnCtxPhoto";
            this.smnCtxPhoto.Size = new System.Drawing.Size(174, 22);
            this.smnCtxPhoto.Text = "Charger une photo";
            this.smnCtxPhoto.ToolTipText = "Charger photo1";
            this.smnCtxPhoto.Click += new System.EventHandler(this.smnCtxPhoto_Click);
            // 
            // frmPersonne
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.ClientSize = new System.Drawing.Size(962, 578);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmPersonne";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Text = "Identification d\'une personne";
            this.Activated += new System.EventHandler(this.frmPersonne_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmPersonne_FormClosed);
            this.Load += new System.EventHandler(this.frmPersonne_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAdresse)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEmail)).EndInit();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTelephone)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPhoto)).EndInit();
            this.ctxMenuPhoto.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox cboSexe;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtNom;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.LinkLabel lblAddGrade;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.LinkLabel lblPhoto;
        private System.Windows.Forms.PictureBox pbPhoto;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtModifieBy;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtDateCreate;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txtCreateBy;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox txtDateModifie;
        private System.Windows.Forms.ComboBox cboGrade;
        private System.Windows.Forms.ComboBox cboEtatCivil;
        private System.Windows.Forms.ContextMenuStrip ctxMenuPhoto;
        private System.Windows.Forms.ToolStripMenuItem smnCtxPhoto;
        private System.Windows.Forms.CheckBox chkEnseignant;
        private System.Windows.Forms.TextBox txtPostNom;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPrenom;
        private System.Windows.Forms.MaskedTextBox txtDateNaissance;
        private System.Windows.Forms.CheckBox chkEtudiant;
        private System.Windows.Forms.CheckBox chkAgent;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button cmdNewAddress;
        private System.Windows.Forms.Button cmdNewEmail;
        private System.Windows.Forms.Button cmdNewPhone;
        private System.Windows.Forms.DataGridView dgvTelephone;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNumeroComplet;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNumero;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId_personne;
        private System.Windows.Forms.DataGridView dgvEmail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIdTel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDesignation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId_personne2;
        private System.Windows.Forms.DataGridView dgvAdresse;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId_adresse;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDesignation2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId_personne3;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNom;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPostnom;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrenom;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSexe;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEtatcivil;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDatenaissance;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colIsenseignant;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colIsagent;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colIsetudiant;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPhoto;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId_grade;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUser_created;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate_created;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUser_modified;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate_modified;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNomComplet;
        private System.Windows.Forms.LinkLabel lblRefreshAllSubDgv;
    }
}