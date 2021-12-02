using System;
using NUnit.Framework;

namespace CodeCube.Azure.Storage.Tests.Factory
{
    [TestFixture]
    public class QueueManager
    {
        [Test]
        public void Get()
        {
            //Setup            
            const string mockConnectionstring = "MockConnectionstring";
            const string mockQueueName = "MockQueueName";
            var factory = new StorageFactory();

            //Act
            var manager = factory.GetQueueManager(mockConnectionstring, mockQueueName);

            //Assert
            Assert.IsNotNull(manager);
        }

        [Test]
        public void Get_WithoutConnectionstring_ThrowsException()
        {
            //Setup            
            const string mockQueueName = "MockQueueName";
            var factory = new StorageFactory();

            //Act + Assert
            var exception = Assert.Throws<ArgumentNullException>(() => factory.GetQueueManager(null, mockQueueName));
            Assert.AreEqual("Name for the queue is required! (Parameter 'connectionstring')", exception.GetBaseException().Message);
        }


        [Test]
        public void Get_WithoutQueueName_ThrowsException()
        {
            //Setup            
            const string mockConnectionstring = "MockConnectionstring";
            var factory = new StorageFactory();

            //Act + Assert
            var exception = Assert.Throws<ArgumentNullException>(() => factory.GetQueueManager(mockConnectionstring, null));
            Assert.AreEqual("Connectionstring for the queue is required! (Parameter 'queueName')", exception.GetBaseException().Message);
        }
    }

    [TestFixture]
    public class TableStorageManager
    {
        [Test]
        public void Get()
        {
            //Setup            
            const string mockConnectionstring = "MockConnectionstring";
            const string mockTableName = "MockTable";
            var factory = new StorageFactory();

            //Act
            var manager = factory.GetTableStorageManager(mockConnectionstring, mockTableName);

            //Assert
            Assert.IsNotNull(manager);
        }

        [Test]
        public void Get_WithoutConnectionstring_ThrowsException()
        {
            //Setup            
            const string mockTableName = "MockTableName";
            var factory = new StorageFactory();

            //Act + Assert
            var exception = Assert.Throws<ArgumentNullException>(() => factory.GetTableStorageManager(null, mockTableName));
            Assert.AreEqual("Connectionstring for tablestorage cannot be empty! (Parameter 'tableConnectionstring')", exception.GetBaseException().Message);
        }


        [Test]
        public void Get_WithoutTableName_ThrowsException()
        {
            //Setup            
            const string mockConnectionstring = "MockConnectionstring";
            var factory = new StorageFactory();

            //Act + Assert
            var exception = Assert.Throws<ArgumentNullException>(() => factory.GetTableStorageManager(mockConnectionstring, null));
            Assert.AreEqual("Tablename for tablestorage cannot be empty! (Parameter 'tableName')", exception.GetBaseException().Message);
        }
    }

    [TestFixture]
    public class BlobStorageManager
    {
        [Test]
        public void Get()
        {
            //Setup
            const string mockUri = "https://mock-uri.com";
            const string mockBlobStorageAccount = "MockAccount";
            const string mockBlobStoragePassword = "MockBlobPassword";
            var factory = new StorageFactory();

            //Act
            var manager = factory.GetBlobStorageManager(mockUri, mockBlobStorageAccount, mockBlobStoragePassword);

            //Assert
            Assert.IsNotNull(manager);
        }

        [Test]
        public void Get_WithoutAccountAndAccesskey_ThrowsException()
        {
            //Setup
            const string mockUri = "MockUri";
            const string mockBlobStorageAccount = "MockAccount";
            string mockBlobStoragePassword = string.Empty;
            var factory = new StorageFactory();

            //Act + Assert
            var exception = Assert.Throws<ArgumentNullException>(() => factory.GetBlobStorageManager(mockUri, mockBlobStorageAccount, mockBlobStoragePassword));
            Assert.AreEqual("Accesskey for BLOB-storage cannot be empty! (Parameter 'blobAccesskey')", exception.GetBaseException().Message);
        }

        [Test]
        public void Get_WithoutAccesskey_ThrowsException()
        {
            //Setup
            var factory = new StorageFactory();

            //Act + Assert
            var exception = Assert.Throws<ArgumentNullException>(() => factory.GetBlobStorageManager(string.Empty, string.Empty, string.Empty));
            Assert.AreEqual("URI for BLOB-storage cannot be empty! (Parameter 'uri')", exception.GetBaseException().Message);
        }

        [Test]
        public void Get_WithoutConnectionstring_ThrowsException()
        {
            //Setup
            var factory = new StorageFactory();

            //Act + Assert
            var exception = Assert.Throws<ArgumentNullException>(() => factory.GetBlobStorageManager(string.Empty));
            Assert.AreEqual("A valid connectionstring for the BLOB-storage is required! (Parameter 'connectionstring')", exception.GetBaseException().Message);
        }
    }
}
