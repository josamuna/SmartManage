using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

namespace smartManage.Tools
{
    public class clsTools
    {
        private static clsTools _instance;

        public static int etat_modification_user = -1;//Variable permettant de prendre le statut pour modification du user (User seul, Mot passe seul ou les deux)
        public static string oldUser = "";
        public static string newUser = "";
        public static string oldPassword = "";
        public static string newPassword = "";
        public static bool activationUser = false;
        public static int nombre_droit = 0;//Variable permettant de récupérer le nombre total des droits de l'utilisateur

        public static System.Collections.Generic.List<string> valueUser = new System.Collections.Generic.List<string>();//Liste qui contient les droits de l'utilisateur connecté

        public static clsTools Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new clsTools();

                return _instance;
            }
        }

        private clsTools()
        {
        }

        #region Good Convertion string and Image
        public string ImageToString64_(string path)
        {
            string strValu = "";
            Image image = null;

            try
            {
                image = Image.FromFile(path);

                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();
                    strValu = Convert.ToBase64String(imageBytes);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (image != null)
                    image.Dispose();
            }
            return strValu;
        }

        public string ImageToString64(string path)
        {
            string strValu = "";
            FileStream fs = null;

            try
            {
                fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                long size = (new FileInfo(path)).Length;

                byte[] data = new byte[size];

                fs.Read(data, 0, (int)size);

                using (MemoryStream ms = new MemoryStream())
                {
                    ms.Write(data, 0, (int)size);
                    byte[] imageBytes = ms.ToArray();
                    strValu = Convert.ToBase64String(imageBytes);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }
            return strValu;
        }

        public string ImageToString64(Image image)
        {
            string strValu = "";
            MemoryStream ms = null;

            try
            {
                ms = new MemoryStream();

                Image newImage = ResizeImage(new Bitmap(Image.FromStream(ms)), 150, 150);
                //image.Save(m, image.RawFormat);
                //byte[] imageBytes = m.ToArray();
                //byte[] imageBytes = CreateThumbnail(m.ToArray(), 50);
                byte[] imageBytes = ms.ToArray();
                strValu = Convert.ToBase64String(imageBytes);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (ms != null)
                    ms.Dispose();
            }
            return strValu;
        }

        public string ImageToString64_(Image image)
        {
            string strValu = "";
            MemoryStream ms = null;

            try
            {
                ms = new MemoryStream();
                image.Save(ms, image.RawFormat);
                byte[] imageBytes = ms.ToArray();
                strValu = Convert.ToBase64String(imageBytes);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (ms != null)
                    ms.Dispose();
            }
            return strValu;
        }

        public Image LoadImage(string strImage)
        {
            if (strImage.Trim().Equals("")) return null;
            byte[] bytes = Convert.FromBase64String(strImage);

            Image image = null;
            try
            {
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    ////image = Image.FromStream(ms);
                    //image = new Bitmap(Image.FromStream(ms), 120, 120);
                    image = ResizeImage(new Bitmap(Image.FromStream(ms)), 150, 150);
                }
            }
            finally
            {
                if (image != null)
                    image.Dispose();
            }
            return image;
        }
        #endregion
        public byte[] CreateThumbnail(byte[] PassedImage, int LargestSide)
        {
            byte[] returnedThumbnail = null;
            MemoryStream startMemoryStream = null;
            MemoryStream newMemoryStream = null;

            try
            {
                startMemoryStream = new MemoryStream();
                newMemoryStream = new MemoryStream();

                // write the string to the stream  
                startMemoryStream.Write(PassedImage, 0, PassedImage.Length);

                // create the start Bitmap from the MemoryStream that contains the image  
                Bitmap startBitmap = new Bitmap(startMemoryStream);

                // set thumbnail height and width proportional to the original image.  
                int newHeight;
                int newWidth;
                double HW_ratio;
                if (startBitmap.Height > startBitmap.Width)
                {
                    newHeight = LargestSide;
                    HW_ratio = LargestSide / (double)startBitmap.Height;
                    newWidth = (int)(HW_ratio * startBitmap.Width);
                }
                else
                {
                    newWidth = LargestSide;
                    HW_ratio = (double)((double)LargestSide / (double)startBitmap.Width);
                    newHeight = (int)(HW_ratio * (double)startBitmap.Height);
                }

                // create a new Bitmap with dimensions for the thumbnail.  
                Bitmap newBitmap = new Bitmap(newWidth, newHeight);

                // Copy the image from the START Bitmap into the NEW Bitmap.  
                // This will create a thumnail size of the same image.  
                newBitmap = ResizeImage(startBitmap, newWidth, newHeight);

                // Save this image to the specified stream in the specified format.  
                newBitmap.Save(newMemoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);

                // Fill the byte[] for the thumbnail from the new MemoryStream.

                returnedThumbnail = newMemoryStream.ToArray();
                //newBitmap.Dispose();
                //startBitmap.Dispose();
                //StartMemoryStream.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (startMemoryStream != null)
                    startMemoryStream.Dispose();
                if (newMemoryStream != null)
                    newMemoryStream.Dispose();
            }

            // return the resized image as a string of bytes.  
            return returnedThumbnail;
        }
        // Resize a Bitmap 
        private Bitmap ResizeImage(Bitmap image, int width, int height)
        {
            Bitmap resizedImage = new Bitmap(width, height);
            using (Graphics gfx = Graphics.FromImage(resizedImage))
            {
                gfx.DrawImage(image, new Rectangle(0, 0, width, height),
                    new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
            }
            return resizedImage;
        }

        //load image from bytes
        public Bitmap GetBitmapFromByte(byte[] byteArray)
        {
            Bitmap image;
            try
            {
                using (MemoryStream stream = new MemoryStream(byteArray))
                {
                    image = new Bitmap(stream);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return image;
        }
        public string GetFileFromByte(byte[] byteArray)
        {
            string fpath = System.IO.Path.GetTempPath() + DateTime.Today.ToString("yyyyMMdd") + ".jpg";

            using (System.IO.FileStream f = new System.IO.FileStream(fpath, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                f.Write(byteArray, 0, byteArray.Length);
            }
            return fpath;
        }

        public Image GetImageFromFile(string file)
        {
            Image image;
            using (FileStream fs = new FileStream(file, FileMode.Open))
            {
                image = Image.FromStream(fs);
            }
            return image;
        }
        public byte[] PictureBoxImageToBytes(System.Drawing.Image imageIn)
        {
            //**bonne fonction pour convertir imag en byte sans utiliser la methode Save()
            byte[] imageBytes = null;
            using (MemoryStream ms = new MemoryStream())
            {
                Image newImage = ResizeImage(new Bitmap(Image.FromStream(ms)), 150, 150);
                imageBytes = ms.ToArray();
            }
            return imageBytes;
        }

        #region Good Convert for type Byte[] and Image
        public byte[] GetByteFromFile(string path)
        {
            byte[] b;
            using (System.IO.FileStream f = System.IO.File.OpenRead(path))
            {
                int size = Convert.ToInt32(f.Length);
                b = new byte[size];
                f.Read(b, 0, size);
            }
            return b;
        }

        public byte[] GetBytesFromImage(Image img)
        {
            byte[] imageBytes = null;
            using (MemoryStream ms = new MemoryStream())
            {
                Bitmap bm = new Bitmap(Image.FromStream(ms));
                imageBytes = ms.ToArray();
            }
            return imageBytes;
        }

        public Image GetImageFromByte(byte[] byteArrayIn)
        {
            MemoryStream ms = null;

            try
            {
                ms = new MemoryStream(byteArrayIn);
                //Image returnImage = Image.FromStream(ms);
                Bitmap bmp = new Bitmap(ms);
                return bmp;
            }
            finally
            {
                if (ms != null)
                    ms.Dispose();
            }
        }
        #endregion

        public string SaveTempImage(System.Windows.Forms.PictureBox pbox)
        {
            string filename = Environment.GetEnvironmentVariables()["TEMP"].ToString() + @"\" + "fTmp" + DateTime.Now.Millisecond.ToString() + ".png";

            using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite))
            {
                System.Drawing.Imaging.ImageFormat imageFormat = System.Drawing.Imaging.ImageFormat.Png;
                pbox.Image.Save(fs, imageFormat);
            }

            return filename;
        }

        public void RemoveTempImage(string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            else
                throw new Exception("Le fichier spécifier n'existe pas !!!");
        }

        public bool LimiteImageSize(string fileName, long byteSize, int heightSize, int widthSize)
        {
            var fileSizeInBytes = new FileInfo(fileName).Length;
            if (fileSizeInBytes > byteSize)
                throw new CustomException(string.Format("L'image est plus large que la taille requise de {0}Mb", byteSize / 1000000));

            using (var img = new Bitmap(fileName))
            {
                if (img.Width > widthSize || img.Height > heightSize)
                    throw new CustomException(string.Format("Les dimension de l'image excèdent celles requises : {0}x{1}", heightSize, widthSize));
            }

            return true;
        }

        public bool LimiteImageSize(string fileName, long byteSize)
        {
            var fileSizeInBytes = new FileInfo(fileName).Length;
            if (fileSizeInBytes > byteSize)
                throw new CustomException(string.Format("L'image est plus large que la taille requise de {0}Mb", byteSize / 1000000));

            return true;
        }
    }
}