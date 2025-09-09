using FTN.Common;

namespace FTN.ESI.SIMES.CIM.CIMAdapter.Importer
{

	/// <summary>
	/// PowerTransformerConverter has methods for populating
	/// ResourceDescription objects using PowerTransformerCIMProfile_Labs objects.
	/// </summary>
	public static class ProjectConverter
	{

		#region Populate ResourceDescription
		public static void PopulateIdentifiedObjectProperties(FTN.IdentifiedObject cimIdentifiedObject, ResourceDescription rd)
		{
			if ((cimIdentifiedObject != null) && (rd != null))
			{
			}
		}

		public static void PopulatePowerSystemResourceProperties(FTN.PowerSystemResource cimPowerSystemResource, ResourceDescription rd)
		{
			if ((cimPowerSystemResource != null) && (rd != null))
			{
				ProjectConverter.PopulateIdentifiedObjectProperties(cimPowerSystemResource, rd);
			}
		}

		public static void PopulateEquipmentProperties(FTN.Equipment cimEquipment, ResourceDescription rd)
		{
			if ((cimEquipment != null) && (rd != null))
			{
				ProjectConverter.PopulatePowerSystemResourceProperties(cimEquipment, rd);

				if (cimEquipment.AggregateHasValue)
				{
					rd.AddProperty(new Property(ModelCode.EQUIPMENT_AGGREGATE, cimEquipment.Aggregate));
				}
				if (cimEquipment.NormallyInServiceHasValue)
				{
					rd.AddProperty(new Property(ModelCode.EQUIPMENT_NORMALLYINSERVICE, cimEquipment.NormallyInService));
				}
			}
		}

		public static void PopulateConductingEquipmentProperties(FTN.ConductingEquipment cimConductingEquipment, ResourceDescription rd)
		{
			if ((cimConductingEquipment != null) && (rd != null))
			{
				ProjectConverter.PopulateEquipmentProperties(cimConductingEquipment, rd);
            }
		}

