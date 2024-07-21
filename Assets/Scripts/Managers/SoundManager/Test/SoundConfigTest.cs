using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Managers.SoundManager.Test
{
    [CreateAssetMenu(menuName = "Configs/Sound/SoundConfigTest", fileName = "SoundConfigTest")]
    public class SoundConfigTest : ScriptableObject
    {
        [SerializeField] private List<SoundDataTest> _sounds;
        [SerializeField] private string _path = "Assets/Scripts/Managers/SoundManager/Test";
        [SerializeField] private string _name = "SoundIdTest";
        [SerializeField] private AssetReference _rer;

        public void Testtt()
        {
            var t = SoundIdTest.ElectricitySparks;
        }

#if UNITY_EDITOR
        [ContextMenu("Generate")]
        public void Generate()
        {
            if (!File.Exists(_path + _name + ".cs"))
            {
                Debug.LogError($"File not found.");
                return;
            }
            
            using (FileStream fileStream = new FileStream(_path + _name + ".cs", FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
            {
                fileStream.SetLength(0);
            }

            using (StreamWriter writer = new StreamWriter(_path + _name + ".cs"))
            {
                writer.WriteLine("public enum " + _name);
                writer.WriteLine("{");

                foreach (var sound in _sounds)
                {
                    var clipName = sound.Clip.name;
                    sound.Name = clipName;
                    clipName = clipName.Replace(" ", "");
                    writer.WriteLine("    " + clipName + ",");
                }
                
                writer.WriteLine("}");
            }
            
            for (int i = 0; i < _sounds.Count; i++)
                _sounds[i]._testId = (SoundIdTest) i;
            
            AssetDatabase.Refresh();

        }
#endif
    }
}