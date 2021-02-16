using GravityVectorToolKit.Common;
using GravityVectorToolKit.CSV.Mapping;
using GravityVectorToolKit.DataModel;
using NUnit.Framework;

namespace GravityVectorToolkit.Test.UnitTests
{

	public class GeohashTests
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public void GeohashTest01()
		{
			var hash = "u5qf";
			var polygon = new GvtkGeohasher().BoundingBox(hash);
			var wkt = polygon.AsText();
		}

		[Test]
		public void GeohashTest02()
		{
			var hash = "u4ur9ec";
			var hasher = new GvtkGeohasher();
			var point = hasher.Decode(hash);
			Assert.AreEqual(hash, hasher.Encode(point, 7));
		}
	}
}