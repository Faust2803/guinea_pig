using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ThreeInRowGame;
using UnityEngine;
using Util;
using Zenject;


namespace Managers.SceneManagers
{
    public class ThreeInRowGameSceneManager : BaseSceneManager
    {
        public const int GREED_SIZE = 8;
        [Inject] protected FactoryLevel _factoryLevel;
        [Inject] protected FactoryCellElement _factoryElement;

        private LevelView _level;
        private ThreeInRowModel _model;
        private int _moveElementCounter = 0;
        
        //private LifeState _currentStage = LifeState.Adding;
        
        private Dictionary<GreedElementView, int> _newElementsPosition = new Dictionary<GreedElementView, int>();
        private Dictionary<int, CellMediator> _cellMediators = new Dictionary<int, CellMediator>();
        private Dictionary<int, CellMediator> _cellMediatorsPool = new Dictionary<int, CellMediator>();
        
        private void Start()
        {
            _loadingObject.SetActive(true);
             _audio.PlaySound(SoundManager.Enums.SoundId.JumperMusic, isLoop: true, false);
             _audio.UpdateVolumeSound(SoundManager.Enums.SoundId.JumperMusic, 0.5f);
            Init();
        }

        private void Init()
        {
            CellMediator.MoveCompletedEvent += OnMoveElementsCompleat;
            _level = _factoryLevel.Create(2, _gameArea);
            _model = new ThreeInRowModel(_level.Elements, _level.SpaunElementType);
            _loadingObject.SetActive(false);
            AddCellElements();
            MoveElements();
        }

        private void AddCellElements()
        {
            var modelElements = _model.AddNewElements();
            for (var i = 0; i < modelElements.Count; i++)
            {
                var element = GetCellMediatorFromPool(modelElements[i].Type) ?? _factoryElement.Create(modelElements[i].Type, _level.GameArea);
                element.SetData(modelElements[i]);
                FinedStartPosition(modelElements[i]);
                _cellMediators.Add(element.GetHashCode(), element);
            }
            _newElementsPosition.Clear();
        }

        private CellMediator GetCellMediatorFromPool(CellType type)
        {
            foreach (var poolElement in _cellMediatorsPool)
            {
                if (poolElement.Value.Data.Type == type)
                {
                    _cellMediatorsPool.Remove(poolElement.Key, out CellMediator element);
                    return element;
                }
            }

            return null;
        }

        private void FinedStartPosition(CellData cell)
        {
            for (var k = 0; k < _level.StartElements.Length; k++)
            {
                //todo
                //if(_level.StartElements[k].IsActive == CellType.NotWorking)continue;
                if (_level.StartElements[k].Poz_X == (int)cell.MovementStack.Peek().x)
                {
                    cell.СoordinatePoz_X = _level.StartElements[k].СoordinatePoz_X;
                    cell.СoordinatePoz_Y = _level.StartElements[k].СoordinatePoz_Y;
                    
                    if (!_newElementsPosition.ContainsKey(_level.StartElements[k]))
                    {
                        _newElementsPosition.Add(_level.StartElements[k], 0);
                    }

                    cell.MoveDelay = _newElementsPosition[_level.StartElements[k]];
                    _newElementsPosition[_level.StartElements[k]]++;

                    return;
                }
            }
        }

        private void MoveElements()
        {
            foreach (var element in _cellMediators)
            {
                if (element.Value.Data.CoordinateStack.Count > 0)
                {
                    _moveElementCounter++;
                }
                element.Value.ReadyToMove();
            }
            //Debug.Log($" _moveElementCounter = {_moveElementCounter}");
        }

        private void OnMoveElementsCompleat()
        {
            _moveElementCounter--;
            //Debug.Log($" _moveElementCounter = {_moveElementCounter}");
            if (_moveElementCounter == 0)
            {
                //Debug.Log($" MOVE ENDED");
                FoundAndDeleteElements();
            }
        }

        private async UniTask FoundAndDeleteElements()
        {
            var foundElements = _model.FinedMath();
            if (foundElements.Count == 0)
            {
                return;
            }
            await UniTask.Delay(1000);
            var deletedKey = new List<int>(foundElements.Count);
            foreach (var value in foundElements)
            {
                foreach (var element in _cellMediators)
                {
                    if (element.Value.Data.Poz_X == value.Value.x && element.Value.Data.Poz_Y == value.Value.y)
                    {
                        Debug.Log($" foundElements key = {value.Key} value = {value.Value} type = {element.Value.Data.Type}");
                        element.Value.Delete();
                        deletedKey.Add(element.Key);
                    }
                }
            }
            await UniTask.Delay(1500);
            foreach (var t in deletedKey)
            {
                _cellMediators.Remove(t, out CellMediator cellMediator);
                _cellMediatorsPool.Add(t, cellMediator);
            }
            Debug.Log($" DELETE COMPLEAT");
        }
        
        


        private void OnDestroy()
        {
            _audio.StopSound(SoundManager.Enums.SoundId.JumperMusic);
            CellMediator.MoveCompletedEvent -= OnMoveElementsCompleat;
        }
    }
    
    public enum LifeState
    {
        Adding,
        StartMove,
        Move,
        Delete,
    }
}
