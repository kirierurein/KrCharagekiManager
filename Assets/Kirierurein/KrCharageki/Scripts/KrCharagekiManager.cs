using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Charageki manager
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrCharagekiManager : MonoBehaviour
{
    // instance.
    public static KrCharagekiManager        I                           = null;     // singleton

    // const.
    private const string                    c_pPREFAB_PATH              = "Prefabs/CharagekiManager"; // charageki manager prefab path

    // private.
    private string[]                        m_pScriptPaths              = null;     // script paths
    private List<KrCharagekiSection>        m_pMainCharagekiSections    = null;     // list for each charageki action
    private int                             m_sSectionIndex             = 0;        // current index for charageki action list
    private KrCharagekiUIController         m_pUIController             = null;     // ui controller
    private KrCharagekiLogContainer         m_pLogContainer             = null;     // log container
    private KrCharagekiScenarioContainer    m_pScenarioContainer        = null;     // scenario container

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // UNITY FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Awake
    void Awake()
    {
        I = this;
    }

    // @Brief : OnDestroy
    void OnDestroy()
    {
        m_pUIController.Unload();
        I = null;
    }

	// @Brief : Start
	void Start()
    {
        m_pLogContainer = new KrCharagekiLogContainer();
        m_pScenarioContainer = new KrCharagekiScenarioContainer();

        KrCharagekiScript pScript = LoadScript();

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        // RESOURCE DOWNLOAD
        List<string> pResourcePaths = pScript.GetResourcesPaths();
        for(int sIndex = 0; sIndex < pResourcePaths.Count; sIndex++)
        {
            // TODO : Add asset download
            KrDebug.Log("Download => " + pResourcePaths[sIndex], typeof(KrCharagekiManager));
        }

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        // INITILIZE COMMAND
        List<KrCharagekiCommand> pInitializeCommands = pScript.GetInitializeCommands();

        KrDebug.Log("Initialize charageki", typeof(KrCharagekiManager));
        for(int sIndex = 0; sIndex < pInitializeCommands.Count; sIndex++)
        {
            pInitializeCommands[sIndex].Exec(this);
        }

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        // SET MAIN SECTION
        m_pMainCharagekiSections = pScript.GetMainSections();
        KrDebug.Log("Get main sections : count = " + m_pMainCharagekiSections.Count, typeof(KrCharagekiManager));
        m_sSectionIndex = 0;

        ScriptUpdate();
	}
	
	// @Breif : Update
	void Update()
    {
        m_pUIController.Update();
        // Auto mode or not waiting for input
        if(m_pUIController.IsAutoMode() || !m_pUIController.IsWaitInput())
        {
            if(m_sSectionIndex < m_pMainCharagekiSections.Count && !m_pUIController.IsInAction())
            {
                bool bIsScriptUpdate = m_pUIController.UpdateAutoMode();
                if(bIsScriptUpdate)
                {
                    m_pUIController.ResetAutoTime();
                    ScriptUpdate();
                }
            }
        }
	}

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief  : Create
    // @Param  : pParent        => Charageki manager parent
    //         : pScripts       => Script paths
    //         : pCharaParent   => Character parent
    //         : eViewMode      => Character display mode
    //         : fAutoTime      => Wait time after completion of processing of auto mode
    //         : pAssetBasePath => Asset base path
    //         : pServerBaseUrl => Server base url
    // @Return : KrCharagekiManager instance
    public static KrCharagekiManager Create(Transform pParent, string[] pScripts, Transform pCharaParent, KrCharagekiUICharacterContainer.eVIEW_MODE eViewMode, float fAutoTime, string pAssetBasePath = "", string pServerBaseUrl = "")
    {
        GameObject pPrefab = KrResources.LoadDataInApp<GameObject>(c_pPREFAB_PATH);
        GameObject pObj = Instantiate<GameObject>(pPrefab, pParent);
        pObj.SetActive(true);
        KrCharagekiManager pManager = pObj.GetComponent<KrCharagekiManager>();
        pManager.Initialize(pScripts, pCharaParent, eViewMode, fAutoTime, pAssetBasePath, pServerBaseUrl);
        return pManager;
    }

    // @Brief : Tap screen process
    public void TapScreen()
    {
        // Accept input
        if(m_pUIController.IsWaitInput())
        {
            if(m_sSectionIndex < m_pMainCharagekiSections.Count && !m_pUIController.IsInAction())
            {
                KrAudioManager.I.StopVoice();
                m_pUIController.ResetAutoTime();
                ScriptUpdate();
            }
            else
            {
                m_pUIController.SkipAction();
            }
        }
    }

    // @Brief  : Get ui controller
    // @Return : KrCharagekiUIController instance
    public KrCharagekiUIController GetUIController()
    {
        return m_pUIController;
    }

    // @Brief  : Get log container
    // @Return : KrCharagekiLogContainer instance
    public KrCharagekiLogContainer GetLogContainer()
    {
        return m_pLogContainer;
    }

    // @Brief  : Get scenario container
    // @Return : KrCharagekiScenarioContainer instance
    public KrCharagekiScenarioContainer GetScenarioContainer()
    {
        return m_pScenarioContainer;
    }

    // @Brief  : Play voice
    // @Param  : pPath  => Asset path of audio clip
    // @Return : Audio source
    public KrAudioSource PlayVoice(string pPath)
    {
        return KrAudioManager.I.PlayVoice(KrCharagekiDef.s_pASSET_BASE_PATH + pPath, false, KrCharagekiDef.IsLoadingFromResources());
    }

    // @Brief  : Play se
    // @Param  : pPath  => Asset path of audio clip
    // @Return : Audio source
    public KrAudioSource PlaySe(string pPath)
    {
        return KrAudioManager.I.PlaySe(KrCharagekiDef.s_pASSET_BASE_PATH + pPath, false, KrCharagekiDef.IsLoadingFromResources());
    }

    // @Brief  : Play bgm
    // @Param  : pPath  => Asset path of audio clip
    // @Return : Audio source
    public KrAudioSource PlayBgm(string pPath)
    {
        return KrAudioManager.I.PlayBgm(KrCharagekiDef.s_pASSET_BASE_PATH + pPath, KrCharagekiDef.IsLoadingFromResources());
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PRIVATE FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief  : Load script
    // @Return : KrCharagekiScript instance
    private KrCharagekiScript LoadScript()
    {
        KrCharagekiScript pScript = new KrCharagekiScript();
        for(int sIndex = 0; sIndex < m_pScriptPaths.Length; sIndex++)
        {
            using(StreamReader pReader = KrResources.LoadText(KrCharagekiDef.s_pASSET_BASE_PATH + m_pScriptPaths[sIndex], KrCharagekiDef.IsLoadingFromResources()))
            {
                pScript.LoadScript(pReader);
            }
        }
        return pScript;
    }

    // @Brief : Update for main script
    private void ScriptUpdate()
    {
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        // UPDATE CHARAGEKI
        KrDebug.Log("Play section. No." + m_sSectionIndex, typeof(KrCharagekiManager));
        KrCharagekiSection pSection = m_pMainCharagekiSections[m_sSectionIndex];
        List<KrCharagekiCommand> pCommands = pSection.GetCommands(); 
        for(int sIndex = 0; sIndex < pCommands.Count; sIndex++)
        {
            pCommands[sIndex].Exec(this);
        }
        m_sSectionIndex++;
    }

    // @Brief : Initialize
    // @Param : pScripts        => Script paths
    //        : pCharaParent    => Character parent
    //        : eViewMode       => Character display mode
    //        : fAutoTime       => Wait time after completion of processing of auto mode
    //        : pAssetBasePath  => Asset base path
    //        : pServerBaseUrl  => Server base url
    private void Initialize(string[] pScripts, Transform pCharaParent, KrCharagekiUICharacterContainer.eVIEW_MODE eViewMode, float fAutoTime, string pAssetBasePath, string pServerBaseUrl = "")
    {
        m_pScriptPaths = pScripts;
        KrCharagekiDef.s_pASSET_BASE_PATH = pAssetBasePath;
        KrCharagekiDef.s_pSERVER_BASE_URL = pServerBaseUrl;
        // Cached controller
        m_pUIController = new KrCharagekiUIController(pCharaParent, eViewMode, fAutoTime);
    }
}
