
using Game;
using UnityEngine;

namespace ThreeInRowGame
{
    
    public class CellMediator: BaseGameMediator
    {
        public CellView CellView { get; set; }
        private CellType _cellType;
        
        private CellData Data => _data as CellData;


        public CellType CellType => _cellType;
       
        
        public void Mediate(CellView value)
        {
            CellView =  value;
        }
        
        public void SetData(CellData data)
        {
            CellView.Element.SetActive(true);
            CellView.Delete.SetActive(false);
            CellView.transform.position = new Vector3(data.СoordinatePoz_X, data.СoordinatePoz_Y, 0);
            //CellView.transform.position = new Vector3(1, -1, 0);
            _data = data;
            Data.State = ElementState.GetReadyToMove;
        }

        public void Move(GreedElementView[] positions)
        {
            Debug.Log($"P = {CellView.transform.position} LP = {CellView.transform.localPosition}");
            
            if (Data.State != ElementState.GetReadyToMove) return;
            
            for (var i = 0; i < positions.Length; i++)
            {
                if (positions[i].Poz_X == Data.Poz_X && positions[i].Poz_Y == Data.Poz_Y)
                {
                    // CellView.transform.position = new Vector3(positions[i].СoordinatePoz_X, positions[i].СoordinatePoz_Y, 0);
                    // Debug.Log($"P = {CellView.transform.position} LP = {CellView.transform.localPosition}");
                    if (positions[i].СoordinatePoz_X != Data.СoordinatePoz_X || positions[i].СoordinatePoz_Y != Data.СoordinatePoz_Y)
                    {
                        CellView.Move(positions[i]);
                        Data.State = ElementState.Move;
                        Data.СoordinatePoz_X = positions[i].СoordinatePoz_X;
                        Data.СoordinatePoz_Y = positions[i].СoordinatePoz_Y;
                    }
                }

            }
            
        }
        
        public void SetType(CellType type)
        {
            _cellType = type;
        }
    }
    
    public enum ElementState
    {
        New,
        Normal,
        GetReadyToMove,
        Move,
        Deleted,
        InPool
    }
}