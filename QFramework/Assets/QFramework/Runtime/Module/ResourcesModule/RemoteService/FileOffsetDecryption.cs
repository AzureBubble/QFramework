﻿using System;
using UnityEngine;
using YooAsset;

namespace QFramework
{
    /// <summary>
    /// 资源文件偏移加载解密类
    /// </summary>
    public class FileOffsetDecryption : IDecryptionServices
    {
        /// <summary>
        /// 同步方式获取解密的资源包对象
        /// 注意：加载流对象在资源包对象释放的时候会自动释放
        /// </summary>
        public DecryptResult LoadAssetBundle(DecryptFileInfo fileInfo)
        {
            DecryptResult decryptResult = new DecryptResult
            {
                ManagedStream = null,
                Result = AssetBundle.LoadFromFile(fileInfo.FileLoadPath, 0, GetFileOffset())
            };
            return decryptResult;
        }

        /// <summary>
        /// 异步方式获取解密的资源包对象
        /// 注意：加载流对象在资源包对象释放的时候会自动释放
        /// </summary>
        public DecryptResult LoadAssetBundleAsync(DecryptFileInfo fileInfo)
        {
            DecryptResult decryptResult = new DecryptResult
            {
                ManagedStream = null,
                CreateRequest = AssetBundle.LoadFromFileAsync(fileInfo.FileLoadPath, 0, GetFileOffset())
            };
            return decryptResult;
        }

        public byte[] ReadFileData(DecryptFileInfo fileInfo)
        {
            throw new System.NotImplementedException();
        }

        public string ReadFileText(DecryptFileInfo fileInfo)
        {
            throw new System.NotImplementedException();
        }

        public static ulong GetFileOffset()
        {
            return 32;
        }
    }
}