using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Apr 2018
// @Brief : Charageki editor window
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrCharagekiEditorWindow : EditorWindow
{
    // private.
    private Vector2         m_vScrollPosition   = new Vector2();    // text area scroll position of script code 
    [SerializeField]
    private string          m_pSourceCode       = null;             // charageki script code
    private string          m_pFilePath         = null;             // absolute path of charageki script

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // UNITY FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : OnGUI
    private void OnGUI()
    {
        EditorGUILayout.LabelField("FilePath:",m_pFilePath);
        EditorGUILayout.BeginHorizontal();
        if(GUILayout.Button("Load"))
        {
            Load();
        }

        if(GUILayout.Button("Save"))
        {
            Save(m_pSourceCode);
        }

        EditorGUILayout.EndHorizontal();

        EditorGUI.BeginChangeCheck();
        m_vScrollPosition = EditorGUILayout.BeginScrollView(m_vScrollPosition);
        // Display code & Store in temporary variable for undo processing
        string pSourceCode = EditorGUILayout.TextArea(m_pSourceCode);

        // Whether TextArea has been updated
        if(EditorGUI.EndChangeCheck())
        {
            // Create undo action 
            Undo.RecordObject(this, "Charageki script");
            m_pSourceCode = pSourceCode;
            Undo.IncrementCurrentGroup();
        }

        EditorGUILayout.EndScrollView();
    }

    // @Brief : Load script
    private void Load()
    {
        string pFilePath = EditorUtility.OpenFilePanel("Open charageki script", Application.dataPath, "txt");
        if(!string.IsNullOrEmpty(pFilePath))
        {
            m_pFilePath = pFilePath;
            m_pSourceCode = File.ReadAllText(m_pFilePath);
        }
        // Change focus and update text area
        GUIUtility.keyboardControl = 0;
    }

    // @Breif : Save script
    // @param : pSourceCode    => Charageki script
    private void Save(string pSourceCode)
    {
        if(string.IsNullOrEmpty(m_pFilePath))
            return;

        File.WriteAllText(m_pFilePath, pSourceCode);
        KrDebug.Log("Save : " + m_pFilePath, typeof(KrCharagekiEditorWindow));
        AssetDatabase.Refresh();
        Compile();
    }

    // @Brief : Compile script
    private void Compile()
    {
        StreamReader pReader = KrResources.LoadText(m_pFilePath);
        KrCharagekiScript pScript = new KrCharagekiScript();
        try
        {
            pScript.LoadScript(pReader);
        }
        finally
        {
            if(pReader != null)
            {
                pReader.Close();
            }
        }
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PRIVATE FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Show window
    [MenuItem("Tools/Kirierurein/Charageki/TextEditor")]
    private static void ShowWindow()
    {
        EditorWindow pWindow = EditorWindow.GetWindow(typeof(KrCharagekiEditorWindow));
        pWindow.Show();
    }
}
