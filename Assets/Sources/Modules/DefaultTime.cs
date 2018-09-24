using System;
using UnityEngine;


/// <summary>
/// This module handles Time-related values of the game.
/// </summary>
[CreateAssetMenu(fileName = "DefaultTime", menuName = "WEngine/Modules/ATime/DefaultTime")]
public class DefaultTime : ATime {
    public override long GetNumberedUtcNow() {
        string date = string.Concat(DateTime.UtcNow.Month,
                                    DateTime.UtcNow.Day,
                                    DateTime.UtcNow.Year,
                                    DateTime.UtcNow.Hour,
                                    DateTime.UtcNow.Minute,
                                    DateTime.UtcNow.Second);
        return Convert.ToInt64(date);
    }
    public override DateTime GetUtcNow() {
        return DateTime.UtcNow;
    }
}


public abstract class ATime : ScriptableObject {
    abstract public long GetNumberedUtcNow();
    abstract public DateTime GetUtcNow();
}