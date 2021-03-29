using System.Text.Json.Serialization;

namespace GravityVectorToolkit.Tools.DatabaseImport
{
	public class DatabaseImportParameters
	{
		public DatabaseImportParameters()
		{
			ConnectionDetails = new ConnectionDetails();
		}

		[JsonIgnore]
		public string ConnectionString
		{
			get
			{
				return $"Server={ConnectionDetails.HostName};Port={ConnectionDetails.Port};Database={ConnectionDetails.DatabaseName};User Id={ConnectionDetails.Username};Password = {ConnectionDetails.Password}";
			}
		}

		public string NormalRoutePath { get; set; }
		public string GravityVectorPath { get; set; }
		public string DeviationMapPath { get; set; }
		public string NearMissMapPath { get; set; }
		public bool DropAndCreate { get; set; }
		public ConnectionDetails ConnectionDetails { get; set; }
	}
}