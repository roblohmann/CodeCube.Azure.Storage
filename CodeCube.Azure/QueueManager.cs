using System;
using CodeCube.Azure.Constants;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CodeCube.Azure
{
    public sealed class QueueManager : BaseManager
    {
        private readonly string _queuename;
        private CloudQueue _queue;

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

        public CloudQueue Connect()
        {
            if(_queue == null) ConnectToQueue();
            
            return _queue;
        }

        #region privates
        private void ConnectToQueue()
        {
            CloudStorageAccount storageAccount = ConnectCloudStorageAccountWithConnectionString();

            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue.
            _queue = queueClient.GetQueueReference(_queuename);

            // Create the queue if it doesn't already exist
            _queue.CreateIfNotExistsAsync();
        }
        #endregion
    }
}
