using c1tr00z.AssistLib.Json;
using c1tr00z.AssistLib.Utils;

namespace c1tr00z.LD51.System {
    public struct GameData : IJsonSerializable, IJsonDeserializable {

        #region Json Fields

        [JsonSerializableField]
        public string playerName;

        #endregion

        #region Accessors

        public bool isEmpty => playerName.IsNullOrEmpty();

        #endregion

    }
}