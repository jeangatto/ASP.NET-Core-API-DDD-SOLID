using System;
using System.Diagnostics.CodeAnalysis;

namespace SGP.Shared.Exceptions
{
    [SuppressMessage("Design", "RCS1194:Implement exception constructors.")]
    [SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly")]
    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message)
        {
        }
    }
}