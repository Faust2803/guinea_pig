using Managers.SoundManager.Base;
using Managers.SoundManager.Enums;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Game.Jumper
{
    public class JumperSoloPlatform : MonoBehaviour
    {
        public const string ANIM_IDLE_PARAM = "cloudType";
        //public const string ANIM_WOOBLE_PARAM = "wooble";
        public const string ANIM_DESTROY_PARAM = "destroy";

        [HideInInspector] public bool Breaked;

        public string TypeName = "Cloud Type";

        [Header("Animations")]
        public int TargetCloudType;

        [SerializeField] float destroyAfterBreak = 5;
        [SerializeField] int idleAnimsCount = 3;
        [SerializeField] Animator anims;
        [SerializeField] bool onlyIdleAnim;
        [Space]

        [Header("Behaviour")]
        [SerializeField] bool randomMoveParam;
        [SerializeField, Range(0, 1)] float canMoveRandomRange = 0.5f;
        [SerializeField] bool canMove;
        public bool DestroyColliderWhenPlayer = true;
        public bool BreakWhenPlayerContact = false;
        public bool JumpBooster;
        public int AddGoldenBeansPerContact;
        public int DamagePlayerPerContact;
        public GameObject PlayerContactParticle;
        public float DamageParticleDuration = 2f;

        public bool IsRewarded =>
            AddGoldenBeansPerContact > 0;

        [HideInInspector]
        public bool RewardGetted;

        [Space]
        [Header("Path (if move)")]
        [SerializeField] bool startOnRandomPoint = true;
        [SerializeField] float pathPointDetectionRadius = 0.1f;
        [SerializeField] float speedMove = 0.1f;
        [SerializeField] bool flipXWhenMove = true;
        [SerializeField] Vector2[] movePath;
        [SerializeField] bool useGlobalNav;

        [Space]
        [Header("Sounds")]
        public SoundId[] InterWithPlayer;
        public SoundId[] DestroySound;

        [Tooltip("Worked only when AddGoldenBeansPerContact > 0")]
        public UnityEvent OnRewardGetted;

        [Inject] ISoundManager sounds;

        private Vector2 inintedPos;

        private void Awake()
        {
            ProjectContext.Instance.Container.Inject(this);
        }

        private void Start()
        {
            inintedPos = transform.position;
            startScale = transform.localScale;
            if (idleAnimsCount != 0 && anims)
            {
                TargetCloudType = Random.Range(0, idleAnimsCount);
                SetupAnimType(TargetCloudType);
            }

            HandleRandomMove();

            if (canMove && startOnRandomPoint)
            {
                currentPoint = Random.Range(0, movePath.Length);
                transform.position = GetPointByIndex(currentPoint);
            }
        }

        private void HandleRandomMove()
        {
            if (randomMoveParam)
                canMove = canMoveRandomRange >= Random.value;
        }

        private void SetupAnimType(int id)
        {
            anims.SetFloat(ANIM_IDLE_PARAM, id);
            anims.Play("Idle", 0, Random.value);
        }

        private void Update()
        {
            if (canMove)
            {
                MoveCycle();
            }
        }

        private int currentPoint;
        private Vector3 startScale;
        private void MoveCycle()
        {
            var targetPoint = GetPointByIndex(currentPoint);
            var distanceToCurrentPoint = Vector3.Distance(transform.position, targetPoint);
            if (distanceToCurrentPoint < pathPointDetectionRadius)
                SelectNextPathPoint();
            else
                transform.position = Vector3.MoveTowards(transform.position, targetPoint, speedMove * Time.deltaTime);

            if (flipXWhenMove)
            {
                var flipX = transform.position.x > targetPoint.x;
                transform.localScale = new Vector3((flipX ? 1 : -1) * startScale.x, startScale.y, startScale.z);
            }
        }

        private void SelectNextPathPoint()
        {
            currentPoint++;
            if (currentPoint >= movePath.Length)
                currentPoint = 0;
        }

        private Vector3 GetPointByIndex(int movePathElementIndex)
        {
            var prev = movePathElementIndex == 0 ?
                useGlobalNav ? Vector3.up * inintedPos.y : inintedPos :
                (Vector2)GetPointByIndex(movePathElementIndex - 1);

            return movePath[movePathElementIndex] + prev;
        }

        public void InteractWithPlayer(JumperSoloPlayer player)
        {
            if (InterWithPlayer.Length > 0)
                sounds.PlaySound(InterWithPlayer[Random.Range(0, InterWithPlayer.Length)]);

            if (JumpBooster)
            {
                if (anims && onlyIdleAnim == false)
                {
                    anims.SetTrigger("destroy");
                }
            }
        }

        public bool BreakDelay { get; private set; }
        public void Break()
        {
            if (Breaked)
                return;

            BreakProcess();
        }

        private void BreakProcess()
        {
            if (DestroyColliderWhenPlayer)
            {
                if (Breaked)
                    return;
                Breaked = true;
            }

            if (DestroyColliderWhenPlayer)
                Destroy(GetComponent<Collider>());

            if (anims && onlyIdleAnim == false)
                anims.SetTrigger(ANIM_DESTROY_PARAM);

            if (DestroyColliderWhenPlayer)
            {
                if (DestroySound.Length > 0)
                    sounds.PlaySound(DestroySound[Random.Range(0, DestroySound.Length)]);

                BreakDelay = true;
                if (gameObject)
                    Destroy(gameObject, destroyAfterBreak);
            }
            else
            {
                Breaked = false;
            }
        }
    }
}