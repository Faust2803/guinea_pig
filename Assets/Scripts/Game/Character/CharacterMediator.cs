using System;
using DG.Tweening;
using UnityEngine;

namespace Game.Character
{
    public class CharacterMediator : BaseGameMediator
    {
        public CharacterView CharacterView { get; set; }
        
        private CharacterType _characterType;
        
        
        public CharacterType CharacterType => _characterType;
        public float CharacterMoveSpeed { get; set; }

        protected CharacterStateType CharacterState { get; set; }

        public virtual void Mediate(CharacterView value)
        {
            CharacterView =  value;
            CharacterState = CharacterStateType.Idle;
        }
        
        public virtual void SetData(object data)
        {
            _data = data;
        }
        
        public void SetType(CharacterType type)
        {
            _characterType = type;
        }

        public virtual void GameLifeСycle()
        {
        }
        
        public virtual void Remove()
        {
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