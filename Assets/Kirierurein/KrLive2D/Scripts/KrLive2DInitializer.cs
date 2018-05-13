using UnityEngine;
using System.Collections;
using live2d;

//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief  : Class for initialize live2d
//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrLive2DInitializer : MonoBehaviour
{
    // instance.
    private static KrLive2DInitializer I = null; // singleton.

    //:::::::::::::::::::::::::::::::::::::::::::::::::::::
    // UNITY FUNCTION
    //:::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Awake
    void Awake()
    {
        I = this;
        // Initialize live2d
        Live2D.init();
    }

    // @Brief : OnDestroy
    void OnDestroy()
    {
        Live2D.dispose();
        I = null;
    }

    //:::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //:::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief  : Create
    // @Return : KrLive2DInitializer instance
    public static KrLive2DInitializer Create()
    {
        if(I == null)
        {
            GameObject pPrefab = KrResources.LoadDataInApp<GameObject>("Prefabs/Live2DInitializer");
            Instantiate(pPrefab);
        }
        return I;
    }
}

