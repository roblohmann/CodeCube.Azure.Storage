using System;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace CodeCube.Azure.Storage.Tests.Factory;

[TestFixture]
public class TableStorageManager
{
    //[Test]
    //public void Get()
    //{
    //    //Setup            
    //    const string mockConnectionstring = "MockConnectionstring";
    //    const string mockTableName = "MockTable";
    //    var factory = new StorageFactory();

    //    //Act
    //    var manager = factory.GetTableStorageManager(mockConnectionstring, mockTableName);

    //    //Assert
    //    Assert.IsNotNull(manager);
    //}

    [Test]
    public void Get_With_Invalid_Connectionstring_Throws_Exception()
    {
        //Setup            
        const string mockTableName = "MockTableName";
        var factory = new StorageFactory();

        //Act + Assert
        var exception = Assert.Throws<InvalidOperationException>(() => factory.GetTableStorageManager("MockConnectionString", mockTableName));
        ClassicAssert.AreEqual("Connection string doesn't have value for keyword 'MockConnectionString'.", exception.GetBaseException().Message);
    }

    [Test]
    public void Get_WithoutConnectionstring_ThrowsException()
    {
        //Setup            
        const string mockTableName = "MockTableName";
        var factory = new StorageFactory();

        //Act + Assert
        var exception = Assert.Throws<ArgumentNullException>(() => factory.GetTableStorageManager(null, mockTableName));
        ClassicAssert.AreEqual("Connectionstring for tablestorage cannot be empty! (Parameter 'tableConnectionstring')", exception.GetBaseException().Message);
    }


    [Test]
    public void Get_WithoutTableName_ThrowsException()
    {
        //Setup            
        const string mockConnectionstring = "MockConnectionstring";
        var factory = new StorageFactory();

        //Act + Assert
        var exception = Assert.Throws<ArgumentNullException>(() => factory.GetTableStorageManager(mockConnectionstring, null));
        ClassicAssert.AreEqual("Tablename for tablestorage cannot be empty! (Parameter 'tableName')", exception.GetBaseException().Message);
    }
}