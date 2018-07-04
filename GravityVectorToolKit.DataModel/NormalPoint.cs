using GeoAPI.Geometries;

namespace GravityVectorToKML
{
    public class NormalPoint
    {
        public double clusterindex { get; set; }
        public double grid_id { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public double sog { get; set; }
        public double cog { get; set; }
        public double dist_med { get; set; }
        public string max_dist_left { get; set; }
        public string max_dist_right { get; set; }
        public string dist_std_left { get; set; }
        public string dist_std_right { get; set; }
        public double max_lesser_sog_diff { get; set; }
        public double max_greater_sog_diff { get; set; }
        public double lesser_sog_std { get; set; }
        public double greater_sog_std { get; set; }
        public string max_lesser_cog_diff { get; set; }
        public string max_greater_cog_diff { get; set; }
        public double lesser_cog_std { get; set; }
        public double greater_cog_std { get; set; }
        public double data_count { get; set; }
    }

    public class NormalPointG : NormalPoint
    {
        public IPoint PositionGeometry { get; set; }
    }
}
