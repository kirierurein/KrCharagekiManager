using UnityEngine;
using System.Collections;

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Apr 2018
// @Brief : Base analysis class of audio data
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public abstract class KrAudioDataFormat
{
    // protected.
    protected string    m_pName = null;     // asset name

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief  : Constructor
    // @Param  : pPath          => Absolute path of Audio asset.
    //         : bFromResources => From resources file
    // @Return : KrWav instance
    public KrAudioDataFormat(string pPath, bool bFromResources)
    {
        m_pName = System.IO.Path.GetFileNameWithoutExtension(pPath);
        byte[] pBytes = KrResources.LoadBytes(pPath, bFromResources);
        ConvertBytes(pBytes);
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief  : Create audio clip
    // @Return : Audio clip
    public abstract AudioClip CreateAudioClip();

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PROTECTED FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Convert bytes
    // @Param : pBytes  => byte data
    protected abstract void ConvertBytes(byte[] pBytes);
}

