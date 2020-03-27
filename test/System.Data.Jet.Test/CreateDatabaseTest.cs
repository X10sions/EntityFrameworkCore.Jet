﻿using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Data.Jet.Test
{
    [TestClass]
    public class CreateDatabaseTest
    {
        private const string StoreName = nameof(CreateDatabaseTest) + ".accdb";
        
        [TestInitialize]
        public void Setup()
        {
            Helpers.DeleteDatabase(StoreName);
        }

        [TestCleanup]
        public void TearDown()
        {
            Helpers.DeleteDatabase(StoreName);
        }

        [TestMethod]
        public void CreateAndDropDatabaseFromConnection()
        {
            using var connection = new JetConnection(JetConnection.GetConnectionString(StoreName));
            connection.CreateEmptyDatabase();

            Assert.IsTrue(File.Exists(StoreName));
            
            using var command = connection.CreateCommand();
            command.CommandText = "DROP DATABASE " + StoreName;
            command.ExecuteNonQuery();
            
            Assert.IsFalse(File.Exists(StoreName));
        }

        [TestMethod]
        public void CreateAndDropDatabaseWithUnsetConnection()
        {
            using var connection = new JetConnection();
            
            var command = connection.CreateCommand();
            command.CommandText = "CREATE DATABASE " + StoreName;
            command.ExecuteNonQuery();
            
            Assert.IsTrue(File.Exists(StoreName));
            
            command.CommandText = "DROP DATABASE " + StoreName;
            command.ExecuteNonQuery();
            
            Assert.IsFalse(File.Exists(StoreName));
        }
    }
}
