using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeCube.Azure.Storage.Constants;
using Microsoft.Azure.Cosmos.Table;

namespace CodeCube.Azure.Storage
{
    /// <summary>
    /// Manager class to communicate with azure table storage
    /// </summary>
    public sealed class TableStorageManager : BaseManager
    {
        private CloudTable _cloudTable;
        private readonly string _tableName;

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

            _tableName = tableName;

            //Setup connection
            ConnectToCloudTable().ConfigureAwait(false).GetAwaiter().GetResult();
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
                await _cloudTable.ExecuteAsync(insertOperation);
            }
            else
            {
                var insertOrMergeOperation = TableOperation.InsertOrReplace(entity);
                await _cloudTable.ExecuteAsync(insertOrMergeOperation);
            }
        }

        /// <summary>
        /// Insert a batch of entities.
        /// </summary>
        /// <typeparam name="T">The type for the entities. Must inherit from TableEntity.</typeparam>
        /// <param name="entities">The batch of entities to insert.</param>
        /// <returns></returns>
        public async Task InsertBatch<T>(IEnumerable<T> entities) where T : TableEntity
        {
            // Create the batch operation.
            var batchOperation = new TableBatchOperation();

            // Add both customer entities to the batch insert operation.
            foreach (var tableEntity in entities)
            {
                batchOperation.Insert(tableEntity);
            }

            // Execute the batch operation.
            await _cloudTable.ExecuteBatchAsync(batchOperation);
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
            var retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);
            var result = await _cloudTable.ExecuteAsync(retrieveOperation);

            return result.Result as T;
        }

        /// <summary>
        /// Delete the specified entity from the tablestorage.
        /// </summary>
        /// <typeparam name="T">The type for the entities. Must inherit from TableEntity.</typeparam>
        /// <param name="entity">The entity to delete.</param>
        /// <returns></returns>
        public async Task<bool> Delete<T>(T entity) where T : TableEntity, new()
        {
            var deleteOperation = TableOperation.Delete(entity);
            await _cloudTable.ExecuteAsync(deleteOperation);

            return true;
        }

        #region privates
        private async Task ConnectToCloudTable()
        {
            var storageAccount = ConnectCloudStorageAccountWithConnectionString();
            var tableClient = storageAccount.CreateCloudTableClient();

            _cloudTable = tableClient.GetTableReference(_tableName);
            await _cloudTable.CreateIfNotExistsAsync();
        }

        private CloudStorageAccount ConnectCloudStorageAccountWithConnectionString()
        {
            if (!CloudStorageAccount.TryParse(Connectionstring, out var storageAccount))
            {
                throw new InvalidOperationException(ErrorConstants.InvalidConnectionstring);
            }

            return storageAccount;
        }
        #endregion
    }
}
