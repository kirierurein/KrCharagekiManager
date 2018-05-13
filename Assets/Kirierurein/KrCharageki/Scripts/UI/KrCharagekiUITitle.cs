using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : title for charageki ui
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrCharagekiUITitle : KrICharagekiUI
{
    // private.
    private System.Action           m_cbShow        = null;     // callback for show title. NOTE : externally registered processing
    private System.Action           m_cbHide        = null;     // callback for hide title. NOTE : externally registered processing
    private System.Action<string>   m_cbSetText     = null;     // callback for set text. NOTE : externally registered processing

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief  : Construcor
    // @Return : KrCharagekiUITitle instance
    public KrCharagekiUITitle()
    {
        m_cbShow = null;
        m_cbHide = null;
        m_cbSetText = null;
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Set text
    // @Param : pStr    => Title name
    public void SetTitle(string pStr)
    {
        if(m_cbSetText != null)
        {
            m_cbSetText(pStr);
        }
    }

    // @Brief : Show
    public void Show()
    {
        if(m_cbShow != null)
        {
            m_cbShow();
        }
    }

    // @Brief : Hide
    public void Hide()
    {
        if(m_cbHide != null)
        {
            m_cbHide();
        }
    }

    // @Brief : Register title
    // @Param : cbSetText       => Callback for set text
    //        : cbShow          => Callback for show title 
    //        : cbHide          => Callback for hide title
    public void RegisterTitle(System.Action<string> cbSetText, System.Action cbShow, System.Action cbHide)
    {
        m_cbSetText = cbSetText;
        m_cbShow = cbShow;
        m_cbHide = cbHide;
    }
}

