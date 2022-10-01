using System.Collections.Generic;
using c1tr00z.AssistLib.DataModels;
using c1tr00z.AssistLib.PropertyReferences;
using TMPro;

namespace c1tr00z.LD51.Common {
    public class ValueReceiverTextTMP : ValueReceiverBase {

        #region Public Fields

        [ReferenceType(typeof(string))] public PropertyReference textRef;
        
        public TMP_Text text;

        #endregion

        #region ValueReceiverBase Implementation

        public override IEnumerator<PropertyReference> GetReferences() {
            yield return textRef;
        }

        public override void UpdateReceiver() {
            text.text = textRef.Get<string>();
        }

        #endregion
    }
}