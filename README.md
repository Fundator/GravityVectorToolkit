# Gravity Vector Toolkit
Tools to import gravity vectors, normal routes and deviation cells from CSV files into a PostGIS database. Also includes tools for combining AIS data with weather data from thredds.met.no.

Requirements:
* Visual Studio 2019 Community Edition (only required to compile from source)
* .NET Core 3.1 SDK (or newer)

## How to use GravityVectorToolkit.Tools.DatabaseImport 

When using a compiled program, DatabaseImport can be executed as follows: 
```$ dotnet run GravityVectorToolkit.Tools.DatabaseImport.exe --connectionstring <str> --gv-merged-path <path> --normal-route-path <path> [--deviation-cells-path <path>]```

DatabaseImport can also be run directly from source. Simply `cd` into the GravityVectorToolkit.Tools.DatabaseImport project folder and run:
```
$ dotnet build 
$ dotnet run --connectionstring <str> --gv-merged-path <path> --normal-route-path <path> [--deviation-cells-path <path>]
```

When using Visual Studio, GVTK can be executed by running the project GravityVectorToolkit.Tools.DatabaseImport. However, the command line arguments must be entered into launchSettings.json or added to the application arguments text box in the project properties view. 

Available options:

Name | Required | Description
------------ | ------------- | -------------
`--connectionstring <str>` | Yes | A full connection string to a PostgreSQL database with PostGIS enabled.
`--gv-merged-path <path>` | Yes | The path to a merged gravity vector file (output from MADART).
`--normal-route-path <path>` | Yes | The path to a merged normal routes file (output from MADART).
`--deviation-cells-path <path>` | No | The path to a deviation cell file (output from MADART). 

## How to use GravityVectorToolKit.Tools.AisCombine

When using a compiled program, AisCombine can be executed as follows: 
```$ dotnet run GravityVectorToolkit.Tools.AisCombine.exe```

AisCombine reads its settings from a configuration file called settings.json. Here is an example settings.json file:
```
{
   "AisSourceDirectory":"/path/to/inputdir",
   "AisDestinationDirectory":"/path/to/outputdir",
   "WeatherDbPath":"/path/to/weather-database.sqlite",
   "MaxParallellism":8,
   "AssumeNoEpochCollissions":true,
   "TimestampColumnName":"date_time_utc",
   "LatitudeColumnName":"lat",
   "LongitudeColumnName":"lon",
   "CalculateClosest":true,
   "GeohashMatchPrecision":5,
   "PrecisionSearchLimit":3
}
```

Available settings:

Name | Required | Default value | Description
------------ | ------------- | ------------- | -------------
`AisSourceDirectory` | Yes | n/a | AIS source file directory containing AIS files in CSV format.
`AisDestinationDirectory` | Yes | n/a | Destination directory (requires approx 150% capacity compared to the total input file sizes).
`AssumeNoEpochCollissions` | Yes | true | Set this to true if the files containing AIS data is split on days. If runtime concurrency errors occur, set this to false. Medium to high negative performance impact.
`CalculateClosest` | Yes | true | Higher precision when matching positions with weather data. Very low negative performance impact, but with greatly improved end result.
`Delimiter` | No | ";" | Which delimiter to use to parse the input CSV files.
`GeohashMatchPrecision` | Yes | 5 | The initial geohash precision to search for weather data (includes neighbours). Should be roughly equivalent to the resolution of the weather data set.
`LatitudeColumnName` | Yes | "lat" | The name of the latitude column in the source AIS files.
`LongitudeColumnName` | Yes | "lon" |  The name of the longitude column in the source AIS files. 
`MaximumEpochAgeMinutes` | No | 15 | The maximum age of cached weather epochs before being vacuumed.
`MaxParallellism` | Yes | maximum available threads | The maximum amount of threads to use. Set this to a lower value if you run out of memory.
`PrecisionSearchLimit` | Yes | 3 | The lowest geohash precision to search for weather data (includes neighbours). Values higher than 3 is not recommended.
`SortBy` | No | "path" | Which sort order to use when processing files. Valid settings are "path" and "size".
`StatusMessageCycleSeconds` | No | 10 | How often the status messages is printed to the console during processing.
`TimestampColumnName` | Yes | "date_time_utc" | The name of the timestamp column in the source AIS files.
`UseDirectSQL` | No | false | Set this to true to query the database directly and cache the results, which is faster for CSV-files grouped by MMSI. For CSV files grouped by day, set this to false.
`WeatherDbPath` | Yes | n/a | The path to the SQLite file containing weather data.

