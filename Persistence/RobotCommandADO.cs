using Npgsql;
using robot_controller_api.Models;

namespace robot_controller_api.Persistence;
public class RobotCommandADO : IRobotCommandDataAccess
{
    // Connection string is usually set in a config file for the ease of change.
    private const string CONNECTION_STRING = "Host=localhost;Username=postgres;Password=12345678;Database=sit331";
    public List<RobotCommand> GetAllRobotCommands()
    {
        var robotCommands = new List<RobotCommand>();
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();
        using var cmd = new NpgsqlCommand("SELECT * FROM robotcommand", conn);
        using var dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            // read values off the data reader and create a new robotCommand here and then add it to the result list.
            var id = (int)dr["id"];
            var name = (string)dr["Name"];
            string descr = (string)dr["description"];
            bool ismovecommand = (bool)dr["ismovecommand"];
            DateTime createddate = (DateTime)dr["createddate"];
            DateTime modifieddate = (DateTime)dr["modifieddate"];

            robotCommands.Add(new RobotCommand(id, name, descr, ismovecommand, createddate, modifieddate));
        }
        return robotCommands;
    }

    public List<RobotCommand> GetMoveCommandsOnly()
    {
        var robotCommands = new List<RobotCommand>();
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();
        using var cmd = new NpgsqlCommand("SELECT * FROM robotcommand", conn);
        using var dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            bool isMoveCommand = (bool)dr["ismovecommand"];
            if (isMoveCommand)
            {
                // read values off the data reader and create a new map here and then add it to the result list.
                var id = (int)dr["id"];
                var name = (string)dr["Name"];
                string descr = (string)dr["description"];
                bool ismovecommand = (bool)dr["ismovecommand"];
                DateTime createddate = (DateTime)dr["createddate"];
                DateTime modifieddate = (DateTime)dr["modifieddate"];

                robotCommands.Add(new RobotCommand(id, name, descr, ismovecommand, createddate, modifieddate));
            }
        }
        return robotCommands;
    }

    public RobotCommand GetRobotCommandById(int id)
    {
        RobotCommand robotCommand = null;
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();
        using var cmd = new NpgsqlCommand("SELECT * FROM robotcommand where id = @Id", conn);
        cmd.Parameters.AddWithValue("Id", id);
        using var dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            var Id = (int)dr["id"];
            var name = (string)dr["Name"];
            string descr = (string)dr["description"];
            bool ismovecommand = (bool)dr["ismovecommand"];
            DateTime createddate = (DateTime)dr["createddate"];
            DateTime modifieddate = (DateTime)dr["modifieddate"];

            robotCommand = new RobotCommand(Id, name, descr, ismovecommand, createddate, modifieddate);
        }
        return robotCommand;
    }

    public RobotCommand AddRobotCommand(RobotCommand newRobotCommand)
    {
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();
        using var cmd = new NpgsqlCommand("INSERT INTO robotcommand (Name, description, ismovecommand, createddate, modifieddate) VALUES (@Name, @description, @ismovecommand, @createddate, @modifieddate) RETURNING id", conn);
        cmd.Parameters.AddWithValue("Name", newRobotCommand.Name);
        cmd.Parameters.AddWithValue("description", newRobotCommand.Description);
        cmd.Parameters.AddWithValue("ismovecommand", newRobotCommand.IsMoveCommand);
        cmd.Parameters.AddWithValue("createddate", DateTime.Now);
        cmd.Parameters.AddWithValue("modifieddate", DateTime.Now);

        int i = (int)cmd.ExecuteScalar();
        return GetRobotCommandById(i);
    }

    public RobotCommand UpdateRobotCommand(int id, RobotCommand newRobotCommand)
    {
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();
        using var cmd = new NpgsqlCommand("UPDATE robotcommand SET Name = @Name, description = @description, ismovecommand =@ismovecommand, modifieddate = @modifieddate WHERE id = @id RETURNING id", conn);
        cmd.Parameters.AddWithValue("@Name", newRobotCommand.Name);
        cmd.Parameters.AddWithValue("@description", newRobotCommand.Description);
        cmd.Parameters.AddWithValue("@ismovecommand", newRobotCommand.IsMoveCommand);
        cmd.Parameters.AddWithValue("@modifieddate", DateTime.Now);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();

        return GetRobotCommandById(id);
    }

    public int DeleteRobotCommand(int id)
    {
        using var conn = new NpgsqlConnection(CONNECTION_STRING);
        conn.Open();
        using var cmd = new NpgsqlCommand("DELETE FROM robotcommand WHERE id = @Id RETURNING id", conn);
        cmd.Parameters.AddWithValue("@Id", id);
        return (int)cmd.ExecuteScalar();
    }
}