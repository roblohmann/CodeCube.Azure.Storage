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
            const string mockBlobStorageAccount = "MockAccount";
            const string mockBlobStoragePassword = "MockBlobPassword";
            var factory = new StorageFactory();

            //Act
            var manager = factory.GetBlobStorageManager(mockBlobStorageAccount,mockBlobStoragePassword);

            //Assert
            Assert.IsNotNull(manager);
        }

        [Test]
        public void GetStorageManager_Blob_WithoutAccountAndPassword_ThrowsException()
        {
            //Setup
            var factory = new StorageFactory();

            //Act + Assert
            var exception = Assert.Throws<ArgumentNullException>(() => factory.GetBlobStorageManager(string.Empty, string.Empty));
            Assert.AreEqual("Accountname for BLOB-storage cannot be empty!\r\nParameter name: blobStorageAccountname", exception.GetBaseException().Message);
        }
    }
}
