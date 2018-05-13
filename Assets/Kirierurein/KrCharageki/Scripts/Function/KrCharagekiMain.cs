using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Charageki script function for main script
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrCharagekiMain : KrCharagekiFunction
{
    // const. 
    private const string                c_pSECTION          = "section";        // section declaration of one action

    // private.
    private List<KrCharagekiSection>    c_pSectionScripts   = null;             // list of scripts by section

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Constructor
    public KrCharagekiMain() : base()
    {
        c_pSectionScripts = new List<KrCharagekiSection>();
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief  : Get main sections
    // @Return : List of charageki section
    public List<KrCharagekiSection>  GetMainSections()
    {
        return c_pSectionScripts;
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
        if(pStr.Equals(c_pSECTION + ":"))
        {
            KrDebug.Log("New section", typeof(KrCharagekiMain));
            KrCharagekiSection pSection = new KrCharagekiSection();
            pSection.Add(pReader, pScript);
            c_pSectionScripts.Add(pSection);
        }
    }
}

