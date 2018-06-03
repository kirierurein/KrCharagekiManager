using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Container for charageki scenario
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrCharagekiScenarioContainer
{
    // private.
    private Dictionary<string, KrCsvData>   m_pScenarioDataDic      = null;     // dictionary for conversation data
    private KrCsvData                       m_pCurrentScenario      = null;     // currently set scenario

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief  : Constructor
    // @Return : KrCharagekiScenarioContainer instance
    public KrCharagekiScenarioContainer()
    {
        m_pScenarioDataDic = new Dictionary<string, KrCsvData>();
        m_pCurrentScenario = null;
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Load scenario
    // @Param : pKey    => Key to register csv
    //          pPath   => Csv path
    public void LoadScenario(string pKey, string pPath)
    {
        KrCsvData pCsvData = KrCsvLoader.Load(KrCharagekiDef.s_pASSET_BASE_PATH + pPath, KrCharagekiDef.IsLoadingFromResources());
        m_pScenarioDataDic.Add(pKey, pCsvData);
    }

    // @Brief : Set the current scenario
    // @Param : pKey    => csv registered key
    public void SettingScenario(string pKey)
    {
        m_pCurrentScenario = m_pScenarioDataDic[pKey];
    }

    // @Brief  : Get scenario
    // @Return : Scenario csv data
    public KrCsvData GetScenario()
    {
        return m_pCurrentScenario;
    }

    // @Brief  : Get scenario
    // @Param  : pValue         => Value
    //         : pColumnName    => Column name
    // @Return : row of Scenario csv
    public KrCsvDataRow GetScenario(string pValue, string pColumnName)
    {
        return m_pCurrentScenario.GetValues(pValue, pColumnName);
    }
}

