using UnityEngine;
using System.Collections;

//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Apr 2018
// @Brief  : Class trying lipsync from string
//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrLive2DLipSyncString : KrLive2DLipSync
{
    // private.
    private float       m_fTalkingTime          = 0.0f;     // talking time for lip sync
    private string      m_pTalkingWord          = null;     // talking word
    private float       m_fCurrentTalkingTime   = 0.0f;     // current talking time
    private float       m_fValue                = 0.0f;     // value of lipsync

    //:::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //:::::::::::::::::::::::::::::::::::::::::::::::::::::
    public KrLive2DLipSyncString(float fTalkingTime, string pTalkingWord)
    {
        m_fTalkingTime = fTalkingTime;
        m_pTalkingWord = pTalkingWord;
    }

    //:::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //:::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Update
    public override void Update()
    {
        m_fCurrentTalkingTime += Time.deltaTime;
        float fTimePerChar = m_fTalkingTime / m_pTalkingWord.Length;
        float fDegree = 90.0f * m_fCurrentTalkingTime / fTimePerChar;
        m_fValue = Mathf.Abs(Mathf.Sin(fDegree * Mathf.Deg2Rad));
    }

    // @Brief : Skip
    public override void Skip()
    {
        m_fCurrentTalkingTime = m_fTalkingTime;
        m_fValue = 0.0f;
    }

    // @Brief  : Get value
    // @Return : Lipsync value
    public override float GetValue()
    {
        return m_fValue;
    }

    // @Brief  : Check is end lipsync
    // @Return : Is end lipsync [TRUE => Ended, FALSE => Not ended]
    public override bool IsEnd()
    {
        return m_fCurrentTalkingTime >= m_fTalkingTime;
    }
}

