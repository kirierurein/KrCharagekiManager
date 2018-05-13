using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Container for character of charageki ui
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrCharagekiUICharacterContainer
{
    // @Brief : Mode for displaying characters
    public enum eVIEW_MODE
    {
        SPRITE = 0, // 2D Sprite
        LIVE2D,     // Live 2D
    }

    // private.
    private Transform                                   m_pCharaParent      = null;                 // character parent
    private eVIEW_MODE                                  m_eMode             = eVIEW_MODE.SPRITE;    // mode for displaying characters
    private Dictionary<uint, KrCharagekiUICharacter>    m_pCharaContainer   = null;                 // character container

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief  : Constructor
    // @Return : KrCharagekiUICharacterContainer instance
    public KrCharagekiUICharacterContainer(Transform pCharaParent, eVIEW_MODE eMode)
    {
        m_pCharaParent = pCharaParent;
        m_eMode = eMode;
        m_pCharaContainer = new Dictionary<uint, KrCharagekiUICharacter>();
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Load
    // @Param : uCharaId    => character id
    public void Load(uint uCharaId)
    {
        KrDebug.Assert(KrCharagekiDef.s_pCHARA_DIC.ContainsKey(uCharaId), "Invalid KrCharagekiDef.s_CHARA_DIC key = " + uCharaId, typeof(KrCharagekiUICharacterContainer));
        KrCharagekiUICharacterData pData = KrCharagekiDef.s_pCHARA_DIC[uCharaId];
        KrDebug.Assert(!m_pCharaContainer.ContainsKey(uCharaId), "It is already registered key = " + uCharaId, typeof(KrCharagekiUICharacterContainer));
        KrCharagekiUICharacter pChara = null;

        // 2D SPRITE
        if(m_eMode == eVIEW_MODE.SPRITE)
        {
            pChara =  KrCharagekiUI2DCharacter.Create(m_pCharaParent, pData);
        }
        // LIVE 2D
        else if(m_eMode == eVIEW_MODE.LIVE2D)
        {
            pChara =  KrCharagekiUILive2DCharacter.Create(m_pCharaParent, pData);
        }

        m_pCharaContainer.Add(uCharaId, pChara);
    }

    // @Brief : Show
    // @Param : uCharaId    => character id
    public void Show(uint uCharaId)
    {
        KrCharagekiUICharacter pChara = GetCharacter(uCharaId);
        pChara.Show();
    }

    // @Brief : Hide
    // @Param : uCharaId    => character id
    public void Hide(uint uCharaId)
    {
        KrCharagekiUICharacter pChara = GetCharacter(uCharaId);
        pChara.Hide();
    }

    // @Brief : Set action
    // @Param : uCharaId    => Character id
    //          uActionId   => Action id
    public void SetAction(uint uCharaId, uint uActionId)
    {
        KrCharagekiUICharacter pChara = GetCharacter(uCharaId);
        pChara.PlayAction(uActionId);
    }

    // @Brief : Set position
    // @Param : uCharaId    => Character id
    //        : vPosition   => Character position
    public void SetPosition(uint uCharaId, Vector3 vPosition)
    {
        KrCharagekiUICharacter pChara = GetCharacter(uCharaId);
        pChara.SetPosition(vPosition);
    }

    // @Brief  : Play Voice
    // @Param  : uCharaId   => Character Id
    //         : pPath      => Asset path of audio clip
    //         : pManager   => Charageki manager
    // @Return : Audio source
    public KrAudioSource PlayVoice(uint uCharaId, string pPath, KrCharagekiManager pManager)
    {
        KrCharagekiUICharacter pChara = GetCharacter(uCharaId);
        return pChara.PlayVoice(pPath, pManager);
    }

    // @Brief : Manually play lip sync
    // @Param : uCharaId    => Character Id
    //        : fTime       => Talking time
    //        : pWord       => Talking word
    public void PlayLipSync(uint uCharaId, float fTime, string pWord)
    {
        KrCharagekiUICharacter pChara = GetCharacter(uCharaId);
        pChara.PlayLipSync(fTime, pWord);
    }

    // @Brief : Skip process
    public void Skip()
    {
        KrCharagekiUICharacter[] pCharacters = new KrCharagekiUICharacter[m_pCharaContainer.Count];
        m_pCharaContainer.Values.CopyTo(pCharacters, 0);
        for(int sIndex = 0; sIndex < pCharacters.Length; sIndex++)
        {
            pCharacters[sIndex].Skip();
        }
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PRIVATE FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief  : Get character
    // @Param  : uCharaId   => character id 
    // @Return : KrCharagekiUICharacter instance corresponding to the character ID
    private KrCharagekiUICharacter GetCharacter(uint uCharaId)
    {
        KrDebug.Assert(m_pCharaContainer.ContainsKey(uCharaId), "Invalid m_pCharaContainer key = " + uCharaId, typeof(KrCharagekiUICharacterContainer));
        return m_pCharaContainer[uCharaId];
    }
}

