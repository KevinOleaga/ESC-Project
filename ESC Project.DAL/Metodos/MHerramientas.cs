﻿using ESC_Project.DAL.Interfaces;
using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace ESC_Project.DAL.Metodos
{
    public class MHerramientas : IHerramientas
    {
        #region Encryption

        // SecretKey (16 Characters)
        public string secretKey = "DrNbEsuT8y9YT2k2";

        // Encrypt()
        public string Encrypt(string data)
        {
            byte[] keyArray;
            byte[] encrypt = Encoding.UTF8.GetBytes(data);

            keyArray = Encoding.UTF8.GetBytes(secretKey);

            var tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] res = cTransform.TransformFinalBlock(encrypt, 0, encrypt.Length);
            tdes.Clear();

            return Convert.ToBase64String(res, 0, res.Length);
        }

        // Decrypt()
        public string Decrypt(string data)
        {
            byte[] keyArray;
            byte[] decrypt = Convert.FromBase64String(data);

            keyArray = Encoding.UTF8.GetBytes(secretKey);

            var tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] res = cTransform.TransformFinalBlock(decrypt, 0, decrypt.Length);
            tdes.Clear();

            return Encoding.UTF8.GetString(res);
        }

        #endregion

        // Capitalize()
        public string Capitalize(string text)
        {
            string res = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text.ToLower());
            return res;
        }
    }
}
