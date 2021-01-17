using GravityVectorToolKit.Common;
using GravityVectorToolKit.Common.Extensions;
using GravityVectorToolKit.CSV.Mapping;
using GravityVectorToolKit.DataModel;
using log4net;
using NetTopologySuite.Geometries;
using NetTopologySuite.Operation.Valid;
using NHibernate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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