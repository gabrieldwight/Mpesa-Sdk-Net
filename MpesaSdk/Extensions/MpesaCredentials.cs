﻿using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MpesaSdk.Extensions
{
    /// <summary>
    /// Encrypt password for business to customer transaction.
    /// </summary>
    /// <remarks>
    /// M-Pesa authenticates a transaction by decrypting the security credentials. 
    /// Security credentials are generated by encrypting the base64 encoded initiator password with M-Pesa’s public key, 
    /// a X509 certificate.
    /// The algorithm for generating security credentials is as follows:
    /// 1. Write the unencrypted password into a byte array.
    /// 2. Encrypt the array with the M-Pesa public key certificate.Use the RSA algorithm,
    ///    and use PKCS #1.5 padding (not OAEP), and add the result to the encrypted stream.
    /// 3. Convert the resulting encrypted byte array into a string using base64 encoding.
    /// The resulting base64 encoded string is the security credential.
    /// </remarks>
    public static class MpesaCredentials
    {
        /// <summary>
        /// Encrypts Mpesa API Security Credential password
        /// </summary>
        /// <param name="certificateFilePath">The file path of the certificate stored.</param>
        /// <param name="password"></param>       
        /// <returns>
        /// Encrypted password
        /// </returns>
        public static string EncryptPassword(string certificateFilePath, string password)
        {
            var certificateDataResult = ReadCertificateFile(certificateFilePath);

            X509Certificate2 x509Certificate2 = new X509Certificate2(certificateDataResult);

            using RSA rsa = x509Certificate2.GetRSAPublicKey();

            var data = rsa.Encrypt(Encoding.UTF8.GetBytes(password), RSAEncryptionPadding.Pkcs1);

            return Convert.ToBase64String(data);
        }

        /// <summary>
        /// Reads and returns certificate data in bytes
        /// </summary>
        /// <param name="certificateFileName">The file name of the certifcate: test.cer or prod.cer</param>
        /// <returns>
        /// Certificate data in bytes
        /// </returns>
        private static byte[] ReadCertificateFile(string certificateFileName)
        {
            return File.ReadAllBytes(certificateFileName);
        }
    }
}
