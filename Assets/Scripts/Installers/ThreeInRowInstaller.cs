using System.Collections.Generic;
using Game;
using Game.Character;
using Game.Environment;
using SO.Scripts;
using SO.Scripts.ThreeInRow;
using ThreeInRowGame;
using UI.Windows;
using UnityEngine;
using Util;
using Zenject;

namespace Installers
{
    public class ThreeInRowInstaller : MonoInstaller
    {
        [Inject] private CellConfig _cellConfig;
        [Inject] private LevelConfig _levelConfig;
        
        public override void InstallBindings()
        {
            //Bind Factory
            Container.BindFactory< CellType, Transform, CellMediator, FactoryCellElement>().FromMethod(InitCellElement);
            Container.BindFactory< int, Transform, LevelView, FactoryLevel>().FromMethod(InitGreed);
        }

        private CellMediator InitCellElement(DiContainer container, CellType type, Transform transform)
        {
            var element = _cellConfig.CellPrefab[(int)type];
            var view = Container.InstantiatePrefabForComponent<CellView>(element);
            view.Init();
            var  mediator = view.Mediator;
            view.gameObject.transform.SetParent(transform,false);
            return mediator;
        }
        
        private LevelView InitGreed(DiContainer container, int levelNumber, Transform transform)
        {
            var level = _levelConfig.LevelPrefab[levelNumber];
            var view = Container.InstantiatePrefabForComponent<LevelView>(level);
            view.gameObject.transform.SetParent(transform,false);
            return view;
        }
    }
}