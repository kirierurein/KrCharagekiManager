using UnityEngine;
using System.Collections;

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Debug
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrDebug
{
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Log
    // @Param : pLog    => Output log
    //        : pClass  => Class displayed in log
    [System.Diagnostics.Conditional("DEBUG")]
    public static void Log(object pLog, System.Type pClass)
    {
        Debug.Log("[" + pClass.ToString() + "]" + pLog);
    }

    // @Brief : Assert
    // @Param : bIsCondition    => Condition
    //        : pLog            => Output log
    //        : pClass          => Class displayed in log
    [System.Diagnostics.Conditional("DEBUG")]
    public static void Assert(bool bIsCondition, string pLog, System.Type pClass)
    {
        if(!bIsCondition)
        {
            Log( "<color=red>" + pLog + "</color>", pClass);
        }
    }

    // @Brief : Warning
    // @Param : bIsCondition    => Condition
    //        : pLog            => Output log
    //        : pClass          => Class displayed in log
    [System.Diagnostics.Conditional("DEBUG")]
    public static void Warning(bool bIsCondition, string pLog, System.Type pClass)
    {
        if(!bIsCondition)
        {
            Log( "<color=yellow>" + pLog + "</color>", pClass);
        }
    }
}
