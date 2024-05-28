using System.Security.Cryptography;

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
    public (byte[] cipherText, byte[] iv) Encrypt(string plainText)
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
                using (StreamWriter sw = new StreamWriter(cs))
                {
                    sw.Write(plainText);
                }

                return (ms.ToArray(), aes.IV);
            }
        }
    }

    // Decrypt a message using AES-128-CBC
    public string Decrypt(byte[] cipherText, byte[] iv)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = _aesKey;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;

            using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
            using (MemoryStream ms = new MemoryStream(cipherText))
            using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            using (StreamReader sr = new StreamReader(cs))
            {
                return sr.ReadToEnd();
            }
        }
    }
}