using CsvHelper.Configuration;
using GeoAPI.Geometries;
using GravityVectorToolKit.DataModel;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using System;
using System.Globalization;

namespace GravityVectorToolKit.CSV.Mapping
{

	public class NormalPointCsvClassMap : ClassMap<NormalPoint>
	{
		public NormalPointCsvClassMap()
		{
			Map(m => m.GravityVectorId).Name("");
			Map(m => m.ClusterIndex).Name("clusterindex");
			Map(m => m.GridId).Name("gridid");
			Map(m => m.Latitude).Name("latitude");
			Map(m => m.Longitude).Name("longitude");
			Map(m => m.SpeedOverGround).Name("speedoverground");
			Map(m => m.CourseOverGround).Name("courseoverground");
			Map(m => m.Eta).Name("eta");
			Map(m => m.EtaLowerStd).Name("lower_eta_std");
			Map(m => m.EtaUpperStd).Name("upper_eta_std");
			Map(m => m.DistanceMedian).Name("dist_med");
			Map(m => m.MaxDistanceLeft).Name("maxdistanceleft");
			Map(m => m.MaxDistanceRight).Name("maxdistanceright");
			Map(m => m.DistanceStdDevLeft).Name("distancestddevleft");
			Map(m => m.DistanceStdDevRight).Name("distancestddevright");
			Map(m => m.MaxLesserSpeedDiff).Name("maxlesserspeeddiff");
			Map(m => m.MaxGreaterSpeedDiff).Name("maxgreaterspeeddiff");
			Map(m => m.LesserSpeedStdDev).Name("lesserspeedstddev");
			Map(m => m.GreaterSpeedStdDev).Name("greaterspeedstddev");
			Map(m => m.MaxLesserCourseDiff).Name("maxlessercoursediff");
			Map(m => m.MaxGreaterCourseDiff).Name("maxgreatercoursediff");
			Map(m => m.LesserCourseStdDev).Name("lessercoursestddev");
			Map(m => m.GreaterCourseStdDev).Name("greatercoursestddev");
			Map(m => m.DataCount).Name("datacount");

			Map(m => m.PositionGeometry).ConvertUsing(row =>
			{
				var p = new Point(new Coordinate(Double.Parse(row.GetField("longitude"), CultureInfo.InvariantCulture), Double.Parse(row.GetField("latitude"), CultureInfo.InvariantCulture)));
				p.SRID = 4326;
				return p;
			});
		}
	}
}