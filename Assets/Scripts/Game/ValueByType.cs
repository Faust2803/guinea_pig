using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI;

namespace Game
{
    [System.Serializable]
    public class ValueByType<T>
    {
        [SerializeField]
        protected T _current;

        public T Value
        {
            get => _current;
            set => UpdateValue(value);
        }

        public UnityAction<T, T> OnValueChanged;

        public ValueByType(T current = default)
        {
            _current = current;
        }

        protected virtual void UpdateValue (T value)
        {
            OnValueChanged?.Invoke(_current, value);
            _current = value;
        }
    }
}