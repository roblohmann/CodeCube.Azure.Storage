using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Data.Tables;
using CodeCube.Azure.Storage.Constants;
using CodeCube.Azure.Storage.Interfaces;

namespace CodeCube.Azure.Storage
{
    /// <summary>
    /// Manager class to communicate with azure table storage
    /// </summary>
    public sealed class TableStorageManager : BaseManager, ITableStorageManager
    {
        private readonly TableClient _tableClient;

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

            _tableClient = new TableClient(Connectionstring, tableName);
            _tableClient.CreateIfNotExists();
        }

        /// <summary>
        /// Insert the specified entity to the table.
        /// </summary>
        /// <typeparam name="T">The type for the entities. Must inherit from <see cref="TableEntity">TableEntity.</see></typeparam>
        /// <param name="entity">The entity to insertin the tablestorage</param>
        /// <param name="cancellationToken">The cancellationtoken.</param>
        /// <returns></returns>
        public async Task<Response> Insert<T>(T entity, CancellationToken cancellationToken = default) where T : ITableEntity, new()
        {
            if (string.IsNullOrWhiteSpace(entity.RowKey))
            {
                throw new ArgumentException(ErrorConstants.Table.RowKeyIsRequired, nameof(entity));
            }
            if (string.IsNullOrWhiteSpace(entity.PartitionKey))
            {
                throw new ArgumentException(ErrorConstants.Table.PartitionKeyIsRequired, nameof(entity));
            }

            return await _tableClient.AddEntityAsync(entity, cancellationToken);
        }

        /// <summary>
        /// Insert a batch of entities.
        /// </summary>
        /// <typeparam name="T">The type for the entities. Must inherit from <see cref="TableEntity">TableEntity.</see></typeparam>
        /// <param name="entities">The batch of entities to insert.</param>
        /// <param name="cancellationToken">The cancellationtoken.</param>
        /// <returns></returns>
        public async Task<Response<IReadOnlyList<Response>>> InsertBatch<T>(List<T> entities, CancellationToken cancellationToken = default) where T : ITableEntity
        {
            var batch = new List<TableTransactionAction>();
            batch.AddRange(entities.Select(tableEntity => new TableTransactionAction(TableTransactionActionType.Add, tableEntity)));

            return await _tableClient.SubmitTransactionAsync(batch, cancellationToken);
        }

        /// <summary>
        /// Update the specified entity in the table storage.
        /// </summary>
        /// <typeparam name="T">The type for the entity. Must inherit from <see cref="TableEntity">TableEntity.</see></typeparam>
        /// <param name="entity">The entity to update.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<Response> Update<T>(T entity, CancellationToken cancellationToken = default) where T : ITableEntity, new()
        {
            if (string.IsNullOrWhiteSpace(entity.RowKey))
            {
                throw new ArgumentException(ErrorConstants.Table.RowKeyIsRequired, nameof(entity));
            }
            if (string.IsNullOrWhiteSpace(entity.PartitionKey))
            {
                throw new ArgumentException(ErrorConstants.Table.PartitionKeyIsRequired, nameof(entity));
            }

            return await _tableClient.UpdateEntityAsync(entity, entity.ETag, TableUpdateMode.Merge, cancellationToken);
        }

        /// <summary>
        /// Retrieve all entities of the given type.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="pageSize"></param>
        /// <param name="cancellationToken">The cancellationtoken.</param>
        /// <param name="propertiesToSelect"></param>
        /// <typeparam name="T">The type for the entities in the list. Must inherit from <see cref="TableEntity">TableEntity.</see></typeparam>
        /// <returns>All entities in the specified table matching the type.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public AsyncPageable<T> Retrieve<T>(string query, IEnumerable<string> propertiesToSelect = null, int pageSize = 25, CancellationToken cancellationToken = default)
            where T : class, ITableEntity, new()
        {
            return _tableClient.QueryAsync<T>(query, pageSize, propertiesToSelect, cancellationToken);
        }

        /// <summary>
        /// Retrieve an entity from the tablestorage
        /// </summary>
        /// <typeparam name="T">The type of the entity. Must inherit from <see cref="TableEntity">TableEntity.</see></typeparam>
        /// <param name="partitionKey">The partitionkey of the entity to retrieve.</param>
        /// <param name="rowKey">The rowkey of the entity to retrieve.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<Response<T>> Retrieve<T>(string partitionKey, string rowKey, CancellationToken cancellationToken = default) where T : class, ITableEntity, new()
        {
            return await _tableClient.GetEntityAsync<T>(partitionKey, rowKey, null, cancellationToken);
        }

        /// <summary>
        /// Delete the specified entity from the tablestorage.
        /// </summary>
        /// <typeparam name="T">The type of the entity. Must inherit from <see cref="TableEntity">TableEntity.</see></typeparam>
        /// <param name="rowKey"></param>
        /// <param name="cancellationToken">The cancellationtoken.</param>
        /// <param name="partitionKey"></param>
        /// <returns></returns>
        public async Task<Response> Delete<T>(string partitionKey, string rowKey, CancellationToken cancellationToken = default) where T : ITableEntity, new()
        {
            return await _tableClient.DeleteEntityAsync(partitionKey, rowKey, ETag.All, cancellationToken);
        }
    }
}
