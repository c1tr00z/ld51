namespace c1tr00z.LD51.Gameplay {
    public abstract class GameEventBase {

        #region Accessors

        public abstract string title { get; }

        #endregion

        #region Class Implementation

        public abstract void Execute(Player player);

        #endregion

    }
}