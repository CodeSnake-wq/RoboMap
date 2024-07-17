using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using robot_controller_api.Models;
using Npgsql;
namespace robot_controller_api.Persistence;

public class MapADO : IMapDataAccess
{
    // Connection string is usually set in a config file for the ease of change.
    private const string CONNECTION_STRING = "Host=localhost;Username=postgres;Password=12345678;Database=sit331";
    public List<Map> GetAllMaps()
    {
        var maps = new List<Map>();
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();
        using var cmd = new NpgsqlCommand("SELECT * FROM map", conn);
        using var dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            // read values off the data reader and create a new map here and then add it to the result list.
            var id = (int)dr["id"];
            int columns = (int)dr["columns"];
            int rows = (int)dr["rows"];
            var name = (string)dr["Name"];
            string? descr = dr["description"] as string;
            DateTime createddate = (DateTime)dr["createddate"];
            DateTime modifieddate = (DateTime)dr["modifieddate"];
            bool issquare = (bool)dr["is_square"];

            maps.Add(new Map(id, name, rows, columns, descr, createddate, modifieddate, issquare));
        }
        return maps;
    }

    public List<Map> GetSquareMapsOnly()
    {
        var maps = new List<Map>();
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();
        using var cmd = new NpgsqlCommand("SELECT * FROM map", conn);
        using var dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            bool isSquare = (bool)dr["is_square"];
            if (isSquare)
            {
                // read values off the data reader and create a new map here and then add it to the result list.
                var id = (int)dr["id"];
                int columns = (int)dr["columns"];
                int rows = (int)dr["rows"];
                var name = (string)dr["Name"];
                string? descr = dr["description"] as string;
                DateTime createddate = (DateTime)dr["createddate"];
                DateTime modifieddate = (DateTime)dr["modifieddate"];

                maps.Add(new Map(id, name, rows, columns, descr, createddate, modifieddate, isSquare));
            }
        }
        return maps;
    }

    public Map GetMapById(int id)
    {
        Map map = null;
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();
        using var cmd = new NpgsqlCommand("SELECT * FROM map where id = @Id", conn);
        cmd.Parameters.AddWithValue("Id", id);
        using var dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            var Id = (int)dr["id"];
            int columns = (int)dr["columns"];
            int rows = (int)dr["rows"];
            var name = (string)dr["Name"];
            string? descr = (string)dr["description"];
            DateTime createddate = (DateTime)dr["createddate"];
            DateTime modifieddate = (DateTime)dr["modifieddate"];
            bool issquare = (bool)dr["is_square"];

            map = new Map(id, name, rows, columns, descr, createddate, modifieddate, issquare);
        }
        return map;
    }

    public Map AddRobotMap(Map newMap)
    {
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();
        using var cmd = new NpgsqlCommand("INSERT INTO map (columns, rows, Name, description, createddate, modifieddate) VALUES (@columns, @rows, @Name, @description, @createddate, @modifieddate) RETURNING id", conn);
        cmd.Parameters.AddWithValue("columns", newMap.Columns);
        cmd.Parameters.AddWithValue("rows", newMap.Rows);
        cmd.Parameters.AddWithValue("Name", newMap.Name);
        cmd.Parameters.AddWithValue("description", newMap.Description);
        cmd.Parameters.AddWithValue("createddate", DateTime.Now);
        cmd.Parameters.AddWithValue("modifieddate", DateTime.Now);

        int i = (int)cmd.ExecuteScalar();
        return GetMapById(i);
    }

    public Map UpdateRobotMap(int id, Map newMap)
    {
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();
        using var cmd = new NpgsqlCommand("UPDATE map SET columns = @columns, rows = @rows, Name = @Name, description = @description, modifieddate = @modifieddate WHERE id = @id RETURNING id", conn);
        cmd.Parameters.AddWithValue("@columns", newMap.Columns);
        cmd.Parameters.AddWithValue("@rows", newMap.Rows);
        cmd.Parameters.AddWithValue("@Name", newMap.Name);
        cmd.Parameters.AddWithValue("@description", newMap.Description);
        cmd.Parameters.AddWithValue("@modifieddate", DateTime.Now);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();

        return GetMapById(id);
    }

    public int DeleteRobotMap(int id)
    {
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();
        using var cmd = new NpgsqlCommand("DELETE FROM map WHERE id = @Id RETURNING id", conn);
        cmd.Parameters.AddWithValue("@Id", id);
        return (int)cmd.ExecuteScalar();
    }

    public bool CheckCoordinate(int id, int x, int y)
    {
        Map map = GetMapById(id);
        if (x < map.Columns && y < map.Rows)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}