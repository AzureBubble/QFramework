using System;
using UnityEngine;
using YooAsset;

namespace QFramework
{
    /// <summary>
    /// 资源文件偏移加密类
    /// </summary>
    public class FileOffsetWebDecryption : IWebDecryptionServices
    {
        public WebDecryptResult LoadAssetBundle(WebDecryptFileInfo fileInfo)
        {
            int offset = GetFileOffset();
            byte[] decryptedData = new byte[fileInfo.FileData.Length - offset];
            Buffer.BlockCopy(fileInfo.FileData, offset, decryptedData, 0, decryptedData.Length);
            // 从内存中加载AssetBundle
            WebDecryptResult decryptResult = new WebDecryptResult();
            decryptResult.Result = AssetBundle.LoadFromMemory(decryptedData);
            return decryptResult;
        }

        private static int GetFileOffset()
        {
            return 32;
        }
    }
}