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
            Container.BindFactory< CellType, CellMediator, FactoryCellElement>().FromMethod(InitCellElement);
            Container.BindFactory< int, LevelView, FactoryLevel>().FromMethod(InitGreed);
        }

        private CellMediator InitCellElement(DiContainer container, CellType type)
        {
            var element = _cellConfig.CellPrefab[(int)type];
            var view = Container.InstantiatePrefabForComponent<CellView>(element);
            view.Init();
            var  mediator = view.Mediator;
            mediator.SetType(type);
            return mediator;
        }
        
        private LevelView InitGreed(DiContainer container, int levelNumber)
        {
            var level = _levelConfig.LevelPrefab[levelNumber];
            return Container.InstantiatePrefabForComponent<LevelView>(level);
        }
    }
}