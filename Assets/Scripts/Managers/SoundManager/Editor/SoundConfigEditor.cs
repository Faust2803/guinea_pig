using Managers.SoundManager.Data;
using UnityEditor;
using UnityEngine;

namespace Managers.SoundManager.Editor
{
    [CustomEditor(typeof(SoundConfig))]
    public class SoundConfigEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            GUILayout.Space(10f);
            
            if (GUILayout.Button("Generate Keys"))
            {
                if(target is SoundConfig config)
                    config.GenerateKeys(); 
            }
        }
    }
}