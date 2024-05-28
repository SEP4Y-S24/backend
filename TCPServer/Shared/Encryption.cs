using System.Security.Cryptography;
using System.Text;

namespace TCPServer;

public class Encryption
{
    private ECDiffieHellman _ecdh;
    private byte[] _aesKey;

    public Encryption()
    {
        // Initialize ECDH with secp256r1 curve
        _ecdh = ECDiffieHellman.Create(ECCurve.NamedCurves.nistP256);
    }

    // Get the public key to share with the other party
    public byte[] GetPublicKey()
    {
        return _ecdh.PublicKey.ExportSubjectPublicKeyInfo();
    }
    
    // Convert public key to Base64 string
    public string GetPublicKeyString()
    {
        return Convert.ToBase64String(GetPublicKey());
    }

    // Convert Base64 string back to public key byte array
    public static byte[] PublicKeyFromString(string publicKeyString)
    {
        return Convert.FromBase64String(publicKeyString);
    }

    // Generate shared secret and derive AES key
    public void GenerateAesKey(byte[] otherPartyPublicKey)
    {
        using (ECDiffieHellman ecdhTemp = ECDiffieHellman.Create(ECCurve.NamedCurves.nistP256))
        {
            ecdhTemp.ImportSubjectPublicKeyInfo(otherPartyPublicKey, out _);
            byte[] sharedSecret = _ecdh.DeriveKeyMaterial(ecdhTemp.PublicKey);
            _aesKey = new byte[16];
            Buffer.BlockCopy(sharedSecret, 0, _aesKey, 0, _aesKey.Length);
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