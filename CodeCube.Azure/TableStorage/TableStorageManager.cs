using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace CodeCube.Azure.TableStorage
{
    public sealed class TableStorageManager
    {
        private CloudTable _cloudTable { get; set; }
        private string _connectionstring { get; }

        internal TableStorageManager(string connectionstring, string tableName)
        {
            _connectionstring = connectionstring;

            ConnectToCloudTable(tableName);
        }

        public async Task InsertOrReplace<T>(T entity, bool insertOnly = true) where T : TableEntity, new()
        {
            if (insertOnly)
            {
                var insertOperation = TableOperation.Insert(entity);
                await _cloudTable.ExecuteAsync(insertOperation);
            }
            else
            {
                var insertOrMergeOperation = TableOperation.InsertOrReplace(entity);
                await _cloudTable.ExecuteAsync(insertOrMergeOperation).ConfigureAwait(false);
            }
        }

        public async Task InsertBatch<T>(List<T> data) where T : TableEntity
        {
            // Create the batch operation.
            TableBatchOperation batchOperation = new TableBatchOperation();

            // Add both customer entities to the batch insert operation.
            foreach (T d in data)
            {
                batchOperation.Insert(d);
            }

            // Execute the batch operation.
            await _cloudTable.ExecuteBatchAsync(batchOperation).ConfigureAwait(false);
        }

        public async Task<T> Retrieve<T>(string partitionKey, string rowKey) where T : TableEntity, new()
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);
            TableResult result = await _cloudTable.ExecuteAsync(retrieveOperation).ConfigureAwait(false);

            return result.Result as T;
        }

        public bool Delete<T>(T entity) where T : TableEntity, new()
        {
            var DeleteOperation = TableOperation.Delete(entity);
            _cloudTable.ExecuteAsync(DeleteOperation).ConfigureAwait(false);

            return true;
        }

        #region privates
        private void ConnectToCloudTable(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentNullException(nameof(tableName), "tableName can't be empty");
            }

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_connectionstring);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            _cloudTable = tableClient.GetTableReference(tableName);
            _cloudTable.CreateIfNotExistsAsync().ConfigureAwait(false);
        }
        #endregion
    }
}
