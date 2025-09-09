using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{
    public class RegulatingControl : PowerSystemResource
    {
        private List<long> regCondEq = new List<long>();
        private bool discrete;
        private RegulatingControlModeKind mode;
        private PhaseCode monitoredPhase;
        private float targetRange;
        private float targetValue;

        public RegulatingControl(long globalId) : base(globalId)
        {
        }

        public bool Discrete
        {
            get { return discrete; }
            set { discrete = value; }
        }

        public RegulatingControlModeKind Mode
        {
            get { return mode; }
            set { mode = value; }
        }

        public PhaseCode MonitoredPhase
        {
            get { return monitoredPhase; }
            set { monitoredPhase = value; }
        }

        public float TargetRange
        {
            get { return targetRange; }
            set { targetRange = value; }
        }

        public float TargetValue
        {
            get { return targetValue; }
            set { targetValue = value; }
        }

        public List<long> RegCondEq
        {
            get { return regCondEq; }
            set { regCondEq = value; }
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                RegulatingControl x = (RegulatingControl)obj;
                return (x.discrete == this.discrete &&
                        x.mode == this.mode &&
                        x.monitoredPhase == this.monitoredPhase &&
                        x.targetRange == this.targetRange &&
                        x.targetValue == this.targetValue &&
                        CompareHelper.CompareLists(x.regCondEq, this.regCondEq));
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

        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.REGULATING_CONTROL_DISC:
                case ModelCode.REGULATING_CONTROL_MODE:
                case ModelCode.REGULATING_CONTROL_M_PHASE:
                case ModelCode.REGULATING_CONTROL_TRG_RANGE:
                case ModelCode.REGULATING_CONTROL_TRG_VAL:
                case ModelCode.REGULATING_CONTROL_RCEQ:
                    return true;
                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.REGULATING_CONTROL_DISC:
                    property.SetValue(discrete);
                    break;
                case ModelCode.REGULATING_CONTROL_MODE:
                    property.SetValue((short)mode);
                    break;
                case ModelCode.REGULATING_CONTROL_M_PHASE:
                    property.SetValue((short)monitoredPhase);
                    break;
                case ModelCode.REGULATING_CONTROL_TRG_RANGE:
                    property.SetValue(targetRange);
                    break;
                case ModelCode.REGULATING_CONTROL_TRG_VAL:
                    property.SetValue(targetValue);
                    break;
                case ModelCode.REGULATING_CONTROL_RCEQ:
                    property.SetValue(regCondEq);
                    break;
                default:
                    base.GetProperty(property);
                    break;
            }
        }

        public override void SetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.REGULATING_CONTROL_DISC:
                    discrete = property.AsBool();
                    break;
                case ModelCode.REGULATING_CONTROL_MODE:
                    mode = (RegulatingControlModeKind)property.AsEnum();
                    break;
                case ModelCode.REGULATING_CONTROL_M_PHASE:
                    monitoredPhase = (PhaseCode)property.AsEnum();
                    break;
                case ModelCode.REGULATING_CONTROL_TRG_RANGE:
                    targetRange = property.AsFloat();
                    break;
                case ModelCode.REGULATING_CONTROL_TRG_VAL:
                    targetValue = property.AsFloat();
                    break;
                default:
                    base.SetProperty(property);
                    break;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (regCondEq != null && regCondEq.Count != 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.REGULATING_CONTROL_RCEQ] = regCondEq.GetRange(0, regCondEq.Count);
            }

            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.REGULATING_CONTROL_RCEQ:
                    regCondEq.Add(globalId);
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
                case ModelCode.REGULATING_CONTROL_RCEQ:

                    if (regCondEq.Contains(globalId))
                    {
                        regCondEq.Remove(globalId);
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
    }
}
