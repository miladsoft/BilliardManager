using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Text;

namespace Billiard.Common.GuardToolkit
{
    public class TextEncryptDecrypt
    {
        /// <summary>

        /// This security key should be very complex and Random for encrypting the text. This playing vital role in encrypting the text.

        /// </summary>

        private const string _securityKey = "MyComplexKey";

 

 

        /// <summary>

        /// This method is used to convert the plain text to Encrypted/Un-Readable Text format.

        /// </summary>

        /// <param name="PlainText">Plain Text to Encrypt before transferring over the network.</param>

        /// <returns>Cipher Text</returns>

        public static string EncryptPlainTextToCipherText(string PlainText)

        {

            //Getting the bytes of Input String.

            byte[] toEncryptedArray = UTF8Encoding.UTF8.GetBytes(PlainText);

 

            MD5CryptoServiceProvider objMD5CryptoService = new MD5CryptoServiceProvider();

 

            //Gettting the bytes from the Security Key and Passing it to compute the Corresponding Hash Value.

            byte[] securityKeyArray = objMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(_securityKey));

 

            //De-allocatinng the memory after doing the Job.

            objMD5CryptoService.Clear();

 

            var objTripleDESCryptoService = new TripleDESCryptoServiceProvider();

 

            //Assigning the Security key to the TripleDES Service Provider.

            objTripleDESCryptoService.Key = securityKeyArray;

 

            //Mode of the Crypto service is Electronic Code Book.

            objTripleDESCryptoService.Mode = CipherMode.ECB;

 

            //Padding Mode is PKCS7 if there is any extra byte is added.

            objTripleDESCryptoService.Padding = PaddingMode.PKCS7;

 

            var objCrytpoTransform = objTripleDESCryptoService.CreateEncryptor();

 

            //Transform the bytes array to resultArray

            byte[] resultArray = objCrytpoTransform.TransformFinalBlock(toEncryptedArray, 0, toEncryptedArray.Length);

 

            //Releasing the Memory Occupied by TripleDES Service Provider for Encryption.

            objTripleDESCryptoService.Clear();

 

            //Convert and return the encrypted data/byte into string format.

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);

        }

 

 

        /// <summary>

        /// This method is used to convert the Cipher/Encypted text to Plain Text.

        /// </summary>

        /// <param name="CipherText">Encrypted Text</param>

        /// <returns>Plain/Decrypted Text</returns>

        public static string DecryptCipherTextToPlainText(string CipherText)

        {

            byte[] toEncryptArray = Convert.FromBase64String(CipherText);

 

            MD5CryptoServiceProvider objMD5CryptoService = new MD5CryptoServiceProvider();

 

            //Gettting the bytes from the Security Key and Passing it to compute the Corresponding Hash Value.

            byte[] securityKeyArray = objMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(_securityKey));

 

            //De-allocatinng the memory after doing the Job.

            objMD5CryptoService.Clear();

 

            var objTripleDESCryptoService = new TripleDESCryptoServiceProvider();

 

            //Assigning the Security key to the TripleDES Service Provider.

            objTripleDESCryptoService.Key = securityKeyArray;

           

            //Mode of the Crypto service is Electronic Code Book.

            objTripleDESCryptoService.Mode = CipherMode.ECB;

 

            //Padding Mode is PKCS7 if there is any extra byte is added.

            objTripleDESCryptoService.Padding = PaddingMode.PKCS7;

 

            var objCrytpoTransform = objTripleDESCryptoService.CreateDecryptor();

 

            //Transform the bytes array to resultArray

            byte[] resultArray = objCrytpoTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

 

            //Releasing the Memory Occupied by TripleDES Service Provider for Decryption.          

            objTripleDESCryptoService.Clear();

 

            //Convert and return the decrypted data/byte into string format.

            return UTF8Encoding.UTF8.GetString(resultArray);

        }

    }
}