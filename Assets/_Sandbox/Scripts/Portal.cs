using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEngine;


public class Portal : MonoBehaviour {
    [SerializeField] private string sceneName;

    [Inject] public ANetwork Network;


    private IEnumerator OnPortal() {
        if(Network.IsInRoom) Network.LeaveRoom(false);
        yield return new WaitWhile(() => Network.IsInRoom);
        yield return new WaitWhile(() => !Network.IsInLobby);
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("OnPortal: " + sceneName);
        StartCoroutine(OnPortal());
    }
}
