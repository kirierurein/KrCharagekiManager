using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Csv loader
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrCsvLoader 
{
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Csv Load
    // @Param : pPath   => Asset path
    public static KrCsvData Load(string pPath)
    {
        KrCsvData pCsvData = new KrCsvData();
        StreamReader pStreamReader = KrResources.LoadText(pPath);
        KrDebug.Log("Load csv. path = " + pPath, typeof(KrCsvData));
        if(pStreamReader.Peek() > -1)
        {
            string pOneLineColumnNames = pStreamReader.ReadLine();
            KrDebug.Log("ColumnName = " + pOneLineColumnNames, typeof(KrCsvData));
            // Setting column names
            string[] pColmnNames = pOneLineColumnNames.Split(new char[]{','});
            pCsvData.SetColumnNames(pColmnNames);
        }

        string pOneLineValues = "";
        while(pStreamReader.Peek() > -1)
        {
            pOneLineValues += pStreamReader.ReadLine();
            // Setting values
            string[] pSplit = pOneLineValues.Split(new char[]{','});
            if(pSplit.Length >= pCsvData.GetColumnNum())
            {
                //Csv treats " as two minutes
                pOneLineValues = pOneLineValues.Replace("\"\"",  "\"");
                KrDebug.Log(pOneLineValues, typeof(KrCsvData));
                pCsvData.SetRow(pSplit);
                pOneLineValues = "";
            }
            else
            {
                pOneLineValues += System.Environment.NewLine;
            }
        }
        pStreamReader.Close();

        return pCsvData;
    }
}

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Csv data
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrCsvData
{
    // private.
    private string[]                m_pColumnNames      = null;     // array of column names
    private List<KrCsvDataRow>      m_pRows             = null;     // rows of csv data

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Constructor
    public KrCsvData()
    {
        m_pColumnNames = null;
        m_pRows = new List<KrCsvDataRow>();
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Set column names
    // @Param : pColumnNames    => Array of column name
    public void SetColumnNames(string[] pColumnNames)
    {
        m_pColumnNames = pColumnNames;
    }

    // @Brief : Set row values
    // @Param : pValues => Array of column value
    public void SetRow(string[] pValues)
    {
        KrCsvDataRow pRow = new KrCsvDataRow(m_pColumnNames, pValues);
        m_pRows.Add(pRow);
    }


    // @Brief  : Get values
    // @Param  : pRow    => Index of row
    // @Return : Row data
    public KrCsvDataRow GetValues(int pRow)
    {
        KrDebug.Assert((pRow >= 0 && pRow < m_pRows.Count), "[KrCsvData] row out of range. row = " + pRow, typeof(KrCsvData));
        return m_pRows[pRow];
    }

    // @Brief  : Get values
    // @Param  : pValue         => Cell value
    //         : pColumnName    => Column Name
    // @Return : Row data
    public KrCsvDataRow GetValues(string pValue, string pColumnName)
    {
        int sColumn = GetColumnIndex(pColumnName);
        for(int sIndex = 0; sIndex < m_pRows.Count; sIndex++)
        {
            if(m_pRows[sIndex].CheckValueEqual(sColumn, pValue))
            {
                return m_pRows[sIndex];
            }
        }
        return null;
    }

    // @Brief  : Get value
    // @Param  : sRow           => Index of row
    //         : pColumnName    => Column Name
    // @Return : Cell valume
    public string GetValue(int sRow, string pColumnName)
    {
        int sColumn = GetColumnIndex(pColumnName);
        return GetValue(sRow, sColumn);
    }

    // @Brief  : Get value
    // @Param  : pRowData       => Row data
    //         : pColumnName    => Column Name
    // @Return : Cell value
    public string GetValue(KrCsvDataRow pRowData, string pColumnName)
    {
        int sColumn = GetColumnIndex(pColumnName);
        return pRowData.GetValue(sColumn);
    }

    // @Brief  : Get value
    // @Param  : sRow    => Index of row
    //         : sColumn => Index if column
    // @Return : Cell value
    public string GetValue(int sRow, int sColumn)
    {
        KrCsvDataRow pRowData = GetValues(sRow);
        return pRowData.GetValue(sColumn);
    }

    // @Brief  : Get column num
    // @Return : Length of column
    public int GetColumnNum()
    {
        return m_pColumnNames.Length;
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PRIVATE
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief  : Getcolumn index
    // @Param  : pColumnName    => Column name
    // @Return : Column index
    private int GetColumnIndex(string pColumnName)
    {
        int sColumn = -1;
        sColumn = System.Array.FindIndex<string>(m_pColumnNames, (pStr) => { return pStr == pColumnName; });
        KrDebug.Assert(sColumn >= 0, "Column name does not exist = " + pColumnName, typeof(KrCsvData));
        return sColumn;
    }
}

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Row of csv data
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrCsvDataRow
{
    // private.
    private string[]    m_pColumnNames      = null;     // array of column names
    private string[]    m_pValues           = null;     // values of csv row

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief  : Constructor
    // @Param  : pColumnNames   => Array of column name
    //         : pValues        => Array of value
    // @Return : Row of csv data
    public KrCsvDataRow(string[] pColumnNames, string[] pValues)
    {
        m_pColumnNames = pColumnNames;
        m_pValues = new string[pValues.Length];
        for(int sIndex = 0; sIndex < pValues.Length; sIndex++)
        {
            string pValue = pValues[sIndex];

            // @Note : Trim the leading and trailing "
            if(pValue.StartsWith("\"") && pValue.EndsWith("\""))
            {
                pValue = pValue.Substring(1, pValue.Length - 2);
            }

            m_pValues[sIndex] = pValue;
        }
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief  : Get value
    // @Param  : sColumn    => Index of column
    // @Return : Cell value
    public string GetValue(int sColumn)
    {
        KrDebug.Assert((sColumn >= 0 && sColumn < m_pValues.Length), "[KrCsvDataRow] column out of range. row = " + sColumn, typeof(KrCsvDataRow));
        return m_pValues[sColumn];
    }

    // @Brief  : Get value
    // @Param  : pColumnName    => Column name
    // @Return : Cell valume
    public string GetValue(string pColumnName)
    {
        int sIndex = GetColumnIndex(pColumnName);
        return GetValue(sIndex);
    }

    // @Brief  : Check if the values are equal
    // @Param  : sColumn    => Index of column
    //         : pInputVal  => Value to be searched
    // @Return : Whether the values match [TRUE = Match, FALSE = Unmatch]
    public bool CheckValueEqual(int sColumn, string pInputVal)
    {
        string pValue = GetValue(sColumn);
        return pValue.Equals(pInputVal);
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PRIVATE
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief  : Getcolumn index
    // @Param  : pColumnName    => Column name
    // @Return : Index of column
    private int GetColumnIndex(string pColumnName)
    {
        int sColumn = -1;
        sColumn = System.Array.FindIndex<string>(m_pColumnNames, (pStr) => { return pStr == pColumnName; });
        KrDebug.Assert(sColumn >= 0, "Column name does not exist = " + pColumnName, typeof(KrCsvDataRow));
        return sColumn;
    }
}
