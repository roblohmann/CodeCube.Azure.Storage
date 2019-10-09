using System;
using CodeCube.Azure.Constants;
using Microsoft.WindowsAzure.Storage;

namespace CodeCube.Azure
{
    public abstract class BaseManager
    {
        protected readonly string _connectionstring;

        protected BaseManager()
        {
            
        }
        protected BaseManager(string connectionstring)
        {
            _connectionstring = connectionstring;
        }

        protected CloudStorageAccount ConnectCloudStorageAccountWithConnectionString()
        {
            if (!CloudStorageAccount.TryParse(_connectionstring, out CloudStorageAccount storageAccount))
            {
                throw new InvalidOperationException(ErrorConstants.InvalidConnectionstring);
            }

            return storageAccount;
        }
    }
}