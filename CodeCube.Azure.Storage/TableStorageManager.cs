using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
    public sealed class TableStorageManager : ITableStorageManager
    {
        private readonly TableClient _tableClient;

        internal TableStorageManager(TableClient tableClient)
        {
            //Validate parameters
            if (tableClient == null)
            {
                throw new ArgumentNullException(nameof(tableClient), ErrorConstants.Table.TableClientRequired);
            }

            _tableClient = tableClient;
            _tableClient.CreateIfNotExists();
        }
        
        /// <summary>
        /// Retrieve an entity from the tablestorage
        /// </summary>
        /// <typeparam name="T">The type of the entity. Must inherit from <see cref="TableEntity">TableEntity.</see></typeparam>
        /// <param name="partitionKey">The partitionkey of the entity to retrieve.</param>
        /// <param name="rowKey">The rowkey of the entity to retrieve.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="RequestFailedException"></exception>
        public async Task<Response<T>> GetSingle<T>(string partitionKey, string rowKey, CancellationToken cancellationToken = default) where T : class, ITableEntity, new()
        {            
            return await _tableClient.GetEntityAsync<T>(partitionKey, rowKey, null, cancellationToken);
        }

        /// <summary>
        /// Retrieve all entities of the given type.
        /// </summary>
        /// <param name="query">The query to use for filtering entites.</param>
        /// <param name="cancellationToken">The cancellationtoken.</param>        
        /// <typeparam name="T">The type for the entities in the list. Must inherit from <see cref="TableEntity">TableEntity.</see></typeparam>
        /// <returns>All entities in the specified table matching the type.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="RequestFailedException"></exception>
        public async Task<T> GetSingle<T>(Expression<Func<T,bool>> query, CancellationToken cancellationToken = default)
            where T : class, ITableEntity, new()
        {
            var responseList = new List<T>();
            var queryResponse = _tableClient.QueryAsync<T>(query, 1, null, cancellationToken);

            await foreach (var responseObject in queryResponse){
                responseList.Add(responseObject);
            }

            return responseList.SingleOrDefault();
        }

        /*
        /// <summary>
        /// Updates the specified list of entites in the table storage.
        /// </summary>
        /// <typeparam name="T">The type for the entity. Must inherit from <see cref="TableEntity">TableEntity.</see></typeparam>
        /// <param name="listOfEntities">The entity to update.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<Response<IReadOnlyList<Response>>> Update<T>(List<T> listOfEntities, CancellationToken cancellationToken = default) where T : ITableEntity, new()
        {
            List<TableTransactionAction> updateEntitiesBatch = new List<TableTransactionAction>();

            updateEntitiesBatch.AddRange(listOfEntities.Select(tableEntity => new TableTransactionAction(TableTransactionActionType.UpdateMerge, tableEntity)));

            return await _tableClient.SubmitTransactionAsync(updateEntitiesBatch, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieve all entities of the given type.
        /// </summary>
        /// <param name="query">The query to use for filtering entites. Eg: PartitionKey eq 'myPartitionKey' </param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <param name="cancellationToken">The cancellationtoken.</param>
        /// <param name="propertiesToSelect">The properties eg coluns to select from your tableEntity.</param>
        /// <typeparam name="T">The type for the entities in the list. Must inherit from <see cref="TableEntity">TableEntity.</see></typeparam>
        /// <returns>All entities in the specified table matching the type.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="RequestFailedException"></exception>
        [Obsolete("Will be removed in a future version. Please use overload which returns List<T>")]
        public AsyncPageable<T> Query<T>(string query, IEnumerable<string> propertiesToSelect = null, int pageSize = 25, CancellationToken cancellationToken = default)
            where T : class, ITableEntity, new()
        {
            return _tableClient.QueryAsync<T>(query, pageSize, propertiesToSelect, cancellationToken);
        }
        */

        /// <summary>
        /// Retrieve all entities of the given type.
        /// </summary>
        /// <param name="query">The query to use for filtering entites.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <param name="cancellationToken">The cancellationtoken.</param>
        /// <typeparam name="T">The type for the entities in the list. Must inherit from <see cref="TableEntity">TableEntity.</see></typeparam>
        /// <returns>All entities in the specified table matching the type.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="RequestFailedException"></exception>
        public async Task<List<T>> Query<T>(Expression<Func<T,bool>> query, int pageSize = 25, CancellationToken cancellationToken = default)
            where T : class, ITableEntity, new()
        {
            var queryResponse = _tableClient.QueryAsync<T>(query, pageSize, null, cancellationToken);

            return await ConvertToList<T>(queryResponse, cancellationToken);
        }

        /// <summary>
        /// Retrieve all entities of the given type.
        /// </summary>
        /// <param name="query">The query to use for filtering entites.</param>
        /// <param name="propertiesToSelect">The properties eg coluns to select from your tableEntity.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <param name="cancellationToken">The cancellationtoken.</param>
        /// <typeparam name="T">The type for the entities in the list. Must inherit from <see cref="TableEntity">TableEntity.</see></typeparam>
        /// <returns>All entities in the specified table matching the type.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="RequestFailedException"></exception>
        public async Task<List<T>> Query<T>(Expression<Func<T,bool>> query, IEnumerable<string> propertiesToSelect = null, int pageSize = 25, CancellationToken cancellationToken = default)
            where T : class, ITableEntity, new()
        {
            var queryResponse = _tableClient.QueryAsync<T>(query, pageSize, propertiesToSelect, cancellationToken);

            return await ConvertToList<T>(queryResponse, cancellationToken);
        }

        /// <summary>
        /// Insert the specified entity to the table.
        /// </summary>
        /// <typeparam name="T">The type for the entities. Must inherit from <see cref="TableEntity">TableEntity.</see></typeparam>
        /// <param name="entity">The entity to insertin the tablestorage</param>
        /// <param name="cancellationToken">The cancellationtoken.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="RequestFailedException"></exception>
        public async Task<Response> Insert<T>(T entity, CancellationToken cancellationToken = default) where T : ITableEntity, new()
        {
            if (string.IsNullOrWhiteSpace(entity.RowKey))
            {
                throw new ArgumentNullException(ErrorConstants.Table.RowKeyIsRequired, nameof(entity));
            }
            if (string.IsNullOrWhiteSpace(entity.PartitionKey))
            {
                throw new ArgumentNullException(ErrorConstants.Table.PartitionKeyIsRequired, nameof(entity));
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
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="RequestFailedException"></exception>
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
        /// <param name="cancellationToken">The cancellationtoken.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="RequestFailedException"></exception>
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
        /// Replace the specified entity in the table storage.
        /// </summary>
        /// <typeparam name="T">The type for the entity. Must inherit from <see cref="TableEntity">TableEntity.</see></typeparam>
        /// <param name="entity">The entity to replace.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<Response> Replace<T>(T entity, CancellationToken cancellationToken = default) where T : ITableEntity, new()
        {
            if (string.IsNullOrWhiteSpace(entity.RowKey))
            {
                throw new ArgumentException(ErrorConstants.Table.RowKeyIsRequired, nameof(entity));
            }
            if (string.IsNullOrWhiteSpace(entity.PartitionKey))
            {
                throw new ArgumentException(ErrorConstants.Table.PartitionKeyIsRequired, nameof(entity));
            }

            return await _tableClient.UpdateEntityAsync(entity, entity.ETag, TableUpdateMode.Replace, cancellationToken);
        }

        /// <summary>
        /// Delete the specified entity from the tablestorage.
        /// </summary>
        /// <typeparam name="T">The type of the entity. Must inherit from <see cref="TableEntity">TableEntity.</see></typeparam>
        /// <param name="rowKey"></param>
        /// <param name="cancellationToken">The cancellationtoken.</param>
        /// <param name="partitionKey"></param>
        /// <returns></returns>
        /// <exception cref="RequestFailedException"></exception>
        public async Task<Response> Delete<T>(string partitionKey, string rowKey, CancellationToken cancellationToken = default) where T : ITableEntity, new()
        {
            return await _tableClient.DeleteEntityAsync(partitionKey, rowKey, ETag.All, cancellationToken);
        }

        private async Task<List<T>> ConvertToList<T>(AsyncPageable<T> queryResponse, CancellationToken cancellationToken)
        {
            var responseList = new List<T>();
            await foreach (var page in queryResponse.AsPages().WithCancellation(cancellationToken))
            {
                foreach (var entity in page.Values)
                {
                    responseList.Add(entity);
                }
            }

            return responseList;
        }
    }
}
