using UnityEngine;

namespace Player
{
    public class Attack : PlayerScriptMonoBehavior
    {
        public void Start()
        {
            this.SetCharacterComponents();
        }

        public void Update()
        {
            var inputActive = Input.GetAxisRaw("Fire1") > 0.0f;

            if (script_Player.currentWeapon)
            {
                script_Player.currentWeapon.DetermineWeaponDirection();
                script_Player.currentWeapon.DetermineAttackState(inputActive);
            }
        }
    }
}
