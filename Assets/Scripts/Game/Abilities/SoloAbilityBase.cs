using DataModels.Achievement;
using Managers;
using Managers.SoundManager.Base;
using Managers.SoundManager.Enums;
using UnityEngine;
using Zenject;

namespace Game.Jumper
{
    public abstract class SoloAbilityBase : MonoBehaviour
    {
        [SerializeField] protected int priceToUse = 2;
        [SerializeField] float durationTime;
        [SerializeField] MeshRenderer effectRenderer;
        [SerializeField] ParticleSystem particles;
        [SerializeField] ParticleSystemRenderer effectR;
        [SerializeField] GameObject groupParticles;

        [SerializeField] internal TMPro.TextMeshProUGUI counter;

        [SerializeField] private SoundId _activateSound;
        [SerializeField] private bool callQuestEventOnActivated;
        [SerializeField] private TargetType abilityAchivType;

        [Inject] protected ISoundManager _audioManager;
        [Inject] private AchievementManager _achievementManager;

        protected float lastActivateTime;
        protected float timeActiavted =>
            Time.time - lastActivateTime;

        public ValueByType<bool> IsActivated = new ValueByType<bool>();
        public ValueByType<int> Count = new ValueByType<int>();

        public bool CanActivate =>
            Count.Value >= priceToUse &&
            GetComponent<IPlayerBase>().IsDead == false &&
            GetComponent<IPlayerBase>().IsFellout == false;

        public int PriceInItems => priceToUse;

        protected virtual void Start()
        {
            IsActivated.OnValueChanged += OnStateChanged;
        }

        protected virtual void Update()
        {
            if (counter != null)
                counter.text = $"{Count.Value}/{priceToUse}";

            if (IsActivated.Value)
            {
                if (ValidateDeactivate())
                {
                    Deactivate();
                }
            }
        }

        protected virtual void OnStateChanged(bool oldActive, bool nowActive)
        {
            if (nowActive)
            {
                _audioManager.PlayOneShot(_activateSound);
            }

            if (effectRenderer)
                effectRenderer.enabled = nowActive;
            if (effectR)
                effectR.enabled = nowActive;

            if (particles)
            {
                if (nowActive)
                {
                    particles.Play();
                }
                else
                {
                    particles.Stop();
                }
            }
            if (groupParticles)
            {
                groupParticles.SetActive(nowActive);
            }
        }

        #region Activation, Deactivation
        public virtual void TryActivate()
        {
            if (CanActivate &&
                IsActivated.Value == false)
            {
                HandleQuestOnActivate();
                OnStateChanged(IsActivated.Value, true);
                IsActivated.Value = true;
                lastActivateTime = Time.time;
                Activated();
                Count.Value -= priceToUse;
            }
        }


        protected virtual void Activated()
        {
            if (groupParticles)
            {
                groupParticles.SetActive(true);
            }
        }

        protected virtual bool ValidateDeactivate()
        {
            return timeActiavted >= durationTime;
        }

        internal virtual void Deactivate()
        {
            if (groupParticles)
            {
                groupParticles.SetActive(false);
            }

            OnStateChanged(IsActivated.Value, false);
            IsActivated.Value = false;
        }
        #endregion

        private void HandleQuestOnActivate()
        {
            _achievementManager.CheckAchievementTarget(new AchievementTarget(abilityAchivType, 1));
        }
    }
}