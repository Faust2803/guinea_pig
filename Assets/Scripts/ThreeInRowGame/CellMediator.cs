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
            _data = data;
            CellView.Element.SetActive(false);
            CellView.Delete.SetActive(false);
            Data.State = ElementState.New;
        }

        public async UniTask ReadyToMove()
        {
            Data.State = ElementState.GetReadyToMove;
            await UniTask.Delay(Data.MoveDelay * 200);
            Data.State = ElementState.Move;
            CellView.transform.position = new Vector3(Data.СoordinatePoz_X, Data.СoordinatePoz_Y, 0);
            CellView.Element.SetActive(true);
            CellView.Move(Data.CoordinateStack);
        }

        private void MoveCompleted()
        {
            Data.State = ElementState.GetReadyToMove;
            Data.MoveDelay = 0;
            // Data.CoordinateStack.Clear();
            Data.MovementStack.Clear();
            Data.СoordinatePoz_X = CellView.transform.position.x;
            Data.СoordinatePoz_Y = CellView.transform.position.y;
            MoveCompletedEvent?.Invoke();
        }

        public async UniTask Delete()
        {
            Data.State = ElementState.Deleted;
            CellView.Element.SetActive(false);
            CellView.Delete.SetActive(true);
            await UniTask.Delay(500);
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