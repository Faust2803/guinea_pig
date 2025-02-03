
using Managers.SceneManagers;
using UI.Panels.BottomGamePanel;
using UnityEngine;
using Zenject;

namespace Game.Character.Player
{
    public class PlayerCharacterMediator : CharacterMediator<PlayerCharacterView, CharacterData>
    {
        private GameObject LastClickedObject;
        
        
        public override void Mediate(CharacterView value)
        {
            base.Mediate(value);
            CharacterMoveSpeed = Target.NavMeshAgent.acceleration;
            LastClickedObject = Target.gameObject;
            
            BottomGamePanelMediator.OnFire += Fire;
            BottomGamePanelMediator.OnReload += Reload;
        }

        public override void Remove()
        {
            BottomGamePanelMediator.OnFire -= Fire;
            BottomGamePanelMediator.OnReload -= Reload;
        }

        public  override void GameLifeСycle()
        {
            if (Input.GetMouseButton(0))
            {
                RaycastHit hit;
                if (Physics.Raycast(Target.Camera.ScreenPointToRay(Input.mousePosition), out hit, 100, Target.LayerMask))
                {
                    if(hit.collider.gameObject.tag == "Enemy")
                    {
                        Target.NavMeshAgent.enabled = false;
                        Target.Animator.Play("IdleNormal02_HG01_Anim 0");
                        Target.Animator.SetTrigger("takeAim");
                        CharacterState = CharacterStateType.TakeAim;
                    }
                    else
                    {
                        Target.NavMeshAgent.enabled = true;
                        Target.NavMeshAgent.SetDestination(hit.point);
                        Target.Animator.Play("RunFWD_HG01_Anim 0");
                        CharacterState = CharacterStateType.Run;
                    }
                    LastClickedObject = hit.collider.gameObject;
                }
            }
            else
            {
                if (CharacterState == CharacterStateType.TakeAim )
                {
                    var weaponAttachment = Target.WeaponAttachment.transform;
                    Target.transform.rotation = Quaternion.Slerp(Target.transform.rotation, 
                        Quaternion.LookRotation(LastClickedObject.transform.position - Target.transform.position),
                        CharacterMoveSpeed * Time.deltaTime);
                    
                    Target.transform.eulerAngles = new Vector3(0, Target.transform.eulerAngles.y, 0);
                }
                
                if (Target.NavMeshAgent.velocity.magnitude < 1 )
                {
                    //if (LastClickedObject.tag != "Enemy")
                    if (LastClickedObject.tag != "Enemy" && CharacterState != CharacterStateType.Fire && CharacterState != CharacterStateType.Reload)
                    {
                        Target.Animator.Play("IdleNormal02_HG01_Anim 0");
                        CharacterState = CharacterStateType.Idle;
                    }
                }

            }
        }

        private void Fire()
        {
            Debug.Log("Fire");
            CharacterState = CharacterStateType.Fire;
            Target.NavMeshAgent.enabled = false;
            //Target.Animator.Play("ShootSingleshot_HG01_Anim 0");
            Target.Animator.SetTrigger("shoot");
            
        }

        private void Reload()
        {
            Debug.Log("Reload");
            CharacterState = CharacterStateType.Reload;
            Target.NavMeshAgent.enabled = false;
            Target.Animator.Play("Reloading_HG01_Anim 0");
        }
    }
}