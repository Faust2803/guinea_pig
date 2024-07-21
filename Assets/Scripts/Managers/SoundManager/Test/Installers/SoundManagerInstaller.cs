using Managers.SoundManager.Base;
using UnityEngine;
using Zenject;

namespace Managers.SoundManager.Test.Installers
{
    public class SoundManagerInstaller : MonoInstaller
    {
        [SerializeField] private SoundManager _soundManager;
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SoundSourceFactory>().AsSingle();
            Container.Bind<ISoundManager>().FromComponentInNewPrefab(_soundManager).AsSingle();
        }
    }
}