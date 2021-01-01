using CsvHelper.Configuration;
using GravityVectorToolKit.DataModel;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;

namespace GravityVectorToolKit.CSV.Mapping
{
	public class NormalRouteCsvClassMap : ClassMap<NormalRoute>
	{
		//private WKTReader wktReader = new WKTReader();

		public NormalRouteCsvClassMap()
		{
			/*
				dep_id,
				arr_id,
				high_error,
				normal_route,
				voyage_count,

				avg_sog,
				system_name,
				ship_type_group,
				normal_route_max_polygon,
				normal_route_std_polygon,
				backbone_harbour,
				avg_gross_tonnage,
				avg_length,
				avg_breadth,
				avg_month,
				avg_hs,
				avg_thq,
				std_hs,
				std_thq,
				std_gross_tonnage,
				std_length,
				std_breadth,
				std_month,
				avg_eta_seconds

			 */
			//Map(m => m.AverageSog).ConvertUsing(row =>
			//{
			//	var field = row.GetField("avg_sog");
			//	return JsonSerializer.Deserialize<List<float>>(field);
			//});

			Map(m => m.BackboneHarbour).Name("backbone_harbour")
					.TypeConverterOption.BooleanValues(true, true, "True", "true")
					.TypeConverterOption.BooleanValues(false, true, "False", "false");

			Map(m => m.NormalRouteId).ConvertUsing(row => row.GetField<string>("normal_route_id"));
			Map(m => m.FromLocationId).Name("dep_id");
			Map(m => m.ToLocationId).Name("arr_id");
			Map(m => m.HighError).Name("high_error")
					.TypeConverterOption.BooleanValues(true, true, "True", "true")
					.TypeConverterOption.BooleanValues(false, true, "False", "false");

			Map(m => m.VoyageCount).ConvertUsing(row => (int)row.GetField<float>("voyage_count"));

			//Map(m => m.FromLocationId).Name("dep_id");
			//Map(m => m.FromLocationId).Name("dep_id");
			//Map(m => m.FromLocationId).Name("dep_id");

			Map(m => m.NormalRouteGeometry).ConvertUsing(row =>
			{
				WKTReader wktReader = new WKTReader();
				Geometry p = wktReader.Read(row.GetField("normal_route"));
				p.SRID = 4326;
				return p;
			});
			//Map(m => m.NormalRouteMaxGeometry).ConvertUsing(row =>
			//{
			//	WKTReader wktReader = new WKTReader();
			//	Geometry p = wktReader.Read(row.GetField("normal_route_max_polygon")).Buffer(0);
			//	p.SRID = 4326;
			//	return p;
			//});
			//Map(m => m.NormalRouteStdGeometry).ConvertUsing(row =>
			//{
			//	WKTReader wktReader = new WKTReader();
			//	Geometry p = wktReader.Read(row.GetField("normal_route_std_polygon")).Buffer(0);
			//	p.SRID = 4326;
			//	return p;
			//});
		}
	}
}