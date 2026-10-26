using Microsoft.Data.Sqlite;

namespace LineMonitor.APP.Data;

public class Database
{
    private readonly string _connectionString;
    public const string TimeFormat = "yyyy-MM-dd HH:mm:ss.fff";

    public Database(string filePath)
    {
        _connectionString = $"Data Source={filePath}";
    }

    public SqliteConnection Open()
    {
        var conn = new SqliteConnection(_connectionString);
        conn.Open();
        return conn;
    }

    public void CreateTables()
    {
        using var conn = Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = """
        CREATE TABLE IF NOT EXISTS alarm_log (
            id          INTEGER PRIMARY KEY AUTOINCREMENT,
            time        TEXT    NOT NULL,
            is_active   INTEGER NOT NULL,
            message     TEXT    NOT NULL,
            temperature REAL    NOT NULL
        );

        CREATE TABLE IF NOT EXISTS line_history (
            id          INTEGER PRIMARY KEY AUTOINCREMENT,
            time        TEXT    NOT NULL,
            running     INTEGER NOT NULL,
            alarm       INTEGER NOT NULL,
            temperature REAL    NOT NULL,
            count       INTEGER NOT NULL
        );

        CREATE INDEX IF NOT EXISTS ix_history_time ON line_history(time);
        """;
        cmd.ExecuteNonQuery();
    }
}