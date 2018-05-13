using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Def for charageki
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrCharagekiDef
{
    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // Path
    public static string s_pASSET_BASE_PATH     = "";   // asset base path
    public static string s_pSERVER_BASE_URL     = "";   // server base url

    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CHARAGEKI MASTER DATA
    // @Brief : Dicitonary for character position
    public static Dictionary<string, Vector3> s_pPOSITION_DIC  = new Dictionary<string, Vector3>()
    {
        {"left", new Vector3(-150.0f, 0.0f, 0.0f)},
        {"center", new Vector3(0.0f, 0.0f, 0.0f)},
        {"right", new Vector3(150.0f, 0.0f, 0.0f)},
    };

    // @Brief : Background master
    public static Dictionary<uint, KrCharagekiUIBackgroundData> s_pBACK_GROUNDS = new Dictionary<uint, KrCharagekiUIBackgroundData>()
    {
        {1, new KrCharagekiUIBackgroundData("demo", "Kirierurein/KrCharageki/Demo/Resources/UI/Sign_Okay.png")},
    };

    // @Brief : Character master
    public static Dictionary<uint, KrCharagekiUICharacterData> s_pCHARA_DIC = new Dictionary<uint, KrCharagekiUICharacterData>()
    {
        {1, new KrCharagekiUICharacterData(1, "ユニティちゃん")},
    };

    // @Brief : 2D character action image master
    public static Dictionary<uint, string> s_p2D_CHARA_ACTION_IMAGE = new Dictionary<uint, string>()
    {
        {1, "Kirierurein/KrCharageki/Demo/Resources/UI/{0:00}/standing_idol.png"},
        {2, "Kirierurein/KrCharageki/Demo/Resources/UI/{0:00}/standing_idol.png"},
        {3, "Kirierurein/KrCharageki/Demo/Resources/UI/{0:00}/standing_idol.png"},
        {4, "Kirierurein/KrCharageki/Demo/Resources/UI/{0:00}/standing_idol.png"},
        {5, "Kirierurein/KrCharageki/Demo/Resources/UI/{0:00}/standing_idol.png"},
        {6, "Kirierurein/KrCharageki/Demo/Resources/UI/{0:00}/standing_idol.png"},
        {7, "Kirierurein/KrCharageki/Demo/Resources/UI/{0:00}/standing_idol.png"},
    };

    // @Brief : Live2D mco file path format
    public static string s_pLIVE2D_MCO_FILE_FORMAT = "Kirierurein/KrCharageki/Demo/Resources/UI/{0:00}/Live2D/chara.moc.bytes";

    // @Brief : Live2D model texture file path formats
    public static string[] s_pLIVE2D_MODEL_TEXTURES_FORMAT = new string[]{"Kirierurein/KrCharageki/Demo/Resources/UI/{0:00}/Live2D/texture_00.png"};

    // @Brief : Live2D motion file master
    public static Dictionary<uint, string> s_pLIVE2D_MOTION_FILE_DIC = new Dictionary<uint, string>()
    {
        {1, "Kirierurein/KrCharageki/Demo/Resources/UI/{0:00}/Live2D/Motions/idle_01.mtn.bytes"},
        {2, "Kirierurein/KrCharageki/Demo/Resources/UI/{0:00}/Live2D/Motions/m_01.mtn.bytes"},
        {3, "Kirierurein/KrCharageki/Demo/Resources/UI/{0:00}/Live2D/Motions/m_02.mtn.bytes"},
        {4, "Kirierurein/KrCharageki/Demo/Resources/UI/{0:00}/Live2D/Motions/m_03.mtn.bytes"},
        {5, "Kirierurein/KrCharageki/Demo/Resources/UI/{0:00}/Live2D/Motions/m_04.mtn.bytes"},
        {6, "Kirierurein/KrCharageki/Demo/Resources/UI/{0:00}/Live2D/Motions/m_05.mtn.bytes"},
        {7, "Kirierurein/KrCharageki/Demo/Resources/UI/{0:00}/Live2D/Motions/m_06.mtn.bytes"},
    };
}

