using UnityEngine;

namespace Game.Jumper
{
    public class MovableDestroyerController : MonoBehaviour
    {
        [SerializeField]
        private AnimationCurve behaviour = AnimationCurve.Linear(0, 0, 1, 1);

        private JumperSoloGameController game;
        private float spawnTime;
        private float timeLife =>
            Time.time - spawnTime;

        private void Start()
        {
            spawnTime = Time.time;
            game = FindObjectOfType<JumperSoloGameController>();
            game.StartedByPlayer.OnValueChanged += OnGameStateChanged;
        }

        private void OnGameStateChanged(bool old, bool now)
        {
            if (now)
            {
                spawnTime = Time.time;
                game.StartedByPlayer.OnValueChanged -= OnGameStateChanged;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (game.StartedByPlayer.Value == false ||
               game.IsGameEnded.Value)
                return;

            if (other.TryGetComponent<JumperSoloPlayer>(out var player))
            {
                player.Health.Change("wave_fire", player.Health.Value);
            }
            else if (other.TryGetComponent<JumperSoloPlatform>(out var platform))
            {
                Destroy(platform.gameObject);
            }
        }

        private void Update()
        {
            if (game.StartedByPlayer.Value &&
                game.IsGameEnded.Value == false)
            {
                var speed = behaviour.Evaluate(timeLife);
                transform.position += Vector3.up * speed * Time.deltaTime;
            }
        }

        public void EnableChild(int idx)
        {
            for (var x = 0; x < transform.childCount; x++)
            {
                transform.GetChild(x).gameObject.SetActive(x == idx);
            }
        }
    }
}