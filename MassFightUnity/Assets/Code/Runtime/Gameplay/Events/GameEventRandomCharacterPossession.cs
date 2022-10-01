using c1tr00z.AssistLib.Utils;

namespace c1tr00z.LD51.Gameplay {
    public class GameEventRandomCharacterPossession : GameEventPosession {
        
        #region GameEventBase Implementation

        public override string title => "Random Character Possession";
        
        public override void Execute(Player player) {
            player.Possess(actorsController.gameCharacters.RandomItem());
        }

        #endregion
    }
}