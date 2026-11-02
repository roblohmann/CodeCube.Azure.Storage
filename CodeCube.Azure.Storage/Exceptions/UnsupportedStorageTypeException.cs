﻿using System;
using CodeCube.Azure.Storage.Enum;

namespace CodeCube.Azure.Storage.Exceptions
{
    /// <summary>
    /// Exception which is thrown when an unsupported <see cref="EStorageType"/> is requested.
    /// </summary>
    public sealed class UnsupportedStorageTypeException : Exception
    {
        /// <summary>
        /// Creates a new exception for the provided storagetype.
        /// </summary>
        /// <param name="storageType">The storagetype which is not supported.</param>
        public UnsupportedStorageTypeException(EStorageType storageType)
            : base($"The storagetype '${storageType}' is not supported!")
        {

        }
    }
}
