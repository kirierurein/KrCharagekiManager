using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Charageki script function for const variable
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrCharagekiConst : KrCharagekiFunction
{
    // private.
    private Dictionary<string, string>  m_pConstDic = null;     // const dictionary for script

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Constructor
    public KrCharagekiConst() : base()
    {
        m_pConstDic = new Dictionary<string, string>();
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief  : Convert const variable to parameter
    // @Note   : If the Const variable does not exist, it returns its value as it is
    // @Param  : pStr   => String of const key
    // @Return : Parsed const variable
    public string ConvertConstVariable(string pStr)
    {
        if(pStr.StartsWith("$"))
        {
            //When "$" is prefixed, it is regarded as a const value and a Dictionary is searched
            string pKey = pStr.Substring(1, (pStr.Length - 1));
            if(m_pConstDic.ContainsKey(pKey))
            {
                return m_pConstDic[pKey];
            }
        }

        return pStr;
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PROTECTED FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Add const
    // @Param : pReader => Stream reader
    //        : pScript => Script container
    //        : pStr    => String of one line
    protected override void Add(StreamReader pReader, KrCharagekiScript pScript, string pStr)
    {
        string[] pSplit = pStr.Split(new char[]{'=', ' '}, System.StringSplitOptions.RemoveEmptyEntries);
        KrDebug.Log("Add const variable => " + pSplit[0] + " : " + pSplit[1], typeof(KrCharagekiConst));

        if(!m_pConstDic.ContainsKey(pSplit[0]))
        {
            m_pConstDic.Add(pSplit[0], pSplit[1]);
        }
        else
        {
            KrDebug.Warning(false, "Duplicate key defined by Const. " + pSplit[0] + " = " + pSplit[1], typeof(KrCharagekiConst));
        }
    }
}

