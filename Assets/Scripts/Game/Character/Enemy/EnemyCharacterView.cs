
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Character.Enemy
{
    public class EnemyCharacterView : GameCharacterView
    {
        [SerializeField] private float _lives = 3;
        [SerializeField] private float _boollets = 10;
        [SerializeField] private float _detectedRadius = 10;
        [SerializeField] private float _detectedDistance = 2;
        [SerializeField] private int _pauzeTime = 2000;
        
        private Vector3 _target;
        private bool _isPauze;
        private void Start()
        {
            base.Start();
        }
        public void Update()
        {
           
            if (LastObject == null)
            {
                var xs = 25;
                var xz = 25;
                
                _target = new Vector3(Random.Range(-1 * xs, xs), 0, Random.Range(-1 * xz, xz));
                GoToTarget(_target, GameSceneManager.EnvironmentView.Environment);
            }
            else
            {
                if(CharacterState ==  CharacterStateType.Run || CharacterState ==  CharacterStateType.Idle)
                {
                    RaycastHit hit;
                    
                    if (Physics.SphereCast(transform.position, _detectedRadius, transform.forward, out hit, _detectedDistance, LayerMask))
                    {
                        Debug.Log($"Враг {hit.collider.name} в зоне обнаружения!");
                        TakeAim(hit.point, hit.collider.gameObject);
                        
                    }
                    else
                    {
                        if(NavMeshAgent.remainingDistance < 0.1F && !_isPauze)
                        {
                            Pauze();
                        }
                    }
                }
                else if(CharacterState ==  CharacterStateType.TakeAim)
                {
                    Ray ray = new Ray(WeaponAttachment.position, transform.forward); 
                    RaycastHit hit; // Данные столкновения

                    Debug.DrawRay(transform.position, transform.forward * _detectedRadius*2, Color.red); 
                    if (Physics.Raycast(ray, out hit, _detectedRadius*2, LayerMask)) 
                    {
                        Debug.Log("Попали в: " + hit.collider.name);
                    }
                }
                
                
                
            }
            base.Update();
        }

        private async UniTask Pauze()
        {
            _isPauze = true;
            await UniTask.Delay(_pauzeTime); 
            LastObject = null;
            _isPauze = false;
        }
    }
}