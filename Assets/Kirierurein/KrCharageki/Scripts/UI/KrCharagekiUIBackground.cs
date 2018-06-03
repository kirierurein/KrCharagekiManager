using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

using BgData = KrCharagekiUIBackgroundData;

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Data for charageki ui back ground
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrCharagekiUIBackgroundData
{
    // private.
    private string  m_pPlaceName    = "";   // place name
    private string  m_pImagePath    = "";   // image path
    private Sprite  m_pBgSprite     = null; // bg sprite

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Constructor
    public KrCharagekiUIBackgroundData(string pPlaceName, string pImagePath)
    {
        m_pPlaceName = pPlaceName;
        m_pImagePath = pImagePath;
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Get place Name
    public string GetPlaceName()
    {
        return m_pPlaceName;
    }

    // @Brief : Load
    public void LoadSprite()
    {
        m_pBgSprite = KrResources.LoadSprite(KrCharagekiDef.s_pASSET_BASE_PATH + m_pImagePath, KrCharagekiDef.IsLoadingFromResources());
    }

    // @Brief : Get Sprite
    public Sprite GetSprite()
    {
        return m_pBgSprite;
    }

    // @Brief : Unload sprite
    public void UnloadSprite()
    {
        MonoBehaviour.Destroy(m_pBgSprite.texture);
        MonoBehaviour.Destroy(m_pBgSprite);
        m_pBgSprite = null;
    }
}

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Background of charageki ui
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrCharagekiUIBackground : KrICharagekiUI
{
    // private.
    private System.Action               m_cbShow                = null;     // callback for show background. NOTE : externally registered processing
    private System.Action               m_cbHide                = null;     // callback for hide background. NOTE : externally registered processing
    private System.Action<Sprite>       m_cbSetBackground       = null;     // callback for set background. NOTE : externally registered processing
    private Dictionary<uint, BgData>    m_pBackgroundDic        = null;     // dictionary for back ground        
    private BgData                      m_pCurrentBackground    = null;     // current background

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief  : Construcor
    // @Return : KrCharagekiUITitle instance
    public KrCharagekiUIBackground()
    {
        m_cbShow = null;
        m_cbHide = null;
        m_cbSetBackground = null;
        m_pBackgroundDic = new Dictionary<uint, BgData>();
        m_pCurrentBackground = null;
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Load
    // @Param : uId     => Background id
    public void Load(uint uId)
    {
        Dictionary<uint, BgData> pMasterDic = KrCharagekiDef.s_pBACK_GROUNDS;
        KrDebug.Assert(pMasterDic.ContainsKey(uId), "background master is not found key = " + uId, typeof(KrCharagekiUIBackground));
        BgData pData = pMasterDic[uId];
        pData.LoadSprite();
        m_pBackgroundDic.Add(uId, pData);
    }

    // @Brief : Unload
    public void Unload()
    {
        if(m_pBackgroundDic != null)
        {
            foreach(KeyValuePair<uint, BgData> pKeyValuePair in m_pBackgroundDic)
            {
                pKeyValuePair.Value.UnloadSprite();
            }
        }
    }

    // @Brief : Set sprite
    // @Param : uId     => Background id
    public void SetSprite(uint uId)
    {
        KrDebug.Assert(m_pBackgroundDic.ContainsKey(uId), "background master is not found key = " + uId, typeof(KrCharagekiUIBackground));
        m_pCurrentBackground = m_pBackgroundDic[uId];
        if(m_cbSetBackground != null)
        {
            m_cbSetBackground(m_pCurrentBackground.GetSprite());
        }
    }

    // @Brief   : Get place name
    // @Return  : Place name
    public string GetPlaceName()
    {
        KrDebug.Assert(m_pCurrentBackground != null, "m_pCurrentBackground is null", typeof(KrCharagekiUIBackground));
        return m_pCurrentBackground.GetPlaceName();
    }

    // @Brief : Show
    public void Show()
    {
        if(m_cbShow != null)
        {
            m_cbShow();
        }
    }

    // @Brief : Hide
    public void Hide()
    {
        if(m_cbHide != null)
        {
            m_cbHide();
        }
    }

    // @Brief : Register background
    // @Param : cbSetBackground => Callback for set background
    //        : cbShow          => Callback for show title 
    //        : cbHide          => Callback for hide title
    public void RegisterBackground(System.Action<Sprite> cbSetBackground, System.Action cbShow, System.Action cbHide)
    {
        m_cbSetBackground = cbSetBackground;
        m_cbShow = cbShow;
        m_cbHide = cbHide;
    }
}

