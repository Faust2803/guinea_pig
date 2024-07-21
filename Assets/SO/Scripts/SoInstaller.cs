using Managers.SoundManager.Data;
using UnityEngine;
using Zenject;

namespace SO.Scripts
{
    [CreateAssetMenu (fileName = "SoInstaller", menuName = "Create SO Installer")]
    public class SoInstaller : ScriptableObjectInstaller<SoInstaller>
    {
        [SerializeField] public WindowsConfig _windowsConfig;
        [SerializeField] public PanelsConfig _panelConfig;
        [SerializeField] public CollectionsConfig _collectionsConfig;
        [SerializeField] public EquipsConfig _equipsConfig;
        [SerializeField] public SoundConfig _soundConfig;
        [SerializeField] public PlayerResourcesIconConfig _playerResourcesIconConfig;
        public override void InstallBindings()
        {
            Container.BindInstance(_windowsConfig).IfNotBound();
            Container.BindInstance(_panelConfig).IfNotBound();
            Container.BindInstance(_collectionsConfig).IfNotBound();
            Container.BindInstance(_equipsConfig).IfNotBound();
            Container.BindInterfacesTo<SoundConfig>().FromInstance(_soundConfig).AsSingle();
            Container.BindInstance(_playerResourcesIconConfig).IfNotBound();
        }
    }
}