using GeoAPI.Geometries;
using NetTopologySuite.Geometries;

namespace GravityVectorToKML.Model
{
    public class NormalPoint
    {
        public virtual int Id { get; set; }
        public virtual double ClusterIndex { get; set; }
        public virtual double GridId { get; set; }
        public virtual double Latitude { get; set; }
        public virtual double Longitude { get; set; }
        public virtual double SpeedOverGround { get; set; }
        public virtual double CourseOverGround { get; set; }
        public virtual double DistanceMedian { get; set; }
        public virtual string MaxDistanceLeft { get; set; }
        public virtual string MaxDistanceRight { get; set; }
        public virtual string DistanceStdDevLeft { get; set; }
        public virtual string DistanceStdDevRight { get; set; }
        public virtual double MaxLesserSpeedDiff { get; set; }
        public virtual double MaxGreaterSpeedDiff { get; set; }
        public virtual double LesserSpeedStdDev { get; set; }
        public virtual double GreaterSpeedStdDev { get; set; }
        public virtual string MaxLesserCourseDiff { get; set; }
        public virtual string MaxGreaterCourseDiff { get; set; }
        public virtual double LesserCourseStdDev { get; set; }
        public virtual double GreaterCourseStdDev { get; set; }
        public virtual double DataCount { get; set; }
    }

    public class NormalPointG : NormalPoint
    {
        public virtual IGeometry PositionGeometry { get; set; }
    }
}