using UnityEngine;
using System.Collections;

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Apr 2018
// @Brief : Audio source
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
[RequireComponent(typeof(AudioSource))]
public class KrAudioSource : MonoBehaviour
{
    // @Brief : Playback status
    private enum ePLAYBACK_STATUS
    {
        PREPARE = 0,
        PLAYING,
        PAUSE,
        STOP,
    }

    // private.
    private AudioSource         m_pAudioSource      = null;                     // audio source
    private ePLAYBACK_STATUS    m_eStatus           = ePLAYBACK_STATUS.PREPARE; // playback status

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // UNITY FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Breif : Awake
    void Awake()
    {
        m_pAudioSource = GetComponent<AudioSource>();
        m_eStatus = ePLAYBACK_STATUS.PREPARE;
    }

    // @Brief : Update
    void Update()
    {
        if(!m_pAudioSource.isPlaying)
        {
            if(m_eStatus == ePLAYBACK_STATUS.PLAYING)
            {
                m_eStatus = ePLAYBACK_STATUS.STOP;
            }
        }
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Play
    // @Param : pClip   => Audio clip
    //        : bIsLoop => Is loop mode
    public void Play(AudioClip pClip, bool bIsLoop)
    {
        m_pAudioSource.clip = pClip;
        m_pAudioSource.loop = bIsLoop;
        m_pAudioSource.Play();
        m_eStatus = ePLAYBACK_STATUS.PLAYING;
    }

    // @Breif : Pause
    public void Pause()
    {
        m_pAudioSource.Pause();
        m_eStatus = ePLAYBACK_STATUS.PAUSE;
    }

    // @Breif : Un pause
    public void UnPause()
    {
        m_pAudioSource.Pause();
        m_eStatus = ePLAYBACK_STATUS.PLAYING;
    }

    // @Brief : Stop
    public void Stop()
    {
        m_pAudioSource.Stop();
        m_eStatus = ePLAYBACK_STATUS.STOP;
    }

    // @Brief : Destroy
    public void Destroy()
    {
        Stop();
        Destroy(gameObject);
    }

    // @Brief  : Is end
    // @Return : Is end audio. [TRUE => stop, FALSE => playing]
    // @Note   : Ignore the pause state
    public bool IsEnd()
    {
        return m_eStatus == ePLAYBACK_STATUS.STOP;
    }

    // @Brief  : Get audio averaged volume per frame
    // @Return : Average audio volume per frame
    // @Memo   : e.g. 40db = 1.0f
    public float GetVolumePerFrame()
    {
        float fSum = 0;
        float[] pWave = new float[256];
        m_pAudioSource.GetOutputData(pWave, (m_pAudioSource.clip.channels - 1));

        // Ask for RMS(root mean square)
        // @MEMO : √(( x1^2 + x2^2 + x3^2 + ...+ xn^2 ) / n)
        for(int sIndex = 0; sIndex < pWave.Length; sIndex++)
        {
            fSum += Mathf.Pow(pWave[sIndex], 2);
        }
        float fRMS = Mathf.Sqrt(fSum / pWave.Length);

        // Ask for decibel
        // @MEMO : db = 20 * log10( "RMS" / "Reference value" )
        //       : 0db = 0.00002f
        float fDecibel = 20 * Mathf.Log10(fRMS / 0.00002f);

        // Divide the calculated dB by 90 and normalize it
        // @MEMO : e.g. 90db    => The size of the voice when shouting
        //       :      40~60db => Voice size of common conversation
        return fDecibel / 90;
    }
}
