using System.Collections.Generic;
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
        [Inject] private BooletConfig _booletConfig;
        
        public override void InstallBindings()
        {
            //Bind Factory
            Container.BindFactory< CharacterType, Transform, CharacterMediator, FactoryCharacter>().FromMethod(InitCharacter);
            Container.BindFactory< EnvironmentType, Transform, EnvironmentView, FactoryEnvironment>().FromMethod(InitEnvironment);
            Container.BindFactory< Transform, BooletView, FactoryBoolet>().FromMethod(InitBoolet);
        }

        private CharacterMediator InitCharacter(DiContainer container, CharacterType character, Transform transform)
        {
            var level = _characterConfig.CharacterPrefab[(int)character];
            var view = Container.InstantiatePrefabForComponent<CharacterView>(level);
            view.Init();
            var mediator = view.Mediator;
            mediator.SetType(character);
            view.gameObject.transform.SetParent(transform,false);
            return mediator;
        }
        
        private EnvironmentView InitEnvironment(DiContainer container, EnvironmentType environment, Transform transform)
        {
            var level = _environmentConfig.EnvironmentPrefab[(int)environment];
            var view = Container.InstantiatePrefabForComponent<EnvironmentView>(level);
            view.gameObject.transform.SetParent(transform,false);
            return view;
        }
        
        private BooletView InitBoolet(DiContainer container, Transform transform)
        {
            var boolet = _booletConfig.BooletPrefab[0];
            var view = Container.InstantiatePrefabForComponent<BooletView>(boolet);
            view.gameObject.transform.SetParent(transform,false);
            return view;
        }
        
    }
}