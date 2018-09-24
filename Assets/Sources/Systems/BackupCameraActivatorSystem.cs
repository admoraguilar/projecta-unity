using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEngine;


/// <summary>
/// Class reponsible for activating the backup camera.
/// This is useful for loading screens because when we are loading
/// there's really no camera, so we use a back up instead to make the 
/// loading screen viewable.
/// </summary>
public class BackupCameraActivatorSystem : MonoBehaviour {
    [SerializeField] private Camera backupCamera;


    private void Start() {
        if(Camera.allCameras.Length > 1) backupCamera.gameObject.SetActive(false);
    }

    private void LateUpdate() {
        if(Camera.allCameras.Length <= 0) backupCamera.gameObject.SetActive(true);
        if(Camera.allCameras.Length > 1) backupCamera.gameObject.SetActive(false);
    }
}
