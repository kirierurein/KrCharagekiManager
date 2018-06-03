using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Apr 2018
// @Brief : Audio manager
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrAudioManager : MonoBehaviour
{
    // instance.
    public static KrAudioManager            I                       = null;     // singleton

    // private inspector.
    [SerializeField]
    private int                             audioClipCacheNum       = 10;       // number of AudioClips to cache

    // private.
    private KrAudioSource                   m_pBgmAudioSource       = null;     // bgm audio source
    private KrAudioSource                   m_pVoiceAudioSource     = null;     // single voice audio source
    private List<KrAudioSource>             m_pSeAudioSources       = null;     // audio source list of se
    private List<KrAudioSource>             m_pVoiceAudioSources    = null;     // audio source list of multi voice
    private Dictionary<string, AudioClip>   m_pChacedAudioClip      = null;     // cached audio clip. [Key => asset path, Value => audio clip]

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // UNITY FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Breif : Awake
    void Awake()
    {
        I = this;
        m_pSeAudioSources = new List<KrAudioSource>();
        m_pVoiceAudioSources = new List<KrAudioSource>();
        m_pChacedAudioClip = new Dictionary<string, AudioClip>();
    }

    // @Breif : OnDestory
    void OnDestory()
    {
        I = null;
    }

    // @Brief : Update
    void Update()
    {
        List<KrAudioSource> pRemoveList = new List<KrAudioSource>();
        // Check the finished voice
        pRemoveList.AddRange(CheckFinishedAudioSources(m_pSeAudioSources));

        // Check the finished voice
        pRemoveList.AddRange(CheckFinishedAudioSources(m_pVoiceAudioSources));

        // Remove finished voice & se list
        RemoveAudioSources(pRemoveList);
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief  : Play bgm
    // @Param  : pPath          => Audio asset path
    //         : bFromResources => From resources file
    // @Return : Audio source
    public KrAudioSource PlayBgm(string pPath, bool bFromResources)
    {
        KrAudioSource pAudioSource = PlayAudio(pPath, true, bFromResources);
        if(pAudioSource == null)
            return null;

        if(m_pBgmAudioSource != null)
        {
            m_pBgmAudioSource.Destroy();
        }
        m_pBgmAudioSource = pAudioSource;
        return m_pBgmAudioSource;
    }

    // @Brief  : Play se
    // @Param  : pPath          => Audio asset path
    //         : bIsLoop        => Is loop mode
    //         : bFromResources => From resources file
    // @Return : Audio source
    public KrAudioSource PlaySe(string pPath, bool bIsLoop, bool bFromResources)
    {
        KrAudioSource pAudioSource = PlayAudio(pPath, bIsLoop, bFromResources);
        if(pAudioSource == null)
            return null;
        
        m_pSeAudioSources.Add(pAudioSource);
        return pAudioSource;
    }

    // @Brief  : Play se
    // @Param  : pPath          => Audio asset path
    //         : bIsMultiVoice  => Is multi voice
    //         : bFromResources => From resources file
    // @Return : Audio source
    public KrAudioSource PlayVoice(string pPath, bool bIsMultiVoice, bool bFromResources)
    {
        KrAudioSource pAudioSource = PlayAudio(pPath, false, bFromResources);
        if(pAudioSource == null)
            return null;
        
        if(bIsMultiVoice)
        {
            m_pVoiceAudioSources.Add(pAudioSource);
        }
        else
        {
            if(m_pVoiceAudioSource != null)
            {
                m_pVoiceAudioSource.Destroy();
            }
            m_pVoiceAudioSource = pAudioSource;
        }
        return pAudioSource;
    }

    // @Brief : Stop bgm
    public void StopBgm()
    {
        if(m_pBgmAudioSource != null)
        {
            m_pBgmAudioSource.Stop();
        }
    }

    // @Brief : Stop se
    public void StopSe()
    {
        for(int sIndex = 0; sIndex < m_pSeAudioSources.Count; sIndex++)
        {
            m_pSeAudioSources[sIndex].Stop();
        }
    }

    // @Brief : Stop se
    public void StopVoice()
    {
        if(m_pVoiceAudioSource != null)
        {
            m_pVoiceAudioSource.Stop();
        }
        for(int sIndex = 0; sIndex < m_pVoiceAudioSources.Count; sIndex++)
        {
            m_pVoiceAudioSources[sIndex].Stop();
        }
    }

    // @Brief  : Check finished audio source list
    // @Param  : pAudioSources  => Audio source list
    // @Return : Finished audio source list
    public List<KrAudioSource> CheckFinishedAudioSources(List<KrAudioSource> pAudioSources)
    {
        List<KrAudioSource> pRemoveAudioSources = new List<KrAudioSource>();
        // Check the finished voice
        for(int sIndex = 0; sIndex < pAudioSources.Count; sIndex++)
        {
            if(pAudioSources[sIndex].IsEnd())
            {
                pAudioSources[sIndex].Destroy();
                pRemoveAudioSources.Add(pAudioSources[sIndex]);
            }
        }
        return pRemoveAudioSources;
    }

    // @Brief : Remove audio sources
    // @Param : pRemoveAudioSources     => Audio source list to remove
    // @Note  : Only se and multi voice are deleted automatically
    public void RemoveAudioSources(List<KrAudioSource> pRemoveAudioSources)
    {
        for(int sIndex = 0; sIndex < pRemoveAudioSources.Count; sIndex++)
        {
            m_pSeAudioSources.Remove(pRemoveAudioSources[sIndex]);
            m_pVoiceAudioSources.Remove(pRemoveAudioSources[sIndex]);
        }
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PRIVATE FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief  : Play audio
    // @Param  : pPath          => Asset path of audio clip
    //         : bIsLoop        => Is loop mode
    //         : bFromResources => From resources file
    // @Return : Audio source
    private KrAudioSource PlayAudio(string pPath, bool bIsLoop, bool bFromResources)
    {
        AudioClip pAudioClip = null;

        // Check if AudioClip exists in cache
        if(m_pChacedAudioClip.ContainsKey(pPath))
        {
            pAudioClip = m_pChacedAudioClip[pPath];
            m_pChacedAudioClip.Remove(pPath);
        }
        else
        {
            pAudioClip = LoadAudioClip(pPath, bFromResources);
        }

        if(pAudioClip == null)
        {
            KrDebug.Warning(false, "Audio clip is null. path = " + pPath, typeof(KrAudioManager));
            return null;
        }

        m_pChacedAudioClip.Add(pPath, pAudioClip);

        // Delete the old cache when the number of AudioClips to be cached is exceeded
        if(m_pChacedAudioClip.Count > audioClipCacheNum)
        {
            string[] pKeys = new string[m_pChacedAudioClip.Keys.Count];
            m_pChacedAudioClip.Keys.CopyTo(pKeys, 0);
            m_pChacedAudioClip.Remove(pKeys[0]);
        }

        KrAudioSource pAudioSource = new GameObject().AddComponent<KrAudioSource>();
        pAudioSource.Play(pAudioClip,bIsLoop);
        pAudioSource.transform.SetParent(transform, false);
        pAudioSource.name = System.IO.Path.GetFileName(pPath);
        return pAudioSource;
    }

    // @Brief  : Load audio clip
    // @Param  : pPath          => Asset path
    //         : bFromResources => From resources file
    // @Return : Audio clip
    private AudioClip LoadAudioClip(string pPath, bool bFromResources = false)
    {
        if(string.IsNullOrEmpty(pPath))
        {
            KrDebug.Warning(false, "The path is empty. path = " + pPath, typeof(KrAudioManager));
            return null;
        }

        if(bFromResources)
        {
            return KrResources.LoadDataInApp<AudioClip>(pPath);
        }
        else
        {
            if(!System.IO.File.Exists(pPath))
            {
                KrDebug.Warning(false, "The file of the specified path does not exist. path = " + pPath, typeof(KrAudioManager));
                return null;
            }

            KrWav pWav = new KrWav(pPath, bFromResources);
            return pWav.CreateAudioClip();
        }
    }
}
