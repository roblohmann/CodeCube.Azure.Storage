using System;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace CodeCube.Azure.Storage.Tests.Factory;

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
        ClassicAssert.IsNotNull(manager);
    }

    [Test]
    public void Get_WithoutConnectionstring_ThrowsException()
    {
        //Setup            
        const string mockQueueName = "MockQueueName";
        var factory = new StorageFactory();

        //Act + Assert
        var exception = Assert.Throws<ArgumentNullException>(() => factory.GetQueueManager(null, mockQueueName));
        ClassicAssert.AreEqual("Name for the queue is required! (Parameter 'connectionstring')", exception.GetBaseException().Message);
    }


    [Test]
    public void Get_WithoutQueueName_ThrowsException()
    {
        //Setup            
        const string mockConnectionstring = "MockConnectionstring";
        var factory = new StorageFactory();

        //Act + Assert
        var exception = Assert.Throws<ArgumentNullException>(() => factory.GetQueueManager(mockConnectionstring, null));
        ClassicAssert.AreEqual("Connectionstring for the queue is required! (Parameter 'queueName')", exception.GetBaseException().Message);
    }
}