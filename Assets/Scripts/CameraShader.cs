using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class CameraShader : MonoBehaviour
{
    
    //public float vignetteSizeMax = 2f;
    //public float vignetteSizeMin = 1f;
    //public float vignetteSmoothness = 0.6f;
    //public float vignetteEdgeRound = 8f;


    private bool growing;
    //public float vignetteSize;
    private int enemiesChasing;
    private Light2D[] lightCharacter;
    private float valueColors;
   //Material material;

    // Use this for initialization
    void Start()
    {
        growing = true;
        //enemiesChasing = 0;
        //vignetteSize = 0;

        //material = new Material(shader);
        lightCharacter = GameObject.FindGameObjectWithTag("Player").GetComponentsInChildren<Light2D>() ;
    }

    //private void OnRenderImage(RenderTexture source, RenderTexture destination)
    //{
    //    material.SetFloat("_u_vignette_size", vignetteSize);
    //    material.SetFloat("_u_vignette_smoothness", vignetteSmoothness);
    //    material.SetFloat("_u_vignette_edge_round", vignetteEdgeRound);
    //    Graphics.Blit(source, destination, material);
    //}

    private void Update()
    {
        // material.EnableKeyword("_NORMALMAP");
        // Debug.Log(enemiesChasing);
        valueColors = lightCharacter[0].color.b;
        if (enemiesChasing > 0)
        {
            if (valueColors < 0.5)
            {
                //vignetteSize = vignetteSizeMin;
                valueColors = valueColors + Time.deltaTime;
                //vignetteSize = vignetteSize + Time.deltaTime;
                growing = true;
            }
            else
            {
                if (growing)
                    valueColors = valueColors + Time.deltaTime;
                else
                    valueColors = valueColors - Time.deltaTime;

                if (valueColors > 1)
                    growing = false;
                if (valueColors < 0.5)
                    growing = true;
                //Debug.Log(vignetteSize);
            }
            foreach (Light2D l2D in lightCharacter)
            {
                l2D.color = new Color(2f, valueColors, valueColors);
            }
           
        }
        else 
        {
            if(valueColors < 1)
                valueColors = valueColors + Time.deltaTime;
            growing = true;
            //Debug.Log(vignetteSize);
            foreach (Light2D l2D in lightCharacter)
            {
                l2D.color = new Color(1, valueColors, valueColors);
            }
        }
    }

    public void changeEnemiesChasing(int value)
    {
        enemiesChasing = enemiesChasing + value;
    }

    public void setEnemiesChasing(int value)
    {
        enemiesChasing =  value;
    }



}
