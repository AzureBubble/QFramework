using System.Collections;
using System.Collections.Generic;
using GameLogic;
using QFramework;
using UnityEngine;

public class GameTimerTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Time.timeScale = 1;
        // GameModule.GameTimerMgr.CreatOnceGameTimer(1, (args) =>
        // {
        //     Debug.Log("CreatOnceGameTimer");
        // });

        // var time = GameModule.GameTimerMgr.CreatOnceGameTimer(6, (args) =>
        // {
        //     Debug.Log("CreatOnceGameTimer Des");
        // });


        // GameTimer time = null;
        // GameModule.GameTimerMgr.CreatOnceGameTimer(5, (args) =>
        // {
        //     GameModule.GameTimerMgr.DestroyGameTimer(time);
        //
        //     Debug.Log("dES CreatLoopGameTimer");
        //
        // });
        // time = GameModule.GameTimerMgr.CreatLoopGameTimer(1, (args) =>
        // {
        //     Debug.Log("CreatLoopGameTimer");
        // });




        // GameModule.GameTimerMgr.CreatUnscaleOnceGameTimer(1, (args) =>
        // {
        //     Debug.Log("CreatUnscaleOnceGameTimer");
        // });
        // var time1 = GameModule.GameTimerMgr.CreatUnscaleLoopGameTimer(1, (args) =>
        // {
        //     Debug.Log("CreatUnscaleLoopGameTimer");
        // });
        //
        // GameModule.GameTimerMgr.CreatUnscaleOnceGameTimer(5, (args) =>
        // {
        //     GameModule.GameTimerMgr.DestroyGameTimer(time1);
        //
        //     Debug.Log("dES CreatUnscaleOnceGameTimer");
        //
        // });
        //
        // GameModule.GameTimerMgr.CreateSystemTimer((o,a) =>
        // {
        //     // GameModule.GameTimerMgr.DestroyGameTimer(time1);
        //
        //     Debug.Log("CreateSystemTimer");
        //
        // });
        //
        // Test test1 = MemoryPoolMgr.Get<Test>();
        // Test test2 =MemoryPoolMgr.Get<Test>();
        // Test test3 =MemoryPoolMgr.Get<Test>();
        // Test test4 =MemoryPoolMgr.Get<Test>();
        // Test test5 =MemoryPoolMgr.Get<Test>();
        // MemoryPoolMgr.Release(test5);
        // MemoryPoolMgr.Release(test2);
        // MemoryPoolMgr.Release(test3);
        // Test test6 =MemoryPoolMgr.Get<Test>();

        GameEventLauncher.Init();

        GameEvent.Get<ITestUI>().Test();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class Test : IMemory
{
    public void OnRelease()
    {
        Debugger.Debug("Test OnRelease");
    }
}