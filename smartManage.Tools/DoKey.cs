using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace smartManage.Tools
{
    public class DoKey
    {     
        public DoKey() 
        {
        }

        private static int getSaltSize(byte[] keyByte)
        {
            var key = new Rfc2898DeriveBytes(keyByte, keyByte, 1000);

            byte[] bt = key.GetBytes(2);
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < bt.Length; i++)
            {
                sb.Append(Convert.ToInt32(bt[i]).ToString());
            }

            int saltSize = 0;
            string str = sb.ToString();

            foreach (char car in str)
            {
                int i = Convert.ToInt32(car.ToString());
                saltSize = saltSize + i;
            }

            return saltSize;
        }

        private static byte[] getRandomBytes(int taille)
        {
            byte[] btReturn = new byte[taille];
            RNGCryptoServiceProvider.Create().GetBytes(btReturn);
            return btReturn;
        }

        //Chiffrement proprement dit avec AES
        private static byte[] AES_Encrypt(byte[] bytesToEncrypt, byte[] keyBytes)
        {
            byte[] encryptedBytes = null;

            byte[] saltBytes = keyBytes;

            //On do le chiffrement AES
            MemoryStream ms = null; 

            try
            {
                ms = new MemoryStream();

                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    //Taille de la cle AES
                    AES.KeySize = 256;

                    //Taille du bloc AES pour le chiffrement
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(keyBytes, saltBytes, 1000);

                    //On considere une taille de 32 caracteres en AES256 et un bloc de 16 caracteres
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    //Mode AES
                    AES.Mode = CipherMode.CBC;

                    //Ecriture dans le MemoryStream
                    CryptoStream cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write);
                    cs.Write(bytesToEncrypt, 0, bytesToEncrypt.Length);
                    encryptedBytes = ms.ToArray();
                }
            }
            finally
            {
                if (ms != null)
                    ms.Dispose();
            }

            return encryptedBytes;
        }

        //Dechiffrement proprement dit avec AES
        private static byte[] AES_Decrypt(byte[] bytesToDecrypte, byte[] keyBytes)
        {
            byte[] decryptedBytes = null;

            byte[] saltBytes = keyBytes;

            //On do le dechiffrement AES
            MemoryStream ms = null;

            try
            {
                ms = new MemoryStream();

                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    //Taille de la cle AES
                    AES.KeySize = 256;

                    //Taille du bloc AES pour le chiffrement
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(keyBytes, saltBytes, 1000);

                    //On considere une taille de 32 caracteres en AES256 et un bloc de 16 caracteres
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);
                    
                    //Mode AES
                    AES.Mode = CipherMode.CBC;

                    //Recuperation du MemoryStream 
                    CryptoStream cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write);
                    cs.Write(bytesToDecrypte, 0, bytesToDecrypte.Length);
                    decryptedBytes = ms.ToArray();
                }
            }
            finally
            {
                if (ms != null)
                    ms.Dispose();
            }

            return decryptedBytes;
        }

        /// <summary>
        /// Methode permettant d'effectuer le chiffrement utilisation AES
        /// </summary>
        /// <param name="chaine">string</param>
        /// <param name="key">string</param>
        /// <param name="vector">string</param>
        /// <returns></returns>
        public static string Crypte(string chaine, string key)
        {
            if (string.IsNullOrEmpty(key)) throw new Exception("Veuillez spécifier une clé valide svp !!!");
            else if (string.IsNullOrEmpty(chaine)) throw new Exception("Veuillez spécifier les valeurs à crypter svp !!!");

            //Tableau des bytes contenant le resultat crypte
            byte[] crypteByte = null;

            //On place le texte a chiffrer dans un tableau d'octets
            byte[] crypteText = Encoding.UTF8.GetBytes(chaine);

            //On place la cle de chiffrement dans un tableau d'octet
            //Et on le hashe avec SHA256
            byte[] crypteKey = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(key));

            //Recuperation du salt size
            int saltSize = getSaltSize(crypteKey);

            //On place le salt dans un tableau des bytes
            byte[] salt = getRandomBytes(saltSize);

            // Ajout des bytes du salt au bytes originaux
            byte[] bytesToBeEncrypted = new byte[salt.Length + crypteText.Length];
            for (int i = 0; i < salt.Length; i++)
            {
                bytesToBeEncrypted[i] = salt[i];
            }
            for (int i = 0; i < crypteText.Length; i++)
            {
                bytesToBeEncrypted[i + salt.Length] = crypteText[i];
            }

            crypteByte = AES_Encrypt(bytesToBeEncrypted, crypteKey);

            return Convert.ToBase64String(crypteByte);
        }

        /// <summary>
        /// Methode permettant d'effectuer le déchiffrement utilisation AES
        /// </summary>
        /// <param name="chaine">string</param>
        /// <param name="key">string</param>
        /// <returns>string</returns>
        public static string Decrypte(string chaine, string key)
        {
            if (string.IsNullOrEmpty(key)) throw new Exception("Veuillez spécifier une clé valide svp !!!");
            else if (string.IsNullOrEmpty(chaine)) throw new Exception("Veuillez spécifier les valeurs à crypter svp !!!");

            //On place le texte a dechiffrer dans un tableau d'octets
            byte[] crypteText = Convert.FromBase64String(chaine);

            //On place la cle de chiffrement dans un tableau d'octet
            //Et on le hashe avec SHA256
            byte[] crypteKey = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(key));

            byte[] decryptedBytes = AES_Decrypt(crypteText, crypteKey);

            //Recuperation de la taille du salt
            int saltSize = getSaltSize(crypteKey);

            //Suppression des bytes ajoutes pour retrouver la taille des bytes originaux
            byte[] originalBytes = new byte[decryptedBytes.Length - saltSize];

            for (int i = saltSize; i < decryptedBytes.Length; i++)
            {
                originalBytes[i - saltSize] = decryptedBytes[i];
            }

            return Encoding.UTF8.GetString(originalBytes);
        }

        /// <summary>
        /// Methode permettant d'effectuer le chiffrement utilisant AES pour une liste d'items
        /// </summary>
        /// <param name="chaine">string</param>
        /// <param name="key">sting</param>
        /// <param name="vector">string</param>
        /// <returns>Liste des string</returns>
        public static List<string> Crypte(List<string> chaine, string key)
        {
            if (string.IsNullOrEmpty(key)) throw new Exception("Veuillez spécifier une clé valide svp !!!");
            else if (chaine.Count == 0) throw new Exception("Veuillez spécifier les valeurs à crypter svp !!!");

            List<string> lstStr = new List<string>();

            //Tableau des bytes contenant le resultat crypte
            byte[] crypteByte = null;

            foreach (string str in chaine)
            {
                //On place le texte a chiffrer dans un tableau d'octets
                byte[] crypteText = Encoding.UTF8.GetBytes(str);

                //On place la cle de chiffrement dans un tableau d'octet
                //Et on le hashe avec SHA256
                byte[] crypteKey = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(key));

                //Recuperation du salt size
                int saltSize = getSaltSize(crypteKey);

                //On place le salt dans un tableau des bytes
                byte[] salt = getRandomBytes(saltSize);

                // Ajout des bytes du salt au bytes originaux
                byte[] bytesToBeEncrypted = new byte[salt.Length + crypteText.Length];
                for (int i = 0; i < salt.Length; i++)
                {
                    bytesToBeEncrypted[i] = salt[i];
                }
                for (int i = 0; i < crypteText.Length; i++)
                {
                    bytesToBeEncrypted[i + salt.Length] = crypteText[i];
                }

                crypteByte = AES_Encrypt(bytesToBeEncrypted, crypteKey);

                lstStr.Add(Convert.ToBase64String(crypteByte));
            }

            return lstStr;
        }

        /// <summary>
        /// Methode permettant d'effectuer le déchiffrement utilisant AES pour une liste d'items
        /// </summary>
        /// <param name="chaine">string</param>
        /// <param name="key">string</param>
        /// <returns>Liste des string</returns>
        public static List<string> Decrypte(List<string> chaine, string key)
        {
            if (string.IsNullOrEmpty(key)) throw new Exception("Veuillez spécifier une clé valide svp !!!");
            else if (chaine.Count == 0) throw new Exception("Veuillez spécifier les valeurs à crypter svp !!!");

            List<string> lstStr = new List<string>();

            foreach (string str in chaine)
            {
                //On place le texte a dechiffrer dans un tableau d'octets
                byte[] crypteText = Convert.FromBase64String(str);

                //On place la cle de chiffrement dans un tableau d'octet
                //Et on le hashe avec SHA256
                byte[] crypteKey = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(key));

                byte[] decryptedBytes = AES_Decrypt(crypteText, crypteKey);

                //Recuperation de la taille du salt
                int saltSize = getSaltSize(crypteKey);

                //Suppression des bytes ajoutes pour retrouver la taille des bytes originaux
                byte[] originalBytes = new byte[decryptedBytes.Length - saltSize];

                for (int i = saltSize; i < decryptedBytes.Length; i++)
                {
                    originalBytes[i - saltSize] = decryptedBytes[i];
                }

                lstStr.Add(Convert.ToBase64String(originalBytes));
            }

            return lstStr;
        }
    }
}
