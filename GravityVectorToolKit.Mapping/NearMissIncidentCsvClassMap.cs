using CsvHelper.Configuration;
using GravityVectorToolKit.DataModel;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;

namespace GravityVectorToolKit.CSV.Mapping
{
	public class NearMissIncidentCsvClassMap : ClassMap<NearMissIncident>
	{
		public NearMissIncidentCsvClassMap()
		{
			Map(m => m.IncidentId).Name("id");
			Map(m => m.Imo).Convert(row => (int?)row.Row.GetField<float?>("imo"));
			Map(m => m.Mmsi).Name("mmsi");
			Map(m => m.Timestamp).Name("timestamp");
			Map(m => m.Lon).Name("lon");
			Map(m => m.Lat).Name("lat");
			Map(m => m.Cog).Name("cog");
			Map(m => m.Sog).Name("sog");
			Map(m => m.Rot).Name("rot");
			Map(m => m.Acceleration).Name("acceleration");
			Map(m => m.TimeToImpact).Name("time_to_impact");
			Map(m => m.PointOfImpactLon).Name("point_of_impact_lon");
			Map(m => m.PointOfImpactLat).Name("point_of_impact_lat");
			Map(m => m.DistanceToImpact).Name("distance_to_impact");
			Map(m => m.TypeOfImpact).Name("type_of_impact");
			Map(m => m.SectorPolygon).Convert(args =>
			{
				var wktReader = new WKTReader();
				var p = wktReader.Read(args.Row.GetField("sector_polygon"));
				p.SRID = 4326;
				return p;
			});
			Map(m => m.MainlandCategory).Name("mainland_category");
			Map(m => m.Draught).Name("draught");
			Map(m => m.Shiptype).Name("shiptype");
			Map(m => m.LengthGroup).Name("length_group");
			Map(m => m.Tobow).Name("tobow");
			Map(m => m.TimeOfImpact).Name("time_of_impact");
			Map(m => m.Geohash).Name("geohash");
			Map(m => m.RelDistStd).Name("rel_dist_std");
			Map(m => m.RelSogStd).Name("rel_sog_std");
		}
	}
}