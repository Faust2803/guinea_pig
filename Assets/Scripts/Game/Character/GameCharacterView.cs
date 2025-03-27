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
            if (other.gameObject.tag == "Boolet")
            {
                
            }
        }
        
        public void IsShoot()
        {
            GameSceneManager.CreateBoolet(WeaponAttachment.transform.position, transform.rotation);
            Debug.Log("FireCompleated");
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
            Debug.Log("Fire");
            CharacterState = CharacterStateType.Fire;
            NavMeshAgent.enabled = false;
            Animator.SetTrigger("shoot");
            Animator.SetBool("isShoot", true);
            
        }

        protected void Reload()
        {
            Debug.Log("Reload");
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

        protected void SetState(CharacterStateType state)
        {
            CharacterState = state;
        }
    }
}