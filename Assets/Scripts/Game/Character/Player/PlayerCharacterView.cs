
using System;
using UnityEngine;

namespace Game.Character.Player
{
   public class PlayerCharacterView : CharacterView
    {
        [SerializeField] private Vector3 _cameraOffset;
        
        private Camera _camera;
       
        private void Start()
        {
            _camera = Camera.main;
        }
        
        protected override void CreateMediator()
        {
            _mediator = new PlayerCharacterMediator();
        }

        
        
        

        private void LateUpdate()
        {
            _camera.transform.position = transform.position + _cameraOffset;
        }
        public Camera Camera => _camera;

    }
}