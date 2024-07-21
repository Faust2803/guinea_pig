using UnityEngine;

namespace Game.Jumper
{
    public class JumperSoloGameItem : MonoBehaviour
    {
        public int RestoreHP;
        public bool AddResource;
        public GameItemType ItemType;
        public int Amount = 1;

        [SerializeField] float detectionRadius = 0.5f;
        [SerializeField] LayerMask scanMask;

        private void Update()
        {
            var colliders = Physics.OverlapSphere(transform.position, detectionRadius, scanMask);
            for (var x = 0; x < colliders.Length; x++)
            {
                if (colliders[x].TryGetComponent<JumperSoloPlayer>(out var player))
                {
                    if (AddResource)
                        player.TakeResource(ItemType, Amount);

                    if (RestoreHP > 0 && player.Health.Value < JumperSoloPlayer.DEFAULT_HP)
                        player.Health.Value = Mathf.Clamp(player.Health.Value + RestoreHP, 0, JumperSoloPlayer.DEFAULT_HP);

                    Destroy(gameObject);
                    break;
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }
    }
}