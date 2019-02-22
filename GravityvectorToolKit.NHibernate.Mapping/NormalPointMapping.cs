using FluentNHibernate.Mapping;
using GeoAPI.Geometries;
using GravityVectorToolKit.DataModel;
using NHibernate.Spatial.Type;

namespace GravityVectorToolKit.CSV.Mapping
{
	public class GravityVectorMapping<T> : ClassMap<GravityVector> where T : IGeometryUserType
	{
		public GravityVectorMapping()
		{
			ImportType<IGeometry>();
			Id(x => x.NormalPointId).GeneratedBy.Identity();
			Map(x => x.GravityVectorId);
			Map(x => x.ClusterIndex);
			Map(x => x.GridId);
			Map(x => x.Latitude).Index("Filter_Idx");
			Map(x => x.Longitude).Index("Filter_Idx");
			Map(x => x.SpeedOverGround).Index("Filter_Idx");
			Map(x => x.CourseOverGround).Index("Filter_Idx");
			Map(x => x.Eta).Index("Filter_Idx");
			Map(x => x.EtaLowerStd).Index("Filter_Idx");
			Map(x => x.EtaUpperStd).Index("Filter_Idx");
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
			Map(x => x.DataCount).Index("Filter_Idx");
			Map(x => x.PositionGeometry).Column("positiongeometry").CustomType<T>().Not.Nullable();
			//References(x => x.NormalRoute, nameof(NormalRoute.NormalRouteId)).Index("NormalRouteId_Idx").Cascade.None();
		}
	}
}