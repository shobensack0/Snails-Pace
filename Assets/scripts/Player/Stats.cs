using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class Stats : PlayerScriptMonoBehavior
    {
        #region Publics
        public GameObject healthBar;
        public GameObject staminaBar;
        public GameObject magicBar;

        public GameObject healthBarFill;
        public GameObject staminaBarFill;
        public GameObject magicBarFill;
        #endregion

        #region Stats
        public float playerMaxHealth = 100.0f;
        public float playerCurrentHealth = 100.0f;
        public float playerHealthIncrement = 0.0f;

        public float playerMaxStamina = 100.0f;
        public float playerCurrentStamina = 100.0f;
        public float playerStaminaIncrement = 0.0f;

        public float playerMaxMagic = 100.0f;
        public float playerCurrentMagic = 100.0f;
        public float playerMagicIncrement = 0.0f;

        private bool canRegenerateStamina = true;
        private float staminaRegenerationRate = .50f;
        #endregion

        #region privates
        private float flashSpeed = 20.0f;

        private bool staminaIsFlashing = false;
        #endregion

        void Start()
        {
            this.SetCharacterComponents();

            // determine sizes of fill and health to figure out how to scale
            this.ScaleStatsUI(healthBar, healthBarFill, playerMaxHealth, playerCurrentHealth, ref playerHealthIncrement);
            this.ScaleStatsUI(staminaBar, staminaBarFill, playerMaxStamina, playerCurrentStamina, ref playerStaminaIncrement);
            //this.ScaleStatsUI(magicBar, magicBarFill, playerMaxMagic, playerCurrentMagic, playerMagicIncrement);
        }

        void Update()
        {
            this.DetermineStaminaBarColor();

            if (canRegenerateStamina && playerCurrentStamina < playerMaxStamina)
            {
                this.UpdateStat(staminaBarFill, ref playerCurrentStamina, playerStaminaIncrement, playerMaxStamina, staminaRegenerationRate);
            }

            // always set, will update next frame if draining stamina
            canRegenerateStamina = true;
        }

        public void TakeDamage(float amount, Vector2 knockbackForce = default)
        {
            this.UpdateStat(healthBarFill, ref playerCurrentHealth, playerHealthIncrement, playerMaxHealth, -amount);

            if (knockbackForce != default)
            {
                this.character_RigidBody.AddForce(knockbackForce);
            }
        }

        public void TakeStamina(float amount)
        {
            canRegenerateStamina = false;
            this.UpdateStat(staminaBarFill, ref playerCurrentStamina, playerStaminaIncrement, playerMaxStamina, -amount);
        }

        public bool HasStamina(float amountToTake)
        {
            return (playerCurrentStamina - amountToTake) > 0.0f;
        }

        #region privates
        private void UpdateStat(GameObject barFill, ref float currentAmount, float increment, float max, float amount)
        {
            var barImageFill = barFill.GetComponent<Image>();

            if (barImageFill && ((currentAmount + amount) <= max))
            {
                var negate = (amount < 0) ? -1 : 1;

                currentAmount += amount;
                var barRect = barImageFill.transform as RectTransform;
                barRect.sizeDelta = new Vector2(barRect.sizeDelta.x + (increment  * amount), barRect.sizeDelta.y);
            }
        }

        private void ScaleStatsUI(GameObject statsBar, GameObject statsBarFill, float max, float current, ref float incrementToScale)
        {
            var barImage = statsBar.GetComponent<Image>();
            var barImageFill = statsBarFill.GetComponent<Image>();

            if (barImage && barImageFill)
            {
                var barImageRect = barImage.transform as RectTransform;
                var barImageFillRect = barImageFill.transform as RectTransform;

                incrementToScale = barImageRect.sizeDelta.x / max;
                barImageFillRect.sizeDelta = new Vector2(barImageRect.sizeDelta.x - 40, barImageFillRect.sizeDelta.y);
            }
        }

        private void DetermineStaminaBarColor()
        {

            if (playerCurrentStamina < playerMaxStamina)
            {
                staminaBar.TryGetComponent<Image>(out Image staminaBarImage);
                staminaBarFill.TryGetComponent<Image>(out Image staminaBarImageFill);

                // stamina
                var maxStaminaBarRatio = (255 / playerMaxStamina) * playerCurrentStamina;
                staminaBarImage.color = new Color(staminaBarImage.color.r, maxStaminaBarRatio * Time.deltaTime, staminaBarImage.color.b);
                staminaBarImageFill.color = new Color(staminaBarImage.color.r, maxStaminaBarRatio * Time.deltaTime, staminaBarImage.color.b);
            }

            //    if (staminaIsFlashing)
            //{
                
            //        staminaBarImage.color = new Color(staminaBarImage.color.r, staminaBarImage.color.g - (flashSpeed * Time.deltaTime), staminaBarImage.color.b - (flashSpeed * Time.deltaTime));
            //    else
            //        staminaIsFlashing = false;
            //}
            //else
            //{
            //    // Todo: move this out for a performance boost
            //    staminaBar.TryGetComponent<Image>(out Image staminaBarImage);
            //    if (staminaBarImage && staminaBarImage.color.g < 255.0f && staminaBarImage.color.b < 255.0f)
            //    {
            //        Debug.Log("test");
            //        staminaBarImage.color = new Color(staminaBarImage.color.r, staminaBarImage.color.g + (flashSpeed * Time.deltaTime), staminaBarImage.color.b + (flashSpeed * Time.deltaTime));
            //    }
            //}
        }
        #endregion
    }
}
