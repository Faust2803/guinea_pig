using UnityEngine;

namespace Game.Jumper
{
    public class JumperSoloActivableObject : MonoBehaviour
    {
        [SerializeField] Transform movablePart;
        [SerializeField] float speedMove = 0.5f;
        [SerializeField] float maxTimeActivated = -1;

        private float activateTime;
        private bool activated;

        private float timeActivated =>
            Time.time - activateTime;

        private Vector3 startPos;

        private void Start()
        {
            startPos = movablePart.localPosition;
        }

        private void Update()
        {
            if (activated)
            {
                movablePart.transform.localPosition = Vector3.MoveTowards(movablePart.transform.localPosition, Vector3.zero, speedMove);
                if (maxTimeActivated > 0 && timeActivated > maxTimeActivated)
                {
                    activated = false;
                }
            }
            else
            {
                movablePart.transform.localPosition = Vector3.MoveTowards(movablePart.transform.localPosition, startPos, speedMove / 2);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<JumperSoloPlayer>(out var player))
            {
                activateTime = Time.time;
                activated = true;
            }
        }
    }
}