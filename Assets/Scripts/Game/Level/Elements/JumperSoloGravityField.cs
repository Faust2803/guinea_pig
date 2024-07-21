using UnityEngine;

namespace Game.Jumper
{
    [RequireComponent(typeof(SphereCollider), typeof(Rigidbody))]
    public class JumperSoloGravityField : MonoBehaviour
    {
        public AnimationCurve Strength = AnimationCurve.Linear(0, 0, 1, 1);
        [Range(0, 1)]
        public float DeathStrengthLevel = 0.7f;
        public float GravityPower = 1;

        [SerializeField] float radius = 5;

        private JumperSoloPlayer player;

        private float forcePerPlayerPos
        {
            get
            {
                if (player == null)
                    return 0;

                var dist = Vector3.Distance(transform.position, player.transform.position);
                var percent = 1 - Mathf.Clamp01(dist / radius);
                return Strength.Evaluate(percent);
            }
        }

        private void Start()
        {
            GetComponent<SphereCollider>().radius = radius;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<JumperSoloPlayer>(out var otherPlayer))
            {
                player = otherPlayer;
                player.OverrideForce = new JumperSoloPlayer.ForceReplace(transform.position, GravityPower, forcePerPlayerPos);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<JumperSoloPlayer>(out var otherPlayer) && otherPlayer == player)
            {
                player.OverrideForce = null;
                player = null;
            }
        }

        private void Update()
        {
            if (player && player.Health.Value > 0)
            {
                player.OverrideForce.Override = forcePerPlayerPos;
                if(forcePerPlayerPos >= DeathStrengthLevel)
                {
                    player.Health.Change("black_hole", player.Health.Value);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}