using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{
    public class SynchronousMachine : RotatingMachine
    {
        private long reactiveCapCurve = 0;

        public SynchronousMachine(long globalId) : base(globalId)
        {
        }

        public long ReactiveCapCurve
        {
            get { return reactiveCapCurve; }
            set { reactiveCapCurve = value; }
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                SynchronousMachine x = (SynchronousMachine)obj;

                return (x.reactiveCapCurve == this.reactiveCapCurve);
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
                case ModelCode.SYNCHRONOUS_MACHINE_RCC:
                    return true;

                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property prop)
        {
            switch (prop.Id)
            {
                case ModelCode.SYNCHRONOUS_MACHINE_RCC:
                    prop.SetValue(reactiveCapCurve);
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
                case ModelCode.SYNCHRONOUS_MACHINE_RCC:
                    reactiveCapCurve = property.AsReference();
                    break;

                default:
                    base.SetProperty(property);
                    break;
            }
        }

        #endregion IAccess implementation

        #region IReference implementation

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (reactiveCapCurve != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
            {
                references[ModelCode.SYNCHRONOUS_MACHINE_RCC] = new List<long>() { reactiveCapCurve };
            }

            base.GetReferences(references, refType);
        }

        #endregion IReference implementation
    }
}
