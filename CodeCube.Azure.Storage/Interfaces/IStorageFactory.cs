using System;

namespace CodeCube.Azure.Storage.Interfaces
{
    public interface IStorageFactory
    {
        /// <summary>
        /// Retrieve an instance of the <see cref="BlobStorageManager"/>.
        /// </summary>
        /// <param name="uri">The URI to the <see cref="BlobStorageManager"/></param>
        /// <param name="blobStorageAccountname">The accountname for the blob.</param>
        /// <param name="blobAccesskey">The access-key for the storage</param>
        /// <returns>An instance of the <see cref="BlobStorageManager"/>.</returns>
        BlobStorageManager GetBlobStorageManager(string uri, string blobStorageAccountname, string blobAccesskey);
        /// <summary>
        /// Retrieve an instance of the <see cref="BlobStorageManager"/>.
        /// </summary>
        /// <param name="connectionstring">The connectionstring to the blob storage</param>
        /// <returns>An instance of the <see cref="BlobStorageManager"/>.</returns>
        BlobStorageManager GetBlobStorageManager(string connectionstring);

        /// <summary>
        /// Retrieve an instance of the <see cref="TableStorageManager"/>.
        /// </summary>
        /// <param name="tableConnectionstring">The connectionstring to the TABLE-storage.</param>
        /// <param name="tableName">The name of the table to connect to.</param>
        /// <returns>An instance of the <see cref="TableStorageManager"/>.</returns>
        ITableStorageManager GetTableStorageManager(string tableConnectionstring, string tableName);

        /// <summary>
        /// Retrieve an instance of the <see cref="QueueManager"/>.
        /// </summary>
        /// <param name="connectionstring">The connectionstring to the queue</param>
        /// <param name="queueName">The name of the queue to connect to</param>
        /// <returns>An instance of the <see cref="QueueManager"/></returns>
        /// <exception cref="ArgumentNullException">Throws an <see cref="ArgumentNullException"/> when parameters are forgotten.</exception>
        QueueManager GetQueueManager(string connectionstring, string queueName);
    }
}
