using ManageQRCode;
using smartManage.Tools;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Text;

namespace smartManage.Desktop
{
    public partial class TestQRCode : Form
    {
        private string path = null;
        public TestQRCode()
        {
            InitializeComponent();
        }

        private void cmd_Click(object sender, EventArgs e)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                //Creation de l'image QRCode
                Image img = QRCodeImage.GetGenerateQRCode(txt.Text, "L", "", 0);//L, M ou Q

                pbox.Image = img;

                //Conversion de l'image se trouvant dans le PictureBox pour le convertir en Base64

                string fileName = clsTools.Instance.SaveTempImage(pbox);
                //Chaine string
                txtText.Text = clsTools.Instance.ImageToString64(clsTools.Instance.GetImageFromByte(fileName));

                //Tableau Bytes
                //lstImg.DataSource = outils.PictureBoxImageToBytes(outils.getImageFromByte(fileName));

                clsTools.Instance.RemoveTempImage(fileName);

                sw.Stop();
                MessageBox.Show("Complete in " + (sw.ElapsedMilliseconds / 1000) + "Sec");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la création et conversion + " + ex.Message, "Création et Conversion QRCode en texte", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void cmdConvert_Click(object sender, EventArgs e)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                pbox.Image = null;
                pbox.Image = clsTools.Instance.LoadImage(txtText.Text);

                sw.Stop();
                MessageBox.Show("Complete in " + (sw.ElapsedMilliseconds / 1000) + "Sec");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la conversion + " + ex.Message, "Conversion image en texte", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sv = new SaveFileDialog();
            sv.Filter = "PNG File|*.png";

            sv.ShowDialog();
            if (!string.IsNullOrEmpty(sv.FileName))
            {
                using (System.IO.FileStream fs = (System.IO.FileStream) sv.OpenFile())
                {
                    System.Drawing.Imaging.ImageFormat imageFormat = System.Drawing.Imaging.ImageFormat.Png;
                    pbox.Image.Save(fs, imageFormat);
                    fs.Close();

                    MessageBox.Show("Saved to " + sv.FileName);
                }
            }

        }

        private void cmdConvertToTxt_Click(object sender, EventArgs e)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                txtImgTxt.Text = clsTools.Instance.ImageToString64(pbox1.Image);

                sw.Stop();
                MessageBox.Show("Complete in " + (sw.ElapsedMilliseconds / 1000) + "Sec");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la conversion + " + ex.Message, "Conversion image en texte", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void cmdLoadPicture_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "PNG File|*.png|JPG File|*.jpg|TIFF File|*.tif|BitMap Image|*.bmp";
            open.ShowDialog();

            if (System.IO.File.Exists(open.FileName))
            {
                switch (open.FilterIndex)
                {
                    case 0:
                        pbox1.Load(open.FileName);
                        break;
                    case 1:
                        pbox1.Load(open.FileName);
                        break;
                    case 2:
                        pbox1.Load(open.FileName);
                        break;
                    case 3:
                        pbox1.Load(open.FileName);
                        break;
                    default:
                        throw new Exception("Format non reconnu !!!");
                }

                path = open.FileName;
            }
        }

        private void cmdConvertByte_Click(object sender, EventArgs e)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                byte[] tbByte = clsTools.Instance.PictureBoxImageToBytes(pbox1.Image);
                //////byte[] tbByte = outils.PictureBoxImageToBytes(Image.FromFile(txtChemin.Text));
                lstImg.DataSource = tbByte;

                sw.Stop();
                MessageBox.Show("Complete in " + (sw.ElapsedMilliseconds / 1000) + "Sec");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la conversion + " + ex.Message, "Conversion image en texte", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void cmdDo_Click(object sender, EventArgs e)
        {
            try
            {
                //string[] tb = ReadBarCodFromFile(path);
                //StringBuilder sb = new StringBuilder();
                //foreach (string str in tb)
                //    sb.Append(str);
                //MessageBox.Show(sb.ToString());
            }catch(Exception ex)
            {
                MessageBox.Show("Erreur de conversion du QRCode + " + ex.Message, "Conversion QRCode", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
