using System;
using UnityEngine;
using YooAsset;

namespace QFramework
{
    /// <summary>
    /// 资源文件偏移解密类
    /// </summary>
    public class FileStreamWebDecryption : IWebDecryptionServices
    {
        public WebDecryptResult LoadAssetBundle(WebDecryptFileInfo fileInfo)
        {
            // 优化：使用Buffer批量操作替代逐字节异或
            byte[] decryptedData = new byte[fileInfo.FileData.Length];
            Buffer.BlockCopy(fileInfo.FileData, 0, decryptedData, 0, fileInfo.FileData.Length);

            for (int i = 0; i < decryptedData.Length; i++)
            {
                decryptedData[i] ^= BundleStream.KEY;
            }

            WebDecryptResult decryptResult = new WebDecryptResult();
            decryptResult.Result = AssetBundle.LoadFromMemory(decryptedData);
            return decryptResult;
        }
    }
}