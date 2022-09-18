using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Text;

namespace Billiard.Common.GuardToolkit
{
public class FileEncryptDecrypt
{
    //call this function to remove the key from memory after use for security purposes
    [System.Runtime.InteropServices.DllImport("KERNEL32.DLL", EntryPoint="RtlZeroMemory")]
    public static extern bool ZeroMemory(IntPtr Destination, int Length);

    //function to generate 64bit key
    public static string GenerateKey(){
        //Create an instance of symetric algorythm. Key and IV is generated automatically
        DESCryptoServiceProvider desCrypto = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create();
        // Use the Automatically generated key for Encryption
        return ASCIIEncoding.ASCII.GetString(desCrypto.Key);
    }

    //encrypt file
    public static void EncryptFile(string sInputFilename, 
        string sOutputFilename, 
        string sKey){
            FileStream fsInput = new FileStream(sInputFilename,
                FileMode.Open, 
                FileAccess.Read);

            FileStream fsEncrypted = new FileStream(sOutputFilename,
                FileMode.Create,
                FileAccess.Write);

            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            ICryptoTransform desencrypt = DES.CreateEncryptor();
            CryptoStream cryptostream = new CryptoStream(fsEncrypted,
                desencrypt,
                CryptoStreamMode.Write);

            byte[] bytearrayinput = new byte[fsInput.Length];
            fsInput.Read(bytearrayinput, 0, bytearrayinput.Length);
            cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);
            cryptostream.Close();
            fsInput.Close();
            fsEncrypted.Close();
    }

    //decrypt file
    public static void DecryptFile(string sInputFilename,
        string sOutputFilename,
        string sKey){
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            //A 63 bit key and IV is required for this provider
            //Set secret key for DES algorythm
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            //Set initialization vector
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

            //Create file stream to read encrypted file
            FileStream fsread = new FileStream(sInputFilename,
                FileMode.Open,
                FileAccess.Read);

            FileStream fsdecrypted =  new FileStream(sOutputFilename, 
                FileMode.Create,
                FileAccess.Write);

            //Create a DES decryptor from the DES instance
            ICryptoTransform desdecrypt = DES.CreateDecryptor();
            //Create crypto stream set to read and do a DES decryption transform on incoming bytes
            CryptoStream cryptostreamDecr = new CryptoStream(fsdecrypted,
            desdecrypt,
            CryptoStreamMode.Write);
            //Decrypt process
            byte[] bytearrayinput = new byte[fsread.Length];
            fsread.Read(bytearrayinput, 0, bytearrayinput.Length);
            cryptostreamDecr.Write(bytearrayinput, 0, bytearrayinput.Length);
            cryptostreamDecr.Flush();            
            fsread.Close();
            fsdecrypted.Close();
    }
    
}
}