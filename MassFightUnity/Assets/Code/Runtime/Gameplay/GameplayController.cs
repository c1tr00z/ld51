using System;
using System.Collections.Generic;
using c1tr00z.AssistLib.AppModules;
using c1tr00z.AssistLib.Common;
using c1tr00z.AssistLib.ResourcesManagement;
using c1tr00z.AssistLib.Utils;
using c1tr00z.LD51.GameActors;
using UnityEngine;

namespace Runtime.Gameplay {
    public class GameplayController : Module {

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

        #endregion
    }
}