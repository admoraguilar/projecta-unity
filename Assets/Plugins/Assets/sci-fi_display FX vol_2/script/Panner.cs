using UnityEngine;
using System.Collections;

public class Panner : MonoBehaviour {

	[SerializeField] private Renderer panMat;
    [SerializeField] private Shader shaderType;

	public float Uspeed;
	public float Vspeed;

	private float uP;
	private float vP;

	private float Ttt;

    private Material materialToPan;


    private void Awake() {
        foreach(Material mat in panMat.materials) {
            if(mat.shader == shaderType) {
                materialToPan = mat; 
            }
        }
    }

    void Update () {
		uP = Uspeed * Time.time;
		vP = Vspeed * Time.time;

        materialToPan.mainTextureOffset = new Vector2(uP, vP);
    }
}
