using FluentNHibernate.Mapping;
using GravityVectorToolKit.DataModel;
using NetTopologySuite.Geometries;
using NHibernate.Spatial.Type;

namespace GravityVectorToolKit.CSV.Mapping
{

	public class NearMissIncidentMapping<T> : ClassMap<NearMissIncident> where T : IGeometryUserType
	{
		public NearMissIncidentMapping()
		{
			Id(x => x.NearMissId).GeneratedBy.Identity();
			Map(x => x.IncidentId).Index("Idx_IncidentId"); ;
			Map(x => x.Imo).Index("Idx_Imo"); ;
			Map(x => x.Mmsi).Index("Idx_Mmsi"); ;
			Map(x => x.Timestamp).Index("Idx_Timestamp"); ;
			Map(x => x.Lon);
			Map(x => x.Lat);
			Map(x => x.Cog);
			Map(x => x.Sog);
			Map(x => x.Rot);
			Map(x => x.Acceleration);
			Map(x => x.TimeToImpact).Index("Idx_TimeToImpact"); ;
			Map(x => x.PointOfImpactLon);
			Map(x => x.PointOfImpactLat);
			Map(x => x.DistanceToImpact).Index("Idx_DistanceToImpact"); ;
			Map(x => x.TypeOfImpact);
			Map(x => x.SectorPolygon).Column("sectorpolygon").CustomType<T>();
			Map(x => x.MainlandCategory);
			Map(x => x.Draught);
			Map(x => x.Shiptype).Index("Idx_ShipType"); ;
			Map(x => x.LengthGroup);
			Map(x => x.Tobow);
			Map(x => x.TimeOfImpact).Index("Idx_TimeOfImpact"); ;
			Map(x => x.Geohash).Index("Idx_GeoHash"); ;
			Map(x => x.RelDistStd).Index("Idx_RelDistStd");
			Map(x => x.RelSogStd);
		}
	}
}