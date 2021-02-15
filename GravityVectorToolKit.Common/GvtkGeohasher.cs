using Geohash;
using NetTopologySuite.Geometries;
using System.Collections.Generic;
using System.Linq;

namespace GravityVectorToolKit.Common
{
	public class GvtkGeohasher
	{
		private Geohasher hasher = new Geohasher();

		public Point Decode(string geohash)
		{
			var result = hasher.Decode(geohash);
			return new Point(result.Item2, result.Item1);
		}

		public string Encode(Point p, int precision = 6)
		{
			return hasher.Encode(p.Y, p.X, precision);
		}

		public Polygon BoundingBox(string geohash)
		{
			var bb = hasher.GetBoundingBox(geohash);

			var coordinates = new List<Coordinate>();
			coordinates.Add(new Coordinate(bb[3], bb[0]));
			coordinates.Add(new Coordinate(bb[3], bb[1]));
			coordinates.Add(new Coordinate(bb[2], bb[1]));
			coordinates.Add(new Coordinate(bb[2], bb[0]));
			coordinates.Add(new Coordinate(bb[3], bb[0]));

			var linearRing = new LinearRing(coordinates.ToArray());

			var polygon = new Polygon(linearRing);

			return polygon;

		}

		public List<string> Neighbours(string geohash)
		{
			return hasher.GetNeighbors(geohash).Select(d => d.Value).ToList();
		}

		public List<string> Subhashes(string geohash)
		{
			return hasher.GetSubhashes(geohash).ToList();
		}

		public string Reduce(string hash)
		{
			if (string.IsNullOrWhiteSpace(hash))
			{
				return hash;
			}
			return hash.Remove(hash.Length - 1);
		}
		public string Reduce(string hash, int precision)
		{
			if (string.IsNullOrWhiteSpace(hash))
			{
				return hash;
			}
			return hash.Remove(precision);
		}

		public List<string> Parents(string hash, int precision = 1)
		{
			var result = new List<string>();
			while (hash.Length > precision)
			{
				hash = hasher.GetParent(hash);
				result.Add(hash);
			}

			return result;
		}
	}
}