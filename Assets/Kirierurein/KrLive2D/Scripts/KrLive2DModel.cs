using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using live2d;
using System.Linq;

//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief  : Class for displaying live2d model
// @Sample : 
//          // Create instance
//          KrLive2DModel pModel = KrLive2DModel.Create(pMocPath, pTextures, pMotion);
//          pModel.SetMotion(pMotion[0], true);
//          pModel2.SetPosition(new Vector2(5.0f, 0.0f));
//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrLive2DModel : MonoBehaviour
{
    // private.
    private Dictionary<string, byte[]>  m_pMotionDatas          = null;     // motion datas for live2d model
    private Live2DModelUnity            m_pLive2dModel          = null;     // live2d model
    private Live2DMotion                m_pMotion               = null;     // live2d motion 
    private MotionQueueManager          m_pMotionManager        = null;     // live2d motion manager
    private Live2DMotion                m_pIdleMotion           = null;     // live2d idle motion 
    private MotionQueueManager          m_pIdleMotionManager    = null;     // live2d idle motion manager
    private KrLive2DLipSync             m_pLipSync              = null;     // process for lip sync

    //:::::::::::::::::::::::::::::::::::::::::::::::::::::
    // UNITY FUNCTION
    //:::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Update
    void Update ()
    {
        // Update motion parameter
        m_pIdleMotionManager.updateParam(m_pLive2dModel);
        m_pMotionManager.updateParam(m_pLive2dModel);

        LipSync();

        m_pLive2dModel.update();

        // Render type is Live2D.L2D_RENDER_DRAW_MESH
        if(m_pLive2dModel.getRenderMode() == Live2D.L2D_RENDER_DRAW_MESH)
        {
            m_pLive2dModel.draw();
        }
    }

    // @Brief : OnDrawGizmos
    void OnDrawGizmos()
    {
        if(m_pLive2dModel != null)
        {
            float fModelWidth = m_pLive2dModel.getCanvasWidth();
            float fModelHeight = m_pLive2dModel.getCanvasHeight();
            Matrix4x4 mOrthoProjection = Matrix4x4.Ortho(0, fModelWidth, fModelWidth, 0, -50.0f, 50.0f);
            Matrix4x4 mWorldPosition = transform.localToWorldMatrix;
            mWorldPosition[13] += (fModelHeight / fModelWidth) * mWorldPosition[5];

            Gizmos.color = Color.red;
            float fWeight = (fModelWidth / fModelWidth) * transform.lossyScale.y;
            float fHeight = (fModelHeight / fModelWidth) * transform.lossyScale.y;
            Vector3[] pVertices = new Vector3[4];
            pVertices[0] = new Vector3(mWorldPosition[12] + fWeight, mWorldPosition[13] + fHeight, mWorldPosition[14]);
            pVertices[1] = new Vector3(mWorldPosition[12] - fWeight, mWorldPosition[13] + fHeight, mWorldPosition[14]);
            pVertices[2] = new Vector3(mWorldPosition[12] - fWeight, mWorldPosition[13] - fHeight, mWorldPosition[14]);
            pVertices[3] = new Vector3(mWorldPosition[12] + fWeight, mWorldPosition[13] - fHeight, mWorldPosition[14]);

            Gizmos.DrawLine(pVertices[0], pVertices[1]);
            Gizmos.DrawLine(pVertices[1], pVertices[2]);
            Gizmos.DrawLine(pVertices[2], pVertices[3]);
            Gizmos.DrawLine(pVertices[3], pVertices[0]);
        }
    }

    // @Brief : OnRenderObject
    void OnRenderObject()
    {
        // Render type is Live2D.L2D_RENDER_DRAW_MESH_NOW
        if(m_pLive2dModel.getRenderMode() == Live2D.L2D_RENDER_DRAW_MESH_NOW)
        {
            m_pLive2dModel.draw();
        }
    }

    //:::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //:::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Create
    // @Param : pMocFilePath        => Moc file path
    //        : pTexturePaths       => Texture paths
    //        : pMotionFilePaths    => Motion file paths
    //        : pParent             => Object parent
    public static KrLive2DModel Create(string pMocFilePath, string[] pTexturePaths, string[] pMotionFilePaths, Transform pParent = null)
    {
        GameObject pPrefab = KrResources.LoadDataInApp<GameObject>("Prefabs/Live2DModel");
        GameObject pObject = Instantiate(pPrefab);
        pObject.transform.SetParent(pParent);

        KrLive2DModel pLive2DModel = pObject.GetComponent<KrLive2DModel>();
        pLive2DModel.Initialize(pMocFilePath, pTexturePaths, pMotionFilePaths);

        return pLive2DModel;
    }

    // @Brief : Initialize
    // @Param : pMocFilePath        => Moc file path
    //        : pTexturePaths       => Texture paths
    //        : pMotionFilePaths    => Motion file paths
    public void Initialize(string pMocFilePath, string[] pTexturePaths, string[] pMotionFilePaths)
    {
        // Initialize live2d
        KrLive2DInitializer.Create();

        // Load model
        byte[] pMocFile = KrResources.LoadBytes(pMocFilePath);
        m_pLive2dModel = Live2DModelUnity.loadModel(pMocFile);

        // Set render mode
        // NOTE : 1. There are advantages and disadvantages depending on the type
        //        2. Each drawing timing is different
        // Ref : http://sites.cybernoids.jp/cubism2/sdk_tutorial/platform-setting/unity/csharp/render-mode
        m_pLive2dModel.setRenderMode(Live2D.L2D_RENDER_DRAW_MESH);


        byte[][] pTextures = new byte[pTexturePaths.Length][];
        for(int sIndex = 0; sIndex < pTexturePaths.Length; sIndex++)
        {
            // Load texture
            pTextures[sIndex] = KrResources.LoadBytes(pTexturePaths[sIndex]);
        }

        for(int sIndex = 0; sIndex < pTextures.Length; sIndex++)
        {
            Texture2D pTexture = new Texture2D(2048, 2048, TextureFormat.RGBA32, false);
            pTexture.LoadImage(pTextures[sIndex]);
            // Set Texture
            m_pLive2dModel.setTexture(sIndex, pTexture);
        }

        // Load Motion
        m_pMotionDatas = new Dictionary<string, byte[]>();
        for(int sIndex = 0; sIndex < pMotionFilePaths.Length; sIndex++)
        {
            m_pMotionDatas.Add(pMotionFilePaths[sIndex], KrResources.LoadBytes(pMotionFilePaths[sIndex]));
        }
        m_pMotionManager = new MotionQueueManager();
        // Create idle motion
        m_pIdleMotionManager = new MotionQueueManager();
        // NOTE : Let the 0th position of the motion list be the idle motion
        m_pIdleMotion = Live2DMotion.loadMotion(pMotionFilePaths[0]);
        m_pIdleMotion.setLoop(true);
        m_pIdleMotionManager.startMotion(m_pIdleMotion, false);

        SetPosition(Vector3.zero);
    }

    // @Brief : Set motion
    // @Param : pMotionName => Motion name
    //        : bIsLoop     => Is loop motion
    public void SetMotion(string pMotionName, bool bIsLoop)
    {
        KrDebug.Assert(m_pMotionDatas.ContainsKey(pMotionName), "Motion is not registered. key = " + pMotionName, typeof(KrLive2DModel));
        byte[] pMotionData =  m_pMotionDatas[pMotionName];
        m_pMotion = Live2DMotion.loadMotion(pMotionData);
        m_pMotion.setLoop(bIsLoop);
        m_pMotionManager.startMotion(m_pMotion, false);
    }

    // @Brief : Set position
    // @Param : vPosition   => position
    public void SetPosition(Vector3 vPosition)
    {
        transform.localPosition = vPosition;
        float fModelWidth = m_pLive2dModel.getCanvasWidth();
        float fModelHeight = m_pLive2dModel.getCanvasHeight();
        // Memo : Matrix calculation is performed to move the model from the default drawing position
        //          to the position shown in the camera
        Matrix4x4 mOrthoProjection = Matrix4x4.Ortho(0, fModelWidth, fModelWidth, 0, -50.0f, 50.0f);
        Matrix4x4 mWorldPosition = transform.localToWorldMatrix;
        // Centering on pivot
        mWorldPosition[13] += ((fModelHeight / fModelWidth) - 1.0f) * mWorldPosition[5];
        // Bottom to pivot
        mWorldPosition[13] += (fModelHeight / fModelWidth) * mWorldPosition[5];
        Matrix4x4 mPosition = mWorldPosition * mOrthoProjection;
        m_pLive2dModel.setMatrix(mPosition);
    }

    // @Brief : Play lip sync
    // @Param : pVoice  => Audio source
    public void PlayLipSync(KrAudioSource pVoice)
    {
        m_pLipSync = new KrLive2DLipSyncAudio(pVoice);
    }

    // @Brief : Play lip sync
    // @Param : fTalkingTime    => Talking time
    //        : pTalkingWord    => Talking word
    public void PlayLipSync(float fTalkingTime, string pTalkingWord)
    {
        m_pLipSync = new KrLive2DLipSyncString(fTalkingTime, pTalkingWord);
    }

    // @Brief : Reset lip sync
    public void Skip()
    {
        if(m_pLipSync != null)
        {
            m_pLipSync.Skip();
        }
    }

    //:::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PRIVATE FUNCTION
    //:::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Lip sync
    private void LipSync()
    {
        if(m_pLipSync != null)
        {
            m_pLipSync.Update();
            float fValue = m_pLipSync.GetValue();
            if(m_pLipSync.IsEnd())
            {
                m_pLipSync = null;
            }
            // Lip sync
            m_pLive2dModel.addToParamFloat ("PARAM_MOUTH_OPEN_Y", fValue, 1);
        }
    }
}
