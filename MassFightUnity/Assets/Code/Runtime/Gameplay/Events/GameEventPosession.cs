using c1tr00z.AssistLib.AppModules;
using c1tr00z.LD51.GameActors;

namespace c1tr00z.LD51.Gameplay {
    public abstract class GameEventPosession : GameEventBase {

        #region Private Fields

        private GameActorsController _actorsController;

        #endregion

        #region Accessors

        protected GameActorsController actorsController => ModulesUtils.GetCachedModule(ref _actorsController);

        #endregion
    }
}