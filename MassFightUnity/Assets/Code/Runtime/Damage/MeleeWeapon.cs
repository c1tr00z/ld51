using System;
using System.Collections.Generic;
using c1tr00z.AssistLib.Utils;
using c1tr00z.LD51.GameActors;
using UnityEngine;

namespace Code.Runtime.Damage {
    public class MeleeWeapon : Weapon {

        #region Private Fields

        private List<GameCharacter> _damagedCharacters = new List<GameCharacter>();

        #endregion

        #region Serialized Fields

        [SerializeField]
        private Collider _damageCollider;

        #endregion

        #region Unity Events

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.layer != (int)GameLayers.CHARACTERS || other.isTrigger) {
                return;
            }

            var character = other.GetComponent<GameCharacter>();

            if (character.IsNull() || character == characterToIgnore || _damagedCharacters.Contains(character)) {
                Debug.LogError("No attack");
                return;
            }
            
            character.life.Damage(damageValue, this);
        }

        #endregion

        #region Weapon Implementation

        public override void StartAttack() {
            _damageCollider.enabled = true;
            _damagedCharacters.Clear();
        }

        public override void FinishAttack() {
            _damageCollider.enabled = false;
            _damagedCharacters.Clear();
        }

        #endregion
    }
}