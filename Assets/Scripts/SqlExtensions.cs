using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;

namespace UnityTemplateProjects
{
    public static class SqlExtensions
    {
        public static string GetSafeString(this IDataRecord record, int index)
        {
            return !record.IsDBNull(index) ? record.GetString(index) : string.Empty;
        }


    }
}