using System;
using Cysharp.Threading.Tasks;
using Game;
using UnityEngine;

namespace ThreeInRowGame
{
    
    public class CellMediator: BaseGameMediator
    {
        public CellView CellView { get; private set; }
        public CellData Data => _data as CellData;
        
        public static event Action MoveCompletedEvent;

        
        public void Mediate(CellView value)
        {
            CellView =  value;
            CellView.MoveCompletedEvent += MoveCompleted;
        }
        
        public void SetData(CellData data)
        {
            CellView.Element.SetActive(true);
            CellView.Delete.SetActive(false);
            CellView.transform.position = new Vector3(data.СoordinatePoz_X, data.СoordinatePoz_Y, 0);
            _data = data;
            Data.State = ElementState.GetReadyToMove;
        }

        public bool Move(GreedElementView[] positions)
        {
            if (Data.State != ElementState.GetReadyToMove) return false;
            for (var i = 0; i < positions.Length; i++)
            {
                if (positions[i].Poz_X == Data.Poz_X && positions[i].Poz_Y == Data.Poz_Y)
                {
                    if (positions[i].СoordinatePoz_X != Data.СoordinatePoz_X || positions[i].СoordinatePoz_Y != Data.СoordinatePoz_Y)
                    {
                        CellView.Move(positions[i]);
                        Data.State = ElementState.Move;
                        Data.СoordinatePoz_X = positions[i].СoordinatePoz_X;
                        Data.СoordinatePoz_Y = positions[i].СoordinatePoz_Y;
                        return true;
                    }
                }

            }
            return false;
        }

        private void MoveCompleted()
        {
            Data.State = ElementState.GetReadyToMove;
            MoveCompletedEvent?.Invoke();
        }

        public async UniTask Delete()
        {
            Data.State = ElementState.Deleted;
            CellView.Element.SetActive(false);
            CellView.Delete.SetActive(true);
            await UniTask.Delay(1000 * CellView.DeleteTimeout);
            CellView.Delete.SetActive(false);
            CellView.gameObject.SetActive(false);
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