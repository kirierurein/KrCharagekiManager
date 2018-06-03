using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa. Apr 2018
// @Brief : Analysis class of Wav data
// @Format : 4byte -> RIFF header. 'R' 'I' 'F' 'F'
//           4byte -> Subsequent file sizes. (File size - 8)
//           4byte -> WAVE header. 'W' 'A' 'V' 'E'
//----------------------------------------------------------------
//           // Number of tag types, add the following (at least "fmt" and "data" required)
//           4byte -> Tag
//           4byte -> data length
//           Nbyte -> data
//----------------------------------------------------------------
//           // Tag format
//         - fmt
//           [Tag]
//           4byte -> fmt chunk. 'f' 'm' 't' ' ' (Including space)
//           [data length]
//           4byte -> Number of fmt chunk bytes.
//           [data]
//           2byte -> Format id
//           2byte -> Number of channel
//           4byte -> Sample rate
//           4byte -> Data speed (byte/sec)
//           2byte -> Block size (byte/sample*channelNum)
//           2byte -> Number of bits per sample (bit/sample)
//           2byte -> Size of extension (In the case of linear PCM, it does not exist)
//           Nbyte -> Extension (In the case of linear PCM, it does not exist)
//         - data
//           [Tag]
//           4byte -> data chunk. 'd' 'a' 't' 'a'
//           [data length]
//           4byte -> Number of bytes
//           [data]
//           Nbyte -> Waveform data
// Ref : http://sky.geocities.jp/kmaedam/directx9/waveform.html#tag
//       http://www.wdic.org/w/TECH/WAV
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class KrWav : KrAudioDataFormat
{
    // @Brief : fmt chunk
    public class KrFmtChunk
    {
        public short    sFormatId       = 0;    // format id
        public short    sChannelNum     = 0;    // number of channel
        public int      sSampleRate     = 0;    // sample rate
        public int      sDataSpeed      = 0;    // data speed (byte/sec)
        public short    sBlockSize      = 0;    // block size (byte / sample * channelNum)
        public short    sBitPerSample   = 0;    // number of bits per sample (bit/sample)
        public short    sExtensionSize  = 0;    // size of extension (In the case of linear PCM, it does not exist)
        public byte[]   pExtension      = null; // extension (In the case of linear PCM, it does not exist)

        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        // PUBLIC FUNCTION
        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        // @Brief : To string
        public override string ToString()
        {
            string pResult = "[fmt ] ";
            pResult += "FormatId = " + sFormatId + ", ";
            pResult += "Channel = " + sChannelNum + ", ";
            pResult += "SampleRate = " + sSampleRate + ", ";
            pResult += "DataSpeed(byte/sec) = " + sDataSpeed + ", ";
            pResult += "BlockSize(byte/sample*channelNum) = " + sBlockSize + ", ";
            pResult += "BitPerSample = " + sBitPerSample + ", ";
            pResult += "ExtensionSize = " + sExtensionSize + ", ";
            return pResult;
        }
    }

    // @Brief : data chunk
    public class KrDataChunk
    {
        public int      pLength         = 0;    // data length
        public byte[]   pWaveData       = null; // number of bytes

        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        // PUBLIC FUNCTION
        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        // @Brief : Get normalized data with Float. Between -1 and 1
        public virtual float[] GetData()
        {
            float[] pResult = new float[pLength];

            int sByteIndex = 0;
            for(int sIndex = 0; sIndex < pResult.Length; sIndex++)
            {
                // Get 1 byte
                byte uByte = pWaveData[sByteIndex];
                sByteIndex++;
                // Convert byte to float -1 to 1
                // NOTE : 1. The value of byte data is 0 to 255
                //      : 2. Convert the value to a float and add -128 to the value
                //      : 3. Divide the value by 256 to complete the conversion
                pResult[sIndex] = (((float)uByte - ((byte.MaxValue + 1) * 0.5f)) / (sbyte.MaxValue + 1));
            }

            return pResult;
        }

        // @Brief : To string
        public override string ToString()
        {
            string pResult = "[data] ";
            pResult += "Length = " + pLength + ", ";
            return pResult;
        }
    }

    // @Brief : 16bit of data chunk
    public class KrDataChunk16 : KrDataChunk
    {
        // @Brief : Get normalized data with Float. Between -1 and 1
        public override float[] GetData()
        {
            // Since it is acquired every 2 bytes from pWaveData, it halves the length of pResult
            float[] pResult = new float[pLength / 2];

            int sByteIndex = 0;
            for(int sIndex = 0; sIndex < pResult.Length; sIndex++)
            {
                // Get 2 byte
                short pShort = KrByteReader.ByteToShort(pWaveData, ref sByteIndex);
                // Convert byte to float -1 to 1
                // NOTE : 1. Convert the value to a float
                //      : 2. Divide the value by 32768 to complete the conversion
                pResult[sIndex] = (float)pShort / (short.MaxValue + 1);
            }

            return pResult;
        }
    }

    // private.
    private KrFmtChunk      m_pFmt      = null; // fmt chunk
    private KrDataChunk     m_pData     = null; // data chunk

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // CONSTRUCTOR
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief  : Constructor
    // @Param  : pPath          => Absolute path of Audio asset.
    //         : bFromResources => From resources file
    // @Return : KrWav instance
    public KrWav(string pPath, bool bFromResources) : base(pPath, bFromResources)
    {
        
    }


    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PUBLIC FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief  : Create audio clip
    // @Return : Audio clip
    public override AudioClip CreateAudioClip()
    {
        float[] pData = m_pData.GetData();
        AudioClip pAudioClip = AudioClip.Create(m_pName, pData.Length, m_pFmt.sChannelNum, m_pFmt.sSampleRate, false);
        pAudioClip.SetData(pData, 0);
        return pAudioClip;
    }

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // PROTECTED FUNCTION
    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    // @Brief : Convert bytes
    // @Param : pBytes  => byte data
    protected override void ConvertBytes(byte[] pBytes)
    {
        KrDebug.Log("Read wav data", typeof(KrWav));
        int sIndex = 0;
        // Get RIFF header. 4byte
        string pRIFFHeader = KrByteReader.ByteToString(pBytes, 4, ref sIndex);
        if(!pRIFFHeader.Equals("RIFF"))
        {
            KrDebug.Assert(false, "Format is not REFF. string = " + pRIFFHeader, typeof(KrWav));
        }

        // Get Subsequent file sizes. (File size - 8). 4byte
        int sFileSize = KrByteReader.ByteToInt(pBytes, ref  sIndex);

        // Get WAVE header. 4byte
        string pWaveHeader = KrByteReader.ByteToString(pBytes, 4, ref  sIndex);
        if(!pWaveHeader.Equals("WAVE"))
        {
            KrDebug.Assert(false, "Format is not WAVE string = " + pWaveHeader, typeof(KrWav));
        }

        while(sIndex < sFileSize)
        {
            // Get tag
            string pTag = KrByteReader.ByteToString(pBytes, 4, ref  sIndex);
            int sDataLength = KrByteReader.ByteToInt(pBytes, ref  sIndex);

            if(pTag.Equals("fmt "))
            {
                m_pFmt = new KrFmtChunk();
                m_pFmt.sFormatId = KrByteReader.ByteToShort(pBytes, ref  sIndex);
                m_pFmt.sChannelNum = KrByteReader.ByteToShort(pBytes, ref  sIndex);
                m_pFmt.sSampleRate = KrByteReader.ByteToInt(pBytes, ref  sIndex);
                m_pFmt.sDataSpeed = KrByteReader.ByteToInt(pBytes, ref  sIndex);
                m_pFmt.sBlockSize = KrByteReader.ByteToShort(pBytes, ref  sIndex);
                m_pFmt.sBitPerSample = KrByteReader.ByteToShort(pBytes, ref sIndex);
                // If sDataLength is greater than 16, there is an extension
                if(sDataLength > 16)
                {
                    m_pFmt.sExtensionSize = KrByteReader.ByteToShort(pBytes, ref sIndex);
                    m_pFmt.pExtension = KrByteReader.ExtractByte(pBytes, m_pFmt.sExtensionSize, ref sIndex);
                }
                KrDebug.Log(m_pFmt.ToString(), typeof(KrWav));
            }
            else if(pTag.Equals("data"))
            {
                // Data output varies depending on the number of bits per sample (bit/sample)
                if(m_pFmt.sBitPerSample == 16)
                {
                    m_pData = new KrDataChunk16();
                }
                else
                {
                    m_pData = new KrDataChunk();
                }
                m_pData.pLength = sDataLength;
                m_pData.pWaveData = KrByteReader.ExtractByte(pBytes, sDataLength, ref sIndex);
                KrDebug.Log(m_pData.ToString(), typeof(KrWav));
            }
            else
            {
                // Add data length
                sIndex += sDataLength;
            }
        }
            
        KrDebug.Log("Read complete", typeof(KrWav));
    }
}
