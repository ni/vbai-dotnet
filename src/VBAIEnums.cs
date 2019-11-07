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

namespace NationalInstruments.VBAI.Enums
{

    public class Version
    {
        public const string VBAI_2010 = "2010";
        public const string VBAI_2011 = "2011";
        public const string VBAI_2012 = "2012";
        public const string VBAI_2013 = "2013";
        public const string VBAI_2014 = "2014";
        public const string VBAI_2014_64 = "2014_64";
        public const string VBAI_2015 = "2015";
        public const string VBAI_2015_64 = "2015_64";
        public const string VBAI_2018 = "2018";
        public const string VBAI_2018_64 = "2018_64";
        public const string VBAI_2019 = "2019";
        public const string VBAI_2019_64 = "2019_64";
    }

    public enum EngineStatus : int
    {
        Offline = 0,
        Online = 1,
        Initialized = 2,
        Configured = 3,
        Inspecting = 4,
    }

    public enum StringFunctionCondition : int
    {
        Str_Equal = 0,
        Str_NotEqual = 1,
        Str_MatchPattern = 2,
        Str_Less = 3,
        Str_LessOrEqual = 4,
        Str_Greater = 5,
        Str_GreaterOrEqual = 6,
        Str_InRange = 7,
    }

    public enum NumericFunctionCondition
    {
        Num_Less = 0,
        Num_LessOrEqual = 1,
        Num_Greater = 2,
        Num_GreaterOrEqual = 3,
        Num_InRange = 4,
        Num_Equal = 5,
        Num_NotEqual = 6,
    }

    public enum PassFailConditionType
    {
        NumericCondition = 0,
        StringCondition = 1,
    }

    public enum VBAITimeZone
    {
        GMT = 0,
        GMTp1 = 1,
        GMTp2 = 2,
        GMTp3 = 3,
        GMTp4 = 4,
        GMTp5 = 5,
        GMTp6 = 6,
        GMTp7 = 7,
        GMTp8 = 8,
        GMTp9 = 9,
        GMTp10 = 10,
        GMTp11 = 11,
        GMTp12 = 12,
        GMTm11 = 13,
        GMTm10 = 14,
        GMTm9 = 15,
        GMTm8 = 16,
        GMTm7 = 17,
        GMTm6 = 18,
        GMTm5 = 19,
        GMTm4 = 20,
        GMTm3 = 21,
        GMTm2 = 22,
        GMTm1 = 23,
    }

    public enum MeasurementType
    {
        PassFail = 0,
        XPosPix = 1,
        YPosPix = 2,
        XPosCal = 3,
        YPosCal = 4,
        Score = 5,
        DistPix = 6,
        DistCal = 7,
        Angle = 8,
        Straightness = 9,
        RadiusPix = 10,
        RadiusCal = 11,
        Roundness = 12,
        AvgIntensity = 13,
        StdDevIntensity = 14,
        MinIntensity = 15,
        MaxIntensity = 16,
        AreaPix = 17,
        AreaCal = 18,
        PercentArea = 19,
        ID = 20,
        NumMatches = 21,
        NumObjects = 22,
        NumEdges = 23,
        Orientation = 24,
        AspectRatio = 25,
        NumHoles = 26,
        Boolean = 27,
        Numeric = 28,
        String = 29,
        NetworkBoolean = 30,
        NetworkNumeric = 31,
        NetworkString = 32,
        CustUIBoolean = 33,
        CustUINumeric = 34,
        CustUIString = 35,
    }

    public enum ImageType
    {
        VBAI_IMAGE_U8 = 0,      // 8-bit unsigned integer grayscale
        VBAI_IMAGE_I16 = 1,     // 16-bit signed integer grayscale
        VBAI_IMAGE_SGL = 2,     // 32-bit floating-point grayscale
        VBAI_IMAGE_RGB = 4,     // RGB color
    }

    public enum MeasurementDataType
    {
        NumericType = 0,
        BooleanType = 1,
        StringType = 2,
        PassFailType = 3,
        ArrayType = 4,
    }

    public enum VariableDataType
    {
        NumericVariableType = 0,
        BooleanVariableType = 1,
        StringVariableType = 2,
        PointVariableType = 3,
        ImageVariableType = 4,
        Numeric1DVariableType = 5,
        Boolean1DVariableType = 6,
        String1DVariableType = 7,
    }

    public enum VariableType
    {
        InspectionVariable = 0,
        SystemVariable = 1,
        SystemUserDefinedVariable = 2,
        NetworkVariable = 3,
    }

    public enum ArrayDataType
    {
        Numeric1DType = 0,
        Boolean1DType = 1,
        String1DType = 2,
    }

    public enum VariableUpdateMode
    {
        UpdateOnNextIteration = 0,
        UpdateImmediately = 1,
    }

