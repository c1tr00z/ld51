using System.Collections;
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

        private Vector2 _playerInput;

        #endregion

        #region Serialized Fields

        [SerializeField] private Weapon _weapon;

        [SerializeField] private SkinnedMeshRenderer _meshRenderer;

        [SerializeField] private Material _redMaterial;
        
        [SerializeField] private Material _bluMaterial;

        [SerializeField] private Collider _mainCollider;

        #endregion

        #region Accessors

        protected Rigidbody rigidbody => this.GetCachedComponent(ref _rigidbody);

        public NavMeshAgent navMeshAgent => this.GetCachedComponent(ref _navMeshAgent);

        private Vector3 velocity => isPossessed ? rigidbody.velocity : navMeshAgent.velocity;

        protected GameCharacterAnimations animations => this.GetCachedComponent(ref _animations);

        public Life life => this.GetCachedComponent(ref _life);

        public bool isAttacked { get; private set; }

        #endregion

        #region Unity Events

        private void Awake() {
            _weapon.Init(this);
        }

        private void LateUpdate() {
            animations.Animate(velocity);
        }

        private void FixedUpdate() {
            MoveControlsFromPlayer();
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
            if (!life.isAlive) {
                return;
            }
            navMeshAgent.enabled = false;
            base.Possess(newSide);
        }

        public override void Unpossess() {
            navMeshAgent.enabled = true;
            _playerInput = Vector2.zero;
            base.Unpossess();
        }

        public override void Action() {
            Attack();
        }

        protected override void ChangeSide(Side newSide) {
            if (newSide != side) {
                throw new UnityException($"Side can not be not same. New: {newSide}, My: {side}");
            }

            _meshRenderer.material = side == Side.RED ? _redMaterial : _bluMaterial;
        }

        public override void MoveByPlayer(Vector2 input) {
            _playerInput = input;
        }

        #endregion

        #region Class Implementation

        public void OnDied() {
            navMeshAgent.enabled = false;
            animations.Die();
            rigidbody.isKinematic = true;
            _mainCollider.enabled = false;
            WaitAndDie();
        }

        public void ResetAttack() {
            isAttacked = false;
        }

        public void Attack() {
            if (isAttacked) {
                return;
            }
            animations.Attack();
            isAttacked = true;
        }

        public void EnableWeapon() {
            _weapon.StartAttack();
        }

        public void DisableWeapon() {
            _weapon.FinishAttack();
        }

        private void WaitAndDie() {
            StartCoroutine(C_WaitAndDestroy());
        }

        private IEnumerator C_WaitAndDestroy() {
            yield return new WaitForSeconds(5);
            Destroy(gameObject);
        }

        private void MoveControlsFromPlayer() {
            if (!isPossessed) {
                return;
            }
            // var forward = transform.forward * (_playerInput.y * navMeshAgent.speed);
            // var right = transform.right * (_playerInput.x * navMeshAgent.speed);
            // var newVelocity = (forward + right);
            // newVelocity.y = rigidbody.velocity.y;

            var newVelocity = new Vector3(_playerInput.x, 0, _playerInput.y) * navMeshAgent.speed;
            
            rigidbody.velocity = newVelocity;
        }

        #endregion
    }
}