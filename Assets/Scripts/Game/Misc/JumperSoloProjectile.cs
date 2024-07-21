using UnityEngine;

namespace Game.Jumper
{
    public class JumperSoloProjectile : MonoBehaviour
    {
        [HideInInspector]
        public string OwnerTypeName;

        [Range(1, 5)]
        public int Damage = 1;

        [SerializeField] float maxTimelife = 15;
        [SerializeField] float speedMove = 2f;
        [SerializeField] float detectionRadius = 0.5f;
        [SerializeField] LayerMask scanMask;

        [SerializeField] float delayBeforeStart;

        private Transform selectedTarget;
        private GameObject owner;
        private float spawnTime;
        private float timelife =>
            Time.time - spawnTime;

        public float DelayBeforeStart =>
            delayBeforeStart;

        private void Start()
        {
            spawnTime = Time.time;
        }

        private void Update()
        {
            if (timelife < delayBeforeStart)
                return;

            if (timelife > maxTimelife)
                Destroy(gameObject);

            transform.position += transform.right * speedMove * Time.deltaTime;

            var colliders = Physics.OverlapSphere(transform.position, detectionRadius, scanMask);
            for (var x = 0; x < colliders.Length; x++)
            {
                if (colliders[x].gameObject == owner)
                    continue;

                HandleDetectionWithObject(colliders[x]);
            }
        }

        private void HandleDetectionWithObject(Collider entry)
        {
            if (entry.TryGetComponent<JumperSoloPlayer>(out var player))
            {
                player.Health.Change(OwnerTypeName, Damage);
                Destroy(gameObject);
            }
            else if (entry.TryGetComponent<JumperSoloEnemy>(out var enemy) && isOwnerPlayer)
            {
                enemy.Damage(Damage);
                Destroy(gameObject);
            }
        }

        private bool isOwnerPlayer;
        public void SetupFromOwner(JumperSoloEnemy enemy)
        {
            OwnerTypeName = enemy.MyTypeName;
            owner = enemy.gameObject;
        }

        public void SetupFromOwner(JumperSoloPlayer player)
        {
            isOwnerPlayer = true;
            owner = player.gameObject;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }
    }
}