using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class WebGLInputField : MonoBehaviour
{
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void ScrollWindow();
		        
		public void ScrollWindowToDefaultValue()
        {
            ScrollWindow();
        }
#else
    private void ScrollWindowToDefaultValue() { }
#endif

    [SerializeField] private TMPro.TMP_InputField _inputField;

    private TouchScreenKeyboard _keyboard;

    public void ShowKeyboard()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        _keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false, false, "nickname");
#endif
    }

    public void Update()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        if (_keyboard == null) return;

        if (_keyboard.status == TouchScreenKeyboard.Status.Visible)
        {
            _inputField.text = _keyboard.text;
        }
        else if (_keyboard.status == TouchScreenKeyboard.Status.Canceled || _keyboard.status == TouchScreenKeyboard.Status.Done)
        {
            ScrollWindowToDefaultValue();
            _keyboard = null;
        }
#endif
    }   
}
