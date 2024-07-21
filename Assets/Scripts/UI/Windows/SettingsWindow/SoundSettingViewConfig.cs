using System;
using System.Collections.Generic;
using Managers.SoundManager.Enums;
using UnityEngine;

namespace UI.Windows.SettingsWindow
{
    [CreateAssetMenu(menuName = "Configs/Sound/SoundSettingViewConfig", fileName = "SoundSettingViewConfig")]
    public class SoundSettingViewConfig : ScriptableObject
    {
        [SerializeField] private List<SoundSettingViewData> _data = new List<SoundSettingViewData>();
        [SerializeField] private SoundSettingViewData _defaultData;
        public SoundSettingViewData GetViewData(SoundType soundType)
        {
            foreach (var data in _data)
            {
                if (data.SoundType == soundType)
                    return data;
            }
            
            Debug.LogError($"[{nameof(SoundSettingViewConfig)}] View data for sound setting with type: {soundType} is not contains in the config.");

            return _defaultData.Copy(soundType.ToString());
        }
    }
    
    [Serializable]
    public class SoundSettingViewData
    {
        public SoundType SoundType => _soundType;
        public string Name => _name;
        public Color ColorBodyFront => _colorBodyFront;
        public Color ColorBodyBack => _colorBodyBack;

        [SerializeField] private SoundType _soundType;
        [SerializeField] private string _name;
        [SerializeField] private Color _colorBodyFront;
        [SerializeField] private Color _colorBodyBack;

        public SoundSettingViewData Copy(string name)
        {
            var result = Copy();
            result._name = name;
            return result;
        }

        public SoundSettingViewData Copy()
        {
            var result = new SoundSettingViewData
            {
                _name = _name,
                _soundType = _soundType,
                _colorBodyBack = _colorBodyBack,
                _colorBodyFront = _colorBodyFront
            };

            return result;
        }
    }
}