namespace CodeCube.Azure.Storage.Constants
{
    internal static class ErrorConstants
    {
        internal const string InvalidConnectionstring = "An invalid connectionstring was provided!";

        internal static class Blob
        {
            internal const string BlobUriRequired = "URI for BLOB-storage cannot be empty!";
            internal const string BlobAccountRequired = "Accountname for BLOB-storage cannot be empty!";
            internal const string BlobAccesskeyRequired = "Accesskey for BLOB-storage cannot be empty!";
            internal const string BlobConnectionstringRequired = "A valid connectionstring for the BLOB-storage is required!";
            internal const string FileCouldNotBeStored = "File could not be stored in the cloud!";
        }

        internal static class Table
        {
            internal const string TableConnectionstringRequired = "Connectionstring for tablestorage cannot be empty!";
            internal const string TableNameRequired = "Tablename for tablestorage cannot be empty!";

            internal const string RowKeyIsRequired = "RowKey is required!";
            internal const string PartitionKeyIsRequired = "PartitionKey is required!";

            internal const string MaxLengthTableNameExceeded = "The max length for the table name is {0} characters!";
            internal const string TableNameNotAllowed = "The table name provided is not valid! Only alphanumeric characters are allowed!";

            internal const string NotConnected = "No connection has been made! Please call 'Connect()' method before executing operations on the table!";
        }

        internal static class Queue
        {
            internal const string QueueNameRequired = "Name for the queue is required!";
            internal const string QueueConnectionstringRequired = "Connectionstring for the queue is required!";
        }
    }
}
