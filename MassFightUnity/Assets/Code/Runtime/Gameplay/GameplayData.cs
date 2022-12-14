using System;
using System.Collections.Generic;
using c1tr00z.AssistLib.ResourcesManagement;
using c1tr00z.LD51.GameActors;
using UnityEngine;

namespace c1tr00z.LD51.Gameplay {
    public class GameplayData : DBEntry {

        #region Nested Classes

        [Serializable]
        public class CharacterPerSide {
            public GameCharacterDBEntry dbEntry;
            public int amount;
        }

        #endregion
        
        #region Public Fields

        public List<CharacterPerSide> charactersPerSide;

        public float eventCooldown = 10;

        #endregion

    }
}