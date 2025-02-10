using System.Collections.Generic;
using Game.Character;
using UnityEngine;

namespace Game.Environment
{
    public class EnvironmentView : MonoBehaviour
    {
        [SerializeField] private List<Transform> _spawnPoint;
        [SerializeField] private List<CharacterType> _enemyType;

        public List<Transform> SpawnPoint => _spawnPoint;
        public List<CharacterType> EnemyType => _enemyType;

    }
}