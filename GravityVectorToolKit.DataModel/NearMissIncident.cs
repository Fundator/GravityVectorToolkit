using NetTopologySuite.Geometries;
using System;

namespace GravityVectorToolKit.DataModel
{

	public class NearMissIncident
	{
		public virtual long NearMissId { get; set; }
		public virtual long IncidentId { get; set; }
		public virtual int? Imo { get; set; }
		public virtual int Mmsi { get; set; }
		public virtual DateTime Timestamp { get; set; }
		public virtual double Lon { get; set; }
		public virtual double Lat { get; set; }
		public virtual double Cog { get; set; }
		public virtual double Sog { get; set; }
		public virtual double Rot { get; set; }
		public virtual double Acceleration { get; set; }
		public virtual double TimeToImpact { get; set; }
		public virtual double PointOfImpactLon { get; set; }
		public virtual double PointOfImpactLat { get; set; }
		public virtual double DistanceToImpact { get; set; }
		public virtual string TypeOfImpact { get; set; }
		public virtual Geometry SectorPolygon { get; set; }
		public virtual string MainlandCategory { get; set; }
		public virtual double Draught { get; set; }
		public virtual string Shiptype { get; set; }
		public virtual string LengthGroup { get; set; }
		public virtual double Tobow { get; set; }
		public virtual DateTime TimeOfImpact { get; set; }
		public virtual string Geohash { get; set; }
		public virtual double RelDistStd { get; set; }
		public virtual double RelSogStd { get; set; }
	}
}