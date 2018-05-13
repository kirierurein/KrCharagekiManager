using UnityEngine;
using System.Collections;

//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Apr 2018
// @Brief  : The lipsync class of the live2d model
//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public abstract class KrLive2DLipSync
{
    //:::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //:::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Update
    public virtual void Update(){}

    // @Brief : Skip
    public virtual void Skip(){}
    
    // @Brief  : Get value
    // @Return : Lipsync value
    public abstract float GetValue();

    // @Brief  : Check is end lipsync
    // @Return : Is end lipsync [TRUE => Ended, FALSE => Not ended]
    public abstract bool IsEnd();
}

