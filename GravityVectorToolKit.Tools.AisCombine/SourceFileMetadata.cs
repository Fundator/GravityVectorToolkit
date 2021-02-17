namespace GravityVectorToolKit.Tools.AisCombine
{
	public class SourceFileMetadata
	{
		public SourceFileMetadata()
		{
		}
		public string Path { get; internal set; }
		public long LineCount { get; internal set; }
		public long FileSize { get; internal set; }
	}
}