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
        protected float Lives { get; set;}
        protected float Boollets { get; set;}
        
        public virtual void SetData(CharacterData data)
        {
            transform.position = data.transform.position;
            Lives = data.lifes;
            Boollets = data.boollets;
        }
        
        public virtual void Remove()
        {
        }
    }
}