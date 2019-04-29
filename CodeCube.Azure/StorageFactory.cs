using System;
using CodeCube.Azure.BlobStorage;
using CodeCube.Azure.Constants;
using CodeCube.Azure.TableStorage;

namespace CodeCube.Azure
{
    /// <summary>
    /// Helper class to retrieve the right helper to communicate with several types of storage.
    /// </summary>
    public sealed class StorageFactory
    {
        public BlobStorageManager GetBlobStorageManager(string blobStorageAccountname, string blobPassword)
        {
            if(string.IsNullOrWhiteSpace(blobStorageAccountname))
            {
                throw new ArgumentNullException(nameof(blobStorageAccountname),ErrorConstants.BlobAccountRequired);
            }
            if (string.IsNullOrWhiteSpace(blobPassword))
            {
                throw new ArgumentNullException(nameof(blobPassword),ErrorConstants.BlobPasswordRequired);
            }

            return new BlobStorageManager(blobStorageAccountname, blobPassword);
        }

        public BlobStorageManager GetBlobStorageManager(string connectionstring)
        {
            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring), ErrorConstants.BlobConnectionstringRequired);
            }

            return new BlobStorageManager(connectionstring);
        }

        public TableStorageManager GetTableStorageManager(string tableConnectionstring, string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableConnectionstring))
            {
                throw new ArgumentNullException(nameof(tableConnectionstring), ErrorConstants.TableConnectionstringRequired);
            }
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException(nameof(tableName), ErrorConstants.TableNameRequired);
            }

            return new TableStorageManager(tableConnectionstring, tableName);
        }
    }
}
