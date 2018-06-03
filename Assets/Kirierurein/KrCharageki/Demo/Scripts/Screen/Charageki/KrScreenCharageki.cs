using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Feb 2018
// @Brief :  Screen for charageki
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrScreenCharageki : KrScreen
{
    // private inspector.
    [Header("InputParam")]
    [SerializeField]
    private string[]                                    scriptPaths         = null;                                                 // script paths
    [SerializeField]
    private KrCharagekiUICharacterContainer.eVIEW_MODE  charagekiMode       = KrCharagekiUICharacterContainer.eVIEW_MODE.LIVE2D;    // view mode

    [Header("Ui")]
    [SerializeField]
    private RectTransform           rectTransform       = null;             // rect transform
    [SerializeField]
    private Vector3                 openPosition        = new Vector3();    // open menu position
    [SerializeField]
    private Vector3                 closePosition       = new Vector3();    // close menu position
    [SerializeField]
    private float                   menuLeapTime        = 1.0f;             // menu animation time
    [SerializeField]
    private Button                  menu                = null;             // menu button
    [SerializeField]
    private Button                  auto                = null;             // auto screen
    [SerializeField]
    private Button                  skip                = null;             // skip button
    [SerializeField]
    private Transform               charaParent         = null;             // character parent
    [SerializeField]
    private GameObject              commentObject       = null;             // comment object
    [SerializeField]
    private Text                    V_CHARA_NAME        = null;             // text for character name
    [SerializeField]
    private Text                    V_COMMENT           = null;             // text for comment
    [SerializeField]
    private Image                   backgroundImage     = null;             // image for background
    [SerializeField]
    private Text                    V_TITLE_TEXT        = null;             // text for title
    [SerializeField]
    private Image                   fadeImage           = null;             // image for fade
    [SerializeField]
    private Button                  tapScreen           = null;             // tap screen

    // private.
    private bool                    m_bIsOpen           = false;            // is open menu
    private KrVectorValueAnimator   m_pValueAnimator    = null;             // menu open & close value animator

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // UNITY FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Breif : Update
    void Update ()
    {   
        if(m_pValueAnimator != null)
        {
            m_pValueAnimator.Update();
            rectTransform.anchoredPosition = m_pValueAnimator.GetVertor3();
        }
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Initialize
    // @Param : pParam  => Arguments for initialize 
    public override void Initialize(KrUIArgumentParameter pParam)
    {
        KrCharagekiManager pManager = KrCharagekiManager.Create(transform, scriptPaths, charaParent, charagekiMode, 1.0f);
        KrCharagekiUIController pUIController = pManager.GetUIController();

        // AddListener tap screen
        tapScreen.onClick.AddListener(() => 
        {
            pManager.TapScreen();
        });

        // Register fade process
        pUIController.RegisterFade(KrValueAnimator.eEASE.NONE, 1.0f, 1.0f, fadeImage.gameObject, (float fFadeValue) =>
        {
            Color pColor = fadeImage.color;
            pColor.a = fFadeValue;
            fadeImage.color = pColor;
        });

        // Register title process
        pUIController.RegisterTitle((string pStr) =>
        {
            V_TITLE_TEXT.text = pStr;
        },
        () => {V_TITLE_TEXT.gameObject.SetActive(true);},
        () => {V_TITLE_TEXT.gameObject.SetActive(false);});

        // Register background process
        pUIController.RegisterBackground((Sprite pBackground) =>
        {
            backgroundImage.sprite = pBackground;
            backgroundImage.SetNativeSize();
        },
        () => {backgroundImage.gameObject.SetActive(true);},
        () => {backgroundImage.gameObject.SetActive(false);});

        // Register text area process
        pUIController.RegisterTextArea(0.1f, (string pCharaName) =>
        {
            V_CHARA_NAME.text = pCharaName;
        },
        (string pComment) =>
        {
            V_COMMENT.text = pComment;
        },
        () => {commentObject.gameObject.SetActive(true);},
        () => {commentObject.gameObject.SetActive(false);});

        // AddListener switch menu 
        menu.onClick.AddListener(cbSwitchMenu);
        // AddListener skip 
        skip.onClick.AddListener(() => 
        {
            pUIController.ResetAutoMode();
        });
        // AddListener auto mode
        auto.onClick.AddListener(() => 
        {
            pUIController.ToggleAutoMode();
        });
    }

    // @Brief : Callback of Switch menu button
    private void cbSwitchMenu()
    {
        if(m_pValueAnimator == null || m_pValueAnimator.IsEnd())
        {
            m_bIsOpen = !m_bIsOpen;
            Vector3 vFrom = new Vector3();
            Vector3 vTo = new Vector3();
            if(m_bIsOpen)
            {
                vFrom = closePosition;
                vTo = openPosition;
            }
            else
            {
                vFrom = openPosition;
                vTo = closePosition;
            }
            m_pValueAnimator = new KrVectorValueAnimator(vFrom, vTo, menuLeapTime, KrValueAnimator.eEASE.NONE, 1.0f);
            m_pValueAnimator.Play();
        }
    }
}
