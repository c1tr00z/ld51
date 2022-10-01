using System;
using System.Collections.Generic;
using System.Linq;
using c1tr00z.AssistLib.AppModules;
using c1tr00z.AssistLib.Common;
using c1tr00z.AssistLib.ResourcesManagement;
using c1tr00z.AssistLib.Utils;
using c1tr00z.LD51.GameActors;
using Code.Runtime.Damage;
using UnityEngine;

namespace c1tr00z.LD51.Gameplay {
    public class GameplayController : Module {

        #region Events

        public static event Action<bool> GameStop;

        #endregion

        #region Private Fields

        private GameplayData _gameplayData;

        private GameActorsController _actorsController;

        #endregion

        #region Public Fields

        public Player playerPrefab;

        #endregion
        
        #region Accesors

        public Player player { get; private set; }

        private GameplayData gameplayData => DBEntryUtils.GetCached(ref _gameplayData);

        private GameActorsController actorsController => ModulesUtils.GetCachedModule(ref _actorsController);

        #endregion

        #region Unity Events

        private void OnEnable() {
            GameActorsController.Initialized += GameActorsControllerOnInitialized;
            Life.Died += LifeOnDied;
        }

        private void OnDisable() {
            GameActorsController.Initialized -= GameActorsControllerOnInitialized;
        }

        #endregion
        
        #region Module Implementation

        public override void InitializeModule(CoroutineRequest request) {

            player = playerPrefab.Clone();
            player.name = playerPrefab.name;
            
            base.InitializeModule(request);
        }

        #endregion

        #region Class Implementation

        private void GameActorsControllerOnInitialized() {
            new List<Side> { Side.RED, Side.BLU }.ForEach(side => {
                gameplayData.charactersPerSide.ForEach(perSide => {
                    for (int i = 0; i < perSide.amount; i++) {
                        actorsController.AskToSpawn(perSide.dbEntry, side);
                    }
                });
            });
        }

        private void LifeOnDied(Life obj) {
            var allAlive = Modules.Get<GameActorsController>().gameCharacters.Where(c => c.life.isAlive).ToList();
            var playerSided = allAlive.Where(c => c.side == player.currentSide).ToList();
            if (playerSided.Count == 0) {
                GameStop?.Invoke(false);
                return;
            }

            var oppositeSided = allAlive.Where(c => c.side != player.currentSide).ToList();
            if (oppositeSided.Count == 0) {
                GameStop?.Invoke(true);
            }
        }

        #endregion
    }
}