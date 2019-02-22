using FluentNHibernate.Mapping;
using GeoAPI.Geometries;
using GravityVectorToolKit.DataModel;
using NHibernate.Spatial.Type;

namespace GravityVectorToolKit.CSV.Mapping
{
	public class NormalRouteMapping<T> : ClassMap<NormalRoute> where T : IGeometryUserType
	{
		public NormalRouteMapping()
		{
			ImportType<IGeometry>();
			Id(x => x.NormalRouteId).GeneratedBy.Assigned();
			Version(x => x.LastModified);
			Map(x => x.FromLocationId).Index("FromLocationToLocation_Idx");
			Map(x => x.ToLocationId).Index("FromLocationToLocation_Idx");
			Map(x => x.HighError).Index("HighError_Idx");
			Map(x => x.VoyageCount).Index("VoyageCount_Idx");
			Map(x => x.NormalRouteGeometry).Column("normalroutegeometry").CustomType<T>();
			HasMany(x => x.GravityVectors).KeyColumn(nameof(NormalRoute.NormalRouteId)).Inverse().Cascade.All();
		}
	}
}