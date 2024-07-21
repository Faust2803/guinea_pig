using Common.HamsterPyramid.InputManager;
using UnityEngine;
using Zenject;

namespace Installers.Misc
{
    public class InputManagerInstaller : MonoInstaller
    {
        [SerializeField] private InputManager _inputManager;
        
        public override void InstallBindings()
        {
            Container.Bind<IInputManager>().FromComponentInNewPrefab(_inputManager);
        }
    }
}