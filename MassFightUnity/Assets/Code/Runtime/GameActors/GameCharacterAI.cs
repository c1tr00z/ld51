using System;
using System.Collections.Generic;
using c1tr00z.AssistLib.Utils;
using UnityEngine;

namespace c1tr00z.LD51.GameActors {
    [RequireComponent(typeof(GameCharacter))]
    public class GameCharacterAI : MonoBehaviour {

        #region Private Fields

        private GameCharacter _gameCharacter;

        private List<GameCharacter> _opponents;

        #endregion

        #region Accessors

        private GameCharacter gameCharacter => this.GetCachedComponent(ref _gameCharacter);

        #endregion
        
        #region Unity Events

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.layer != (int)GameLayers.CHARACTERS) {
                return;
            }
            
        }

        #endregion
    }
}