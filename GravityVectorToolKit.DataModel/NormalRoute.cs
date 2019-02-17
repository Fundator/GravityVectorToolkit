using GeoAPI.Geometries;
using System.Collections.Generic;

namespace GravityVectorToolKit.DataModel
{
	public class NormalRoute
	{
		public virtual int NormalRouteId { get; set; }
		public virtual int FromLocationId { get; set; }
		public virtual int ToLocationId { get; set; }
		public virtual IGeometry NormalRouteGeometry { get; set; }
		public virtual IList<NormalPoint> NormalPoints { get; set; }
	}
}