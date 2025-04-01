using System;
using UnityEngine.Windows;
using YooAsset;

namespace QFramework
{
    /// <summary>
    /// 文件偏移加密方式
    /// </summary>
    public class FileOffsetEncryption : IEncryptionServices
    {
        public EncryptResult Encrypt(EncryptFileInfo fileInfo)
        {
            var offset = (int)FileOffsetDecryption.GetFileOffset();
            var fileBytes = File.ReadAllBytes(fileInfo.FileLoadPath);
            var encryptedData = new byte[fileBytes.Length + offset];
            Buffer.BlockCopy(fileBytes, 0, encryptedData, offset, fileBytes.Length);
            EncryptResult result = new EncryptResult
            {
                Encrypted = true,
                EncryptedData = encryptedData
            };
            return result;
        }
    }
}