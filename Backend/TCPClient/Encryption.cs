using System.Security.Cryptography;
using System.Text;

namespace TCPClient;

public class Encryption
{
    private ECDiffieHellman _ecdh;
    private byte[] _aesKey;

    public Encryption()
    {
        // Initialize ECDH with secp256r1 curve
        _ecdh = ECDiffieHellman.Create(ECCurve.NamedCurves.nistP256);
    }
    
    // Get the public key in raw format (64 bytes)
    public byte[] GetPublicKey()
    {
        ECParameters ecParams = _ecdh.ExportParameters(false);
        byte[] publicKey = new byte[64];
        Buffer.BlockCopy(ecParams.Q.X, 0, publicKey, 0, 32);
        Buffer.BlockCopy(ecParams.Q.Y, 0, publicKey, 32, 32);
        return publicKey;
    }

    // Get the public key to share with the other party
    public byte[] GetPublicKeyX509()
    {
        return _ecdh.PublicKey.ExportSubjectPublicKeyInfo();
    }
    public bool IsAesKeyNull()
    {
        return _aesKey == null;
    }
    // Convert public key to Base64 string
    public string GetPublicKeyString()
    {
        return Encoding.ASCII.GetString(GetPublicKey());
    }

    // Convert Base64 string back to public key byte array
    public static byte[] PublicKeyFromString(string publicKeyString)
    {
        return Encoding.ASCII.GetBytes(publicKeyString);
    }

    // Generate shared secret and derive AES key
        public void GenerateAesKey(byte[] otherPartyPublicKey)
        {
            ECParameters ecParams = new ECParameters
            {
                Curve = ECCurve.NamedCurves.nistP256,
                Q = new ECPoint
                {
                    X = new byte[32],
                    Y = new byte[32]
                }
            };
            Buffer.BlockCopy(otherPartyPublicKey, 0, ecParams.Q.X, 0, 32);
            Buffer.BlockCopy(otherPartyPublicKey, 32, ecParams.Q.Y, 0, 32);
    
            using (ECDiffieHellman ecdhTemp = ECDiffieHellman.Create(ecParams))
            {
                byte[] sharedSecret = _ecdh.DeriveKeyMaterial(ecdhTemp.PublicKey);
    
                // Hash the shared secret with MD5
                using (SHA1 sha1 = SHA1.Create())
                {
                    byte[] hashedSecret = sha1.ComputeHash(sharedSecret);
                    _aesKey = new byte[16];
                    Buffer.BlockCopy(hashedSecret, 0, _aesKey, 0, _aesKey.Length);
                }
            }
        }

    // Encrypt a message using AES-128-CBC
    public byte[] Encrypt(string plainText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = _aesKey;
            aes.Mode = CipherMode.CBC;
            aes.GenerateIV();

            using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    byte[] plainTextBytes = Encoding.ASCII.GetBytes(plainText);
                    cs.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cs.FlushFinalBlock();

                    byte[] cipherText = ms.ToArray();
                    byte[] result = new byte[aes.IV.Length + cipherText.Length];
                    Buffer.BlockCopy(aes.IV, 0, result, 0, aes.IV.Length);
                    Buffer.BlockCopy(cipherText, 0, result, aes.IV.Length, cipherText.Length);

                    return result;
                }
            }
        }
    }

    // Decrypt a message using AES-128-CBC
    public string Decrypt(byte[] cipherTextWithIv)
    {
        using (Aes aes = Aes.Create())
        {
            byte[] iv = new byte[16];
            byte[] cipherText = new byte[cipherTextWithIv.Length - 16];

            Buffer.BlockCopy(cipherTextWithIv, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(cipherTextWithIv, iv.Length, cipherText, 0, cipherText.Length);

            aes.Key = _aesKey;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;

            using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
            using (MemoryStream ms = new MemoryStream(cipherText))
            using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            {
                byte[] plainTextBytes = new byte[cipherText.Length];
                int decryptedByteCount = cs.Read(plainTextBytes, 0, plainTextBytes.Length);
                return Encoding.ASCII.GetString(plainTextBytes, 0, decryptedByteCount);
            }
        }
    }    
}