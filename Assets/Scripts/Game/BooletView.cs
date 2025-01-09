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
        

        private void OnEnable()
        {
            Remove();
        }
        
        private async void Remove()
        {
            await Task.Delay(_removeTime);
            GameSceneManager.RemoveBoolet(gameObject);
        }

        private void OnDisable()
        {
            GameSceneManager.RemoveBoolet(gameObject);
        }

        private void FixedUpdate()
        {
            transform.Translate(Vector3.forward * _moveSpeed * Time.deltaTime);
        }
    }
}