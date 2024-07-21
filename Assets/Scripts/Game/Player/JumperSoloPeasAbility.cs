using UnityEngine;

namespace Game.Jumper
{
    public class JumperSoloPeasAbility : SoloAbilityFillingBase
    {
        [SerializeField] float targetYForce = 3;

        private JumperSoloPlayer player;
        [SerializeField] private Animator jumperAbilityHamsterAnimator;

        protected override void Start()
        {
            base.Start();

            player = transform.GetComponent<JumperSoloPlayer>();
        }

        protected override void Activated()
        {
            base.Activated();

            player.FreezeY = true;

            jumperAbilityHamsterAnimator.Play("Hamster_Jumper_Puk");
        }

        protected override void Update()
        {
            base.Update();

            player.CurrentForce.y = targetYForce;
        }

        internal override void Deactivate()
        {
            player.FreezeY = false;

            base.Deactivate();
        }
    }
}