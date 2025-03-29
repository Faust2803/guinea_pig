using System;
using UnityEngine;

namespace Game.Character
{
    [Serializable]
    public class CharacterData
    {
        public Transform transform;
        public CharacterType type;
        public int lifes = 3;
        public int boollets = 10;
    }
}