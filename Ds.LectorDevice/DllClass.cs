using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ds.LectorDevice
{
    class DllClass
    {
        //Abre el puerto serial
        [DllImport("CRT_288.dll")]
        public static extern UInt32 CommOpen(string port);
        //Abre el puerto serie a la velocidad de transmisión especificada
        [DllImport("CRT_288.dll")]
        public static extern long CommOpenWithBaut(string port, byte _BaudOption);
        //Cierra puerto serial
        [DllImport("CRT_288.dll")]
        public static extern int CommClose(UInt32 ComHandle);


        //Número de serie del lector de tarjetas
        [DllImport("CRT_288.dll")]
        public static extern int CRT88_ReadSnr(UInt32 ComHandle, byte[] _SNData, ref byte _dataLen);

        //Reiniciar el lector de tarjetas // 0x30 = No inicia la tarjeta 0x31 = inicia la tarjeta
        [DllImport("CRT_288.dll")]
        public static extern int CRT288_Reset(UInt32 ComHandle, byte _CardOut);



        //Expulsar
        [DllImport("CRT_288.dll")]
        public static extern int CRT288_Eject(UInt32 ComHandle);

        //Lee información de estado del lector de tarjetas
        [DllImport("CRT_288.dll")]
        public static extern int CRT288_GetStatus(UInt32 ComHandle, ref byte _CardStatus);
        //Control de encendido y apagado de la luz indicadora
        [DllImport("CRT_288.dll")]
        public static extern int CRT288_LEDSet(UInt32 ComHandle, byte _ONOFF);

        //Tiempo del indicador
        [DllImport("CRT_288.dll")]
        public static extern int CRT288_LEDTime(UInt32 ComHandle, byte _T1, byte _T2);


        //Leer datos de la pista
        [DllImport("CRT_288.dll")]
        public static extern int MC_ReadTrack(UInt32 ComHandle, byte _Mode, byte _track, ref int _TrackDataLen, byte[] _TrackData);


        //IC CARD POWER ON
        [DllImport("CRT_288.dll")]
        public static extern int CRT_IC_CardOpen(UInt32 ComHandle);

        //IC CARD POWER OFF
        [DllImport("CRT_288.dll")]
        public static extern int CRT_IC_CardClose(UInt32 ComHandle);

        [DllImport("CRT_288.dll")]
        public static extern int CRT_R_DetectCard(UInt32 ComHandle, ref byte _CardType, ref byte _CardInfor);

        [DllImport("CRT_288.dll")]
        public static extern int SLE4442_Reset(UInt32 ComHandle);

        [DllImport("CRT_288.dll")]
        public static extern int SLE4442_Read(UInt32 ComHandle, byte _Address, byte _BlockDataLen, byte[] _BlockData);

        [DllImport("CRT_288.dll")]
        public static extern int SLE4442_ReadP(UInt32 ComHandle, byte _BlockDataLen, byte[] _BlockData);

        [DllImport("CRT_288.dll")]
        public static extern int SLE4442_ReadS(UInt32 ComHandle, byte _BlockDataLen, byte[] _BlockData);

        [DllImport("CRT_288.dll")]
        public static extern int SLE4442_VerifyPWD(UInt32 ComHandle, byte _PWDatalen, byte[] _PWData);

        [DllImport("CRT_288.dll")]
        public static extern int SLE4442_Write(UInt32 ComHandle, byte _Address, byte _dataLen, byte[] _BlockData);

        [DllImport("CRT_288.dll")]
        public static extern int SLE4442_WriteP(UInt32 ComHandle, byte _Address, byte _dataLen, byte[] _BlockData);

        [DllImport("CRT_288.dll")]
        public static extern int SLE4442_WritePWD(UInt32 ComHandle, byte _PWDatalen, byte[] _PWData);

        [DllImport("CRT_288.dll")]
        public static extern int CPU_ColdReset(UInt32 ComHandle, byte _CPUMode, ref byte _CPUType, byte[] _exData, ref int _exdataLen);

        [DllImport("CRT_288.dll")]
        public static extern int CPU_WarmReset(UInt32 ComHandle, ref byte _CPUType, byte[] _exData, ref int _exdataLen);

        [DllImport("CRT_288.dll")]
        public static extern int CPU_T0_C_APDU(UInt32 ComHandle, int _dataLen, byte[] _APDUData, byte[] _exData, ref int _exdataLen);

        [DllImport("CRT_288.dll")]
        public static extern int CPU_T1_C_APDU(UInt32 ComHandle, int _dataLen, byte[] _APDUData, byte[] _exData, ref int _exdataLen);


        [DllImport("CRT_288.dll")]
        public static extern int SIM_Reset(UInt32 ComHandle, byte _VOLTAGE, byte _SIMNo, ref byte _SIMTYPE, byte[] _exData, ref int _exdataLen);

        [DllImport("CRT_288.dll")]
        public static extern int SIM_T0_C_APDU(UInt32 ComHandle, byte SIMNo, int _dataLen, byte[] _APDUData, byte[] _exData, ref int _exdataLen);

        [DllImport("CRT_288.dll")]
        public static extern int SIM_T1_C_APDU(UInt32 ComHandle, byte SIMNo, int _dataLen, byte[] _APDUData, byte[] _exData, ref int _exdataLen);

        [DllImport("CRT_288.dll")]
        public static extern int SIM_CardClose(UInt32 ComHandle, byte _AddH, byte _AddL);

        [DllImport("CRT_288.dll")]
        public static extern int RF_DetectCard(UInt32 ComHandle);

        [DllImport("CRT_288.dll")]
        public static extern int RF_GetCardID(UInt32 ComHandle, ref byte _CardIDLen, byte[] _CardID);

        [DllImport("CRT_288.dll")]
        public static extern int RF_LoadSecKey(UInt32 ComHandle, byte _Sec, byte _KEYType, byte _KEYLen, byte[] _KEY);

        [DllImport("CRT_288.dll")]
        public static extern int RF_ChangeSecKey(UInt32 ComHandle, byte _Sec, byte _KEYLen, byte[] _KEY);

        [DllImport("CRT_288.dll")]
        public static extern int RF_ReadBlock(UInt32 ComHandle, byte _Sec, byte _Block, ref byte _BlockDataLen, byte[] _BlockData);

        [DllImport("CRT_288.dll")]
        public static extern int RF_WriteBlock(UInt32 ComHandle, byte _Sec, byte _Block, byte _BlockDataLen, byte[] _BlockData);

        [DllImport("CRT_288.dll")]
        public static extern int RF_InitValue(UInt32 ComHandle, byte _Sec, byte _Block, byte _DataLen, byte[] _Data);

        [DllImport("CRT_288.dll")]
        public static extern int RF_ReadValue(UInt32 ComHandle, byte _Sec, byte _Block, ref byte _BlockDataLen, byte[] _BlockData);

        [DllImport("CRT_288.dll")]
        public static extern int RF_Increment(UInt32 ComHandle, byte _Sec, byte _Block, byte _DataLen, byte[] _Data);

        [DllImport("CRT_288.dll")]
        public static extern int RF_Decrement(UInt32 ComHandle, byte _Sec, byte _Block, byte _DataLen, byte[] _Data);


        //type A/b Card
        [DllImport("CRT_288.dll")]
        public static extern int CPU_SelRfCard(UInt32 ComHandle, byte _TypeCardType, byte[] _exData, ref int _exdataLen);


        [DllImport("CRT_288.dll")]
        public static extern int CPU_GetRfCardID(UInt32 ComHandle, ref int _CardUidLen, byte[] _CardUid);

        [DllImport("CRT_288.dll")]
        public static extern int CPU_SendRfAPDU(UInt32 ComHandle, int _APDUDLen, byte[] _APDUData, byte[] _exData, ref int _exdataLen);

        [DllImport("CRT_288.dll")]
        public static extern int CPU_DESelRfCard(UInt32 ComHandle);
    }
}