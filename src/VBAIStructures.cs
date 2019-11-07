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
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using NationalInstruments.Vision;
using NationalInstruments.VBAI.Enums;
using NativeSession = System.UInt32;

namespace NationalInstruments.VBAI.Structures
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct Engine
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)] public string sessionName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)] public string deviceName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]  public string IPAddress;
        [MarshalAs(UnmanagedType.Bool)] public bool engineReady;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] public string internalVersion;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]  public string modelCode;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] public string MACAddress;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct InspectionInfo
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)] public string path;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)] public string productName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)] public string creationDate;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)] public string lastModifiedDate;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)] public string comment;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct EngineStatusInfo
    {
        public EngineStatus status;
        public string inspectionName;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct InspectionStateInfo
    {
        public StateInfo[] stateArray;
        public int firstState;
        public TransitionInfo[] transitionArray;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct VBAIDateTimeZone
    {
        public VBAIDateTime dateTime;
        public VBAITimeZone timeZone;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct StringConditionPriv
    {
        public uint used;
        public IntPtr value1;
        public IntPtr value2;
        public StringFunctionCondition function;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct NumericConditionPriv
    {
        public uint used;
        public double value1;
        public double value2;
        public NumericFunctionCondition function;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VBAIPoint
    {
        public double x;
        public double y;
    }

    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi, Pack = 1)]
    internal struct conditionUnionPriv
    {
        [FieldOffset(0)]
        public NumericConditionPriv numericCondition;
        [FieldOffset(0)]
        public StringConditionPriv stringCondition;
    }

    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi, Pack = 1)]
    internal struct VariableValueUnion
    {
        [FieldOffset(0)]
        public double numValue;
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.Bool)] public bool boolValue;
        [FieldOffset(0)]
        public IntPtr strValue;
        [FieldOffset(0)]
        public VBAIPoint pointValue;
        [FieldOffset(0)]
        public IntPtr imgValue;
        [FieldOffset(0)]
        public IntPtr num1DValue;
        [FieldOffset(0)]
        public IntPtr bool1DValue;
        [FieldOffset(0)]
        public IntPtr str1DValue;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct PassFailCondition
    {
        public PassFailConditionType typeCondition;
        public bool used;
        public double num1;
        public double num2;
        public NumericFunctionCondition numFunction;
        public string str1;
        public string str2;
        public StringFunctionCondition strFunction;
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    internal struct VariableValuePriv
    {
        [MarshalAs(UnmanagedType.LPStr)]
        public string name;
        public VariableDataType type;
        public VariableValueUnion value;
        public int arraySize;
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct VariableValue
    {
        public string name;
        public VariableDataType type;
        public double numValue;
        public bool boolValue;
        public string strValue;
        public VBAIPoint pointValue;
        public VisionImage imgValue;
        public double[] num1DValue;
        public bool[] bool1DValue;
        public string[] str1DValue;
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    internal struct PassFailConditionPriv
    {
        public PassFailConditionType typeCondition;
        public conditionUnionPriv conditionPriv;
    };

    // This is structure is dynamically allocated so we don't have preallocated string sizes of array of conditions.
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct InspectionStep
    {
        public string stateName;
        public string stepName;
        public string stepGUID;
        public string stepClass;
        public PassFailCondition[] conditionsArray;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    internal struct InspectionStepPriv
    {
        [MarshalAs(UnmanagedType.LPStr)]
        public string stateName;
        [MarshalAs(UnmanagedType.LPStr)]
        public string stepName;
        [MarshalAs(UnmanagedType.LPStr)]
        public string stepGUID;
        [MarshalAs(UnmanagedType.LPStr)]
        public string stepClass;
        public IntPtr conditionsArrayIntPtr;
        public int numConditions;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    internal struct InspectionStepsPriv
    {
        public IntPtr stepsIntPtr;
        public int numSteps;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct Variable
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string name;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string GUID;
        public VariableType type;
        public VariableDataType dataType;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct StateInfo
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string stateGUID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string stateName;
        public bool terminal;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct TransitionInfo
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string transitionGUID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string transitionName;
        public bool defaultTransition;
        public int fromIndex;
        public int toIndex;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VBAIDateTime
    {
        public int year;
        public UInt32 month;
        public UInt32 day;
        public UInt32 hour;
        public UInt32 minute;
        public double second;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct StepResult
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string stepGUID;
        public bool pass;
        public double numericResult;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string stringResult;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string reportText;
        VBAIDateTime timeStamp;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    internal struct PassFailMeasurementPriv
    {
        public uint pass;
        public double numericResult;
        public IntPtr stringResultIntPtr;
        public IntPtr reportTextIntPtr;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct PassFailMeasurement
    {
        public bool pass;
        public double numericResult;
        public string stringResult;
        public string reportText;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct ArrayMeasurement
    {
        public ArrayDataType type;
        public double[] numArrayData;
        public bool[] boolArrayData;
        public string[] strArrayData;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    internal struct ArrayMeasurementPriv
    {
        public ArrayDataType type;
        public IntPtr arrayDataIntPtr;
        public uint size;
    }

    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi, Pack = 1)]
    internal struct measurementUnion
    {
        [FieldOffset(0)]
        public double numeric;
        [FieldOffset(0)]
        public uint boolean;
        [FieldOffset(0)]
        public IntPtr strIntPtr;
        [FieldOffset(0)]
        public PassFailMeasurementPriv passFail;
        [FieldOffset(0)]
        public ArrayMeasurementPriv arrayData;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    internal struct SingleMeasurementPriv
    {
        public MeasurementDataType type;
        public measurementUnion measurement;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct SingleMeasurement
    {
        public MeasurementDataType type;
        public double numData;
        public bool boolData;
        public string strData;
        public PassFailMeasurement passData;
        public ArrayMeasurement arrayData;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    internal struct StepMeasurementPriv
    {
        [MarshalAs(UnmanagedType.LPStr)]
        public string resultGUID;
        [MarshalAs(UnmanagedType.LPStr)]
        public string displayName;
        public MeasurementType measurementType;
        public SingleMeasurementPriv measurement;
        [MarshalAs(UnmanagedType.LPStr)]
        public string unit;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    internal struct StepMeasurementsPriv
    {
        [MarshalAs(UnmanagedType.LPStr)]
        public string stepGUID;
        public IntPtr MeasurementsIntPtr;
        public int numMeasurements;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    internal struct MeasurementsPriv
    {
        public int numSteps;
        public IntPtr StepMeasurementsIntPtr;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct StepMeasurements
    {
        public string resultGUID;
        public string displayName;
        public MeasurementType measurementType;
        public SingleMeasurement measurement;
        public string unit;

    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct InspectionMeasurements
    {
        public string stepGUID;
        public StepMeasurements[] measurements;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct InspectionStatistics
    {
        public double iterationsPerSecond;
        public uint numPass;
        public uint numFail;
        public double yieldPercent;
        public double activeTime;
        public double idleTime;
        public double ratioActiveIdle;

    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ImageInfo
    {
        public byte[] imageData;
        public int numBytes;
        public int width;
        public int height;
        public NationalInstruments.VBAI.Enums.ImageType type;
    }

    public struct Session
    {
        public bool Valid
        {
            get { return nativeSession != 0; }
        }
        public uint Value
        {
            get { return nativeSession; }
        }
        //.NET will initialize session to 0
        internal  NativeSession nativeSession;
    }
}
