using System;
using c1tr00z.AssistLib.AppModules;
using c1tr00z.AssistLib.DataModels;
using c1tr00z.AssistLib.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace c1tr00z.LD51.Gameplay.UI {
    public class GameEventControllerDataModel : DataModelBase {

        #region Private Fields

        private GameEventsController _gameEventsController;

        #endregion

        #region Public Fields

        public UnityEvent OnEventExecuted;

        #endregion

        #region Accessors

        public GameEventsController gameEventsController => ModulesUtils.GetCachedModule(ref _gameEventsController);

        public float timeLeft => gameEventsController.timeLeft;
        
        public float eventsCooldown => gameEventsController.eventsCooldown;

        public Quaternion timerRotation {
            get {
                var zAngle = -30f + 60 * timeLeft / eventsCooldown;
                return Quaternion.Euler(new Vector3(0, 0, zAngle));
            }
        }

        public int secondsLeft => Mathf.FloorToInt(timeLeft);

        public string secondsLeftString => secondsLeft.ToString("00");

        public float millisecondsLeft => timeLeft - secondsLeft;

        public string millisecondsLeftString => (millisecondsLeft * 100).ToString("00");

        public string eventTitle => gameEventsController.eventTitle;

        #endregion

        #region Unity Events

        private void OnEnable() {
            GameEventsController.Updated += GameEventsControllerOnUpdated;
            GameEventsController.EventExecuted += GameEventsControllerOnEventExecuted;
        }

        private void OnDisable() {
            GameEventsController.Updated -= GameEventsControllerOnUpdated;
            GameEventsController.EventExecuted -= GameEventsControllerOnEventExecuted;
        }

        #endregion

        #region Class Implementation

        private void GameEventsControllerOnUpdated() {
            OnDataChanged();
        }

        private void GameEventsControllerOnEventExecuted() {
            OnEventExecuted.SafeInvoke();
        }

        #endregion
    }
}