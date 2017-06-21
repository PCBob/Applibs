
using System;
using System.Security.Cryptography;
using System.Text;

namespace Applibs.CYPS
{
    public static class Hashing
    {
        public static string SHA1(string inputText) => SHA1(inputText, Encoding.UTF8);

        public static string SHA1(string inputText, Encoding encoding) => Encrpt(HashType.SHA1, inputText, encoding);

        public static bool VerifySHA1(string inputText, string hashText) => VerifySHA1(inputText, hashText, Encoding.UTF8);

        public static bool VerifySHA1(string inputText, string hashText, Encoding encoding) => VerifyHash(HashType.SHA1, inputText, hashText, encoding);

        public static string SHA256(string inputText) => SHA256(inputText, Encoding.UTF8);

        public static string SHA256(string inputText, Encoding encoding) => Encrpt(HashType.SHA256, inputText, encoding);

        public static bool VerifySHA256(string inputText, string hashText) => VerifySHA256(inputText, hashText, Encoding.UTF8);

        public static bool VerifySHA256(string inputText, string hashText, Encoding encoding) => VerifyHash(HashType.SHA256, inputText, hashText, encoding);

        public static string SHA384(string inputText) => SHA384(inputText, Encoding.UTF8);

        public static string SHA384(string inputText, Encoding encoding) => Encrpt(HashType.SHA384, inputText, encoding);

        public static bool VerifySHA384(string inputText, string hashText) => VerifySHA384(inputText, hashText, Encoding.UTF8);

        public static bool VerifySHA384(string inputText, string hashText, Encoding encoding) => VerifyHash(HashType.SHA384, inputText, hashText, encoding);

        public static string SHA512(string inputText) => SHA512(inputText, Encoding.UTF8);

        public static string SHA512(string inputText, Encoding encoding) => Encrpt(HashType.SHA512, inputText, encoding);

        public static bool VerifySHA512(string inputText, string hashText) => VerifySHA512(inputText, hashText, Encoding.UTF8);

        public static bool VerifySHA512(string inputText, string hashText, Encoding encoding) => VerifyHash(HashType.SHA512, inputText, hashText, encoding);

        public static string MD5(string inputText) => MD5(inputText, Encoding.UTF8);

        public static string MD5(string inputText, Encoding encoding) => Encrpt(HashType.MD5, inputText, encoding);

        public static bool VerifyMD5(string inputText, string hashText) => VerifyMD5(inputText, hashText, Encoding.UTF8);

        public static bool VerifyMD5(string inputText, string hashText, Encoding encoding) => VerifyHash(HashType.MD5, inputText, hashText, encoding);

        private static string Encrpt(HashType hashtype, string inputText, Encoding encoding)
        {
            if (inputText == null)
            {
                throw new ArgumentNullException(nameof(inputText));
            }
            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }
            var hashbytes = ComputeHash(hashtype, inputText, encoding);
            var resultBuilder = new StringBuilder();
            foreach (var b in hashbytes)
            {
                resultBuilder.Append(Convert.ToString(b, 16));
            }
            var result = resultBuilder.ToString();
            resultBuilder.Clear();

            return result;
        }

        private static bool VerifyHash(HashType hashtype, string inputText, string hashText, Encoding encoding)
        {
            if (inputText == null)
            {
                throw new ArgumentNullException(nameof(inputText));
            }
            if (hashText == null)
            {
                throw new ArgumentNullException(nameof(hashText));
            }
            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            return string.Equals(Encrpt(hashtype, inputText, encoding), hashText, StringComparison.CurrentCultureIgnoreCase);
        }

        private static byte[] ComputeHash(HashType hashtype, string inputText, Encoding encoding)
        {
            if (inputText == null)
            {
                throw new ArgumentNullException(nameof(inputText));
            }
            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }
            return ComputeHash(hashtype, encoding.GetBytes(inputText));
        }

        private static byte[] ComputeHash(HashType hashtype, byte[] inputbytes)
        {
            if (inputbytes == null)
            {
                throw new ArgumentNullException(nameof(inputbytes));
            }
            using (var hashAlgorithm = SelectHashAlgorithm(hashtype))
            {
                return hashAlgorithm.ComputeHash(inputbytes, 0, inputbytes.Length);
            }
        }

        private static HashAlgorithm SelectHashAlgorithm(HashType hashtype)
        {
            HashAlgorithm hashAlgorithm;
            switch (hashtype)
            {
                case HashType.SHA1:
                    hashAlgorithm = System.Security.Cryptography.SHA1.Create();
                    break;
                case HashType.SHA256:
                    hashAlgorithm = System.Security.Cryptography.SHA256.Create();
                    break;
                case HashType.SHA384:
                    hashAlgorithm = System.Security.Cryptography.SHA384.Create();
                    break;
                case HashType.SHA512:
                    hashAlgorithm = System.Security.Cryptography.SHA512.Create();
                    break;
                case HashType.MD5:
                    hashAlgorithm = System.Security.Cryptography.MD5.Create();
                    break;
                default:
                    throw new InvalidOperationException();
            }
            return hashAlgorithm;
        }

        private enum HashType
        {
            SHA1 = 1,
            SHA256 = 2,
            SHA384 = 4,
            SHA512 = 8,
            MD5 = 16,
        }
    }
}