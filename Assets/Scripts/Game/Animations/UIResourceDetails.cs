using DataModels.PlayerData;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game
{
    [CreateAssetMenu(menuName = "UI/Resource Preset")]
    public class UIResourceList : ScriptableObject
    {
        [SerializeField] private UIItemData[] resources;

        public UIItemData GetAnchorByResource(ResourcesType target)
        {
            foreach (var resource in resources)
                if (resource.Resource == target)
                    return resource;
            return null;
        }

        public AssetReferenceGameObject this[ResourcesType res]
        {
            get
            {
                foreach (var resource in resources)
                    if (resource.Resource == res)
                        return resource.Prefab;
                return null;
            }
        }

        [System.Serializable]
        public class UIItemData
        {
            public ResourcesType Resource;
            public AssetReferenceGameObject Prefab;
        }
    }
}