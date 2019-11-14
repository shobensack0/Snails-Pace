using UnityEngine;
using Player;
using UnityEngine.UI;

namespace Weapons
{
    public abstract class WeaponMonoBehavior : MonoBehaviour
    {
        #region Stats
        public bool isAttacking = false;
        public float staminaUsage;

        public DamageRange damageRange;
        #endregion

        #region Weapon
        protected Collider2D this_Collider;
        protected SpriteRenderer this_SpriteRenderer;
        protected Animator this_Animator;
        #endregion

        #region Character
        protected Animator character_Animator;
        protected Movement character_Movement;
        protected Power character_Power;
        protected Stats character_Stats;
        #endregion

        public void Awake()
        {
            this.TryGetComponent<Collider2D>(out this_Collider);
            this.TryGetComponent<SpriteRenderer>(out this_SpriteRenderer);
            this.TryGetComponent<Animator>(out this_Animator);
        }

        #region Implemented Methods
        public virtual void DetermineWeaponDirection()
        {
            if (character_Animator.GetFloat("Horizontal") != character_Movement.directionX)
            {
                this.ChangeWeaponDirection();
            }

            if (character_Animator.GetFloat("Vertical") != character_Movement.directionY)
            {
                this.ChangeWeaponSortOrder();
            }
        }

        public virtual void DetermineAttackState(bool inputActive)
        {
            var isWeaponIdle = this_Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle");

            if (character_Stats.HasStamina(staminaUsage))
            {
                if (inputActive && isWeaponIdle && character_Power.AllowOtherActions())
                {
                    character_Stats.TakeStamina(staminaUsage);
                    this_Animator.SetTrigger("Attack");
                    isAttacking = true;
                }
            }
            else
            {
                character_Stats.IndicateStaminaIsEmpty();
            }

            this.ResetAttackAnimations(inputActive, isWeaponIdle);
        }

        public void Equip(GameObject character)
        {
            character.TryGetComponent<Animator>(out character_Animator);
            character.TryGetComponent<Movement>(out character_Movement);
            character.TryGetComponent<Power>(out character_Power);
            character.TryGetComponent<Stats>(out character_Stats);

            // make sure weapon is facing the right before player actually moves
            this.ChangeWeaponDirection();
            this.ChangeWeaponSortOrder();
        }

        private void ResetAttackAnimations(bool inputActive, bool isWeaponIdle)
        {
            if (!inputActive && isWeaponIdle)
            {
                isAttacking = false;
            }
        }

        private void ChangeWeaponDirection()
        {
            if (character_Movement.directionX <= 0)
                this_Collider.transform.localEulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
            else
                this_Collider.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        }

        private void ChangeWeaponSortOrder()
        {
            if (character_Movement.directionY <= 0)
                this_SpriteRenderer.sortingOrder = 6;
            else
                this_SpriteRenderer.sortingOrder = 4;
        }
        #endregion
    }
}
