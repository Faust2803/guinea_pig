using System;

namespace Game.Jumper
{
    public abstract class SoloAbilityFillingBase : SoloAbilityBase
    {
        private AbilityFillingState _currentFillingState = AbilityFillingState.Empty;
        public Action<AbilityFillingState> OnAbilityStateChanged;
        public Action OnAbilityCountChanged;

        protected override void Start()
        {
            base.Start();

            Count.OnValueChanged += OnCountChanged;
        }

        protected virtual void OnDestroy()
        {
            Count.OnValueChanged -= OnCountChanged;
        }

        private void OnCountChanged(int previousValue, int newValue)
        {
            OnAbilityCountChanged?.Invoke();
            var newFillingState = AbilityFillingState.Empty;
            if (newValue <= 0)
            {
                newFillingState = AbilityFillingState.Empty;
            }
            else if (newValue < priceToUse)
            {
                newFillingState = AbilityFillingState.Partial;
            }
            else if (newValue >= priceToUse)
            {
                newFillingState = AbilityFillingState.Full;
            }
            //if (newFillingState == _currentFillingState) return;
            _currentFillingState = newFillingState;
            OnAbilityStateChanged?.Invoke(_currentFillingState);
        }
    }
}