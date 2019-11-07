//////////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2019, National Instruments Corp.

// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:

// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
//////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using NationalInstruments.Vision;
using NationalInstruments.VBAI.Structures;
using NationalInstruments.VBAI.Enums;
using NationalInstruments.VBAI.Internal;

using NativeSession = System.UInt32;

namespace NationalInstruments.VBAI
{
    public class VBAIEngine
    {
        #region Member Variables
        private Session _session;
        private string _sessionName;
        private VisionImage _image;
        #endregion

        #region Imports from VBAIInterfaceC.dll
        [DllImport("VBAIInterfaceC.dll", EntryPoint="vbaiLaunchLocalVBAIEngine", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.StdCall)]
        private static extern Error vbaiLaunchLocalVBAIEngine(string name, string version, bool show, [In, Out] byte[] sessionName, int sessionNameLength);

        [DllImport("VBAIInterfaceC.dll", EntryPoint="vbaiOpenConnection", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.StdCall)]
        private static extern Error vbaiOpenConnection(string sessionName, string password, bool forceConnection, out NativeSession session);

        [DllImport("VBAIInterfaceC.dll", EntryPoint="vbaiCloseConnection", CallingConvention=CallingConvention.StdCall)]
        private static extern Error vbaiCloseConnection(NativeSession session, bool closeLocalEngine);

        [DllImport("VBAIInterfaceC.dll", EntryPoint="vbaiCloseLocalVBAIEngine", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.StdCall)]
        private static extern Error vbaiCloseLocalVBAIEngine(string sessionName, int timeout);

        [DllImport("VBAIInterfaceC.dll", EntryPoint="vbaiGetVBAIEngineStatus", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.StdCall)]
        private static extern Error vbaiGetVBAIEngineStatus(NativeSession session, out EngineStatus status, [In, Out] byte[] inspectionName, int inspectionNameLength);

