using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Text;

namespace GravityVectorToolKit.DataModel
{

	/// <summary>
	/// geohash,Unnamed: 0,disttoP,Plat,Plon,rel_dist_std,rel_dist_max,rel_cog_std,rel_cog_max,rel_sog_std,rel_sog_max,g1lat,g1lon,
	/// g2lat,g2lon,significance,lat,lon,dist_std_left,dist_std_right,dist_max_left,dist_max_right,bbox,geometry
	/// 
	/// RelDistStd, RelDistMax, RelCogStd, RelCogMax, RelSogStd, RelSogMax, G1Lat, G1Lon, G2Lat, G2Lon, Significance, Lat, Lon, DistStdLeft, DistStdRight, DistMaxLeft, DistMaxRight, Bbox, Geometry
	/// 
	/// </summary>
	public class DeviationCell
	{
		public virtual string DeviationCellId { get; set; }
		public virtual double DistToP { get; set; }
		public virtual Point PPoint { get; set; }
		public virtual double RelDistStd { get; set; }
		public virtual double RelDistMax { get; set; }
		public virtual double RelCogStd { get; set; }
		public virtual double RelCogMax { get; set; }
		public virtual double RelSogStd { get; set; }
		public virtual double RelSogMax { get; set; }
		public virtual Point G1 { get; set; }
		public virtual Point G2 { get; set; }
		public virtual double Significance { get; set; }
		public virtual Point Position { get; set; }
		public virtual double DistStdLeft { get; set; }
		public virtual double DistStdRight { get; set; }
		public virtual double DistMaxLeft { get; set; }
		public virtual double DistMaxRight { get; set; }
		public virtual Geometry Geom { get; set; }

	}
}
