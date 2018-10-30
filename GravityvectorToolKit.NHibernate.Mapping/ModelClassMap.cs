using FluentNHibernate.Mapping;
using GeoAPI.Geometries;
using GravityVectorToKML.Model;
using NHibernate.Spatial.Type;

namespace GravityVectorToKML.CSV.Mapping
{
	public class NormalPointGMapping<T> : ClassMap<NormalPointG> where T : IGeometryUserType
	{
		public NormalPointGMapping()
		{
			ImportType<IGeometry>();

			//Map(m => m.ClusterIndex).Name("clusterindex");
			//Map(m => m.GridId).Name("grid_id");
			//Map(m => m.Latitude).Name("x");
			//Map(m => m.Longitude).Name("y");
			//Map(m => m.SpeedOverGround).Name("sog");
			//Map(m => m.CourseOverGround).Name("cog");
			//Map(m => m.DistanceMedian).Name("dist_med");
			//Map(m => m.MaxDistanceLeft).Name("max_dist_left");
			//Map(m => m.MaxDistanceRight).Name("max_dist_right");
			//Map(m => m.DistanceStdDevLeft).Name("dist_std_left");
			//Map(m => m.DistanceStdDevRight).Name("dist_std_right");
			//Map(m => m.MaxLesserSpeedDiff).Name("max_lesser_sog_diff");
			//Map(m => m.MaxGreaterSpeedDiff).Name("max_greater_sog_diff");
			//Map(m => m.LesserSpeedStdDev).Name("lesser_sog_std");
			//Map(m => m.GreaterSpeedStdDev).Name("greater_sog_std");
			//Map(m => m.MaxLesserCourseDiff).Name("max_lesser_cog_diff");
			//Map(m => m.MaxGreaterCourseDiff).Name("max_greater_cog_diff");
			//Map(m => m.LesserCourseStdDev).Name("lesser_cog_std");
			//Map(m => m.GreaterCourseStdDev).Name("greater_cog_std");
			//Map(m => m.DataCount).Name("data_count");


			Id(x => x.Id).GeneratedBy.Identity();
			Map(x => x.FromLocationId).Index("FromLocationToLocation_Idx");
			Map(x => x.ToLocationId).Index("FromLocationToLocation_Idx");
			Map(x => x.ClusterIndex);
			Map(x => x.GridId);
			Map(x => x.Latitude);
			Map(x => x.Longitude);
			Map(x => x.SpeedOverGround);
			Map(x => x.CourseOverGround);
            Map(x => x.Eta);
            Map(x => x.EtaLowerStd);
            Map(x => x.EtaUpperStd);
            Map(x => x.DistanceMedian);
			Map(x => x.MaxDistanceLeft);
			Map(x => x.MaxDistanceRight);
			Map(x => x.DistanceStdDevLeft);
			Map(x => x.DistanceStdDevRight);
			Map(x => x.MaxLesserSpeedDiff);
			Map(x => x.MaxGreaterSpeedDiff);
			Map(x => x.LesserSpeedStdDev);
			Map(x => x.GreaterSpeedStdDev);
			Map(x => x.MaxLesserCourseDiff);
			Map(x => x.MaxGreaterCourseDiff);
			Map(x => x.LesserCourseStdDev);
			Map(x => x.GreaterCourseStdDev);
			Map(x => x.DataCount);

			Map(x => x.PositionGeometry).Column("positiongeometry").CustomType<T>().Not.Nullable();
			

		}
	}
}
