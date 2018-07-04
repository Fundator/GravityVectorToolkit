using CsvHelper.Configuration;

namespace GravityVectorToKML
{
    public class ModelClassMap : ClassMap<NormalPoint>
    {
        public ModelClassMap()
        {
            Map(m => m.clusterindex).Name("clusterindex");
            Map(m => m.grid_id).Name("grid_id");
            Map(m => m.x).Name("x");
            Map(m => m.y).Name("y");
            Map(m => m.sog).Name("sog");
            Map(m => m.cog).Name("cog");
            Map(m => m.dist_med).Name("dist_med");
            Map(m => m.max_dist_left).Name("max_dist_left");
            Map(m => m.max_dist_right).Name("max_dist_right");
            Map(m => m.dist_std_left).Name("dist_std_left");
            Map(m => m.dist_std_right).Name("dist_std_right");
            Map(m => m.max_lesser_sog_diff).Name("max_lesser_sog_diff");
            Map(m => m.max_greater_sog_diff).Name("max_greater_sog_diff");
            Map(m => m.lesser_sog_std).Name("lesser_sog_std");
            Map(m => m.greater_sog_std).Name("greater_sog_std");
            Map(m => m.max_lesser_cog_diff).Name("max_lesser_cog_diff");
            Map(m => m.max_greater_cog_diff).Name("max_greater_cog_diff");
            Map(m => m.lesser_cog_std).Name("lesser_cog_std");
            Map(m => m.greater_cog_std).Name("greater_cog_std");
            Map(m => m.data_count).Name("data_count");
        }
    }
}
