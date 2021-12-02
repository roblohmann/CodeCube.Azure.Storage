using System;
using System.Text.RegularExpressions;
using CodeCube.Azure.Storage.Constants;
using CodeCube.Azure.Storage.Interfaces;

namespace CodeCube.Azure.Storage
{
    /// <summary>
    /// Helper class to retrieve the right helper to communicate with several types of storage.
    /// </summary>
    public sealed class StorageFactory : IStorageFactory
    {
        /// <summary>
        /// Retrieve an instance of the <see cref="BlobStorageManager"/>.
        /// </summary>
        /// <param name="uri">The URI to the <see cref="BlobStorageManager"/></param>
        /// <param name="blobStorageAccountname">The accountname for the blob.</param>
        /// <param name="blobAccesskey">The access-key for the storage</param>
        /// <returns>An instance of the <see cref="BlobStorageManager"/>.</returns>
        public BlobStorageManager GetBlobStorageManager(string uri, string blobStorageAccountname, string blobAccesskey)
        {
            if (string.IsNullOrWhiteSpace(uri))
            {
                throw new ArgumentNullException(nameof(uri), ErrorConstants.Blob.BlobUriRequired);
            }
            if (string.IsNullOrWhiteSpace(blobStorageAccountname))
            {
                throw new ArgumentNullException(nameof(blobStorageAccountname),ErrorConstants.Blob.BlobAccountRequired);
            }
            if (string.IsNullOrWhiteSpace(blobAccesskey))
            {
                throw new ArgumentNullException(nameof(blobAccesskey),ErrorConstants.Blob.BlobAccesskeyRequired);
            }

            return new BlobStorageManager(uri, blobStorageAccountname, blobAccesskey);
        }

        /// <summary>
        /// Retrieve an instance of the <see cref="BlobStorageManager"/>.
        /// </summary>
        /// <param name="connectionstring">The connectionstring to the blob storage</param>
        /// <returns>An instance of the <see cref="BlobStorageManager"/>.</returns>
        public BlobStorageManager GetBlobStorageManager(string connectionstring)
        {
            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring), ErrorConstants.Blob.BlobConnectionstringRequired);
            }

            return new BlobStorageManager(connectionstring);
        }

        /// <summary>
        /// Retrieve an instance of the <see cref="TableStorageManager"/>.
        /// </summary>
        /// <param name="tableConnectionstring">The connectionstring to the TABLE-storage.</param>
        /// <param name="tableName">The name of the table to connect to.</param>
        /// <returns>An instance of the <see cref="TableStorageManager"/>.</returns>
        public TableStorageManager GetTableStorageManager(string tableConnectionstring, string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableConnectionstring))
            {
                throw new ArgumentNullException(nameof(tableConnectionstring), ErrorConstants.Table.TableConnectionstringRequired);
            }
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException(nameof(tableName), ErrorConstants.Table.TableNameRequired);
            }

            if(!Regex.IsMatch(tableName, "^[a-zA-Z][a-zA-Z0-9]*$", RegexOptions.IgnoreCase)) 
                throw new InvalidOperationException(ErrorConstants.Table.TableNameNotAllowed);

            if (tableName.Length > ValidationConstants.MaxLengthTableStorage) 
                throw new InvalidOperationException(string.Format(ErrorConstants.Table.MaxLengthTableNameExceeded, ValidationConstants.MaxLengthTableStorage));

            return new TableStorageManager(tableConnectionstring, tableName);
        }

        /// <summary>
        /// Retrieve an instance of the <see cref="QueueManager"/>.
        /// </summary>
        /// <param name="connectionstring">The connectionstring to the queue</param>
        /// <param name="queueName">The name of the queue to connect to</param>
        /// <returns>An instance of the <see cref="QueueManager"/></returns>
        /// <exception cref="ArgumentNullException">Throws an <see cref="ArgumentNullException"/> when parameters are forgotten.</exception>
        public QueueManager GetQueueManager(string connectionstring, string queueName)
        {
            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring), ErrorConstants.Queue.QueueNameRequired);
            }
            if (string.IsNullOrWhiteSpace(queueName))
            {
                throw new ArgumentNullException(nameof(queueName), ErrorConstants.Queue.QueueConnectionstringRequired);
            }

            return new QueueManager(connectionstring, queueName);
        }
    }
}
