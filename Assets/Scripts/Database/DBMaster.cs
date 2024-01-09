using SimpleSQL;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
#endif

public class DBMaster : MonoBehaviour
{
    #region SingletonInit
    private static DBMaster instance = null;

    public static DBMaster Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);

        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

    }
    #endregion


    [TabGroup("references", "References")][SerializeField] private SimpleSQLManager _manager;

    private void Start()
    {
        //CreateTableGameLogs();
    }

    /// <summary>
    /// Resets the GameLogs table
    /// </summary>
    public void CreateTableGameLogs()
    {
        try
        {
            string deleteString = "DROP TABLE GameLogs";
            _manager.Execute(deleteString);
            Debug.Log("Deleted old GameLogs table");
        }
        catch (Exception)
        {
            Debug.Log("No prior GameLogs table existing");
        }

        string commandString = "CREATE TABLE GameLogs (ID INTEGER PRIMARY KEY," +
                                                        "StartTime DATETIME NOT NULL," +
                                                        "EndTime DATETIME NOT NULL," +
                                                        "AvgWPM INT NOT NULL," +
                                                        "AvgPrecision INT NOT NULL," +
                                                        "Difficulty INT NOT NULL," +
                                                        "Won BIT NOT NULL," +
                                                        "GoldWon INT NOT NULL," +
                                                        "KillCount INT NOT NULL)";
        _manager.Execute(commandString);
        Debug.Log("Created Table GameLogs");
    }
   
    /// <summary>
    /// Method inserting a log into the game log table
    /// </summary>
    /// <param name="avgwpm"></param>
    /// <param name="avgprecision"></param>
    /// <param name="difficulty"></param>
    /// <param name="won"></param>
    public void InsertIntoGameLogs(DateTime starttime, int avgwpm, int avgprecision, int difficulty, bool won, int goldwon, int killcount)
    {
        string sqlCommand = $"INSERT INTO GameLogs (StartTime, EndTime, AvgWPM, AvgPrecision, Difficulty, Won, GoldWon, KillCount) VALUES (?, ?, ?, ?, ?, ?, ?, ?)";

        _manager.Execute(sqlCommand, starttime, DateTime.Now, avgwpm, avgprecision, difficulty, won, goldwon, killcount);

    }
    /// <summary>
    /// Method querying the DB and returning the list of all GameLogs
    /// </summary>
    /// <returns></returns>
    public List<GameLogs> QueryAllGameLogs()
    {
        string sqlCommand = "SELECT * FROM GameLogs";

        List<GameLogs> list = _manager.Query<GameLogs>(sqlCommand);

        return list;
    }
   


}


//List<ClicksAndDate> logs = manager.Query<ClicksAndDate>(sql);

// manager.Execute(sql, counter, DateTime.Now);
