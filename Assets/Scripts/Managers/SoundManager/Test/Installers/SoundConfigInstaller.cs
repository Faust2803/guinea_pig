using Managers.SoundManager.Data;
using UnityEngine;
using Zenject;

namespace Managers.SoundManager.Test.Installers
{
    [CreateAssetMenu(menuName = "Configs/Sound/Installer/SoundConfigInstaller", fileName = "SoundConfigInstaller")]
    public class SoundConfigInstaller : ScriptableObjectInstaller<SoundConfigInstaller>
    {
        [SerializeField] private SoundConfig _config;
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<SoundConfig>().FromInstance(_config).AsSingle();
        }
    }
}