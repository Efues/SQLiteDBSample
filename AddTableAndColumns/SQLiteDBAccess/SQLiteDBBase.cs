using System;
using System.Data.SQLite;
using System.Data.Linq;
using System.IO;
using System.Data;

namespace SQLiteDBAccess
{
  public class SQLiteDBBase:IDisposable
  {
    protected readonly string _DBFile;
    protected readonly SQLiteConnection _connection;
    protected readonly DataContext _dataContext;

    protected DataContext DataContext { get { return _dataContext; } }

    public SQLiteDBBase(string DBFilePath)
    {
      _DBFile = DBFilePath;
      if (!File.Exists(_DBFile))
      {
        throw new FileNotFoundException("Cannot find DB File.", _DBFile);
      }
      var dataSource = _DBFile;
      if (dataSource.Substring(0, 2) == @"\\") dataSource = @"\\" + dataSource;
      var conStr = "DataSource=" + dataSource;
      //      conStr += " ReadOnly=True";
      _connection = new SQLiteConnection(conStr);
      _dataContext = new DataContext(_connection);
    }

    protected DataTable ReadToDataTable(string sql)
    {
      DataTable datatable = new DataTable();
      try
      {
        _connection.Open();
        using (var command = new SQLiteCommand(sql, _connection))
        {
          using (var reader = command.ExecuteReader())
          {
            datatable.Load(reader);
            reader.Close();
          }
        }
      }
      finally
      {
        _connection.Close();
      }

      return datatable;
    }

    protected int ExecuteNonQuery(string sql)
    {
      try
      {
        _connection.Open();
        using (var command = new SQLiteCommand(sql, _connection))
        {
          return command.ExecuteNonQuery();
        }
      }
      finally
      {
        _connection.Close();
      }
    }

    public void Dispose()
    {
      _dataContext.Dispose();
      _connection.Close();
      _connection.Dispose();
    }
  }
}
