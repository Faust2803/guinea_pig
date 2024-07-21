using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Jumper
{
    public class JumperSoloCornAbility : SoloAbilityFillingBase
    {
        [SerializeField, Range(-2f, 2f)] float percentOfForce = .5f;
        [SerializeField] private GameObject abilityObject;
        [SerializeField] private LayerMask platformMask;
        [SerializeField] private float radiusDetection = 5;
        [SerializeField] private float detectionHeightOffset = 5;
        [SerializeField] private float minDistToMove = 0.15f;

        private JumperSoloPlayer player;
        [SerializeField] private JumperSoloPlatform target;

        protected override void Start()
        {
            base.Start();

            player = transform.GetComponent<JumperSoloPlayer>();
        }

        protected override void Activated()
        {
            base.Activated();

            player.SpeedCutterPercent = percentOfForce;
            target = GetNearestPlatform(radiusDetection);
        }

        protected override void Update()
        {
            base.Update();

            if(target != null && IsActivated.Value)
            {
                if(player.GetHorizontalAxis == 0)
                {
                    var distByX = player.transform.position.x - target.transform.position.x;
                    var dist = Mathf.Abs(distByX);

                    if (dist < minDistToMove)
                        player.OverrideHorizAxis = 0;
                    else
                    {
                        var targetLeft = distByX < 0;
                        player.OverrideHorizAxis = targetLeft ? 1 : -1;
                    }
                }
            }
            else
            {
                player.OverrideHorizAxis = 0;
            }
        }

        internal override void Deactivate()
        {
            base.Deactivate();

            player.SpeedCutterPercent = 1f;
            player.OverrideHorizAxis = 0;
        }

        private Vector3 detectionCenter =>
            new Vector3(0, transform.position.y) + Vector3.up * detectionHeightOffset;

        private JumperSoloPlatform GetNearestPlatform (float radius)
        {
            var colliders = Physics.OverlapSphere(detectionCenter, radius);
            var platforms = new List<JumperSoloPlatform>();
            for(var x = 0; x < colliders.Length; x++)
            {
                var current = colliders[x];
                JumperSoloPlatform platform;
                if (current.TryGetComponent(out platform))
                {
                    if (current.GetComponent<JumperSoloEnemy>())
                        continue;
                }
                else
                {
                    platform = current.GetComponentInParent<JumperSoloPlatform>();
                }

                if (platforms.Contains(platform))
                    continue;

                platforms.Add(platform);
            }

            if (platforms.Count > 0)
                return platforms.OrderBy((x) => Vector3.Distance(transform.position, x.transform.position)).Last();

            return null;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(detectionCenter, radiusDetection);
        }
    }
}