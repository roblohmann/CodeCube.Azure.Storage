using System;
using CodeCube.Azure.Storage.Enum;

namespace CodeCube.Azure.Storage.Exceptions
{
    public sealed class UnsupportedStorageTypeException : Exception
    {
        public UnsupportedStorageTypeException(EStorageType storageType)
            : base($"The storagetype '${storageType}' is not supported!")
        {

        }
    }
}
