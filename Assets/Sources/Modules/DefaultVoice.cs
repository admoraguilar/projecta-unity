using System;
using UnityEngine;
using WEngine;


[CreateAssetMenu(fileName = "DefaultVoice", menuName = "WEngine/Modules/AVoice/DefaultVoice")]
public class DefaultVoice : AVoice, IInitializable {
    public int Priority { get { return 1000; } }

    public override string ChannelName { get { return channelName; } }
    public override string ChannelInfo { get { return channelInfo; } }
    public override uint UserId { get { return userId; } }
    public override bool IsPaused { get { return isPaused; } }
    public override bool IsMuted { get { return isMuted; } }

    [Header("Debug")]
    [SerializeField] private string channelName;
    [SerializeField] private string channelInfo;
    [SerializeField] private uint userId;
    [SerializeField] private bool isPaused;
    [SerializeField] private bool isMuted;


    public override void JoinChannel(string name, string info, uint userId) {
        channelName = name;
        channelInfo = info;
        this.userId = userId;
    }

    public override void Pause() {
        isPaused = true;
    }

    public override void Resume() {
        isPaused = false;
    }

    public override void LeaveChannel() {
        channelName = "";
        channelInfo = "";
        userId = 0;
    }

    public override void MuteLocalAudioStream(bool isMute) {
        isMuted = isMute;
    }

    public void Initialize() {
        var defaultVoiceMono = new GameObject("DefaultVoiceMono").ExtensionAddComponent<DefaultVoiceMono>();
        DontDestroyOnLoad(defaultVoiceMono.gameObject);
        defaultVoiceMono.Initialize();
    }
}


public abstract class AVoice : ScriptableObject {
    abstract public string ChannelName { get; }
    abstract public string ChannelInfo { get; }
    abstract public uint UserId { get; }
    abstract public bool IsPaused { get; }
    abstract public bool IsMuted { get; }

    abstract public void JoinChannel(string channelName, string info, uint userId);
    abstract public void Pause();
    abstract public void Resume();
    abstract public void LeaveChannel();
    abstract public void MuteLocalAudioStream(bool isMute);
}


public class DefaultVoiceMono : MonoBehaviour {
    [Inject] public AVoice Voice;
    [Inject] public AInput Input;
    [Inject] public ANetwork Network;


    #region Network Events

    private void OnJoinRoom() {
        Voice.JoinChannel(Network.RoomName, "", 0);

        // If we're muted
        if(Voice.IsMuted) Voice.Pause();
    }

    private void OnLeaveRoom() {
        Voice.LeaveChannel();
    }

    #endregion

    public void Initialize() {
        Network.OnJoinRoom += OnJoinRoom;
        Network.OnLeaveRoom += OnLeaveRoom;
    }

    private void Update() {
        if(Input.GetButtonDown("Talk")) {
            Voice.MuteLocalAudioStream(!Voice.IsMuted);

            if(Voice.IsMuted) Voice.Pause();
            else Voice.Resume();
        }
    }

    private void OnApplicationPause(bool pause) {
        if(pause) {
            Voice.Pause();
        } else {
            Voice.Resume();
        }
    }

    private void OnApplicationQuit() {
        Voice.LeaveChannel();
    }
}
