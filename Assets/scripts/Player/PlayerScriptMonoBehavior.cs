using UnityEngine;

namespace Player
{
    /// <summary>
    /// Stuff you'll need everywhere
    /// </summary>
    public class PlayerScriptMonoBehavior  : MonoBehaviour
    {
        #region Character and Components
        protected Animator character_Animator;
        protected SpriteRenderer character_SpriteRenderer;
        protected Rigidbody2D character_RigidBody;
        #endregion

        #region Other Scripts
        protected Movement script_Movement;
        protected Power script_Power;
        protected Attack script_Attack;
        protected Player script_Player;
        protected Stats script_Stats;
        #endregion

        // TODO: should this be done at the parent level?
        //public void Start()
        //{
        //    this.SetCharacterComponents();
        //}

        protected void SetCharacterComponents()
        {
            this.TryGetComponent<Animator>(out character_Animator);
            this.TryGetComponent<SpriteRenderer>(out character_SpriteRenderer);
            this.TryGetComponent<Rigidbody2D>(out character_RigidBody);

            // scripts
            this.TryGetComponent<Movement>(out script_Movement);
            this.TryGetComponent<Power>(out script_Power);
            this.TryGetComponent<Attack>(out script_Attack);
            this.TryGetComponent<Player>(out script_Player);
            this.TryGetComponent<Stats>(out script_Stats);
        }
    }
}
