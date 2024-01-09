using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellLogs
{
    public int ID { get; set; }
    public string SpellName { get; set; }
    public DateTime CastTime { get; set; }
    public int WPM { get; set; }
    public int Precision { get; set; }
    public int GameID { get; set; }
}
