using System.Collections.Generic;
using c1tr00z.AssistLib.DataModels;
using c1tr00z.AssistLib.PropertyReferences;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace c1tr00z.LD51.Common {
    public class ValueReceiverRotation : ValueReceiverBase {

        #region Public Fields

        [ReferenceType(typeof(Quaternion))]
        public PropertyReference rotationRef; 

        public Transform target;

        #endregion

        #region ValueReceiverBase Implementation

        public override IEnumerator<PropertyReference> GetReferences() {
            yield return rotationRef;
        }

        public override void UpdateReceiver() {
            target.localRotation = rotationRef.Get<Quaternion>();
        }

        #endregion
    }
}