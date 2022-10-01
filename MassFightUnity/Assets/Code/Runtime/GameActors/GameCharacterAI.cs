using System;
using System.Collections.Generic;
using c1tr00z.AssistLib.Utils;
using UnityEngine;

namespace c1tr00z.LD51.GameActors {
    [RequireComponent(typeof(GameCharacter))]
    public class GameCharacterAI : MonoBehaviour {

        #region Private Fields

        private GameCharacter _gameCharacter;

        private List<GameCharacter> _opponents = new List<GameCharacter>();

        private float _lastAttentionTimeCheck = 0;

        private GameCharacter _target;

        #endregion

        #region Serialized Fields

        [SerializeField] private float _attentionCooldown = 1;

        [SerializeField] private float _minAttackRange;
        
        [SerializeField] private float _maxAttackRange;

        #endregion

        #region Accessors

        private GameCharacter gameCharacter => this.GetCachedComponent(ref _gameCharacter);

        #endregion
        
        #region Unity Events

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.layer != (int)GameLayers.CHARACTERS) {
                return;
            }

            var character = other.gameObject.GetComponent<GameCharacter>();

            if (character == null || _opponents.Contains(character) || character.side == gameCharacter.side) {
                return;
            }
            
            _opponents.Add(character);
        }

        private void OnTriggerExit(Collider other) {
            if (other.gameObject.layer != (int)GameLayers.CHARACTERS) {
                return;
            }

            var character = other.gameObject.GetComponent<GameCharacter>();

            if (character == null || !_opponents.Contains(character)) {
                return;
            }

            _opponents.Remove(character);
        }

        private void Update() {
            if (gameCharacter.isPossessed || !gameCharacter.life.isAlive) {
                return;
            }
            AttentionCheck();
            TargetCheck();
        }

        #endregion

        #region Class Implementation

        private void AttentionCheck() {
            if ((!_target.IsNull() && _target.life.isAlive) || Time.time - _lastAttentionTimeCheck < _attentionCooldown) {
                return;
            }

            _lastAttentionTimeCheck = Time.time;

            if (_opponents.Count == 0) {
                return;
            }

            _target = _opponents.MinElement(c => (c.transform.position - transform.position).magnitude);
        }

        private void TargetCheck() {
            if (_target.IsNull() || !_target.life.isAlive || gameCharacter.isAttacked || !gameCharacter.navMeshAgent.enabled) {
                return;
            }

            var heading = (_target.transform.position - transform.position);
            var distance = heading.magnitude;
            if (distance >= _minAttackRange && distance <= _maxAttackRange) {
                transform.LookAt(_target.transform);
                gameCharacter.Attack();
                return;
            }

            if (distance < _minAttackRange) {
                var direction = heading / distance;
                var oppositeDirection = new Vector3(direction.x * -1, 0, direction.z * -1);
                var oppositePoint = transform.position + oppositeDirection * 10;
                gameCharacter.navMeshAgent.destination = oppositePoint;
                return;
            }

            if (distance > _maxAttackRange) {
                gameCharacter.navMeshAgent.destination = _target.transform.position;
                return;
            }
        }

        #endregion
    }
}