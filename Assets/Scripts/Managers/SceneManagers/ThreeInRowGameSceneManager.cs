using System.Collections.Generic;
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
        private LifeState _currentStage = LifeState.Adding;
        
        private Dictionary<int, int> _newElementsPosition = new Dictionary<int, int>();
        private List<CellMediator> _cellMediators = new List<CellMediator>();
        
        private void Start()
        {
            _loadingObject.SetActive(true);
             _audio.PlaySound(SoundManager.Enums.SoundId.JumperMusic, isLoop: true, false);
             _audio.UpdateVolumeSound(SoundManager.Enums.SoundId.JumperMusic, 0.5f);
            Init();
        }

        private void Init()
        {
            _level = _factoryLevel.Create(0, _gameArea);
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
                var element =_factoryElement.Create(modelElements[i].Type, _level.GameArea);
                FinedStartPosition(modelElements[i]);
                element.SetData(modelElements[i]);
                _cellMediators.Add(element);
            }
            _newElementsPosition.Clear();
        }

        private void FinedStartPosition(CellData cell)
        {
            for (var k = 0; k < _level.StartElements.Length; k++)
            {
                //todo
                //if(_level.StartElements[k].IsActive == CellType.NotWorking)continue;
                if (_level.StartElements[k].Poz_X == cell.Poz_X)
                {
                    if (_newElementsPosition.ContainsKey(cell.Poz_X))
                    {
                        _newElementsPosition[cell.Poz_X]++;
                    }
                    else
                    {
                        _newElementsPosition.Add(cell.Poz_X, 0);
                    }
                    
                    cell.СoordinatePoz_X = _level.StartElements[k].СoordinatePoz_X;
                    cell.СoordinatePoz_Y = _level.StartElements[k].СoordinatePoz_Y + _newElementsPosition[cell.Poz_X] * 0.5f;
                    
                }
            }
        }

        private void MoveElements()
        {
            for (var i = 0; i < _cellMediators.Count; i++)
            {
                _cellMediators[i].Move(_level.Elements);
            }
        }

        private void OnDestroy()
        {
            _audio.StopSound(SoundManager.Enums.SoundId.JumperMusic);
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
