using System;
using UnityEngine;

namespace Game.Character.Enemy
{
    public class EnemyDetector : MonoBehaviour
    {
        public CharacterView CharacterView { get; set; }

       private float _radius;
        public void SetDetectedRadius(float radius)
        {
            _radius = radius;
        }

        private void Start()
        {
            transform.localPosition = Vector3.forward * _radius;
            transform.localScale = Vector3.one * _radius*2;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                CharacterView = other.GetComponent<CharacterView>();
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                CharacterView = null;
            }
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _radius);
            
        }
    }
}