using UnityEngine;

namespace Player
{
    public class Power : PlayerScriptMonoBehavior
    {
        #region Public Properties
        public bool isPowerActive = false;
        public bool isOverCharged = false;

        public float overChargeCounterTime = 5.0f;
        public float currentOverChargeCounter = 0.0f;
        public float currentPowerSize = 0.0f;
        #endregion

        #region Power and Components
        public GameObject currentPowerPrefab;
        private GameObject currentPower;

        /// <summary>
        /// TODO: can we get this at run time so we don't have to add a reference for every power?
        /// 
        /// should be able to parse animator for the "Power" state, get the animation attatched,
        /// and determine the time.
        /// </summary>
        public AnimationClip currentPower_Animation;

        private Animator currentPower_Animator;
        private SpriteRenderer currentPower_SpriteRenderer;
        private PolygonCollider2D currentPower_Collider;
        #endregion

        #region Power Variables
        //private float powerAnimationTime = 0.0f;
        #endregion

        public void Start()
        {
            this.ResetPower(currentPowerPrefab);
            this.SetCharacterComponents();
        }
        
        public void Update()
        {
            var fire_inputActive = Input.GetAxisRaw("Fire1") > 0.0f;
            var charge_inputActive = Input.GetAxisRaw("Fire2") > 0.0f;
            this.DeterminePowerState(charge_inputActive, fire_inputActive);
        }

        public bool AllowOtherActions()
        {
            return !isPowerActive && !isOverCharged;
        }

        #region Private Methods
        private void ResetPower(GameObject powerPrefab)
        {
            if (powerPrefab == null)
                return;

            currentPower = Instantiate(currentPowerPrefab, this.gameObject.transform);

            currentPower.TryGetComponent<SpriteRenderer>(out currentPower_SpriteRenderer);
            currentPower.TryGetComponent<Animator>(out currentPower_Animator);
            currentPower.TryGetComponent<PolygonCollider2D>(out currentPower_Collider);
        }

        private void DeterminePowerState(bool charge_inputActive, bool fire_inputActive)
        {
            if (charge_inputActive)
            {
                // fire charged power
                if (isPowerActive && fire_inputActive && currentPowerSize > 0.25f)
                {
                    currentPower.TryGetComponent<PowerFire>(out PowerFire powerScript);

                    if (powerScript && currentPower.transform.parent == this.transform)
                    {
                        // detatch power from this object
                        currentPower_Animator.enabled = false;
                        currentPower_SpriteRenderer = null;
                        currentPower_Animation = null;
                        currentPower_Animator = null;
                        currentPower_Collider = null;
                        currentPower.transform.parent = null;

                        // TODO: multiple get created
                        powerScript.Fire(this, Camera.main.ScreenToWorldPoint(Input.mousePosition), 10);
                        this.ResetPower(currentPowerPrefab);
                    }
                }
                else
                {
                    // continue charging
                    currentPower_Animator.SetBool("Charging", true);
                    character_Animator.SetBool("Is Carrying", true);
                    this.UpdatePowerCharge();
                    isPowerActive = true;
                }
            }
            else
            {
                currentPower_Animator.SetBool("Charging", false);
                character_Animator.SetBool("Is Carrying", false);
                isPowerActive = false;
            }

            this.DetermineOverChargeState();
        }

        private void UpdatePowerCharge()
        {
            if (!isOverCharged)
            {
                // determine scale
                if (currentPowerSize > 0.0f && currentPowerSize < 1 && !isPowerActive)
                {
                    // if we have a charge and power isn't active yet
                    currentPower_Animator.Play("Power", 0, currentPowerSize);
                    currentPower_SpriteRenderer.transform.localScale = new Vector2(currentPowerSize, currentPowerSize);
                }
                else
                {
                    var multiplier = currentPower_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

                    currentPower_SpriteRenderer.transform.localScale = new Vector2(multiplier, multiplier);
                    currentPowerSize = multiplier;
                }
            }

            if (currentPowerSize > 0.90f)
            {
                // ensure you can't resume when charge is too high
            }
        }

        private void RenderCharacterOverCharged()
        {
            character_Animator.SetBool("Is Carrying", false);
            character_SpriteRenderer.color = Color.black;
        }

        private void ResetCharacter()
        {
            character_Animator.SetBool("Is Carrying", false);
            character_SpriteRenderer.color = Color.white;
            isPowerActive = false;
        }

        private void DetermineOverChargeState()
        {
            var isOverCharged = currentPower_Animator.GetCurrentAnimatorStateInfo(0).IsName("Over Charged");
            var isChargeAnimationPlaying = currentPower_Animator.GetCurrentAnimatorStateInfo(0).IsName("Charging");

            if (isPowerActive)
            {
                if (isOverCharged)
                {
                    // enter overcharge
                    this.RenderCharacterOverCharged();
                    this.isOverCharged = true;
                }
            }
            else
            {
                if (isOverCharged)
                {
                    // exit overcharge
                    this.ResetCharacter();
                    currentPower_Animator.Play("No Power");
                    this.isOverCharged = false;
                }
            }
        }
        #endregion
    }
}
