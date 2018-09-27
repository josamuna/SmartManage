﻿using System;
using System.Drawing;
using System.IO;

namespace smartManage.Tools
{
    public class clsTools
    {
        public static int etat_modification_user = -1;//Variable permettant de prendre le statut pour modification du user (User seul, Mot passe seul ou les deux)
        public static string oldUser = "";
        public static string newUser = "";
        public static string oldPassword = "";
        public static string newPassword = "";
        public static bool activationUser = false;
        public static int nombre_droit = 0;//Variable permettant de récupérer le nombre total des droits de l'utilisateur

        public static System.Collections.Generic.List<string> valueUser = new System.Collections.Generic.List<string>();//Liste qui contient les droits de l'utilisateur connecté

        public clsTools()
        {
        }

        public string ImageToString64(string path)
        {
            string strValu = "";
            try
            {
                using (Image image = Image.FromFile(path))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        image.Save(m, image.RawFormat);
                        byte[] imageBytes = m.ToArray();
                        strValu = Convert.ToBase64String(imageBytes);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return strValu;
        }

        public string ImageToString64(Image image)
        {
            string strValu = "";
            try
            {
                //using (Image image = Image.FromFile(path))
                //{
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();
                    strValu = Convert.ToBase64String(imageBytes);
                }
                //}
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return strValu;
        }
        public byte[] CreateThumbnail(byte[] PassedImage, int LargestSide)
        {
            byte[] ReturnedThumbnail;
            try
            {

                using (MemoryStream StartMemoryStream = new MemoryStream(),
                                    NewMemoryStream = new MemoryStream())
                {
                    // write the string to the stream  
                    StartMemoryStream.Write(PassedImage, 0, PassedImage.Length);

                    // create the start Bitmap from the MemoryStream that contains the image  
                    Bitmap startBitmap = new Bitmap(StartMemoryStream);

                    // set thumbnail height and width proportional to the original image.  
                    int newHeight;
                    int newWidth;
                    double HW_ratio;
                    if (startBitmap.Height > startBitmap.Width)
                    {
                        newHeight = LargestSide;
                        HW_ratio = (double)((double)LargestSide / (double)startBitmap.Height);
                        newWidth = (int)(HW_ratio * (double)startBitmap.Width);
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
                    newBitmap.Save(NewMemoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);

                    // Fill the byte[] for the thumbnail from the new MemoryStream.

                    ReturnedThumbnail = NewMemoryStream.ToArray();
                    newBitmap.Dispose();
                    startBitmap.Dispose();
                    StartMemoryStream.Close();
                    StartMemoryStream.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            //finally
            //{
            //    ReturnedThumbnail = null;
            //}

            // return the resized image as a string of bytes.  
            return ReturnedThumbnail;
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

        public Image LoadImage(string strImage)
        {
            if (strImage.Trim().Equals("")) return null;
            byte[] bytes = Convert.FromBase64String(strImage);

            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
            }

            return image;
        }
        //load image from bytes
        public Bitmap GetImageFromByte(byte[] byteArray)
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
        public byte[] GetFileToByte(string file)
        {
            byte[] b;
            using (System.IO.FileStream f = System.IO.File.OpenRead(file))
            {
                int size = Convert.ToInt32(f.Length);
                b = new byte[size];
                f.Read(b, 0, size);
            }
            return b;
        }
        public Image GetImageFromByte(string file)
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
            //**bonne fonction pour convertir imag en byte Avec ou sans les deux lignes en commentaire
            MemoryStream ms = new MemoryStream();
            ////Bitmap bitm = new Bitmap(imageIn);
            ////bitm.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            imageIn.Save(ms, imageIn.RawFormat);
            byte[] buff = ms.ToArray();
            ms.Close();
            ms.Dispose();
            return buff;
        }

        public static byte[] GetBytesFromImage(Image img)
        {
            MemoryStream ms = new MemoryStream();
            img.Save(ms, img.RawFormat);
            return ms.ToArray();
        }

        public Image ByteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            //Image returnImage = Image.FromStream(ms);
            Bitmap bmp = new Bitmap(ms);
            ms.Close();
            return bmp;
        }

        public string SaveTempImage(System.Windows.Forms.PictureBox pbox)
        {
            string filename = Environment.GetEnvironmentVariables()["TEMP"].ToString() + @"\" + "fTmp" + DateTime.Now.Millisecond.ToString() + ".png";

            using (System.IO.FileStream fs = new System.IO.FileStream(filename, FileMode.Create, FileAccess.ReadWrite))
            {
                System.Drawing.Imaging.ImageFormat imageFormat = System.Drawing.Imaging.ImageFormat.Png;
                pbox.Image.Save(fs, imageFormat);
                fs.Close();
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
    }
}