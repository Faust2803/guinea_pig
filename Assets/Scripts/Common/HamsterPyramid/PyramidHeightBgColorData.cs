using System;
using UnityEngine;

namespace Common.HamsterPyramid
{
    [CreateAssetMenu(menuName = "Configs/Lobby/PyramidBgColor", fileName = "PyramidBgColor")]
    public class PyramidHeightBgColorData : ScriptableObject
    {
        [SerializeField] private PercentageColorData[] _data;
        [SerializeField] private Color _defaultColor;

        public Color GetColorLerp(float percentage)
        {
            percentage = Mathf.Clamp01(percentage);

            for (int i = 0; i < _data.Length; i++)
            {
                var data = _data[i];

                if (data.Percentage < percentage) continue;

                if (i == 0)
                    return data.Color;

                var prevData = _data[i - 1];
                var t = Extensions.InverseLerp(prevData.Percentage, data.Percentage, percentage);
                Color.RGBToHSV(prevData.Color, out var firstH, out var firstS, out var firstV);
                Color.RGBToHSV(data.Color, out var secondH, out var secondS, out var secondV);
                var resultH = Mathf.Lerp(firstH, secondH, t);
                var resultS = Mathf.Lerp(firstS, secondS, t);
                var resultV = Mathf.Lerp(firstV, secondV, t);

                return Color.HSVToRGB(resultH, resultS, resultV, false);
            }

            return _defaultColor;
        }

        [Serializable]
        private class PercentageColorData
        {
            [Range(0f, 1f)] public float Percentage;
            public Color Color;
        }
    }
}