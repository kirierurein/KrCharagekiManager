using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Charageki script
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrCharagekiScript
{
    // private.
    private KrCharagekiConst        m_pConstManager         = null;         // const manager for script
    private KrCharagekiResource     m_pResourceManager      = null;         // resource manager for script
    private KrCharagekiInitialize   m_pInitializeManager    = null;         // initialize command manager for script
    private KrCharagekiMain         m_pMainScriptManager    = null;         // main script manager for script

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief  : Constructor
    // @Return : KrCharagekiScript instance
    public KrCharagekiScript()
    {
        m_pConstManager = new KrCharagekiConst();
        m_pResourceManager = new KrCharagekiResource();
        m_pInitializeManager = new KrCharagekiInitialize();
        m_pMainScriptManager = new KrCharagekiMain();
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Load script
    // @Param : pReader => Stream reader
    public void LoadScript(StreamReader pReader)
    {
        while(pReader.Peek() > -1)
        {
            string pLineStr = pReader.ReadLine();
            //::::::::::::::::::::::::::::::::::::::::::::::::::::::::
            // CONST
            if(pLineStr.Equals(KrCharagekiFunction.c_pCONST_FUNC + ":"))
            {
                m_pConstManager.Add(pReader, this);
            }
            //::::::::::::::::::::::::::::::::::::::::::::::::::::::::
            // RESOURCE
            else if(pLineStr.Equals(KrCharagekiFunction.c_pRESOURCES_FUNC + ":"))
            {
                m_pResourceManager.Add(pReader, this);
            }
            //::::::::::::::::::::::::::::::::::::::::::::::::::::::::
            // INITIALIZE COMMAND
            else if(pLineStr.Equals(KrCharagekiFunction.c_pINITIALIZE_FUNC + ":"))
            {
                m_pInitializeManager.Add(pReader, this);
            }
            //::::::::::::::::::::::::::::::::::::::::::::::::::::::::
            // MAIN SCRIPT COMMAND
            else if(pLineStr.Equals(KrCharagekiFunction.c_pMAIN_FUNC + ":"))
            {
                m_pMainScriptManager.Add(pReader, this);
            }
        }
    }

    // @Brief  : Convert const variable to parameter
    // @Note   : If the Const variable does not exist, it returns its value as it is
    // @Param  : pStr => Key for obtaining a constant
    // @Return : Const defined characters
    public string ConvertConstVariable(string pStr)
    {
        return m_pConstManager.ConvertConstVariable(pStr);
    }

    // @Brief  : Get resource paths
    // @Return : Resource path list
    public List<string> GetResourcesPaths()
    {
        return m_pResourceManager.GetDonwloadResourcePaths();
    }

    // @Brief  : Get initialize commands
    // @Return : Initialize command list
    public List<KrCharagekiCommand> GetInitializeCommands()
    {
        return m_pInitializeManager.GetCommands();
    }

    // @Brief  : Get main sections
    // @Return : Section comannd list
    public List<KrCharagekiSection>  GetMainSections()
    {
        return m_pMainScriptManager.GetMainSections();
    }
}

