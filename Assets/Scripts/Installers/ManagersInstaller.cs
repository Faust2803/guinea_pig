using Common.Statistic;
using Managers;
using Managers.Analytics;
using Managers.SoundManager;
using Managers.SoundManager.Base;
using UnityEngine;
using Zenject;
namespace Installers
{
    public class ManagersInstaller: MonoInstaller
    {
        [SerializeField] private SoundManager _soundManagerPrefab;
        // [SerializeField] private AssetReferenceT<SoundConfig> _soundConfig;
        public override void InstallBindings()
        {
            //Bind Manager
            Container.BindInterfacesAndSelfTo<AdressableLoaderManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<AuthorizationManager>().AsSingle().NonLazy();
            Container.Bind<INetworkManager>().To<UWRNetworkManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<SceneLoadManagers>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<UiManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PlayerManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<CollectionManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<TutorialManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<AchievementManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<LeaderboardManager>().AsSingle().NonLazy();
            

            Container.BindInterfacesTo<StatisticManager>().AsSingle();
            SignalInstaller.Install(Container);
            Container.Bind<ISoundManager>().FromComponentInNewPrefab(_soundManagerPrefab).AsSingle().NonLazy();
            Container.Bind<ISoundSourceHandlerFactory>().FromInstance(new SoundSourceFactory()).AsSingle();
        }

        private void OnApplicationQuit()
        {
            Container.Resolve<INetworkManager>().Dispose();
        }
    }
}