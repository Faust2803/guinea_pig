using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Managers.SoundManager.Base;
using Managers.SoundManager.Enums;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace Managers.SoundManager.Data
{
    [CreateAssetMenu(menuName = "Configs/Sound/SoundConfig", fileName = "SoundConfig")]
    public class SoundConfig : ScriptableObject, ISoundDataProvider, IInitializable
    {
        [SerializeField] private List<SoundData> _soundData = new List<SoundData>();
        
        [Header("For generation enum SoundId")]
        [SerializeField] private string _path = "Assets/Scripts/Managers/SoundManager/Enums/";
        [SerializeField] private string _namespace = string.Empty;
        private const string ENUM_NAME = "SoundId";
        
        private Dictionary<SoundId, ISoundData> _soundMapById = new Dictionary<SoundId, ISoundData>();
        private Dictionary<string, ISoundData> _soundMapByKey = new Dictionary<string, ISoundData>();
        
        public void Initialize()
        {
            _soundMapById = _soundData.ToDictionary(key => key.IdInfo.Id, value => (ISoundData) value);
            _soundMapByKey = _soundData.ToDictionary(key => key.IdInfo.Key, value => (ISoundData) value);
        }
        
        public bool TryGetData(SoundId id, out ISoundData data)
        {
            return TryGetData(id, _soundMapById, out data);
        }

        public bool TryGetData(string id, out ISoundData data)
        {
            if (_soundMapByKey.Count == 0)
                Initialize();

            return TryGetData(id, _soundMapByKey, out data);
        }

        private bool TryGetData<T>(T id, IDictionary<T, ISoundData> map, out ISoundData data)
        {
            data = null;
            
            if (map.TryGetValue(id, out data)) return true;
            
            Debug.LogError($"[{nameof(SoundConfig)}] Sound data for sound id: {id} is not contains in the map;");
            return false;
        }

#if UNITY_EDITOR
        public void GenerateKeys()
        {
            if (!File.Exists(_path + ENUM_NAME + ".cs"))
            {
                Debug.LogError($"File not found!");
                return;
            }

            try
            {
                using (FileStream fileStream = new FileStream(_path + ENUM_NAME + ".cs", FileMode.Open, FileAccess.Write, FileShare.Read))
                {
                    fileStream.SetLength(0);
                }

                using (StreamWriter writer = new StreamWriter(_path + ENUM_NAME + ".cs"))
                {
                    if (!string.IsNullOrEmpty(_namespace))
                    {
                        writer.WriteLine("namespace " + _namespace);
                        writer.WriteLine("{");
                    }

                    writer.WriteLine("public enum " + ENUM_NAME);
                    writer.WriteLine("{");

                    foreach (var data in _soundData)
                    {
                        var key = GenerateFromLabel(data.EditorLabel);
                        writer.WriteLine("    " + key + ",");
                    }
                
                    writer.WriteLine("}");
                    
                    if (!string.IsNullOrEmpty(_namespace))
                        writer.WriteLine("}");
                }
            
                for (int i = 0; i < _soundData.Count; i++)
                    _soundData[i].SetKeyInfo(new SoundKeyPairId(GenerateFromLabel(_soundData[i].EditorLabel), (SoundId) i));
            
                AssetDatabase.Refresh();
                
                EditorUtility.SetDirty(this);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }

        private string GenerateFromLabel (string editorLabel)
        {
            var key = editorLabel.Replace(" ", "");
            key = key.Replace("-", "_");
            key = key.Replace("(", "");
            key = key.Replace(")", "");
            key = key.Replace(".", "_");
            return key;
        }
#endif
    }
}