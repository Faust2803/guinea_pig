
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
            base.Update();
            switch (CharacterState)
            {
                case CharacterStateType.Run :
                case CharacterStateType.Idle :
                    //Debug.Log("RI");
                    if (_canShoot && _enemyDetector.CharacterView != null)
                    {
                        //Debug.Log("RI CS");
                        if(CanSeeEnemy(transform, _enemyDetector.CharacterView.transform))
                        {
                            //Debug.Log("RI CS TA");
                            TakeAim(_enemyDetector.CharacterView.transform.position, _enemyDetector.CharacterView.gameObject);
                            _cts?.Cancel();
                        }
                    }
                    else if (LastObject == null)
                    {
                        //Debug.Log("RI LO = null");
                        TakeNewPoint();
                    }
                    else if(NavMeshAgent.remainingDistance < 0.1F && _cts == null)
                    {
                        //Debug.Log("RI new point");
                        _cts = new CancellationTokenSource(); 
                        Pause(_cts.Token).Forget();
                    }
                    return;
                case CharacterStateType.TakeAim:
                    Debug.Log("TA");
                    RotateToAim();
                    if (_canShoot && CanSeeEnemy(transform))
                    {
                        Debug.Log("TA CS");
                        Fire();
                    }
                    else if (_enemyDetector.CharacterView == null)
                    {
                        TakeNewPoint();
                    }
                    return;
                case CharacterStateType.Fire:
                    //Debug.Log("F");
                    if (_enemyDetector.CharacterView != null)
                    {
                        RotateToAim();
                    }
                    else
                    {
                        TakeNewPoint();
                    }
                    return;
                case CharacterStateType.FireCompleated:
                    //Debug.Log("FC");
                    ShootingPause().Forget();
                    SelectAfterSootingAction();
                    return;
                case CharacterStateType.Hit:
                case CharacterStateType.Death:
                case CharacterStateType.Reload:
                    //Debug.Log("Other");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void TakeNewPoint()
        {
            var target = new Vector3(Random.Range(-1 * GameSceneManager.EnvironmentView.EnemySpaseX, GameSceneManager.EnvironmentView.EnemySpaseX), 0,
                Random.Range(-1 * GameSceneManager.EnvironmentView.EnemySpaseZ, GameSceneManager.EnvironmentView.EnemySpaseZ));
            GoToTarget(target, GameSceneManager.EnvironmentView.Environment);
            _cts = null;
        }
        
        private bool CanSeeEnemy(Transform observer, Transform target = null)
        {
            var direction = transform.forward;
            if (target != null)
            {
                direction = (target.position - observer.position + _offset).normalized;
            }
            
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
                if (Vector3.Distance(_enemyDetector.CharacterView.transform.position, transform.position) < _minDistance && _enemyDetector.CharacterView != null)
                {
                    //TakeAim(_enemyDetector.CharacterView.transform.position, _enemyDetector.CharacterView.gameObject);
                    SetState(CharacterStateType.TakeAim);
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