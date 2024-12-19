using System;
using Managers.SceneManagers;
using UnityEngine;
using Zenject;

namespace Game
{
    public class BooletView : MonoBehaviour
    {
        [Inject] protected GameBaseSceneManager _gameBaseSceneManager;
        [SerializeField] protected Collider _collider;
        [SerializeField] protected float _moveSpeed;
        

        private void OnEnable()
        {
            throw new NotImplementedException();
        }

        private void OnDisable()
        {
            _gameBaseSceneManager.RemoveBoolet(gameObject);
        }

        private void FixUpdate()
        {
            transform.position += Vector3.forward * Time.deltaTime* _moveSpeed;
        }
    }
}