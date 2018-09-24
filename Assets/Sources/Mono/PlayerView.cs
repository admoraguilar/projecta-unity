using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using WEngine;


/// <summary>
/// Class responsible for:
/// * Syncing avatar changes across players.
/// </summary>
public class PlayerView : MonoBehaviour {
    [Inject] public ANetwork Network;
    [Inject] public AGameData GameData;
    
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform uiTransform;
    [SerializeField] private Text uiPlayerName;

    private AvatarData currentAvatar;
    private string avatarKey;
    private string userName;
    [SerializeField] private Vector3 m_originalCameraPos;
    [SerializeField] private Vector3 m_originalUiPos;
     
    private Transform thisTransform;
    private PhotonView thisPhotonView;


    private void LoadNewAvatar() {
        if(currentAvatar != null) Destroy(currentAvatar.gameObject);

        // If the avatarId doesn't exist, we load the default one
        AvatarData avatarData = null;
        if(!GameData.Container.Asset.Avatars.TryGetValue(avatarKey, out avatarData)) {
            avatarData = GameData.Container.Asset.Avatars["Default"];
        }

        currentAvatar = UnityExtension.ExtensionInstantiate(avatarData,
           (AvatarData avatar) => {
               // Configure avatar
               avatar.name = avatarKey;

               Transform t = avatar.GetComponent<Transform>();
               t.SetParent(thisTransform);

               t.localPosition = Vector3.zero;
               t.localRotation = Quaternion.identity;
           });

        if(Main.IsModuleAssignableFrom(Network, typeof(PhotonPUNNetwork))) {
            if(!thisPhotonView.isMine) return;
        }

        // Configure camera
        if(currentAvatar.CameraPosition) {
            cameraTransform.position = currentAvatar.CameraPosition.position;
        } else {
            cameraTransform.localPosition = m_originalCameraPos;
        }

        currentAvatar.gameObject.RunOnChildrenRecursive((go) => {
            go.layer = LayerMask.NameToLayer("CulledFromCamera");
        });
    }

    #region Network Events

    private void OnSetPlayerCustomProperties(object player, object changedProperties) {
        string newAvatarId = "";

        if(Main.IsModuleAssignableFrom(Network, typeof(PhotonPUNNetwork))) {
            PhotonPlayer p = (PhotonPlayer)player;
            if(this.thisPhotonView.owner != p) return;

            newAvatarId = (string)thisPhotonView.owner.CustomProperties[GameData.Key.Player.CustomPropertyAvatarIdKey];
        }

        if(Main.IsModuleAssignableFrom(Network, typeof(DefaultNetwork))) {
            newAvatarId = (string)Network.PlayerCustomProperties[GameData.Key.Player.CustomPropertyAvatarIdKey];
        }

        if(avatarKey != newAvatarId) {
            avatarKey = newAvatarId;
            LoadNewAvatar();

            Debug.Log("Avatar loaded: " + avatarKey);
        }
    }

    #endregion

    private void Awake() {
        thisTransform = GetComponent<Transform>();
        thisPhotonView = GetComponent<PhotonView>();
    }

    private void Start() {
        m_originalCameraPos = cameraTransform.localPosition;
        m_originalUiPos = uiTransform.localPosition;

        if(Main.IsModuleAssignableFrom(Network, typeof(PhotonPUNNetwork))) {
            avatarKey = (string)thisPhotonView.owner.CustomProperties[GameData.Key.Player.CustomPropertyAvatarIdKey];

            userName = thisPhotonView.owner.NickName;
            uiPlayerName.text = userName;
        }

        if(Main.IsModuleAssignableFrom(Network, typeof(DefaultNetwork))) {
            avatarKey = (string)Network.PlayerCustomProperties[GameData.Key.Player.CustomPropertyAvatarIdKey];

            userName = GameData.Transient.Player.DisplayName;
            uiPlayerName.text = userName;
        }

        LoadNewAvatar();
        Network.OnSetPlayerCustomProperties += OnSetPlayerCustomProperties;
    }

    private void OnDestroy() {
        Network.OnSetPlayerCustomProperties -= OnSetPlayerCustomProperties;
    }
}
