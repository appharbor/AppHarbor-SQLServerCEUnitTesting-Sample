using System.Data.SqlServerCe;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AppHarbor.SQLServerCEUnitTesting.Test
{
	[TestClass]
	public class Tests
	{
		private static string _databaseFilename = "database.sdf";
		private static string _connectionString = string.Format("DataSource=\"{0}\";", _databaseFilename);

		[ClassInitialize]
		public static void ClassInitialize(TestContext testContext)
		{
			var sqlCeEngine = new SqlCeEngine(_connectionString);
			sqlCeEngine.CreateDatabase();
		}

		[TestInitialize]
		public void TestInitialize()
		{
			var createTableSQL = "create table Test(TestColumn nvarchar(10))";
			ExecuteCommand(createTableSQL);
		}

		[ClassCleanup]
		public static void ClassCleanup()
		{
			File.Delete(_databaseFilename);
		}

		[TestCleanup]
		public void TestCleanup()
		{
			var dropTableSQL = "drop table test";
			ExecuteCommand(dropTableSQL);
		}

		[TestMethod]
		public void TestSqlCe()
		{
			var insertSql = "insert into Test (TestColumn) values ('foo')";
			ExecuteCommand(insertSql);

			var query = "select * from Test";
			int count = 0;

			using (var connection = new SqlCeConnection(_connectionString))
			{
				connection.Open();
				var command = new SqlCeCommand(query, connection);
				var reader = command.ExecuteReader();
				
				while (reader.Read())
				{
					count++;
				}
				reader.Close();
			}
			Assert.AreEqual<int>(1, count);
		}

		private static void ExecuteCommand(string sql)
		{
			using (var connection = new SqlCeConnection(_connectionString))
			{
				connection.Open();
				var command = new SqlCeCommand(sql, connection);
				command.ExecuteNonQuery();
			}
		}
	}
}
