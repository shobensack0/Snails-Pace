using UnityEngine;

namespace Player
{
    public class Attack : PlayerScriptMonoBehavior
    {
        #region Public Properties
        public bool isAttacking = false;
        #endregion

        #region Weapon and Components
        public GameObject currentWeapon;

        private Animator currentWeapon_Animator;
        private SpriteRenderer currentWeapon_SpriteRenderer;
        private PolygonCollider2D currentWeapon_Collider;
        #endregion

        public void Start()
        {
            this.EquipWeapon(currentWeapon);
            this.SetCharacterComponents();
        }

        public void Update()
        {
            var inputActive = Input.GetAxisRaw("Fire1") > 0.0f;

            this.DetermineWeaponDirection();
            this.DetermineAttackState(inputActive);
        }

        #region Public Methods

        #endregion

        #region Private Methods
        private void DetermineAttackState(bool inputActive)
        {
            var isWeaponIdle = currentWeapon_Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle");

            if (inputActive && isWeaponIdle && script_Power.AllowOtherActions())
            {
                currentWeapon_Animator.SetTrigger("Attack");
                isAttacking = true;
            }

            this.ResetAttackAnimations(inputActive, isWeaponIdle);
        }

        private void EquipWeapon(GameObject weapon)
        {
            if (weapon == null)
                return;

            currentWeapon = weapon;

            weapon.TryGetComponent<SpriteRenderer>(out currentWeapon_SpriteRenderer);
            weapon.TryGetComponent<Animator>(out currentWeapon_Animator);
            weapon.TryGetComponent<PolygonCollider2D>(out currentWeapon_Collider);
        }

        private void ResetAttackAnimations(bool inputActive, bool isWeaponIdle)
        {
            if (!inputActive && isWeaponIdle)
            {
                isAttacking = false;
            }
        }

        private void DetermineWeaponDirection()
        {
            if (character_Animator.GetFloat("Horizontal") != script_Movement.directionX)
            {
                if (script_Movement.directionX <= 0)
                    currentWeapon_Collider.transform.localEulerAngles = new Vector3(0f, 180.0f, 0f);
                else
                    currentWeapon_Collider.transform.localEulerAngles = new Vector3(0f, 0.0f, 0f);
            }

            if (character_Animator.GetFloat("Vertical") != script_Movement.directionY)
            {
                if (script_Movement.directionY <= 0)
                    currentWeapon_SpriteRenderer.sortingOrder = 6;
                else
                    currentWeapon_SpriteRenderer.sortingOrder = 4;
            }
        }
        #endregion
    }
}
