using Game.Character;
using Game.Environment;
using UI.Panels;
using UI.Windows;
using Zenject;

namespace Util
{
    public class FactoryWindow : PlaceholderFactory<WindowType, BaseWindowView> { }
    public class FactoryPanel : PlaceholderFactory<PanelType, BasePanelView> { }
    public class FactoryCharacter : PlaceholderFactory<CharacterType, CharacterView> { }
    public class FactoryEnvironment : PlaceholderFactory<EnvironmentType, EnvironmentView> { }
}