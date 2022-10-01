using System;
using c1tr00z.AssistLib.AppModules;
using c1tr00z.AssistLib.Utils;
using c1tr00z.LD51.GameActors;
using Code.Runtime.Damage;
using UnityEngine;
using UnityEngine.InputSystem;

namespace c1tr00z.LD51.Gameplay {
    public class Player : MonoBehaviour {

        #region Events

        public static event Action Updated;

        #endregion

        #region Private Fields

        private GameActor _posessedActor;

        private bool _isDoAction;

        #endregion

        #region Serialized Fields

        [SerializeField] private Camera _camera;

        #endregion

        #region Accessors

        private GameCharacter gameCharacter => _posessedActor as GameCharacter;

        public Side currentSide { get; private set; } = Side.NEUTRAL;

        #endregion

        #region Unity Events

        private void OnEnable() {
            Life.Died += LifeOnDied;
        }

        private void OnDisable() {
            Life.Died -= LifeOnDied;
        }

        private void LateUpdate() {
            if (_posessedActor == null) {
                return;
            }

            UpdateRotation();
            
            transform.position = _posessedActor.transform.position;
        }

        #endregion

        #region Class Implementation

        public void UnpossessCurrent() {
            if (_posessedActor.IsNull()) {
                return;
            }
            transform.parent = null;
            _posessedActor.Unpossess();
            _posessedActor = null;
            Updated?.Invoke();
        }

        public void Possess(GameActor newActor) {
            UnpossessCurrent();

            if (newActor == null) {
                return;
            }
            
            newActor.Possess(newActor.side);
            _posessedActor = newActor;
            currentSide = _posessedActor.side;
            Updated?.Invoke();
        }

        public void Move(InputAction.CallbackContext callbackContext) {
            if (gameCharacter == null) {
                return;
            }
            
            gameCharacter.MoveByPlayer(callbackContext.ReadValue<Vector2>());
        }

        public void Action(InputAction.CallbackContext callbackContext) {
            if (_posessedActor.IsNull()) {
                return;
            }
            
            var value = callbackContext.ReadValue<float>() > 0;
            if (_isDoAction == value) {
                return;
            }

            _isDoAction = value;

            if (!_isDoAction) {
                return;
            }
            _posessedActor.Action();
        }
        
        private void LifeOnDied(Life life) {
            if (gameCharacter == null) {
                return;
            }

            if (gameCharacter.life == life) {
                UnpossessCurrent();
            }
        }

        private void UpdateRotation() {
            if (_posessedActor.IsNull()) {
                return;
            }

            var actorScreenPosition = _camera.WorldToScreenPoint(_posessedActor.transform.position).ToVector2();
            var mousePosition = Input.mousePosition.ToVector2();
            var heading = mousePosition - actorScreenPosition;
            var direction2D = heading / heading.magnitude;

            var direction = new Vector3(direction2D.x, 0, direction2D.y);
            
            _posessedActor.transform.rotation = Quaternion.LookRotation(direction);
        }

        #endregion

    }
}