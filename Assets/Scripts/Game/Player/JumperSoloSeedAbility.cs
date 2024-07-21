using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Jumper
{
    public class JumperSoloSeedAbility : SoloAbilityFillingBase
    {
        [SerializeField] JumperSoloProjectile bulletPrefab;
        [SerializeField] float heightOffset = 1.2f;
        [SerializeField] LayerMask scanMask;
        [SerializeField] float scanRadius = 15;
        [SerializeField] float crosshairDownOffset = 1f;

        protected override void Activated()
        {
            base.Activated();
            CreateBullet();
        }

        private void CreateBullet()
        {
            var instance = Instantiate(bulletPrefab, GetProjectileSpawnPoint, Quaternion.identity);
            instance.SetupFromOwner(GetComponent<JumperSoloPlayer>());

            var target = GetNearestTarget();
            instance.transform.right = target == null ? Vector3.up : (target.position + Vector3.down * crosshairDownOffset) - transform.position;
        }

        private Transform GetNearestTarget()
        {
            var colliders = Physics.OverlapSphere(transform.position, scanRadius, scanMask);
            var variants = new List<Transform>();
            for (var x = 0; x < colliders.Length; x++)
            {
                if (colliders[x].gameObject == gameObject ||
                    colliders[x].transform.position.y < transform.position.y ||
                    colliders[x].GetComponent<JumperSoloEnemy>() == null)
                    continue;

                variants.Add(colliders[x].transform);
            }

            if (variants.Count > 0)
            {
                return variants.OrderBy((x) => Vector3.Distance(x.position, transform.position)).First();
            }

            return null;
        }

        private Vector3 GetProjectileSpawnPoint
        {
            get => transform.position + Vector3.up * heightOffset;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(GetProjectileSpawnPoint, 0.1f);
            Gizmos.DrawWireSphere(transform.position, scanRadius);
        }
    }
}