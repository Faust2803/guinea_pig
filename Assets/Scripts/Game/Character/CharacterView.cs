using System;
using UnityEngine;

namespace Game.Character
{
    public abstract class CharacterView : MonoBehaviour
    {
        [SerializeField] protected Animator _animator;
        
        protected Animator Animator => _animator;
        public int Lives { get; set;}
        protected float Boollets { get; set;}
        public CharacterType CharacterType { get; set;}
        
        public event Action OnDeathEnemy;
        public event Action OnLostLife;
        
        public virtual void SetData(CharacterData data)
        {
            transform.position = data.transform.position;
            CharacterType = data.type;
            Lives = data.lifes;
            Boollets = data.boollets;
        }

        public virtual void TurnOffDeathPlayer(GameCharacterView character, int emainingActiveEnemy)
        {
            OnDeathEnemy?.Invoke();
        }
        
        protected virtual void LostLife()
        {
            OnLostLife?.Invoke();
        }
    }
}