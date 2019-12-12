using UnityEngine;
using Weapons;

namespace Player
{
    public class Player : PlayerScriptMonoBehavior
    {
        #region Inventory
        public WeaponMonoBehavior[] weapons;
        public WeaponMonoBehavior currentWeapon;
        #endregion

        #region Utilities
        public bool isDebug = false;
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            this.SetCharacterComponents();
        }

        // Update is called once per frame
        void Update()
        {

            // TODO: obviously we need to set up an inventory system
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                this.EquipWeapon(0);

            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                this.EquipWeapon(1);
            }

            this.ConsoleDebug(isDebug);
        }

        private void EquipWeapon(int weaponIndex)
        {
            // remove current weapon
            foreach(var child in GetComponentsInChildren<Transform>(true))
            {
                if (child.tag == "Weapon")
                {
                    Destroy(child.gameObject);
                }
            }

            // add new weapon
            currentWeapon = Instantiate(weapons[weaponIndex], this.transform);
            currentWeapon.Equip(this.gameObject);
        }

        #region debug
        private void ConsoleDebug(bool isDebug = false)
        { }
        #endregion
    }
}