﻿using System;
using System.Runtime.Serialization;

namespace smartManage.Desktop
{
    [Serializable]
    public class CustomException : Exception
    {
        public CustomException()
        {

        }

        public CustomException(string message) : base(message)
        {

        }

        public CustomException(string message, Exception innerException) : base(message, innerException)
        {

        }

        public CustomException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
