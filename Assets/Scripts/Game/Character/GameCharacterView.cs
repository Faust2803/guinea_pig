using System;
using Managers.SceneManagers;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Game.Character
{
    public abstract class GameCharacterView : CharacterView
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] protected Transform _weaponAttachment;
        [SerializeField] protected LayerMask _layerMask ;
        
        protected float CharacterMoveSpeed { get; set; }
        public GameObject LastObject { get;  set; }
        public CharacterStateType CharacterState { get;  private set; }
        [Inject] public GameSceneManager GameSceneManager {get;}
        
        public Transform WeaponAttachment => _weaponAttachment;
        public LayerMask LayerMask => _layerMask;
        public NavMeshAgent NavMeshAgent => _navMeshAgent;

        public void Start()
        {
            CharacterMoveSpeed = NavMeshAgent.acceleration;
            CharacterState = CharacterStateType.Idle;
            Animator.Play("IdleNormal02_HG01_Anim 0");
        }

        public void Update()
        {
            if (CharacterState == CharacterStateType.TakeAim )
            {
                var weaponAttachment = WeaponAttachment.transform;
                transform.rotation = Quaternion.Slerp(transform.rotation, 
                    Quaternion.LookRotation(LastObject.transform.position - transform.position),
                    CharacterMoveSpeed * Time.deltaTime);
                    
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            }
            else
            {
                if (LastObject != null && NavMeshAgent.velocity.magnitude < 0.1F )
                {
                    if ((LastObject.tag != "Enemy" || LastObject.tag != "Player") &&
                        CharacterState != CharacterStateType.Fire &&
                        CharacterState != CharacterStateType.Reload &&
                        CharacterState != CharacterStateType.Idle)
                    {
                        Animator.Play("IdleNormal02_HG01_Anim 0");
                        CharacterState = CharacterStateType.Idle;
                    }
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Boolet")
            {
                
            }
        }
        
        public void IsShoot()
        {
            GameSceneManager.CreateBoolet(WeaponAttachment.transform.position, transform.rotation);
        }
        
        protected void Fire()
        {
            Debug.Log("Fire");
            CharacterState = CharacterStateType.Fire;
            NavMeshAgent.enabled = false;
            Animator.SetTrigger("shoot");
            
        }

        protected void Reload()
        {
            Debug.Log("Reload");
            CharacterState = CharacterStateType.Reload;
            NavMeshAgent.enabled = false;
            Animator.Play("Reloading_HG01_Anim 0");
        }

        protected void GoToTarget(Vector3 target, GameObject lastObject = null)
        {
            NavMeshAgent.enabled = true;
            NavMeshAgent.SetDestination(target);
            Animator.Play("RunFWD_HG01_Anim 0");
            CharacterState = CharacterStateType.Run;
            LastObject = lastObject;
        }

        protected void TakeAim(Vector3 target, GameObject lastObject = null)
        {
            NavMeshAgent.enabled = false;
            Animator.Play("IdleNormal02_HG01_Anim 0");
            CharacterState = CharacterStateType.TakeAim;
            LastObject = lastObject;
        }
    }
}