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
			//Map(m => m.NormalRouteId).Name("");
			Map(m => m.FromLocationId).Name("dep_id");
			Map(m => m.ToLocationId).Name("arr_id");
			Map(m => m.NormalRouteGeometry).ConvertUsing(row =>
			{
				var p = wktReader.Read(row.GetField("normal_route"));
				p.SRID = 4326;
				return p;
			});
		}
	}
}