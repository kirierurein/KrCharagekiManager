using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Charageki script main function for section declaration of one action
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrCharagekiSection : KrCharagekiCommandContainer
{
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Constructor
    public KrCharagekiSection() : base()
    {
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PROTECTED FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Add initialize command
    // @Param : pReader => Stream reader
    //        : pScript => Script container
    //        : pStr => String of one line
    protected override void Add(StreamReader pReader, KrCharagekiScript pScript, string pStr)
    {
        
        KrCharagekiCommand pCommand = null;

        // Create initialize comannd
        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        // SET COMMENT
        if(pStr.IndexOf("set_text") == 0)
        {
            pCommand = new KrCharagekiCommandSetText();
        }
        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        // SHOW TEXT
        else if(pStr.IndexOf("show_text") == 0)
        {
            pCommand = new KrCharagekiCommandShowText();
        }
        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        // SET SCENARIO
        else if(pStr.IndexOf("set_scenario") == 0)
        {
            pCommand = new KrCharagekiCommandSetScenario();
        }
        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        // SET BACK GROUND
        else if(pStr.IndexOf("set_bg") == 0)
        {
            pCommand = new KrCharagekiCommandSetSpriteBg();
        }
        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        // SHOW BACK GROUND
        else if(pStr.IndexOf("show_bg") == 0)
        {
            pCommand = new KrCharagekiCommandShowBg();
        }
        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        // HIDE BACK GROUND
        else if(pStr.IndexOf("hide_bg") == 0)
        {
            pCommand = new KrCharagekiCommandHideBg();
        }
        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        // SET TITLE NAME
        else if(pStr.IndexOf("set_title") == 0)
        {
            pCommand = new KrCharagekiCommandSetTitle();
        }
        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        // SHOW TITLE
        else if(pStr.IndexOf("show_title") == 0)
        {
            pCommand = new KrCharagekiCommandShowTitle();
        }
        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        // HIDE TITLE
        else if(pStr.IndexOf("hide_title") == 0)
        {
            pCommand = new KrCharagekiCommandHideTitle();
        }
        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        // FADE OUT
        else if(pStr.IndexOf("fade_out") == 0)
        {
            pCommand = new KrCharagekiCommandFadeOut();
        }
        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        // FADE IN
        else if(pStr.IndexOf("fade_in") == 0)
        {
            pCommand = new KrCharagekiCommandFadeIn();
        }
        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        // SET WAIT INPUT
        else if(pStr.IndexOf("wait_input") == 0)
        {
            pCommand = new KrCharagekiCommandWaitInput();
        }
        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        // SET WAIT TIME
        else if(pStr.IndexOf("wait_time") == 0)
        {
            pCommand = new KrCharagekiCommandWaitTime();
        }
        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        // SHOW CHARACTER
        else if(pStr.IndexOf("show_chara") == 0)
        {
            pCommand = new KrCharagekiCommandShowCharacter();
        }
        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        // HIDE CHARACTER
        else if(pStr.IndexOf("hide_chara") == 0)
        {
            pCommand = new KrCharagekiCommandHideCharacter();
        }
        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        // CHARACTER ACTION
        else if(pStr.IndexOf("chara_action") == 0)
        {
            pCommand = new KrCharagekiCommandCharacterAction();
        }
        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        // SET CHARACTER POSITION
        else if(pStr.IndexOf("chara_position") == 0)
        {
            pCommand = new KrCharagekiCommandSetCharacterPosition();
        }
        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        // PLAY SE
        else if(pStr.IndexOf("play_se") == 0)
        {
            pCommand = new KrCharagekiCommandPlaySe();
        }
        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        // PLAY BGM
        else if(pStr.IndexOf("play_bgm") == 0)
        {
            pCommand = new KrCharagekiCommandPlayBgm();
        }
        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        // NOT SUPPORTED
        else
        {
            KrDebug.Assert(false, "Not Supported section command => " + pStr, typeof(KrCharagekiInitialize));
        }

        if(pCommand != null)
        {
            KrDebug.Log("    Add Section command => " + pStr, typeof(KrCharagekiSection));
            pCommand.Load(pScript, pStr);
            m_pCommands.Add(pCommand);
        }
    }
}

