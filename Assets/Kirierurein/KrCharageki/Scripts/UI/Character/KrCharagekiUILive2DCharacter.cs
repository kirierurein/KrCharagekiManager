using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Data for charageki ui live2d character
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrCharagekiUILive2DCharacter : KrCharagekiUICharacter
{
    // const.
    private const string                c_pPREFAB_PATH  = "Prefabs/Live2DCharacter";  // prefab path

    // private.
    private KrLive2DModel               m_pLive2DModel  = null;                             // live 2d model
    private Dictionary<uint, string>    m_MotionDic     = null;                             // dictionary of live 2d motion data

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Set position
    // @Param : vPosition   => Character position
    public override void SetPosition(Vector3 vPosition)
    {
        m_pLive2DModel.SetPosition(vPosition);
    }

    // @Brief : Play animation
    // @Param : sActionId => Action id
    public override void PlayAction(uint sActionId)
    {
        m_pLive2DModel.SetMotion(m_MotionDic[sActionId], false);
    }

    // @Brief  : Play voice
    // @Param  : pPath      => Asset path of audio clip
    //         : pManager   => Charageki manager
    // @Return : Audio source
    public override KrAudioSource PlayVoice(string pPath, KrCharagekiManager pManager)
    {
        KrAudioSource pAudioSource = base.PlayVoice(pPath, pManager);
        m_pLive2DModel.PlayLipSync(pAudioSource);
        return pAudioSource;
    }

    // @Brief : Manually play lip sync
    // @Param : fTime   => Talking time
    //        : pWord   => Talking word
    public override void PlayLipSync(float fTime, string pWord)
    {
        m_pLive2DModel.PlayLipSync(fTime, pWord);
    }

    // @Brief : Skip process
    public override void Skip()
    {
        m_pLive2DModel.Skip();
    }

    // @Brief : Show
    public override void Show()
    {
        gameObject.SetActive(true);
    }

    // @Brief : Hide
    public override void Hide()
    {
        gameObject.SetActive(false);
    }

    // @Brief  : Create
    // @Param  : pParent    => Object parent
    //           pData      => Character data
    // @Return : KrCharagekiUI2DCharacter instance
    public static KrCharagekiUILive2DCharacter Create(Transform pParent, KrCharagekiUICharacterData pData)
    {
        GameObject pPrefab = KrResources.LoadDataInApp<GameObject>(c_pPREFAB_PATH);
        GameObject pObject = Instantiate<GameObject>(pPrefab);
        pObject.transform.SetParent(pParent, false);
        KrCharagekiUILive2DCharacter pCharacter = pObject.GetComponent<KrCharagekiUILive2DCharacter>();
        pCharacter.Initialize(pData);
        return pCharacter;
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PROTECTED FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Initialize
    // @Param : pData   => Character data
    protected override void Initialize(KrCharagekiUICharacterData pData)
    {
        base.Initialize(pData);
        string pMocPath = KrCharagekiDef.s_pASSET_BASE_PATH + "/" + string.Format(KrCharagekiDef.s_pLIVE2D_MCO_FILE_FORMAT, pData.GetCharacterId());
        string[] pTexturePaths = new string[KrCharagekiDef.s_pLIVE2D_MODEL_TEXTURES_FORMAT.Length];
        for(int sIndex = 0; sIndex < KrCharagekiDef.s_pLIVE2D_MODEL_TEXTURES_FORMAT.Length; sIndex++)
        {
            string pTexturePath = KrCharagekiDef.s_pASSET_BASE_PATH + "/" + string.Format(KrCharagekiDef.s_pLIVE2D_MODEL_TEXTURES_FORMAT[sIndex], pData.GetCharacterId());
            pTexturePaths[sIndex] = pTexturePath;
        }
        List<string> pMotions = new List<string>();

        m_MotionDic = new Dictionary<uint, string>();
        foreach(KeyValuePair<uint, string> pKeyValue in KrCharagekiDef.s_pLIVE2D_MOTION_FILE_DIC)
        {
            string pDataPath = KrCharagekiDef.s_pASSET_BASE_PATH + "/" + string.Format(pKeyValue.Value, m_pCharaData.GetCharacterId());
            m_MotionDic.Add(pKeyValue.Key, pDataPath);
            pMotions.Add(pDataPath);
        }

        m_pLive2DModel = KrLive2DModel.Create(pMocPath, pTexturePaths, pMotions.ToArray(), transform);
        Hide();
    }
}

