using QFramework;
using UnityEngine;

/// <summary>
/// 游戏启动器 进行初始化设置
/// </summary>
public class GameEntrance : MonoBehaviour
{
    private void Awake()
    {
        ModuleSystem.GetModule<IMonoDriver>();
        DontDestroyOnLoad(gameObject);
    }
}