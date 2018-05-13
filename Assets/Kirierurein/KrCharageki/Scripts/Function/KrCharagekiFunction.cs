using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Charageki script function
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public abstract class KrCharagekiFunction
{
    // const.
    public      const string        c_pEND_FUNC         = "end";        // end function name for charageki script
    public      const string        c_pCONST_FUNC       = "const";      // const function name for charageki script
    public      const string        c_pRESOURCES_FUNC   = "resources";  // resources function name for charageki script
    public      const string        c_pINITIALIZE_FUNC  = "initialize"; // initialize function name for charageki script
    public      const string        c_pMAIN_FUNC        = "main";       // main function name for charageki script
    protected   const string        c_pCOMMENT          = "//";         // comment command for charageki script

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Constructor
    public KrCharagekiFunction()
    {
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Add
    // @Param : pReader => Stream reader
    //        : pScript => Script container
    public void Add(StreamReader pReader, KrCharagekiScript pScript)
    {
        while(pReader.Peek() > -1)
        {
            string pLineStr = pReader.ReadLine();

            // Trim unwanted tabs
            pLineStr = pLineStr.Trim();
            pLineStr = pLineStr.Replace("\"", ""); 

            // In the case of "end", the function terminates
            if(pLineStr.Equals(c_pEND_FUNC))
                break;

            if(!(pLineStr.IndexOf(c_pCOMMENT) == 0) && !string.IsNullOrEmpty(pLineStr))
            {
                Add(pReader, pScript, pLineStr);
            }
        }
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PROTECTED FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Add
    // @Param : pReader => Stream reader
    //        : pScript => Script container
    //        : pStr    => String of one line
    protected abstract void Add(StreamReader pReader, KrCharagekiScript pScriptContainer, string pStr); 
}

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Charageki script function for command container
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public abstract class KrCharagekiCommandContainer : KrCharagekiFunction
{
    // protected.
    protected List<KrCharagekiCommand>    m_pCommands   = null;             // command list 

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Constructor
    public KrCharagekiCommandContainer() : base()
    {
        m_pCommands = new List<KrCharagekiCommand>();
    }
     
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Get initialize commands
    public List<KrCharagekiCommand> GetCommands()
    {
        return m_pCommands;
    }
}

