using System;
using Managers.SceneManagers;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Game.Character
{
    public abstract class CharacterView : MonoBehaviour
    {
        [SerializeField] protected Animator _animator;
        
        public Animator Animator => _animator;
        
        public virtual void SetData(CharacterData data)
        {
            transform.position = data.transform.position;  
        }
        
        public virtual void Remove()
        {
        }

        public void OnDisable()
        {
            
        }
    }
}