using ManageUtilities;
using System;
using System.Drawing;

namespace ManageQRCode
{
    public class QRCodeImage
    {
        const string DirectoryUtilLog = "Log";

        public QRCodeImage()
        {

        }

        public static Image GetGenerateQRCode(string txtValue, string level, string iconPath, int iconeSize)
        {
            Image img = null;

            try
            {
                QRCodeGenerator.ECCLevel eccLevel = (QRCodeGenerator.ECCLevel)(level == "L" ? 0 : level == "M" ? 1 : level == "Q" ? 2 : 3);

                using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                {
                    using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(txtValue, eccLevel))
                    {
                        using (QRCode qrCode = new QRCode(qrCodeData))
                        {
                            img = qrCode.GetGraphic(20, Color.Black, Color.White, GetIconBitmap(iconPath), iconeSize);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec de génération QR Code : " + ex.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(ex.Message);
            }

            return img;
        }

        private static Bitmap GetIconBitmap(string iconPath)
        {
            Bitmap img = null;
            if (iconPath.Length > 0)
            {
                try
                {
                    img = new Bitmap(iconPath);
                }
                catch (Exception)
                {
                }
            }
            return img;
        }

        private static Bitmap GetIconBitmap(Bitmap bitmap)
        {
            return bitmap;
        }
    }
}
