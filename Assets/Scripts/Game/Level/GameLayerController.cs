using System;
using System.Collections.Generic;
using DataModels.Achievement;
using Managers;
using Managers.SoundManager.Base;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Game.Jumper
{
    public class GameLayerController : MonoInstaller
    {
        public const int TARGET_METERS_AMOUNT = 384400;

        public LayerSettings[] Map;
        public LayerSettings.SpawnItem Resources;
        public LayerSettings.SpawnItem OtherItems;
        public GameObject FinishPlatform;
        public UnityAction<int> EnterSphere;
        public int CurrentLayerId { get; private set; }
        public bool FinishSpawned { get; private set; }

        public LayerSettings CurrentLayer => Map[CurrentLayerId];
        public float PercentOfLayer => GetPercentOfSphereByObject(true);
        public float GlobalPercentOfMap => playerY / totalHeight;
        public float GlobalPercentOfSmog => smogController.transform.position.y / totalHeight;

        [SerializeField] JumperSoloLineSeparator newSphereLine;
        [SerializeField] internal GameObstaclesController spawner;
        [SerializeField] float newChunkDetectionOffset = 10;
        [SerializeField] float finishDetectRange = 1;
        [SerializeField] MovableDestroyerController smogController;

        [Inject] ISoundManager sounds;
        [Inject] INetworkManager network;
        [Inject] private AchievementManager _achievementManager;
        
        private Dictionary<int, bool> playerEntered = new ();
        

        internal JumperSoloGameController game;
        private Transform player;
        private float lastChangedYPos;

        private float playerY =>
            player != null ? player.position.y : 0;
        private bool isCurrentLayerLast =>
           CurrentLayerId == Map.Length - 1;

        private float totalHeight
        {
            get
            {
                var result = 0f;
                for (var x = 0; x < Map.Length; x++)
                    result += Map[x].Height;
                return result;
            }
        }
        private Camera cam => Camera.main;

        public Transform Player => player;

        #region Calculation
        
        public int ConvertToGameMetersFromPos (float y)
        {
            var sphereId = GetSphereByPosY(y);
            var percent = GetPercentOfSphereByObjectPos(y);
            return ConvertToGameMeters(sphereId, percent);
        }
        
        public int ConvertToGameMeters (int layerId)
        {
            if (layerId == Map.Length)
                return TARGET_METERS_AMOUNT;
            var percent = GetStartLayerHeight(layerId) / totalHeight;
            var result = (int)(percent * TARGET_METERS_AMOUNT);
            return result;
        }

        public int ConvertToGameMeters(int layerId, float percent)
        {
            var startHeight = GetStartLayerHeight(layerId);
            var heightInLayer = Map[layerId].Height * percent;
            var resultPercent = (startHeight + heightInLayer) / totalHeight;
            var result = (int)(resultPercent * TARGET_METERS_AMOUNT);
            return result;
        }

        public (int, float) GetHeightscoreFromStartData ()
        {
            return GetPairFromMeters(game.StartData.high_score);
        }

        public int GetMetersFromPair (int sphereId, float percent)
        {
            var result = 0f;
            for(var x = 0; x < Map.Length && x <= sphereId; x++)
            {
                if (x < sphereId)
                    result += Map[x].Height;
                else
                    result += Map[x].Height * percent;
            }
            return (int)result;
        }

        public (int, float) GetPairFromMeters (int meters)
        {
            var sphereId = GetSphereByPosY(meters);
            var percent = GetPercentOfSphereByObjectPos(meters);
            return (sphereId, percent);
        }

        public float GetPercentOfSphereByObject(bool forPlayer)
        {
            var target = forPlayer ? player.transform : smogController.transform;
            return GetPercentOfSphereByObjectPos(target.position.y);
        }

        public float GetPercentOfSphereByObjectPos(float objectPosY)
        {
            var sphere = Mathf.Clamp(GetSphereByPosY(objectPosY), 0, Map.Length - 1);
            var min = GetStartLayerHeight(sphere);
            var max = Map[sphere].Height;
            return (objectPosY - min) / max;
        }

        public int GetSmogShpereId() =>
            GetSphereByPosY(smogController.transform.position.y);

        public int GetPlayerShpereId() =>
            GetSphereByPosY(playerY);

        public int GetSphereByPosY(float y)
        {
            var max = 0f;
            for (var x = 0; x < Map.Length; x++)
            {
                max += Map[x].Height;
                if (y < max)
                    return x;
            }
            if (y > max) return Map.Length - 1;
            return -1;
        }
        #endregion

        public override void InstallBindings()
        {
            Container.
                Bind<GameLayerController>().
                FromInstance(this).
                AsSingle().
                NonLazy();
        }

        public override void Start()
        {
            playerEntered.Clear();
            game = transform.GetComponentInParent<JumperSoloGameController>();
            SetupPlayer();
            SetupCurrentLayer(0);
            spawner.GenerateChunk();
            lastLinePosY = CurrentLayer.Height;
            
            game.IsGameBegin.OnValueChanged += OnGameBegin;
            BuildSperators();
            UpdateCameraColor(true);
        }
        
        private void OnGameBegin (bool old, bool current)
        {
            if (current)
            {
                SetupHightscoreIfHave();
                game.IsGameBegin.OnValueChanged -= OnGameBegin;
            }
        }
        
        private void BuildSperators ()
        {
            for(var x = 0; x < Map.Length; x++)
            {
                var y = GetStartLayerHeight(x);
                var result = Instantiate(newSphereLine, Vector3.up * y, newSphereLine.transform.rotation);
                result.Setup(Map[x].name.Substring(3), ConvertToGameMeters(x), x);
            }
        }

        public void SetupPlayer()
        {
            player = GameObject.FindWithTag("Player").transform;
        }

        private async void Update()
        {
            if (game.IsGameBegin.Value && game.IsGameEnded.Value == false)
            {
                var sphereId = GetSphereByPosY(playerY);
                UpdateCameraColor();
                HandleLayerAudio();

                if (heightscoreLine && playerY > heightscoreLine.transform.position.y)
                {
                    heightscoreLine.PlayParticles();
                }

                if (playerY > spawner.lastGeneratedY - newChunkDetectionOffset)
                {
                    spawner.GenerateChunk();
                }

                if (!playerEntered.ContainsKey(sphereId))
                {
                    EnterSphere?.Invoke(sphereId);
                    playerEntered.Add(sphereId, true);
                    
                    if (sphereId == Map.Length-1)
                    {
                        _achievementManager.CheckAchievementTarget(new AchievementTarget(TargetType.Achieve5Zone, 1));
                    } 
                    else if (sphereId == Map.Length-2)
                    {
                        _achievementManager.CheckAchievementTarget(new AchievementTarget(TargetType.Achieve4Zone, 1));
                    }
                    else if (sphereId == Map.Length-3)
                    {
                        _achievementManager.CheckAchievementTarget(new AchievementTarget(TargetType.Achieve3Zone, 1));
                    }
                    else if (sphereId == Map.Length-4)
                    {
                        _achievementManager.CheckAchievementTarget(new AchievementTarget(TargetType.Achieve2Zone, 1));
                    }

                    var bankInfo = await network.GetMoonInfo();
                    game.StartData.moon_bank = bankInfo.BankAmount;
                    spawner.UpdateMoonBank();
                }

                if (playerY > lastChangedYPos + CurrentLayer.Height)
                {
                    if (isCurrentLayerLast)
                    {
                        if (finishInstance)
                        {
                            if (playerY > finishInstance.transform.position.y - Mathf.Abs(finishDetectRange))
                                game.PlayerWin();
                        }
                        else
                        {
                            finishInstance = Instantiate(FinishPlatform, Vector3.up * spawner.lastGeneratedY, Quaternion.identity);
                            Debug.Log($"Shpere {CurrentLayer.name} has rewarded clouds: {spawner.RewardedBlocksInSphere}");
                            FinishSpawned = true;
                        }
                        //game finished
                    }
                    else
                    {
                        SetupCurrentLayer(CurrentLayerId + 1);
                    }
                }
            }
        }

        private void UpdateCameraColor (bool onStart = false)
        {
            if (onStart)
            {
                cam.backgroundColor = Map[0].Color;
            }
            else
            {
                var playerSphereId = GetPlayerShpereId();
                var percent = GetPercentOfSphereByObject(true);
                var startColor = Map[playerSphereId].Color;
                var endColor = Map[Mathf.Clamp(playerSphereId + 1, 0, Map.Length - 1)].Color;
                cam.backgroundColor = Color.Lerp(startColor, endColor, percent);
            }
        }

        private void HandleLayerAudio ()
        {
            for(var x = 0; x < CurrentLayer.Audio.Length; x++)
            {
                var volume = CurrentLayer.Audio[x].Volume.Evaluate(PercentOfLayer);
                var soundId = CurrentLayer.Audio[x].Sound;
                if(sounds.IsPlaying(soundId) == false)
                    sounds.PlaySound(soundId, true, false);
                sounds.UpdateVolumeSound(soundId, volume);
            }
        }

        internal bool NeedStopGenerateChunk ()
        {
            return spawner.lastGeneratedY > GetStartLayerHeight(CurrentLayerId + 1);
        }

        [SerializeField, Multiline]
        private string lineTextTemplate = "<sprite index=0><sprite index=0><sprite index=0>{0} ({1}m)";

        [SerializeField]
        private bool spawnFirstSeparator = true;

        private float lastLinePosY;
        internal GameObject finishInstance;
        private void SetupCurrentLayer(int layerIdx)
        {
            Debug.Log($"Shpere {CurrentLayer.name} has rewarded clouds: {spawner.RewardedBlocksInSphere}");
            spawner.lastObstacles.Clear();
            spawner.RewardedBlocksInSphere = 0;
            CurrentLayerId = layerIdx;
            spawner.CurrentLayer = CurrentLayer;
            lastChangedYPos = playerY;

            smogController.EnableChild(CurrentLayer.SmogChildIdEnabled);
        }

        private JumperSoloLineSeparator heightscoreLine;
        private void SetupHightscoreIfHave()
        {
            var hs = GetHeightscoreFromStartData();
            var layerId = hs.Item1;
            if (layerId >= 0 && hs.Item2 > 0)
            {
                var min = GetStartLayerHeight(layerId);
                var heightWithPercent = Map[layerId].Height * hs.Item2;
                var resHeight = min + heightWithPercent;
                var pos = Vector3.up * resHeight;
                if (pos.y > 10)
                {
                    heightscoreLine = Instantiate(newSphereLine, pos, newSphereLine.transform.rotation);
                    heightscoreLine.Setup("Your best", ConvertToGameMeters(layerId, hs.Item2), Map.Length);
                }
            }
        }

        private int GetStartLayerHeight(int layerIdx)
        {
            var result = 0f;
            for (var x = 0; x < layerIdx && x < Map.Length; x++)
            {
                result += Map[x].Height;
            }
            return (int)result;
        }
    }
}