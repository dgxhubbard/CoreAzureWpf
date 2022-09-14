using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CoreBase.Utilities
{
    public static class CryptoUtility
    {
        #region Constants

        private const string Salt = "Kosher";
        private const string HashAlgorithm = "SHA1";
        private const int PasswordIterations = 2;
        private const string InitialVector = "OFRna73m*aze01xY";
        private const int KeySize = 256;

        private const string Password = "";


        #endregion

        #region Constructors

        static CryptoUtility ()
        {
            InitialVectorBytes = Encoding.ASCII.GetBytes( InitialVector );
            SaltValueBytes = Encoding.ASCII.GetBytes( Salt );            

        }

        #endregion

        #region Properties

        private static byte [] InitialVectorBytes
        { get; set; }


        private static byte [] SaltValueBytes
        { get; set; }

        #endregion

        #region Methods

        public static string Encrypt ( string plainText )
        {
            SymmetricAlgorithm provider = null;

            if ( string.IsNullOrEmpty(plainText) )                   
                return string.Empty;              

            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);              
            var derivedPassword = new PasswordDeriveBytes(Password, SaltValueBytes, HashAlgorithm, PasswordIterations);              
            var keyBytes = derivedPassword.GetBytes(KeySize / 8);

            provider = Aes.Create ();
            provider.Mode = CipherMode.CBC;
            provider.Padding = PaddingMode.PKCS7;

            byte [] cipherTextBytes = null;

            using ( var encryptor = provider.CreateEncryptor( keyBytes, InitialVectorBytes ) )             
            {                  
                using ( var memStream = new MemoryStream())                  
                {                    
                    using ( var cryptoStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write))                      
                    {                          
                        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);                          
                        cryptoStream.FlushFinalBlock();                     

                        cipherTextBytes = memStream.ToArray();                          
                        memStream.Close();                        
                        cryptoStream.Close();                      
                    }                  
                }              
            }

            provider.Clear();              
            
            return Convert.ToBase64String(cipherTextBytes);  
        }  
        
        
        public static string Decrypt ( string cipherText )
        {
            SymmetricAlgorithm provider = null;

            if ( string.IsNullOrEmpty( cipherText ) )                   
                return "";            

            var cipherTextBytes = Convert.FromBase64String(cipherText);
            var plainTextBytes = new byte [ cipherTextBytes.Length ];
            var derivedPassword = new PasswordDeriveBytes( Password, SaltValueBytes, HashAlgorithm, PasswordIterations );             
            var keyBytes = derivedPassword.GetBytes(KeySize / 8);

            provider = Aes.Create ();
            provider.Mode = CipherMode.CBC;
            provider.Padding = PaddingMode.PKCS7;

            int byteCount = 0;

            using ( var decryptor = provider.CreateDecryptor( keyBytes, InitialVectorBytes ) )              
            {                
                using ( var memStream = new MemoryStream(cipherTextBytes))                
                {
                    var output = new MemoryStream ();
                    using ( var cryptStream = new CryptoStream ( memStream, decryptor, CryptoStreamMode.Read ) )
                    {
                        var buffer = new byte[1024];
                        var read = cryptStream.Read ( buffer, 0, buffer.Length );
                        while ( read > 0 )
                        {
                            output.Write ( buffer, 0, read );
                            read = cryptStream.Read ( buffer, 0, buffer.Length );
                        }
                        cryptStream.Flush ();
                        plainTextBytes = output.ToArray ();
                        byteCount = plainTextBytes.Length;
                    }
                }             
            }

            provider.Clear();      
       
            return Encoding.UTF8.GetString(plainTextBytes, 0, byteCount);
        }

        #endregion

    }
}
