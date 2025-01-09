using UnityEngine;
using Zenject;

namespace SO.Scripts.ThreeInRow
{
    [CreateAssetMenu (fileName = "ThreeInRowSoInstaller", menuName = "Configs/ThreeInRow/SO Installer")]
    public class ThreeInRowSoInstaller : ScriptableObjectInstaller<SoInstaller>
    {
        [SerializeField] public CellConfig _cellConfig;
        [SerializeField] public LevelConfig _levelConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(_cellConfig).IfNotBound();
            Container.BindInstance(_levelConfig).IfNotBound();
        }
    }
}