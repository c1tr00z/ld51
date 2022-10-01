using UnityEngine;

namespace c1tr00z.LD51.GameActors {
    public abstract class GameActor : MonoBehaviour {
        
        #region Public Fields

        public Side side;

        #endregion

        #region Accessors

        public bool isPossessed { get; private set; }

        #endregion
        
        #region Class Implementation

        public virtual void Possess(Side newSide) {
            isPossessed = true;
        }

        public virtual void Unpossess(Side newSide) {
            isPossessed = false;
        }

        public abstract void Action();

        protected abstract void ChangeSide(Side newSide);

        #endregion

    }
}