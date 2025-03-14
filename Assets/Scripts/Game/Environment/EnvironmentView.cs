using System.Collections.Generic;
using Game.Character;
using UnityEngine;

namespace Game.Environment
{
    public class EnvironmentView : MonoBehaviour
    {
        [SerializeField] private List<Transform> _spawnPoint;
        [SerializeField] private List<CharacterType> _enemyType;
        [SerializeField] private GameObject _environment;

        public List<Transform> SpawnPoint => _spawnPoint;
        public GameObject Environment => _environment;
        public List<CharacterType> EnemyType => _enemyType;

    }
}