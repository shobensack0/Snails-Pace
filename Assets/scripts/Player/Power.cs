using UnityEngine;

namespace Player
{
    public class Power : PlayerScriptMonoBehavior
    {
        #region Public Properties
        public bool isPowerActive = false;
        #endregion

        #region Power and Components
        public GameObject currentPower;

        private Animator currentPower_Animator;
        private SpriteRenderer currentPower_SpriteRenderer;
        private PolygonCollider2D currentPower_Collider;
        #endregion

        public void Start()
        {
            this.EquipPower(currentPower);
            this.SetCharacterComponents();
        }
        
        public void Update()
        {
            var inputActive = Input.GetAxisRaw("Fire2") > 0.0f;
            this.DeterminePowerState(inputActive);

        }

        private void DeterminePowerState(bool inputActive)
        {
            if (inputActive)
            {
                currentPower_Animator.SetBool("Charging", true);
                character_Animator.SetBool("Is Carrying", true);
                isPowerActive = true;
            }

            this.DetermineOverChargeState();
        }

        private void RenderCharacterOverCharged()
        {
            character_SpriteRenderer.color = Color.black;
        }

        private void ResetCharacter()
        {
            character_SpriteRenderer.color = Color.white;
        }

        private void DetermineOverChargeState()
        {
            var isOverCharged = currentPower_Animator.GetCurrentAnimatorStateInfo(0).IsName("Over Charged");
            var isChargeAnimationPlaying = currentPower_Animator.GetCurrentAnimatorStateInfo(0).IsName("Charging");

            if (isPowerActive)
            {
                if (isOverCharged)
                {
                    this.RenderCharacterOverCharged();
                }
            }
            else
            {
                if (isOverCharged)
                {
                    this.ResetCharacter();
                    currentPower_Animator.Play("No Power");
                }
                else
                {
                    if (isChargeAnimationPlaying)
                    {
                        currentPower_Animator.Play("No Power");
                        character_Animator.SetBool("Is Carrying", false);
                    }
                }
            }
        }

        public bool AllowOtherActions()
        {
            return true;
        }

        #region Private Methods
        private void EquipPower(GameObject power)
        {
            if (power == null)
                return;

            currentPower = power;

            power.TryGetComponent<SpriteRenderer>(out currentPower_SpriteRenderer);
            power.TryGetComponent<Animator>(out currentPower_Animator);
            power.TryGetComponent<PolygonCollider2D>(out currentPower_Collider);
        }
        #endregion
    }
}
