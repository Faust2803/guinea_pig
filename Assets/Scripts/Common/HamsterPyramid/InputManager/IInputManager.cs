using System;
using UnityEngine;

namespace Common.HamsterPyramid.InputManager
{
    public interface IInputManager
    {
        event Action<Vector3> OnSingleTouchBegin;
        event Action<Vector3> OnSingleTouchHold;
        event Action<Vector3> OnSingleTouchEnded;
        bool IsPointerOverUiObject();
    }
}