		public static void PopulateRegulatingCondEqProperties(FTN.RegulatingCondEq cimRegulatingCondEq, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimRegulatingCondEq != null) && (rd != null))
			{
				ProjectConverter.PopulateConductingEquipmentProperties(cimRegulatingCondEq, rd);
			}
            if (cimRegulatingCondEq.RegulatingControlHasValue)
            {
                long gid = importHelper.GetMappedGID(cimRegulatingCondEq.RegulatingControl.ID);
                if (gid < 0)
                {
                    report.Report.Append("WARNING: Convert ").Append(cimRegulatingCondEq.GetType().ToString()).Append(" rdfID = \"").Append(cimRegulatingCondEq.ID);
                    report.Report.Append("\" - Failed to set reference to RegulatingControl: rdfID \"").Append(cimRegulatingCondEq.RegulatingControl.ID).AppendLine(" \" is not mapped to GID!");
                }
                rd.AddProperty(new Property(ModelCode.REGULATING_COND_EQ_REG_CTRL, gid));
            }
        }

        public static void PopulateRotatingMachineProperties(FTN.RotatingMachine cimRotatingMachine, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimRotatingMachine != null) && (rd != null))
			{
				ProjectConverter.PopulateRegulatingCondEqProperties(cimRotatingMachine, rd, importHelper, report);
			}
		}

		public static void PopulateSynchronousMachineProperties(FTN.SynchronousMachine cimSynchronousMachine, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimSynchronousMachine != null) && (rd != null))
			{
				ProjectConverter.PopulateRotatingMachineProperties(cimSynchronousMachine, rd, importHelper, report);
            }
            if (cimSynchronousMachine.ReactiveCapabilityCurvesHasValue)
            {
                long gid = importHelper.GetMappedGID(cimSynchronousMachine.ReactiveCapabilityCurves.ID);
                if (gid < 0)
                {
                    report.Report.Append("WARNING: Convert ").Append(cimSynchronousMachine.GetType().ToString()).Append(" rdfID = \"").Append(cimSynchronousMachine.ID);
                    report.Report.Append("\" - Failed to set reference to ReactiveCapabilityCurves: rdfID \"").Append(cimSynchronousMachine.ReactiveCapabilityCurves.ID).AppendLine(" \" is not mapped to GID!");
                }
                rd.AddProperty(new Property(ModelCode.SYNCHRONOUS_MACHINE_RCC, gid));
            }
        }

		public static void PopulateRegulatingControlProperties(FTN.RegulatingControl cimRegulatingControl, ResourceDescription rd)
		{
			if ((cimRegulatingControl != null) && (rd != null))
			{
				ProjectConverter.PopulatePowerSystemResourceProperties(cimRegulatingControl, rd);

				if (cimRegulatingControl.Discrete)
				{
					rd.AddProperty(new Property(ModelCode.REGULATING_CONTROL_DISC, cimRegulatingControl.Discrete));
				}
				if (cimRegulatingControl.TargetRangeHasValue)
				{
					rd.AddProperty(new Property(ModelCode.REGULATING_CONTROL_TRG_RANGE, cimRegulatingControl.TargetRange));
				}
				if (cimRegulatingControl.TargetValueHasValue)
				{
					rd.AddProperty(new Property(ModelCode.REGULATING_CONTROL_TRG_VAL, cimRegulatingControl.TargetValue));
				}
				if (cimRegulatingControl.ModeHasValue)
				{
					rd.AddProperty(new Property(ModelCode.REGULATING_CONTROL_MODE, (short)GetDMSRegulatingControlModeKind(cimRegulatingControl.Mode)));
				}
				if (cimRegulatingControl.MonitoredPhaseHasValue)
				{
					rd.AddProperty(new Property(ModelCode.REGULATING_CONTROL_M_PHASE, (short)GetDMSPhaseCode(cimRegulatingControl.MonitoredPhase)));
				}
			}
		}

		public static void PopulateTapChangerControlProperties(FTN.TapChangerControl cimTapChangerControl, ResourceDescription rd)
		{
			if ((cimTapChangerControl != null) && (rd != null))
			{
				ProjectConverter.PopulateRegulatingControlProperties(cimTapChangerControl, rd);

				if (cimTapChangerControl.LimitVoltageHasValue)
				{
					rd.AddProperty(new Property(ModelCode.TAP_CHANGER_CONTROL_LMT_VLT, cimTapChangerControl.LimitVoltage));
				}
				if (cimTapChangerControl.LineDropCompensationHasValue)
				{
					rd.AddProperty(new Property(ModelCode.TAP_CHANGER_CONTROL_LDC, cimTapChangerControl.LineDropCompensation));
				}
				if (cimTapChangerControl.LineDropRHasValue)
				{
					rd.AddProperty(new Property(ModelCode.TAP_CHANGER_CONTROL_LDR, cimTapChangerControl.LineDropR));
				}
				if (cimTapChangerControl.LineDropXHasValue)
				{
					rd.AddProperty(new Property(ModelCode.TAP_CHANGER_CONTROL_LDX, cimTapChangerControl.LineDropX));
				}
				if (cimTapChangerControl.ReverseLineDropRHasValue)
				{
					rd.AddProperty(new Property(ModelCode.TAP_CHANGER_CONTROL_RLDR, cimTapChangerControl.ReverseLineDropR));
				}
				if (cimTapChangerControl.ReverseLineDropXHasValue)
				{
					rd.AddProperty(new Property(ModelCode.TAP_CHANGER_CONTROL_RLDX, cimTapChangerControl.ReverseLineDropX));
				}
			}
		}

        public static void PopulateTerminalProperties(FTN.Terminal cimTerminal, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimTerminal != null) && (rd != null))
            {
                ProjectConverter.PopulateIdentifiedObjectProperties(cimTerminal, rd);
            }
            if (cimTerminal.ConductingEquipmentHasValue)
            {
                long gid = importHelper.GetMappedGID(cimTerminal.ConductingEquipment.ID);
                if (gid < 0)
                {
                    report.Report.Append("WARNING: Convert ").Append(cimTerminal.GetType().ToString()).Append(" rdfID = \"").Append(cimTerminal.ID);
                    report.Report.Append("\" - Failed to set reference to ConductingEquipment: rdfID \"").Append(cimTerminal.ConductingEquipment.ID).AppendLine(" \" is not mapped to GID!");
                }
                rd.AddProperty(new Property(ModelCode.TERMINAL_COND_EQ, gid));
            }
        }

        public static void PopulateControlProperties(FTN.Control cimControl, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimControl != null) && (rd != null))
            {
                ProjectConverter.PopulateIdentifiedObjectProperties(cimControl, rd);
            }
            if (cimControl.RegulatingCondEqHasValue)
            {
                long gid = importHelper.GetMappedGID(cimControl.RegulatingCondEq.ID);
                if (gid < 0)
                {
                    report.Report.Append("WARNING: Convert ").Append(cimControl.GetType().ToString()).Append(" rdfID = \"").Append(cimControl.ID);
                    report.Report.Append("\" - Failed to set reference to RegulatingCondEq: rdfID \"").Append(cimControl.RegulatingCondEq.ID).AppendLine(" \" is not mapped to GID!");
                }
                rd.AddProperty(new Property(ModelCode.CONTROL_REG_COND_EQ, gid));
            }
        }

        public static void PopulateCurveProperties(FTN.Curve cimCurve, ResourceDescription rd)
        {
            if ((cimCurve != null) && (rd != null))
            {
                ProjectConverter.PopulateIdentifiedObjectProperties(cimCurve, rd);

                if (cimCurve.CurveStyleHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.CURVE_STYLE, (short)GetDMSCurveStyle(cimCurve.CurveStyle)));
                }
                if (cimCurve.XMultiplierHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.CURVE_X_MULTI, (short)GetDMSUnitMultiplier(cimCurve.XMultiplier)));
                }
                if (cimCurve.XUnitHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.CURVE_X_UNIT, (short)GetDMSUnitSymbol(cimCurve.XUnit)));
                }
                if (cimCurve.Y1MultiplierHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.CURVE_Y1_MULTI, (short)GetDMSUnitMultiplier(cimCurve.Y1Multiplier)));
                }
                if (cimCurve.Y1UnitHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.CURVE_Y1_UNIT, (short)GetDMSUnitSymbol(cimCurve.Y1Unit)));
                }
                if (cimCurve.Y2MultiplierHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.CURVE_Y2_MULTI, (short)GetDMSUnitMultiplier(cimCurve.Y2Multiplier)));
                }
                if (cimCurve.Y2UnitHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.CURVE_Y2_UNIT, (short)GetDMSUnitSymbol(cimCurve.Y2Unit)));
                }
                if (cimCurve.Y3MultiplierHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.CURVE_Y3_MULTI, (short)GetDMSUnitMultiplier(cimCurve.Y3Multiplier)));
                }
                if (cimCurve.Y3UnitHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.CURVE_Y3_UNIT, (short)GetDMSUnitSymbol(cimCurve.Y3Unit)));
                }
            }
        }

        public static void PopulateReactiveCapabilityCurveProperties(FTN.ReactiveCapabilityCurve cimReactiveCapabilityCurve, ResourceDescription rd)
        {
            if ((cimReactiveCapabilityCurve != null) && (rd != null))
            {
                ProjectConverter.PopulateCurveProperties(cimReactiveCapabilityCurve, rd);
            }
        }

        public static void PopulateCurveDataProperties(FTN.CurveData cimCurveData, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimCurveData != null) && (rd != null))
            {
                ProjectConverter.PopulateIdentifiedObjectProperties(cimCurveData, rd);
            }
            if (cimCurveData.CurveHasValue)
            {
                long gid = importHelper.GetMappedGID(cimCurveData.Curve.ID);
                if (gid < 0)
                {
                    report.Report.Append("WARNING: Convert ").Append(cimCurveData.GetType().ToString()).Append(" rdfID = \"").Append(cimCurveData.ID);
                    report.Report.Append("\" - Failed to set reference to Curve: rdfID \"").Append(cimCurveData.Curve.ID).AppendLine(" \" is not mapped to GID!");
                }
                rd.AddProperty(new Property(ModelCode.CURVE_DATA_CURVES, gid));
            }
        }


        #endregion Populate ResourceDescription

        #region Enums convert
        public static PhaseCode GetDMSPhaseCode(FTN.PhaseCode phases)
		{
			switch (phases)
			{
				case FTN.PhaseCode.A:
					return PhaseCode.A;
				case FTN.PhaseCode.AB:
					return PhaseCode.AB;
				case FTN.PhaseCode.ABC:
					return PhaseCode.ABC;
				case FTN.PhaseCode.ABCN:
					return PhaseCode.ABCN;
				case FTN.PhaseCode.ABN:
					return PhaseCode.ABN;
				case FTN.PhaseCode.AC:
					return PhaseCode.AC;
				case FTN.PhaseCode.ACN:
					return PhaseCode.ACN;
				case FTN.PhaseCode.AN:
					return PhaseCode.AN;
				case FTN.PhaseCode.B:
					return PhaseCode.B;
				case FTN.PhaseCode.BC:
					return PhaseCode.BC;
				case FTN.PhaseCode.BCN:
					return PhaseCode.BCN;
				case FTN.PhaseCode.BN:
					return PhaseCode.BN;
				case FTN.PhaseCode.C:
					return PhaseCode.C;
				case FTN.PhaseCode.CN:
					return PhaseCode.CN;
				case FTN.PhaseCode.N:
					return PhaseCode.N;
				case FTN.PhaseCode.s12N:
					return PhaseCode.ABN;
				case FTN.PhaseCode.s1N:
					return PhaseCode.AN;
				case FTN.PhaseCode.s2N:
					return PhaseCode.BN;
				default: return PhaseCode.A;
			}
		}

        public static RegulatingControlModeKind GetDMSRegulatingControlModeKind(FTN.RegulatingControlModeKind RegulatingControlModeKind)
        {
            switch (RegulatingControlModeKind)
            {
                case FTN.RegulatingControlModeKind.activePower:
                    return RegulatingControlModeKind.activePower;
                case FTN.RegulatingControlModeKind.admittance:
                    return RegulatingControlModeKind.admittance;
                case FTN.RegulatingControlModeKind.currentFlow:
                    return RegulatingControlModeKind.currentFlow;
                case FTN.RegulatingControlModeKind.@fixed:
                    return RegulatingControlModeKind.@fixed;
                case FTN.RegulatingControlModeKind.powerFactor:
                    return RegulatingControlModeKind.powerFactor;
                case FTN.RegulatingControlModeKind.reactivePower:
                    return RegulatingControlModeKind.reactivePower;
                case FTN.RegulatingControlModeKind.temperature:
                    return RegulatingControlModeKind.temperature;
                case FTN.RegulatingControlModeKind.timeScheduled:
                    return RegulatingControlModeKind.timeScheduled;
                case FTN.RegulatingControlModeKind.voltage:
                    return RegulatingControlModeKind.voltage;
                default:
                    return RegulatingControlModeKind.activePower;
            }
        }

        public static CurveStyle GetDMSCurveStyle(FTN.CurveStyle CurveStyle)
		{
			switch (CurveStyle)
			{
				case FTN.CurveStyle.constantYValue:
					return CurveStyle.constantYValue;
                case FTN.CurveStyle.formula:
                    return CurveStyle.formula;
                case FTN.CurveStyle.rampYValue:
                    return CurveStyle.rampYValue;
                case FTN.CurveStyle.straightLineYValues:
                    return CurveStyle.straightLineYValues;
                default:
					return CurveStyle.constantYValue;
			}
		}

		public static UnitMultiplier GetDMSUnitMultiplier(FTN.UnitMultiplier UnitMultiplier)
		{
			switch (UnitMultiplier)
			{
				case FTN.UnitMultiplier.G:
					return UnitMultiplier.G;
				case FTN.UnitMultiplier.M:
					return UnitMultiplier.M;
				case FTN.UnitMultiplier.T:
					return UnitMultiplier.T;
                case FTN.UnitMultiplier.c:
					return UnitMultiplier.c;
                case FTN.UnitMultiplier.d:
                    return UnitMultiplier.d;
				case FTN.UnitMultiplier.k:
                    return UnitMultiplier.k;
                case FTN.UnitMultiplier.m:
                    return UnitMultiplier.m;
                case FTN.UnitMultiplier.micro:
                    return UnitMultiplier.micro;
                case FTN.UnitMultiplier.n:
                    return UnitMultiplier.n;
                case FTN.UnitMultiplier.none:
                    return UnitMultiplier.none;
                case FTN.UnitMultiplier.p:
                    return UnitMultiplier.p;
                default:
					return UnitMultiplier.G;
			}
		}

		public static UnitSymbol GetDMSUnitSymbol(FTN.UnitSymbol UnitSymbol)
		{
            switch (UnitSymbol)
            {
                case FTN.UnitSymbol.A:
                    return UnitSymbol.A;
                case FTN.UnitSymbol.F:
                    return UnitSymbol.F;
                case FTN.UnitSymbol.H:
                    return UnitSymbol.H;
                case FTN.UnitSymbol.Hz:
                    return UnitSymbol.Hz;
                case FTN.UnitSymbol.J:
                    return UnitSymbol.J;
                case FTN.UnitSymbol.N:
                    return UnitSymbol.N;
                case FTN.UnitSymbol.Pa:
                    return UnitSymbol.Pa;
                case FTN.UnitSymbol.S:
                    return UnitSymbol.S;
                case FTN.UnitSymbol.V:
                    return UnitSymbol.V;
                case FTN.UnitSymbol.VA:
                    return UnitSymbol.VA;
                case FTN.UnitSymbol.VAh:
                    return UnitSymbol.VAh;
                case FTN.UnitSymbol.VAr:
                    return UnitSymbol.VAr;
                case FTN.UnitSymbol.VArh:
                    return UnitSymbol.VArh;
                case FTN.UnitSymbol.W:
                    return UnitSymbol.W;
                case FTN.UnitSymbol.Wh:
                    return UnitSymbol.Wh;
                case FTN.UnitSymbol.deg:
                    return UnitSymbol.deg;
                case FTN.UnitSymbol.degC:
                    return UnitSymbol.degC;
                case FTN.UnitSymbol.g:
                    return UnitSymbol.g;
                case FTN.UnitSymbol.h:
                    return UnitSymbol.h;
                case FTN.UnitSymbol.m:
                    return UnitSymbol.m;
                case FTN.UnitSymbol.min:
                    return UnitSymbol.min;
                case FTN.UnitSymbol.none:
                    return UnitSymbol.none;
                case FTN.UnitSymbol.ohm:
                    return UnitSymbol.ohm;
                case FTN.UnitSymbol.rad:
                    return UnitSymbol.rad;
                case FTN.UnitSymbol.s:
                    return UnitSymbol.s;
                default:
                    return UnitSymbol.A;
            }
        }
		#endregion Enums convert
	}
}
