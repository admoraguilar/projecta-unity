using UnityEngine;
using WEngine;


/// <summary>
/// Making sure that non-local objects will not have,
/// scripts that are supposedly for local objects.
/// </summary>
public class NetworkedObjectsHandler : MonoBehaviour {
    [Inject] public ANetwork Network;

    [SerializeField] private Component[] componenetsToDelete;
    [SerializeField] private GameObject[] gameObjectsToDelete;

    private PhotonView photonView;


    private void Awake() {
        photonView = GetComponent<PhotonView>();

        // We're injecting manually because auto-injection happens
        // after Awake()
        Main.Inject(this);

        // Disable when we're not using PUNNetwork
        if(!Main.IsModuleAssignableFrom(Network, typeof(PhotonPUNNetwork))) {
            enabled = false;
        }
    }

    private void Start () {
        if(!photonView.isMine) {
            foreach(Component c in componenetsToDelete) Destroy(c);
            foreach(GameObject go in gameObjectsToDelete) Destroy(go);
        }
	}
}
