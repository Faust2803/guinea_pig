using Game;
using Game.Character;
using Game.Environment;
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
        [Inject] private BooletConfig _booletConfigConfig;
        
        public override void InstallBindings()
        {
            //Bind Factory
            Container.BindFactory< CharacterType, CharacterView, FactoryCharacter>().FromMethod(InitCharacter);
            Container.BindFactory< EnvironmentType, EnvironmentView, FactoryEnvironment>().FromMethod(InitEnvironment);
            Container.BindFactory< BooletView, FactoryBoolet>().FromMethod(InitBoolet);
        }

        private CharacterView InitCharacter(DiContainer container, CharacterType character)
        {
            var level = _characterConfig.CharacterPrefab[(int)character];
            return Container.InstantiatePrefabForComponent<CharacterView>(level);
        }
        
        private EnvironmentView InitEnvironment(DiContainer container, EnvironmentType environment)
        {
            var level = _environmentConfig.EnvironmentPrefab[(int)environment];
            return Container.InstantiatePrefabForComponent<EnvironmentView>(level);
        }
        
        private BooletView InitBoolet(DiContainer container)
        {
            var boolet = _booletConfigConfig.BooletPrefab[0];
            return Container.InstantiatePrefabForComponent<BooletView>(boolet);
        }
    }
}