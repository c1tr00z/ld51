using System;
using c1tr00z.AssistLib.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Runtime.Damage {
    public class Life : MonoBehaviour {

        #region Events

        public static event Action<Life> Died; 

        #endregion

        #region Public Fields

        public int maxHP;

        public int damage;

        public UnityEvent OnDamaged;
        
        public UnityEvent OnDied;

        #endregion

        #region Accessors

        public int hp => maxHP - damage;

        public bool isAlive => hp > 0;

        #endregion

        #region Unity Events

        private void Start() {
            damage = 0;
        }

        #endregion

        #region Class Implementation

        public void Damage(int newDamage, Weapon weapon) {
            if (!isAlive) {
                return;
            }
            damage += newDamage;
            if (damage >= maxHP) {
                OnDied.SafeInvoke();
                Died?.Invoke(this);
                return;
            }
            
            OnDamaged.SafeInvoke();
        }

        #endregion
    }
}