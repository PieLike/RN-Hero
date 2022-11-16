using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
//using System;
using UnityEngine.Networking;
using System.Collections;

static class WorkWithDataBase
{
    private const string standartFileName = "db.bytes";
    private static string DBPath;
    private static SqliteConnection connection;
    private static SqliteCommand command;

    static WorkWithDataBase()
    {
        DBPath = GetDatabasePath();
    }
    
    /// <summary> Возвращает путь к БД. Если её нет в нужной папке на Андроиде, то копирует её с исходного apk файла. </summary>
    private static string GetDatabasePath(string fileName = standartFileName)
    {
    //#if UNITY_EDITOR
    //    return Path.Combine(Application.streamingAssetsPath, fileName);
    //#elif UNITY_STANDALONE
    #if UNITY_STANDALONE
        /*string filePath = Path.Combine(Application.dataPath, fileName);
        if(!File.Exists(filePath)) UnpackDatabase(filePath, fileName);
        return filePath;*/
        return Path.Combine(Application.streamingAssetsPath, fileName);
    #elif UNITY_ANDROID
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        if(!File.Exists(filePath)) UnpackDatabase(filePath, fileName);
        return filePath;
    #endif
    }

    /// <summary> Распаковывает базу данных в указанный путь. </summary>
    /// <param name="toPath"> Путь в который нужно распаковать базу данных. </param>
    //private static void UnpackDatabase(string toPath, string fileName)
    private static IEnumerator UnpackDatabase(string toPath, string fileName)
    {
        string fromPath = Path.Combine(Application.streamingAssetsPath, fileName);

        UnityWebRequest reader = new UnityWebRequest(fromPath);
        //WWW reader = new WWW(fromPath);
        //while (!reader.isDone) { }
        yield return reader.SendWebRequest();

        //File.WriteAllBytes(toPath, reader.bytes);
        if (reader.result == UnityWebRequest.Result.Success)
            File.WriteAllBytes(toPath, reader.downloadHandler.data);
    }

    /// <summary> Этот метод открывает подключение к БД. </summary>
    private static void OpenConnection()
    {
        connection = new SqliteConnection("Data Source=" + DBPath);
        command = new SqliteCommand(connection);
        connection.Open();
    }

    /// <summary> Этот метод закрывает подключение к БД. </summary>
    public static void CloseConnection()
    {
        connection.Close();
        command.Dispose();
    }

    /// <summary> Этот метод выполняет запрос query. </summary>
    /// <param name="query"> Собственно запрос. </param>
    public static void ExecuteQueryWithoutAnswer(string query)
    {
        OpenConnection();
        command.CommandText = query;
        command.ExecuteNonQuery();
        CloseConnection();
    }

    public static void ExecuteQueryWithoutAnswer(string query, string dataBaseName)
    {
        DBPath = GetDatabasePath(dataBaseName);

        OpenConnection();
        command.CommandText = query;
        command.ExecuteNonQuery();
        CloseConnection();
    }

    /// <summary> Этот метод выполняет запрос query и возвращает ответ запроса. </summary>
    /// <param name="query"> Собственно запрос. </param>
    /// <returns> Возвращает значение 1 строки 1 столбца, если оно имеется. </returns>
    public static string ExecuteQueryWithAnswer(string query)
    {
        OpenConnection();
        command.CommandText = query;
        var answer = command.ExecuteScalar();
        CloseConnection();

        if (answer != null) return answer.ToString();
        else return null;
    }

    public static string ExecuteQueryWithAnswer(string query, string dataBaseName)
    {
        DBPath = GetDatabasePath(dataBaseName);
        
        OpenConnection();
        command.CommandText = query;
        var answer = command.ExecuteScalar();
        CloseConnection();

        if (answer != null) return answer.ToString();
        else return null;
    }

    /// <summary> Этот метод возвращает таблицу, которая является результатом выборки запроса query. </summary>
    /// <param name="query"> Собственно запрос. </param>
    public static DataTable GetTable(string query)
    {
        OpenConnection();

        SqliteDataAdapter adapter = new SqliteDataAdapter(query, connection);

        DataSet DS = new DataSet();
        adapter.Fill(DS);
        adapter.Dispose();

        CloseConnection();

        return DS.Tables[0];
    }

    public static DataTable GetTable(string query, string dataBaseName)
    {
        DBPath = GetDatabasePath(dataBaseName);
        
        OpenConnection();

        SqliteDataAdapter adapter = new SqliteDataAdapter(query, connection);

        DataSet DS = new DataSet();
        adapter.Fill(DS);
        adapter.Dispose();

        CloseConnection();

        return DS.Tables[0];
    }

    public static void InsertOneRow(string newDateBaseName, DataTable dataRow, string tableName)
    {
        //записать одну строку в базу данных

        //пишем запрос на добавление новой строки в дб новую (активного словаря)
        //соотносим колонки и их значения строки первой таблицы(общего словаря) с колонками и значениями для запроса во второй (активный словарь)
        string insertQuery = $"INSERT INTO " + tableName + "(местополей) VALUES (местозначений);", columnsList = "", valuesList = "";
        foreach (DataColumn column in dataRow.Columns)
        {            
            if (columnsList == "")
            {
                columnsList = column.ToString();
                valuesList = "'" + dataRow.Rows[0][column].ToString() + "'";
            }
            else
            {
                columnsList = columnsList + "," + column.ToString();
                valuesList = valuesList + "," + "'" + dataRow.Rows[0][column].ToString() + "'";
            }
        }
        insertQuery = insertQuery.Replace("местополей", columnsList);        
        insertQuery = insertQuery.Replace("местозначений", valuesList);

        WorkWithDataBase.ExecuteQueryWithoutAnswer(insertQuery, newDateBaseName);
    }

    public static void InsertManyRow(string newDateBaseName, DataTable dataRows, string tableName) //куда, что(откуда)
    {
        //записать несколько строк в базу данных

        //пишем запрос на добавление новой строки в дб новую (активного словаря)
        //соотносим колонки и их значения строки первой таблицы(общего словаря) с колонками и значениями для запроса во второй (активный словарь)
        string insertQuery = $"INSERT INTO " + tableName + "(местополей) VALUES (местозначений);", columnsList = "", valuesList = "";


        foreach (DataRow row in dataRows.Rows)
        {
            foreach (DataColumn column in dataRows.Columns)
            {            
                if (columnsList == "")
                {
                    columnsList = column.ToString();
                    valuesList = "'" + row[column].ToString() + "'";
                }
                else
                {
                    columnsList = columnsList + "," + column.ToString();
                    valuesList = valuesList + "," + "'" + row[column].ToString() + "'";
                }                
            }
            insertQuery = insertQuery.Replace("местополей", columnsList);        
            insertQuery = insertQuery.Replace("местозначений", valuesList);
            
            WorkWithDataBase.ExecuteQueryWithoutAnswer(insertQuery, newDateBaseName);
            
            columnsList = "";
            valuesList = "";
            insertQuery = $"INSERT INTO " + tableName + "(местополей) VALUES (местозначений);";
        }
    }

}