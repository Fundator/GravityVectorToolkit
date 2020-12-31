using FluentNHibernate.Mapping;
using GravityVectorToolKit.DataModel;
using NetTopologySuite.Geometries;
using NHibernate.Spatial.Type;

namespace GravityVectorToolKit.CSV.Mapping
{
	public class DeviationCellMapping<T> : ClassMap<DeviationCell> where T : IGeometryUserType
	{
		public DeviationCellMapping()
		{
			ImportType<Geometry>();
			Id(x => x.DeviationCellId).GeneratedBy.Assigned();
			Map(x => x.DistToP).Index("Deviation_Filter_Idx");
			Map(x => x.PPoint).Column("ppoint").CustomType<T>();
			Map(x => x.RelDistStd).Index("Deviation_Filter_Idx");
			Map(x => x.RelDistMax).Index("Deviation_Filter_Idx");
			Map(x => x.RelCogStd).Index("Deviation_Filter_Idx");
			Map(x => x.RelCogMax).Index("Deviation_Filter_Idx");
			Map(x => x.RelSogStd).Index("Deviation_Filter_Idx");
			Map(x => x.RelSogMax).Index("Deviation_Filter_Idx");
			Map(x => x.G1).Column("g1").CustomType<T>();
			Map(x => x.G2).Column("g2").CustomType<T>();
			Map(x => x.Significance).Index("Deviation_Filter_Idx");
			Map(x => x.Position).Column("position").CustomType<T>();
			Map(x => x.DistStdLeft).Index("Deviation_Filter_Idx");
			Map(x => x.DistStdRight).Index("Deviation_Filter_Idx");
			Map(x => x.DistMaxLeft).Index("Deviation_Filter_Idx");
			Map(x => x.DistMaxRight).Index("Deviation_Filter_Idx");
			Map(x => x.Geom).Column("geom").CustomType<T>();
		}
	}
}