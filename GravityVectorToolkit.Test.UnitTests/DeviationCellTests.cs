using GravityVectorToolKit.Common;
using GravityVectorToolKit.CSV.Mapping;
using GravityVectorToolKit.DataModel;
using NUnit.Framework;

namespace GravityVectorToolkit.Test.UnitTests
{
	public class DeviationCellTests
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public void TestDeviationMapParsing01()
		{
			var file = "Resources/deviation_map_sample.csv";
			var records = Util.ReadCsvFile<DeviationCell, DeviationCellCsvClassMap>(file);
			Assert.IsTrue(records.Count > 0);
		}

		[Test]
		public void TestDeviationMapParsing02()
		{
			var file = "Resources/deviation_map_sample.csv";
			int i = 0;
			foreach (var record in Util.ReadCsvFileByRow<DeviationCell, DeviationCellCsvClassMap>(file))
			{
				i++;
			}

			Assert.IsTrue(i > 0);
		}
	}
}