using Microsoft.Data.Sqlite;

namespace OpencartTests.Helpers;

// Manages SQLite connection and executes SQL commands
public class DbHelper : IDisposable
{
    private SqliteConnection? _connection;
    private bool _disposed;

    // Opens a new SQLite connection
    public SqliteConnection OpenConnection(string connectionString)
    {
        _connection = new SqliteConnection(connectionString);
        _connection.Open();
        return _connection;
    }

    // Runs SELECT query — returns rows via DataReader
    public SqliteDataReader ExecuteQuery(string sql)
    {
        EnsureConnected();
        // Don't dispose the command here, it would close the reader
        var command = _connection!.CreateCommand();
        command.CommandText = sql;
        return command.ExecuteReader();
    }

    // Runs query that returns a single value (COUNT, MAX, etc.)
    public object? ExecuteScalar(string sql)
    {
        EnsureConnected();
        using var command = _connection!.CreateCommand();
        command.CommandText = sql;
        return command.ExecuteScalar();
    }

    // Runs INSERT, UPDATE, DELETE — returns number of rows affected
    public int ExecuteNonQuery(string sql)
    {
        EnsureConnected();
        using var command = _connection!.CreateCommand();
        command.CommandText = sql;
        return command.ExecuteNonQuery();
    }

    // Closes and disposes the connection
    public void CloseConnection()
    {
        if (_connection != null)
        {
            _connection.Close();
            _connection.Dispose();
            _connection = null;
        }
    }

    private void EnsureConnected()
    {
        if (_connection == null || _connection.State != System.Data.ConnectionState.Open)
            throw new InvalidOperationException(
                "No open database connection. Call OpenConnection() first.");
    }

    // IDisposable — ensures connection is always cleaned up
    public void Dispose()
    {
        if (!_disposed)
        {
            CloseConnection();
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }
}
