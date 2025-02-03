using System.Collections.Generic;
using UnityEngine;

//using System.Numerics;

namespace ThreeInRowGame
{
    public class CellData
    {
        public int Poz_X;
        public int Poz_Y;
        public float СoordinatePoz_X;
        public float СoordinatePoz_Y;
        public CellType Type;
        public ElementState State;
        public Stack<Vector2> MovementStack = new Stack<Vector2>();
        public Stack<Vector3> CoordinateStack = new Stack<Vector3>();
        public int MoveDelay;

    }
}