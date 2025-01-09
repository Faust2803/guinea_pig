using System.Collections.Generic;
using UnityEngine;

namespace ThreeInRowGame
{
    public class LevelView : MonoBehaviour
    {
        
        [SerializeField] private GreedElementView[] _elements = new GreedElementView[64];
        [SerializeField] private GreedElementView[] _startElements = new GreedElementView[8];
        [SerializeField] private List<CellType> _spaunElementType;
        

        public GreedElementView[] Elements => _elements;
        public GreedElementView[] StartElements => _startElements;
        public List<CellType> SpaunElementType => _spaunElementType;
        


    }
}