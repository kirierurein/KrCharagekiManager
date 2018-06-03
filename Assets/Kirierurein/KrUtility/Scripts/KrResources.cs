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
    // @Param  : pPath          => Asset path
    //         : bFromResources => From resources file
    // @Return : Bytes data
    public static byte[] LoadBytes(string pPath, bool bFromResources = false)
    {
        if(bFromResources)
        {
            TextAsset pTextAsset = LoadDataInApp<TextAsset>(pPath);
            return pTextAsset.bytes;
        }
        else
        {
            FileStream pFileStream = new FileStream(pPath, FileMode.Open);
            byte[] pResult = new byte[pFileStream.Length];
            pFileStream.Read(pResult, 0, pResult.Length);
            pFileStream.Close();
            return pResult;
        }
    }

    // @Brief  : Load text
    // @Note   : Please use streamReader.Close () yourself after using streamReader!!
    // @Param  : pPath          => Asset path
    //         : bFromResources => From resources file
    // @Return : Stream reader
    public static StreamReader LoadText(string pPath, bool bFromResources = false)
    {
        if(bFromResources)
        {
            TextAsset pTextAsset = LoadDataInApp<TextAsset>(pPath);
            MemoryStream pMemoryStream = new MemoryStream(pTextAsset.bytes);
            StreamReader pStreamReader = new StreamReader(pMemoryStream);
            return pStreamReader;
        }
        else
        {
            StreamReader pStreamRender = new StreamReader(pPath, System.Text.Encoding.UTF8);
            return pStreamRender;
        }
    }

    // @Brief  : Load texture 2d
    // @Param  : pPath          => Asset path
    //         : bFromResources => From resources file
    // @Return : Texture 2d
    public static Texture2D LoadTexture2D(string pPath, bool bFromResources = false)
    {
        if(bFromResources)
        {
            return LoadDataInApp<Texture2D>(pPath);
        }
        else
        {
            byte[] pByte = LoadBytes(pPath, bFromResources);

            // Since width and height are reset by LoadImage (), 1 is set
            Texture2D pTexture = new Texture2D(1, 1);
            pTexture.LoadImage(pByte);
            return pTexture;
        }
    }

    // @Brief  : Load sptite
    // @Param  : pPath          => Asset path
    //         : bFromResources => From resources file
    // @Return : Sprite
    public static Sprite LoadSprite(string pPath, bool bFromResources = false)
    {
        if(bFromResources)
        {
            return LoadDataInApp<Sprite>(pPath);
        }
        else
        {
            Texture2D pTexture = LoadTexture2D(pPath, bFromResources);
            return Sprite.Create(pTexture, new Rect(0, 0, pTexture.width, pTexture.height), new Vector2(0.5f, 0.5f), 100, 0, SpriteMeshType.FullRect);
        }
    }

    // @Brief  : Load data in the applcation
    // @Param  : pPath   => Resources Relative path of the following folder
    // @Return : Return the object declared in the template
    public static T LoadDataInApp<T>(string pPath) where T : Object
    {
        return Resources.Load<T>(pPath);
    }
}
