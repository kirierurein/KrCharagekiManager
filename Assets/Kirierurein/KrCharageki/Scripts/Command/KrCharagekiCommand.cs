using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Charageki script command
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public abstract class KrCharagekiCommand
{
    // @Brief : Option for charageki script command
    protected class KrCharagekiCommandOption
    {
        // public
        public string m_pKey    = "";   // option key
        public string m_pValue  = "";   // option value

        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        // CONSTRUCTOR
        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        // @Brief : Constructor
        // @Param : pKey    => String of command key
        //        : pValue  => String of command value
        public KrCharagekiCommandOption(string pKey, string pValue)
        {
            m_pKey = pKey;
            m_pValue = pValue;
        }
    }

    // protected.
    protected List<KrCharagekiCommandOption> m_pOptions = null;     // command option list

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Constructor
    public KrCharagekiCommand()
    {
        m_pOptions = new List<KrCharagekiCommandOption>();
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Load command
    // @Param : pScript     => Script container
    //        : pCommand    => String of command
    public void Load(KrCharagekiScript pScript, string pCommand)
    {
        string[] pSplit = pCommand.Split(new char[]{' '}, System.StringSplitOptions.RemoveEmptyEntries);
        KrDebug.Log("Add command => " + pSplit[0], typeof(KrCharagekiCommand));
        // Since array 0 is the command name, it starts from the first
        for(int sIndex = 1; sIndex < pSplit.Length; sIndex++)
        {
            KrDebug.Log("    option => " + pSplit[sIndex], typeof(KrCharagekiCommand));

            string[] pKeyValue = pSplit[sIndex].Split(new char[]{'='});
            KrCharagekiCommandOption pOption = new KrCharagekiCommandOption(pKeyValue[0], pScript.ConvertConstVariable(pKeyValue[1]));
            m_pOptions.Add(pOption);
        }
    }

    // @Brief : Execution command
    // @Param : pManager    => Charageki manager
    public abstract void Exec(KrCharagekiManager pManager);
}

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Charageki script command for option of id
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public abstract class KrCharagekiCommandIdOption : KrCharagekiCommand
{
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Constructor
    public KrCharagekiCommandIdOption() : base()
    {
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Execution command
    // @Param : pManager    => Charageki manager
    public override void Exec(KrCharagekiManager pManager)
    {
        uint sId = 0;
        for(int sIndex = 0; sIndex < m_pOptions.Count; sIndex++)
        {
            //:::::::::::::::::::::::::::::::::::::::::::::::::::
            // ID
            if(m_pOptions[sIndex].m_pKey == "id")
            {
                bool bIsSuccess = uint.TryParse(m_pOptions[sIndex].m_pValue, out sId);
                KrDebug.Assert(bIsSuccess, "Parsing failed to type unsigned int = " + m_pOptions[sIndex].m_pValue, typeof(KrCharagekiCommandSetText));
            }
        }

        KrDebug.Assert(sId > 0, "there is no id!!", typeof(KrCharagekiCommandSetText));
        Exec(sId, pManager);
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PROTECTED FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Execution with option
    // @Param : uId         => Id
    //        : pManager    => Charageki manager
    protected abstract void Exec(uint uId, KrCharagekiManager pManager);
}

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. May 2018
// @Brief : Charageki script command for option of path
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public abstract class KrCharagekiCommandPathOption : KrCharagekiCommand
{
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Constructor
    public KrCharagekiCommandPathOption() : base()
    {
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Execution command
    // @Param : pManager    => Charageki manager
    public override void Exec(KrCharagekiManager pManager)
    {
        string pPath = "";
        for(int sIndex = 0; sIndex < m_pOptions.Count; sIndex++)
        {
            //:::::::::::::::::::::::::::::::::::::::::::::::::::
            // PATH
            if(m_pOptions[sIndex].m_pKey == "path")
            {
                pPath = m_pOptions[sIndex].m_pValue;
            }
        }
        KrDebug.Assert(!string.IsNullOrEmpty(pPath), "there is no path!!", typeof(KrCharagekiCommandPathOption));
        Exec(pPath, pManager);
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PROTECTED FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Execution with option
    // @Param : pPath       => Path
    //        : pManager    => Charageki manager
    protected abstract void Exec(string pPath, KrCharagekiManager pManager);
}
