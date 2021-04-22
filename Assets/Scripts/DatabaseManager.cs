﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Mono.Data.Sqlite;
using UnityEngine;

namespace UnityTemplateProjects
{
    public class DatabaseManager
    {
        private readonly string dbUrl = "URI=file:" + Application.dataPath + "/monsters.sqlite";
        private IDbConnection connection;
        private IDbCommand command;

        public DatabaseManager(bool resetDb = false)
        {
            Debug.Log(dbUrl);
            OpenConnection();
            CreateCommand();
            if (resetDb)
            {
                command.CommandText = Query.DROP_MONSTERS_IF_EXISTS;
                command.ExecuteNonQuery();
            }

            command.CommandText = Query.CREATE_TABLE_MONSTERS;
            command.ExecuteNonQuery();

            Close();
        }

        public void Insert(Monster monster)
        {
            OpenConnection();

            var preparedStatement = new SqliteCommand
            {
                CommandText = Query.INSERT_INTO_MONSTERS,
                Connection = (SqliteConnection) connection
            };

            preparedStatement.Parameters.Add(CreateParameter("class", monster.MonsterClass.ToString()));
            preparedStatement.Parameters.Add(CreateParameter("family", monster.Family));
            preparedStatement.Parameters.Add(CreateParameter("creature", monster.Creature));
            preparedStatement.Parameters.Add(CreateParameter("trait", monster.TraitName));
            preparedStatement.Parameters.Add(CreateParameter("description", monster.TraitDescription));
            preparedStatement.Parameters.Add(CreateParameter("material", monster.MaterialName));
            preparedStatement.ExecuteNonQuery();

            preparedStatement.Dispose();
            Close();
        }

        private IDbDataParameter CreateParameter(string name, string value)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;

            return parameter;
        }

        private string WrapLike(string value)
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

        public List<Monster> QueryForMonsters(QueryObject query)
        {
            OpenConnection();
            CreateCommand();

            var preparedStatement = new SqliteCommand
            {
                Connection = (SqliteConnection) connection
            };

            var sb = new StringBuilder();
            var hasCondition = false;

            if (!string.IsNullOrEmpty(query.MClass))
            {
                hasCondition = true;
                sb.Append("class LIKE $class");
                preparedStatement.Parameters.Add(CreateParameter("class", WrapLike(query.MClass)));
            }

            if (!string.IsNullOrEmpty(query.Family))
            {
                if (hasCondition)
                {
                    sb.Append(" AND ");
                }
                hasCondition = true;

                sb.Append("family LIKE $family");
                preparedStatement.Parameters.Add(CreateParameter("family", WrapLike(query.Family)));
            }

            if (!string.IsNullOrEmpty(query.Trait))
            {
                if (hasCondition)
                {
                    sb.Append(" AND ");
                }
                hasCondition = true;

                sb.Append("trait LIKE $trait");
                preparedStatement.Parameters.Add(CreateParameter("trait", WrapLike(query.Trait)));
            }

            if (query.GetCreatures()?.Any() == true)
            {
                if (hasCondition)
                {
                    sb.Append(" AND ");
                }
                hasCondition = true;

                const string creatureSql = "({creatures})";
                var parameters = AddParameterList(
                    query.GetCreatures(),
                    "creature",
                    preparedStatement);
                sb.Append(creatureSql.Replace("{creatures}", parameters));
            }

            if (query.GetDescription()?.Any() == true)
            {
                if (hasCondition)
                {
                    sb.Append(" AND ");
                }
                hasCondition = true;

                const string descriptionSql = "({description})";
                var parameters = AddParameterList(
                    query.GetDescription(),
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
            
            var monsters = new List<Monster>();
            while (reader.Read())
            {
                monsters.Add(new Monster
                {
                    MonsterClass = reader.GetString(0),
                    Family = reader.GetString(1),
                    Creature = reader.GetString(2),
                    TraitName = reader.GetString(3),
                    TraitDescription = reader.GetString(4),
                    MaterialName = reader.GetString(5)
                });
            }
            
            Close();

            return monsters;
        }

        public List<string> QueryForColumn(string query)
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