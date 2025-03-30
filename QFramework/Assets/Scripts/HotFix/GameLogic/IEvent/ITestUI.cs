using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace GameLogic
{
    [EventInterface(EEventGroup.GroupUI)]
    public interface ITestUI
    {
        void Test();
        void Test1();
        void Test2();
    }
}