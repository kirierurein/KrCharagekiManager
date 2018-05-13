using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Charageki script function for resource loding
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrCharagekiResource : KrCharagekiFunction
{
    // private.
    private List<string> m_pResourceFilePaths   = null;     // resource file path list
    
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Constructor
    public KrCharagekiResource() : base()
    {
        m_pResourceFilePaths = new List<string>();
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief  : Get resource paths
    // @Return : List of resource path
    public List<string> GetDonwloadResourcePaths()
    {
        return m_pResourceFilePaths;
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PROTECTED FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Add resource
    // @Param : pReader => Stream reader
    //        : pScript => Script container
    //        : pStr    => String of one line
    protected override void Add(StreamReader pReader, KrCharagekiScript pScript, string pStr)
    {
        pStr = pScript.ConvertConstVariable(pStr);
        KrDebug.Log("Add resource path => " + pStr, typeof(KrCharagekiResource));
        m_pResourceFilePaths.Add(pStr);
    }
}

