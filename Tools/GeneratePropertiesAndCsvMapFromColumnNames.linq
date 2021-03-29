<Query Kind="Statements">
  <Reference>&lt;RuntimeDirectory&gt;\mscorlib.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Globalization.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Globalization.Extensions.dll</Reference>
  <Namespace>System.Globalization</Namespace>
</Query>

// Automatically generate appropriate property names and CSV class mappings based on a CSV header
// Author: Andreas Ravnestad (march 2021)
var headerStr = "imo,mmsi,timestamp,lon,lat,cog,sog,rot,acceleration,time_to_impact,point_of_impact_lon,point_of_impact_lat,distance_to_impact,type_of_impact,sector_polygon,mainland_category,draught,shiptype,length_group,tobow,time_of_impact,geohash,rel_dist_std,rel_sog_std,id";
var propDefs = new List<string>();
var csvMapDefs = new List<string>();
var nHibMapDefs = new List<string>();
var pieces = headerStr.Split(',');
foreach(var piece in pieces) {
	var propName = piece.Trim().ToLower().Replace("_", " ");
	propName = CultureInfo.CurrentCulture.TextInfo
					.ToTitleCase(propName)
						.Replace(" ", string.Empty);
	var propDef = $"public virtual object {propName} {{ get; set; }}";
	var csvMapDef = $"Map(m => m.{propName}).Name(\"{piece}\");";
	var nHibMapDef = $"Map(x => x.{propName});";
	propDefs.Add(propDef);
	csvMapDefs.Add(csvMapDef);	
	nHibMapDefs.Add(nHibMapDef);	
	
}
Console.WriteLine("=== Model class property definitions: ===");
Console.WriteLine();

foreach(var propDef in propDefs) {
	Console.WriteLine(propDef);
}
Console.WriteLine();
Console.WriteLine("=== CSV mapping definitions: ===");
Console.WriteLine();
foreach(var mapDef in csvMapDefs) {
	Console.WriteLine(mapDef);
}
Console.WriteLine();
Console.WriteLine("=== NHibernate mapping definitions: ===");
Console.WriteLine();
foreach(var mapDef in nHibMapDefs) {
	Console.WriteLine(mapDef);
}