using LineMonitor.APP.Models;

namespace LineMonitor.APP.Data;

public class HistoryRepository
{
    private readonly Database _db;

    public HistoryRepository(Database db)
    {
        _db = db;
    }

    public void Insert(LineStatus s)
    {
        using var conn = _db.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = """
            INSERT INTO line_history (time, running, alarm, temperature, count)
            VALUES (@time, @running, @alarm, @temperature, @count)
            """;
        cmd.Parameters.AddWithValue("@time", s.Time.ToString(Database.TimeFormat));
        cmd.Parameters.AddWithValue("@running", s.Running ? 1 : 0);
        cmd.Parameters.AddWithValue("@alarm", s.Alarm ? 1 : 0);
        cmd.Parameters.AddWithValue("@temperature", s.Temperature);
        cmd.Parameters.AddWithValue("@count", (long)s.Count);
        cmd.ExecuteNonQuery();
    }

    public List<LineStatus> GetRange(DateTime from, DateTime to)
    {
        using var conn = _db.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = """
        SELECT time, running, alarm, temperature, count
        FROM line_history
        WHERE time >= @from AND time <= @to
        ORDER BY time
        """;
        cmd.Parameters.AddWithValue("@from", from.ToString(Database.TimeFormat));
        cmd.Parameters.AddWithValue("@to", to.ToString(Database.TimeFormat));

        var list = new List<LineStatus>();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new LineStatus
            {
                Time = DateTime.ParseExact(reader.GetString(0), Database.TimeFormat, null),
                Running = reader.GetInt32(1) == 1,
                Alarm = reader.GetInt32(2) == 1,
                Temperature = reader.GetDouble(3),
                Count = (uint)reader.GetInt64(4)
            });
        }
        return list;
    }
}