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

        public List<Vector2> FinedMath()
        {
            var result = new List<Vector2>();

            return result;
        }
    }
}