using System;
using UnityEngine;
using WEngine;


[Serializable]
public class ContainerData {
    // Constants
    public AssetContainerData Asset;

    // Editor-assignable instances
    public UiContainerData Ui;

    // Pure instances
    public SceneContainerData Scene = new SceneContainerData();
}
