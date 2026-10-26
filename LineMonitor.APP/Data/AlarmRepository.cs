using LineMonitor.APP.Models;

namespace LineMonitor.APP.Data;

public class AlarmRepository
{
    private readonly Database _db;

    public AlarmRepository(Database db)
    {
        _db = db;
    }

    public void Insert(AlarmEvent a)
    {
        using var conn = _db.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = """
            INSERT INTO alarm_log (time, is_active, message, temperature)
            VALUES (@time, @isActive, @message, @temperature)
            """;
        cmd.Parameters.AddWithValue("@time", a.Time.ToString(Database.TimeFormat));
        cmd.Parameters.AddWithValue("@isActive", a.IsActive ? 1 : 0);
        cmd.Parameters.AddWithValue("@message", a.Message);
        cmd.Parameters.AddWithValue("@temperature", a.Temperature);
        cmd.ExecuteNonQuery();
    }

    public List<AlarmEvent> GetRecent(int count)
    {
        using var conn = _db.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = """
        SELECT time, is_active, message, temperature
        FROM alarm_log
        ORDER BY id DESC
        LIMIT @count
        """;
        cmd.Parameters.AddWithValue("@count", count);

        var list = new List<AlarmEvent>();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new AlarmEvent
            {
                Time = DateTime.ParseExact(reader.GetString(0), Database.TimeFormat, null),
                IsActive = reader.GetInt32(1) == 1,
                Message = reader.GetString(2),
                Temperature = reader.GetDouble(3)
            });
        }
        return list;
    }
}