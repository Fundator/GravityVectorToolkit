using CsvHelper;
using CsvHelper.Configuration;
using GravityVectorToKML.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace GravityVector.Common
{
    public class Util
    {
        public static List<NormalPoint> ReadGravityVector(string file)
        {
            var csv = new CsvReader(File.OpenText(file), new Configuration
            {
                CultureInfo = CultureInfo.InvariantCulture
            });
            var records = csv.GetRecords<NormalPoint>().ToList();
            return records;
        }
    }
}
