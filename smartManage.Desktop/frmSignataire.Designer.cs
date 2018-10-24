namespace smartManage.Desktop
{
    partial class frmSignataire
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCode_ac = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSignature_specimen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colId_personne = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUser_created = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDate_created = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUser_modified = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDate_modified = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNomComplet = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cboAC = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboPersonne = new System.Windows.Forms.ComboBox();
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
            this.lblAddAC = new System.Windows.Forms.LinkLabel();
            this.label9 = new System.Windows.Forms.Label();
            this.txtId = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ctxMenuPhoto = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.smnCtxPhoto = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
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
            this.colCode_ac,
            this.colSignature_specimen,
            this.colId_personne,
            this.colUser_created,
            this.colDate_created,
            this.colUser_modified,
            this.colDate_modified,
            this.colNomComplet});
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
            this.dgv.Size = new System.Drawing.Size(523, 168);
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
            // colCode_ac
            // 
            this.colCode_ac.DataPropertyName = "Code_ac";
            this.colCode_ac.HeaderText = "Année Acad.";
            this.colCode_ac.Name = "colCode_ac";
            this.colCode_ac.Width = 120;
            // 
            // colSignature_specimen
            // 
            this.colSignature_specimen.DataPropertyName = "Signature_specimen";
            this.colSignature_specimen.HeaderText = "Signature_specimen";
            this.colSignature_specimen.Name = "colSignature_specimen";
            this.colSignature_specimen.Visible = false;
            // 
            // colId_personne
            // 
            this.colId_personne.DataPropertyName = "Id_personne";
            this.colId_personne.HeaderText = "Id_personne";
            this.colId_personne.Name = "colId_personne";
            this.colId_personne.Visible = false;
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
            this.groupBox1.Location = new System.Drawing.Point(8, 205);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(529, 193);
            this.groupBox1.TabIndex = 540;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Affichage des données manipulées";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.cboAC);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.cboPersonne);
            this.groupBox2.Controls.Add(this.txtDateModifie);
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Controls.Add(this.txtModifieBy);
            this.groupBox2.Controls.Add(this.label18);
            this.groupBox2.Controls.Add(this.txtDateCreate);
            this.groupBox2.Controls.Add(this.label19);
            this.groupBox2.Controls.Add(this.txtCreateBy);
            this.groupBox2.Controls.Add(this.label20);
            this.groupBox2.Controls.Add(this.groupBox5);
            this.groupBox2.Controls.Add(this.lblAddAC);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.txtId);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(8, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(529, 196);
            this.groupBox2.TabIndex = 510;
            this.groupBox2.TabStop = false;
            // 
            // cboAC
            // 
            this.cboAC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAC.DropDownWidth = 130;
            this.cboAC.FormattingEnabled = true;
            this.cboAC.Location = new System.Drawing.Point(102, 66);
            this.cboAC.Name = "cboAC";
            this.cboAC.Size = new System.Drawing.Size(185, 21);
            this.cboAC.TabIndex = 1;
            this.cboAC.DropDown += new System.EventHandler(this.cboAC_DropDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 159;
            this.label2.Text = "Année Acad. : ";
            // 
            // cboPersonne
            // 
            this.cboPersonne.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboPersonne.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboPersonne.DropDownWidth = 130;
            this.cboPersonne.FormattingEnabled = true;
            this.cboPersonne.Location = new System.Drawing.Point(102, 40);
            this.cboPersonne.Name = "cboPersonne";
            this.cboPersonne.Size = new System.Drawing.Size(185, 21);
            this.cboPersonne.TabIndex = 0;
            this.cboPersonne.DropDown += new System.EventHandler(this.cboPersonne_DropDown);
            // 
            // txtDateModifie
            // 
            this.txtDateModifie.Location = new System.Drawing.Point(102, 167);
            this.txtDateModifie.Name = "txtDateModifie";
            this.txtDateModifie.ReadOnly = true;
            this.txtDateModifie.Size = new System.Drawing.Size(185, 20);
            this.txtDateModifie.TabIndex = 156;
            this.txtDateModifie.TabStop = false;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(7, 171);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(98, 13);
            this.label17.TabIndex = 48;
            this.label17.Text = "Date modification : ";
            // 
            // txtModifieBy
            // 
            this.txtModifieBy.Location = new System.Drawing.Point(102, 142);
            this.txtModifieBy.Name = "txtModifieBy";
            this.txtModifieBy.ReadOnly = true;
            this.txtModifieBy.Size = new System.Drawing.Size(185, 20);
            this.txtModifieBy.TabIndex = 155;
            this.txtModifieBy.TabStop = false;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(7, 146);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(68, 13);
            this.label18.TabIndex = 46;
            this.label18.Text = "Modifier par :";
            // 
            // txtDateCreate
            // 
            this.txtDateCreate.Location = new System.Drawing.Point(102, 117);
            this.txtDateCreate.Name = "txtDateCreate";
            this.txtDateCreate.ReadOnly = true;
            this.txtDateCreate.Size = new System.Drawing.Size(185, 20);
            this.txtDateCreate.TabIndex = 154;
            this.txtDateCreate.TabStop = false;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(7, 121);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(80, 13);
            this.label19.TabIndex = 44;
            this.label19.Text = "Date création : ";
            // 
            // txtCreateBy
            // 
            this.txtCreateBy.Location = new System.Drawing.Point(102, 92);
            this.txtCreateBy.Name = "txtCreateBy";
            this.txtCreateBy.ReadOnly = true;
            this.txtCreateBy.Size = new System.Drawing.Size(185, 20);
            this.txtCreateBy.TabIndex = 153;
            this.txtCreateBy.TabStop = false;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(7, 96);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(53, 13);
            this.label20.TabIndex = 42;
            this.label20.Text = "Créé par :";
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.lblPhoto);
            this.groupBox5.Controls.Add(this.pbPhoto);
            this.groupBox5.Location = new System.Drawing.Point(324, 9);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(197, 179);
            this.groupBox5.TabIndex = 5;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Spécimen de signature";
            // 
            // lblPhoto
            // 
            this.lblPhoto.AutoSize = true;
            this.lblPhoto.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.lblPhoto.Location = new System.Drawing.Point(50, 158);
            this.lblPhoto.Name = "lblPhoto";
            this.lblPhoto.Size = new System.Drawing.Size(95, 13);
            this.lblPhoto.TabIndex = 3;
            this.lblPhoto.TabStop = true;
            this.lblPhoto.Text = "Charger une photo";
            this.lblPhoto.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.lblPhoto.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblPhoto_LinkClicked);
            // 
            // pbPhoto
            // 
            this.pbPhoto.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbPhoto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbPhoto.Location = new System.Drawing.Point(13, 20);
            this.pbPhoto.Name = "pbPhoto";
            this.pbPhoto.Size = new System.Drawing.Size(172, 135);
            this.pbPhoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbPhoto.TabIndex = 2;
            this.pbPhoto.TabStop = false;
            this.pbPhoto.MouseLeave += new System.EventHandler(this.pbPhoto_MouseLeave);
            this.pbPhoto.MouseHover += new System.EventHandler(this.pbPhoto_MouseHover);
            // 
            // lblAddAC
            // 
            this.lblAddAC.AutoSize = true;
            this.lblAddAC.Location = new System.Drawing.Point(289, 70);
            this.lblAddAC.Name = "lblAddAC";
            this.lblAddAC.Size = new System.Drawing.Size(29, 13);
            this.lblAddAC.TabIndex = 2;
            this.lblAddAC.TabStop = true;
            this.lblAddAC.Text = "New";
            this.lblAddAC.VisitedLinkColor = System.Drawing.Color.Blue;
            this.lblAddAC.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblAddAC_LinkClicked);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 46);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(61, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "Personne : ";
            // 
            // txtId
            // 
            this.txtId.Location = new System.Drawing.Point(102, 15);
            this.txtId.Name = "txtId";
            this.txtId.ReadOnly = true;
            this.txtId.Size = new System.Drawing.Size(185, 20);
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
            // frmSignataire
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.ClientSize = new System.Drawing.Size(544, 406);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmSignataire";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Text = "Identification d\'un signataire";
            this.Activated += new System.EventHandler(this.frmPersonne_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmSignataire_FormClosed);
            this.Load += new System.EventHandler(this.frmSignataire_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
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
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel lblAddAC;
        private System.Windows.Forms.Label label9;
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
        private System.Windows.Forms.ComboBox cboPersonne;
        private System.Windows.Forms.ContextMenuStrip ctxMenuPhoto;
        private System.Windows.Forms.ToolStripMenuItem smnCtxPhoto;
        private System.Windows.Forms.ComboBox cboAC;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCode_ac;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSignature_specimen;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId_personne;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUser_created;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate_created;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUser_modified;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate_modified;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNomComplet;
    }
}