        [DllImport("VBAIInterfaceC.dll", EntryPoint = "vbaiEnumerateVBAIEngines", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern Error vbaiEnumerateVBAIEngines([In, Out] Engine[] engineArray, out int size);

        [DllImport("VBAIInterfaceC.dll", EntryPoint = "vbaiGetTargetInspections", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern Error vbaiGetTargetInspections(NativeSession session, [In, Out] InspectionInfo[] inspectionArray, out int size);

        [DllImport("VBAIInterfaceC.dll", EntryPoint="vbaiOpenInspection", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.StdCall)]
        private static extern Error vbaiOpenInspection(NativeSession session, string path);

        [DllImport("VBAIInterfaceC.dll", EntryPoint="vbaiGetInspectionInfo", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.StdCall)]
        private static extern Error vbaiGetInspectionInfo(NativeSession session, out InspectionInfo info);

        [DllImport("VBAIInterfaceC.dll", EntryPoint = "vbaiGetInspectionSteps", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern Error vbaiGetInspectionSteps(NativeSession session, ref IntPtr steps);

        [DllImport("VBAIInterfaceC.dll", EntryPoint = "vbaiGetVariables", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern Error vbaiGetVariables(NativeSession session, [In, Out] Variable[] variableArray, out int size);

        [DllImport("VBAIInterfaceC.dll", EntryPoint = "vbaiSetVariables", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern Error vbaiSetVariables(NativeSession session, VariableUpdateMode updateMode, VariableValuePriv[] variableArray, int size);

        [DllImport("VBAIInterfaceC.dll", EntryPoint = "vbaiGetInspectionStateDiagram", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern Error vbaiGetInspectionStateDiagram(NativeSession session, [In, Out] StateInfo[] statesArray, out int stateSize, out int firstState, [In, Out] TransitionInfo[] transitionArray, out int transitionSize);

        [DllImport("VBAIInterfaceC.dll", EntryPoint = "vbaiGetTargetDateTime", CallingConvention = CallingConvention.StdCall)]
        private static extern Error vbaiGetTargetDateTime(NativeSession session, out VBAIDateTime VBAIDateTime, out VBAITimeZone VBAITimeZone);

        [DllImport("VBAIInterfaceC.dll", EntryPoint = "vbaiSetTargetDateTime", CallingConvention = CallingConvention.StdCall)]
        private static extern Error vbaiSetTargetDateTime(NativeSession session, VBAIDateTime VBAIDateTime, VBAITimeZone VBAITimeZone);

        [DllImport("VBAIInterfaceC.dll", EntryPoint = "vbaiRunInspectionOnce", CallingConvention = CallingConvention.StdCall)]
        private static extern Error vbaiRunInspectionOnce(NativeSession session, int timeout);

        [DllImport("VBAIInterfaceC.dll", EntryPoint = "vbaiStartInspection", CallingConvention = CallingConvention.StdCall)]
        private static extern Error vbaiStartInspection(NativeSession session);

        [DllImport("VBAIInterfaceC.dll", EntryPoint = "vbaiStopInspection", CallingConvention = CallingConvention.StdCall)]
        private static extern Error vbaiStopInspection(NativeSession session);

        [DllImport("VBAIInterfaceC.dll", EntryPoint = "vbaiGetInspectionImage", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern Error vbaiGetInspectionImage(NativeSession session, string imageGUID, int xReduction, int yReduction, IntPtr image, out bool newImageAvailable);

        [DllImport("VBAIInterfaceC.dll", EntryPoint = "vbaiGetInspectionResults", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern Error vbaiGetInspectionResults(NativeSession session, [In, Out] StepResult[] stepResultArray, out int size, out VBAIDateTime VBAIDateTime, out bool inspectionStatus);

        [DllImport("VBAIInterfaceC.dll", EntryPoint = "vbaiEnableInspectionMeasurements", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern Error vbaiEnableInspectionMeasurements(NativeSession session, bool enable);

        [DllImport("VBAIInterfaceC.dll", EntryPoint = "vbaiGetInspectionMeasurements", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern Error vbaiGetInspectionMeasurements(NativeSession session, byte[,] stepGUIDs, int numGUIDs, ref IntPtr measurements, out VBAIDateTime VBAIDateTime);

        [DllImport("VBAIInterfaceC.dll", EntryPoint = "vbaiResetInspectionStatistics", CallingConvention = CallingConvention.StdCall)]
        private static extern Error vbaiResetInspectionStatistics(NativeSession session);

        [DllImport("VBAIInterfaceC.dll", EntryPoint = "vbaiGetInspectionStatistics", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern Error vbaiGetInspectionStatistics(NativeSession session, out InspectionStatistics stats, out VBAIDateTime VBAIDateTime);

        [DllImport("VBAIInterfaceC.dll", EntryPoint = "vbaiDispose", CallingConvention = CallingConvention.StdCall)]
        private static extern Error vbaiDispose(IntPtr data);

        [DllImport("VBAIInterfaceC.dll", EntryPoint = "vbaiGetStepGUIDFromName", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        internal static extern Error vbaiGetStepGUIDFromName(NativeSession session, string stateName, string stepName, [In, Out] byte[] stepGUID);

        [DllImport("VBAIInterfaceC.dll", EntryPoint = "vbaiGetErrorString", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        internal static extern Error vbaiGetErrorString(Error code, [In, Out] byte[] sessionName, int sessionNameLength);

        internal static string ConvertBytesToString(byte[] bytes)
        {
            Int32 numBytes = Array.FindIndex(bytes, delegate(byte b) { return b == '\0'; });
            if (numBytes == -1)
            {
                numBytes = bytes.Length;
            }
            return System.Text.Encoding.Default.GetString(bytes, 0, numBytes);
        }

        #endregion

        #region Interface

       public VBAIEngine(string localEngineName, string version, bool showLocalEngine)
        {
            byte[] str = new byte[256];
            _image = new VisionImage();
            Error err = vbaiLaunchLocalVBAIEngine(localEngineName, version, showLocalEngine, str, 256);

            if (err == Error.Success)
            {
                _sessionName = ConvertBytesToString(str);
                err = vbaiOpenConnection(_sessionName, "", false, out _session.nativeSession);
            }
            ExceptionBuilder.CheckErrorAndThrow(err);
        }

       public VBAIEngine(string ipAddress, bool forceConnection, string password)
       {
           _image = new VisionImage();

           Error err = vbaiOpenConnection(ipAddress, password, forceConnection, out _session.nativeSession);
           ExceptionBuilder.CheckErrorAndThrow(err);
       }

        public static Engine[] EnumerateVBAIEngines()
        {
            int size;
            Engine[] VBAIEngineArray;

            Error err = vbaiEnumerateVBAIEngines(null, out size);
            if (err != Error.Success)
            {
                VBAIEngineArray = null;
                ExceptionBuilder.CheckErrorAndThrow(err);
                return VBAIEngineArray;
            }
            VBAIEngineArray = new Engine[size];
            err = vbaiEnumerateVBAIEngines(VBAIEngineArray, out size);
            ExceptionBuilder.CheckErrorAndThrow(err);
            return VBAIEngineArray;
        }

        public InspectionInfo[] GetTargetInspections()
        {
            int size;
            InspectionInfo[] inspectionArray;

            Error err = vbaiGetTargetInspections(_session.nativeSession, null, out size);
            if (err != Error.Success)
            {
                inspectionArray = null;
                ExceptionBuilder.CheckErrorAndThrow(err);
                return inspectionArray;
            }
            inspectionArray = new InspectionInfo[size];
            err = vbaiGetTargetInspections(_session.nativeSession, inspectionArray, out size);
            ExceptionBuilder.CheckErrorAndThrow(err);
            return inspectionArray;
        }


        public EngineStatusInfo GetEngineStatus()
        {
            byte[] byteName = new byte[256];
            EngineStatusInfo info;
            info.inspectionName = "";
            Error err = vbaiGetVBAIEngineStatus(_session.nativeSession, out info.status, byteName, 256);
            if (err != Error.Success)
            {
                ExceptionBuilder.CheckErrorAndThrow(err);
                return info;
            }
            info.inspectionName = ConvertBytesToString(byteName);
            return info;
        }

        public void OpenInspection(string path)
        {
            Error err = vbaiOpenInspection(_session.nativeSession, path);
            ExceptionBuilder.CheckErrorAndThrow(err);
        }

        public InspectionInfo GetInspectionInfo()
        {
            InspectionInfo info;

            Error err = vbaiGetInspectionInfo(_session.nativeSession, out info);
            if (err != Error.Success)
            {
                ExceptionBuilder.CheckErrorAndThrow(err);
            }
            return info;
        }

        public InspectionMeasurements[] GetInspectionMeasurements(string[] stepGUIDs, out VBAIDateTime timeStamp)
        {
            IntPtr measurementsPtr = IntPtr.Zero;
            int i, j, offset1=0, offset2 = 0, index;
            InspectionMeasurements[] measurementsArray;

            MeasurementsPriv measuresPriv = new MeasurementsPriv();
            StepMeasurementsPriv stepMeasuresPriv = new StepMeasurementsPriv();
            StepMeasurementPriv stepMeasurePriv = new StepMeasurementPriv();
            int size = 0;
            byte[,] guids = null;
            if (stepGUIDs != null)
            {
                if (stepGUIDs.Length != 0)
                {
                    size = stepGUIDs.Length;
                    guids = new byte[stepGUIDs.Length, 256];

                    for (i = 0; i < stepGUIDs.Length; i++)
                    {
                        byte[] temp = System.Text.Encoding.ASCII.GetBytes(stepGUIDs[i]);
                        for (j = 0; j < temp.Length; j++)
                            guids[i, j] = temp[j];
                    }
                }
            }

            Error err = vbaiGetInspectionMeasurements(_session.nativeSession, guids, size, ref measurementsPtr, out timeStamp);
            if (err != Error.Success)
            {
                ExceptionBuilder.CheckErrorAndThrow(err);
                measurementsArray = null;
                return measurementsArray;
            }
            measuresPriv = (MeasurementsPriv)Marshal.PtrToStructure(measurementsPtr, typeof(MeasurementsPriv));
            measurementsArray = new InspectionMeasurements[measuresPriv.numSteps];
            for (i=0; i<measuresPriv.numSteps; i++)
            {
                stepMeasuresPriv = (StepMeasurementsPriv)Marshal.PtrToStructure((IntPtr)((int)measuresPriv.StepMeasurementsIntPtr + offset1), typeof(StepMeasurementsPriv));
                offset1 += Marshal.SizeOf(typeof(StepMeasurementsPriv));
                measurementsArray[i].stepGUID = String.Copy(stepMeasuresPriv.stepGUID);
                measurementsArray[i].measurements = new StepMeasurements[stepMeasuresPriv.numMeasurements];
                offset2 = 0;
                for (j = 0; j < stepMeasuresPriv.numMeasurements; j++)
                {
                    stepMeasurePriv = (StepMeasurementPriv)Marshal.PtrToStructure((IntPtr)((int)stepMeasuresPriv.MeasurementsIntPtr + offset2), typeof(StepMeasurementPriv));
                    offset2 += Marshal.SizeOf(typeof(StepMeasurementPriv));
                    if (stepMeasurePriv.displayName != null)
                        measurementsArray[i].measurements[j].displayName = String.Copy(stepMeasurePriv.displayName);
                    if (stepMeasurePriv.resultGUID != null)
                        measurementsArray[i].measurements[j].resultGUID = String.Copy(stepMeasurePriv.resultGUID);
                    if (stepMeasurePriv.unit != null)
                        measurementsArray[i].measurements[j].unit = String.Copy(stepMeasurePriv.unit);
                    measurementsArray[i].measurements[j].measurementType = stepMeasurePriv.measurementType;
                    measurementsArray[i].measurements[j].measurement.type = stepMeasurePriv.measurement.type;
                    switch (measurementsArray[i].measurements[j].measurement.type)
                    {
                        case MeasurementDataType.NumericType:
                            measurementsArray[i].measurements[j].measurement.numData = stepMeasurePriv.measurement.measurement.numeric;
                            break;
                        case MeasurementDataType.BooleanType:
                            measurementsArray[i].measurements[j].measurement.boolData = stepMeasurePriv.measurement.measurement.boolean == 1;
                            break;
                        case MeasurementDataType.StringType:
                            measurementsArray[i].measurements[j].measurement.strData = Marshal.PtrToStringAnsi(stepMeasurePriv.measurement.measurement.strIntPtr);
                            break;
                        case MeasurementDataType.PassFailType:
                            measurementsArray[i].measurements[j].measurement.passData.numericResult = stepMeasurePriv.measurement.measurement.passFail.numericResult;
                            measurementsArray[i].measurements[j].measurement.passData.pass = stepMeasurePriv.measurement.measurement.passFail.pass == 1;
                            if (stepMeasurePriv.measurement.measurement.passFail.reportTextIntPtr != null)
                                measurementsArray[i].measurements[j].measurement.passData.reportText = Marshal.PtrToStringAnsi(stepMeasurePriv.measurement.measurement.passFail.reportTextIntPtr);
                            if (stepMeasurePriv.measurement.measurement.passFail.stringResultIntPtr != null)
                                measurementsArray[i].measurements[j].measurement.passData.stringResult = Marshal.PtrToStringAnsi(stepMeasurePriv.measurement.measurement.passFail.stringResultIntPtr);
                            break;
                        case MeasurementDataType.ArrayType:
                            ArrayMeasurementPriv arrayData = stepMeasurePriv.measurement.measurement.arrayData;
                            measurementsArray[i].measurements[j].measurement.arrayData.type = arrayData.type;
                            if (arrayData.size != 0)
                            {
                                if (arrayData.type == ArrayDataType.Numeric1DType)
                                {
                                        measurementsArray[i].measurements[j].measurement.arrayData.numArrayData = new double[arrayData.size];
                                        Marshal.Copy(arrayData.arrayDataIntPtr, measurementsArray[i].measurements[j].measurement.arrayData.numArrayData, 0, (int)arrayData.size);
                                }
                                else if (arrayData.type == ArrayDataType.Boolean1DType)
                                {
                                    measurementsArray[i].measurements[j].measurement.arrayData.boolArrayData = new bool[arrayData.size];
                                    int[] tempBoolArray = new int[arrayData.size];
                                    Marshal.Copy(arrayData.arrayDataIntPtr, tempBoolArray, 0, (int)arrayData.size);
                                    for (index = 0; index < arrayData.size; index++)
                                        measurementsArray[i].measurements[j].measurement.arrayData.boolArrayData[index] = tempBoolArray[index] == 1;
                                }
                                else
                                {
                                    measurementsArray[i].measurements[j].measurement.arrayData.strArrayData = new string[arrayData.size];
                                    for (index = 0; index < arrayData.size; index++)
                                    {
                                        // We have a char**, so dereference once to get char*
                                        IntPtr deref = (IntPtr)Marshal.PtrToStructure((IntPtr)((int)arrayData.arrayDataIntPtr + 4 * index), typeof(IntPtr));
                                        measurementsArray[i].measurements[j].measurement.arrayData.strArrayData[index] = Marshal.PtrToStringAnsi(deref);
                                    }
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            return measurementsArray;
        }

        public InspectionStep[] GetInspectionSteps()
        {
            IntPtr stepsPtr = IntPtr.Zero;
            int i = 0;
            int j = 0;
            InspectionStep[] steps;


            InspectionStepsPriv stepsPriv = new InspectionStepsPriv();
            InspectionStepPriv[] stepArrayPriv;
            PassFailConditionPriv[] conditionArrayPriv;
            Error err = vbaiGetInspectionSteps(_session.nativeSession, ref stepsPtr);
            if (err != Error.Success)
            {
                steps = null;
                ExceptionBuilder.CheckErrorAndThrow(err);
                return steps;
            }
            stepsPriv = (InspectionStepsPriv)Marshal.PtrToStructure(stepsPtr, typeof(InspectionStepsPriv));
            steps = new InspectionStep[stepsPriv.numSteps];
            stepArrayPriv = new InspectionStepPriv[stepsPriv.numSteps];
            for (i = 0; i < stepsPriv.numSteps; i++)
            {
                int structSize = Marshal.SizeOf(typeof(InspectionStepPriv));
                stepArrayPriv[i] = (InspectionStepPriv)Marshal.PtrToStructure((System.IntPtr)((int)stepsPriv.stepsIntPtr + i * structSize), typeof(InspectionStepPriv));
                steps[i].stateName = String.Copy(stepArrayPriv[i].stateName);
                steps[i].stepClass = String.Copy(stepArrayPriv[i].stepClass);
                steps[i].stepGUID = String.Copy(stepArrayPriv[i].stepGUID);
                steps[i].stepName = String.Copy(stepArrayPriv[i].stepName);
                steps[i].conditionsArray = new PassFailCondition[stepArrayPriv[i].numConditions];
                conditionArrayPriv = new PassFailConditionPriv[stepArrayPriv[i].numConditions];
                int offset = 0;
                for (j = 0; j < stepArrayPriv[i].numConditions; j++)
                {
                    PassFailConditionPriv temp = (PassFailConditionPriv)Marshal.PtrToStructure((System.IntPtr)((int)stepArrayPriv[i].conditionsArrayIntPtr + offset), typeof(PassFailConditionPriv));
                    offset += 28;   // increment for next item on next iteration. Marshal.SizeOf(typeof(NumericCondition));
                    steps[i].conditionsArray[j].typeCondition = temp.typeCondition;

                    if (temp.typeCondition == (PassFailConditionType.NumericCondition ))
                    {
                        steps[i].conditionsArray[j].used = (temp.conditionPriv.numericCondition.used==1);
                        steps[i].conditionsArray[j].num1 = temp.conditionPriv.numericCondition.value1;
                        steps[i].conditionsArray[j].num2 = temp.conditionPriv.numericCondition.value2;
                        steps[i].conditionsArray[j].numFunction = temp.conditionPriv.numericCondition.function;
                    }
                    else
                    {
                        steps[i].conditionsArray[j].used= (temp.conditionPriv.stringCondition.used==1);
                        steps[i].conditionsArray[j].str1 = Marshal.PtrToStringAnsi(temp.conditionPriv.stringCondition.value1);
                        steps[i].conditionsArray[j].str2 = Marshal.PtrToStringAnsi(temp.conditionPriv.stringCondition.value2);
                        steps[i].conditionsArray[j].strFunction = temp.conditionPriv.stringCondition.function;
                    }
                }
            }
            vbaiDispose(stepsPtr);
            return steps;
        }

        public Variable[] GetVariables()
        {
            int size;
            Variable[] variableArray;

            Error err = vbaiGetVariables(_session.nativeSession, null, out size);
            if (err != Error.Success)
            {
                variableArray = null;
                ExceptionBuilder.CheckErrorAndThrow(err);
                return variableArray;
            }
            variableArray = new Variable[size];
            err = vbaiGetVariables(_session.nativeSession, variableArray, out size);
            ExceptionBuilder.CheckErrorAndThrow(err);
            return variableArray;
        }

        public void SetVariables(VariableUpdateMode updateMode, VariableValue[] variableArray)
        {
            if (variableArray == null)
            {
                throw new ArgumentException("variableArray");
            }

            if (variableArray.Length == 0)
            {
                throw new ArgumentException("variableArray is empty");
            }

            VariableValuePriv[] variables = new VariableValuePriv[variableArray.Length];
            List<IntPtr> HandlesToDispose = new List<IntPtr>();
            for (int i = 0; i < variableArray.Length; i++)
            {
                VariableValueUnion val = new VariableValueUnion();
                variables[i].name = variableArray[i].name;
                variables[i].type = variableArray[i].type;
                switch (variableArray[i].type)
                {
                    case VariableDataType.BooleanVariableType:
                        val.boolValue = variableArray[i].boolValue;
                        break;
                    case VariableDataType.NumericVariableType:
                        val.numValue = variableArray[i].numValue;
                        break;
                    case VariableDataType.StringVariableType:
                        val.strValue = Marshal.StringToHGlobalAnsi(variableArray[i].strValue);
                        HandlesToDispose.Add(val.strValue);
                        break;
                    case VariableDataType.PointVariableType:
                        val.pointValue = variableArray[i].pointValue;
                        break;
                    case VariableDataType.ImageVariableType:
                        val.imgValue = variableArray[i].imgValue._image;
                        break;
                    case VariableDataType.Numeric1DVariableType:
                        val.num1DValue = Marshal.AllocHGlobal(sizeof(double) * variableArray[i].num1DValue.Length);
                        Marshal.Copy(variableArray[i].num1DValue, 0, val.num1DValue, variableArray[i].num1DValue.Length);
                        variables[i].arraySize = variableArray[i].num1DValue.Length;
                        HandlesToDispose.Add(val.num1DValue);
                        break;
                    case VariableDataType.Boolean1DVariableType:
                        // Unmanaged code has 4 bytes for bool and managed code only has 1 byte.
                        // When copying 1 byte into 4 byte data space, need to make sure memory is initialized to all zeros so when we set one byte to false (0),
                        // we don't have to worry about remaining 3 bytes possibly having data that would make the False look like a True in C code.
                        int boolSizeinBytes = Marshal.SizeOf(new bool()) * variableArray[i].bool1DValue.Length;

                        IntPtr currentLocation = val.bool1DValue = Marshal.AllocHGlobal(boolSizeinBytes);
                        byte[] emptyByteArray = new byte[boolSizeinBytes];
                        Marshal.Copy(emptyByteArray, 0, currentLocation, boolSizeinBytes);
                        for (int j = 0; j < variableArray[i].bool1DValue.Length; j++)
                        {
                            Marshal.WriteByte(currentLocation, Convert.ToByte(variableArray[i].bool1DValue[j]));
                            currentLocation = new IntPtr(currentLocation.ToInt64() + Marshal.SizeOf(new bool()));
                        }
                        variables[i].arraySize = variableArray[i].bool1DValue.Length;
                        HandlesToDispose.Add(val.bool1DValue);
                        break;
                    case VariableDataType.String1DVariableType:
                        IntPtr[] stringArray = new IntPtr[variableArray[i].str1DValue.Length];
                        for (int j = 0; j < variableArray[i].str1DValue.Length; j++)
                        {
                            stringArray[j] = Marshal.StringToHGlobalAnsi(variableArray[i].str1DValue[j]);
                            HandlesToDispose.Add(stringArray[j]);
                        }
                        val.str1DValue = Marshal.AllocHGlobal(Marshal.SizeOf(new IntPtr()) * stringArray.Length);
                        Marshal.Copy(stringArray, 0, val.str1DValue, variableArray[i].str1DValue.Length);

                        HandlesToDispose.Add(val.str1DValue);

                        variables[i].arraySize = variableArray[i].str1DValue.Length;
                        break;
                    default:
                        break;
                }
                variables[i].value = val;
            }
            Error err = vbaiSetVariables(_session.nativeSession, updateMode, variables, variables.Length);
            if (err != Error.Success)
            {
                ExceptionBuilder.CheckErrorAndThrow(err);
            }

            // Free allocated data
            for (int i = 0; i < HandlesToDispose.Count; i++)
            {
                Marshal.FreeHGlobal(HandlesToDispose[i]);
            }
            HandlesToDispose.Clear();
        }

        public InspectionStateInfo GetInspectionStateDiagram()
        {
            int stateSize, transitionSize;
            int firstState;
            InspectionStateInfo info;

            Error err = vbaiGetInspectionStateDiagram(_session.nativeSession, null, out stateSize, out firstState, null, out transitionSize);
            if (err != Error.Success)
            {
                ExceptionBuilder.CheckErrorAndThrow(err);
                info.firstState = 0;
                info.stateArray = null;
                info.transitionArray = null;
                return info;
            }
            info.stateArray = new StateInfo[stateSize];
            info.transitionArray = new TransitionInfo[transitionSize];
            err = vbaiGetInspectionStateDiagram(_session.nativeSession, info.stateArray, out stateSize, out info.firstState, info.transitionArray, out transitionSize);
            if (err != Error.Success)
            {
                ExceptionBuilder.CheckErrorAndThrow(err);
                info.firstState = 0;
                info.stateArray = null;
                info.transitionArray = null;
                return info;
            }
            return info;
        }

        public VBAIDateTimeZone GetTargetDateTime()
        {
            VBAIDateTimeZone dateTimeZone;

            Error err = vbaiGetTargetDateTime(_session.nativeSession, out dateTimeZone.dateTime, out dateTimeZone.timeZone);
            if (err != Error.Success)
            {
                ExceptionBuilder.CheckErrorAndThrow(err);
            }
            return dateTimeZone;
        }

        public void SetTargetDateTime(VBAIDateTime VBAIDateTime, VBAITimeZone VBAITimeZone)
        {
            Error err = vbaiSetTargetDateTime(_session.nativeSession, VBAIDateTime, VBAITimeZone);
            ExceptionBuilder.CheckErrorAndThrow(err);
        }

        public void RunInspectionOnce(int timeout)
        {
            Error err = vbaiRunInspectionOnce(_session.nativeSession, timeout);
            ExceptionBuilder.CheckErrorAndThrow(err);
        }

        public void StartInspection()
        {
            Error err = vbaiStartInspection(_session.nativeSession);
            ExceptionBuilder.CheckErrorAndThrow(err);
        }

        public void StopInspection()
        {
            Error err = vbaiStopInspection(_session.nativeSession);
            ExceptionBuilder.CheckErrorAndThrow(err);
        }

        public VisionImage GetInspectionImage(string imageGUID, int xReduction, int yReduction, out bool newImageAvailable)
        {
            newImageAvailable = false;
            Error err = vbaiGetInspectionImage(_session.nativeSession, imageGUID, xReduction, yReduction, _image._image, out newImageAvailable);
            ExceptionBuilder.CheckErrorAndThrow(err);
            return _image;
        }

        public void GetInspectionImage(string imageGUID, int xReduction, int yReduction, VisionImage userImage, out bool newImageAvailable)
        {
            newImageAvailable = false;
            Error err = vbaiGetInspectionImage(_session.nativeSession, imageGUID, xReduction, yReduction, userImage._image, out newImageAvailable);
            ExceptionBuilder.CheckErrorAndThrow(err);
        }

        public StepResult[] GetInspectionResults(out VBAIDateTime VBAIDateTime, out bool inspectionStatus)
        {
            int size;
            StepResult[] stepResultArray;
            Error err = vbaiGetInspectionResults(_session.nativeSession, null, out size, out VBAIDateTime, out inspectionStatus);
            if (err != Error.Success)
            {
                stepResultArray = null;
                ExceptionBuilder.CheckErrorAndThrow(err);
                return stepResultArray;
            }
            stepResultArray = new StepResult[size];
            err = vbaiGetInspectionResults(_session.nativeSession, stepResultArray, out size, out VBAIDateTime, out inspectionStatus);
            ExceptionBuilder.CheckErrorAndThrow(err);
            return stepResultArray;
        }

        public void EnableInspectionMeasurements()
        {
            Error err = vbaiEnableInspectionMeasurements(_session.nativeSession, true);
            ExceptionBuilder.CheckErrorAndThrow(err);
        }

        public void EnableInspectionMeasurements(bool enable)
        {
            Error err = vbaiEnableInspectionMeasurements(_session.nativeSession, enable);
            ExceptionBuilder.CheckErrorAndThrow(err);
        }

        public void ResetInspectionStatistics()
        {
            Error err = vbaiResetInspectionStatistics(_session.nativeSession);
            ExceptionBuilder.CheckErrorAndThrow(err);
        }

        public InspectionStatistics GetInspectionStatistics(out VBAIDateTime VBAIDateTime)
        {
            InspectionStatistics statistics;

            Error err = vbaiGetInspectionStatistics(_session.nativeSession, out statistics, out VBAIDateTime);
            ExceptionBuilder.CheckErrorAndThrow(err);
            return statistics;
        }

        public void CloseConnection()
        {
            Error err = vbaiCloseConnection(_session.nativeSession, true);
            ExceptionBuilder.CheckErrorAndThrow(err);
        }

        public void CloseConnection(bool closeLocalVBAIEngine)
        {
            Error err = vbaiCloseConnection(_session.nativeSession, closeLocalVBAIEngine);
            ExceptionBuilder.CheckErrorAndThrow(err);
        }

        public void GetStepGUIDFromName(string stateName, string stepName, out string stepGUID)
        {
            byte[] stepGUIDbytes = new byte[256];
            Error err = vbaiGetStepGUIDFromName(_session.nativeSession, stateName, stepName, stepGUIDbytes);
            if (err == Error.Success)
            {
                stepGUID = ConvertBytesToString(stepGUIDbytes);
            }
            else
            {
                stepGUID = "";
            }
            ExceptionBuilder.CheckErrorAndThrow(err);

        }

        internal Error GetErrorString(Error code, out string errorDescription)
        {
            byte[] str = new byte[256];
            Error err = vbaiGetErrorString(code, str, 256);
            errorDescription = ConvertBytesToString(str);
            return err;
        }
        #endregion

    }
}
