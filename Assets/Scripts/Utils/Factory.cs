using Game;
using Game.Character;
using Game.Environment;
using ThreeInRowGame;
using UI.Panels;
using UI.Windows;
using UnityEngine;
using Zenject;

namespace Util
{
    public class FactoryWindow : PlaceholderFactory<WindowType, BaseWindowView> { }
    public class FactoryPanel : PlaceholderFactory<PanelType, BasePanelView> { }
    public class FactoryCharacter : PlaceholderFactory<CharacterType, Transform, CharacterMediator> { }
    public class FactoryEnvironment : PlaceholderFactory<EnvironmentType, Transform, EnvironmentView> { }
    public class FactoryBoolet : PlaceholderFactory<Transform, BooletView> { }
    
    //ThreeInRowGame Factorys
    public class FactoryLevel : PlaceholderFactory<int, Transform, LevelView> { }
    public class FactoryCellElement : PlaceholderFactory<CellType, Transform, CellMediator> { }
    
    
    
}