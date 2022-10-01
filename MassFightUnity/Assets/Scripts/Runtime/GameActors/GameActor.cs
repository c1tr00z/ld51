using UnityEngine;

namespace c1tr00z.LD51.GameActors {
    public abstract class GameActor : MonoBehaviour {

        #region Accessors

        public bool isPossessed { get; private set; }

        #endregion
        
        #region Class Implementation

        public virtual void Possess() {
            isPossessed = true;
        }

        public virtual void Unpossess() {
            isPossessed = false;
        }

        public abstract void Action();

        #endregion

    }
}