    public enum Error
    {
        Success = unchecked((int)0),                            // Success
        ErrorSessionNotFound = unchecked((int)-354700),         // Cannot find the specified session
        ErrorInvalidName = unchecked((int)-354701),             // Name is not valid for local engine. Cannot contain spaces. Only alphanumeric characters allowed
        ErrorTooManyLocalEngines = unchecked((int)-354702),     // Too many local VBAI Engines launched
        ErrorInstallError = unchecked((int)-354703),            // Software installation corrupt. Missing critical files.
        ErrorCloseLocalEngine = unchecked((int)-354704),        // Error occurred trying to close the local VBAI engine.
        ErrorConnectionInUse = unchecked((int)-354705),         // Connection failed. Another instance may already be connected to engine
        ErrorInvalidSession = unchecked((int)-354706),          // The session is no longer valid
        ErrorInitializeSession = unchecked((int)-354707),       // Error initializing the session
        ErrorTimeout = unchecked((int)-354708),                 // Timeout error. The engine did not complete the task in time.
        ErrorFunctionNotSupported = unchecked((int)-354709),    // This function is not supported by the version of the VBAI engine called.
        ErrorGetImage = unchecked((int)-354710),                // There was an error reading the image from the engine
        ErrorAcquireLockTimeout = unchecked((int)-354711),      // Timeout error. The engine is busy with another session
        ErrorSystemMemoryFull = unchecked((int)-354712),        // Out of memory and cannot copy image
        ErrorDestinationImage = unchecked((int)-354713),        // The destination image is not a valid Vision Image
        ErrorNoImage = unchecked((int)-354714),                 // There is no image available from the engine.
        ErrorRemoteFunctionOnly = unchecked((int)-354715),      // This function is only supported for Remote Targets, not local engines.
        ErrorEnumerate = unchecked((int)-354716),               // There was an error enumerating targets on the network.
        ErrorEngineTimeout = unchecked((int)-354717),           // The engine did not respond in time.
        ErrorUnknown = unchecked((int)-354718),                 // An unknown error occurred
        ErrorCannotOpenFile = unchecked((int)-354800),          // The file cannot be opened. The settings in Vision Builder System Config.ini may be corrupt.
        ErrorFileAlreadyOpen = unchecked((int)-354801),         // Cannot open the inspection because it is already open in another instance of Vision Builder AI.
        ErrorFileNotValid = unchecked((int)-354802),            // The file selected is not a valid inspection.
        ErrorFileCorrupt = unchecked((int)-354803),             // The selected inspection file is corrupt.
        ErrorOpenFile = unchecked((int)-354804),                // Cannot open the inspection file.
        ErrorFileCRCBad = unchecked((int)-354805),              // The selected inspection file is corrupt. The signature of the file is incorrect.
        ErrorFileInvalid = unchecked((int)-354806),             // Invalid inspection file selected.
        ErrorFieVersionOlder = unchecked((int)-354807),         // The version of the inspection is earlier than the version of Vision Builder AI that you are trying to open it with.
        ErrorFileVersionNewer = unchecked((int)-354808),        // The version of the inspection is later than the version of Vision Builder AI you are trying to open it with.
        ErrorLoadInspectionFailed = unchecked((int)-354809),    // The validation of the inspection returns errors.
        ErrorStateDiagramNotValid = unchecked((int)-354810),    // The selected inspection has a state diagram that is not runnable (i.e. can't get from start to end state).
        ErrorConnectionFailed = unchecked((int)-354811),        // Connection failed. Device may still be loading Vision Builder AI or the IP address is invalid.
        ErrorServerConnectionFailed = unchecked((int)-354812),  // Connection failed. Device may still be loading Vision Builder AI or the IP address is invalid.
        ErrorTooManyConnections = unchecked((int)-354813),      // Access denied. Too many hosts are already connected to this target.
        ErrorPasswordIncorrect = unchecked((int)-354814),       // The password is incorrect.
        ErrorVersionMismatch = unchecked((int)-354815),         // The versions of Vision Builder AI running on the host and the selected remote target do not match.
        ErrorDisconnected = unchecked((int)-354816),            // The connection to the remote target was lost because another user has connected to the remote target.
        ErrorAlreadyConnected = unchecked((int)-354817),        // Another instance of Vision Builder AI is already connected to the target.
        ErrorConnectionLost = unchecked((int)-354818),          // The connection to the Vision Builder AI engine has been lost.
        ErrorServerConnectionLost = unchecked((int)-354819),    // The connection to the Vision Builder AI engine has been lost.
        ErrorEvaluationPeriodOver = unchecked((int)-354820),    // The 30-day evaluation period of the software has expired.
        ErrorAlreadyRunning = unchecked((int)-354821),          // The inspection is running. You must stop the inspection before you can use the Run Inspection Once function.
        ErrorInvalidImageGUID = unchecked((int)-354822),        // No image is available for the specified GUID. This GUID may not be valid.
        ErrorNoImageAvailable = unchecked((int)-354997),        // No image is currently available from the Vision Builder AI engine.
        ErrorEngineClosed = unchecked((int)-354998),            // The connection to the local Vision Builder AI engine has been lost.
        ErrorReadEngineResponse = unchecked((int)-354999),      // There was an error reading the response from the Vision Builder AI Engine.
        ErrorInvalidPointer = unchecked((int)-354900),          // NULL pointer not expected
        ErrorNotEnoughMemory = unchecked((int)-354901),         // Not enough memory to complete the operation. This may occur if the pointer size specified is not large enough to contain all the data returned by the function.
        ErrorStringLength = unchecked((int)-354902),            // Size of string provided is not long enough. Function needs a longer string to successfully complete.
        ErrorNotVBAIError = -354903,                            // Returned by vbaiGetErrorString if error code specified is not a Vision Builder AI error.
    }
}
