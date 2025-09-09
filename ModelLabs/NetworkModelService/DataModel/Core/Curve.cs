using FTN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Core
{
    public class Curve : IdentifiedObject
    {
        private CurveStyle curveStyle;
        private UnitMultiplier xMultiplier;
        private UnitSymbol xUnit;
        private UnitMultiplier y1Multiplier;
        private UnitSymbol y1Unit;
        private UnitMultiplier y2Multiplier;
        private UnitSymbol y2Unit;
        private UnitMultiplier y3Multiplier;
        private UnitSymbol y3Unit;

        // lista referenci na CurveData
        private List<long> curveDatas = new List<long>();

        public Curve(long globalId) : base(globalId)
        {
        }

        public CurveStyle CurveStyle
        {
            get { return curveStyle; }
            set { curveStyle = value; }
        }

        public UnitMultiplier XMultiplier
        {
            get { return xMultiplier; }
            set { xMultiplier = value; }
        }

        public UnitSymbol XUnit
        {
            get { return xUnit; }
            set { xUnit = value; }
        }

        public UnitMultiplier Y1Multiplier
        {
            get { return y1Multiplier; }
            set { y1Multiplier = value; }
        }

        public UnitSymbol Y1Unit
        {
            get { return y1Unit; }
            set { y1Unit = value; }
        }

        public UnitMultiplier Y2Multiplier
        {
            get { return y2Multiplier; }
            set { y2Multiplier = value; }
        }

        public UnitSymbol Y2Unit
        {
            get { return y2Unit; }
            set { y2Unit = value; }
        }

        public UnitMultiplier Y3Multiplier
        {
            get { return y3Multiplier; }
            set { y3Multiplier = value; }
        }

        public UnitSymbol Y3Unit
        {
            get { return y3Unit; }
            set { y3Unit = value; }
        }

        public List<long> CurveDatas
        {
            get { return curveDatas; }
            set { curveDatas = value; }
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                Curve x = (Curve)obj;

                return ((x.curveStyle == this.curveStyle) &&
                        (x.xMultiplier == this.xMultiplier) &&
                        (x.xUnit == this.xUnit) &&
                        (x.y1Multiplier == this.y1Multiplier) &&
                        (x.y1Unit == this.y1Unit) &&
                        (x.y2Multiplier == this.y2Multiplier) &&
                        (x.y2Unit == this.y2Unit) &&
                        (x.y3Multiplier == this.y3Multiplier) &&
                        (x.y3Unit == this.y3Unit) &&
                        CompareHelper.CompareLists(x.CurveDatas, this.CurveDatas));
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
                case ModelCode.CURVE_STYLE:
                case ModelCode.CURVE_X_MULTI:
                case ModelCode.CURVE_X_UNIT:
                case ModelCode.CURVE_Y1_MULTI:
                case ModelCode.CURVE_Y1_UNIT:
                case ModelCode.CURVE_Y2_MULTI:
                case ModelCode.CURVE_Y2_UNIT:
                case ModelCode.CURVE_Y3_MULTI:
                case ModelCode.CURVE_Y3_UNIT:
                case ModelCode.CURVE_CRV_DATAS:
                    return true;

                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property prop)
        {
            switch (prop.Id)
            {
                case ModelCode.CURVE_STYLE:
                    prop.SetValue((short)curveStyle);
                    break;
                case ModelCode.CURVE_X_MULTI:
                    prop.SetValue((short)xMultiplier);
                    break;
                case ModelCode.CURVE_X_UNIT:
                    prop.SetValue((short)xUnit);
                    break;
                case ModelCode.CURVE_Y1_MULTI:
                    prop.SetValue((short)y1Multiplier);
                    break;
                case ModelCode.CURVE_Y1_UNIT:
                    prop.SetValue((short)y1Unit);
                    break;
                case ModelCode.CURVE_Y2_MULTI:
                    prop.SetValue((short)y2Multiplier);
                    break;
                case ModelCode.CURVE_Y2_UNIT:
                    prop.SetValue((short)y2Unit);
                    break;
                case ModelCode.CURVE_Y3_MULTI:
                    prop.SetValue((short)y3Multiplier);
                    break;
                case ModelCode.CURVE_Y3_UNIT:
                    prop.SetValue((short)y3Unit);
                    break;
                case ModelCode.CURVE_CRV_DATAS:
                    prop.SetValue(curveDatas);
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
                case ModelCode.CURVE_STYLE:
                    curveStyle = (CurveStyle)property.AsEnum();
                    break;
                case ModelCode.CURVE_X_MULTI:
                    xMultiplier = (UnitMultiplier)property.AsEnum();
                    break;
                case ModelCode.CURVE_X_UNIT:
                    xUnit = (UnitSymbol)property.AsEnum();
                    break;
                case ModelCode.CURVE_Y1_MULTI:
                    y1Multiplier = (UnitMultiplier)property.AsEnum();
                    break;
                case ModelCode.CURVE_Y1_UNIT:
                    y1Unit = (UnitSymbol)property.AsEnum();
                    break;
                case ModelCode.CURVE_Y2_MULTI:
                    y2Multiplier = (UnitMultiplier)property.AsEnum();
                    break;
                case ModelCode.CURVE_Y2_UNIT:
                    y2Unit = (UnitSymbol)property.AsEnum();
                    break;
                case ModelCode.CURVE_Y3_MULTI:
                    y3Multiplier = (UnitMultiplier)property.AsEnum();
                    break;
                case ModelCode.CURVE_Y3_UNIT:
                    y3Unit = (UnitSymbol)property.AsEnum();
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
                return (curveDatas.Count > 0) || base.IsReferenced;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (curveDatas != null && curveDatas.Count > 0 &&
                (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.CURVE_CRV_DATAS] = curveDatas.GetRange(0, curveDatas.Count);
            }

            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.CURVE_CRV_DATAS:
                    curveDatas.Add(globalId);
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
                case ModelCode.CURVE_CRV_DATAS:

                    if (curveDatas.Contains(globalId))
                    {
                        curveDatas.Remove(globalId);
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
