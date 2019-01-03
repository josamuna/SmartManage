namespace smartManage.Desktop
{
    partial class frmReportMateriel
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdLstPPM = new System.Windows.Forms.RadioButton();
            this.rdLstNetete = new System.Windows.Forms.RadioButton();
            this.rdLstFrequence = new System.Windows.Forms.RadioButton();
            this.rdLstMAC_Adresse = new System.Windows.Forms.RadioButton();
            this.rdLstPortee = new System.Windows.Forms.RadioButton();
            this.label14 = new System.Windows.Forms.Label();
            this.cboMACAdresse = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.cboPPM = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.cboPortee = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.cboNetete = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.cboFrequence = new System.Windows.Forms.ComboBox();
            this.cboCategorieMat = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.chkArchiver = new System.Windows.Forms.CheckBox();
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
            this.panel1.Size = new System.Drawing.Size(1289, 106);
            this.panel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdLstPPM);
            this.groupBox1.Controls.Add(this.rdLstNetete);
            this.groupBox1.Controls.Add(this.rdLstFrequence);
            this.groupBox1.Controls.Add(this.rdLstMAC_Adresse);
            this.groupBox1.Controls.Add(this.rdLstPortee);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.cboMACAdresse);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.cboPPM);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.cboPortee);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.cboNetete);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.cboFrequence);
            this.groupBox1.Controls.Add(this.cboCategorieMat);
            this.groupBox1.Controls.Add(this.label11);
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
            this.groupBox1.Size = new System.Drawing.Size(1189, 99);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            // 
            // rdLstPPM
            // 
            this.rdLstPPM.AutoSize = true;
            this.rdLstPPM.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.rdLstPPM.Location = new System.Drawing.Point(398, 78);
            this.rdLstPPM.Name = "rdLstPPM";
            this.rdLstPPM.Size = new System.Drawing.Size(67, 17);
            this.rdLstPPM.TabIndex = 20;
            this.rdLstPPM.TabStop = true;
            this.rdLstPPM.Text = "Par PPM";
            this.rdLstPPM.UseVisualStyleBackColor = true;
            this.rdLstPPM.CheckedChanged += new System.EventHandler(this.rdLstPPM_CheckedChanged);
            // 
            // rdLstNetete
            // 
            this.rdLstNetete.AutoSize = true;
            this.rdLstNetete.ForeColor = System.Drawing.Color.Teal;
            this.rdLstNetete.Location = new System.Drawing.Point(288, 78);
            this.rdLstNetete.Name = "rdLstNetete";
            this.rdLstNetete.Size = new System.Drawing.Size(76, 17);
            this.rdLstNetete.TabIndex = 19;
            this.rdLstNetete.TabStop = true;
            this.rdLstNetete.Text = "Par Netété";
            this.rdLstNetete.UseVisualStyleBackColor = true;
            this.rdLstNetete.CheckedChanged += new System.EventHandler(this.rdLstNetete_CheckedChanged);
            // 
            // rdLstFrequence
            // 
            this.rdLstFrequence.AutoSize = true;
            this.rdLstFrequence.ForeColor = System.Drawing.Color.DimGray;
            this.rdLstFrequence.Location = new System.Drawing.Point(164, 78);
            this.rdLstFrequence.Name = "rdLstFrequence";
            this.rdLstFrequence.Size = new System.Drawing.Size(95, 17);
            this.rdLstFrequence.TabIndex = 18;
            this.rdLstFrequence.TabStop = true;
            this.rdLstFrequence.Text = "Par Fréquence";
            this.rdLstFrequence.UseVisualStyleBackColor = true;
            this.rdLstFrequence.CheckedChanged += new System.EventHandler(this.rdLstFrequence_CheckedChanged);
            // 
            // rdLstMAC_Adresse
            // 
            this.rdLstMAC_Adresse.AutoSize = true;
            this.rdLstMAC_Adresse.ForeColor = System.Drawing.Color.OliveDrab;
            this.rdLstMAC_Adresse.Location = new System.Drawing.Point(98, 78);
            this.rdLstMAC_Adresse.Name = "rdLstMAC_Adresse";
            this.rdLstMAC_Adresse.Size = new System.Drawing.Size(67, 17);
            this.rdLstMAC_Adresse.TabIndex = 17;
            this.rdLstMAC_Adresse.TabStop = true;
            this.rdLstMAC_Adresse.Text = "Par MAC";
            this.rdLstMAC_Adresse.UseVisualStyleBackColor = true;
            this.rdLstMAC_Adresse.CheckedChanged += new System.EventHandler(this.rdLstMAC_Adresse_CheckedChanged);
            // 
            // rdLstPortee
            // 
            this.rdLstPortee.AutoSize = true;
            this.rdLstPortee.ForeColor = System.Drawing.Color.RoyalBlue;
            this.rdLstPortee.Location = new System.Drawing.Point(6, 78);
            this.rdLstPortee.Name = "rdLstPortee";
            this.rdLstPortee.Size = new System.Drawing.Size(74, 17);
            this.rdLstPortee.TabIndex = 16;
            this.rdLstPortee.TabStop = true;
            this.rdLstPortee.Text = "Par portée";
            this.rdLstPortee.UseVisualStyleBackColor = true;
            this.rdLstPortee.CheckedChanged += new System.EventHandler(this.rdLstPortee_CheckedChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(906, 56);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(77, 13);
            this.label14.TabIndex = 174;
            this.label14.Text = "MAC Adresse :";
            // 
            // cboMACAdresse
            // 
            this.cboMACAdresse.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboMACAdresse.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboMACAdresse.DropDownWidth = 150;
            this.cboMACAdresse.FormattingEnabled = true;
            this.cboMACAdresse.Location = new System.Drawing.Point(909, 74);
            this.cboMACAdresse.Name = "cboMACAdresse";
            this.cboMACAdresse.Size = new System.Drawing.Size(110, 21);
            this.cboMACAdresse.TabIndex = 25;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(790, 56);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(107, 13);
            this.label13.TabIndex = 172;
            this.label13.Text = "Page par min.(PPM) :";
            // 
            // cboPPM
            // 
            this.cboPPM.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboPPM.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboPPM.DropDownWidth = 150;
            this.cboPPM.FormattingEnabled = true;
            this.cboPPM.Location = new System.Drawing.Point(792, 74);
            this.cboPPM.Name = "cboPPM";
            this.cboPPM.Size = new System.Drawing.Size(110, 21);
            this.cboPPM.TabIndex = 24;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(472, 56);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(80, 13);
            this.label12.TabIndex = 170;
            this.label12.Text = "Portée (Mètre) :";
            // 
            // cboPortee
            // 
            this.cboPortee.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboPortee.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboPortee.DropDownWidth = 150;
            this.cboPortee.FormattingEnabled = true;
            this.cboPortee.Location = new System.Drawing.Point(474, 74);
            this.cboPortee.Name = "cboPortee";
            this.cboPortee.Size = new System.Drawing.Size(96, 21);
            this.cboPortee.TabIndex = 21;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(691, 56);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(45, 13);
            this.label15.TabIndex = 163;
            this.label15.Text = "Netété :";
            // 
            // cboNetete
            // 
            this.cboNetete.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboNetete.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboNetete.DropDownWidth = 150;
            this.cboNetete.FormattingEnabled = true;
            this.cboNetete.Location = new System.Drawing.Point(694, 74);
            this.cboNetete.Name = "cboNetete";
            this.cboNetete.Size = new System.Drawing.Size(91, 21);
            this.cboNetete.TabIndex = 23;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(573, 57);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(86, 13);
            this.label16.TabIndex = 161;
            this.label16.Text = "Fréquence (Hz) :";
            // 
            // cboFrequence
            // 
            this.cboFrequence.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboFrequence.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboFrequence.DropDownWidth = 150;
            this.cboFrequence.FormattingEnabled = true;
            this.cboFrequence.Location = new System.Drawing.Point(576, 74);
            this.cboFrequence.Name = "cboFrequence";
            this.cboFrequence.Size = new System.Drawing.Size(112, 21);
            this.cboFrequence.TabIndex = 22;
            // 
            // cboCategorieMat
            // 
            this.cboCategorieMat.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboCategorieMat.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboCategorieMat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCategorieMat.DropDownWidth = 250;
            this.cboCategorieMat.FormattingEnabled = true;
            this.cboCategorieMat.Location = new System.Drawing.Point(9, 28);
            this.cboCategorieMat.Name = "cboCategorieMat";
            this.cboCategorieMat.Size = new System.Drawing.Size(110, 21);
            this.cboCategorieMat.TabIndex = 0;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 12);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(81, 13);
            this.label11.TabIndex = 159;
            this.label11.Text = "Catégorie mat, :";
            // 
            // chkArchiver
            // 
            this.chkArchiver.AutoSize = true;
            this.chkArchiver.ForeColor = System.Drawing.Color.MediumVioletRed;
            this.chkArchiver.Location = new System.Drawing.Point(401, 31);
            this.chkArchiver.Name = "chkArchiver";
            this.chkArchiver.Size = new System.Drawing.Size(62, 17);
            this.chkArchiver.TabIndex = 3;
            this.chkArchiver.TabStop = false;
            this.chkArchiver.Text = "Archivé";
            this.chkArchiver.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.DarkSlateGray;
            this.label10.Location = new System.Drawing.Point(1025, 7);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(1, 92);
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
            this.txtDateAcquisitionFin.TabIndex = 15;
            // 
            // txtDateAcquisitionDebut
            // 
            this.txtDateAcquisitionDebut.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtDateAcquisitionDebut.Location = new System.Drawing.Point(1073, 13);
            this.txtDateAcquisitionDebut.Name = "txtDateAcquisitionDebut";
            this.txtDateAcquisitionDebut.Size = new System.Drawing.Size(109, 20);
            this.txtDateAcquisitionDebut.TabIndex = 14;
            // 
            // rdLstDateAcquisition
            // 
            this.rdLstDateAcquisition.AutoSize = true;
            this.rdLstDateAcquisition.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.rdLstDateAcquisition.Location = new System.Drawing.Point(398, 55);
            this.rdLstDateAcquisition.Name = "rdLstDateAcquisition";
            this.rdLstDateAcquisition.Size = new System.Drawing.Size(65, 17);
            this.rdLstDateAcquisition.TabIndex = 8;
            this.rdLstDateAcquisition.TabStop = true;
            this.rdLstDateAcquisition.Text = "Par date";
            this.rdLstDateAcquisition.UseVisualStyleBackColor = true;
            this.rdLstDateAcquisition.CheckedChanged += new System.EventHandler(this.rdLstDateAcquisition_CheckedChanged);
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.DarkSlateGray;
            this.label8.Location = new System.Drawing.Point(467, 7);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(1, 91);
            this.label8.TabIndex = 150;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(472, 13);
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
            this.cboIdentifiant.Location = new System.Drawing.Point(474, 31);
            this.cboIdentifiant.Name = "cboIdentifiant";
            this.cboIdentifiant.Size = new System.Drawing.Size(96, 21);
            this.cboIdentifiant.TabIndex = 9;
            // 
            // rdLstIdentifiant
            // 
            this.rdLstIdentifiant.AutoSize = true;
            this.rdLstIdentifiant.ForeColor = System.Drawing.Color.DarkCyan;
            this.rdLstIdentifiant.Location = new System.Drawing.Point(6, 55);
            this.rdLstIdentifiant.Name = "rdLstIdentifiant";
            this.rdLstIdentifiant.Size = new System.Drawing.Size(90, 17);
            this.rdLstIdentifiant.TabIndex = 4;
            this.rdLstIdentifiant.TabStop = true;
            this.rdLstIdentifiant.Text = "Par Identifiant";
            this.rdLstIdentifiant.UseVisualStyleBackColor = true;
            this.rdLstIdentifiant.CheckedChanged += new System.EventHandler(this.rdLstIdentifiant_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(789, 13);
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
            this.cboMacWifi.Location = new System.Drawing.Point(791, 31);
            this.cboMacWifi.Name = "cboMacWifi";
            this.cboMacWifi.Size = new System.Drawing.Size(111, 21);
            this.cboMacWifi.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(906, 13);
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
            this.cboMacLAN.Location = new System.Drawing.Point(908, 31);
            this.cboMacLAN.Name = "cboMacLAN";
            this.cboMacLAN.Size = new System.Drawing.Size(111, 21);
            this.cboMacLAN.TabIndex = 13;
            // 
            // rdLstMAC
            // 
            this.rdLstMAC.AutoSize = true;
            this.rdLstMAC.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.rdLstMAC.Location = new System.Drawing.Point(288, 55);
            this.rdLstMAC.Name = "rdLstMAC";
            this.rdLstMAC.Size = new System.Drawing.Size(107, 17);
            this.rdLstMAC.TabIndex = 7;
            this.rdLstMAC.TabStop = true;
            this.rdLstMAC.Text = "Par adresse MAC";
            this.rdLstMAC.UseVisualStyleBackColor = true;
            this.rdLstMAC.CheckedChanged += new System.EventHandler(this.rdLstMAC_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(691, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Délais (années) :";
            // 
            // rdLstEndGarantie
            // 
            this.rdLstEndGarantie.AutoSize = true;
            this.rdLstEndGarantie.ForeColor = System.Drawing.Color.SeaGreen;
            this.rdLstEndGarantie.Location = new System.Drawing.Point(164, 55);
            this.rdLstEndGarantie.Name = "rdLstEndGarantie";
            this.rdLstEndGarantie.Size = new System.Drawing.Size(120, 17);
            this.rdLstEndGarantie.TabIndex = 6;
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
            this.cboDelais.Location = new System.Drawing.Point(694, 31);
            this.cboDelais.Name = "cboDelais";
            this.cboDelais.Size = new System.Drawing.Size(91, 21);
            this.cboDelais.TabIndex = 11;
            // 
            // rdLstEtat
            // 
            this.rdLstEtat.AutoSize = true;
            this.rdLstEtat.ForeColor = System.Drawing.Color.DarkOliveGreen;
            this.rdLstEtat.Location = new System.Drawing.Point(98, 55);
            this.rdLstEtat.Name = "rdLstEtat";
            this.rdLstEtat.Size = new System.Drawing.Size(63, 17);
            this.rdLstEtat.TabIndex = 5;
            this.rdLstEtat.TabStop = true;
            this.rdLstEtat.Text = "Par Etat";
            this.rdLstEtat.UseVisualStyleBackColor = true;
            this.rdLstEtat.CheckedChanged += new System.EventHandler(this.rdLstEtat_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(573, 14);
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
            this.cboItems.Location = new System.Drawing.Point(127, 28);
            this.cboItems.Name = "cboItems";
            this.cboItems.Size = new System.Drawing.Size(268, 21);
            this.cboItems.TabIndex = 1;
            // 
            // cboEtat
            // 
            this.cboEtat.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboEtat.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboEtat.DropDownWidth = 150;
            this.cboEtat.FormattingEnabled = true;
            this.cboEtat.Location = new System.Drawing.Point(575, 31);
            this.cboEtat.Name = "cboEtat";
            this.cboEtat.Size = new System.Drawing.Size(113, 21);
            this.cboEtat.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(124, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Selection item pour rapport :";
            // 
            // cmdView
            // 
            this.cmdView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdView.BackColor = System.Drawing.Color.SeaShell;
            this.cmdView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdView.ForeColor = System.Drawing.Color.DarkRed;
            this.cmdView.Location = new System.Drawing.Point(1204, 39);
            this.cmdView.Name = "cmdView";
            this.cmdView.Size = new System.Drawing.Size(74, 22);
            this.cmdView.TabIndex = 26;
            this.cmdView.Text = "Afficher";
            this.cmdView.UseVisualStyleBackColor = false;
            this.cmdView.Click += new System.EventHandler(this.cmdView_Click);
            // 
            // crvReport
            // 
            this.crvReport.ActiveViewIndex = -1;
            this.crvReport.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crvReport.Cursor = System.Windows.Forms.Cursors.Default;
            this.crvReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crvReport.Location = new System.Drawing.Point(0, 106);
            this.crvReport.Name = "crvReport";
            this.crvReport.Size = new System.Drawing.Size(1289, 449);
            this.crvReport.TabIndex = 1;
            // 
            // frmReportMateriel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1289, 555);
            this.Controls.Add(this.crvReport);
            this.Controls.Add(this.panel1);
            this.Name = "frmReportMateriel";
            this.Text = "Rapports pour matériel";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmReportMateriel_FormClosed);
            this.Load += new System.EventHandler(this.frmReportMateriel_Load);
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
        private System.Windows.Forms.ComboBox cboCategorieMat;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cboPortee;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox cboNetete;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ComboBox cboFrequence;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cboPPM;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox cboMACAdresse;
        private System.Windows.Forms.RadioButton rdLstPPM;
        private System.Windows.Forms.RadioButton rdLstNetete;
        private System.Windows.Forms.RadioButton rdLstFrequence;
        private System.Windows.Forms.RadioButton rdLstMAC_Adresse;
        private System.Windows.Forms.RadioButton rdLstPortee;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer crvReport;
    }
}