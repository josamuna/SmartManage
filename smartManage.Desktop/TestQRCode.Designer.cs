namespace smartManage.Desktop
{
    partial class TestQRCode
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestQRCode));
            this.cmd = new System.Windows.Forms.Button();
            this.txt = new System.Windows.Forms.TextBox();
            this.cmdConvert = new System.Windows.Forms.Button();
            this.cmdSave = new System.Windows.Forms.Button();
            this.cmdLoadPicture = new System.Windows.Forms.Button();
            this.cmdConvertToTxt = new System.Windows.Forms.Button();
            this.lstImg = new System.Windows.Forms.ListBox();
            this.cmdConvertByte = new System.Windows.Forms.Button();
            this.txtImgTxt = new System.Windows.Forms.RichTextBox();
            this.txtText = new System.Windows.Forms.RichTextBox();
            this.cmdDo = new System.Windows.Forms.Button();
            this.pbox1 = new System.Windows.Forms.PictureBox();
            this.pbox = new System.Windows.Forms.PictureBox();
            this.cmd1 = new System.Windows.Forms.Button();
            this.cboData = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbox)).BeginInit();
            this.SuspendLayout();
            // 
            // cmd
            // 
            this.cmd.Location = new System.Drawing.Point(142, 11);
            this.cmd.Name = "cmd";
            this.cmd.Size = new System.Drawing.Size(153, 23);
            this.cmd.TabIndex = 0;
            this.cmd.Text = "Create and Convert to Text";
            this.cmd.UseVisualStyleBackColor = true;
            this.cmd.Click += new System.EventHandler(this.cmd_Click);
            // 
            // txt
            // 
            this.txt.Location = new System.Drawing.Point(14, 14);
            this.txt.Name = "txt";
            this.txt.Size = new System.Drawing.Size(115, 20);
            this.txt.TabIndex = 2;
            this.txt.Text = "test";
            // 
            // cmdConvert
            // 
            this.cmdConvert.Location = new System.Drawing.Point(337, 14);
            this.cmdConvert.Name = "cmdConvert";
            this.cmdConvert.Size = new System.Drawing.Size(142, 23);
            this.cmdConvert.TabIndex = 3;
            this.cmdConvert.Text = "Convert Text to Image";
            this.cmdConvert.UseVisualStyleBackColor = true;
            this.cmdConvert.Click += new System.EventHandler(this.cmdConvert_Click);
            // 
            // cmdSave
            // 
            this.cmdSave.Location = new System.Drawing.Point(22, 213);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(65, 23);
            this.cmdSave.TabIndex = 5;
            this.cmdSave.Text = "Save";
            this.cmdSave.UseVisualStyleBackColor = true;
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // cmdLoadPicture
            // 
            this.cmdLoadPicture.Location = new System.Drawing.Point(22, 405);
            this.cmdLoadPicture.Name = "cmdLoadPicture";
            this.cmdLoadPicture.Size = new System.Drawing.Size(65, 23);
            this.cmdLoadPicture.TabIndex = 7;
            this.cmdLoadPicture.Text = "Load";
            this.cmdLoadPicture.UseVisualStyleBackColor = true;
            this.cmdLoadPicture.Click += new System.EventHandler(this.cmdLoadPicture_Click);
            // 
            // cmdConvertToTxt
            // 
            this.cmdConvertToTxt.Location = new System.Drawing.Point(232, 218);
            this.cmdConvertToTxt.Name = "cmdConvertToTxt";
            this.cmdConvertToTxt.Size = new System.Drawing.Size(101, 23);
            this.cmdConvertToTxt.TabIndex = 9;
            this.cmdConvertToTxt.Text = "Convert To Text";
            this.cmdConvertToTxt.UseVisualStyleBackColor = true;
            this.cmdConvertToTxt.Click += new System.EventHandler(this.cmdConvertToTxt_Click);
            // 
            // lstImg
            // 
            this.lstImg.FormattingEnabled = true;
            this.lstImg.Location = new System.Drawing.Point(232, 340);
            this.lstImg.Name = "lstImg";
            this.lstImg.Size = new System.Drawing.Size(247, 82);
            this.lstImg.TabIndex = 10;
            // 
            // cmdConvertByte
            // 
            this.cmdConvertByte.Location = new System.Drawing.Point(363, 218);
            this.cmdConvertByte.Name = "cmdConvertByte";
            this.cmdConvertByte.Size = new System.Drawing.Size(116, 23);
            this.cmdConvertByte.TabIndex = 11;
            this.cmdConvertByte.Text = "Convert To Byte";
            this.cmdConvertByte.UseVisualStyleBackColor = true;
            this.cmdConvertByte.Click += new System.EventHandler(this.cmdConvertByte_Click);
            // 
            // txtImgTxt
            // 
            this.txtImgTxt.Location = new System.Drawing.Point(233, 247);
            this.txtImgTxt.Name = "txtImgTxt";
            this.txtImgTxt.Size = new System.Drawing.Size(246, 87);
            this.txtImgTxt.TabIndex = 13;
            this.txtImgTxt.Text = "";
            // 
            // txtText
            // 
            this.txtText.Location = new System.Drawing.Point(232, 62);
            this.txtText.Name = "txtText";
            this.txtText.Size = new System.Drawing.Size(247, 145);
            this.txtText.TabIndex = 14;
            this.txtText.Text = "";
            // 
            // cmdDo
            // 
            this.cmdDo.Location = new System.Drawing.Point(132, 405);
            this.cmdDo.Name = "cmdDo";
            this.cmdDo.Size = new System.Drawing.Size(75, 23);
            this.cmdDo.TabIndex = 15;
            this.cmdDo.Text = "Read Bar Code";
            this.cmdDo.UseVisualStyleBackColor = true;
            this.cmdDo.Click += new System.EventHandler(this.cmdDo_Click);
            // 
            // pbox1
            // 
            this.pbox1.Image = ((System.Drawing.Image)(resources.GetObject("pbox1.Image")));
            this.pbox1.Location = new System.Drawing.Point(22, 247);
            this.pbox1.Name = "pbox1";
            this.pbox1.Size = new System.Drawing.Size(152, 152);
            this.pbox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbox1.TabIndex = 6;
            this.pbox1.TabStop = false;
            // 
            // pbox
            // 
            this.pbox.Image = ((System.Drawing.Image)(resources.GetObject("pbox.Image")));
            this.pbox.Location = new System.Drawing.Point(22, 55);
            this.pbox.Name = "pbox";
            this.pbox.Size = new System.Drawing.Size(152, 152);
            this.pbox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbox.TabIndex = 1;
            this.pbox.TabStop = false;
            // 
            // cmd1
            // 
            this.cmd1.Location = new System.Drawing.Point(180, 40);
            this.cmd1.Name = "cmd1";
            this.cmd1.Size = new System.Drawing.Size(75, 23);
            this.cmd1.TabIndex = 16;
            this.cmd1.Text = "button1";
            this.cmd1.UseVisualStyleBackColor = true;
            this.cmd1.Click += new System.EventHandler(this.cmd1_Click);
            // 
            // cboData
            // 
            this.cboData.FormattingEnabled = true;
            this.cboData.Location = new System.Drawing.Point(272, 40);
            this.cboData.Name = "cboData";
            this.cboData.Size = new System.Drawing.Size(121, 21);
            this.cboData.TabIndex = 17;
            // 
            // TestQRCode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(491, 436);
            this.Controls.Add(this.cboData);
            this.Controls.Add(this.cmd1);
            this.Controls.Add(this.cmdDo);
            this.Controls.Add(this.txtText);
            this.Controls.Add(this.txtImgTxt);
            this.Controls.Add(this.cmdConvertByte);
            this.Controls.Add(this.lstImg);
            this.Controls.Add(this.cmdConvertToTxt);
            this.Controls.Add(this.cmdLoadPicture);
            this.Controls.Add(this.pbox1);
            this.Controls.Add(this.cmdSave);
            this.Controls.Add(this.cmdConvert);
            this.Controls.Add(this.txt);
            this.Controls.Add(this.pbox);
            this.Controls.Add(this.cmd);
            this.Name = "TestQRCode";
            this.Text = "TestQRCode";
            ((System.ComponentModel.ISupportInitialize)(this.pbox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmd;
        private System.Windows.Forms.PictureBox pbox;
        private System.Windows.Forms.TextBox txt;
        private System.Windows.Forms.Button cmdConvert;
        private System.Windows.Forms.Button cmdSave;
        private System.Windows.Forms.PictureBox pbox1;
        private System.Windows.Forms.Button cmdLoadPicture;
        private System.Windows.Forms.Button cmdConvertToTxt;
        private System.Windows.Forms.ListBox lstImg;
        private System.Windows.Forms.Button cmdConvertByte;
        private System.Windows.Forms.RichTextBox txtImgTxt;
        private System.Windows.Forms.RichTextBox txtText;
        private System.Windows.Forms.Button cmdDo;
        private System.Windows.Forms.Button cmd1;
        private System.Windows.Forms.ComboBox cboData;
    }
}