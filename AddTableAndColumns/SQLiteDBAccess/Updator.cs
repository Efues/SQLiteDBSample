using System;
using System.Collections.Generic;

namespace SQLiteDBAccess
{
  public class Updator
  {
    private string _dbFilePath;

    public Updator(string dbFilePath)
    {
      this._dbFilePath = dbFilePath;
    }

    public void Execute()
    {
      AddTableIfNotExist();
      AddColumnIfNotExist();
    }

    private void AddTableIfNotExist()
    {
      var addingTableName = "Customers";
      var columnList = new Dictionary<string, string>()
      {
        { "Name", "TEXT" },
        { "Address", "TEXT" },
      };

      using (var db = new SamepleDB(_dbFilePath))
      {
        var tableList = db.SelectTableList();
        if (!tableList.Contains(addingTableName))
        {
          db.CreateTable(addingTableName, columnList);
        }
        else
        {
          return;
        }

        tableList = db.SelectTableList();
        if (tableList.Contains(addingTableName))
        {
          Console.WriteLine($"{addingTableName} table is added successfully.");
        }
        else
        {
          Console.WriteLine($"Failed to add {addingTableName} table.");
        }
      }
    }

    private void AddColumnIfNotExist()
    {
      var targetTableName = "Products";
      var addingColumn = new KeyValuePair<string, string>("Stock", "INTEGER");

      using (var db = new SamepleDB(_dbFilePath))
      {
        var columnList = db.SelectColumnList(targetTableName);
        if (!columnList.Contains(addingColumn.Key))
        {
          db.AddColumn(targetTableName, addingColumn);
        }
        else
        {
          return;
        }

        columnList = db.SelectColumnList(targetTableName);
        if (columnList.Contains(addingColumn.Key))
        {
          Console.WriteLine($"{addingColumn.Key} column is added successfully.");
        }
        else
        {
          Console.WriteLine($"Failed to add {addingColumn.Key} column.");
        }
      }
    }
  }
}
