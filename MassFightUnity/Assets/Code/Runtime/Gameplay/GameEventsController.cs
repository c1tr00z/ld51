using System;
using System.Collections.Generic;
using c1tr00z.AssistLib.AppModules;
using c1tr00z.AssistLib.Common;
using c1tr00z.AssistLib.ResourcesManagement;
using c1tr00z.AssistLib.Utils;
using UnityEngine;

namespace c1tr00z.LD51.Gameplay {
    public class GameEventsController : Module {

        #region Events

        public static event Action Updated;

        public static event Action EventExecuted;

        #endregion

        #region Private Fields

        private List<GameEventBase> _events = new List<GameEventBase>();

        private GameplayData _gameplayData;

        private GameEventBase _currentEvent = null;

        private Player _player;

        private bool _isRolling = true;

        #endregion

        #region Accessors

        public float timeLeft { get; private set; }

        private GameplayData gameplayData => DBEntryUtils.GetCached(ref _gameplayData);

        public float eventsCooldown => gameplayData.eventCooldown;

        public bool hasEvent => _currentEvent != null;

        public string eventTitle => hasEvent ? _currentEvent.title : "--???--";

        public Player player => CommonUtils.GetCachedObject(ref _player, () => Modules.Get<GameplayController>()?.player);

        #endregion

        #region Unity Events

        private void OnEnable() {
            
        }

        private void OnDisable() {
            
        }

        private void Update() {
            if (!isInitialized) {
                return;
            }
            if (timeLeft <= 0) {
                timeLeft = gameplayData.eventCooldown;
                RollAndExecute();
                return;
            }
            timeLeft -= Time.deltaTime;
            Updated?.Invoke();
        }

        #endregion

        #region Module Implementation

        public override void InitializeModule(CoroutineRequest request) {

            _events = ReflectionUtils.GetSubclassesOf<GameEventBase>(false)
                .SelectNotNull(type => (GameEventBase)Activator.CreateInstance(type));
            
            base.InitializeModule(request);
        }

        private void RollAndExecute() {
            _currentEvent = _events.RandomItem();
            _currentEvent.Execute(player);
            EventExecuted?.Invoke();
        }

        #endregion
    }
}