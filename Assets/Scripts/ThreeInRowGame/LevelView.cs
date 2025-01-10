using System;
using System.Collections.Generic;
using Managers.SceneManagers;
using UnityEngine;

namespace ThreeInRowGame
{
    public class LevelView : MonoBehaviour
    {
        
        [SerializeField] private GreedElementView[] _elements = new GreedElementView[ThreeInRowGameSceneManager.GREED_SIZE * ThreeInRowGameSceneManager.GREED_SIZE];
        [SerializeField] private GreedElementView[] _startElements = new GreedElementView[ThreeInRowGameSceneManager.GREED_SIZE];
        [SerializeField] private List<CellType> _spaunElementType;
        [SerializeField] private Transform  _gameArea;
        [SerializeField] private int _moves = 10;
        

        public GreedElementView[] Elements => _elements;
        public int Moves => _moves;
        public GreedElementView[] StartElements => _startElements;
        public List<CellType> SpaunElementType => _spaunElementType;
        public Transform GameArea => _gameArea;
        


    }
}