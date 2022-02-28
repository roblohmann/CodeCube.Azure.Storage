using System;
using System.Collections.Generic;
using System.Threading;
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
        private bool _isConnected;

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
        }

        public async Task Connect()
        {
            //Setup connection
            await ConnectToCloudTable().ConfigureAwait(false);

            _isConnected = true;
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
                throw new ArgumentException(ErrorConstants.Table.RowKeyIsRequired, nameof(entity));
            }
            if (string.IsNullOrWhiteSpace(entity.PartitionKey))
            {
                throw new ArgumentException(ErrorConstants.Table.PartitionKeyIsRequired, nameof(entity));
            }

            if (!_isConnected) throw new InvalidOperationException(ErrorConstants.Table.NotConnected);

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
            if (!_isConnected) throw new InvalidOperationException(ErrorConstants.Table.NotConnected);

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
        /// Retrieve all entities of the given type.
        /// </summary>
        /// <typeparam name="T">The type of entity. Must inherit from TableEntity</typeparam>
        /// <returns>All entities in the specified table matching the type.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<List<T>> Retrieve<T>() where T : TableEntity, new()
        {
            if (!_isConnected) throw new InvalidOperationException(ErrorConstants.Table.NotConnected);

            TableContinuationToken token = null;
            var entities = new List<T>();
            do
            {
                var queryResult = await _cloudTable.ExecuteQuerySegmentedAsync(new TableQuery<T>(), token);
                entities.AddRange(queryResult.Results);
                token = queryResult.ContinuationToken;

            } while (token != null);

            return entities;
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
            if (!_isConnected) throw new InvalidOperationException(ErrorConstants.Table.NotConnected);

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
            if (!_isConnected) throw new InvalidOperationException(ErrorConstants.Table.NotConnected);

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
