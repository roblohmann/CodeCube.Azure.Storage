﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using CodeCube.Azure.Storage.Constants;
using CodeCube.Azure.Storage.Interfaces;

namespace CodeCube.Azure.Storage
{
    /// <summary>
    /// Manager class to connect to one single Azure Storage Queue
    /// </summary>
    public sealed class QueueManager : BaseManager, IQueueManager
    {
        private readonly string _queuename;
        private QueueClient? _queueClient;

        internal QueueManager(string connectionstring, string queuename) : base(connectionstring)
        {
            //Validate parameters
            if (string.IsNullOrEmpty(queuename))
            {
                throw new ArgumentNullException(nameof(queuename), ErrorConstants.Queue.QueueNameRequired);
            }

            if (string.IsNullOrEmpty(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring), ErrorConstants.Queue.QueueConnectionstringRequired);
            }

            _queuename = queuename;
        }

        /// <summary>
        /// Connect to the desired queue and return the client.
        /// </summary>
        /// <returns>The queueclient.</returns>
        public async Task<QueueClient> ConnectAsync(CancellationToken cancellationToken)
        {
            return _queueClient ??= await ConnectToQueue(cancellationToken);
        }

        #region privates
        private async Task<QueueClient> ConnectToQueue(CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(Connectionstring))
            {
                throw new InvalidOperationException(ErrorConstants.Queue.QueueConnectionstringRequired);
            }

            var queueClient = new QueueClient(Connectionstring, _queuename);

            var queueExists = await queueClient.ExistsAsync(cancellationToken);
            if (!queueExists)
            {
                await queueClient.CreateAsync(cancellationToken: cancellationToken);
            }

            return queueClient;
        }
        #endregion
    }
}
