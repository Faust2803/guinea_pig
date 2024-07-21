using UnityEngine;
using Zenject;

namespace Game.Visual
{
    public class PrefabSpawner : MonoInstaller
    {
        public GameObject Target;
        public float MaxTimelife = -1;
        public Vector3 SpawnOffset = Vector3.left * 0.5f;
        public Vector2 XLimits = new Vector2(-4, 4);

        public override void InstallBindings()
        {
            Container.
                Bind<PrefabSpawner>().
                FromInstance(this).
                AsSingle().
                NonLazy();
        }

        public void Spawn (Vector3 pos)
        {
            var resultPos = pos + SpawnOffset;
            resultPos.x = Mathf.Clamp(resultPos.x, XLimits.x, XLimits.y);
            var instance = Instantiate(Target, resultPos, Quaternion.identity);
            if (MaxTimelife > 0)
                Destroy(instance, MaxTimelife);
        }

        public void SpawnAtPlayer ()
        {
            var player = GameObject.FindWithTag("Player");
            if (player != null)
                Spawn(player.transform.position);
        }
    }
}