namespace CodeCube.Azure.Storage.Interfaces
{
    internal interface IStorageFactory
    {
        /// <summary>
        /// Retrieve an instance of the <see cref="BlobStorageManager"/>.
        /// </summary>
        /// <param name="uri">The URI to the <see cref="BlobStorageManager"/></param>
        /// <param name="blobStorageAccountname">The accountname for the blob.</param>
        /// <param name="blobAccesskey">The access-key for the storage</param>
        /// <returns>An instance of the BLOB-storagemanager</returns>
        BlobStorageManager GetBlobStorageManager(string uri, string blobStorageAccountname, string blobAccesskey);
        /// <summary>
        /// Retrieve an instance of the <see cref="BlobStorageManager"/>.
        /// </summary>
        /// <param name="connectionstring">The connectionstring to the BLOB-storage.</param>
        /// <returns>An instance of the BLOB-storagemanager.</returns>
        BlobStorageManager GetBlobStorageManager(string connectionstring);

        /// <summary>
        /// Retrieve an instance of the <see cref="TableStorageManager"/>.
        /// </summary>
        /// <param name="tableConnectionstring">The connectionstring to the TABLE-storage.</param>
        /// <param name="tableName">The name of the table to connect to.</param>
        /// <returns></returns>
        TableStorageManager GetTableStorageManager(string tableConnectionstring, string tableName);

        /// <summary>
        /// Retrieve an instance of the <see cref="QueueManager"/>.
        /// </summary>
        /// <param name="connectionstring"></param>
        /// <param name="queueName"></param>
        /// <returns></returns>
        QueueManager GetQueueManager(string connectionstring, string queueName);
    }
}
