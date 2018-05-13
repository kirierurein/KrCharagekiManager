using UnityEngine;
using System.Collections;

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Charageki script command for play text
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrCharagekiCommandSetText : KrCharagekiCommandIdOption
{
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Constructor
    public KrCharagekiCommandSetText() : base()
    {
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PROTECTED FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Execution with option
    // @Param : uId         => Value of "No" column of csv
    //        : pManager    => Charageki manager
    protected override void Exec(uint uId, KrCharagekiManager pManager)
    {
        KrCharagekiScenarioContainer pScenarioContainer = pManager.GetScenarioContainer();
        // Get row data of csv
        KrCsvDataRow pCsvDataRow = pScenarioContainer.GetScenario(uId.ToString(), "No");

        uint uCharaId = 0;
        bool bIsSuccess = uint.TryParse(pCsvDataRow.GetValue("CharaId"), out uCharaId);
        KrDebug.Assert(bIsSuccess, "Parsing failed to type int = " + pCsvDataRow.GetValue("CharaId"), typeof(KrCharagekiCommandSetText));

        KrDebug.Assert(KrCharagekiDef.s_pCHARA_DIC.ContainsKey(uCharaId), "Invalid KrCharagekiDef.s_CHARA_NAME key = " + uCharaId, typeof(KrCharagekiCommandSetText));
        string pCharaName = KrCharagekiDef.s_pCHARA_DIC[uCharaId].GetCharacterName();
        string pComment = pCsvDataRow.GetValue("Comment");

        KrDebug.Log("Play Text : " + "CharaId" + " = " + pCharaName + ", " + "Comment" + " = " + pComment, typeof(KrCharagekiCommandSetText));
        KrCharagekiUIController pUIController = pManager.GetUIController();
        // Set text
        float fReadingTime = pUIController.SetText(pCharaName, pComment);

        // Play voice
        string pVoiceName = pCsvDataRow.GetValue("VoiceId");
        KrDebug.Log("Play Voice: " + "CharaId" + " = " + pCharaName + ", " + "VoiceId" + " = " + pVoiceName, typeof(KrCharagekiCommandSetText));
        KrAudioSource pAudioSource = pUIController.PlayVoice(uCharaId, pVoiceName, pManager);

        if(pAudioSource == null)
        {
            // If there is no voice data, set the lip sync time from the length of the text
            pUIController.PlayLipSync(uCharaId, fReadingTime, pComment);
        }

        // Set log
        KrCharagekiLog pLog = new KrCharagekiLog(pCharaName, pComment);
        KrCharagekiLogContainer pLogContainer = pManager.GetLogContainer();
        pLogContainer.AddLog(pLog);
    }
}

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Charageki script command for set the current scenario
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrCharagekiCommandSetScenario : KrCharagekiCommand
{
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Constructor
    public KrCharagekiCommandSetScenario() : base()
    {
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Execution command
    // @Param : pManager    => Charageki manager
    public override void Exec(KrCharagekiManager pManager)
    {
        string pKey = "";
        for(int sIndex = 0; sIndex < m_pOptions.Count; sIndex++)
        {
            //:::::::::::::::::::::::::::::::::::::::::::::::::::
            // KEY
            if(m_pOptions[sIndex].m_pKey == "key")
            {
                pKey = m_pOptions[sIndex].m_pValue;
            }
        }
        KrDebug.Assert(!string.IsNullOrEmpty(pKey), "there is no key!!", typeof(KrCharagekiCommandLoadScenario));

        KrCharagekiScenarioContainer pScenarioContainer = pManager.GetScenarioContainer();
        KrDebug.Log("Set scenario = " + pKey, typeof(KrCharagekiCommandLoadScenario));
        // set the current scenario
        pScenarioContainer.SettingScenario(pKey);
    }
}

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Charageki script command for set background sprite
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrCharagekiCommandSetSpriteBg : KrCharagekiCommandIdOption
{
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Constructor
    public KrCharagekiCommandSetSpriteBg() : base()
    {
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PROTECTED FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Execution with option
    // @Param : uId         => Background id
    //        : pManager    => Charageki manager
    protected override void Exec(uint uId, KrCharagekiManager pManager)
    {
        KrDebug.Log("Set bg : sId = " + uId, typeof(KrCharagekiCommandSetSpriteBg));
        KrCharagekiUIController pUIController = pManager.GetUIController();
        pUIController.SetBg(uId);
    }
}

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. May 2018
// @Brief : Charageki script command for show background
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrCharagekiCommandShowBg : KrCharagekiCommand
{
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Constructor
    public KrCharagekiCommandShowBg() : base()
    {
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Execution command
    // @Param : pManager    => Charageki manager
    public override void Exec(KrCharagekiManager pManager)
    {
        KrDebug.Log("Show bg : ", typeof(KrCharagekiCommandSetSpriteBg));
        KrCharagekiUIController pUIController = pManager.GetUIController();
        // MEMO : Non option
        pUIController.ShowBg();
    }
}

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. May 2018
// @Brief : Charageki script command for hide background
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrCharagekiCommandHideBg : KrCharagekiCommand
{
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Constructor
    public KrCharagekiCommandHideBg() : base()
    {
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Execution command
    // @Param : pManager    => Charageki manager
    public override void Exec(KrCharagekiManager pManager)
    {
        KrDebug.Log("Hide bg : ", typeof(KrCharagekiCommandSetSpriteBg));
        KrCharagekiUIController pUIController = pManager.GetUIController();
        // MEMO : Non option
        pUIController.HideBg();
    }
}

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Charageki script command for set title
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrCharagekiCommandSetTitle : KrCharagekiCommand
{
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Constructor
    public KrCharagekiCommandSetTitle() : base()
    {
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Execution command
    // @Param : pManager    => Charageki manager
    public override void Exec(KrCharagekiManager pManager)
    {
        KrDebug.Log("Set title name : ", typeof(KrCharagekiCommandSetSpriteBg));
        KrCharagekiUIController pUIController = pManager.GetUIController();
        // MEMO : Non option
        pUIController.SetTitle();
    }
}

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Charageki script command for show title
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrCharagekiCommandShowTitle : KrCharagekiCommand
{
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Constructor
    public KrCharagekiCommandShowTitle() : base()
    {
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Execution command
    // @Param : pManager    => Charageki manager
    public override void Exec(KrCharagekiManager pManager)
    {
        KrDebug.Log("Show title : ", typeof(KrCharagekiCommandSetSpriteBg));
        KrCharagekiUIController pUIController = pManager.GetUIController();
        // MEMO : Non option
        pUIController.ShowTitle();
    }
}

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Charageki script command for hide title
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrCharagekiCommandHideTitle : KrCharagekiCommand
{
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Constructor
    public KrCharagekiCommandHideTitle() : base()
    {
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Execution command
    // @Param : pManager    => Charageki manager
    public override void Exec(KrCharagekiManager pManager)
    {
        KrDebug.Log("Hide title : ", typeof(KrCharagekiCommandShowText));
        KrCharagekiUIController pUIController = pManager.GetUIController();
        // MEMO : Non option
        pUIController.HideTitle();
    }
}

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Charageki script command for show text
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrCharagekiCommandShowText : KrCharagekiCommand
{
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Constructor
    public KrCharagekiCommandShowText() : base()
    {
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Execution command
    // @Param : pManager    => Charageki manager
    public override void Exec(KrCharagekiManager pManager)
    {
        KrDebug.Log("Show text : ", typeof(KrCharagekiCommandShowText));
        KrCharagekiUIController pUIController = pManager.GetUIController();
        // MEMO : Non option
        pUIController.ShowText();
    }
}

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Charageki script command for fade out
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrCharagekiCommandFadeOut : KrCharagekiCommand
{
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Constructor
    public KrCharagekiCommandFadeOut() : base()
    {
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Execution command
    // @Param : pManager    => Charageki manager
    public override void Exec(KrCharagekiManager pManager)
    {
        KrDebug.Log("Fade out : ", typeof(KrCharagekiCommandFadeOut));
        KrCharagekiUIController pUIController = pManager.GetUIController();
        // MEMO : Non option
        pUIController.FadeOut();
    }
}

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Charageki script command for fade in
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrCharagekiCommandFadeIn : KrCharagekiCommand
{
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Constructor
    public KrCharagekiCommandFadeIn() : base()
    {
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Execution command
    // @Param : pManager    => Charageki manager
    public override void Exec(KrCharagekiManager pManager)
    {
        KrDebug.Log("Fade in : ", typeof(KrCharagekiCommandFadeIn));
        KrCharagekiUIController pUIController = pManager.GetUIController();
        // MEMO : Non option
        pUIController.FadeIn();
    }
}

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Charageki script command for wait input
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrCharagekiCommandWaitInput : KrCharagekiCommand
{
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Constructor
    public KrCharagekiCommandWaitInput() : base()
    {
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Execution command
    // @Param : pManager    => Charageki manager
    public override void Exec(KrCharagekiManager pManager)
    {
        bool bWait = false;
        for(int sIndex = 0; sIndex < m_pOptions.Count; sIndex++)
        {
            //:::::::::::::::::::::::::::::::::::::::::::::::::::
            // KEY
            if(m_pOptions[sIndex].m_pKey == "wait")
            {
                bool bIsSuccess = bool.TryParse(m_pOptions[sIndex].m_pValue, out bWait);
                KrDebug.Assert(bIsSuccess, "Parsing failed to type bool = " + m_pOptions[sIndex].m_pValue, typeof(KrCharagekiCommandWaitInput));
            }
        }

        KrCharagekiUIController pUIController = pManager.GetUIController();
        KrDebug.Log("Set wait input = " + bWait, typeof(KrCharagekiCommandWaitInput));
        pUIController.SetWaitInput(bWait);
    }
}

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Charageki script command for wait time
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrCharagekiCommandWaitTime : KrCharagekiCommand
{
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Constructor
    public KrCharagekiCommandWaitTime() : base()
    {
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Execution command
    // @Param : pManager    => Charageki manager
    public override void Exec(KrCharagekiManager pManager)
    {
        float fWaitTime = 0.0f;
        for(int sIndex = 0; sIndex < m_pOptions.Count; sIndex++)
        {
            //:::::::::::::::::::::::::::::::::::::::::::::::::::
            // KEY
            if(m_pOptions[sIndex].m_pKey == "time")
            {
                bool bIsSuccess = float.TryParse(m_pOptions[sIndex].m_pValue, out fWaitTime);
                KrDebug.Assert(bIsSuccess, "Parsing failed to type float = " + m_pOptions[sIndex].m_pValue, typeof(KrCharagekiCommandWaitTime));
            }
        }

        KrCharagekiUIController pUIController = pManager.GetUIController();
        KrDebug.Log("Set wait time = " + fWaitTime, typeof(KrCharagekiCommandWaitTime));
        pUIController.SetWaitTime(fWaitTime);
    }
}

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Charageki script command for show character
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrCharagekiCommandShowCharacter : KrCharagekiCommandIdOption
{
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Constructor
    public KrCharagekiCommandShowCharacter() : base()
    {
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PROTECTED FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Execution with option
    // @Param : sId         => Character id
    //        : pManager    => Charageki manager
    protected override void Exec(uint sId, KrCharagekiManager pManager)
    {
        KrCharagekiUIController pUIController = pManager.GetUIController();
        KrDebug.Log("Show character : id = " + sId, typeof(KrCharagekiCommandShowCharacter));
        pUIController.ShowCharacter(sId);
    }
}

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Charageki script command for hide character
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrCharagekiCommandHideCharacter : KrCharagekiCommandIdOption
{
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Constructor
    public KrCharagekiCommandHideCharacter() : base()
    {
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PROTECTED FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Execution with option
    // @Param : uId         => Character id
    //        : pManager    => Charageki manager
    protected override void Exec(uint uId, KrCharagekiManager pManager)
    {
        KrCharagekiUIController pUIController = pManager.GetUIController();
        KrDebug.Log("Hide character : id = " + uId, typeof(KrCharagekiCommandShowCharacter));
        pUIController.HideCharacter(uId);
    }
}

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Charageki script command for character action
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrCharagekiCommandCharacterAction : KrCharagekiCommand
{
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Constructor
    public KrCharagekiCommandCharacterAction() : base()
    {
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Execution command
    // @Param : pManager    => Charageki manager
    public override void Exec(KrCharagekiManager pManager)
    {
        uint uCharaId = 0;
        uint uActionId = 0;
        for(int sIndex = 0; sIndex < m_pOptions.Count; sIndex++)
        {
            //:::::::::::::::::::::::::::::::::::::::::::::::::::
            // CHARA ID
            if(m_pOptions[sIndex].m_pKey == "chara")
            {
                bool bIsSuccess = uint.TryParse(m_pOptions[sIndex].m_pValue, out uCharaId);
                KrDebug.Assert(bIsSuccess, "Parsing failed to type unsigned int = " + m_pOptions[sIndex].m_pValue, typeof(KrCharagekiCommandCharacterAction));
            }
            //:::::::::::::::::::::::::::::::::::::::::::::::::::
            // ACTION ID
            else if(m_pOptions[sIndex].m_pKey == "action")
            {
                bool bIsSuccess = uint.TryParse(m_pOptions[sIndex].m_pValue, out uActionId);
                KrDebug.Assert(bIsSuccess, "Parsing failed to type unsigned int = " + m_pOptions[sIndex].m_pValue, typeof(KrCharagekiCommandCharacterAction));
            }
        }

        KrDebug.Assert(uCharaId > 0, "there is no chara id!!", typeof(KrCharagekiCommandSetText));
        KrDebug.Assert(uActionId > 0, "there is no action id!!", typeof(KrCharagekiCommandSetText));
        KrCharagekiUIController pUIController = pManager.GetUIController();
        KrDebug.Log("chara action : chara = " + uCharaId + ", action = " + uActionId, typeof(KrCharagekiCommandCharacterAction));
        pUIController.SetAction(uCharaId, uActionId);
    }
}

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Charageki script command for set character position
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrCharagekiCommandSetCharacterPosition : KrCharagekiCommand
{
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Constructor
    public KrCharagekiCommandSetCharacterPosition() : base()
    {
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Execution command
    // @Param : pManager    => Charageki manager
    public override void Exec(KrCharagekiManager pManager)
    {
        uint uCharaId = 0;
        string pPositionKey = "";
        for(int sIndex = 0; sIndex < m_pOptions.Count; sIndex++)
        {
            //:::::::::::::::::::::::::::::::::::::::::::::::::::
            // CHARA ID
            if(m_pOptions[sIndex].m_pKey == "chara")
            {
                bool bIsSuccess = uint.TryParse(m_pOptions[sIndex].m_pValue, out uCharaId);
                KrDebug.Assert(bIsSuccess, "Parsing failed to type unsigned int = " + m_pOptions[sIndex].m_pValue, typeof(KrCharagekiCommandCharacterAction));
            }
            //:::::::::::::::::::::::::::::::::::::::::::::::::::
            // ACTION ID
            else if(m_pOptions[sIndex].m_pKey == "position")
            {
                pPositionKey = m_pOptions[sIndex].m_pValue;
            }
        }

        KrDebug.Assert(uCharaId > 0, "there is no chara id!!", typeof(KrCharagekiCommandSetCharacterPosition));
        KrDebug.Assert(!string.IsNullOrEmpty(pPositionKey), "there is no position!!", typeof(KrCharagekiCommandSetCharacterPosition));
        KrCharagekiUIController pUIController = pManager.GetUIController();
        KrDebug.Log("Chara action : chara = " + uCharaId + ", position = " + pPositionKey, typeof(KrCharagekiCommandSetCharacterPosition));

        KrDebug.Assert(KrCharagekiDef.s_pPOSITION_DIC.ContainsKey(pPositionKey), "Key not found from KrCharagekiDef.s_pPOSITION_DIC. key = " + pPositionKey, typeof(KrCharagekiCommandSetCharacterPosition));
        Vector3 vPosition = KrCharagekiDef.s_pPOSITION_DIC[pPositionKey];
        pUIController.SetPosition(uCharaId, vPosition);
    }
}

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. May 2018
// @Brief : Charageki script command for play se
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrCharagekiCommandPlaySe : KrCharagekiCommandPathOption
{
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Constructor
    public KrCharagekiCommandPlaySe() : base()
    {
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PROTECTED FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Execution with option
    // @Param : pPath       => Path
    //        : pManager    => Charageki manager
    protected override void Exec(string pPath, KrCharagekiManager pManager)
    {
        KrDebug.Log("Play se : path = " + pPath, typeof(KrCharagekiCommandPlaySe));
        pManager.PlaySe(pPath);
    }
}

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. May 2018
// @Brief : Charageki script command for play bgm
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrCharagekiCommandPlayBgm : KrCharagekiCommandPathOption
{
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Constructor
    public KrCharagekiCommandPlayBgm() : base()
    {
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PROTECTED FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Execution with option
    // @Param : pPath       => Path
    //        : pManager    => Charageki manager
    protected override void Exec(string pPath, KrCharagekiManager pManager)
    {
        KrDebug.Log("Play bgm : path = " + pPath, typeof(KrCharagekiCommandPlayBgm));
        pManager.PlayBgm(pPath);
    }
}
