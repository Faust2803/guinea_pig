using Common.Signals.StatisticSignal;
using Common.Statistic;
using Game.Visual;
using Managers;
using Managers.SoundManager.Base;
using Managers.SoundManager.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Game.Jumper
{
    [DefaultExecutionOrder(500)]
    public class JumperSoloPlayer : PlayerBase, IPlayerBase
    {
        public const int DEFAULT_HP = 3;
        private const string SAVE_PATH_BASE = "Jumper/Solo/Heightscore";

        public Vector2 HeightscorePercent
        {
            get
            {
                var hs = jumperGame.Layers.GetHeightscoreFromStartData();
                return new Vector2(hs.Item1, hs.Item2);
            }
            set
            {
                PlayerPrefs.SetFloat(SAVE_PATH_BASE + "/SphereID", value.x);
                PlayerPrefs.SetFloat(SAVE_PATH_BASE + "/Percent", value.y);
                PlayerPrefs.Save();
            }
        }

        [Inject] private UiManager _uiManager;
        [Inject] private ISoundManager _sounds;
        [Inject] private SignalBus _signalBus;
        [Inject] private PrefabSpawner moneyCounterVFX;
        [Inject] private PlayerManager playerManager;

        public Vector2 CurrentForce;
        public ForceReplace OverrideForce;
        public HealthValue Health = new HealthValue(DEFAULT_HP);
        public ValueByType<bool> Fellout { get; private set; } = new ValueByType<bool>();
        public ValueByType<bool> Flip { get; private set; } = new ValueByType<bool>();

        public string Nickname
        {
            get
            {
                try
                {
                    //return _playerData.PlayerName;
                    return "Player";
                }
                catch (System.Exception)
                {
                    Debug.Log("PlayerName64 null");
                    return "Error Name";
                }
            }
        }
        public bool EnableControlls;

        public int OwnerId => 0;
        public int UniqueID => 0;

        public bool IsDead =>
            Health.Value <= 0;

        public bool IsFellout =>
            Fellout.Value;

        #region Resource Holder 
        public int PeansCurrency =>
            Peas.Count.Value;
        public int CornsCurrency =>
            Corns.Count.Value;
        public int SeedsCurrency =>
            Seeds.Count.Value;

        public int BeansCurrency;
        public int GoldenBeansCurrency;

        int IResourceHolder.BeansCurrency
        {
            get => BeansCurrency;
            set => BeansCurrency = value;
        }

        int IResourceHolder.GoldenBeansCurrency
        {
            get => GoldenBeansCurrency;
            set => GoldenBeansCurrency = value;
        }
        #endregion

        [SerializeField] protected float heightModifer = 3f;
        [SerializeField] protected float boostJumpModifer = 3f;
        [SerializeField] AnimationCurve jumpForceBehaviour;
        [SerializeField] float sideForce = 4f;
        [SerializeField] float gravityForce = -5f;
        [SerializeField] float jumpRate = 3f;

        [SerializeField] protected LayerMask groundMask;
        [SerializeField] float groundRayLength = .15f;
        [SerializeField] TextMeshPro playerName;
        [SerializeField] Animator jumperHamsterAnimator;
        [SerializeField] GameObject wings;
        [SerializeField] int idleAnimsCount = 6;
        [SerializeField] bool isFalling;
        [SerializeField] float minJumpForceForJump;

        public float SpeedCutterPercent = 1;
        public bool FreezeY;

        private bool wasLoose;
        private float lastJumpTime;
        internal float heightestPos;
        protected JumperSoloGameController jumperGame { get; private set; }

        private JumperSoloGamePanel gameScreen;
        private Vector3 lastPlayerPos;

        protected virtual void Start()
        {
            jumperGame = FindObjectOfType<JumperSoloGameController>();

            Health.Value = Mathf.Clamp(playerManager.CurrentCollectionItem.collection_current_hp, 1, 100);
            Health.OnValueChanged += OnHealthChanged;
            Fellout.OnValueChanged += OnFelloutChanged;
            Flip.OnValueChanged += OnFlipChanged;
            jumperHamsterAnimator.SetFloat("idleType", Random.Range(0, idleAnimsCount));

            OnFlipChanged(false, Flip.Value);
            UpdateNickname();

            InitUI();
            
            jumperGame.IsGameBegin.OnValueChanged += (old, current) =>
            {
                if(current)
                    gameScreen.SetupGameController(FindObjectOfType<GameLayerController>());
            }; 
        }

        private UI.Panels.PanelType gamePanelType =>
             UI.Panels.PanelType.GamePanel;

        private void InitUI()
        {
            if (_uiManager.GetPanelInCreated(gamePanelType) != null)
                _uiManager.ClosePanel(gamePanelType);
            _uiManager.OpenPanel(gamePanelType);
            gameScreen = _uiManager.GetPanelInCreated(gamePanelType).PanelView as JumperSoloGamePanel;

            Peas.OnAbilityStateChanged += state => gameScreen.peanAbility.ChangeState(state);
            Corns.OnAbilityStateChanged += state => gameScreen.cornAbility.ChangeState(state);
            Seeds.OnAbilityStateChanged += state => gameScreen.seedAbility.ChangeState(state);
            Peas.Count.Value = Peas.PriceInItems;
            gameScreen.PlayerControllsVisibled = false;

            gameScreen.cornAbility.buttonComponent.onClick.AddListener(() => Corns.TryActivate());
            gameScreen.peanAbility.buttonComponent.onClick.AddListener(() =>
            {
                Peas.TryActivate();

                if (jumperGame.StartedByPlayer.Value == false)
                {
                    EnableControlls = true;
                    gameScreen.PlayerControllsVisibled = true;
                    jumperGame.StartedByPlayer.Value = true;
                }
            });
            gameScreen.seedAbility.buttonComponent.onClick.AddListener(() => Seeds.TryActivate());
        }

        #region Nickname
        protected void UpdateNickname()
        {
            if (playerName != null)
                playerName.text = Nickname;
        }
        #endregion

        #region Game Logic
        private void OnFelloutChanged(bool oldValue, bool newValue)
        {
            if (newValue)
                PlayerLoose();
        }

        private void OnHealthChanged(int oldHealth, int health)
        {
            if (IsDead || IsFellout) return;

            UpdateHeightscoreIfNeed();

            if (health <= 0)
                PlayerLoose();
        }

        private void OnFlipChanged(bool old, bool current)
        {
            var scaleX = Mathf.Abs(wings.transform.localScale.x);
            wings.transform.localScale = new Vector3(current ? scaleX : -scaleX,
                wings.transform.localScale.y,
                wings.transform.localScale.z);

            var posX = Mathf.Abs(wings.transform.localPosition.x);
            wings.transform.localPosition = new Vector3(current ? -posX : posX,
                wings.transform.localPosition.y,
                wings.transform.localPosition.z);

            var source = jumperHamsterAnimator.transform.localEulerAngles;
            jumperHamsterAnimator.transform.localRotation = Quaternion.Euler(source.x, current ? 180 : 240, source.z);
        }

        protected virtual void FixedUpdate()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown("3"))
            {
                Peas.TryActivate();

                if (jumperGame.StartedByPlayer.Value == false)
                {
                    EnableControlls = true;
                    jumperGame.StartedByPlayer.Value = true;
                }
            }
            else if (Input.GetKeyDown("1"))
                Corns.TryActivate();
            else if (Input.GetKeyDown("2"))
                Seeds.TryActivate();
#endif

            if (IsFellout ||
                IsDead ||
                jumperGame == null ||
                jumperGame.IsGameBegin.Value == false)
                return;

            if (jumperGame.IsGameEnded.Value == false && EnableControlls)
            {
                var modifer = 1f;
                if (boosterSpecModif != null)
                    modifer = boosterSpecModif.Value;
                else if (boostJump)
                    modifer = boostJumpModifer;

                if (FreezeY == false)
                {
                    CurrentForce.y = isOutOfBehaviour ?
                            gravityForce :
                            jumpForceBehaviour.Evaluate(jumpTimeDiff) * heightModifer * modifer;
                }

                HandleJump();

                CurrentForce.x = GetMoveAxis * sideForce * Mathf.Abs(SpeedCutterPercent);

                FallingCheckerPosY();
                HandleOverrideForce();
                MoveByCurrentForce();
            }

            if (transform.position.y > heightestPos)
                heightestPos = transform.position.y;

            lastPlayerPos = transform.position;
        }

        private void HandleOverrideForce()
        {
            if (OverrideForce == null)
                return;

            var dir = OverrideForce.CenterOfGravity - transform.position;
            var force = dir * OverrideForce.Force;

            CurrentForce = Vector2.Lerp(CurrentForce, force, OverrideForce.Override);
        }

        protected virtual void PlayerLoose()
        {
            if (wasLoose)
                return;

            for(var c = 1; c < transform.childCount; c++)
                transform.GetChild(c).gameObject.SetActive(false);

            jumperHamsterAnimator.transform.localRotation = Quaternion.Euler(0, 180, 0);
            jumperHamsterAnimator.SetBool("dead", true);

            Flip.OnValueChanged -= OnFlipChanged;
            Health.OnValueChanged -= OnHealthChanged;
            Fellout.OnValueChanged -= OnFelloutChanged;

            wasLoose = true;
        }

        private void UpdateHeightscoreIfNeed()
        {
            var playerSphere = jumperGame.Layers.GetSphereByPosY(heightestPos);
            var playerPercent = jumperGame.Layers.GetPercentOfSphereByObjectPos(heightestPos);
            //Debug.Log($"Check record ({playerSphere}, {playerPercent}) heightest {heightestPos}");
            if (playerSphere > HeightscorePercent.x ||
                playerSphere == HeightscorePercent.x && playerPercent > HeightscorePercent.y)
            {
                HeightscorePercent = new Vector2(playerSphere, playerPercent);
                Debug.Log($"Heightscore setted: {HeightscorePercent}");
            }
        }

        private void FallingCheckerPosY()
        {
            if (CurrentForce.y < minJumpForceForJump && !isFalling)
            {
                jumperHamsterAnimator.Play("Hamster_Jumper_FallStart");
                isFalling = true;
            }
        }

        public float GetMoveAxis
        {
            get => GetHorizontalAxis != 0 ? GetHorizontalAxis : OverrideHorizAxis;
        }

        public float GetHorizontalAxis
        {
            get
            {
                if (EnableControlls == false)
                    return 0f;

                return gameScreen != null && gameScreen.joystick.Axis != 0 ?
                    gameScreen.joystick.Axis :
                    Input.GetAxis("Horizontal");
            }
        }

        public float OverrideHorizAxis { get; set; }
        #endregion

        #region Game end
        public void GameEndRpc(int winnerUniqueID)
        {
            
        }

        protected bool IsWinnerAfterGameEnd { get; private set; }
        protected virtual void SetupLooseOrWin(bool isWin)
        {
            IsWinnerAfterGameEnd = isWin;

            if (GetActivatedAbility != null)
                GetActivatedAbility.Deactivate();

            if (isWin)
                Jump();
        }
        #endregion

        #region Logic
        private void HandleJump()
        {
            if (transform.position.y < /*jumperGame.GetDefaultPlatformHeight -*/ 0)
            {
                Jump(false, false);
                return;
            }

            if (IsGrounded(out var platform) &&
                    platform.GetComponent<Rigidbody>() == null &&
                    isFalling &&
                    Peas.IsActivated.Value == false &&
                    Corns.IsActivated.Value == false)
            {
                JumpProcessByPlatform(platform);
                boosterSpecModif = null;
                jumperHamsterAnimator.Play("Hamster_Jumper_JumpStart");
                isFalling = false;
            }
        }
        
        public UnityAction<JumperSoloPlatform> OnPlatformInteract;

        protected virtual void JumpProcessByPlatform(GameObject platform)
        {
            if (Time.time - lastJumpTime < jumpRate)
                return;

            var platformComponent = platform.GetComponent<JumperSoloPlatform>();
            platformComponent.InteractWithPlayer(this);
            OnPlatformInteract?.Invoke(platformComponent);
            
            if (platformComponent)
            {
                Jump(platformComponent.JumpBooster);

                if (platformComponent.IsRewarded &&
                    platformComponent.RewardGetted == false)
                {
                    GoldenBeansCurrency += platformComponent.AddGoldenBeansPerContact;
                    _signalBus.Fire(new StatisticSignal(StatisticSourceId.Beans, platformComponent.AddGoldenBeansPerContact));
                    _sounds.PlayOneShot(SoundId.GoldenBean);
                    platformComponent.RewardGetted = true;
                    platformComponent.OnRewardGetted?.Invoke();
                    moneyCounterVFX.Spawn(transform.position);
                }

                if (platformComponent.DamagePlayerPerContact > 0)
                {
                    Health.Change(platformComponent.TypeName, platformComponent.DamagePlayerPerContact);

                    if (platformComponent.PlayerContactParticle)
                    {
                        var particles = Instantiate(platformComponent.PlayerContactParticle, transform);
                        Destroy(particles, platformComponent.DamageParticleDuration);
                    }
                }

                if (platformComponent.BreakWhenPlayerContact)
                    platformComponent.Break();
            }
            else
            {
                Jump();
            }
        }

        private void MoveByCurrentForce()
        {
            var forceWithGravity = FreezeY ?
                CurrentForce.y :
                Mathf.Lerp(CurrentForce.y, gravityForce, Mathf.Clamp01(jumpTimeDiff / jumpBehaviourTime));

            if (FreezeY == false && forceWithGravity < 0)
                forceWithGravity *= SpeedCutterPercent;

            Flip.Value = CurrentForce.x > 0;
            if (transform.position.x < jumperGame.MinAndMaxPlayerX.x && GetMoveAxis < 0)
                CurrentForce.x = 0;
            else if (transform.position.x > jumperGame.MinAndMaxPlayerX.y && GetMoveAxis > 0)
                CurrentForce.x = 0;

            var direction = Vector3.right * CurrentForce.x + Vector3.up * forceWithGravity;
            transform.position += direction * Time.fixedDeltaTime;
        }

        private float jumpTimeDiff =>
            Time.time - lastJumpTime;

        private float jumpBehaviourTime =>
            jumpForceBehaviour.keys[jumpForceBehaviour.length - 1].time;

        private bool isOutOfBehaviour =>
            jumpTimeDiff >= jumpBehaviourTime;

        private bool boostJump;
        private float? boosterSpecModif;
        public virtual void Jump(bool ultraJump = false, bool withSound = true)
        {
            lastJumpTime = Time.time;
            boostJump = ultraJump;

            if (withSound)
                PlaySound(ultraJump ? jumpSpringSounds : jumpSounds);
        }

        public void SpecialJump(float specif)
        {
            lastJumpTime = Time.time;
            boosterSpecModif = specif;
        }

        public bool IsGrounded()
        {
            return IsGrounded(out _);
        }

        public bool IsGrounded(out GameObject other)
        {
            other = null;
            if (transform.position.y < 0)
                return true;

            if (Physics.Raycast(lastPlayerPos,
                Vector3.down,
                out var hit,
                groundRayLength + Vector3.Distance(transform.position, lastPlayerPos),
                groundMask))
            {
                if (hit.collider.GetComponentInParent<JumperSoloPlatform>())
                    other = hit.collider.GetComponentInParent<JumperSoloPlatform>().gameObject;
                else
                    other = hit.collider.gameObject;

                return true;
            }
            return false;
        }
        #endregion

        #region Abilities 
        public JumperSoloPeasAbility Peas;
        public JumperSoloSeedAbility Seeds;
        public JumperSoloCornAbility Corns;

        public SoloAbilityBase GetActivatedAbility
        {
            get
            {
                if (Peas.IsActivated.Value)
                    return Peas;

                if (Seeds.IsActivated.Value)
                    return Seeds;

                if (Corns.IsActivated.Value)
                    return Corns;

                return null;
            }
        }
        #endregion

        #region Sounds
        [SerializeField] SoundId jumpSounds;
        [SerializeField] SoundId jumpSpringSounds;

        public void PlaySound(SoundId sound)
        {
            _sounds.PlayOneShot(sound);
        }
        #endregion

        #region Debug
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = IsGrounded() ? Color.green : Color.red;
            Gizmos.DrawLine(lastPlayerPos, lastPlayerPos + Vector3.down *
                (groundRayLength + Vector3.Distance(transform.position, lastPlayerPos)));
        }
        #endregion

        #region Resources
        public void TakeResource(GameItemType resourceType, int count)
        {
            switch (resourceType)
            {
                case GameItemType.Corns:
                    Corns.Count.Value += count;
                    break;
                case GameItemType.Peans:
                    Peas.Count.Value += count;
                    break;
                case GameItemType.Seeds:
                    Seeds.Count.Value += count;
                    break;
                case GameItemType.GoldenBeans:
                    GoldenBeansCurrency += count;
                    _signalBus.Fire(new StatisticSignal(StatisticSourceId.Beans, count));
                    break;
            }
        }

        public void SetGameEndedClientRpc(int winnerID)
        {
            throw new System.NotImplementedException();
        }
        #endregion

        [System.Serializable]
        public class ForceReplace
        {
            public Vector3 CenterOfGravity;
            public float Force;
            public float Override;

            public ForceReplace(Vector3 centerOfGravity,
                                float force,
                                float newForceValue)
            {
                CenterOfGravity = centerOfGravity;
                Force = force;
                Override = newForceValue;
            }
        }
    }

    public class HealthValue : ValueByType<int>
    {
        public UnityAction<string, int, int> OnChangedBy;

        public HealthValue(int health)
        {
            _current = health;
        }

        public void Change (string damageType, int value)
        {
            OnChangedBy?.Invoke(damageType, Value, Value - value);
            Value -= value;
        }
    }
}