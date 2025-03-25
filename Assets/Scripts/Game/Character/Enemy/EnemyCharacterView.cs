
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Character.Enemy
{
    public class EnemyCharacterView : GameCharacterView
    {
        [Space]
        [SerializeField] private float _lives = 3;
        [SerializeField] private float _boollets = 10;
        [Space]
        [SerializeField] private float _detectedRadius = 10;
        [SerializeField] private int _pauzeTime = 2000;
        [SerializeField] private EnemyDetector _enemyDetector;
        [SerializeField] private Vector3 _offset;
        
        
        private Vector3 _target;
        private CancellationTokenSource _cts;
        

        private void Awake()
        {
            _enemyDetector.SetDetectedRadius(_detectedRadius);
        }
        private void Start()
        {
            base.Start();
        }

        private void OnDrawGizmos()
        {
            if (CharacterState == CharacterStateType.TakeAim)
            {
                // Gizmos.color = Color.red;
                // Gizmos.DrawLine(transform.position, transform.position * _detectedRadius);
            }
            //if (CharacterState == CharacterStateType.Run || CharacterState == CharacterStateType.Idle)
            {
                
                if (_enemyDetector.CharacterView == null)
                {
                    return;
                }
                Gizmos.color = Color.green;
                var direction = _enemyDetector.CharacterView.transform.position - transform.position + _offset;
                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }
                
                Gizmos.DrawLine(transform.position + _offset, direction * _detectedRadius);
                
            }
        }

        public void Update()
        {
            if (LastObject == null)
            {
                _target = new Vector3(Random.Range(-1 * GameSceneManager.EnvironmentView.EnemySpaseX, GameSceneManager.EnvironmentView.EnemySpaseX), 0,
                    Random.Range(-1 * GameSceneManager.EnvironmentView.EnemySpaseZ, GameSceneManager.EnvironmentView.EnemySpaseZ));
                GoToTarget(_target, GameSceneManager.EnvironmentView.Environment);
                _cts = null;
            }
            else
            {
                if (_enemyDetector.CharacterView != null)
                {
                    if (CharacterState == CharacterStateType.Run || CharacterState == CharacterStateType.Idle)
                    {
                        var direction = _enemyDetector.CharacterView.transform.position - transform.position + _offset;
                        
                        if (Physics.Raycast(transform.position +_offset, direction.normalized, out RaycastHit hit, _detectedRadius*2))
                        {
                            if (!hit.transform != _enemyDetector.CharacterView.transform) 
                            {
                                Debug.Log("Между объектами нет препятствие: " + hit.collider.name);
                                TakeAim(_enemyDetector.CharacterView.transform.position, _enemyDetector.CharacterView.gameObject);
                                _cts?.Cancel();
                            }
                        }
                    }
                } 
                else if(NavMeshAgent.remainingDistance < 0.1F && _cts == null)
                {
                    _cts = new CancellationTokenSource(); 
                    Pauze(_cts.Token).Forget();
                }
            }
            base.Update();
        }

        private async UniTask Pauze(CancellationToken token)
        {
            try
            {
                await UniTask.Delay(_pauzeTime, cancellationToken: token); 
                LastObject = null;
            }
            catch (OperationCanceledException)
            {
                
            }
        }
    }
}