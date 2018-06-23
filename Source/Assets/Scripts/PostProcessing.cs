using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO issue: fire looks wrong in fog as it doesn't appear in density buffer

[RequireComponent(typeof(Camera))]
public class PostProcessing : MonoBehaviour {

    // Subshader pass indices
    const int fogBrightnessPrefilter = 0;
    const int fogBoxBlur = 1;
    const int fogBlurHorizontal = 2;
    const int fogBlurVertical = 3;
    const int fogEffect = 4;
    const int edgeBlurHorizontal = 0;
    const int edgeBlurVertical = 1;
    const int edgeDetectColour = 2;
    const int edgeDetectDepth = 3;


    Camera cam;

    public Material fog;
	public Material edgeDetection;
    public Material visor;
    public Material blend;

    public bool drawEdges;
    public bool drawFog;
    public bool drawVisor;

    public bool edgeOnly;

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
                Graphics.Blit(source, temp1, fog, fogBrightnessPrefilter);

                // Blur for bloom effect
                //Graphics.Blit(temp1, destination, mat, fogBoxBlur);
                Graphics.Blit(temp1, temp2, fog, fogBlurHorizontal);
                Graphics.Blit(temp2, temp1, fog, fogBlurVertical);
                fog.SetTexture("_BaseTex", temp1);

                // Add to base and apply fog
                Graphics.Blit(source, fogRender, fog, fogEffect);
            }

            if (drawEdges)
            {
                //// 2 pass Gaussian blur
                //Graphics.Blit(source, temp1, edgeDetection, edgeBlurHorizontal);
                //Graphics.Blit(temp1, temp2, edgeDetection, edgeBlurVertical);
                // Detect edges
                //Graphics.Blit(temp2, edgeRender, edgeDetection, edgeDetectColour);

                // depth based edges
                Graphics.Blit(source, edgeRender, edgeDetection, edgeDetectDepth);
            }

            RenderTexture toDistort;

            if(drawEdges && drawFog)
            {
                blend.SetTexture("_BaseTex", edgeRender);
                Graphics.Blit(fogRender, temp1, blend);
                toDistort = temp1;
            } else if (drawEdges)
            {
                if (edgeOnly)
                {
                    toDistort = edgeRender;
                }
                else
                {
                    blend.SetTexture("_BaseTex", edgeRender);
                    Graphics.Blit(source, temp1, blend);
                    toDistort = temp1;
                }
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
