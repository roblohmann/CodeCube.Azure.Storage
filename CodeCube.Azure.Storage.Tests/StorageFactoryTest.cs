using System;
using NUnit.Framework;

namespace CodeCube.Azure.Tests
{
    [TestFixture]
    public class StorageFactoryTest
    {
        [Test]
        public void GetStorageManager_Blob()
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
        public void GetStorageManager_Blob_WithoutAccountAndAccesskey_ThrowsException()
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
        public void GetStorageManager_Blob_WithoutAccesskey_ThrowsException()
        {
            //Setup
            var factory = new StorageFactory();

            //Act + Assert
            var exception = Assert.Throws<ArgumentNullException>(() => factory.GetBlobStorageManager(string.Empty, string.Empty, string.Empty));
            Assert.AreEqual("URI for BLOB-storage cannot be empty! (Parameter 'uri')", exception.GetBaseException().Message);
        }

        [Test]
        public void GetStoragemanager_Blob_WithoutConnectionstring_ThrowsException()
        {
            //Setup
            var factory = new StorageFactory();

            //Act + Assert
            var exception = Assert.Throws<ArgumentNullException>(() => factory.GetBlobStorageManager(string.Empty));
            Assert.AreEqual("A valid connectionstring for the BLOB-storage is required! (Parameter 'connectionstring')", exception.GetBaseException().Message);
        }
    }
}
