using Common.HamsterPyramid;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class PyramidInstaller : MonoInstaller
    {
        [SerializeField] private HamsterPyramidController _pyramidController;
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<HamsterPyramidDataProvider>().AsSingle();
            Container.Bind<HamsterPyramidController>().FromComponentInNewPrefab(_pyramidController).AsSingle();
        }
    }
}