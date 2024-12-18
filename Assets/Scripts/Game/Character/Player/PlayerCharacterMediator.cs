
using UnityEngine;

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
        }
        
        public  override void GameLifeСycle()
        {
            if (Input.GetMouseButton(0))
            {
                RaycastHit hit;
                if (Physics.Raycast(Target.Camera.ScreenPointToRay(Input.mousePosition), out hit, 1000, Target.LayerMask))
                {
                    if(hit.collider.gameObject.tag == "Enemy")
                    {
                        Target.NavMeshAgent.enabled = false;
                        Target.Animator.Play("IdleNormal02_HG01_Anim 0");
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
                    Target.transform.rotation = Quaternion.Slerp(Target.transform.rotation, 
                        Quaternion.LookRotation(LastClickedObject.transform.position - Target.transform.position),
                        CharacterMoveSpeed * Time.deltaTime);
                    
                    // RaycastHit hit;
                    // var ray = new Ray(Target.transform.position, Target.transform.forward);
                    // //Debug.DrawRay(Target.transform.position, Target.transform.forward*100, Color.green);
                    // if (Physics.Raycast(ray, out hit))
                    // {
                    //     if (hit.collider.gameObject.tag == "EnemySenter")
                    //     {
                    //         Target.Animator.Play("ShootSingleshot_HG01_Anim 0");
                    //         Debug.Log("hit");
                    //         
                    //         
                    //         CharacterState = CharacterStateType.FireNext;
                    //     }
                    //
                    // }
                    
                }
                
                if (Target.NavMeshAgent.velocity.magnitude < 1 )
                {
                    if (LastClickedObject.tag != "Enemy")
                    {
                        Target.Animator.Play("IdleNormal02_HG01_Anim 0");
                        CharacterState = CharacterStateType.Idle;
                    }
                    
                }

            }
        }

        float PreviousRotation;
    }
}