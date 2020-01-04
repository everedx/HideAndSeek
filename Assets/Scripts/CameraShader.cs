using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShader : MonoBehaviour
{
    public Shader shader;
    public float vignetteSizeMax = 2f;
    public float vignetteSizeMin = 1f;
    public float vignetteSmoothness = 0.6f;
    public float vignetteEdgeRound = 8f;


    private bool growing;
    public float vignetteSize;
    private int enemiesChasing;
    private Material material;

    // Use this for initialization
    void Start()
    {
        growing = true;
        enemiesChasing = 0;
        vignetteSize = 0;
        material = new Material(shader);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        material.SetFloat("u_vignette_size", vignetteSize);
        material.SetFloat("u_vignette_smoothness", vignetteSmoothness);
        material.SetFloat("u_vignette_edge_round", vignetteEdgeRound);
        Graphics.Blit(source, destination, material);
    }

    private void Update()
    {
       // material.EnableKeyword("_NORMALMAP");
       // Debug.Log(enemiesChasing);
        if (enemiesChasing > 0)
        {
            if (vignetteSize < vignetteSizeMin)
            {
                vignetteSize = vignetteSizeMin;
                //vignetteSize = vignetteSize + Time.deltaTime;
                growing = true;
            }
            else
            {
                if (growing)
                    vignetteSize = vignetteSize + Time.deltaTime;
                else
                    vignetteSize = vignetteSize - Time.deltaTime;

                if (vignetteSize > vignetteSizeMax)
                    growing = false;
                if (vignetteSize < vignetteSizeMin)
                    growing = true;
                //Debug.Log(vignetteSize);
            } 
        }
        else 
        {
            if(vignetteSize > 0)
                vignetteSize = vignetteSize - Time.deltaTime;
            growing = true;
            //Debug.Log(vignetteSize);

        }
    }

    public void changeEnemiesChasing(int value)
    {
        enemiesChasing = enemiesChasing + value;
    }



}
