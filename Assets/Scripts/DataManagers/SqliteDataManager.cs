using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Constants;
using Mono.Data.Sqlite;
using UnityEngine;
using UnityTemplateProjects;

namespace DataManagers
{
    [Serializable]
    public class SqliteDataManager : IDataManager
    {
        private readonly string dbUrl = Path.Combine("URI=file:" + Application.streamingAssetsPath, "monsters.sqlite");
        private IDbConnection connection;
        private IDbCommand command;

        public void Initialize(IEnumerable<CreatureModel> creatures)
        {
            OpenConnection();
            CreateCommand();

            command.CommandText = Query.DROP_MONSTERS_IF_EXISTS;
            command.ExecuteNonQuery();

            command.CommandText = Query.CREATE_TABLE_MONSTERS;
            command.ExecuteNonQuery();

            foreach (var creature in creatures)
            {
                Insert(creature);
            }

            Close();
        }

        public void Insert(CreatureModel creatureModel)
        {
            OpenConnection();

            var preparedStatement = new SqliteCommand
            {
                CommandText = Query.INSERT_INTO_MONSTERS,
                Connection = (SqliteConnection) connection
            };

            preparedStatement.Parameters.Add(CreateParameter("class", creatureModel.CreatureClass));
            preparedStatement.Parameters.Add(CreateParameter("family", creatureModel.Family));
            preparedStatement.Parameters.Add(CreateParameter("creature", creatureModel.CreatureName));
            preparedStatement.Parameters.Add(CreateParameter("trait", creatureModel.TraitName));
            preparedStatement.Parameters.Add(CreateParameter("description", creatureModel.TraitDescription));
            preparedStatement.Parameters.Add(CreateParameter("material", creatureModel.MaterialName));
            preparedStatement.ExecuteNonQuery();

            preparedStatement.Dispose();
            Close();
        }

        public IEnumerable<CreatureModel> QueryForCreatures(CreatureQueryModel queryModel)
        {
            OpenConnection();
            CreateCommand();

            var preparedStatement = new SqliteCommand
            {
                Connection = (SqliteConnection) connection
            };

            var sb = new StringBuilder();
            var hasCondition = false;

            if (!string.IsNullOrEmpty(queryModel.MClass))
            {
                hasCondition = true;
                sb.Append("class LIKE $class");
                preparedStatement.Parameters.Add(CreateParameter("class", WrapLike(queryModel.MClass)));
            }

            if (!string.IsNullOrEmpty(queryModel.Family))
            {
                if (hasCondition)
                {
                    sb.Append(" AND ");
                }
                hasCondition = true;

                sb.Append("family LIKE $family");
                preparedStatement.Parameters.Add(CreateParameter("family", WrapLike(queryModel.Family)));
            }

            if (!string.IsNullOrEmpty(queryModel.Trait))
            {
                if (hasCondition)
                {
                    sb.Append(" AND ");
                }
                hasCondition = true;

                sb.Append("trait LIKE $trait");
                preparedStatement.Parameters.Add(CreateParameter("trait", WrapLike(queryModel.Trait)));
            }

            if (queryModel.GetCreatures()?.Any() == true)
            {
                if (hasCondition)
                {
                    sb.Append(" AND ");
                }
                hasCondition = true;

                const string creatureSql = "({creatures})";
                var parameters = AddParameterList(
                    queryModel.GetCreatures(),
                    "creature",
                    preparedStatement);
                sb.Append(creatureSql.Replace("{creatures}", parameters));
            }

            if (queryModel.GetDescription()?.Any() == true)
            {
                if (hasCondition)
                {
                    sb.Append(" AND ");
                }
                hasCondition = true;

                const string descriptionSql = "({description})";
                var parameters = AddParameterList(
                    queryModel.GetDescription(),
                    "trait_description",
                    preparedStatement);
                sb.Append(descriptionSql.Replace("{description}", parameters));
            }

            if (hasCondition)
            {
                sb.Insert(0, " WHERE ");
            }

            sb.Insert(0, Query.SELECT_FROM_MONSTER);

            preparedStatement.CommandText = sb.ToString();
            var reader = preparedStatement.ExecuteReader();
            
            var monsters = new List<CreatureModel>();
            while (reader.Read())
            {
                try
                {
                    monsters.Add(new CreatureModel
                    {
                        CreatureClass = reader.GetSafeString(0),
                        Family = reader.GetSafeString(1),
                        CreatureName = reader.GetSafeString(2),
                        TraitName = reader.GetSafeString(3),
                        TraitDescription = reader.GetSafeString(4),
                        MaterialName = reader.GetSafeString(5)
                    });
                }
                catch (InvalidCastException e)
                {
                    Debug.LogError(e);
                    Debug.LogError(reader);
                    throw;
                }
            }
            
            Close();

            return monsters;
        }
    
        public List<string> QueryDistinctClass()
        {
            // TODO: does not filter out unplayable classes yes, use LinqDataManager or implement changes
            Debug.LogWarning("Function not up to date, see code comment");
            var classes = QueryForColumn(Query
                .SELECT_FIELD_FROM_MONSTERS
                .Replace("{field}", "distinct class"));
            classes.Sort((s1, s2) => string.Compare(s1, s2, StringComparison.Ordinal));
            classes.Insert(0, "");
            return classes;
        }

        public List<string> QueryDistinctFamily()
        {
            var families = QueryForColumn(Query
                .SELECT_FIELD_FROM_MONSTERS
                .Replace("{field}", "distinct family"));
            families.Sort((s1, s2) => string.Compare(s1, s2, StringComparison.Ordinal));
            families.Insert(0, "");
            return families;
        }
    
        private IDbDataParameter CreateParameter(string name, string value)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;

            return parameter;
        }

        private static string WrapLike(string value)
        {
            return "%" + value + "%";
        }
    
        private void OpenConnection()
        {
            connection ??= new SqliteConnection(dbUrl);
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
        }

        private void CreateCommand()
        {
            command ??= connection.CreateCommand();
        }

        private void Close()
        {
            command?.Dispose();

            if (connection != null && connection.State != ConnectionState.Closed)
            {
                connection.Dispose();
            }
        }

        private List<string> QueryForColumn(string query)
        {
            OpenConnection();
            CreateCommand();
            
            var preparedStatement = new SqliteCommand
            {
                CommandText = query,
                Connection = (SqliteConnection) connection
            };

            var data = new List<string>();
            var reader = preparedStatement.ExecuteReader();
            while (reader.Read())
            {
                data.Add(reader.GetString(0));
            }
            
            Close();

            return data;
        }

        private string AddParameterList(IEnumerable<string> parameters, string identifier, SqliteCommand statement)
        {
            var queryList = new List<string>();
            var index = 0;
            foreach (var parameter in parameters)
            {
                queryList.Add(identifier + " LIKE $" + identifier + "_" + index);
                statement.Parameters.Add(CreateParameter(identifier + "_" + index++, "%" + parameter + "%"));
            }

            return string.Join(" OR ", queryList);
        }
    }
}