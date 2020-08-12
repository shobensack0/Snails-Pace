using Player;
using UnityEngine;
using UnityEngine.UI;

namespace GameWorld
{
    /// <summary>
    /// Stuff you'll need everywhere.
    /// 
    /// NPC's are interactive, friendly characters in the game world.
    /// </summary>
    public abstract class InteractiveGameObject : InteractiveMonoBehavior
    {
        #region Attributes
        /// <summary>
        /// The radius of the circle collider that represents the area in which the player is able to interact with this object
        /// 
        /// This either gets set to a percent of the objects sprite by default, or can be set accordingly
        /// </summary>
        //public float interactRadius;
        public bool playerCanInteract;
        #endregion

        #region Dialogue Properties
        public bool showingDialogue;
        public string dialogueText;
        #endregion

        #region Abstract Methods
        public abstract void SetColliderRadius();
        #endregion

        public virtual void Start()
        {
            SetCharacterComponents();
            SetColliderRadius();

            InitDialougerEvents();
        }

        // Update is called once per frame
        public virtual void Update()
        {
            if (playerCanInteract && Input.GetKeyDown(KeyCode.E))
            {
                this.player_Script_Player.allowPlayerInput = false;

            }
        }

        public virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                character_InteractivePromptSpriteRenderer.enabled = true;
                playerCanInteract = true;
            }
        }

        public virtual void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                character_InteractivePromptSpriteRenderer.enabled = false;
                playerCanInteract = false;
            }
        }

        /// <summary>
        /// Called when this object is initialized to setup dialogue for this object.
        /// </summary>
        public void InitDialougerEvents()
        {
            Dialoguer.events.onStarted += OnStarted;
            Dialoguer.events.onEnded += OnEnded;
            Dialoguer.events.onTextPhase += OnTextPhase;
        }

        public void OnGUI()
        {
            if (!showingDialogue)
                return;

            GUI.Box(new Rect(10, 10, 200, 200), dialogueText);
        }

        /// <summary>
        /// Method to be called when dialogue starts
        /// </summary>
        private void OnStarted()
        {
            this.showingDialogue = true;
        }

        /// <summary>
        /// Method to be called when dialogue ends
        /// </summary>
        private void OnEnded()
        {
            this.showingDialogue = false;
        }

        /// <summary>
        /// Method called when text is to be displayed.
        /// 
        /// </summary>
        /// <param name="data">The actual text data that needs to be displayed on screen.</param>
        private void OnTextPhase(DialoguerTextData data)
        {
            dialogueText = data.text;
        }
    }
}

