using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : 2D character of charageki ui
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrCharagekiUI2DCharacter : KrCharagekiUICharacter
{
    // const.
    private const string                c_pPREFAB_PATH      = "Prefabs/2DCharacter";  // prefab path

    // private inspector.
    [SerializeField]
    private RectTransform               rectTransform       = null; // rect transform
    [SerializeField]
    private Image                       charaImage          = null; // character iamge

    // private
    private Dictionary<uint, Sprite>    m_pActionSpriteDic  = null; // Sprite dictionary for each action

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // UNITY FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : OnDestroy
    void OnDestroy()
    {
        foreach(KeyValuePair<uint, Sprite> pKeyValue in m_pActionSpriteDic)
        {
            Destroy(pKeyValue.Value.texture);
            Destroy(pKeyValue.Value);
        }
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Set position
    // @Param : vPosition   => Character position
    public override void SetPosition(Vector3 vPosition)
    {
        rectTransform.anchoredPosition = vPosition;
    }

    // @Brief : Play animation
    // @Param : uActionId => Action id
    public override void PlayAction(uint uActionId)
    {
        Sprite pSprite = m_pActionSpriteDic[uActionId];
        charaImage.sprite = pSprite;
    }

    // @Brief  : Create
    // @Param  : pParent    => Object parent
    //           pData      => Character data
    // @Return : KrCharagekiUI2DCharacter instance
    public static KrCharagekiUI2DCharacter Create(Transform pParent, KrCharagekiUICharacterData pData)
    {
        GameObject pPrefab = KrResources.LoadDataInApp<GameObject>(c_pPREFAB_PATH);
        GameObject pObject = Instantiate<GameObject>(pPrefab);
        pObject.transform.SetParent(pParent, false);
        KrCharagekiUI2DCharacter pCharacter = pObject.GetComponent<KrCharagekiUI2DCharacter>();
        pCharacter.Initialize(pData);
        return pCharacter;
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PROTECTED FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Error check
    protected override void ErrorCheck()
    {
        KrDebug.Assert(rectTransform != null, "rectTransform is null", typeof(KrCharagekiUI2DCharacter));
        KrDebug.Assert(charaImage != null, "charaImage is null", typeof(KrCharagekiUI2DCharacter));
    }

    // @Brief : Initialize
    // @Param : pData   => Character data
    protected override void Initialize(KrCharagekiUICharacterData pData)
    {
        base.Initialize(pData);
        m_pActionSpriteDic = new Dictionary<uint, Sprite>();
        foreach(KeyValuePair<uint, string> pKeyValue in KrCharagekiDef.s_p2D_CHARA_ACTION_IMAGE)
        {
            string pDataPath = KrCharagekiDef.s_pASSET_BASE_PATH + string.Format(pKeyValue.Value, m_pCharaData.GetCharacterId());
            Sprite pSprite = KrResources.LoadSprite(pDataPath, KrCharagekiDef.IsLoadingFromResources());
            m_pActionSpriteDic.Add(pKeyValue.Key, pSprite);
        }
        Hide();
    }
}
