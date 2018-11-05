using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxTint : MonoBehaviour {

	private Skybox sb;
	private Material sbMaterial;
	private Transform player;

	private Color lightCol = new Color (.5f, .5f, .5f, .5f);
	private Color mediumCol = new Color (.35f, .35f, .35f, .5f);
	private Color darkCol = new Color (.2f, .2f, .2f, .5f);

	void Start () {
		sb = GetComponent<Skybox>();
		sbMaterial = sb.material;
		player = transform.parent;
	}
	
	void Update () {
		if (player.position.y > 50f && player.position.y < 100f) {
			sbMaterial.SetColor("_Tint", mediumCol);
		} else if (player.position.y >= 100f) {
			sbMaterial.SetColor("_Tint", darkCol);
		} else {
			sbMaterial.SetColor("_Tint", lightCol);
		}
	}
}

