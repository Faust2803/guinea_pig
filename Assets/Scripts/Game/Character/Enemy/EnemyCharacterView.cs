
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
        [SerializeField] private int _shootingPauzeTime = 1000;
        [SerializeField] private EnemyDetector _enemyDetector;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _minDistance = 4;
        
        
        private bool _canShoot = true;
        private CancellationTokenSource _cts;
        

        private void Awake()
        {
            base.Awake();
            _enemyDetector.SetDetectedRadius(_detectedRadius);
        }
        private void Start()
        {
            base.Start();
        }

        private void OnDrawGizmos()
        {
            if (_enemyDetector.CharacterView == null)return;
            Gizmos.color = Color.green;
            var direction = _enemyDetector.CharacterView.transform.position - transform.position + _offset;
            if (direction == Vector3.zero)
            {
                direction = transform.forward;
            }
            Gizmos.DrawLine(transform.position + _offset, direction * _detectedRadius);
        }

        public void Update()
        {
            if (LastObject == null)
            {
                var target = new Vector3(Random.Range(-1 * GameSceneManager.EnvironmentView.EnemySpaseX, GameSceneManager.EnvironmentView.EnemySpaseX), 0,
                    Random.Range(-1 * GameSceneManager.EnvironmentView.EnemySpaseZ, GameSceneManager.EnvironmentView.EnemySpaseZ));
                GoToTarget(target, GameSceneManager.EnvironmentView.Environment);
                _cts = null;
            }
            else
            {
                if (_canShoot && _enemyDetector.CharacterView != null)
                {
                    if (CharacterState == CharacterStateType.Run || CharacterState == CharacterStateType.Idle)
                    {
                        
                        if(IsTernedToEnemy(transform, _enemyDetector.CharacterView.transform))
                        {
                            TakeAim(_enemyDetector.CharacterView.transform.position, _enemyDetector.CharacterView.gameObject);
                            _cts?.Cancel();
                        }
                    }
                } 
                else if(NavMeshAgent.remainingDistance < 0.1F && _cts == null)
                {
                    _cts = new CancellationTokenSource(); 
                    Pause(_cts.Token).Forget();
                }
                
            }
            base.Update();
            if (_canShoot && CharacterState == CharacterStateType.TakeAim && IsLookingAtTarget(transform, _enemyDetector.CharacterView.transform))
            {
                Fire();
            }
            else if (CharacterState == CharacterStateType.FireCompleated)
            {
                ShootingPause().Forget();
                SelectAfterSootingAction();
            }
        }
        
        private bool IsLookingAtTarget(Transform observer, Transform target, float minDot = 1f)
        {
            var toTarget = (target.position - observer.position).normalized;
            var dot = Vector3.Dot(observer.forward, toTarget);
            return dot >= minDot; 
        }

        private bool IsTernedToEnemy(Transform observer, Transform target)
        {
            var direction = (target.position - observer.position + _offset).normalized;
            if (Physics.Raycast(observer.position +_offset, direction.normalized, out RaycastHit hit, _detectedRadius*2))
            {
                if (hit.collider.tag ==  "Player") 
                {
                    return true;
                }
            }
            return false;
        }

        private void SelectAfterSootingAction()
        {
            if (_enemyDetector.CharacterView != null)
            {
                if (Vector3.Distance(_enemyDetector.CharacterView.transform.position, transform.position) < _minDistance)
                {
                    TakeAim(_enemyDetector.CharacterView.transform.position, _enemyDetector.CharacterView.gameObject);
                }
                else
                {
                    GoToTarget(_enemyDetector.CharacterView.transform.position, _enemyDetector.CharacterView.gameObject);
                }
                
                
            }
            else
            {
                LastObject = null;
            }
        }
        
        

        private async UniTask Pause(CancellationToken token)
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
        
        private async UniTask ShootingPause()
        {
            try
            {
                _canShoot = false;
                await UniTask.Delay(_shootingPauzeTime); 
                _canShoot = true;
            }
            catch (OperationCanceledException)
            {
                
            }
        }
    }
}