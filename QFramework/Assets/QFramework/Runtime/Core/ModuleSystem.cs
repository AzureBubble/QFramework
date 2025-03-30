using System;
using System.Collections.Generic;
using QFramework.Utility;

namespace QFramework
{
    /// <summary>
    /// 游戏框架模块实现类管理系统。
    /// </summary>
    public static class ModuleSystem
    {
        /// <summary>
        /// 默认设计的模块数量。
        /// <remarks>有增删可以自行修改减少内存分配与GCAlloc。</remarks>
        /// </summary>
        internal const int DEFAULT_MODULE_COUNT = 16;

        private static readonly Dictionary<Type, Module> m_moduleMaps = new Dictionary<Type, Module>(DEFAULT_MODULE_COUNT);
        private static readonly LinkedList<Module> m_moduleLinkList = new LinkedList<Module>();
        private static readonly LinkedList<Module> m_updateModuleLinkList = new LinkedList<Module>();
        private static readonly List<IUpdateModule> m_updateModuleExecuteList = new List<IUpdateModule>(DEFAULT_MODULE_COUNT);

        private static bool m_isExecuteListDirty;

        #region OnUpdate

        public static void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            if (m_isExecuteListDirty)
            {
                m_isExecuteListDirty = false;
                BuildExecuteList();
            }

            int executeCount = m_updateModuleExecuteList.Count;
            for (int i = 0; i < executeCount; i++)
            {
                m_updateModuleExecuteList[i].OnUpdate(elapseSeconds, realElapseSeconds);
            }
        }

        /// <summary>
        /// 构造执行队列。
        /// </summary>
        private static void BuildExecuteList()
        {
            m_updateModuleExecuteList.Clear();
            foreach (var updateModule in m_updateModuleLinkList)
            {
                m_updateModuleExecuteList.Add(updateModule as IUpdateModule);
            }
        }

        #endregion

        #region Dispose

        public static void OnDispose()
        {
            for (LinkedListNode<Module> current = m_moduleLinkList.Last; current != null; current = current.Previous)
            {
                current.Value.OnDispose();
            }

            m_moduleLinkList.Clear();
            m_moduleMaps.Clear();
            m_updateModuleLinkList.Clear();
            m_updateModuleExecuteList.Clear();
            // TODO:
            // MemoryPool.ClearAll();
            // Utility.Marshal.FreeCachedHGlobal();
        }

        #endregion

        #region CreateModule

        /// <summary>
        /// 创建游戏框架模块。
        /// </summary>
        /// <param name="moduleType">要创建的游戏框架模块类型。</param>
        /// <returns>要创建的游戏框架模块。</returns>
        public static Module CreateModule(Type moduleType)
        {
            Module module = Activator.CreateInstance(moduleType) as Module;

            if (module == null)
            {
                throw new Exception(StringFormatHelper.Format("Can not create module '{0}'.", moduleType.FullName));
            }

            m_moduleMaps[moduleType] = module;

            RegisterUpdateModule(module);
            return module;
        }

        #endregion

        #region GetModule

        /// <summary>
        /// 获取游戏框架模块。
        /// </summary>
        /// <typeparam name="T">要获取的游戏框架模块类型。</typeparam>
        /// <returns>要获取的游戏框架模块。</returns>
        /// <remarks>如果要获取的游戏框架模块不存在，则自动创建该游戏框架模块。</remarks>
        public static T GetModule<T>() where T : class
        {
            Type interfaceType = typeof(T);
            if (!interfaceType.IsInterface)
            {
                throw new QFrameworkException(StringFormatHelper.Format("You must get module by interface, but '{0}' is not.", interfaceType.FullName));
            }

            if (m_moduleMaps.TryGetValue(interfaceType, out Module module))
            {
                return module as T;
            }

            string moduleName = StringFormatHelper.Format("{0}.{1}", interfaceType.Namespace, interfaceType.Name.Substring(1));
            Type moduleType = Type.GetType(moduleName);
            if (moduleType == null)
            {
                throw new QFrameworkException(StringFormatHelper.Format("Can not find Game Framework module type '{0}'.", moduleName));
            }

            return GetModule(moduleType) as T;
        }

        /// <summary>
        /// 获取游戏框架模块。
        /// </summary>
        /// <param name="moduleType">要获取的游戏框架模块类型。</param>
        /// <returns>要获取的游戏框架模块。</returns>
        /// <remarks>如果要获取的游戏框架模块不存在，则自动创建该游戏框架模块。</remarks>
        private static Module GetModule(Type moduleType)
        {
            return m_moduleMaps.TryGetValue(moduleType, out Module module) ? module : CreateModule(moduleType);
        }

        #endregion

        #region RegisterModule

        /// <summary>
        /// 注册自定义Module。
        /// </summary>
        /// <param name="module">Module。</param>
        /// <returns>Module实例。</returns>
        /// <exception cref="QFrameworkException">框架异常。</exception>
        public static T RegisterModule<T>(Module module) where T : class
        {
            Type interfaceType = typeof(T);
            if (!interfaceType.IsInterface)
            {
                throw new QFrameworkException(StringFormatHelper.Format("You must get module by interface, but '{0}' is not.", interfaceType.FullName));
            }

            m_moduleMaps[interfaceType] = module;

            RegisterUpdateModule(module);

            return module as T;
        }

        private static void RegisterUpdateModule(Module module)
        {
            LinkedListNode<Module> current = m_moduleLinkList.First;

            while (current != null)
            {
                if (module.Priority > current.Value.Priority)
                {
                    break;
                }
                current = current.Next;
            }

            if (current != null)
            {
                m_moduleLinkList.AddBefore(current, module);
            }
            else
            {
                m_moduleLinkList.AddLast(module);
            }

            Type interfaceType = typeof(IUpdateModule);
            bool implementsInterface = interfaceType.IsAssignableFrom(module.GetType());

            if (implementsInterface)
            {
                LinkedListNode<Module> currentUpdate = m_updateModuleLinkList.First;
                while (currentUpdate != null)
                {
                    if (module.Priority > currentUpdate.Value.Priority)
                    {
                        break;
                    }
                    currentUpdate = currentUpdate.Next;
                }
                if (currentUpdate != null)
                {
                    m_updateModuleLinkList.AddBefore(currentUpdate, module);
                }
                else
                {
                    m_updateModuleLinkList.AddLast(module);
                }

                m_isExecuteListDirty = true;
            }

            module.OnInit();
        }

        #endregion
    }
}