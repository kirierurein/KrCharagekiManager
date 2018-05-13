using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Text area for charageki ui
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrCharagekiUITextArea : KrICharagekiUI
{
    // private.
    private System.Action<string>   m_cbSetCharaName    = null; // callback for set character name. NOTE : externally registered processing
    private System.Action<string>   m_cbSetComment      = null; // callback for set comment. NOTE : externally registered processing
    private System.Action           m_cbShow            = null; // callback for show text area. NOTE : externally registered processing
    private System.Action           m_cbHide            = null; // callback for hide text area. NOTE : externally registered processing
    private float                   m_fReadingTime      = 0.0f; // reading time for comment
    private string                  m_pCharaName        = "";   // chara name
    private string                  m_pComment          = "";   // comment
    private int                     m_sCommentIndex     = 0;    // length of displayed comment
    private float                   m_fWaitingTime      = 0.0f; // waition read time 

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief  : Constructor
    // @Return : KrCharagekiUITextArea instance
    public KrCharagekiUITextArea()
    {
        m_fReadingTime = 0.0f;
        m_cbSetCharaName = null;
        m_cbSetComment = null;
        m_cbShow = null;
        m_cbHide = null;
        m_pCharaName = "";
        m_pComment = "";
        m_sCommentIndex = 0;
        m_fWaitingTime = 0.0f;
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Update
    public void Update()
    {
        if(m_sCommentIndex < m_pComment.Length)
        {
            m_fWaitingTime += Time.deltaTime;
            m_sCommentIndex = Mathf.Min((int)(m_fWaitingTime / m_fReadingTime), m_pComment.Length);
            if(m_cbSetComment != null)
            {
                if(m_sCommentIndex <= 0)
                {
                    m_cbSetComment("");
                }
                else
                {
                    m_cbSetComment(m_pComment.Substring(0, m_sCommentIndex));
                }
            }
            if(m_fWaitingTime >= (m_fReadingTime * m_pComment.Length))
            {
                m_fWaitingTime = 0;
            }
        }
    }

    // @Brief  : Set text
    // @Param  : pCharaName => chara name
    //         : pComment   => chara comment
    // @Return : Time when all text finishes playing
    public float SetText(string pCharaName, string pComment)
    {
        m_pCharaName = pCharaName;
        m_pComment = pComment;
        if(m_cbSetCharaName != null)
        {
            m_cbSetCharaName(m_pCharaName);
        }
        if(m_cbSetComment != null)
        {
            m_cbSetComment("");
        }
        m_sCommentIndex = 0;
        m_fWaitingTime = 0;
        return m_fReadingTime * m_pComment.Length;
    }

    // @Brief : Display all text
    public void DisplayAllText()
    {
        if(m_cbSetComment != null)
        {
            m_cbSetComment(m_pComment);
        }
        m_sCommentIndex = m_pComment.Length;
    }

    // @Brief  : Check reading text
    // @Return : Is reading comment
    public bool IsReading()
    {
        return !string.IsNullOrEmpty(m_pComment) && m_sCommentIndex < m_pComment.Length;
    }

    // @Brief : Show object
    public void Show()
    {
        if(m_cbShow != null)
        {
            m_cbShow();
        }
    }

    // @Brief : Hide object
    public void Hide()
    {
        if(m_cbHide != null)
        {
            m_cbHide();
        }
    }

    // @Brief : Register text area
    // @Param : fReadingTime    => Reading time of comment
    //        : cbSetCharaName  => Callback for set character name
    //        : cbSetComment    => Callback for set comment
    //        : cbShow          => Callback for show title 
    //        : cbHide          => Callback for hide title
    public void RegisterTextArea(float fReadingTime, System.Action<string> cbSetCharaName, System.Action<string> cbSetComment, System.Action cbShow, System.Action cbHide)
    {
        m_fReadingTime = fReadingTime;
        m_cbSetCharaName = cbSetCharaName;
        m_cbSetComment = cbSetComment;
        m_cbShow = cbShow;
        m_cbHide = cbHide;
    }
}
