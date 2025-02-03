using System.Collections.Generic;
using Managers.SceneManagers;
using UnityEngine;

namespace ThreeInRowGame
{
    public class ThreeInRowModel
    {
        private CellType[,] _elements = new CellType[ThreeInRowGameSceneManager.GREED_SIZE,ThreeInRowGameSceneManager.GREED_SIZE];
        private List<CellType> _spaunElementType;
        private GreedElementView[] _levelGreed;
        
        public CellType[,] Elements => _elements;
        
        public ThreeInRowModel(GreedElementView[] levelGreed, List<CellType> spaunElementType)
        {
            _spaunElementType = spaunElementType;
            _levelGreed = levelGreed;
            for (var i = 0; i < _levelGreed.Length; i++)
            {
                var element = _levelGreed[i];
                _elements[element.Poz_X, element.Poz_Y] = element.IsActive;
            }
        }

        public List<CellData> AddNewElements()
        {
            var rnd = new System.Random();
            var newElements = new List<CellData>();
            for (var j = ThreeInRowGameSceneManager.GREED_SIZE - 1; j > -1; j--)
            {
                for (var i = 0; i < ThreeInRowGameSceneManager.GREED_SIZE; i++)
                {
                    if (_elements[i,j] == CellType.Empty)
                    {
                        _elements[i,j] =  _spaunElementType[rnd.Next(0, _spaunElementType.Count)];
                        var newElement = new CellData() { Poz_X = i, Poz_Y = j, Type = _elements[i, j] };
                        //Debug.Log($"x = {newElement.Poz_X} y = {newElement.Poz_Y} type = {newElement.Type}");
                        FinedMovementQueue(newElement);
                        newElements.Add(newElement);
                    }
                }
            }
            return newElements;
        }

        private void FinedMovementQueue(CellData data)
        {
            var k = data.Poz_X;
            
            var coordinate = new Vector2(data.Poz_X, data.Poz_Y);
            data.MovementStack.Push(coordinate);
            data.CoordinateStack.Push(SetMoveCoordinate(coordinate));
            
            //Debug.Log($"movementQueue {data.Poz_X} {data.Poz_Y} q ={data.Poz_X},{data.Poz_Y}");
            if (data.Poz_Y > 0)
            {
                for (var i = data.Poz_Y - 1; i > -1; i--)
                {
                    var finedMove = false;
                    if (_elements[k, i] == CellType.Empty)
                    {
                        finedMove = true;
                    }
                    else
                    {
                        if (k == 0)
                        {
                            if (_elements[k + 1, i] == CellType.Empty)
                            {
                                k++;
                                finedMove = true;
                            }
                        }
                        else if (k == ThreeInRowGameSceneManager.GREED_SIZE - 1)
                        {
                            if (_elements[k - 1, i] == CellType.Empty)
                            {
                                k--;
                                finedMove = true;
                            }
                        }
                        else
                        {
                            if (_elements[k - 1, i] == CellType.Empty)
                            {
                                k--;
                                finedMove = true;
                            }
                            else
                            {
                                if (_elements[k + 1, i] == CellType.Empty)
                                {
                                    k--;
                                    finedMove = true;
                                }
                            }
                        }
                    }
                    if (finedMove)
                    {
                        var v = new Vector2(k, i);
                        data.MovementStack.Push(v);
                        data.CoordinateStack.Push(SetMoveCoordinate(v));
                        //Debug.Log($"movementQueue {data.Poz_X} {data.Poz_Y} q ={k},{i}");
                    }
                    else
                    {
                        //Debug.Log("________");
                        return;
                    }
                }
            }
            //Debug.Log("________");
        }
        
        private Vector3 SetMoveCoordinate(Vector2 position)
        {
            for (var i = 0; i < _levelGreed.Length; i++)
            {
                if (_levelGreed[i].Poz_X == (int)position.x && _levelGreed[i].Poz_Y == (int)position.y)
                {
                    return new Vector3(_levelGreed[i].СoordinatePoz_X, _levelGreed[i].СoordinatePoz_Y, 0);
                }
            }
            return Vector3.zero;
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

            for (var i = 0; i < result.Count; i++)
            {
                _elements[(int)result[i].x, (int)result[i].y] = CellType.Empty;
            }
            return result;
        }
        
        
    }
}