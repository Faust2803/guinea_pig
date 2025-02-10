using Auth;
using Managers;
using Managers.ConfigDataManager;
using Managers.DatabaseManager;
using Managers.SceneManagers;
using Managers.SoundManager;
using Managers.SoundManager.Base;
using UnityEngine;
using Zenject;
namespace Installers
{
    public class ManagersInstaller: MonoInstaller
    {
        [SerializeField] private SoundManager _soundManagerPrefab;
        public override void InstallBindings()
        {
            //Bind Manager
            Container.BindInterfacesAndSelfTo<AdressableLoaderManager>().AsSingle().NonLazy(); ;
            Container.Bind<INetworkManager>().To<UWRNetworkManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<SceneLoadManagers>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<UiManager>().AsSingle().NonLazy();
            Container.Bind<IDatabaseManager>().To<FirebaseDatabaseManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PlayerManager>().AsSingle().NonLazy();
            Container.Bind<IConfigDataManager>().To<FirebaseConfigDataManager>().AsSingle().NonLazy();
            Container.Bind<IAuth>().To<FirebaseAuthManager>().AsSingle().NonLazy();
            
            
            Container.Bind<ISoundManager>().FromComponentInNewPrefab(_soundManagerPrefab).AsSingle().NonLazy();
            Container.Bind<ISoundSourceHandlerFactory>().FromInstance(new SoundSourceFactory()).AsSingle();
        }
    }
}