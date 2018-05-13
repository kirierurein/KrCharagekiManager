using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Jan 2018
// @Brief : Resources loader
//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrResources
{
    //:::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //:::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief  : Load bytes
    // @Note   : Only absolute path is supported
    // @Param  : pPath => Asset path
    // @Return : Bytes data
    public static byte[] LoadBytes(string pPath)
    {
        FileStream pFileStream = new FileStream(pPath, FileMode.Open);
        byte[] pResult = new byte[pFileStream.Length];
        pFileStream.Read(pResult, 0, pResult.Length);
        pFileStream.Close();
        return pResult;
    }

    // @Brief  : Load text
    // @Note   : Please use streamReader.Close () yourself after using streamReader!!
    // @Param  : pPath => Asset path
    // @Return : Stream reader
    public static StreamReader LoadText(string pPath)
    {
        StreamReader pStreamRender = new StreamReader(pPath, System.Text.Encoding.UTF8);
        return pStreamRender;
    }

    // @Brief  : Load sptite
    // @Param  : pPath => Asset path
    // @Return : Sprite
    public static Sprite LoadSprite(string pPath)
    {
        byte[] pByte = LoadBytes(pPath);

        // Since width and height are reset by LoadImage (), 1 is set
        Texture2D pTexture = new Texture2D(1, 1);
        pTexture.LoadImage(pByte);
        return Sprite.Create(pTexture, new Rect(0, 0, pTexture.width, pTexture.height), new Vector2(0.5f, 0.5f), 100, 0, SpriteMeshType.FullRect);
    }

    // @Brief  : Load data in the applcation
    // @Param  : pPath   => Resources Relative path of the following folder
    // @Return : Return the object declared in the template
    public static T LoadDataInApp<T>(string pPath) where T : Object
    {
        return Resources.Load(pPath) as T;
    }
}
