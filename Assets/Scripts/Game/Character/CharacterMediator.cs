using System;
using DG.Tweening;
using UnityEngine;

namespace Game.Character
{
    public class CharacterMediator : BaseGameMediator
    {
        public CharacterView CharacterView { get; set; }
        
        private CharacterType _characterType;
        
        public CharacterType CharacterType
        {
            get { return _characterType; }
        }

        public virtual void Mediate(CharacterView value)
        {
            CharacterView =  value;
        }
        
        public virtual void SetData(object data)
        {
            _data = data;
        }
        
        public void SetType(CharacterType type)
        {
            _characterType = type;
        }
    }
    
    public abstract class CharacterMediator<T, Z> : CharacterMediator where T : CharacterView where Z : CharacterData
    {
        public T Target
        {
            get { return CharacterView as T; }
        }
    
        public Z Data
        {
            get { return _data as Z; }
        }
    }
}