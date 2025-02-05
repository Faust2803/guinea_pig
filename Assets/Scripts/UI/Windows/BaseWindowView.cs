using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public abstract class BaseWindowView : BaseView
    {
     
        [SerializeField] private RectTransform  _saveAreaPanelTransform;
        
        [SerializeField] private Button  _closeButton;
        
        protected BaseWindowMediator _mediator;
        
        public BaseWindowMediator BaseMediator => _mediator;

        public void OnCreateMediator(out BaseWindowMediator mediator)
        {
            mediator = _mediator;
        }

        public Button CloseButton => _closeButton;


        public override void Init()
        {
            base.Init();
            _mediator.Mediate(this);
        }
        
        private CanvasScaler canvasScaler;
        private float bottomUnits, topUnits;
        void Start()
        {
            var obj = FindObjectOfType<CanvasScaler>();
            if (obj == null)
            {
                canvasScaler = obj;
            }
            //ApplyVerticalSafeArea();
        }
    
        
        private void ApplyVerticalSafeArea()
        {
            var bottomPixels = Screen.safeArea.y;
            var topPixel = Screen.currentResolution.height - (Screen.safeArea.y + Screen.safeArea.height);
    
            var bottomRatio = bottomPixels / Screen.currentResolution.height;
            var topRatio = topPixel / Screen.currentResolution.height;
    
            var referenceResolution = canvasScaler.referenceResolution;
            bottomUnits = referenceResolution.y * bottomRatio;
            topUnits = referenceResolution.y * topRatio;
    
            
            _saveAreaPanelTransform.offsetMin = new Vector2(_saveAreaPanelTransform.offsetMin.x, bottomUnits);
            _saveAreaPanelTransform.offsetMax = new Vector2(_saveAreaPanelTransform.offsetMax.x, -topUnits);
        }
    }
}