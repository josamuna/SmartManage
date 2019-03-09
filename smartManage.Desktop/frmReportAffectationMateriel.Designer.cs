namespace smartManage.Desktop
{
    partial class frmReportAffectationMateriel
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
            if(disposing && (conn != null))
            {
                conn.Close();
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkArchiver = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDateFinAffectation = new System.Windows.Forms.DateTimePicker();
            this.txtDateDebutAffectation = new System.Windows.Forms.DateTimePicker();
            this.cboCategorieItem = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cboIdentifiant = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cboLieuAffectation = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cboCategorieMateriel = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboTypeLieuAffectation = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboAC = new System.Windows.Forms.ComboBox();
            this.cmdView = new System.Windows.Forms.Button();
            this.rpvReport = new Microsoft.Reporting.WinForms.ReportViewer();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Beige;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.chkArchiver);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.cmdView);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1289, 67);
            this.panel1.TabIndex = 0;
            // 
            // chkArchiver
            // 
            this.chkArchiver.AutoSize = true;
            this.chkArchiver.ForeColor = System.Drawing.Color.MediumVioletRed;
            this.chkArchiver.Location = new System.Drawing.Point(1167, 40);
            this.chkArchiver.Name = "chkArchiver";
            this.chkArchiver.Size = new System.Drawing.Size(62, 17);
            this.chkArchiver.TabIndex = 8;
            this.chkArchiver.TabStop = false;
            this.chkArchiver.Text = "Archivé";
            this.chkArchiver.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtDateFinAffectation);
            this.groupBox1.Controls.Add(this.txtDateDebutAffectation);
            this.groupBox1.Controls.Add(this.cboCategorieItem);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.cboIdentifiant);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.cboLieuAffectation);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cboCategorieMateriel);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cboTypeLieuAffectation);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cboAC);
            this.groupBox1.Location = new System.Drawing.Point(6, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1150, 59);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.DarkSlateGray;
            this.label10.Location = new System.Drawing.Point(985, 7);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(1, 52);
            this.label10.TabIndex = 164;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(989, 39);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(27, 13);
            this.label9.TabIndex = 163;
            this.label9.Text = "Fin :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(988, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 162;
            this.label1.Text = "Début :";
            // 
            // txtDateFinAffectation
            // 
            this.txtDateFinAffectation.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtDateFinAffectation.Location = new System.Drawing.Point(1033, 35);
            this.txtDateFinAffectation.Name = "txtDateFinAffectation";
            this.txtDateFinAffectation.Size = new System.Drawing.Size(109, 20);
            this.txtDateFinAffectation.TabIndex = 7;
            // 
            // txtDateDebutAffectation
            // 
            this.txtDateDebutAffectation.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtDateDebutAffectation.Location = new System.Drawing.Point(1033, 10);
            this.txtDateDebutAffectation.Name = "txtDateDebutAffectation";
            this.txtDateDebutAffectation.Size = new System.Drawing.Size(109, 20);
            this.txtDateDebutAffectation.TabIndex = 6;
            // 
            // cboCategorieItem
            // 
            this.cboCategorieItem.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboCategorieItem.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboCategorieItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCategorieItem.DropDownWidth = 250;
            this.cboCategorieItem.FormattingEnabled = true;
            this.cboCategorieItem.Location = new System.Drawing.Point(7, 29);
            this.cboCategorieItem.Name = "cboCategorieItem";
            this.cboCategorieItem.Size = new System.Drawing.Size(246, 21);
            this.cboCategorieItem.TabIndex = 0;
            this.cboCategorieItem.SelectedIndexChanged += new System.EventHandler(this.cboCategorieItem_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(5, 12);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(118, 13);
            this.label11.TabIndex = 159;
            this.label11.Text = "Catégorie pour rapport :";
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.DarkSlateGray;
            this.label8.Location = new System.Drawing.Point(259, 7);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(1, 52);
            this.label8.TabIndex = 150;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(265, 11);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(78, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "ID du matériel :";
            // 
            // cboIdentifiant
            // 
            this.cboIdentifiant.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboIdentifiant.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboIdentifiant.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboIdentifiant.DropDownWidth = 150;
            this.cboIdentifiant.FormattingEnabled = true;
            this.cboIdentifiant.Location = new System.Drawing.Point(267, 29);
            this.cboIdentifiant.Name = "cboIdentifiant";
            this.cboIdentifiant.Size = new System.Drawing.Size(96, 21);
            this.cboIdentifiant.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(623, 11);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(94, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Lieu d\'affectation :";
            // 
            // cboLieuAffectation
            // 
            this.cboLieuAffectation.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboLieuAffectation.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboLieuAffectation.DropDownWidth = 150;
            this.cboLieuAffectation.FormattingEnabled = true;
            this.cboLieuAffectation.Location = new System.Drawing.Point(625, 29);
            this.cboLieuAffectation.Name = "cboLieuAffectation";
            this.cboLieuAffectation.Size = new System.Drawing.Size(216, 21);
            this.cboLieuAffectation.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(845, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Catégorie Mat. :";
            // 
            // cboCategorieMateriel
            // 
            this.cboCategorieMateriel.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboCategorieMateriel.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboCategorieMateriel.DropDownWidth = 150;
            this.cboCategorieMateriel.FormattingEnabled = true;
            this.cboCategorieMateriel.Location = new System.Drawing.Point(847, 29);
            this.cboCategorieMateriel.Name = "cboCategorieMateriel";
            this.cboCategorieMateriel.Size = new System.Drawing.Size(132, 21);
            this.cboCategorieMateriel.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(484, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Type lieu d\'aff. :";
            // 
            // cboTypeLieuAffectation
            // 
            this.cboTypeLieuAffectation.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboTypeLieuAffectation.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboTypeLieuAffectation.DropDownWidth = 150;
            this.cboTypeLieuAffectation.FormattingEnabled = true;
            this.cboTypeLieuAffectation.Location = new System.Drawing.Point(487, 29);
            this.cboTypeLieuAffectation.Name = "cboTypeLieuAffectation";
            this.cboTypeLieuAffectation.Size = new System.Drawing.Size(131, 21);
            this.cboTypeLieuAffectation.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(366, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Année académique :";
            // 
            // cboAC
            // 
            this.cboAC.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboAC.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboAC.DropDownWidth = 150;
            this.cboAC.FormattingEnabled = true;
            this.cboAC.Location = new System.Drawing.Point(368, 29);
            this.cboAC.Name = "cboAC";
            this.cboAC.Size = new System.Drawing.Size(113, 21);
            this.cboAC.TabIndex = 2;
            // 
            // cmdView
            // 
            this.cmdView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdView.BackColor = System.Drawing.Color.SeaShell;
            this.cmdView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdView.ForeColor = System.Drawing.Color.DarkRed;
            this.cmdView.Location = new System.Drawing.Point(1167, 9);
            this.cmdView.Name = "cmdView";
            this.cmdView.Size = new System.Drawing.Size(111, 22);
            this.cmdView.TabIndex = 9;
            this.cmdView.Text = "Afficher";
            this.cmdView.UseVisualStyleBackColor = false;
            this.cmdView.Click += new System.EventHandler(this.cmdView_Click);
            // 
            // rpvReport
            // 
            this.rpvReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rpvReport.Location = new System.Drawing.Point(0, 67);
            this.rpvReport.Name = "rpvReport";
            this.rpvReport.Size = new System.Drawing.Size(1289, 488);
            this.rpvReport.TabIndex = 2;
            // 
            // frmReportAffectationMateriel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1289, 555);
            this.Controls.Add(this.rpvReport);
            this.Controls.Add(this.panel1);
            this.Name = "frmReportAffectationMateriel";
            this.Text = "Rapports pour affectation des matériels  et/ou des personnes";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmReportAffectationMateriel_FormClosed);
            this.Load += new System.EventHandler(this.frmReportAffectationMateriel_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button cmdView;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboTypeLieuAffectation;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboAC;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboCategorieMateriel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cboLieuAffectation;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cboIdentifiant;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cboCategorieItem;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker txtDateFinAffectation;
        private System.Windows.Forms.DateTimePicker txtDateDebutAffectation;
        private System.Windows.Forms.CheckBox chkArchiver;
        private Microsoft.Reporting.WinForms.ReportViewer rpvReport;
    }
}