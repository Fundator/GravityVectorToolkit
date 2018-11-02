using CsvHelper.Configuration;
using GeoAPI.Geometries;
using GravityVectorToKML.Model;
using NetTopologySuite.Geometries;
using System;
using System.Globalization;

namespace GravityVectorToKML.CSV.Mapping
{
    //public class ModelClassMap : ClassMap<NormalPoint>
    //{
    //    public ModelClassMap()
    //    {
    //        Map(m => m.ClusterIndex).Name("clusterindex");
    //        Map(m => m.GridId).Name("grid_id");
    //        Map(m => m.Latitude).Name("x");
    //        Map(m => m.Longitude).Name("y");
    //        Map(m => m.SpeedOverGround).Name("sog");
    //        Map(m => m.CourseOverGround).Name("cog");
    //        Map(m => m.DistanceMedian).Name("dist_med");
    //        Map(m => m.MaxDistanceLeft).Name("max_dist_left");
    //        Map(m => m.MaxDistanceRight).Name("max_dist_right");
    //        Map(m => m.DistanceStdDevLeft).Name("dist_std_left");
    //        Map(m => m.DistanceStdDevRight).Name("dist_std_right");
    //        Map(m => m.MaxLesserSpeedDiff).Name("max_lesser_sog_diff");
    //        Map(m => m.MaxGreaterSpeedDiff).Name("max_greater_sog_diff");
    //        Map(m => m.LesserSpeedStdDev).Name("lesser_sog_std");
    //        Map(m => m.GreaterSpeedStdDev).Name("greater_sog_std");
    //        Map(m => m.MaxLesserCourseDiff).Name("max_lesser_cog_diff");
    //        Map(m => m.MaxGreaterCourseDiff).Name("max_greater_cog_diff");
    //        Map(m => m.LesserCourseStdDev).Name("lesser_cog_std");
    //        Map(m => m.GreaterCourseStdDev).Name("greater_cog_std");
    //        Map(m => m.DataCount).Name("data_count");
    //    }
    //}

	public class ModelClassMapG : ClassMap<NormalPointG>
	{
		public ModelClassMapG()
		{
            Map(m => m.GravityVectorId).Name("id");
			Map(m => m.ClusterIndex).Name("clusterindex");
			Map(m => m.GridId).Name("grid_id");
			Map(m => m.Latitude).Name("x");
			Map(m => m.Longitude).Name("y");
			Map(m => m.SpeedOverGround).Name("sog");
            Map(m => m.CourseOverGround).Name("cog");
            Map(m => m.Eta).Name("eta");
            Map(m => m.EtaLowerStd).Name("lower_eta_std");
            Map(m => m.EtaUpperStd).Name("upper_eta_std");
            Map(m => m.DistanceMedian).Name("dist_med");
			Map(m => m.MaxDistanceLeft).Name("max_dist_left");
			Map(m => m.MaxDistanceRight).Name("max_dist_right");
			Map(m => m.DistanceStdDevLeft).Name("dist_std_left");
			Map(m => m.DistanceStdDevRight).Name("dist_std_right");
			Map(m => m.MaxLesserSpeedDiff).Name("max_lesser_sog_diff");
			Map(m => m.MaxGreaterSpeedDiff).Name("max_greater_sog_diff");
			Map(m => m.LesserSpeedStdDev).Name("lesser_sog_std");
			Map(m => m.GreaterSpeedStdDev).Name("greater_sog_std");
			Map(m => m.MaxLesserCourseDiff).Name("max_lesser_cog_diff");
			Map(m => m.MaxGreaterCourseDiff).Name("max_greater_cog_diff");
			Map(m => m.LesserCourseStdDev).Name("lesser_cog_std");
			Map(m => m.GreaterCourseStdDev).Name("greater_cog_std");
			Map(m => m.DataCount).Name("data_count");
			Map(m => m.PositionGeometry).ConvertUsing(row => {
				var p = new Point(new Coordinate(Double.Parse(row.GetField("y"), CultureInfo.InvariantCulture), Double.Parse(row.GetField("x"), CultureInfo.InvariantCulture)));
				p.SRID = 4326;
				return p;
			});
		}
	}
}
