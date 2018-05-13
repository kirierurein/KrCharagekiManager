using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Controller for charageki ui 
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrCharagekiUIController
{
    // private.
    private KrCharagekiUITitle              m_pTitle            = null;     // title
    private KrCharagekiUITextArea           m_pTextArea         = null;     // text area
    private KrCharagekiUIBackground         m_pBackground       = null;     // back ground
    private KrCharagekiUIFade               m_pFade             = null;     // fade
    private KrCharagekiUICharacterContainer m_pCharaContainer   = null;     // character container
    private bool                            m_bWaitInput        = true;     // wait for input
    private float                           m_fWaitTime         = 0.0f;     // wait time
    private float                           m_fCurrentWaitTime  = 0.0f;     // current wait time
    private bool                            m_bIsAudoMode       = false;    // is auto mode
    private float                           m_fAutoTime         = 0.0f;     // wait time after completion of processing of auto mode
    private float                           m_fCurrentAutoTime  = 0.0f;     // current wait time after completion of processing of auto mode


    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // UNITY FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief  : Constructor
    // @Param  : pCharaParent   => Parent object for character
    //         : eViewMode      => Character display mode
    //         : fAutoTime      => Wait time after completion of processing of auto mode
    // @Return : KrCharagekiUIController instance
    public KrCharagekiUIController(Transform pCharaParent, KrCharagekiUICharacterContainer.eVIEW_MODE eViewMode, float fAutoTime)
    {
        m_pTitle = new KrCharagekiUITitle();
        m_pBackground = new KrCharagekiUIBackground();
        m_pFade = new KrCharagekiUIFade();
        m_pTextArea = new KrCharagekiUITextArea();
        m_pCharaContainer = new KrCharagekiUICharacterContainer(pCharaParent, eViewMode);
        m_bWaitInput = true;
        m_fWaitTime = 0.0f;
        m_fCurrentWaitTime = 0.0f;
        m_bIsAudoMode = false;
        m_fAutoTime = fAutoTime;
        m_fCurrentAutoTime = 0.0f;
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Update
    public void Update ()
    {
        if(m_fCurrentWaitTime < m_fWaitTime)
        {
            m_fCurrentWaitTime += Time.deltaTime;
        }
        m_pFade.Update();
        m_pTextArea.Update();
    }

    // @Brief  : Update auto mode
    // @Return : Wait for the standby time since the process is finished. [TRUE => Wait, FALSE => Not waiting]
    public bool UpdateAutoMode()
    {
        m_fCurrentAutoTime += Time.deltaTime;
        return m_fCurrentAutoTime >= m_fAutoTime;
    }

    // @Brief : Reset auto time
    public void ResetAutoTime()
    {
        m_fCurrentAutoTime = 0.0f;
    }

    // @Brief : Unload
    public void Unload()
    {
        m_pBackground.Unload();
    }

    // @Brief : Set title
    public void SetTitle()
    {
        string pTitleName = m_pBackground.GetPlaceName();
        m_pTitle.SetTitle(pTitleName);
    }

    // @Brief : Show title
    public void ShowTitle()
    {
        m_pTitle.Show();
    }

    // @Brief : Hide title
    public void HideTitle()
    {
        m_pTitle.Hide();
    }

    // @Brief  : Set text
    // @Param  : pCharaName => Chara name
    //         : pComment   => Chara comment
    // @Return : Time when all text finishes playing
    public float SetText(string pCharaName, string pComment)
    {
        float fReadingTime = m_pTextArea.SetText(pCharaName, pComment);
        return fReadingTime;
    }

    // @Brief : Show text
    public void ShowText()
    {
        m_pTextArea.Show();
    }

    // @Brief  : Check acting on the UI
    // @Return : Is in action
    public bool IsInAction()
    {
        return m_pTextArea.IsReading() || m_pFade.IsFade() || IsWaiting();
    }

    // @Brife : Skip ui action
    public void SkipAction()
    {
        m_pTextArea.DisplayAllText();
        m_pCharaContainer.Skip();
    }

    // @Brief : Load background
    // @Param : uId => Background id
    public void LoadBg(uint uId)
    {
        m_pBackground.Load(uId);
    }

    // @Brief : Set background
    // @Param : uId => Background id
    public void SetBg(uint uId)
    {
		m_pBackground.SetSprite(uId);
    }

    // @Brief : Show background
    public void ShowBg()
    {
        m_pBackground.Show();
    }

    // @Brief : Hide background
    public void HideBg()
    {
        m_pBackground.Hide();
    }

    // @Brief : Fade out
    public void FadeOut()
    {
        m_pFade.PlayFadeOut();
    }

    // @Brief : Fade in
    public void FadeIn()
    {
        m_pFade.PlayFadeIn();
    }

    // @Brief : Set wait input
    // @Param : bIsWait => Wait for input [TRUE = Wait, FALSE = Not wait]
    public void SetWaitInput(bool bIsWait)
    {
        m_bWaitInput = bIsWait;
    }

    // @Brief  : Check wait for input
    // @Return : Wait for input [TRUE = Wait, FALSE = Not wait]
    public bool IsWaitInput()
    {
        return m_bWaitInput;
    }

    // @Brief  : Get whether it is auto mode
    // @Return : Whether it is auto mode [TRUE = auto mode, FALSE = not auto mode]
    public bool IsAutoMode()
    {
        return m_bIsAudoMode;
    }

    // @Brief : Set ui of wait time
    // @Param : fWaitTime   => Wait time
    public void SetWaitTime(float fWaitTime)
    {
        m_fWaitTime = fWaitTime;
        m_fCurrentWaitTime = 0.0f;
    }

    // @Brief : Load character
    // @Param : uCharaId    => Character id
    public void LoadCharacter(uint uCharaId)
    {
        m_pCharaContainer.Load(uCharaId);
    }

    // @Brief : Show character
    // @Param : uCharaId    => Character id
    public void ShowCharacter(uint uCharaId)
    {
        m_pCharaContainer.Show(uCharaId);
    }

    // @Brief : Hide character
    // @Param : uCharaId    => Character id
    public void HideCharacter(uint uCharaId)
    {
        m_pCharaContainer.Hide(uCharaId);
    }

    // @Brief : Set action
    // @Param : uCharaId    => Character id
    //          uActionId   => Action id
    public void SetAction(uint uCharaId, uint uActionId)
    {
        m_pCharaContainer.SetAction(uCharaId, uActionId);
    }
        
    // @Brief : Set position
    // @Param : uCharaId    => Character id
    //        : vPosition   => Character position
    public void SetPosition(uint uCharaId, Vector3 vPosition)
    {
        m_pCharaContainer.SetPosition(uCharaId, vPosition);
    }

    // @Brief  : Play Voice
    // @Param  : uCharaId   => Character Id
    //         : pPath      => Asset path of audio clip
    //         : pManager   => Charageki manager
    // @Return : Audio source
    public KrAudioSource PlayVoice(uint uCharaId, string pPath, KrCharagekiManager pManager)
    {
        return m_pCharaContainer.PlayVoice(uCharaId, pPath, pManager);
    }

    // @Brief : Manually play lip sync
    // @Param : uCharaId    => Character Id
    //        : fTime       => Talking time
    //        : pWord       => Talking word
    public virtual void PlayLipSync(uint uCharaId, float fTime, string pWord)
    {
        m_pCharaContainer.PlayLipSync(uCharaId, fTime, pWord);
    }

    // @Brief  : Toggle auto mode
    // @Return : Whether it is auto mode [TRUE = auto mode, FALSE = not auto mode]
    public bool ToggleAutoMode()
    {
        SetAutoMode(!m_bIsAudoMode);
        return m_bIsAudoMode;
    }

    // @Brief : Reset auto mode
    public void ResetAutoMode()
    {
        SetAutoMode(false);
    }

    //----------------------------------------------------------------
    // REGISTER CALLBACK FUNCTION
    //----------------------------------------------------------------
    // @Brief : Register title
    // @Param : cbSetText       => Callback for set text
    //        : cbShow          => Callback for show title 
    //        : cbHide          => Callback for hide title
    public void RegisterTitle(System.Action<string> cbSetText, System.Action cbShow, System.Action cbHide)
    {
        m_pTitle.RegisterTitle(cbSetText, cbShow, cbHide);
    }

    // @Brief : Register fade update
    // @Param : eEaseType       => Easing type
    //        : fEase           => Ease
    //        : fEaseTime       => Easing time
    //        : pFadeObject     => Fade object
    //        : cbUpdateFade    => Callback for update fade
    public void RegisterFade(KrValueAnimator.eEASE eEaseType, float fEase, float fEaseTimeGameObject, GameObject pFadeObject, System.Action<float> cbUpdateFade)
    {
        m_pFade.RegisterFade(eEaseType, fEase, fEaseTimeGameObject, pFadeObject, cbUpdateFade);
    }

    // @Brief : Register background
    // @Param : cbSetBackground => Callback for set background
    //        : cbShow          => Callback for show title 
    //        : cbHide          => Callback for hide title
    public void RegisterBackground(System.Action<Sprite> cbSetBackground, System.Action cbShow, System.Action cbHide)
    {
        m_pBackground.RegisterBackground(cbSetBackground, cbShow, cbHide);
    }

    // @Brief : Register text area
    // @Param : fReadingTime    => Reading time of comment
    //        : cbSetCharaName  => Callback for set character name
    //        : cbSetComment    => Callback for set comment
    //        : cbShow          => Callback for show title 
    //        : cbHide          => Callback for hide title
    public void RegisterTextArea(float fReadingTime, System.Action<string> cbSetCharaName, System.Action<string> cbSetComment, System.Action cbShow, System.Action cbHide)
    {
        m_pTextArea.RegisterTextArea(fReadingTime, cbSetCharaName, cbSetComment, cbShow, cbHide);
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PRIVATE FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief  : Check wait for second
    // @Return : Check wait for second [TRUE => Waiting, FALSE => Not waiting]
    private bool IsWaiting()
    {
        return m_fCurrentWaitTime < m_fWaitTime;
    }

    // @Brief : Set auto mode
    // @Param : bIsAutoMode => Is auto mode [TRUE = auto mode, FALSE = not auto mode]
    private void SetAutoMode(bool bIsAutoMode)
    {
        m_bIsAudoMode = bIsAutoMode;
    }
}
