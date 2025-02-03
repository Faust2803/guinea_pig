using System;
using System.Threading.Tasks;
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

        private void OnEnable()
        {
            _speed = _moveSpeed * Time.fixedDeltaTime;
            Create();
        }
        
        private async void Create()
        {
            await Task.Delay(_removeTime);
            Remove();
        }
        
        public void Remove()
        {
            GameSceneManager.RemoveBoolet(gameObject);
        }
        
        private void FixedUpdate()
        {
            transform.Translate(Vector3.forward * _speed);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            //if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Player")
            {
                GameSceneManager.RemoveBoolet(gameObject);
            }
        }
    }
}