using UnityEngine;

namespace Game
{
    public class GameAnimatorHelper : MonoBehaviour
    {
        [SerializeField] Animator animator;
        [SerializeField] string nodeName = "Idle";

        private void Awake()
        {
            animator?.Play(nodeName, 0, Random.value);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            animator = GetComponent<Animator>();
        }
#endif
    }
}