using GravityVectorToolKit.Common;
using GravityVectorToolKit.CSV.Mapping;
using GravityVectorToolKit.DataModel;
using NUnit.Framework;

namespace GravityVectorToolkit.Test.UnitTests
{

	public class NearMissTests
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public void TestNearMissMapParsing01()
		{
			var file = "Resources/near_miss_sample.csv";
			var records = Util.ReadCsvFile<NearMissIncident, NearMissIncidentCsvClassMap>(file);
			Assert.IsTrue(records.Count > 0);
		}

		[Test]
		public void TestNearMissMapParsing02()
		{
			var file = "Resources/near_miss_sample.csv";
			int i = 0;
			foreach (var record in Util.ReadCsvFileByRow<NearMissIncident, NearMissIncidentCsvClassMap>(file))
			{
				i++;
			}
			Assert.IsTrue(i > 0);
		}
	}
}