using Player;
using System;
using UnityEngine;

namespace GameWorld
{
    /// <summary>
    /// Stuff you'll need everywhere.
    /// 
    /// NPC's are interactive, friendly characters in the game world.
    /// </summary>
    public class NPCTest : InteractiveGameObject
    {
        public override void SetColliderRadius()
        {
        }

        public override void Start()
        {
            base.Start();
            SetColliderRadius();
        }

        public void StartDialogue(int id, DialoguerCallback callback)
        {
            Dialoguer.StartDialogue(id, callback);
        }
    }
}

