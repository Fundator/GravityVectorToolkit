# Gravity Vector Toolkit

A tool to import gravity vectors, normal routes and deviation cells from CSV files into a PostGIS database.

Requirements:
* Visual Studio 2019 Community Edition (only required to compile from source)
* .NET Core 3.1 or newer

## Usage

When using a compiled program, GVTK can be executed as follows: 
```$ dotnet run GravityVectorToolkit.Tools.DatabaseImport.exe --connectionstring <str> --gv-merged-path <path> --normal-route-path <path> [--deviation-cells-path <path>]```

GVTK can also be run directly from source. Simply `cd` into the GravityVectorToolkit.Tools.DatabaseImport project folder and run:
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
