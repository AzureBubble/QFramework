using System;
using System.Collections;
using System.Collections.Generic;
using GameLogic;
using QFramework;
using UnityEngine;

public class GameEventTest : MonoBehaviour
{
    private void OnEnable()
    {
        GameEvent.AddEventListener(ITestUI_Event.Test, Test);
    }

    private void Test()
    {
        Debug.Log("GameEvent.Test");
        Destroy(this);
    }

    private void OnDisable()
    {
        GameEvent.RemoveEventListener(ITestUI_Event.Test, Test);
    }

    private void OnDestroy()
    {
        Debug.Log("GameEvent.Test.OnDestroy");
    }
}