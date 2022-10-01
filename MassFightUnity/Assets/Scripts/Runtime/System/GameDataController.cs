using System.Collections;
using c1tr00z.AssistLib.AppModules;
using c1tr00z.AssistLib.Common;
using c1tr00z.AssistLib.Utils;
using UnityEngine;

namespace c1tr00z.LD51.System {
    public class GameDataController : Module {

        #region Readonly Fields

        private static readonly string PLAYER_PREFS_DATA_KEY = "MassBattlesData.Key";

        #endregion

        #region Private Fields

        private GameData _gameData;

        #endregion

        #region Accessors

        public GameData gameData => _gameData;

        #endregion

        #region Module Implementation

        public override void InitializeModule(CoroutineRequest request) {
            StartCoroutine(C_Initialize(request));
        }

        #endregion

        #region Class Implementation

        private IEnumerator C_Initialize(CoroutineRequest request) {

            var playerPrefsString = PlayerPrefs.GetString(PLAYER_PREFS_DATA_KEY);

            if (!playerPrefsString.IsNullOrEmpty()) {
                _gameData = JSONUtuls.Deserialize<GameData>(playerPrefsString);
            }

            yield return null;
            
            base.InitializeModule(request);
        }

        public void Save() {
            var jsonString = gameData.ToJsonString();
            
            PlayerPrefs.SetString(PLAYER_PREFS_DATA_KEY, jsonString);
            PlayerPrefs.Save();
        }

        public void SetPlayerName(string newName) {
            _gameData.playerName = newName;
            Save();
        }

        #endregion
    }
}