using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO issue: fire looks wrong in fog as it doesn't appear in density buffer

[RequireComponent(typeof(Camera))]
public class PostProcessing : MonoBehaviour {

    Camera cam;

    public Material fog;
	public Material edgeDetection;
    public Material visor;
    public Material blend;

    public bool drawEdges;
    public bool drawFog;
    public bool drawVisor;

    //TODO figure out if different settings for this, or have multiple cameras

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
        RenderTexture fogRender = RenderTexture.GetTemporary(source.descriptor);
        RenderTexture edgeRender = RenderTexture.GetTemporary(source.descriptor);

        // Putting in try/catch block to ensure RenderTextures release
        try
        {
            // TODO maybe have an ongoing texture, which combines effects as they're done?

            if (drawFog)
            {
                // Filter to get bright areas only
                Graphics.Blit(source, temp1, fog, 0);

                // Blur for bloom effect
                //Graphics.Blit(temp1, destination, mat, 1);
                Graphics.Blit(temp1, temp2, fog, 2);
                Graphics.Blit(temp2, temp1, fog, 3);
                fog.SetTexture("_BaseTex", temp1);

                // Add to base and apply fog
                Graphics.Blit(source, fogRender, fog, 4);
            }

            if (drawEdges)
            {
                // TODO further stuff with edges
                Graphics.Blit (source, edgeRender, edgeDetection);
            }

            RenderTexture toDistort;

            if(drawEdges && drawFog)
            {
                blend.SetTexture("_BaseTex", edgeRender);
                Graphics.Blit(fogRender, temp1, blend);
                toDistort = temp1;
            } else if (drawEdges)
            {
                blend.SetTexture("_BaseTex", edgeRender);
                Graphics.Blit(source, temp1, blend);
                toDistort = temp1;
            } else if (drawFog)
            {
                toDistort = fogRender;
            } else
            {
                toDistort = source;
            }

            if (drawVisor)
            {
                Graphics.Blit(toDistort, destination, visor);
            } else
            {
                Graphics.Blit(toDistort, destination);
            }

            RenderTexture.ReleaseTemporary(temp1);
            RenderTexture.ReleaseTemporary(temp2);
            RenderTexture.ReleaseTemporary(fogRender);
            RenderTexture.ReleaseTemporary(edgeRender);
        } catch
        {
            RenderTexture.ReleaseTemporary(temp1);
            RenderTexture.ReleaseTemporary(temp2);
            RenderTexture.ReleaseTemporary(fogRender);
            RenderTexture.ReleaseTemporary(edgeRender);
        }
    }
}
