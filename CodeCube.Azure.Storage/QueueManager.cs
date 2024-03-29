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
        private QueueClient _queueClient;

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
            if (_queueClient == null) await ConnectToQueue(cancellationToken);

            return _queueClient;
        }

        #region privates
        private async Task ConnectToQueue(CancellationToken cancellationToken)
        {
            _queueClient = new QueueClient(Connectionstring, _queuename);

            var queueExists = await _queueClient.ExistsAsync(cancellationToken);
            if (!queueExists)
            {
                await _queueClient.CreateAsync(cancellationToken: cancellationToken);
            }
        }
        #endregion
    }
}
