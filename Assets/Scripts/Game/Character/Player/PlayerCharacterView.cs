using System;
using UI.Panels.BottomGamePanel;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace Game.Character.Player
{
   public class PlayerCharacterView : GameCharacterView
    {
        [SerializeField] private float _camSpeedPosition = 3;
        [SerializeField] private float _camSpeedRotation = 5;
        [Space]
        [SerializeField] private Transform _dathPosition;
        [SerializeField] private Transform _victoryPosition;
        
        private Camera _camera;
        private Transform _cameraTransform;
        
        public Camera Camera => _camera;
        
        private void Start()
        {
            _camera = Camera.main;
            _cameraTransform = _camera.gameObject.transform.parent.transform;
            BottomGamePanelMediator.OnFire += Fire;
            BottomGamePanelMediator.OnReload += Reload;
            base.Start();
        }

        public  void Update()
        {
            base.Update();
            switch (CharacterState)
            {
                case CharacterStateType.TakeAim:
                    RotateToAim();
                    break;
                case CharacterStateType.Fire:
                    if (LastObject.tag == "Enemy")
                    {
                        RotateToAim();
                    }
                    break;
                case CharacterStateType.Death:
                case CharacterStateType.Victory:   
                    return;
                case CharacterStateType.Run :
                case CharacterStateType.Idle :
                case CharacterStateType.FireCompleated:
                case CharacterStateType.Hit:
                case CharacterStateType.Reload:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.ScreenPointToRay(Input.mousePosition), out hit, 100))
                {
                    if(hit.collider.gameObject.tag == "Enemy")
                    {
                        TakeAim(hit.point, hit.collider.gameObject);
                    }
                    else
                    {
                        if (hit.collider.gameObject.tag == "Ground")
                        {
                            GoToTarget(hit.point, hit.collider.gameObject);
                        }
                    }
                }
            }
        }
        
        public override void TurnOffDeathPlayer(GameCharacterView character, int emainingActiveEnemy)
        {
            base.TurnOffDeathPlayer(character, emainingActiveEnemy);
            if (emainingActiveEnemy == 0 && Lives > 0)
            {
                Victory();
            }
        }

        protected override void Dad()
        {
            base.Dad();
            BottomGamePanelMediator.OnFire -= Fire;
            BottomGamePanelMediator.OnReload -= Reload;
        }

        private void LateUpdate()
        {
            var position = transform.position;
            var rotation = transform.rotation;

            switch (CharacterState)
            {
                case CharacterStateType.Death:
                    position = _dathPosition.position;
                    rotation = _dathPosition.rotation;   
                    break;
                case CharacterStateType.Victory:
                    position = _victoryPosition.position;
                    rotation = _victoryPosition.rotation; 
                    break;
            }
          
            var newPosition = Vector3.Lerp(_cameraTransform.position, position, _camSpeedPosition * Time.deltaTime);
            _cameraTransform.position = newPosition;
            
            var newRotation = Quaternion.Lerp(_cameraTransform.rotation, rotation, _camSpeedRotation * Time.deltaTime);
            _cameraTransform.rotation = newRotation;
        }

        private void OnDestroy()
        {
            BottomGamePanelMediator.OnFire -= Fire;
            BottomGamePanelMediator.OnReload -= Reload;
        }
    }
}