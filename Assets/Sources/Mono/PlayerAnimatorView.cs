using System.Collections;
using UnityEngine;
using WEngine;


/// <summary>
/// Class responsible for syncing animations of avatars.
/// </summary>
public class PlayerAnimatorView : MonoBehaviour {
    [Inject] public ANetwork Network;
    [Inject] public AInput Input;
    [Inject] public AGameData GameData;

    [SerializeField] private Animator animator;
    [SerializeField] private string verticalInputAnimKey = "Speed";
    [SerializeField] private string jumpInputAnimKey = "Jump";
    [SerializeField] private string emoteInputAnimKey = "Emote";

    [SerializeField] private float moveVerticalInput;
    [SerializeField] private bool jumpInput;
    [SerializeField] private bool emoteInput;

    private Coroutine emoteRoutine;

    private PhotonView thisPhotonView;


    private void UpdateInput() {
        if(Main.IsModuleAssignableFrom(Network, typeof(PhotonPUNNetwork))) {
            if(!thisPhotonView.isMine) return;
        }

        if(Input.GetButtonDown("Emote")) {
            GameData.Transient.Player.EmoteInput = true;
        }

        moveVerticalInput = GameData.Transient.Player.MoveVerticalInput;
        jumpInput = GameData.Transient.Player.JumpInput;
        emoteInput = GameData.Transient.Player.EmoteInput;
    }

    #region Network Events
    private void OnSetPlayerCustomProperties(object player, object changedProperties) {
        if(Main.IsModuleAssignableFrom(Network, typeof(PhotonPUNNetwork))) {
            PhotonPlayer p = (PhotonPlayer)player;
            if(this.thisPhotonView.owner != p) return;

            moveVerticalInput = (float)thisPhotonView.owner.CustomProperties[GameData.Key.Player.CustomPropertyMoveVerticalInputKey];
            jumpInput = (bool)thisPhotonView.owner.CustomProperties[GameData.Key.Player.CustomPropertyJumpInputKey];
            emoteInput = (bool)thisPhotonView.owner.CustomProperties[GameData.Key.Player.CustomPropertyEmoteInputKey];
        }
    }
    #endregion

    private void Awake() {
        thisPhotonView = GetComponent<PhotonView>();
    }

    private void Start() {
        Network.OnSetPlayerCustomProperties += OnSetPlayerCustomProperties;
    }

    private void Update() {
        UpdateInput();

        if(animator != null) {
            if(animator.GetCurrentAnimatorStateInfo(0).IsName(emoteInputAnimKey) && 
               animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .98f) {
                GameData.Transient.Player.EmoteInput = false;
            }

            // Set animator properties
            animator.SetFloat(verticalInputAnimKey, moveVerticalInput);
            animator.SetBool(jumpInputAnimKey, jumpInput);
            animator.SetBool(emoteInputAnimKey, emoteInput);
        } else {
            animator = GetComponentInChildren<Animator>();
        }
    }
}
