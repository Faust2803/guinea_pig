using Cysharp.Threading.Tasks;
using Managers.SceneManagers;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Game.Character
{
    public abstract class GameCharacterView : CharacterView
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private Collider _collider;
        [SerializeField] private Transform _weaponAttachment;
        [SerializeField] private LayerMask _layerMask;
        [Space]
        [SerializeField] private int _removeAfterDed = 7000;
        
        
        protected float CharacterMoveSpeed { get; set; }
        protected GameObject LastObject { get;  set; }
        protected CharacterStateType CharacterState { get;  private set; }

        [Inject] protected GameSceneManager GameSceneManager {get;}
        
        protected Transform WeaponAttachment => _weaponAttachment;
        protected LayerMask LayerMask => _layerMask;
        protected NavMeshAgent NavMeshAgent => _navMeshAgent;

        public void Awake()
        {
            NavMeshAgent.enabled = false;
        }

        public void Start()
        {
            CharacterMoveSpeed = NavMeshAgent.acceleration;
            CharacterState = CharacterStateType.Idle;
            Animator.Play("IdleNormal02_HG01_Anim 0");
        }

        public void Update()
        {
            Animator.SetFloat("speed", NavMeshAgent.velocity.magnitude);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Boolet" && CharacterState != CharacterStateType.Victory)
            {
                Lives--;
                LostLife(); 
                Debug.Log($"lives = {Lives}");
                if (Lives == 0 )
                {
                    Dad(other.gameObject.GetComponent<BooletView>().Shooter);
                }
            }
        }
        
        public void IsShoot()
        {
            GameSceneManager.CreateBoolet(WeaponAttachment.transform.position, transform.rotation, this);
            //Debug.Log("FireCompleated");
            CharacterState = CharacterStateType.FireCompleated;
        }

        protected void RotateToAim()
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, 
            Quaternion.LookRotation(LastObject.transform.position - transform.position),
            CharacterMoveSpeed * Time.deltaTime);
                    
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }
        
        protected void Fire()
        {
            //Debug.Log("Fire");
            CharacterState = CharacterStateType.Fire;
            NavMeshAgent.enabled = false;
            Animator.SetTrigger("shoot");
            Animator.SetBool("isShoot", true);
            
        }

        protected void Reload()
        {
            //Debug.Log("Reload");
            CharacterState = CharacterStateType.Reload;
            NavMeshAgent.enabled = false;
            Animator.Play("Reloading_HG01_Anim 0");
            Animator.SetBool("isShoot", false);
        }

        protected void GoToTarget(Vector3 target, GameObject lastObject = null)
        {
            NavMeshAgent.enabled = true;
            NavMeshAgent.SetDestination(target);
            Animator.Play("RunFWD_HG01_Anim 0");
            CharacterState = CharacterStateType.Run;
            LastObject = lastObject;
            Animator.SetBool("isShoot", false);
        }

        protected void TakeAim(Vector3 target, GameObject lastObject = null)
        {
            NavMeshAgent.enabled = false;
            Animator.Play("IdleNormal02_HG01_Anim 0");
            CharacterState = CharacterStateType.TakeAim;
            LastObject = lastObject;
            Animator.SetBool("isShoot", false);
        }

        protected void Idle()
        {
            NavMeshAgent.enabled = true;
            NavMeshAgent.SetDestination(transform.position);
            CharacterState = CharacterStateType.Idle;
        }
        
        protected virtual void Dad(GameCharacterView shooter)
        {
            Debug.Log("Die");
            CharacterState = CharacterStateType.Death;
            NavMeshAgent.enabled = false;
            Animator.SetBool("isShoot", false);
            Animator.SetBool("die", true);
            Animator.Play("Die_HG01_Anim 0");
            _collider.enabled = false;
            DeathPause().Forget();
            GameSceneManager.TurnOffDeathCharacter(this, shooter);
        }

        protected void Victory()
        {
            Debug.Log("Victory");
            CharacterState = CharacterStateType.Victory;
            NavMeshAgent.enabled = false;
            Animator.SetBool("isShoot", false);
            Animator.SetBool("victory", true);
            Animator.Play("Victory_HG01_Anim 0");
            _collider.enabled = false;
        }
        
        private async UniTask DeathPause()
        {
            await UniTask.Delay(_removeAfterDed); 
            gameObject.SetActive(false);
            GameSceneManager.CheckEndLevel();
        }

        protected void SetState(CharacterStateType state)
        {
            CharacterState = state;
        }
    }
}