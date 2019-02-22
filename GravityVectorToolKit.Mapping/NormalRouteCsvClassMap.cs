using CsvHelper.Configuration;
using GeoAPI.Geometries;
using GravityVectorToolKit.DataModel;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using System;
using System.Globalization;

namespace GravityVectorToolKit.CSV.Mapping
{
	public class NormalRouteCsvClassMap : ClassMap<NormalRoute>
	{
		private WKTReader wktReader = new WKTReader();

		public NormalRouteCsvClassMap()
		{
			Map(m => m.NormalRouteId).ConvertUsing(row => (int)row.GetField<float>("normal_route_id"));
			Map(m => m.FromLocationId).Name("dep_id");
			Map(m => m.ToLocationId).Name("arr_id");
			Map(m => m.HighError).Name("high_error")
					.TypeConverterOption.BooleanValues(true, true, "True", "true")
					.TypeConverterOption.BooleanValues(false, true, "False", "false");
			Map(m => m.VoyageCount).ConvertUsing(row => (int)row.GetField<float>("voyage_count"));
			Map(m => m.NormalRouteGeometry).ConvertUsing(row =>
			{
				var p = wktReader.Read(row.GetField("normal_route"));
				p.SRID = 4326;
				return p;
			});
		}
	}
}