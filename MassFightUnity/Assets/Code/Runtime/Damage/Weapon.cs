using c1tr00z.LD51.GameActors;
using UnityEngine;

namespace Code.Runtime.Damage {
    public abstract class Weapon : MonoBehaviour {

        #region Protected Fields

        protected GameCharacter characterToIgnore;

        #endregion
        
        #region Serialized Fields

        [SerializeField] private int _damage;

        #endregion

        #region Accessors

        public int damageValue => _damage;

        #endregion

        #region Class Implementation

        public void Init(GameCharacter owner) {
            characterToIgnore = owner;
        }

        public abstract void StartAttack();

        public abstract void FinishAttack();

        #endregion

    }
}