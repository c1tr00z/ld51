using c1tr00z.AssistLib.Utils;
using UnityEngine;

namespace c1tr00z.LD51.GameActors {
    public class GameCharacterAnimationEvents : MonoBehaviour {

        #region Private Fields

        private GameCharacter _character;

        #endregion

        #region Accessors

        private GameCharacter character => this.GetCachedComponentInParent(ref _character);

        #endregion

        #region Class Implementation

        public void ResetAttack() {
            character.ResetAttack();
        }

        public void AttackStarted() {
            character.EnableWeapon();
        }

        public void AttackFinished() {
            character.DisableWeapon();
        }

        #endregion

    }
}