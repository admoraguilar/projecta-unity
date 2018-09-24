using System;
using UnityEngine;
using WEngine;
using agora_gaming_rtc;


/// <summary>
/// Agora integration for AVoice module.
/// Note that Agora only works on an actual build, if you try to run
/// this on the editor or on other platforms it will complain that 
/// it can't find the required dll it needs to run.
/// </summary>
[CreateAssetMenu(fileName = "AgoraVoice", menuName = "WEngine/Modules/AVoice/AgoraVoice")]
public class AgoraVoice : AVoice, IInitializable {
    public int Priority { get { return 1000; } }

    public override string ChannelName { get { return channelName; } }
    public override string ChannelInfo { get { return channelInfo; } }
    public override uint UserId { get { return userId; } }
    public override bool IsPaused { get { return isPaused; } }
    public override bool IsMuted { get { return isMuted; } }

    [SerializeField] private string appId;

    private IRtcEngine rtcEngine;

    [Header("Debug")]
    [SerializeField] private string channelName;
    [SerializeField] private string channelInfo;
    [SerializeField] private uint userId;
    [SerializeField] private bool isPaused;
    [SerializeField] private bool isMuted;


    public override void JoinChannel(string name, string info, uint userId) {
#if !UNITY_EDITOR && UNITY_ANDROID
        _JoinChannel(name, info, userId);
#endif
    }

    private void _JoinChannel(string name, string info, uint userId) {
        rtcEngine.JoinChannel(name, info, userId);

        // We set some values only when we've successfully joined the
        // channel. This is useful for debugging
        IRtcEngine.JoinChannelSuccessHandler onJoinChannelSuccess = delegate { };
        onJoinChannelSuccess += (string channelName, uint uid, int elapsed) => {
            this.channelName = channelName;
            this.channelInfo = info;
            this.userId = uid;

            // We remove the event once we're done
            // so you can call this a one-shot event
            rtcEngine.OnJoinChannelSuccess -= onJoinChannelSuccess;

            Debug.Log("Join voice channel success: " + name);
        };

        rtcEngine.OnJoinChannelSuccess += onJoinChannelSuccess;
        Debug.Log("Join voice channel: " + name);
    }

    public override void Pause() {
        isPaused = true;

#if !UNITY_EDITOR && UNITY_ANDROID
        _Pause();
#endif
    }

    private void _Pause() {
        rtcEngine.Pause();
        Debug.Log("Voice pause");
    }

    public override void Resume() {
        isPaused = false;

#if !UNITY_EDITOR && UNITY_ANDROID
        _Resume();
#endif
    }

    private void _Resume() {
        rtcEngine.Resume();
        Debug.Log("Voice resume");
    }

    public override void LeaveChannel() {
#if !UNITY_EDITOR && UNITY_ANDROID
        _LeaveChannel();
#endif
    }

    private void _LeaveChannel() {
        rtcEngine.LeaveChannel();

        // One-shot event for leave channel
        IRtcEngine.LeaveChannelHandler onLeaveChannel = delegate { };
        onLeaveChannel = (RtcStats stats) => {
            Debug.Log("Left voice channel success: " + channelName);

            this.channelName = "";
            this.channelInfo = "";
            this.userId = 0;

            rtcEngine.OnLeaveChannel -= onLeaveChannel;
        };

        rtcEngine.OnLeaveChannel += onLeaveChannel;
        Debug.Log("Left voice channel: " + name);
    }

    public override void MuteLocalAudioStream(bool isMute) {
        isMuted = isMute;

#if !UNITY_EDITOR && UNITY_ANDROID
        _MuteLocalAudioStream(isMuted);
#endif
    }

    private void _MuteLocalAudioStream(bool isMute) {
        rtcEngine.MuteLocalAudioStream(isMute);
        Debug.Log("Voice mute");
    }

    private void Update() {
#if !UNITY_EDITOR && UNITY_ANDROID
        _Update();
#endif
    }

    private void _Update() {
        if(rtcEngine != null) rtcEngine.Poll();
    }

    public void Initialize() {
#if !UNITY_EDITOR && UNITY_ANDROID
        _Initialize();
#endif

        var agoraVoiceMono = new GameObject("AgoraVoiceMono").ExtensionAddComponent<AgoraVoiceMono>();
        DontDestroyOnLoad(agoraVoiceMono.gameObject);
        agoraVoiceMono.Initialize();

        agoraVoiceMono.OnUpdate += Update;
    }

    private void _Initialize() {
        rtcEngine = IRtcEngine.GetEngine(appId);
        rtcEngine.SetLogFilter(LOG_FILTER.INFO);
        rtcEngine.SetChannelProfile(CHANNEL_PROFILE.GAME_FREE_MODE);
    }
}


public class AgoraVoiceMono : MonoBehaviour {
    public event Action OnUpdate = delegate { };

    [Inject] public AVoice Voice;
    [Inject] public AInput Input;
    [Inject] public ANetwork Network;


    #region Network Events

    private void OnJoinRoom() {
        Voice.JoinChannel(Network.RoomName, "", 0);

        // If we're muted
        //if(Voice.IsMuted) Voice.Pause();
        Voice.MuteLocalAudioStream(Voice.IsMuted);
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
        OnUpdate();

        if(Input.GetButtonDown("Talk")) {
            Voice.MuteLocalAudioStream(!Voice.IsMuted);

            //if(Voice.IsMuted) Voice.Pause();
            //else Voice.Resume();
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
