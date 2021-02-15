using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NetTopologySuite.Geometries;

#nullable disable

namespace GravityVectorToolKit.Tools.AisCombine.DataAccess
{
	public partial class WeatherHist
	{
		public long Epoch { get; set; }
		public double? Hs { get; set; }
		public double? Thq { get; set; }
		public double? Ff { get; set; }
		public double? Dd { get; set; }
		public string Geohash { get; set; }
	}
}
