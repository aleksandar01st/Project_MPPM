﻿using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{
    public class ReactiveCapabilityCurve : Curve
    {
        private List<long> synchronousMachine = new List<long>();

        public ReactiveCapabilityCurve(long globalId) : base(globalId)
        {
        }

        public List<long> SynchronousMachine
        {
            get { return synchronousMachine; }
            set { synchronousMachine = value; }
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                ReactiveCapabilityCurve x = (ReactiveCapabilityCurve)obj;
                return (CompareHelper.CompareLists(x.synchronousMachine, this.synchronousMachine));
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #region IAccess implementation

        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.REACTIVE_CAPABILITY_CURVE_SYM:
                    return true;

                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property prop)
        {
            switch (prop.Id)
            {
                case ModelCode.REACTIVE_CAPABILITY_CURVE_SYM:
                    prop.SetValue(synchronousMachine);
                    break;

                default:
                    base.GetProperty(prop);
                    break;
            }
        }

        public override void SetProperty(Property property)
        {
            switch (property.Id)
            {
                default:
                    base.SetProperty(property);
                    break;
            }
        }

        #endregion IAccess implementation


        #region IReference implementation

        public override bool IsReferenced
        {
            get
            {
                return synchronousMachine.Count > 0 || base.IsReferenced;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (synchronousMachine != null && synchronousMachine.Count != 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.REACTIVE_CAPABILITY_CURVE_SYM] = synchronousMachine.GetRange(0, synchronousMachine.Count);
            }

            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.REACTIVE_CAPABILITY_CURVE_SYM:
                    synchronousMachine.Add(globalId);
                    break;

                default:
                    base.AddReference(referenceId, globalId);
                    break;
            }
        }

        public override void RemoveReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.REACTIVE_CAPABILITY_CURVE_SYM:

                    if (synchronousMachine.Contains(globalId))
                    {
                        synchronousMachine.Remove(globalId);
                    }
                    else
                    {
                        CommonTrace.WriteTrace(CommonTrace.TraceWarning, "Entity (GID = 0x{0:x16}) doesn't contain reference 0x{1:x16}.", this.GlobalId, globalId);
                    }

                    break;

                default:
                    base.RemoveReference(referenceId, globalId);
                    break;
            }
        }

        #endregion IReference implementation
    }
}
