using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeCube.Azure.Constants;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace CodeCube.Azure
{
    public sealed class TableStorageManager : BaseManager
    {
        private CloudTable CloudTable { get; set; }
        private string TableName { get; }

        internal TableStorageManager(string connectionstring, string tableName) : base(connectionstring)
        {
            //Validate parameters
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentNullException(nameof(tableName), ErrorConstants.Table.TableNameRequired);
            }

            if (string.IsNullOrEmpty(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring), ErrorConstants.Table.TableConnectionstringRequired);
            }

            TableName = tableName;

            //Setup connection
            ConnectToCloudTable();
        }

        /// <summary>
        /// Insert the specified entity to the table.
        /// </summary>
        /// <typeparam name="T">The type of the entity. Must inherit from TableEntity.</typeparam>
        /// <param name="entity">The entity to insert or replace in the tablestorage</param>
        /// <param name="insertOnly">Indicates wether an TableEntity is only allowed to be inserted. Defaults to 'True'</param>
        /// <returns></returns>
        public async Task InsertOrReplace<T>(T entity, bool insertOnly = true) where T : TableEntity, new()
        {
            if (string.IsNullOrWhiteSpace(entity.RowKey))
            {
                throw new InvalidOperationException(ErrorConstants.Table.RowKeyIsRequired);
            }
            if (string.IsNullOrWhiteSpace(entity.PartitionKey))
            {
                throw new InvalidOperationException(ErrorConstants.Table.PartitionKeyIsRequired);
            }

            if (insertOnly)
            {
                var insertOperation = TableOperation.Insert(entity);
                await CloudTable.ExecuteAsync(insertOperation);
            }
            else
            {
                var insertOrMergeOperation = TableOperation.InsertOrReplace(entity);
                await CloudTable.ExecuteAsync(insertOrMergeOperation).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Insert a batch of entities.
        /// </summary>
        /// <typeparam name="T">The type for the entities. Must inherit from TableEntity.</typeparam>
        /// <param name="entities">The batch of entities to insert.</param>
        /// <returns></returns>
        public async Task InsertBatch<T>(List<T> entities) where T : TableEntity
        {
            // Create the batch operation.
            TableBatchOperation batchOperation = new TableBatchOperation();

            // Add both customer entities to the batch insert operation.
            foreach (T d in entities)
            {
                batchOperation.Insert(d);
            }

            // Execute the batch operation.
            await CloudTable.ExecuteBatchAsync(batchOperation).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieve an entity from the tablestorage
        /// </summary>
        /// <typeparam name="T">The type for the entities. Must inherit from TableEntity.</typeparam>
        /// <param name="partitionKey">The partitionkey of the entity to retrieve.</param>
        /// <param name="rowKey">The rowkey of the entity to retrieve.</param>
        /// <returns></returns>
        public async Task<T> Retrieve<T>(string partitionKey, string rowKey) where T : TableEntity, new()
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);
            TableResult result = await CloudTable.ExecuteAsync(retrieveOperation).ConfigureAwait(false);

            return result.Result as T;
        }

        /// <summary>
        /// Delete the specified entity from the tablestorage.
        /// </summary>
        /// <typeparam name="T">The type for the entities. Must inherit from TableEntity.</typeparam>
        /// <param name="entity">The entity to delete.</param>
        /// <returns></returns>
        public bool Delete<T>(T entity) where T : TableEntity, new()
        {
            var DeleteOperation = TableOperation.Delete(entity);
            CloudTable.ExecuteAsync(DeleteOperation).ConfigureAwait(false);

            return true;
        }

        #region privates
        private void ConnectToCloudTable()
        {
            CloudStorageAccount storageAccount = ConnectCloudStorageAccountWithConnectionString();
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            CloudTable = tableClient.GetTableReference(TableName);
            CloudTable.CreateIfNotExistsAsync().ConfigureAwait(false);
        }
        #endregion
    }
}
