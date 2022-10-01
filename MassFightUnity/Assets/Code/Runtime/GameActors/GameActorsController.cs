using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using c1tr00z.AssistLib.AppModules;
using c1tr00z.AssistLib.Common;
using c1tr00z.AssistLib.ResourcesManagement;
using c1tr00z.AssistLib.Utils;
using Code.Runtime.Damage;
using UnityEngine;

namespace c1tr00z.LD51.GameActors {
    public class GameActorsController : Module {

        #region Unity Events

        public static event Action Initialized;

        #endregion

        #region Nested Classes

        private class CharacterRequest {
            public GameCharacterDBEntry dbEntry;
            public Side side;
        }

        #endregion

        #region Private Fields

        private Queue<GameCharacter> _toDelete = new Queue<GameCharacter>();

        private List<GameActor> _allGameActors = new List<GameActor>();

        private Queue<CharacterRequest> _toSpawn = new Queue<CharacterRequest>();

        private Dictionary<GameCharacterDBEntry, GameCharacter> _characterPrefabs =
            new Dictionary<GameCharacterDBEntry, GameCharacter>();

        #endregion

        #region Accessors

        public List<GameCharacter> gameCharacters { get; private set; } = new List<GameCharacter>();

        public List<GameActor> allGameActors {
            get {
                if (_allGameActors.Count == 0) {
                    _allGameActors = FindObjectsOfType<GameActor>().ToList();
                    _allGameActors.RemoveAll(a => gameCharacters.Contains(a));
                }

                return _allGameActors;
            }
        }

        #endregion

        #region Unity Events

        private void OnEnable() {
            Life.Died += LifeOnDied;
        }

        private void OnDisable() {
            Life.Died -= LifeOnDied;
        }

        private void Update() {

            while (!isInitialized) {
                return;
            }
            
            if (Remove()) {
                return;
            }

            if (Spawn()) {
                return;
            }
        }

        #endregion

        #region Module Implementation

        public override void InitializeModule(CoroutineRequest request) {
            StartCoroutine(C_InitializeModule(request));
        }

        #endregion

        #region Class Implementation

        private IEnumerator C_InitializeModule(CoroutineRequest request) {

            var allDBEntries = DB.GetAll<GameCharacterDBEntry>();
            
            foreach (var characterDBEntry in allDBEntries) {
                var characterRequest = characterDBEntry.LoadPrefabAsync<GameCharacter>();
                yield return characterRequest;
                _characterPrefabs.Add(characterDBEntry, characterRequest.asset);
            }
            
            base.InitializeModule(request);
            Initialized?.Invoke();
        }

        private void LifeOnDied(Life life) {
            var character = gameCharacters.FirstOrDefault(c => c.life == life);
            if (!character.IsNull()) {
                _toDelete.Enqueue(character);
            }
        }

        public void AskToSpawn(GameCharacterDBEntry dbEntry, Side side) {
            if (side == Side.NEUTRAL) {
                return;
            }
            
            _toSpawn.Enqueue(new CharacterRequest {
                dbEntry = dbEntry,
                side = side,
            });
        }

        private bool Remove() {
            if (_toDelete.Count == 0) {
                return false;
            }

            var characterToDelete = _toDelete.Dequeue();
            if (characterToDelete.IsNull()) {
                gameCharacters = gameCharacters.SelectNotNull();
                return true;
            }

            if (gameCharacters.Contains(characterToDelete)) {
                gameCharacters.Remove(characterToDelete);
                return true;
            }

            return true;
        }

        private bool Spawn() {
            if (_toSpawn.Count == 0) {
                return false;
            }

            var request = _toSpawn.Dequeue();

            if (_characterPrefabs.ContainsKey(request.dbEntry)) {
                var character = _characterPrefabs[request.dbEntry].Clone();
                character.name = request.dbEntry.name;
                character.Init(request.side);
                character.transform.position =
                    VectorUtils.RandomV3(new Vector3(-30, .3f, -30), new Vector3(30, .35f, 30));
            }

            return true;
        }

        #endregion
    }
}