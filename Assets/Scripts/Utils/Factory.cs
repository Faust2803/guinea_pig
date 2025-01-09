using Game;
using Game.Character;
using Game.Environment;
using ThreeInRowGame;
using UI.Panels;
using UI.Windows;
using Zenject;

namespace Util
{
    public class FactoryWindow : PlaceholderFactory<WindowType, BaseWindowView> { }
    public class FactoryPanel : PlaceholderFactory<PanelType, BasePanelView> { }
    public class FactoryCharacter : PlaceholderFactory<CharacterType, CharacterView> { }
    public class FactoryEnvironment : PlaceholderFactory<EnvironmentType, EnvironmentView> { }
    public class FactoryBoolet : PlaceholderFactory<BooletView> { }
    
    //ThreeInRowGame Factorys
    public class FactoryLevel : PlaceholderFactory<int, LevelView> { }
    public class FactoryCellElement : PlaceholderFactory<CellType, CellMediator> { }
    
    
    
}