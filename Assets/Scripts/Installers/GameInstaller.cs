using Game;
using SO.Scripts;
using UI.Windows;
using UnityEngine;
using Util;
using Zenject;

namespace Installers
{
    public class GameInstaller : MonoInstaller
    {
        [Inject] private CharacterConfig _characterConfig;
        [Inject] private EnvironmentConfig _environmentConfig;
        
        public override void InstallBindings()
        {
            //Bind Factory
            Container.BindFactory< CharacterType, BaseCharacterView, FactoryCharacter>().FromMethod(InitCharacter);
            Container.BindFactory< EnvironmentType, GameObject, FactoryEnvironment>().FromMethod(InitEnvironment);
        }

        private BaseCharacterView InitCharacter(DiContainer container, CharacterType character)
        {
            var level = _characterConfig.CharacterPrefab[(int)character];
            return Container.InstantiatePrefabForComponent<BaseCharacterView>(level);
        }
        
        private GameObject InitEnvironment(DiContainer container, EnvironmentType environment)
        {
            var level = _environmentConfig.EnvironmentPrefab[(int)environment];
            return Container.InstantiatePrefabForComponent<GameObject>(level);
        }
    }
}