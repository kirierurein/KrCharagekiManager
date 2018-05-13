using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Charageki log
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrCharagekiLog
{
    // private.
    private string      m_pCharaName    = "";   // Character name
    private string      m_pComment      = "";   // Comment

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Constructor
    // @Param : pCharaName  => Character name
    //        :             => Comment
    public KrCharagekiLog(string pCharaName, string pComment)
    {
        m_pCharaName = pCharaName;
        m_pComment = pComment;
    }
}

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Container for charageki log
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrCharagekiLogContainer
{
    // private.
    private List<KrCharagekiLog>    m_pCharagekiLogList = null;     // log list

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief  : Constructor
    // @Return : KrCharagekiLogContainer instance
    public KrCharagekiLogContainer()
    {
        m_pCharagekiLogList = new List<KrCharagekiLog>();
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Add log
    // @Param : pLog    => log of charageki
    public void AddLog(KrCharagekiLog pLog)
    {
        m_pCharagekiLogList.Add(pLog);
    }

    // @Brief  : Get log list
    // @Return : Log list
    public List<KrCharagekiLog> GetLogList()
    {
        return m_pCharagekiLogList;
    }
}

