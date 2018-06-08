using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO issue: fire looks wrong in fog as it doesn't appear in density buffer

[RequireComponent(typeof(Camera))]
public class PostProcessing : MonoBehaviour {

    Camera cam;

    public Material mat;    //HACK


	// Use this for initialization
	void Start () {
        cam = GetComponent<Camera>();
        cam.depthTextureMode = DepthTextureMode.DepthNormals;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
		RenderTexture temp1 = RenderTexture.GetTemporary (source.descriptor);
		RenderTexture temp2 = RenderTexture.GetTemporary (source.descriptor);


		// TODO render between different rendertextures using different passes to get effect needed

		//TODO try 3x3 blur, if not enough write some two-pass linear thing?

		// currently 1 is just fog, rewrite as more passes worked out

		// Filter to get bright areas only
		Graphics.Blit(source, temp1, mat, 0);

		// Blur for bloom effect
		//Graphics.Blit(temp1, destination, mat, 1);
		Graphics.Blit(temp1, temp2, mat, 2);
		Graphics.Blit (temp2, temp1, mat, 3);
		mat.SetTexture ("_BaseTex", temp1);

		// Add to base and apply fog
		Graphics.Blit(source, destination, mat, 4);

		RenderTexture.ReleaseTemporary(temp1);
		RenderTexture.ReleaseTemporary(temp2);
    }
}
