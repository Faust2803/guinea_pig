using UnityEngine;

namespace ThreeInRowGame
{
    public class GreedElementView : MonoBehaviour
    {
        
        [SerializeField] private int _x;
        [SerializeField] private int _y;

        public int Poz_X => _x;
        public int Poz_Y => _y;
        public float СoordinatePoz_X => transform.position.x;
        public float СoordinatePoz_Y => transform.position.y;
        public CellType IsActive {
            get
            {
                if (gameObject.activeSelf)
                {
                    return CellType.Empty;
                }
                else
                {
                    return CellType.NotWorking;
                }
            }
        } 
        
    }
}