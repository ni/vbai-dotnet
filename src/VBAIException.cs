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
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Diagnostics;
using System.Globalization;
using NationalInstruments.VBAI;
using NationalInstruments.VBAI.Enums;


namespace NationalInstruments.VBAI.Internal
{
    //==============================================================================================
    /// <summary>
    /// Represents an error code received from the underlying driver.
    /// </summary>
    /// <remarks>
    /// Occasionally, errors occur when this API makes calls into the underlying driver. When an error occurs, the
    /// error is exposed through this exception class.
    /// </remarks>
    /// <threadsafety safety="safe"/>
    //==============================================================================================

    internal sealed class ExceptionBuilder
    {
        private ExceptionBuilder()
        {
        }

        public static void CheckErrorAndThrow(Error status)
        {
            if (status < 0)
                throw VBAIError(status);
        }

        public static Exception VBAIError(Error error)
        {
            Debug.Assert(error < 0);
            string message = "";

            try
            {
                byte[] str = new byte[256];
                Error err = VBAIEngine.vbaiGetErrorString(error, str, 256);
                message = VBAIEngine.ConvertBytesToString(str);

                message = message.Replace("\n", Environment.NewLine);
            }
            catch (Exception)
            {
                message = String.Format(CultureInfo.InvariantCulture, "Could not get error message.");
            }
            finally
            {
                throw new VBAIException(message, (int)error);
            }
        }
    }
}

namespace NationalInstruments.VBAI
{
    [Serializable]
    public class VBAIException : SystemException, ISerializable
    {
        private const string ErrorCodeKey = "ErrorCode";

        private int _errorCode;

        //==========================================================================================
        /// <summary>
        /// Initializes a new instance of the <see cref="NationalInstruments.Tdms.TdmsException"/> class.
        /// </summary>
        //==========================================================================================
        public VBAIException()
            : base()
        {
        }

        //==========================================================================================
        /// <summary>
        /// Initializes a new instance of the <see cref="NationalInstruments.Tdms.TdmsException"/> class using the
        /// given error message.
        /// </summary>
        /// <param name="message">
        /// An error message describing this exception.
        /// </param>
        //==========================================================================================
        public VBAIException(string message)
            : this(message, null)
        {
        }

        //==========================================================================================
        /// <summary>
        /// Initializes a new instance of the <see cref="NationalInstruments.Tdms.TdmsException"/> class using the
        /// given error message and inner exception.
        /// </summary>
        /// <param name="message">
        /// An error message describing this exception.
        /// </param>
        /// <param name="inner">
        /// The inner exception that causes this exception to be thrown.
        /// </param>
        //==========================================================================================
        public VBAIException(string message, Exception inner)
            : this(message, inner, 0)
        {
        }

        //==========================================================================================
        /// <summary>
        /// Initializes a new instance of the <see cref="NationalInstruments.Tdms.TdmsException"/> class
        /// with the given error message and error code.
        /// </summary>
        /// <param name="message">
        /// An error message describing this exception.
        /// </param>
        /// <param name="errorCode">
        /// The error code associated with this exception.
        /// </param>
        //==========================================================================================
        public VBAIException(string message, int errorCode)
            : this(message, null, errorCode)
        {
        }

        //==========================================================================================
        /// <summary>
        /// Initializes a new instance of the <see cref="NationalInstruments.Tdms.TdmsException"/> class using the
        /// given error message, inner exception, and error code.
        /// </summary>
        /// <param name="message">
        /// An error message describing this exception.
        /// </param>
        /// <param name="inner">
        /// The inner exception that caused this exception to be thrown.
        /// </param>
        /// <param name="errorCode">
        /// The error code associated with this exception.
        /// </param>
        //==========================================================================================
        public VBAIException(string message, Exception inner, int errorCode)
            : base(message, inner)
        {
            _errorCode = errorCode;
        }


        //==========================================================================================
        /// <summary>
        /// Initializes a new instance of <see cref="NationalInstruments.Tdms.TdmsException"/> using
        /// serialized data.
        /// </summary>
        /// <param name="info">
        /// Object that holds the serialized object data.
        /// </param>
        /// <param name="context">
        /// Contextual information about the source or destination.
        /// </param>
        //==========================================================================================
        protected VBAIException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            if (info == null)
                throw new ArgumentNullException("info");

            _errorCode = (int)info.GetInt32(ErrorCodeKey);
        }

        //==========================================================================================
        /// <summary>
        /// Sets the <see cref="System.Runtime.Serialization.SerializationInfo"/> object with information about the exception.
        /// </summary>
        /// <param name="info">
        /// The object that holds the serialized object data.
        /// </param>
        /// <param name="context">
        /// Contextual information about the source or destination.
        /// </param>
        //==========================================================================================
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            if (info == null)
                throw new ArgumentNullException("info");

            info.AddValue(ErrorCodeKey, _errorCode);
        }

        //==========================================================================================
        /// <summary>
        /// Gets the error code associated with the error.
        /// </summary>
        /// <value>
        /// The value of the error code.
        /// </value>
        //==========================================================================================
        public int ErrorCode
        {
            get
            {
                return _errorCode;
            }
        }
    }
}
