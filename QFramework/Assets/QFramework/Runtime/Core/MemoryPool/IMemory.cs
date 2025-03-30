using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    public interface IMemory
    {
        /// <summary>
        /// 释放对象并返回对象池
        /// </summary>
        void OnRelease();
    }
}