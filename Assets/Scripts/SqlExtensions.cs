using System.Data;
using Mono.Data.Sqlite;

namespace UnityTemplateProjects
{
    public static class SqlExtensions
    {
        public static string GetSafeString(this IDataRecord reader, int index)
        {
            return !reader.IsDBNull(index) ? reader.GetString(index) : string.Empty;
        }
    }
}