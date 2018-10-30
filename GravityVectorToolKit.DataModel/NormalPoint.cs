﻿using GeoAPI.Geometries;
using NetTopologySuite.Geometries;

namespace GravityVectorToKML.Model
{
	public class NormalPoint
	{
		public virtual int Id { get; set; }
		public virtual int FromLocationId { get; set; }
		public virtual int ToLocationId { get; set; }
		public virtual double ClusterIndex { get; set; }
		public virtual double GridId { get; set; }
		public virtual double Latitude { get; set; }
		public virtual double Longitude { get; set; }
		public virtual double SpeedOverGround { get; set; }
		public virtual double CourseOverGround { get; set; }
		public virtual double Eta { get; set; }
		public virtual double EtaLowerStd { get; set; }
		public virtual double EtaUpperStd { get; set; }
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
		public virtual double DataCount { get; set; }
	}

	public class NormalPointG : NormalPoint
	{
		public virtual IGeometry PositionGeometry { get; set; }
	}
}