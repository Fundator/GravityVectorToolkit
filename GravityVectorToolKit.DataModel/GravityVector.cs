using NetTopologySuite.Geometries;
using System;

namespace GravityVectorToolKit.DataModel
{
	public class GravityVector
	{
		public virtual int GravityVectorId { get; set; }
		public virtual NormalRoute NormalRoute { get; set; }
		public virtual int SerialId { get; set; }
		public virtual double ClusterIndex { get; set; }
		public virtual double GridId { get; set; }
		public virtual double Latitude { get; set; }
		public virtual double Longitude { get; set; }
		public virtual double SpeedOverGround { get; set; }
		public virtual double CourseOverGround { get; set; }
		public virtual double Eta { get; set; }
		public virtual double EtaLowerStd { get; set; }
		public virtual double EtaUpperStd { get; set; }

		#region Position statistics

		public virtual double DistanceMedian { get; set; }
		public virtual double MaxDistanceLeft { get; set; }
		public virtual double MaxDistanceRight { get; set; }
		public virtual double DistanceStdDevLeft { get; set; }
		public virtual double DistanceStdDevRight { get; set; }
		public virtual double MaxLesserSpeedDiff { get; set; }
		public virtual double MaxGreaterSpeedDiff { get; set; }
		public virtual double LesserSpeedStdDev { get; set; }
		public virtual double GreaterSpeedStdDev { get; set; }
		public virtual double MaxLesserCourseDiff { get; set; }
		public virtual double MaxGreaterCourseDiff { get; set; }
		public virtual double LesserCourseStdDev { get; set; }
		public virtual double GreaterCourseStdDev { get; set; }

		#endregion Position statistics

		#region Vessel detail statistics

		public virtual double MaxLesserShipGrossTonnageDiff { get; set; }
		public virtual double MaxGreaterShipGrossTonnageDiff { get; set; }
		public virtual double LesserShipGrossTonnageStdDev { get; set; }
		public virtual double GreaterShipGrossTonnageStdDev { get; set; }
		public virtual double MaxLesserShipLengthDiff { get; set; }
		public virtual double MaxGreaterShipLengthDiff { get; set; }
		public virtual double LesserShipLengthStdDev { get; set; }
		public virtual double GreaterShipLengthStdDev { get; set; }
		public virtual double MaxLesserShipBreadthDiff { get; set; }
		public virtual double MaxGreaterShipBreadthDiff { get; set; }
		public virtual double LesserShipBreadthStdDev { get; set; }
		public virtual double GreaterShipBreadthStdDev { get; set; }

		#endregion Vessel detail statistics

		#region Wave statistics

		public virtual double MaxLesserShipWaveHeightDiff { get; set; }
		public virtual double MaxGreaterShipWaveHeightDiff { get; set; }
		public virtual double LesserShipWaveHeightStdDev { get; set; }
		public virtual double GreaterShipWaveHeightStdDev { get; set; }
		public virtual double MaxLesserShipWaveDirectionDiff { get; set; }
		public virtual double MaxGreaterShipWaveDirectionDiff { get; set; }
		public virtual double LesserShipWaveDirectionStdDev { get; set; }
		public virtual double GreaterShipWaveDirectionStdDev { get; set; }

		#endregion Wave statistics

		public virtual double DataCount { get; set; }
		public virtual DateTime LastModified { get; set; }
		public virtual Geometry PositionGeometry { get; set; }

		#region Metadata

		/// <summary>
		/// This field is only used for lookup to identify the correct normalroute based on its id
		/// </summary>
		public virtual string NormalRouteId { get; set; }

		#endregion Metadata
	}
}