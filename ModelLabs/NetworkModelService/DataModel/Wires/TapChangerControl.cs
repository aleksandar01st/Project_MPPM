using FTN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{
    public class TapChangerControl : RegulatingControl
    {
        private float limVoltage;
        private bool lineDropCompensation;
        private float lineDropR;
        private float lineDropX;
        private float reverseLineDropR;
        private float reverseLineDropX;

        public TapChangerControl(long globalId) : base(globalId)
        {
        }

        public float LimVoltage
        {
            get { return limVoltage; }
            set { limVoltage = value; }
        }
        public bool LineDropCompensation
        {
            get { return lineDropCompensation; }
            set { lineDropCompensation = value; }
        }
        public float LineDropR
        {
            get { return lineDropR; }
            set { lineDropR = value; }
        }
        public float LineDropX
        {
            get { return lineDropX; }
            set { lineDropX = value; }
        }
        public float ReverseLineDropR
        {
            get { return reverseLineDropR; }
            set { reverseLineDropR = value; }
        }
        public float ReverseLineDropX
        {
            get { return reverseLineDropX; }
            set { reverseLineDropX = value; }
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                TapChangerControl x = (TapChangerControl)obj;
                return ((x.LimVoltage == this.LimVoltage) &&
                        (x.LineDropCompensation == this.LineDropCompensation) &&
                        (x.LineDropR == this.LineDropR) &&
                        (x.LineDropX == this.LineDropX) &&
                        (x.ReverseLineDropR == this.ReverseLineDropR) &&
                        (x.ReverseLineDropX == this.ReverseLineDropX));
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
                case ModelCode.TAP_CHANGER_CONTROL_LMT_VLT:
                case ModelCode.TAP_CHANGER_CONTROL_LDC:
                case ModelCode.TAP_CHANGER_CONTROL_LDR:
                case ModelCode.TAP_CHANGER_CONTROL_LDX:
                case ModelCode.TAP_CHANGER_CONTROL_RLDR:
                case ModelCode.TAP_CHANGER_CONTROL_RLDX:
                    return true;
                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.TAP_CHANGER_CONTROL_LMT_VLT:
                    property.SetValue(limVoltage);
                    break;
                case ModelCode.TAP_CHANGER_CONTROL_LDC:
                    property.SetValue(lineDropCompensation);
                    break;
                case ModelCode.TAP_CHANGER_CONTROL_LDR:
                    property.SetValue(lineDropR);
                    break;
                case ModelCode.TAP_CHANGER_CONTROL_LDX:
                    property.SetValue(lineDropX);
                    break;
                case ModelCode.TAP_CHANGER_CONTROL_RLDR:
                    property.SetValue(reverseLineDropR);
                    break;
                case ModelCode.TAP_CHANGER_CONTROL_RLDX:
                    property.SetValue(reverseLineDropX);
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
                case ModelCode.TAP_CHANGER_CONTROL_LMT_VLT:
                    limVoltage = property.AsFloat();
                    break;
                case ModelCode.TAP_CHANGER_CONTROL_LDC:
                    lineDropCompensation = property.AsBool();
                    break;
                case ModelCode.TAP_CHANGER_CONTROL_LDR:
                    lineDropR = property.AsFloat();
                    break;
                case ModelCode.TAP_CHANGER_CONTROL_LDX:
                    lineDropX = property.AsFloat();
                    break;
                case ModelCode.TAP_CHANGER_CONTROL_RLDR:
                    reverseLineDropX = property.AsFloat();
                    break;
                case ModelCode.TAP_CHANGER_CONTROL_RLDX:
                    reverseLineDropR = property.AsFloat();
                    break;
                default:
                    base.SetProperty(property);
                    break;
            }
        }
    }
}
