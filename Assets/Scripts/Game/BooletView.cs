using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Managers.SceneManagers;
using UnityEngine;
using Zenject;

namespace Game
{
    public class BooletView : MonoBehaviour
    {
        [Inject] public GameSceneManager GameSceneManager;
        [SerializeField] protected Collider _collider;
        [SerializeField] protected float _moveSpeed;
        [SerializeField] protected int _removeTime = 5000;

        private float _speed;
        private bool _hit;
        private CancellationTokenSource _cts;

        private void OnEnable()
        {
            _speed = _moveSpeed * Time.fixedDeltaTime;
            _cts = new CancellationTokenSource(); 
            Create(_cts.Token).Forget();
        }
        
        private async UniTask Create(CancellationToken token)
        {
            try
            {
                await UniTask.Delay(_removeTime, cancellationToken: token);
                Remove();
            }
            catch (OperationCanceledException)
            {
                
            }
            
        }
        
        private void Remove()
        {
            GameSceneManager.RemoveBoolet(gameObject);
            _cts.Cancel();
        }
        
        private void FixedUpdate()
        {
            transform.Translate(Vector3.forward * _speed);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.name != "EnemyDetector" && other.tag != "Boolet")
            {
                Remove();
            }
            
        }
    }
}