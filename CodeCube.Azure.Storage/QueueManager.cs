using System;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using CodeCube.Azure.Constants;

namespace CodeCube.Azure
{
    /// <summary>
    /// Manager class to connect to one single Azure Storage Queue
    /// </summary>
    public sealed class QueueManager : BaseManager
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
        public async Task<QueueClient> ConnectAsync()
        {
            if(_queueClient == null) await ConnectToQueue();

            return _queueClient;
        }

        #region privates
        private async Task ConnectToQueue()
        {
            _queueClient = new QueueClient(Connectionstring, _queuename);

            await _queueClient.CreateIfNotExistsAsync();
        }
        #endregion
    }
}
