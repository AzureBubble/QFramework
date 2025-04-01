using UnityEngine.Windows;
using YooAsset;

namespace QFramework
{
    /// <summary>
    /// 文件流加密方式
    /// </summary>
    public class FileStreamEncryption : IEncryptionServices
    {
        public EncryptResult Encrypt(EncryptFileInfo fileInfo)
        {
            var fileBytes = File.ReadAllBytes(fileInfo.FileLoadPath);

            for (int i = 0; i < fileBytes.Length; i++)
            {
                fileBytes[i] ^= BundleStream.KEY;
            }
            EncryptResult result = new EncryptResult
            {
                Encrypted = true,
                EncryptedData = fileBytes
            };
            return result;
        }
    }
}