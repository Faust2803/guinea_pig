using Game;
using UI.Panels;
using UI.Windows;
using UnityEngine;
using Zenject;

namespace Util
{
    public class FactoryWindow : PlaceholderFactory<WindowType, BaseWindowView> { }
    public class FactoryPanel : PlaceholderFactory<PanelType, BasePanelView> { }
    public class FactoryCharacter : PlaceholderFactory<CharacterType, BaseCharacterView> { }
    public class FactoryEnvironment : PlaceholderFactory<EnvironmentType, EnvironmentView> { }
}