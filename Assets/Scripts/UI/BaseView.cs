using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public abstract class BaseView : MonoBehaviour
    {
        [FormerlySerializedAs("_windowPanel")]
        [Header("Animation Panel")]
        
        [SerializeField] protected GameObject _animationPanel;
        [SerializeField] protected bool _openAnimation;
        [SerializeField] protected bool _closeAnimation;
        [SerializeField] protected bool _deleteAfterClose = true;
        [SerializeField] protected float _openCloseDuration = 0.1F;
        
        public bool OpenAnimation => _openAnimation;
        public bool CloseAnimation => _closeAnimation;
        public bool DeleteAfterClose => _deleteAfterClose;
        public float OpenCloseDuration => _openCloseDuration;
        public GameObject AnimationPanel => _animationPanel;
        
        public virtual void Init()
        {
            CreateMediator();
        }
        
        protected abstract void CreateMediator();

        public void ShowStart()
        {
            gameObject.SetActive(true);
            transform.SetAsLastSibling();
        }

        public virtual void Close()
        {
            gameObject.SetActive(false);

            if (_deleteAfterClose)
            {
                Destroy(gameObject);
                Destroy(this);
            }
            
        }
    }
}