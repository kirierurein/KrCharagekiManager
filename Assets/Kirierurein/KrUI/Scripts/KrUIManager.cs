using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Feb 2018
// @Brief : Manager for UI
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrUIManager : MonoBehaviour
{
    // enum.
    // @Brief : Screen type
    public enum eSCREEN_TYPE
    {
        COMMON = 0,
    }

    // @Brief : Dialog type
    public enum eDIALOG_TYPE
    {
        COMMON = 0,
    }

    // readonly.
    // dialog resourse path dictionary
    private readonly Dictionary<eSCREEN_TYPE, string> s_pSCREEN_PATH_DIC = new Dictionary<eSCREEN_TYPE, string>()
    {
        {eSCREEN_TYPE.COMMON, "Prefabs/Screen/ScreenCharageki"},
    };

    // dialog resourse path dictionary
    private readonly Dictionary<eDIALOG_TYPE, string> s_pDIALOG_PATH_DIC = new Dictionary<eDIALOG_TYPE, string>()
    {
        {eDIALOG_TYPE.COMMON, "Prefabs/Dialog/DialogCommon"},
    };

    // private inspector.
    [SerializeField]
    private GameObject          barrierToClone      = null; // barrier to clone
    [SerializeField]
    private Transform           screenBase          = null; // base object of the screen
    [SerializeField]
    private Transform           dialogBase          = null; // base object of the dialog
    [SerializeField]
    private int                 cacheScreenNum      = 10;   // number of screens to cache

    // private.
    private List<KrScreenCache> m_pScreenCaches     = null; // cache data of the screen

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // UNITY FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    void Awake()
    {
        KrDebug.Assert(barrierToClone != null, "barrierToClone is null", typeof(KrUIManager));
        KrDebug.Assert(screenBase != null, "screenBase is null", typeof(KrUIManager));
        KrDebug.Assert(dialogBase != null, "dialogBase is null", typeof(KrUIManager));
    }

    // @Brief : Start
    void Start ()
    {
        m_pScreenCaches = new List<KrScreenCache>();
        KrUIArgumentParameter pParam = new KrUIArgumentParameter();
        RequestScreen(eSCREEN_TYPE.COMMON, pParam);
    }
	
    // @Breif : Update
    void Update ()
    {	
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Request screen
    // @Param : eScreenType => Screen type
    //        : pParam      => Initialization parameters
    public void RequestScreen(eSCREEN_TYPE eScreenType, KrUIArgumentParameter pParam)
    {
        KrDebug.Assert(s_pSCREEN_PATH_DIC.ContainsKey(eScreenType), "The path of the corresponding dialog is not set. eScreenType = " + eScreenType.ToString(), typeof(KrUIManager));
        KrScreen pScreen = Create<KrScreen>(s_pSCREEN_PATH_DIC[eScreenType], screenBase);
        pScreen.Initialize(pParam);

        // Cache screen data
        CacheScreen(eScreenType, pParam);
    }

    // @Brief : Request dialog
    // @Param : eDialogType => Dialog type
    //        : pParam      => Initialization parameters
    public void RequestDialog(eDIALOG_TYPE eDialogType, KrUIArgumentParameter pParam)
    {
        Transform pBarrier = Create<Transform>(barrierToClone, dialogBase);
        KrDebug.Assert(s_pDIALOG_PATH_DIC.ContainsKey(eDialogType), "The path of the corresponding dialog is not set. eDialogType = " + eDialogType.ToString(), typeof(KrUIManager));
        KrDialog pDialog = Create<KrDialog>(s_pDIALOG_PATH_DIC[eDialogType], pBarrier);
        pDialog.Initialize(pParam);
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PRIVATE FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Cache screen data
    // @Param : eScreenType => Screen type
    //        : pParam      => Initialization parameters
    private void CacheScreen(eSCREEN_TYPE eScreenType, KrUIArgumentParameter pParam)
    {
        // Do not cache if the same screen as the current screen
        if(m_pScreenCaches.Count > 0)
        {
            if(m_pScreenCaches[0].IsSameType(eScreenType))
            {
                return;
            }
        }

        KrScreenCache pCache = new KrScreenCache(eScreenType, pParam);
        m_pScreenCaches.Add(pCache);

        // When the number of screens that can be cached is exceeded, the cache is deleted from the old one
        if(m_pScreenCaches.Count > cacheScreenNum)
        {
            m_pScreenCaches.RemoveAt(cacheScreenNum);
        }
    }

    // @Brief  : Create
    // @Param  : pPath   => Asset path under the resources folder
    //         : pParent => Parent of generated prefab
    // @Return : Component specified in template
    private T Create<T>(string pPath, Transform pParent)
    {
        GameObject pPrefab = KrResources.LoadDataInApp<GameObject>(pPath);
        return Create<T>(pPrefab, pParent);
    }

    // @Brief  : Create
    // @Param  : pPrefab => GameObject to be generated
    //         : pParent => Parent of generated prefab
    // @Return : Component specified in template
    private T Create<T>(GameObject pPrefab, Transform pParent)
    {
        GameObject pObj = Instantiate<GameObject>(pPrefab, pParent);
        pObj.SetActive(true);
        return pObj.GetComponent<T>();
    }
}

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Feb 2018
// @Brief : Argument parameter for UI
// @Memo : Argument class to pass to UI
//       : Base class has no arguments
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrUIArgumentParameter{}

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Feb 2018
// @Brief : Screen cache data
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrScreenCache
{
    public KrUIManager.eSCREEN_TYPE     m_eScreenType   = KrUIManager.eSCREEN_TYPE.COMMON;  // screen type
    public KrUIArgumentParameter        m_pParameter    = null;                             // initialization parameters

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief  : Constructor
    // @Param  : eScreenType    => Screen type
    //         : pParam         => Initialization parameters
    // @Return : KrScreenCache instance
    public KrScreenCache(KrUIManager.eSCREEN_TYPE eScreenType, KrUIArgumentParameter pParam)
    {
        m_eScreenType = eScreenType;
        m_pParameter = pParam;
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Breif  : Check if it is the same screen type
    // @Param  : eScreenType    => Initialization parameters
    // @Return : Is it the same screen type [TRUE = same screen, FALSE = not same screen]
    public bool IsSameType(KrUIManager.eSCREEN_TYPE eScreenType)
    {
        return eScreenType == m_eScreenType;
    }
}
