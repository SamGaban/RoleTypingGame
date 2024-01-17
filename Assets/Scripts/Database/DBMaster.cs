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

/// <summary>
/// This class is responsible for managing the game database.
/// </summary>
public class DBMaster : MonoBehaviour
{
    #region SingletonInit
    private static DBMaster instance = null;

    public static DBMaster Instance
    {
        get { return instance; }
    }

    /// <summary>
    /// This method is called when the object is initialized.
    /// It checks if an instance of the object already exists. If not, it sets the instance to this object and marks it to not be destroyed when loading a new scene.
    /// If an instance already exists and it's not the same as this object, it destroys this object.
    /// </summary>
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
#if !UNITY_WEBGL

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
#endif
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
#if !UNITY_WEBGL
        string sqlCommand = $"INSERT INTO GameLogs (StartTime, EndTime, AvgWPM, AvgPrecision, Difficulty, Won, GoldWon, KillCount) VALUES (?, ?, ?, ?, ?, ?, ?, ?)";

        _manager.Execute(sqlCommand, starttime, DateTime.Now, avgwpm, avgprecision, difficulty, won, goldwon, killcount);
#endif
    }

    /// <summary>
    /// Method querying the DB and returning the list of all GameLogs
    /// </summary>
    /// <returns></returns>
    public List<GameLogs> QueryAllGameLogs()
    {
#if !UNITY_WEBGL
        string sqlCommand = "SELECT * FROM GameLogs";

        List<GameLogs> list = _manager.Query<GameLogs>(sqlCommand);

        return list;
#else
        return new List<GameLogs>();
#endif
    }
   


}


//List<ClicksAndDate> logs = manager.Query<ClicksAndDate>(sql);

// manager.Execute(sql, counter, DateTime.Now);
