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
        [SerializeField] public SoundConfig _soundConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(_windowsConfig).IfNotBound();
            Container.BindInstance(_panelConfig).IfNotBound();
            Container.BindInterfacesTo<SoundConfig>().FromInstance(_soundConfig).AsSingle();
        }
    }
}