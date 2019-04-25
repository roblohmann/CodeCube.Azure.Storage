using System;
using CodeCube.Azure.Enum;

namespace CodeCube.Azure.Exceptions
{
    public sealed class UnsupportedStorageTypeException : Exception
    {
        public UnsupportedStorageTypeException(EStorageType storageType)
            : base($"The storagetype '${storageType}' is not supported!")
        {

        }
    }
}
