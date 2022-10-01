using c1tr00z.AssistLib.Utils;
using UnityEngine;

namespace c1tr00z.LD51.GameActors {
    public class GameCharacterAnimations : MonoBehaviour {

        #region Private Fields

        private Animator _animator;

        #endregion

        #region Accessors

        private Animator animator => this.GetCachedComponentInChildren(ref _animator);

        #endregion

        #region Class Implementation

        public void Animate(Vector3 velocity) {
            if (velocity == Vector3.zero) {
                animator.SetFloat("Forward", 0);
                animator.SetFloat("Right", 0);
                return;
            }

            var directionDiffY = Vector3.Dot(transform.forward, velocity);
            var directionDiffX = Vector3.Dot(transform.right, velocity);
            //
            animator.SetFloat("Forward", directionDiffY);
            animator.SetFloat("Right", directionDiffX);
        }

        public void Attack() {
            animator.SetTrigger("Attack");
        }

        public void Die() {
            animator.SetBool("Dead", true);
        }

        #endregion
    }
}