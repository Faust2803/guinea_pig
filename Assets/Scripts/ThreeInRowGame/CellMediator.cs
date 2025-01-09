
using Game;

namespace ThreeInRowGame
{
    public class CellMediator : BaseGameMediator
    {
        public CellView CellView { get; set; }
        private CellType _cellType;
        public CellType CellType => _cellType;
       
        
        public void Mediate(CellView value)
        {
            CellView =  value;
        }
        
        public void SetData(object data)
        {
            _data = data;
        }
        
        public void SetType(CellType type)
        {
            _cellType = type;
        }
    }
}