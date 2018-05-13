using System.Text;

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Apr 2018
// @Brief : Class for converting byte data
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrByteReader
{
    // @Brief  : Byte to short
    // @Param  : pBytes => Byte data
    //         : sIndex => Byte data index
    // @Return : Value of short
    public static short ByteToShort(byte[] pBytes, ref int sIndex)
    {
        byte[] pResult = ExtractByte(pBytes, sizeof(short), ref sIndex);
        return System.BitConverter.ToInt16(pResult, 0);
    }

    // @Brief : Byte to int
    // @Param : pBytes => Byte data
    //        : sIndex => Byte data index
    // @Return : Value of int
    public static int ByteToInt(byte[] pBytes, ref int sIndex)
    {
        byte[] pResult = ExtractByte(pBytes, sizeof(int), ref sIndex);
        return System.BitConverter.ToInt32(pResult, 0);
    }

    // @Brief  : Byte to float
    // @Param  : pBytes => Byte data
    //         : sIndex => Byte data index
    // @Return : Value of float
    public static float ByteToFloat(byte[] pBytes, ref int sIndex)
    {
        byte[] pResult = ExtractByte(pBytes, sizeof(int), ref sIndex);
        return System.BitConverter.ToSingle(pResult, 0);
    }

    // @Brief  : Byte to string
    // @Param  : pBytes => Byte data
    //         : sIndex => Byte data index
    // @Return : Value of string
    public static string ByteToString(byte[] pBytes, int sSize, ref int sIndex)
    {
        byte[] pResult = ExtractByte(pBytes, sSize, ref sIndex);
        return Encoding.UTF8.GetString(pResult);
    }

    // @Brief  : Extract byte data
    // @Param  : pBytes => Byte data
    //         : sSize  => Extraction size
    //         : sIndex => Byte data index
    // @Return : The extracted byte data
    public static byte[] ExtractByte(byte[] pBytes, int sSize, ref int sIndex)
    {
        byte[] pResult = new byte[sSize];
        System.Array.Copy(pBytes, sIndex, pResult, 0, pResult.Length);
        sIndex += pResult.Length;
        return pResult;
    }
}

