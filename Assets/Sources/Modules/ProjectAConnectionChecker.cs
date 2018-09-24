using System;
using System.Collections;
using UnityEngine;
using WEngine;
using GameSparks.Core;


/// <summary>
/// ProjectA specific implementation for DefaultConnectionChecker module.
/// </summary>
[CreateAssetMenu(fileName = "ProjectAConnectionChecker", menuName = "WEngine/Modules/AConnectionChecker/ProjectAConnectionChecker")]
public class ProjectAConnectionChecker : AConnectionChecker, IInitializable {
    public int Priority { get { return 1000; } }

    public override bool IsCheckingConnection { get { return isCheckingConnection; } }
    public override bool IsInternetOk { get { return isInternetOk; } }
    public override bool IsPlayerDataServiceOk { get { return isPlayerDataServiceOk; } }
    public override bool IsNetworkServiceOk { get { return isNetworkServiceOk; } }
    public override bool IsAllConnectionOk { get { return isAllConnectionOk; } }

    [Header("Data")]
    [SerializeField] private float checkInterval = 5f;

    [Header("Status")]
    [SerializeField] private bool isCheckingConnection;
    [SerializeField] private bool isInternetOk;
    [SerializeField] private bool isPlayerDataServiceOk;
    [SerializeField] private bool isNetworkServiceOk;
    [SerializeField] private bool isAllConnectionOk;


    public void CheckConnection(Action<bool> result) {
        if(isCheckingConnection || isAllConnectionOk) return; 

        ActionQueue action = new ActionQueue("ProjectAConnectionChecker.CheckConnection");
        action.AddAction(_CheckConnection(result));
    }

    private IEnumerator _CheckConnection(Action<bool> result) {
        isCheckingConnection = true;

        while(!isAllConnectionOk) {
            // Check internet
            WWW www = new WWW("http://google.com");
            yield return www;
            isInternetOk = www.error == null;

            // Check services
            isPlayerDataServiceOk = GS.Available;
            isNetworkServiceOk = PhotonNetwork.connected;

            if(isInternetOk && isPlayerDataServiceOk && isNetworkServiceOk) isAllConnectionOk = true;

            result(isAllConnectionOk);

            // We check again after an interval if connections are still not ok
            if(!isAllConnectionOk) yield return new WaitForSeconds(checkInterval);
        }

        isCheckingConnection = false;
    }

    public void Initialize() {
        CheckConnection((success) => {
            if(success) Debug.Log("Connection successful");
        });
    }
}