using Managers.SoundManager.Enums;
using UnityEngine;

namespace Game.Jumper
{
    [CreateAssetMenu(menuName = "Jumper/Layer Settings")]
    public class LayerSettings : ScriptableObject
    {
        public float Height = 400;
        public Color Color = Color.white;
        public int MaxStackBlocks = 5;
        public AnimationCurve MaxEmptyCellsOnOneLine = AnimationCurve.Linear(0, 4, 1, 1);
        public AnimationCurve Density = AnimationCurve.Linear(0, 0.35f, 1, 0.5f);
        [Tooltip("Build a continuous line with plaftorm, along which you can jump 100%?")]
        public bool FollowWinLine = true;
        public Vector2 PlatformOffsetRange = new Vector2(0.6f, 1.2f);

        public SpawnItem[] Obstacles;
        public PlatformsSettings Platforms;
        public bool ForceStablePath = true;
        public int SmogChildIdEnabled;
        public Vector2 UIPosCursor;

        public SoundsSettings[] Audio;

        public float GetRandomStepHeight() =>
            Random.Range(PlatformOffsetRange.x, PlatformOffsetRange.y);

        [System.Serializable]
        public class SpawnItem
        {
            public GameObject[] Prefabs;

            public AnimationCurve Rate = AnimationCurve.Linear(0, 0, 1, 1);
            public float MinYDifference = 2f;

            public bool UseUniqueXPos;
            public bool CanBeInvert;
            public bool ChangeSizeOnInvert = true;
            public bool DisblePlatfromGenWithObstacles;
            public float UniquePos;

            public bool NeedSpawn(float percentOfLayerComplition) =>
                Prefabs.Length > 0 &&
                Random.value < Rate.Evaluate(percentOfLayerComplition);

            public GameObject GetRandomItem =>
                Prefabs[Random.Range(0, Prefabs.Length)];

            public GameObject GetRandomItemWithIndex (out int position)
            {
                position = Random.Range(0, Prefabs.Length);
                return Prefabs[position];
            }
        }

        [System.Serializable]
        public class PlatformsSettings
        {
            public GameObject Default;
            public int MaxDefaultPlatformsCount = 100;
            public SpawnItem Breakable;
            public SpawnItem Booster;
        }

        [System.Serializable]
        public class SoundsSettings
        {
            public SoundId Sound;
            public AnimationCurve Volume = AnimationCurve.Linear(0, 0, 1, 1);
        }
    }
}