using UnityEngine;
using System.Collections;


//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Charageki script command for load
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public abstract class KrCharagekiCommandLoad : KrCharagekiCommand
{
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Constructor
    public KrCharagekiCommandLoad() : base()
    {
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Execution command
    // @Param : pManager   => Charageki manager
    public override void Exec(KrCharagekiManager pManager)
    {
        string pKey = "";
        string pPath = "";
        for(int sIndex = 0; sIndex < m_pOptions.Count; sIndex++)
        {
            //:::::::::::::::::::::::::::::::::::::::::::::::::::
            // KEY
            if(m_pOptions[sIndex].m_pKey == "key")
            {
                // Keys for using csv
                pKey = m_pOptions[sIndex].m_pValue;
            }
            //:::::::::::::::::::::::::::::::::::::::::::::::::::
            // RESOURCE PATH
            else if(m_pOptions[sIndex].m_pKey == "path")
            {
                pPath = m_pOptions[sIndex].m_pValue;
            }
        }

        KrDebug.Assert(!string.IsNullOrEmpty(pKey), "there is no key!!", typeof(KrCharagekiCommandLoad));
        KrDebug.Assert(!string.IsNullOrEmpty(pPath), "there is no path!!", typeof(KrCharagekiCommandLoad));

        Exec(pKey, pPath, pManager);
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PROTECTED FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Execution with key & path
    // @Param : pKey        => Key name
    //        : pPath       => Asset path
    //        : pManager    => Charageki manager
    protected abstract void Exec(string pKey, string pPath, KrCharagekiManager pManager);
}

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Charageki script command for load scenario
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrCharagekiCommandLoadScenario : KrCharagekiCommandLoad
{
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Constructor
    public KrCharagekiCommandLoadScenario() : base()
    {
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PROTECTED FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Execution with key & path
    // @Param : pKey        => Key name
    //        : pPath       => Asset path of csv
    //        : pManager    => Charageki manager
    protected override void Exec(string pKey, string pPath, KrCharagekiManager pManager)
    {
        KrCharagekiScenarioContainer pScenarioContainer = pManager.GetScenarioContainer();
        KrDebug.Log("Load scenario : key = " + pKey + ", path = " + pPath, typeof(KrCharagekiCommandLoadScenario));
        // Load scenario csv data
        pScenarioContainer.LoadScenario(pKey, pPath);
    }
}

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Charageki script command for load background
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrCharagekiCommandLoadBg : KrCharagekiCommandIdOption
{
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Constructor
    public KrCharagekiCommandLoadBg() : base()
    {
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PROTECTED FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Execution with option
    // @Param : uId        => Background id
    //        : pManager   => Charageki manager
    protected override void Exec(uint uId, KrCharagekiManager pManager)
    {
        KrDebug.Log("Load bg : id = " + uId, typeof(KrCharagekiCommandLoadBg));
        KrCharagekiUIController pUIController = pManager.GetUIController();
        pUIController.LoadBg(uId);
    }
}

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Charageki script command for load character
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrCharagekiCommandLoadCharacter : KrCharagekiCommandIdOption
{
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Constructor
    public KrCharagekiCommandLoadCharacter() : base()
    {
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PROTECTED FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Execution with option
    // @Param : uId        => Character id
    //        : pManager   => Charageki manager
    protected override void Exec(uint uId, KrCharagekiManager pManager)
    {
        KrDebug.Log("Load character : id = " + uId, typeof(KrCharagekiCommandLoadCharacter));
        KrCharagekiUIController pUIController = pManager.GetUIController();
        pUIController.LoadCharacter(uId);
    }
}
