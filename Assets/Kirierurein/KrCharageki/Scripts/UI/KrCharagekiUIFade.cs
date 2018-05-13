using UnityEngine;
using System.Collections;
using UnityEngine.UI;


//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Fade of charageki ui
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrCharagekiUIFade : KrICharagekiUI
{
    // private
    private GameObject              m_pFadeObject        = null;                        // fade object. NOTE : externally registered object
    private System.Action<float>    m_cbUpdateFade      = null;                         // callback for update date. NOTE : externally registered processing
    private KrValueAnimator.eEASE   m_EaseType          = KrValueAnimator.eEASE.NONE;   // ease type
    private float                   m_fEase             = 1.0f;                         // ease value
    private float                   m_fTime             = 1.0f;                         // fade time
    private KrValueAnimator         m_pValueAnimator    = null;                         // fade value animator  
    private bool                    m_bIsFade           = false;                        // is fade

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief  : Constructor
    // @Param  : eEaseType  => easing type
    //         : fEase      => ease
    //         : fEaseTime  => easing time
    // @Return : KrCharagekiUIFade instance
    public KrCharagekiUIFade()
    {
        m_EaseType = KrValueAnimator.eEASE.NONE;
        m_fEase = 1.0f;
        m_fTime = 1.0f;
        m_cbUpdateFade = null;
        m_pValueAnimator = null;
        m_bIsFade = false;
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Update
    public void Update ()
    {
        if(m_pValueAnimator != null)
        {
            m_pValueAnimator.Update();
            if(m_cbUpdateFade != null)
            {
                m_cbUpdateFade(m_pValueAnimator.GetValue());
            }
        }
    }

    // @Brief : Play
    // @Param : fFrom   => Value from of animation
    //          fTo     => Value to of animation
    //          cbEnd       => Callback at the end of the animation
    public void Play(float fFrom, float fTo, System.Action cbEnd = null)
    {
        m_pValueAnimator = new KrValueAnimator(fFrom, fTo, m_fTime, m_EaseType, m_fEase, cbEnd);
        m_pValueAnimator.Play();
    }

    // @Brief : Play fade in
    public void PlayFadeIn()
    {
        Show();
        m_bIsFade = true;
        Play(0.0f, 1.0f, () => 
        {
            m_bIsFade = false;
        });
    }

    // @Brief : Play fade Out
    public void PlayFadeOut()
    {
        m_bIsFade = true;
        Play(1.0f, 0.0f, () => 
        {
            m_bIsFade = false;
            Hide();
        });
    }

    // @Brief : Play fade in & out
    public void PlayFade()
    {
        Show();
        m_bIsFade = true;
        // Fade in
        Play(0.0f, 1.0f, () => 
        {
            // Fade out
            Play(1.0f, 0.0f, () => 
            {
                m_bIsFade = false;
                Hide();
            });
        });
    }

    // @Brief  : Is fade
    // @Return : Is fade [TRUE = playing, False = stop]
    public bool IsFade()
    {
        return m_bIsFade;
    }

    // @Brief : Show
    public void Show()
    {
        if(m_pFadeObject != null)
        {
            m_pFadeObject.SetActive(true);
        }
    }

    // @Brief : Hide
    public void Hide()
    {
        if(m_pFadeObject != null)
        {
            m_pFadeObject.SetActive(false);
        }
    }

    // @Brief : Register fade
    // @Param : eEaseType       => Easing type
    //        : fEase           => Ease
    //        : fEaseTime       => Easing time
    //        : pFadeObject     => Fade object
    //        : cbUpdateFade    => Callback for update fade
    public void RegisterFade(KrValueAnimator.eEASE eEaseType, float fEase, float fEaseTime, GameObject pFadeObject, System.Action<float> cbUpdateFade)
    {
        m_EaseType = eEaseType;
        m_fEase = fEase;
        m_fTime = fEaseTime;
        m_pFadeObject = pFadeObject;
        m_cbUpdateFade = cbUpdateFade;
    }
}

