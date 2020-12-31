using CsvHelper.Configuration;
using GravityVectorToolKit.DataModel;
using NetTopologySuite.Geometries;
using System;
using System.Globalization;

namespace GravityVectorToolKit.CSV.Mapping
{
	public class GravityVectorCsvClassMap : ClassMap<GravityVector>
	{
		public GravityVectorCsvClassMap()
		{
			Map(m => m.SerialId).Name("");
			Map(m => m.NormalRouteId).Name("normal_route_id");
			Map(m => m.FromHarbourId).Name("from");
			Map(m => m.ToHarbourId).Name("to");
			Map(m => m.ClusterIndex).Name("clusterindex");
			Map(m => m.GridId).Name("gridid");
			Map(m => m.Latitude).Name("latitude");
			Map(m => m.Longitude).Name("longitude");
			Map(m => m.SpeedOverGround).Name("speedoverground");
			Map(m => m.CourseOverGround).Name("courseoverground");
			Map(m => m.Eta).Name("eta");
			Map(m => m.EtaLowerStd).Name("lower_eta_std");
			Map(m => m.EtaUpperStd).Name("upper_eta_std");

			#region Position statistics

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



			#endregion Position statistics

			#region Vessel detail statistics

			Map(m => m.MaxLesserShipGrossTonnageDiff).Name("maxlessershipgrosstonnagediff");
			Map(m => m.MaxGreaterShipGrossTonnageDiff).Name("maxgreatershipgrosstonnagediff");
			Map(m => m.LesserShipGrossTonnageStdDev).Name("lessershipgrosstonnagestddev");
			Map(m => m.GreaterShipGrossTonnageStdDev).Name("greatershipgrosstonnagestddev");
			Map(m => m.MaxLesserShipLengthDiff).Name("maxlessershiplengthdiff");
			Map(m => m.MaxGreaterShipLengthDiff).Name("maxgreatershiplengthdiff");
			Map(m => m.LesserShipLengthStdDev).Name("lessershiplengthstddev");
			Map(m => m.GreaterShipLengthStdDev).Name("greatershiplengthstddev");
			Map(m => m.MaxLesserShipBreadthDiff).Name("maxlessershipbreadthdiff");
			Map(m => m.MaxGreaterShipBreadthDiff).Name("maxgreatershipbreadthdiff");
			Map(m => m.LesserShipBreadthStdDev).Name("lessershipbreadthstddev");
			Map(m => m.GreaterShipBreadthStdDev).Name("greatershipbreadthstddev");

			#endregion Vessel detail statistics

			#region Wave statistics

			Map(m => m.MaxLesserShipWaveHeightDiff).Name("maxlessershipwaveheightdiff");
			Map(m => m.MaxGreaterShipWaveHeightDiff).Name("maxgreatershipwaveheightdiff");
			Map(m => m.LesserShipWaveHeightStdDev).Name("lessershipwaveheightstddev");
			Map(m => m.GreaterShipWaveHeightStdDev).Name("greatershipwaveheightstddev");
			Map(m => m.MaxLesserShipWaveDirectionDiff).Name("maxlessershipwavedirectiondiff");
			Map(m => m.MaxGreaterShipWaveDirectionDiff).Name("maxgreatershipwavedirectiondiff");
			Map(m => m.LesserShipWaveDirectionStdDev).Name("lessershipwavedirectionstddev");
			Map(m => m.GreaterShipWaveDirectionStdDev).Name("greatershipwavedirectionstddev");

			#endregion Wave statistics

			Map(m => m.DataCount).Name("datacount");
			Map(m => m.TrajectoryCount).Name("trajectorycount");
			Map(m => m.PositionGeometry).ConvertUsing(row =>
			{
				var p = new Point(
							new Coordinate(
								Double.Parse(row.GetField("longitude"), CultureInfo.InvariantCulture),
								Double.Parse(row.GetField("latitude"), CultureInfo.InvariantCulture)));
				p.SRID = 4326;
				return p;
			});

			Map(m => m.StdDevLeftPosition).ConvertUsing(row =>
			{
				var p = new Point(
							new Coordinate(
								Double.Parse(row.GetField("stdcoordleftlon"), CultureInfo.InvariantCulture),
								Double.Parse(row.GetField("stdcoordleftlat"), CultureInfo.InvariantCulture)));
				p.SRID = 4326;
				return p;
			});

			Map(m => m.StdDevRightPosition).ConvertUsing(row =>
			{
				var p = new Point(
							new Coordinate(
								Double.Parse(row.GetField("stdcoordrightlon"), CultureInfo.InvariantCulture),
								Double.Parse(row.GetField("stdcoordrightlat"), CultureInfo.InvariantCulture)));
				p.SRID = 4326;
				return p;
			});

			Map(m => m.MaxDevLeftPosition).ConvertUsing(row =>
			{
				var p = new Point(
							new Coordinate(
								Double.Parse(row.GetField("maxcoordleftlon"), CultureInfo.InvariantCulture),
								Double.Parse(row.GetField("maxcoordleftlat"), CultureInfo.InvariantCulture)));
				p.SRID = 4326;
				return p;
			});

			Map(m => m.MaxDevRightPosition).ConvertUsing(row =>
			{
				var p = new Point(
							new Coordinate(
								Double.Parse(row.GetField("maxcoordrightlon"), CultureInfo.InvariantCulture),
								Double.Parse(row.GetField("maxcoordrightlat"), CultureInfo.InvariantCulture)));
				p.SRID = 4326;
				return p;
			});


		}
	}
}