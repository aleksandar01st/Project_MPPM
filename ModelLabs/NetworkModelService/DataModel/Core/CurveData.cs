using FTN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Core
{
    public class CurveData : IdentifiedObject
    {
        private float xValue;
        private float y1Value;
        private float y2Value;
        private float y3Value;
        private long curveDataCurves = 0;

        public CurveData(long globalId) : base(globalId)
        {
        }

        public float XValue
        {
            get { return xValue; }
            set { xValue = value; }
        }

        public float Y1Value
        {
            get { return y1Value; }
            set { y1Value = value; }
        }

        public float Y2Value
        {
            get { return y2Value; }
            set { y2Value = value; }
        }

        public float Y3Value
        {
            get { return y3Value; }
            set { y3Value = value; }
        }

        public long CurveDataCurves
        {
            get { return curveDataCurves; }
            set { curveDataCurves = value; }
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                CurveData x = (CurveData)obj;

                return (x.curveDataCurves == this.curveDataCurves && 
                    x.xValue == this.xValue &&
                        x.y1Value == this.y1Value && 
                        x.y2Value == this.y2Value &&
                        x.y3Value == this.y3Value);
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
                case ModelCode.CURVE_DATA_X_VALUE:
                case ModelCode.CURVE_DATA_Y1_VALUE:
                case ModelCode.CURVE_DATA_Y2_VALUE:
                case ModelCode.CURVE_DATA_Y3_VALUE:
                case ModelCode.CURVE_DATA_CURVES:
                    return true;

                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property prop)
        {
            switch (prop.Id)
            {
                case ModelCode.CURVE_DATA_X_VALUE:
                    prop.SetValue(xValue);
                    break;
                case ModelCode.CURVE_DATA_Y1_VALUE:
                    prop.SetValue(y1Value);
                    break;
                case ModelCode.CURVE_DATA_Y2_VALUE:
                    prop.SetValue(y2Value);
                    break;
                case ModelCode.CURVE_DATA_Y3_VALUE:
                    prop.SetValue(y3Value);
                    break;
                case ModelCode.CURVE_DATA_CURVES:
                    prop.SetValue(curveDataCurves);
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
                case ModelCode.CURVE_DATA_X_VALUE:
                    xValue = property.AsFloat();
                    break;
                case ModelCode.CURVE_DATA_Y1_VALUE:
                    y1Value = property.AsFloat();
                    break;
                case ModelCode.CURVE_DATA_Y2_VALUE:
                    y2Value = property.AsFloat();
                    break;
                case ModelCode.CURVE_DATA_Y3_VALUE:
                    y3Value = property.AsFloat();
                    break;
                case ModelCode.CURVE_DATA_CURVES:
                    curveDataCurves = property.AsReference();
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
            if (curveDataCurves != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
            {
                references[ModelCode.CURVE_DATA_CURVES] = new List<long>() { curveDataCurves };
            }

            base.GetReferences(references, refType);
        }

        #endregion IReference implementation

    }
}
