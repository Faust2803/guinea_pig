using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.Jumper
{
    public class GameObstaclesController : MonoInstaller
    {
        public int RewardedBlocksInSphere;

        public LayerSettings CurrentLayer;
        public float CurrentStepHeight = -2;

        internal float lastGeneratedY;

        private int genFollowLineIdx;
        private GameLayerController layerController;

        [SerializeField] JumperSoloPlayer player;
        [SerializeField] LineController[] lines;
        [SerializeField] int partsInChunk = 10;
        [SerializeField] string moonTextTemplate = "{0}";
        [SerializeField, Range(0.05f, 3f)] float chunksUpdateRate = 1.5f;

        private float lastChunkUpdateTime;
        private float fromLastUpdateSeconds =>
            Time.time - lastChunkUpdateTime;

        public override void InstallBindings()
        {
            Container.
                Bind<GameObstaclesController>().
                FromInstance(this).
                AsSingle().
                NonLazy();
        }

        private void Awake()
        {
            layerController = GetComponentInParent<GameLayerController>();
        }

        private void Update()
        {
            if(fromLastUpdateSeconds > chunksUpdateRate)
            {
                UpdateChunks();
                lastChunkUpdateTime = Time.time;
            }
        }

        #region Chunks
        public void UpdateChunks ()
        {
            for(var x = transform.childCount - 1; x >= 0; x--)
            {
                if(transform.GetChild(x).childCount == 0)
                {
                    Destroy(transform.GetChild(x).gameObject);
                    continue;
                }

                var chunk = transform.GetChild(x);
                var distanceToPlayer = Vector3.Distance(chunk.GetChild(chunk.childCount / 2).position, player.transform.position);
                var visibled = distanceToPlayer < partsInChunk * 4;
                transform.GetChild(x).gameObject.SetActive(visibled);
            }
        }

        private int chunkId;
        public void GenerateChunk()
        {
            var chunkParent = new GameObject("Chunk " + chunkId);
            chunkParent.transform.parent = transform;

            for (var steps = 0; steps < partsInChunk; steps++)
            {
                GenerateLine(chunkParent.transform);
                if (layerController.NeedStopGenerateChunk())
                    continue;
            }

            chunkId++;
            UpdateMoonBank();
        }

        internal Dictionary<int, float> lastObstacles = new Dictionary<int, float>(); 
        private void GenerateLine(Transform parent)
        {
            if (layerController.FinishSpawned)
                return;

            var allPoints = GeneratePoses();

            var empty = 0;
            var obst = GetObstacle(layerController.PercentOfLayer, CurrentStepHeight);

            if (obst == null || obst.DisblePlatfromGenWithObstacles == false)
            {
                var percentGen = layerController.GetPercentOfSphereByObjectPos(lastGeneratedY);
                for (var x = 0; x < allPoints.Count; x++)
                {
                    var need = x == genFollowLineIdx;
                    var currentPoint = allPoints[x];
                    if (need && CurrentLayer.FollowWinLine)
                    {
                        GeneratePart(currentPoint, parent);
                        continue;
                    }

                    var currentDensity = CurrentLayer.Density.Evaluate(percentGen);
                    var maxEmpty = Mathf.RoundToInt(CurrentLayer.MaxEmptyCellsOnOneLine.Evaluate(percentGen));
                    //Debug.Log($"{percentGen}: Den {currentDensity}, max: {maxEmpty}");
                    if (Random.value <= currentDensity &&
                        empty < maxEmpty)
                    {
                        empty++;
                    }
                    else
                    {
                        GeneratePart(currentPoint, parent);
                    }
                }
            }

            if (obst != null)
            {
                var spawnSide = false;
                var source = allPoints[Random.Range(0, allPoints.Count)];
                var item = obst.GetRandomItemWithIndex(out var obstIndex);

                if (lastObstacles.ContainsKey(obstIndex))
                    lastObstacles[obstIndex] = source.y;
                else
                    lastObstacles.Add(obstIndex, source.y);

                if (obst.UseUniqueXPos)
                {
                    spawnSide = Random.Range(0, 2) == 0;
                    source.x = obst.CanBeInvert ?
                        spawnSide ? -obst.UniquePos : obst.UniquePos :
                        obst.UniquePos;
                }
                var result = Instantiate(item, source, item.transform.rotation, transform);
                if (obst.ChangeSizeOnInvert)
                    result.transform.localScale = new Vector3((spawnSide ? 1 : -1) * result.transform.localScale.x, 1, 1);
            }

            GenerateWinIndex();
            CurrentStepHeight += CurrentLayer.GetRandomStepHeight();
        }

        private int repeatWinCount;
        private int lastGenWinIndex;
        private void GenerateWinIndex ()
        {
            var variants = GetWinVariants();
            genFollowLineIdx = variants[Random.Range(0, variants.Length)];

            if (genFollowLineIdx == lastGenWinIndex)
                repeatWinCount++;
            else if(repeatWinCount > 0)
                repeatWinCount--;

            lastGenWinIndex = genFollowLineIdx;
        }

        private int[] GetWinVariants ()
        {
            var result = new List<int>();

            for(var x = 0; x < lines.Length; x++)
            {
                if(x == lastGenWinIndex && CurrentLayer.MaxStackBlocks >= repeatWinCount)
                    continue;

                result.Add(x);
            }

            return result.ToArray();
        }

        private void GeneratePart(Vector2 pos, Transform parent)
        {
            GeneratePart(pos.x, pos.y, parent);
        }

        private void GeneratePart(float posX, float posY, Transform parent)
        {
            var stage = GetRandomStage();
            var pos = new Vector3(posX, posY, 0);
            lastGeneratedY = posY;
            Instantiate(stage, pos, stage.transform.rotation, parent);
            GenerateRandomResourceIfNeed(pos + Vector3.up * 0.15f);
        }

        private void GenerateRandomResourceIfNeed(Vector3 pos)
        {
            if (layerController.Resources.NeedSpawn(layerController.PercentOfLayer))
            {
                var itemPrefab = layerController.Resources.GetRandomItem;
                var item = Instantiate(itemPrefab, pos, itemPrefab.transform.rotation);
            }
            else if (layerController.OtherItems.NeedSpawn(layerController.PercentOfLayer))
            {
                var itemPrefab = layerController.OtherItems.GetRandomItem;
                var item = Instantiate(itemPrefab, pos, itemPrefab.transform.rotation);
            }
        }

        private GameObject GetRandomStage()
        {
            var hasLimit = RewardedBlocksInSphere >= CurrentLayer.Platforms.MaxDefaultPlatformsCount;
            var percent = layerController.PercentOfLayer;
            var defaultBlockRate = layerController.CurrentLayer.Platforms.MaxDefaultPlatformsCount / layerController.CurrentLayer.Height;
            var notBreakable = Random.value < defaultBlockRate;
            if (notBreakable)
            {
                var isBreak = hasLimit && CurrentLayer.Platforms.Breakable.NeedSpawn(percent);
                if(isBreak == false)
                    RewardedBlocksInSphere++;
                return isBreak ? CurrentLayer.Platforms.Breakable.GetRandomItem : CurrentLayer.Platforms.Default;
            }
            else
            {
                var isBooster = CurrentLayer.Platforms.Booster.NeedSpawn(layerController.PercentOfLayer);
                var isBreakable = CurrentLayer.Platforms.Breakable.NeedSpawn(layerController.PercentOfLayer);
                return isBooster ? CurrentLayer.Platforms.Booster.GetRandomItem : (isBreakable ? CurrentLayer.Platforms.Breakable.GetRandomItem : CurrentLayer.Platforms.Default);
            }
        }

        private List<Vector3> GeneratePoses()
        {
            var result = new List<Vector3>();

            for (var x = 0; x < lines.Length; x++)
                result.Add(lines[x].BuildRandomHorizontal(CurrentStepHeight));

            return result;
        }

        private LayerSettings.SpawnItem GetObstacle(float percentOfLayer, float y)
        {
            if (CurrentLayer.Obstacles.Length > 0)
            {
                for (var x = 0; x < CurrentLayer.Obstacles.Length; x++)
                {
                    var obst = CurrentLayer.Obstacles[x];

                    if (obst.NeedSpawn(percentOfLayer))
                    {
                        if (lastObstacles.ContainsKey(x))
                        {
                            var diff = Mathf.Abs(lastObstacles[x] - y);
                            if (diff > obst.MinYDifference)
                                return obst;
                            else
                                continue;
                        }
                        else
                        {
                            return obst;
                        }
                    }
                }
            }

            return null;
        }
        #endregion

        public void UpdateMoonBank ()
        {
            if (layerController.FinishSpawned)
            {
                var text = layerController.finishInstance.GetComponentInChildren<TextMeshPro>();
                text.text = string.Format(moonTextTemplate, layerController.game.StartData.moon_bank);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            foreach (var line in lines)
                line.GizmosDraw();
        }
#endif

        [System.Serializable]
        public class LineController
        {
            public float X;
            public Vector2 Width = new Vector2(-1, 1);

            [SerializeField] float maxYRandomOffset = 0.1f;

            public Vector3 BuildPoint(float y) =>
                new Vector3(X, y, 0);

            public Vector3 BuildRandomHorizontal(float y, bool randomRangeOffset = true)
            {
                var x = Random.Range(Width.x, Width.y);
                var offset = randomRangeOffset ? Random.Range(-maxYRandomOffset, maxYRandomOffset) : 0;
                return new Vector3(x + X, y + offset, 0);
            }

#if UNITY_EDITOR
            [SerializeField] Color lineDebugColor = Color.yellow;

            internal void GizmosDraw()
            {
                const int height = 50;

                Gizmos.color = lineDebugColor / 2;
                Gizmos.DrawLine(BuildPoint(0), BuildPoint(height));

                Gizmos.color = lineDebugColor;
                Gizmos.DrawLine(new Vector3(X + Width.y, 0),
                    new Vector3(X + Width.y, height));
                Gizmos.DrawLine(new Vector3(X - Mathf.Abs(Width.x), 0),
                    new Vector3(X - Mathf.Abs(Width.x), height));
            }
#endif
        }
    }
}