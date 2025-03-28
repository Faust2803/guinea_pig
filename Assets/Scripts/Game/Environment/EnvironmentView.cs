using System.Collections.Generic;
using Game.Character;
using UnityEngine;

namespace Game.Environment
{
    public class EnvironmentView : MonoBehaviour
    {
        [SerializeField] private Transform _playerSpawnPoint;
        [Space]
        [SerializeField] private List<Transform> _spawnPoint;
        [SerializeField] private List<CharacterType> _enemyType;
        [SerializeField] private GameObject _environment;
        [Space]
        [SerializeField] private float _enemySpaseX = 25;
        [SerializeField] private float _enemySpaseZ = 25;
        public List<Transform> SpawnPoint => _spawnPoint;
        public Transform PlayerSpawnPoint => _playerSpawnPoint;
        public GameObject Environment => _environment;
        public float EnemySpaseX => _enemySpaseX;
        public float EnemySpaseZ => _enemySpaseZ;
        public List<CharacterType> EnemyType => _enemyType;

    }
}