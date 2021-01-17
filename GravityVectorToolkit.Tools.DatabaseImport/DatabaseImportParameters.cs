namespace GravityVectorToolkit.Tools.DatabaseImport
{
	public class DatabaseImportParameters
	{
		public string ConnectionString { get; set; }
		public string NormalRoutePath { get; set; }
		public string GravityVectorPath { get; set; }
		public string DeviationMapPath { get; set; }
		public bool DropAndCreate { get; set; }
	}
}