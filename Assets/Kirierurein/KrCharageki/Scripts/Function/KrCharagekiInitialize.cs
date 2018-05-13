using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Charageki script function for initialize
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrCharagekiInitialize : KrCharagekiCommandContainer
{
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Constructor
    public KrCharagekiInitialize() : base()
    {
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PROTECTED FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Add initialize command
    // @Param : pReader => Stream reader
    //        : pScript => Script container
    //        : pStr    => String of one line
    protected override void Add(StreamReader pReader, KrCharagekiScript pScript, string pStr)
    {
        KrCharagekiCommand pCommand = null;

        // Create initialize comannd
        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        // LOAD SCENARIO
        if(pStr.IndexOf("load_scenario") == 0)
        {
            pCommand = new KrCharagekiCommandLoadScenario();
        }
        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        // LOAD BGM
        else if(pStr.IndexOf("load_bgm") == 0)
        {
            pCommand = null;
        }
        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        // LOAD SPRITE
        else if(pStr.IndexOf("load_bg") == 0)
        {
            pCommand = new KrCharagekiCommandLoadBg();
        }
        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        // LOAD CHARACTER
        else if(pStr.IndexOf("load_chara") == 0)
        {
            pCommand = new KrCharagekiCommandLoadCharacter();
        }
        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        // NOT SUPPORTED
        else
        {
            KrDebug.Assert(false, "Not Supported initialize command => " + pStr, typeof(KrCharagekiInitialize));
        }

        if(pCommand != null)
        {
            KrDebug.Log("Add initialize command => " + pStr, typeof(KrCharagekiInitialize));
            pCommand.Load(pScript, pStr);
            m_pCommands.Add(pCommand);
        }
    }
}

