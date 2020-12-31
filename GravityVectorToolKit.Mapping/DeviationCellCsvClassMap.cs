using CsvHelper.Configuration;
using GravityVectorToolKit.DataModel;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using System;
using System.Globalization;

namespace GravityVectorToolKit.CSV.Mapping
{
	public class DeviationCellCsvClassMap : ClassMap<DeviationCell>
	{
		/// <summary>
		/// geohash,Unnamed: 0,disttoP,Plat,Plon,rel_dist_std,rel_dist_max,rel_cog_std,rel_cog_max,rel_sog_std,rel_sog_max,g1lat,g1lon,
		/// g2lat,g2lon,significance,lat,lon,dist_std_left,dist_std_right,dist_max_left,dist_max_right,bbox,geometry
		/// </summary>
		public DeviationCellCsvClassMap()
		{
			Map(m => m.DeviationCellId).Name("geohash");
			//Map(m => m.NormalRouteId).ConvertUsing(row => row.GetField<string>("normal_route_id").Split("_")[0]);
			Map(m => m.DistToP).Name("disttoP");
			Map(m => m.PPoint).ConvertUsing(row =>
			{
				var p = new Point(
							new Coordinate(
								Double.Parse(row.GetField("Plon"), CultureInfo.InvariantCulture),
								Double.Parse(row.GetField("Plat"), CultureInfo.InvariantCulture)));
				p.SRID = 4326;
				return p;
			});
			Map(m => m.RelDistStd).Name("rel_dist_std");
			Map(m => m.RelDistMax).Name("rel_dist_max");
			Map(m => m.RelCogStd).Name("rel_cog_std");
			Map(m => m.RelCogMax).Name("rel_cog_max");
			Map(m => m.RelSogStd).Name("rel_sog_std");
			Map(m => m.RelSogMax).Name("rel_sog_max");
			Map(m => m.G1).ConvertUsing(row =>
			{
				var p = new Point(
							new Coordinate(
								Double.Parse(row.GetField("g1lon"), CultureInfo.InvariantCulture),
								Double.Parse(row.GetField("g1lat"), CultureInfo.InvariantCulture)));
				p.SRID = 4326;
				return p;
			});
			Map(m => m.G2).ConvertUsing(row =>
			{
				var p = new Point(
							new Coordinate(
								Double.Parse(row.GetField("g2lon"), CultureInfo.InvariantCulture),
								Double.Parse(row.GetField("g2lat"), CultureInfo.InvariantCulture)));
				p.SRID = 4326;
				return p;
			});
			Map(m => m.Significance).Name("significance");
			Map(m => m.Position).ConvertUsing(row =>
			{
				var p = new Point(
							new Coordinate(
								Double.Parse(row.GetField("lon"), CultureInfo.InvariantCulture),
								Double.Parse(row.GetField("lat"), CultureInfo.InvariantCulture)));
				p.SRID = 4326;
				return p;
			});
			Map(m => m.DistStdLeft).Name("dist_std_left");
			Map(m => m.DistStdRight).Name("dist_std_right");
			Map(m => m.DistMaxLeft).Name("dist_max_left");
			Map(m => m.DistMaxRight).Name("dist_max_right");
			Map(m => m.Geom).ConvertUsing(row =>
			{
				var wktReader = new WKTReader();
				var p = wktReader.Read(row.GetField("geometry"));
				p.SRID = 4326;
				return p;
			});
		}
	}
}