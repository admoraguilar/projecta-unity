using UnityEngine;


public class AvatarData : MonoBehaviour {
    [Header("References")]
    public Transform CameraPosition;

    [Header("Info")]
    public string Id;
    public Sprite Banner;
    [Multiline] public string Info;

    public bool IsDebug;
}
