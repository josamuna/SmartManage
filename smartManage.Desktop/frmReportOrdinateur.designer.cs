namespace smartManage.Desktop
{
    partial class frmReportOrdinateur
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtDateAcquisitionFin = new System.Windows.Forms.DateTimePicker();
            this.txtDateAcquisitionDebut = new System.Windows.Forms.DateTimePicker();
            this.rdLstDateAcquisition = new System.Windows.Forms.RadioButton();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cboIdentifiant = new System.Windows.Forms.ComboBox();
            this.rdLstIdentifiant = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.cboMacWifi = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cboMacLAN = new System.Windows.Forms.ComboBox();
            this.rdLstMAC = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.rdLstEndGarantie = new System.Windows.Forms.RadioButton();
            this.cboDelais = new System.Windows.Forms.ComboBox();
            this.rdLstEtat = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.cboItems = new System.Windows.Forms.ComboBox();
            this.cboEtat = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmdView = new System.Windows.Forms.Button();
            this.chkArchiver = new System.Windows.Forms.CheckBox();
            this.crvReport = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Beige;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.cmdView);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1289, 73);
            this.panel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkArchiver);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtDateAcquisitionFin);
            this.groupBox1.Controls.Add(this.txtDateAcquisitionDebut);
            this.groupBox1.Controls.Add(this.rdLstDateAcquisition);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.cboIdentifiant);
            this.groupBox1.Controls.Add(this.rdLstIdentifiant);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.cboMacWifi);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cboMacLAN);
            this.groupBox1.Controls.Add(this.rdLstMAC);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.rdLstEndGarantie);
            this.groupBox1.Controls.Add(this.cboDelais);
            this.groupBox1.Controls.Add(this.rdLstEtat);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cboItems);
            this.groupBox1.Controls.Add(this.cboEtat);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(6, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1189, 63);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.DarkSlateGray;
            this.label10.Location = new System.Drawing.Point(1023, 7);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(1, 55);
            this.label10.TabIndex = 155;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(1029, 42);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(27, 13);
            this.label9.TabIndex = 154;
            this.label9.Text = "Fin :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(1028, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 13);
            this.label5.TabIndex = 153;
            this.label5.Text = "Début :";
            // 
            // txtDateAcquisitionFin
            // 
            this.txtDateAcquisitionFin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtDateAcquisitionFin.Location = new System.Drawing.Point(1073, 38);
            this.txtDateAcquisitionFin.Name = "txtDateAcquisitionFin";
            this.txtDateAcquisitionFin.Size = new System.Drawing.Size(109, 20);
            this.txtDateAcquisitionFin.TabIndex = 12;
            // 
            // txtDateAcquisitionDebut
            // 
            this.txtDateAcquisitionDebut.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtDateAcquisitionDebut.Location = new System.Drawing.Point(1073, 13);
            this.txtDateAcquisitionDebut.Name = "txtDateAcquisitionDebut";
            this.txtDateAcquisitionDebut.Size = new System.Drawing.Size(109, 20);
            this.txtDateAcquisitionDebut.TabIndex = 11;
            // 
            // rdLstDateAcquisition
            // 
            this.rdLstDateAcquisition.AutoSize = true;
            this.rdLstDateAcquisition.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.rdLstDateAcquisition.Location = new System.Drawing.Point(399, 11);
            this.rdLstDateAcquisition.Name = "rdLstDateAcquisition";
            this.rdLstDateAcquisition.Size = new System.Drawing.Size(65, 17);
            this.rdLstDateAcquisition.TabIndex = 4;
            this.rdLstDateAcquisition.TabStop = true;
            this.rdLstDateAcquisition.Text = "Par date";
            this.rdLstDateAcquisition.UseVisualStyleBackColor = true;
            this.rdLstDateAcquisition.CheckedChanged += new System.EventHandler(this.rdLstDateAcquisition_CheckedChanged);
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.DarkSlateGray;
            this.label8.Location = new System.Drawing.Point(465, 7);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(1, 55);
            this.label8.TabIndex = 150;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(470, 13);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(78, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "ID du matériel :";
            // 
            // cboIdentifiant
            // 
            this.cboIdentifiant.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboIdentifiant.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboIdentifiant.DropDownWidth = 150;
            this.cboIdentifiant.FormattingEnabled = true;
            this.cboIdentifiant.Location = new System.Drawing.Point(472, 34);
            this.cboIdentifiant.Name = "cboIdentifiant";
            this.cboIdentifiant.Size = new System.Drawing.Size(96, 21);
            this.cboIdentifiant.TabIndex = 6;
            // 
            // rdLstIdentifiant
            // 
            this.rdLstIdentifiant.AutoSize = true;
            this.rdLstIdentifiant.ForeColor = System.Drawing.Color.DarkCyan;
            this.rdLstIdentifiant.Location = new System.Drawing.Point(6, 11);
            this.rdLstIdentifiant.Name = "rdLstIdentifiant";
            this.rdLstIdentifiant.Size = new System.Drawing.Size(90, 17);
            this.rdLstIdentifiant.TabIndex = 0;
            this.rdLstIdentifiant.TabStop = true;
            this.rdLstIdentifiant.Text = "Par Identifiant";
            this.rdLstIdentifiant.UseVisualStyleBackColor = true;
            this.rdLstIdentifiant.CheckedChanged += new System.EventHandler(this.rdLstIdentifiant_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(787, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "MAC Wifi :";
            // 
            // cboMacWifi
            // 
            this.cboMacWifi.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboMacWifi.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboMacWifi.DropDownWidth = 150;
            this.cboMacWifi.FormattingEnabled = true;
            this.cboMacWifi.Location = new System.Drawing.Point(789, 34);
            this.cboMacWifi.Name = "cboMacWifi";
            this.cboMacWifi.Size = new System.Drawing.Size(111, 21);
            this.cboMacWifi.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(904, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "MAC LAN :";
            // 
            // cboMacLAN
            // 
            this.cboMacLAN.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboMacLAN.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboMacLAN.DropDownWidth = 150;
            this.cboMacLAN.FormattingEnabled = true;
            this.cboMacLAN.Location = new System.Drawing.Point(906, 34);
            this.cboMacLAN.Name = "cboMacLAN";
            this.cboMacLAN.Size = new System.Drawing.Size(111, 21);
            this.cboMacLAN.TabIndex = 10;
            // 
            // rdLstMAC
            // 
            this.rdLstMAC.AutoSize = true;
            this.rdLstMAC.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.rdLstMAC.Location = new System.Drawing.Point(288, 11);
            this.rdLstMAC.Name = "rdLstMAC";
            this.rdLstMAC.Size = new System.Drawing.Size(107, 17);
            this.rdLstMAC.TabIndex = 3;
            this.rdLstMAC.TabStop = true;
            this.rdLstMAC.Text = "Par adresse MAC";
            this.rdLstMAC.UseVisualStyleBackColor = true;
            this.rdLstMAC.CheckedChanged += new System.EventHandler(this.rdLstMAC_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(689, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Délais (années) :";
            // 
            // rdLstEndGarantie
            // 
            this.rdLstEndGarantie.AutoSize = true;
            this.rdLstEndGarantie.ForeColor = System.Drawing.Color.SeaGreen;
            this.rdLstEndGarantie.Location = new System.Drawing.Point(164, 11);
            this.rdLstEndGarantie.Name = "rdLstEndGarantie";
            this.rdLstEndGarantie.Size = new System.Drawing.Size(120, 17);
            this.rdLstEndGarantie.TabIndex = 2;
            this.rdLstEndGarantie.TabStop = true;
            this.rdLstEndGarantie.Text = "Par delais dépassée";
            this.rdLstEndGarantie.UseVisualStyleBackColor = true;
            this.rdLstEndGarantie.CheckedChanged += new System.EventHandler(this.rdLstEndGarantie_CheckedChanged);
            // 
            // cboDelais
            // 
            this.cboDelais.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboDelais.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboDelais.DropDownWidth = 150;
            this.cboDelais.FormattingEnabled = true;
            this.cboDelais.Location = new System.Drawing.Point(692, 34);
            this.cboDelais.Name = "cboDelais";
            this.cboDelais.Size = new System.Drawing.Size(91, 21);
            this.cboDelais.TabIndex = 8;
            // 
            // rdLstEtat
            // 
            this.rdLstEtat.AutoSize = true;
            this.rdLstEtat.ForeColor = System.Drawing.Color.DarkOliveGreen;
            this.rdLstEtat.Location = new System.Drawing.Point(98, 11);
            this.rdLstEtat.Name = "rdLstEtat";
            this.rdLstEtat.Size = new System.Drawing.Size(63, 17);
            this.rdLstEtat.TabIndex = 1;
            this.rdLstEtat.TabStop = true;
            this.rdLstEtat.Text = "Par Etat";
            this.rdLstEtat.UseVisualStyleBackColor = true;
            this.rdLstEtat.CheckedChanged += new System.EventHandler(this.rdLstEtat_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(571, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Etat du matériel :";
            // 
            // cboItems
            // 
            this.cboItems.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboItems.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboItems.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboItems.DropDownWidth = 250;
            this.cboItems.FormattingEnabled = true;
            this.cboItems.Location = new System.Drawing.Point(162, 34);
            this.cboItems.Name = "cboItems";
            this.cboItems.Size = new System.Drawing.Size(295, 21);
            this.cboItems.TabIndex = 5;
            // 
            // cboEtat
            // 
            this.cboEtat.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboEtat.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboEtat.DropDownWidth = 150;
            this.cboEtat.FormattingEnabled = true;
            this.cboEtat.Location = new System.Drawing.Point(573, 34);
            this.cboEtat.Name = "cboEtat";
            this.cboEtat.Size = new System.Drawing.Size(113, 21);
            this.cboEtat.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(81, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Selection item :";
            // 
            // cmdView
            // 
            this.cmdView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdView.BackColor = System.Drawing.Color.SeaShell;
            this.cmdView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdView.ForeColor = System.Drawing.Color.DarkRed;
            this.cmdView.Location = new System.Drawing.Point(1204, 26);
            this.cmdView.Name = "cmdView";
            this.cmdView.Size = new System.Drawing.Size(74, 22);
            this.cmdView.TabIndex = 13;
            this.cmdView.Text = "Afficher";
            this.cmdView.UseVisualStyleBackColor = false;
            this.cmdView.Click += new System.EventHandler(this.cmdView_Click);
            // 
            // chkArchiver
            // 
            this.chkArchiver.AutoSize = true;
            this.chkArchiver.ForeColor = System.Drawing.Color.MediumVioletRed;
            this.chkArchiver.Location = new System.Drawing.Point(5, 38);
            this.chkArchiver.Name = "chkArchiver";
            this.chkArchiver.Size = new System.Drawing.Size(62, 17);
            this.chkArchiver.TabIndex = 158;
            this.chkArchiver.TabStop = false;
            this.chkArchiver.Text = "Archivé";
            this.chkArchiver.UseVisualStyleBackColor = true;
            // 
            // crvReport
            // 
            this.crvReport.ActiveViewIndex = -1;
            this.crvReport.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crvReport.Cursor = System.Windows.Forms.Cursors.Default;
            this.crvReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crvReport.Location = new System.Drawing.Point(0, 73);
            this.crvReport.Name = "crvReport";
            this.crvReport.Size = new System.Drawing.Size(1289, 482);
            this.crvReport.TabIndex = 1;
            // 
            // frmReportOrdinateur
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1289, 555);
            this.Controls.Add(this.crvReport);
            this.Controls.Add(this.panel1);
            this.Name = "frmReportOrdinateur";
            this.Text = "Rapports pour Ordinateur";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmReportOrdinateur_FormClosed);
            this.Load += new System.EventHandler(this.frmReportOrdinateur_Load);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button cmdView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboItems;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboDelais;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboEtat;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdLstMAC;
        private System.Windows.Forms.RadioButton rdLstEndGarantie;
        private System.Windows.Forms.RadioButton rdLstEtat;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboMacLAN;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cboMacWifi;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cboIdentifiant;
        private System.Windows.Forms.RadioButton rdLstIdentifiant;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.RadioButton rdLstDateAcquisition;
        private System.Windows.Forms.DateTimePicker txtDateAcquisitionFin;
        private System.Windows.Forms.DateTimePicker txtDateAcquisitionDebut;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkArchiver;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer crvReport;
    }
}