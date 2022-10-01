using UnityEngine;

namespace Code.Runtime.Common {
    public class SimpleAnimationPlayer : MonoBehaviour {

        #region Public Fields

        public Animation animation;

        #endregion

        #region Class Implementation

        public void Play() {
            animation.Play();
        }

        #endregion
    }
}