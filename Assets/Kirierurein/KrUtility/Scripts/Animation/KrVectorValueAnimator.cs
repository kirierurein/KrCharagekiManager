using UnityEngine;
using System.Collections;

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Feb 2018
// @Brief : Animator for vector value
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrVectorValueAnimator
{

    // private.
    private KrValueAnimator     m_pXValue       = null; // animator of x value
    private KrValueAnimator     m_pYValue       = null; // animator of y value
    private KrValueAnimator     m_pZValue       = null; // animator of z value
    private System.Action       m_cbEnd         = null; // callback of end animation
    private int                 m_sAnimCount    = 0;    // animation count

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Constructor
    // @Param : vFrom       => Value from of animation
    //          vTo         => Value to of animation
    //          fTime       => Animation end time
    //          eEaseType   => Ease type
    //          fEase       => Ease
    //          cbEnd       => Callback at the end of the animation
    public KrVectorValueAnimator(Vector3 vFrom, Vector3 vTo, float fTime, KrValueAnimator.eEASE eEaseType, float fEase, System.Action cbEnd = null)
    {
        m_pXValue = new KrValueAnimator(vFrom.x, vTo.x, fTime, eEaseType, fEase, cbCountDownEndAnim);
        m_pYValue = new KrValueAnimator(vFrom.y, vTo.y, fTime, eEaseType, fEase, cbCountDownEndAnim);
        m_pZValue = new KrValueAnimator(vFrom.z, vTo.z, fTime, eEaseType, fEase, cbCountDownEndAnim);
        m_cbEnd = cbEnd;
        m_sAnimCount = 3;
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Update
    public void Update()
    {
        m_pXValue.Update();
        m_pYValue.Update();
        if(m_pZValue != null)
        {
            m_pZValue.Update();
        }
    }

    // @Brief : Play animation
    public void Play()
    {
        m_pXValue.Play();
        m_pYValue.Play();
        if(m_pZValue != null)
        {
            m_pZValue.Play();
        }
    }

    // @Brief : Get vector3
    // @Return : Animation value
    public Vector3 GetVertor3()
    {
        float fX = m_pXValue.GetValue();
        float fY = m_pYValue.GetValue();
        float fZ = m_pZValue != null ? m_pZValue.GetValue() : 0.0f;
        return new Vector3(fX, fY, fZ);
    }

    // @Brief  : Is animation end
    // @Return : Is animation end [TRUE = end, FALSE = not end]
    public bool IsEnd()
    {
        bool bIsEnd = true;
        bIsEnd &= m_pXValue.IsEnd();
        bIsEnd &= m_pYValue.IsEnd();
        if(m_pZValue != null)
        {
            bIsEnd &= m_pZValue.IsEnd();
        }
        return bIsEnd;
    }

    // @Brief  : Is playing animation
    // @Return : Is playing end [TRUE = playing, FALSE = stop]
    public bool IsPlaying()
    {
        bool bIsPlaying = true;
        bIsPlaying &= m_pXValue.IsPlaying();
        bIsPlaying &= m_pYValue.IsPlaying();
        if(m_pZValue != null)
        {
            bIsPlaying &= m_pZValue.IsPlaying();
        }
        return bIsPlaying;
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PRIVATE FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Callback of count of end value animator animation
    private void cbCountDownEndAnim()
    {
        m_sAnimCount--;
        if(m_sAnimCount <= 0)
        {
            if(m_cbEnd != null)
            {
                m_cbEnd();
            }
        }
    }
}

