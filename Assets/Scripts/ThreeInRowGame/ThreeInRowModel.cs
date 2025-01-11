using System.Collections.Generic;
using Managers.SceneManagers;
using UnityEngine;

namespace ThreeInRowGame
{
    public class ThreeInRowModel
    {
        private CellType[,] _elements = new CellType[ThreeInRowGameSceneManager.GREED_SIZE,ThreeInRowGameSceneManager.GREED_SIZE];
        private List<CellType> _spaunElementType;
        
        public CellType[,] Elements => _elements;
        
        public ThreeInRowModel(GreedElementView[] levelGreed, List<CellType> spaunElementType)
        {
            _spaunElementType = spaunElementType;
            for (var i = 0; i < levelGreed.Length; i++)
            {
                var element = levelGreed[i];
                _elements[element.Poz_X, element.Poz_Y] = element.IsActive;
            }
        }

        public List<CellData> AddNewElements()
        {
            var rnd = new System.Random();
            var newElements = new List<CellData>();
            for (var i = 0; i < ThreeInRowGameSceneManager.GREED_SIZE; i++)
            {
                for (var j = ThreeInRowGameSceneManager.GREED_SIZE - 1; j > -1; j--)
                {
                    if (_elements[i,j] == CellType.Empty)
                    {
                        _elements[i,j] =  _spaunElementType[rnd.Next(0, _spaunElementType.Count)];
                        var newElement = new CellData() { Poz_X = i, Poz_Y = j, Type = _elements[i, j] };
                        //Debug.Log($"x = {newElement.Poz_X} y = {newElement.Poz_Y} type = {newElement.Type}");
                        newElements.Add(newElement);
                    }
                    
                }
            }
            return newElements;
        }

        public Dictionary<int, Vector3> FinedMath()
        {
            var result = new Dictionary<int, Vector3>();
            for (var i = 0; i < ThreeInRowGameSceneManager.GREED_SIZE; i++)
            {
                for (var j = 0; j < ThreeInRowGameSceneManager.GREED_SIZE - 2; j++)
                {
                    if (_elements[i, j] != CellType.Empty && _elements[i, j] != CellType.NotWorking )
                    {
                        if (_elements[i, j] == _elements[i, j + 1] && _elements[i, j] == _elements[i, j + 2])
                        {
                            for (var k = j; k < j + 3; k++)
                            {
                                var element = new Vector3(i, k, (float)_elements[i, k]);
                                if (!result.ContainsKey(element.GetHashCode()))
                                {
                                    result.Add(element.GetHashCode(), element);
                                }
                            }
                        }
                    }
                }
            }
            for (var j = 0; j < ThreeInRowGameSceneManager.GREED_SIZE; j++)
            {
                for (var i = 0; i < ThreeInRowGameSceneManager.GREED_SIZE - 2; i++)
                {
                    if (_elements[i, j] != CellType.Empty && _elements[i, j] != CellType.NotWorking )
                    {
                        if (_elements[i, j] == _elements[i + 1, j] && _elements[i, j] == _elements[i + 2, j])
                        {
                            for (var k = i; k < i + 3; k++)
                            {
                                var element = new Vector3(k, j, (float)_elements[k, j]);
                                if (!result.ContainsKey(element.GetHashCode()))
                                {
                                    result.Add(element.GetHashCode(), element);
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
    }
}