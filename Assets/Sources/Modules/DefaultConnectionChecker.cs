using UnityEngine;


/// <summary>
/// This module handles checking of all connections the game makes.
/// Useful for having a centralized checking of connection.
/// </summary>
[CreateAssetMenu(fileName = "DefaultConnectionChecker", menuName = "WEngine/Modules/AConnectionChecker/DefaultConnectionChecker")]
public class DefaultConnectionChecker : AConnectionChecker {
    public override bool IsCheckingConnection { get { return false; } }
    public override bool IsAllConnectionOk { get { return true; } }
    public override bool IsInternetOk { get { return true; } }
    public override bool IsPlayerDataServiceOk { get { return true; } }
    public override bool IsNetworkServiceOk { get { return true; } }
}


public abstract class AConnectionChecker : ScriptableObject {
    abstract public bool IsCheckingConnection { get; }
    abstract public bool IsAllConnectionOk { get; }
    abstract public bool IsInternetOk { get; }
    abstract public bool IsPlayerDataServiceOk { get; }
    abstract public bool IsNetworkServiceOk { get; }
}