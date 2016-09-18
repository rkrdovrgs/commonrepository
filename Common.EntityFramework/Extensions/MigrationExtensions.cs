using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.EntityFramework.Extensions
{
    public static class MigrationExtensions
    {
        public static void ExecuteSqlCommandFromResource(this System.Data.Entity.Database database, string resourceName) 
        {
            var frame = new StackFrame(1);
            var assembly = frame.GetMethod().DeclaringType.Assembly;
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (TextReader reader = new StreamReader(stream))
            {
                string[] hierarchyQueries = reader.ReadToEnd().Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                var currentQuery = new StringBuilder();
                foreach (var hq in hierarchyQueries)
                {
                    if (hq.StartsWith("--")) continue;
                    if (hq.ToUpper().Trim() != "GO")
                        currentQuery.AppendFormat(" {0}", hq);
                    else
                    {
                        database.ExecuteSqlCommand(currentQuery.ToString());
                        currentQuery.Clear();
                    }
                }
            }
        }
    }
}
