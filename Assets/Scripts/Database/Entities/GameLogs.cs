using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogs
{
    public int ID { get; set; }

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int AvgWPM { get; set; }
    public int AvgPrecision { get; set; }
    public int Difficulty { get; set; }
    public bool Won { get; set; }
    public int GoldWon { get; set; }
    public int KillCount { get; set; }
}
