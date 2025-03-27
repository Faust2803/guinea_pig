
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

        private void OnEnable()
        {
            NavMeshAgent.enabled = true;
        }

        public  void Update()
        {
            base.Update();
            if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                RaycastHit hit;
                //if (Physics.Raycast(Camera.ScreenPointToRay(Input.mousePosition), out hit, 100, LayerMask))
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

        public void OnDestroy()
        {
            BottomGamePanelMediator.OnFire -= Fire;
            BottomGamePanelMediator.OnReload -= Reload;
        }
    }
}