using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{
    public class RegulatingCondEq : ConductingEquipment
    {
        private long regulatingControl = 0;
        private List<long> control = new List<long>();

        public RegulatingCondEq(long globalId) : base(globalId)
        {
        }

        public List<long> Control
        {
            get { return control; }
            set { control = value; }
        }

        public long RegulatingControl
        {
            get { return regulatingControl; }
            set { regulatingControl = value; }
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                RegulatingCondEq x = (RegulatingCondEq)obj;
                return ((x.RegulatingControl == this.RegulatingControl) &&
                        CompareHelper.CompareLists(x.Control, this.Control));
            }
            else
            {
                return false;
            }
        }

        #region IAccess implementation

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.REGULATING_COND_EQ_CTRL:
                case ModelCode.REGULATING_COND_EQ_REG_CTRL:
                    return true;
                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.REGULATING_COND_EQ_CTRL:
                    property.SetValue(control);
                    break;
                case ModelCode.REGULATING_COND_EQ_REG_CTRL:
                    property.SetValue((short)regulatingControl);
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
                case ModelCode.REGULATING_COND_EQ_REG_CTRL:
                    regulatingControl = property.AsReference();
                    break;
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
                return (control.Count > 0) || base.IsReferenced;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (regulatingControl != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
            {
                references[ModelCode.REGULATING_COND_EQ_REG_CTRL] = new List<long>();
                references[ModelCode.REGULATING_COND_EQ_REG_CTRL].Add(regulatingControl);
            }

            if (control != null && control.Count > 0 &&
                (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.REGULATING_COND_EQ_CTRL] = control.GetRange(0, control.Count);
            }

            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.REGULATING_COND_EQ_CTRL:
                    control.Add(globalId);
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
                case ModelCode.REGULATING_COND_EQ_CTRL:

                    if (control.Contains(globalId))
                    {
                        control.Remove(globalId);
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
