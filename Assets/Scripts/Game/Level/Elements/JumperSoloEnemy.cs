using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Jumper
{
    public class JumperSoloEnemy : MonoBehaviour
    {
        public string MyTypeName = "F.e. enemy";
        public ValueByType<int> Health = new ValueByType<int>(1);
        public UnityEvent OnShoot;

        [SerializeField] float fireRate = 1.5f;
        [SerializeField] float delayAfterAnimBeforeShoot = 0.5f;
        [SerializeField] JumperSoloProjectile projectile;
        [SerializeField] Transform projectileOutputPoint;
        [SerializeField] float detectionRadius = 15;

        private JumperSoloPlayer player;
        private JumperSoloPlatform platform;

        private float lastShootTime;
        private float delayFromLastShoot =>
            Time.time - lastShootTime;
        private float distanceToPlayer =>
            Vector3.Distance(transform.position, player.transform.position);
        private bool canShoot =>
            delayFromLastShoot > fireRate;
        private bool playerInAttackRadius =>
            distanceToPlayer < detectionRadius;

        private void Start()
        {
            player = GameObject.FindWithTag("Player").GetComponent<JumperSoloPlayer>();
            platform = GetComponent<JumperSoloPlatform>();
        }

        private void Update()
        {
            TryShoot();
        }

        private async void TryShoot()
        {
            if (canShoot && playerInAttackRadius && (platform == null || platform.BreakDelay == false))
            {
                lastShootTime = Time.time;
                if (TryGetComponent<Animator>(out var anims))
                    anims.SetTrigger("attack");
                await UniTask.Delay((int)(delayAfterAnimBeforeShoot * 1000f));
                PrepareBullet();
                OnShoot?.Invoke();
            }
        }

        private async void PrepareBullet()
        {
            if (platform != null && platform.BreakDelay)
                return;

            var pos = projectileOutputPoint != null ? projectileOutputPoint.position : transform.position;
            var result = Instantiate(projectile, pos, Quaternion.identity);
            result.SetupFromOwner(this);
            result.transform.right = player.transform.position - transform.position;

            if (result.DelayBeforeStart > 0)
            {
                await UniTask.Delay((int)(result.DelayBeforeStart * 1000));
                if (transform == null)
                    return;
                result.transform.right = player.transform.position - transform.position;

                if (result.TryGetComponent<Animator>(out var anims))
                    anims.SetBool("out", true);
            }
        }

        public void Damage(int damage)
        {
            Health.Value -= damage;
            if (Health.Value <= 0)
                Destroy(gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }
    }
}