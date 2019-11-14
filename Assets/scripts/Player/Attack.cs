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
            var inputActive = Input.GetMouseButtonDown(0);

            if (script_Player.currentWeapon)
            {
                script_Player.currentWeapon.DetermineWeaponDirection();
                script_Player.currentWeapon.DetermineAttackState(inputActive);
            }
        }
    }
}
