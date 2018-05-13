using UnityEngine;
using System.Collections;

//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Apr 2018
// @Brief  : Class trying lipsync from audio
//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrLive2DLipSyncAudio : KrLive2DLipSync
{
    // private.
    private KrAudioSource       m_pAudioSource          = null;     // audio source

    //:::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //:::::::::::::::::::::::::::::::::::::::::::::::::::::
    public KrLive2DLipSyncAudio(KrAudioSource pAudioSource)
    {
        m_pAudioSource = pAudioSource;
    }

    //:::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //:::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief  : Get value
    // @Return : Lipsync value
    public override float GetValue()
    {
        return m_pAudioSource.GetVolumePerFrame();
    }

    // @Brief  : Check is end lipsync
    // @Return : Is end lipsync [TRUE => Ended, FALSE => Not ended]
    public override bool IsEnd()
    {
        return m_pAudioSource.IsEnd();
    }
}

