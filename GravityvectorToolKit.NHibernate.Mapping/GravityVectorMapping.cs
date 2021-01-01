using FluentNHibernate.Mapping;
using GravityVectorToolKit.DataModel;
using NetTopologySuite.Geometries;
using NHibernate.Spatial.Type;

namespace GravityVectorToolKit.CSV.Mapping
{
	public class GravityVectorMapping<T> : ClassMap<GravityVector> where T : IGeometryUserType
	{
		public GravityVectorMapping()
		{
			ImportType<Geometry>();
			Id(x => x.GravityVectorId).GeneratedBy.Identity();
			Version(x => x.LastModified);
			Map(x => x.SerialId);
			Map(x => x.ClusterIndex);
			Map(x => x.GridId);
			Map(x => x.FromHarbourId).Index("Filter_Idx");
			Map(x => x.ToHarbourId).Index("Filter_Idx");
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
			Map(x => x.TrajectoryCount).Index("Filter_Idx");
			Map(x => x.PositionGeometry).Column("positiongeometry").CustomType<T>().Not.Nullable();
			Map(x => x.StdDevLeftPosition).Column("stddevleftposition").CustomType<T>().Not.Nullable();
			Map(x => x.StdDevRightPosition).Column("stddevrightposition").CustomType<T>().Not.Nullable();
			Map(x => x.MaxDevLeftPosition).Column("maxdevleftposition").CustomType<T>().Not.Nullable();
			Map(x => x.MaxDevRightPosition).Column("maxdevrightposition").CustomType<T>().Not.Nullable();
			References(x => x.NormalRoute, nameof(NormalRoute.NormalRouteId)).Index("NormalRouteId_Idx").Cascade.None();
		}
	}
}