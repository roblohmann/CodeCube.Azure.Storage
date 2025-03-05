using System;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace CodeCube.Azure.Storage.Tests.Factory
{
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
            ClassicAssert.IsNotNull(manager);
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
            ClassicAssert.AreEqual("Accesskey for BLOB-storage cannot be empty! (Parameter 'blobAccesskey')", exception.GetBaseException().Message);
        }

        [Test]
        public void Get_WithoutAccesskey_ThrowsException()
        {
            //Setup
            var factory = new StorageFactory();

            //Act + Assert
            var exception = Assert.Throws<ArgumentNullException>(() => factory.GetBlobStorageManager(string.Empty, string.Empty, string.Empty));
            ClassicAssert.AreEqual("URI for BLOB-storage cannot be empty! (Parameter 'uri')", exception.GetBaseException().Message);
        }

        [Test]
        public void Get_WithoutConnectionstring_ThrowsException()
        {
            //Setup
            var factory = new StorageFactory();

            //Act + Assert
            var exception = Assert.Throws<ArgumentNullException>(() => factory.GetBlobStorageManager(string.Empty));
            ClassicAssert.AreEqual("A valid connectionstring for the BLOB-storage is required! (Parameter 'connectionstring')", exception.GetBaseException().Message);
        }
    }
}
