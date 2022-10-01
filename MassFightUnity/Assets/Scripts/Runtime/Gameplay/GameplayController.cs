using c1tr00z.AssistLib.AppModules;
using c1tr00z.AssistLib.Common;
using UnityEngine;

namespace Runtime.Gameplay {
    public class GameplayController : Module {

        #region Accesors

        public Player player { get; private set; }

        #endregion
        
        #region Module Implementation

        public override void InitializeModule(CoroutineRequest request) {

            player = new GameObject("Player").AddComponent<Player>();
            
            base.InitializeModule(request);
        }

        #endregion
    }
}