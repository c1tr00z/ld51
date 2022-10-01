using c1tr00z.AssistLib.Utils;
using Code.Runtime.Damage;
using UnityEngine;
using UnityEngine.AI;
using NotImplementedException = System.NotImplementedException;

namespace c1tr00z.LD51.GameActors {
    public class GameCharacter : GameActor {

        #region Private Fields

        private NavMeshAgent _navMeshAgent;

        private Rigidbody _rigidbody;

        private GameCharacterAnimations _animations;

        private Life _life;

        #endregion

        #region Serialized Fields

        [SerializeField] private SkinnedMeshRenderer _meshRenderer;

        [SerializeField] private Material _redMaterial;
        
        [SerializeField] private Material _bluMaterial;

        #endregion

        #region Accessors

        protected Rigidbody rigidbody => this.GetCachedComponent(ref _rigidbody);

        private NavMeshAgent navMeshAgent => this.GetCachedComponent(ref _navMeshAgent);

        private Vector3 velocity => isPossessed ? rigidbody.velocity : navMeshAgent.velocity;

        protected GameCharacterAnimations animations => this.GetCachedComponent(ref _animations);

        public Life life => this.GetCachedComponent(ref _life);

        #endregion

        #region Unity Events

        private void LateUpdate() {
            animations.Animate(velocity);
        }

        #endregion

        #region GameActor Implementation

        public void Init(Side newSide) {
            if (newSide == Side.NEUTRAL) {
                throw new UnityException($"Side can not be {Side.NEUTRAL}");
            }
            side = newSide;
            ChangeSide(side);
        }

        public override void Possess(Side newSide) {
            navMeshAgent.enabled = false;
            base.Possess(newSide);
        }

        public override void Unpossess(Side newSide) {
            navMeshAgent.enabled = true;
            base.Unpossess(newSide);
        }

        public override void Action() {
            
        }

        protected override void ChangeSide(Side newSide) {
            if (newSide != side) {
                throw new UnityException($"Side can not be not same. New: {newSide}, My: {side}");
            }

            _meshRenderer.material = side == Side.RED ? _redMaterial : _bluMaterial;
        }

        public void OnDied() {
            Destroy(gameObject);
        }

        #endregion
    }
}