
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Character.Player
{
   public class PlayerCharacterView : CharacterView
    {

        
        [SerializeField] private float _camSpeedPosition = 3;
        [SerializeField] private float _camSpeedRotation = 5;
        
        private Camera _camera;
        private Transform _cameraTransform;
        private void Start()
        {
            _camera = Camera.main;
            _cameraTransform = _camera.gameObject.transform.parent.transform;
        }
        
        protected override void CreateMediator()
        {
            _mediator = new PlayerCharacterMediator();
        }

        private void LateUpdate()
        {
            var newPosition = Vector3.Lerp(_cameraTransform.position,
                transform.position,
                _camSpeedPosition * Time.deltaTime);
            
            _cameraTransform.position = newPosition;
            
            var newRotation = Quaternion.Lerp(_cameraTransform.rotation,
                transform.rotation,
                _camSpeedRotation * Time.deltaTime);
            
            _cameraTransform.rotation = newRotation;
        }
        public Camera Camera => _camera;

        
    }
}