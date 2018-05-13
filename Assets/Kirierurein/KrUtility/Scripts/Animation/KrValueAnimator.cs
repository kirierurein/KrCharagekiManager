using UnityEngine;
using System.Collections;
using System;

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Animator for value
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrValueAnimator
{
    // @Brief : Ease type
    public enum eEASE
    {
        NONE    = 0,
        EASE_IN,
        EASE_OUT,
        EASE_BOTH
    }

    // private.
    private float   m_fFrom     = 0.0f;         // value from of animation
    private float   m_fTo       = 0.0f;         // value to of animation
    private float   m_fTime     = 0.0f;         // animation end time
    private eEASE   m_eEaseType = eEASE.NONE;   // ease type
    private float   m_fEase     = 1.0f;         // ease
    private float   m_fAnimTime = 0.0f;         // current animation time
    private float   m_fValue    = 0.0f;         // animation value
    private bool    m_bIsPlay   = false;        // flag of animation play
    private Action  m_cbEnd     = null;         // callback of animation end

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Constructor
    // @Param : fFrom       => Value from of animation
    //          fTo         => Value to of animation
    //          fTime       => Animation end time
    //          eEaseType   => Ease type
    //          fEase       => Ease
    //          cbEnd       => Callback at the end of the animation
    public KrValueAnimator(float fFrom, float fTo, float fTime, eEASE eEaseType, float fEase, Action cbEnd = null)
    {
        m_fFrom = fFrom;
        m_fTo = fTo;
        m_fTime = fTime;
        m_eEaseType = eEaseType;
        m_fEase = fEase;
        m_fValue = m_fFrom;
        m_cbEnd = cbEnd;
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Update
    public void Update()
    {
        if(m_bIsPlay)
        {
            m_fAnimTime += Time.deltaTime;
            m_fValue = Leap(m_fFrom, m_fTo, m_fAnimTime / m_fTime, m_eEaseType, m_fEase);
            if(m_fAnimTime >= m_fTime)
            {
                m_bIsPlay = false;
                if(m_cbEnd != null)
                {
                    m_cbEnd();
                }
            }
        }
    }

    // @Brief : Play animation
    public void Play()
    {
        m_fAnimTime = 0.0f;
        m_bIsPlay = true;
    }

    // @Brief  : Get value
    // @Return : Animation value
    public float GetValue()
    {
        return m_fValue; 
    }

    // @Brief  : Is animation end
    // @Return : Is animation end [TRUE = end, FALSE = not end]
    public bool IsEnd()
    {
        return m_fAnimTime >= m_fTime;
    }

    // @Brief  : Is playing animation
    // @Return : Is playing end [TRUE = playing, FALSE = stop]
    public bool IsPlaying()
    {
        return m_bIsPlay;
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PRIVATE FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief  : Leap Animation
    // @Param  : fFrom      => Value from of animation
    //           fTo        => Value to of animation
    //           fTime      => Animation end time
    //           eEaseType  => Ease type
    //           fEase      => Ease
    // @Return : Leap value
    private float Leap(float fFrom, float fTo, float fTime, eEASE eEaseType, float fEase)
    {
        float fRate = 0.0f;
        // Normal
        if(eEaseType == eEASE.NONE)
        {
            fRate = fTime;
        }
        // Ease in
        else if(eEaseType == eEASE.EASE_IN)
        {
            fRate = Mathf.Pow(fTime, fEase);
        }
        // Ease out
        else if(eEaseType == eEASE.EASE_OUT)
        {
            fTime -= 1.0f;
            fRate = (1.0f + Mathf.Pow(fTime, fEase));
        }
        // Ease both
        else if(eEaseType == eEASE.EASE_BOTH)
        {
            fRate = Mathf.Pow(fTime, fEase) * (3.0f - 2.0f * Mathf.Pow(fTime, (fEase - 1.0f)));
        }

        // Clamp
        fRate = Mathf.Max(0, Mathf.Min(fRate, 1.0f));
        return (fFrom * (1.0f - fRate)) + (fTo * fRate);
    }
}

