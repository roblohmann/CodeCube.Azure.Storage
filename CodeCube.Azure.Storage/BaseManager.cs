﻿namespace CodeCube.Azure.Storage
{
    /// <summary>
    /// Base class for the storage managers which holds the connectionstring to the storage.
    /// </summary>
    public abstract class BaseManager
    {
        /// <summary>
        /// The connectionstring to the storage. Is <c>null</c> when the manager was not created with a connectionstring.
        /// </summary>
        protected readonly string? Connectionstring;

        /// <summary>
        /// Creates a new manager without a connectionstring.
        /// </summary>
        protected BaseManager()
        {
        }

        /// <summary>
        /// Creates a new manager with the provided connectionstring.
        /// </summary>
        /// <param name="connectionstring">The connectionstring to the storage.</param>
        protected BaseManager(string connectionstring)
        {
            Connectionstring = connectionstring;
        }
    }
}
