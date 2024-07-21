using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;
using Sequence = DG.Tweening.Sequence;
using DataModels.PlayerData;

namespace Game
{
    public class ResourcesMoveUIAnimation : MonoBehaviour
    {
        #region Fields Declaration
        private const int DefaultMaxAmount = 100;

        [Header("Spawn Resources Info")]

        //[SerializeField] private GameObject resourcePrefab;
        [SerializeField] private int resourceImgSize;
        [SerializeField] private int resourceImgAmount;

        [Space(5)]

        [SerializeField] private List<GameObject> spawnedResources;

        [Space(5)]

        [SerializeField] private RectTransform resourceSpawnPos;
        [SerializeField] private RectTransform resourceEndPos;
        [SerializeField] private RectTransform resourceDefaultEndPos;

        [Space(10)]
        [Header("Randomizing Spawn Position")]

        [SerializeField] private float minX;
        [SerializeField] private float maxX;
        [SerializeField] private float minY;
        [SerializeField] private float maxY;

        [Space(10)]
        [Header("Burst Spawn Position")]

        [SerializeField] private float burstMultPos;
        [SerializeField] private float burstDuration;

        [Space(10)]
        [Header("Movement Spawn Position")]

        [SerializeField] private float jumpPower;
        [SerializeField] private float jumpMinDuration;
        [SerializeField] private float jumpMaxDuration;

        [SerializeField] private UIResourceList prefabList;
        [SerializeField] private ResourceEndPos[] resourceEndPoses;

        private float resourceEndUpScale = 1.15f;
        private float resourceEndOriginalScale = 1f;

        #endregion Fields Declaration

        public RectTransform this[ResourcesType res]
        {
            get
            {
                foreach (var resource in resourceEndPoses)
                    if (resource.Resource == res)
                        return resource.Image;

                return resourceDefaultEndPos;
            }
        }

        public void SpawnResources(int amount,
            ResourcesType resource,
            Vector2 startPos,
            int maxAmount = DefaultMaxAmount)
        {
            SpawnResources(amount, startPos, this[resource], resource, maxAmount);
        }

        public async void SpawnResources(int amount,
                                    Vector2 startScreenPoint,
                                    RectTransform endScreenPoint,
                                    ResourcesType resType,
                                    int maxAmount = DefaultMaxAmount)
        {
            var targetPrefab = await prefabList[resType].LoadAssetAsync();
            resourceEndPos = endScreenPoint;
            resourceImgAmount = amount;
            //PlayerCopyMovement.BeansCurrency//TODO: add bean amount to all screens
            spawnedResources.Clear();

            for (int i = 0; i < Math.Clamp(resourceImgAmount, 0, maxAmount); i++)
            {
                GameObject resInstance = Instantiate(targetPrefab, resourceSpawnPos);

                float xPosition = startScreenPoint.x * Random.Range(minX, maxX);
                float yPosition = startScreenPoint.y * Random.Range(minY, maxY);
                resInstance.transform.localPosition = new Vector3(xPosition, yPosition);

                spawnedResources.Add(resInstance);

                resourceEndOriginalScale = resourceEndPos.localScale.x;
            }

            prefabList[resType].ReleaseAsset();
            StartMoveResources();
        }

        public void SpawnResources(int amount, ResourcesType resType, int maxAmount = DefaultMaxAmount)
        {
            SpawnResources(amount, resType, resourceEndPos.anchoredPosition, maxAmount);
        }

        public void SpawnResources(int amount, ResourcesType resType)
        {
            Debug.Log($"{resType} {amount}");
            SpawnResources(amount, resType, this[resType].anchoredPosition);
        }

        public void SpawnBeanRes(int amount)
        {
            SpawnResources(amount, ResourcesType.Bean);
        }

        public void SpawnGoldenBeanRes(int amount)
        {
            SpawnResources(amount, ResourcesType.GoldenBean);
        }

        public void SpawnPeaRes(int amount)
        {
            SpawnResources(amount, ResourcesType.Pease);
        }

        public void SpawnCornRes(int amount)
        {
            SpawnResources(amount, ResourcesType.Corn);
        }

        public void SpawnSeedRes(int amount)
        {
            SpawnResources(amount, ResourcesType.Seed);
        }

        private void StartMoveResources()
        {
            Vector3 endPos = resourceEndPos.transform.position;

            foreach (var spawnedResource in spawnedResources)
            {
                RectTransform spawnedResourcePos = spawnedResource.transform.GetComponent<RectTransform>();
                SpriteRenderer spawnedResourceAlpha = spawnedResourcePos.GetComponent<SpriteRenderer>();
                float jumpDuration = Random.Range(jumpMinDuration, jumpMaxDuration) / 5;
                float randomSpawnDelay = Random.Range(0f, 0.5f);

                spawnedResourceAlpha.DOFade(0f, 0f);
                spawnedResourcePos.DOScale(1, 0f);

                Sequence resourceMovement = DOTween.Sequence();

                //Resource burst
                resourceMovement.Join(spawnedResourcePos.DOLocalMove(new Vector2(Random.Range(-burstMultPos, burstMultPos),
                    Random.Range(-burstMultPos, burstMultPos)), burstDuration)).SetEase(Ease.OutQuint).SetDelay(randomSpawnDelay, false);
                resourceMovement.Join(spawnedResourcePos.DOScale(resourceImgSize, burstDuration));
                resourceMovement.Join(spawnedResourceAlpha.DOFade(1, burstDuration));

                //Resource start moving to the point of destination
                resourceMovement.Append(spawnedResourcePos.DOJump(endPos, -jumpPower, 1, jumpDuration).SetEase(Ease.InCirc));
                resourceMovement.Join(spawnedResourcePos.DOScale(resourceImgSize / 2.25f, jumpDuration));
                resourceMovement.Join(spawnedResourceAlpha.DOFade(0.75f, jumpDuration));

                //Resource reach the point of destination
                resourceMovement.Append(resourceEndPos.DOScale(resourceEndOriginalScale * resourceEndUpScale, 0.15f));
                resourceMovement.Join(spawnedResourcePos.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InCirc));
                resourceMovement.Append(resourceEndPos.transform.DOScale(resourceEndOriginalScale, 0.005f))
                    .OnComplete(() =>
                    {
                        Destroy(spawnedResource);
                        resourceMovement?.Kill();
                        resourceMovement = null;
                    });
            }
        }

        [Serializable]
        public class ResourceEndPos
        {
            public ResourcesType Resource;
            public RectTransform Image;
        }
    }
}