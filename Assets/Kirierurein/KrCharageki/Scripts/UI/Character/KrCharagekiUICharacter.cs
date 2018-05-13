using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Data for charageki ui character
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrCharagekiUICharacterData
{
    // private.
    private uint    m_uCharacterId      = 0;    // character id
    private string  m_pCharacterName    = "";   // character name

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Constructor
    // @Param : uId             => Character id
    //        : pCharacterName  => Character name
    public KrCharagekiUICharacterData(uint uId, string pCharacterName)
    {
        m_uCharacterId = uId;
        m_pCharacterName = pCharacterName;
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief  : Get character id
    // @Return : Character id
    public uint GetCharacterId()
    {
        return m_uCharacterId;
    }

    // @Brief  : Get character name
    // @Return : Character name
    public string GetCharacterName()
    {
        return m_pCharacterName;
    }
}

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Character of charageki ui
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public abstract class KrCharagekiUICharacter : MonoBehaviour, KrICharagekiUI
{
    // protected. 
    protected KrCharagekiUICharacterData    m_pCharaData    = null;     // character data

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // UNITY FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    protected virtual void Awake()
    {
        ErrorCheck();
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Set position
    // @Param : vPosition   => Character position
    public virtual void SetPosition(Vector3 vPosition)
    {
        transform.localPosition = vPosition;
    }

    // @Brief : Play animation
    // @Param : uActionId => Action id
    public abstract void PlayAction(uint uActionId);

    // @Brief  : Play voice
    // @Param  : pPath      => Asset path of audio clip
    //         : pManager   => Charageki manager
    // @Return : Audio source
    public virtual KrAudioSource PlayVoice(string pPath, KrCharagekiManager pManager)
    {
        return pManager.PlayVoice(pPath);
    }

    // @Brief : Manually play lip sync
    // @Param : fTime   => Talking time
    //        : pWord   => Talking word
    public virtual void PlayLipSync(float fTime, string pWord)
    {
    }

    // @Brief : Skip process
    public virtual void Skip()
    {
    }

    // @Brief : Show
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    // @Brief : Hide
    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PROTECTED FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Error check
    protected virtual void ErrorCheck()
    {
    }

    // @Brief : Initialize
    // @Param : pData   => Character data
    protected virtual void Initialize(KrCharagekiUICharacterData pData)
    {
        m_pCharaData = pData;
    }
}
