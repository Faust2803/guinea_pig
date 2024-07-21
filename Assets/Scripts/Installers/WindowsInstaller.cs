using SO.Scripts;
using UI.Windows;
using Util;
using Zenject;

namespace Installers
{
    public class WindowsInstaller : MonoInstaller
    {
        [Inject] private WindowsConfig _windowsConfig;
        
        public override void InstallBindings()
        {
            //Bind Factory
            Container.BindFactory< WindowType, BaseWindowView, FactoryWindow>().FromMethod(InitWindow);
        }

        private BaseWindowView InitWindow(DiContainer container, WindowType window)
        {
            var level = _windowsConfig.WindowsPrefab[(int)window];
            return Container.InstantiatePrefabForComponent<BaseWindowView>(level);
        }
    }
}