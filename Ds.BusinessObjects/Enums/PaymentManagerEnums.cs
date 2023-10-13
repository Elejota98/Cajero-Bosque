using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds.BusinessObjects.Enums
{
    public enum CashCode_Handle : int
    {
        User = 0x0800,
    }

    /// <summary>
    /// Start Payment Manager Enumerators
    /// </summary>

    public enum Device : int
    {
        ALL = 0,
        Coin_Validator = 3,
        Bill_Validator = 2,
        Cashless = 4,
        Hoppers = 8,
        Hoppers_Coin = 9,
    }

    public enum Protocol : int
    {
        ALL = 0,
        MDB_S1 = 1,
        ccTalk = 2,
        ccNet = 4,
    }

    /// <summary>
    /// Windows Messages Enumerator WParams
    /// </summary>

    public enum WParam_Message : int
    {
        Event_Range = 0xF0,
        Payment_Unit_Range = 0x0F,
    }

    public enum Event : int
    {
        Payment_Unit = 0,
        Cash_Acceptance = 1,
        Cash_Payout = 2,
        Escrow = 3,
        Error = 4,
    }

    public enum Pay_Unit : int
    {
        Payment_Manager = 0,
        Coin_Validator = 1,
        Bill_Validator = 2,
        Cashless_System = 3,
        Hopper1 = 4,
        Hopper2 = 5,
        Hopper3 = 6,
        Hopper4 = 7,

        Inputs = 14,
    }

    /// <summary>
    /// Windows Messages Enumerator LParams
    /// </summary>

    public enum Status_Message : int
    {
        Found_Startup = 0,
        Ready = 1,
        Hardware_Problem = 2,
        Not_Found_Running = 3,
        Tube_Coin_Empty = 4,
        Not_Found_Startup = 5,
        Reconnected = 6,
        Bills_Removed = 7,
        No_Configuration_Found = 8,
        Cash_Box_Replaced = 9,
        Routing = 16,
    }

    public enum Escrow_Message : int
    {
        Return_Lever_Activated = 0,
        Not_Returned_From_Escrow = -1,
    }

    public enum LParam_Error_Message : int
    {
        Error_Type_Range = 0xFF00,
        Error_Code_Range = 0x00FF,
    }

    public enum NRI_ErrorCode : int
    {
        Communication_Error = 0,
        Coin_Validator_Reset = 1,
        Bill_Validator_Reset = 2,
        Sensor_Problem = 3,
        Defective_Motor = 4,
        Coin_Jam_Tube = 5,
        Jam_Validator = 6,
        Bill_Validator_ROM_Checksum_Error = 7,
        Coin_Validator_ROM_Checksum_Error = 8,
        Cash_Box_Removed = 9,
        Defective_Tube_Sensor = 10,
        Payment_Unit_Disabled = 11,
        Validator_Unplugged = 12,
        Coin_Jam = 13,
        Coin_Sorting_Error = 14,
        String_Recognition = 15,
        Cash_Box_Full = 16,
        Jam_Cash_Box = 17,
        Cash_Box_Error = 18,
        Hopper_Motor_Blocked = 19,
        Hopper_Empty = 20,
        Hopper_Optics_Blocked = 21,
        Hopper_Optics_Error = 22,
        Hopper_Payout_Blocked = 23,
        Bill_Unit_To_Be_Removed = 24,
        Tube_Cassette_Removed = 25,
        Sorting_Opened = 26,
        Cheated = 27,
        Bills_In_Dispenser_B2B = 28,
        Low_Voltage_Coin_Charger_Detected = 29,
        Rejected = 30,
    }

    public enum B2B_Troubleshooting_ErrorCode : int
    {
        Error_Link2_81 = 0x81,
        Error_Link2_82 = 0x82,
        Error_Link2_83 = 0x83,
        Error_Link2_84 = 0x84,
        Error_Link2_85 = 0x85,
        Error_Link2_86 = 0x86,
        Error_Link2_87 = 0x87,
        Error_Link2_88 = 0x88,
        Error_Link2_89 = 0x89,
        Error_Link2_8A = 0x8A,
        Error_Link2_8D = 0x8D,
        Error_Link2_8E = 0x8E,
        Error_Link2_8F = 0x8F,

        Error_Transport_90 = 0x90,
        Error_Transport_91 = 0x91,
        Error_Transport_92 = 0x92,
        Error_Transport_93 = 0x93,
        Error_Transport_94 = 0x94,
        Error_Transport_95 = 0x95,
        Error_Transport_96 = 0x96,

        Error_Switch_97 = 0x97,
        Error_Switch_98 = 0x98,
        Error_Switch_99 = 0x99,

        Error_Cassette1_A1 = 0xA1,
        Error_Cassette1_A2 = 0xA2,
        Error_Cassette1_A3 = 0xA3,
        Error_Cassette1_A4 = 0xA4,
        Error_Cassette1_A5 = 0xA5,
        Error_Cassette1_A6 = 0xA6,
        Error_Cassette1_A7 = 0xA7,
        Error_Cassette1_A8 = 0xA8,
        Error_Cassette1_A9 = 0xA9,
        Error_Cassette1_AA = 0xAA,
        Error_Cassette1_AB = 0xAB,
        Error_Cassette1_AC = 0xAC,
        Error_Cassette1_AD = 0xAD,

        Error_Cassette2_AE = 0xAE,
        Error_Cassette2_AF = 0xAF,
        Error_Cassette2_B0 = 0xB0,
        Error_Cassette2_B1 = 0xB1,
        Error_Cassette2_B2 = 0xB2,
        Error_Cassette2_B3 = 0xB3,
        Error_Cassette2_B4 = 0xB4,
        Error_Cassette2_B5 = 0xB5,
        Error_Cassette2_B6 = 0xB6,
        Error_Cassette2_B7 = 0xB7,
        Error_Cassette2_B8 = 0xB8,
        Error_Cassette2_B9 = 0xB9,
        Error_Cassette2_BA = 0xBA,

        Error_Cassette3_BB = 0xBB,
        Error_Cassette3_BC = 0xBC,
        Error_Cassette3_BD = 0xBD,
        Error_Cassette3_BE = 0xBE,
        Error_Cassette3_BF = 0xBF,
        Error_Cassette3_C0 = 0xC0,
        Error_Cassette3_C1 = 0xC1,
        Error_Cassette3_C2 = 0xC2,
        Error_Cassette3_C3 = 0xC3,
        Error_Cassette3_C4 = 0xC4,
        Error_Cassette3_C5 = 0xC5,
        Error_Cassette3_C6 = 0xC6,
        Error_Cassette3_C7 = 0xC7,

        Error_Dispenser_C8 = 0xC8,
        Error_Dispenser_C9 = 0xC9,
        Error_Dispenser_CB = 0xCB,
        Error_Dispenser_CE = 0xCE,
        Error_Dispenser_CF = 0xCF,
        Error_Dispenser_D0 = 0xD0,
        Error_Dispenser_D1 = 0xD1,
        Error_Dispenser_D2 = 0xD2,
        Error_Dispenser_D3 = 0xD3,
        Error_Dispenser_D4 = 0xD4,
        Error_Dispenser_D5 = 0xD5,
        Error_Dispenser_D6 = 0xD6,
        Error_Dispenser_D7 = 0xD7,
        Error_Dispenser_D8 = 0xD8,
        Error_Dispenser_D9 = 0xD9,
        Error_Dispenser_DA = 0xDA,

        Jam_Transport_DB = 0xDB,
        Jam_Transport_DC = 0xDC,
        Jam_Transport_DD = 0xDD,
        Jam_Transport_DE = 0xDE,
        Jam_Transport_DF = 0xDF,
        Jam_Transport_E0 = 0xE0,
        Jam_Transport_E1 = 0xE1,
        Jam_Transport_E2 = 0xE2,
        Jam_Transport_E3 = 0xE3,
        Jam_Transport_E4 = 0xE4,
        Jam_Transport_E5 = 0xE5,
        Jam_Transport_E6 = 0xE6,

        Error_Link1_E7 = 0xE7,
        Error_Link1_E8 = 0xE8,
        Error_Link1_E9 = 0xE9,
        Error_Link1_EA = 0xEA,
        Error_Link1_EB = 0xEB,
        Error_Link1_EC = 0xEC,
        Chassis_Removed = 0xED,

        Error_HV_F0 = 0x0F0,
        Error_HV_F1 = 0x0F1,
        Error_HV_F2 = 0x0F2,
        Error_HV_F3 = 0x0F3,
        Error_HV_F4 = 0x0F4,
        Error_HV_F5 = 0x0F5,
        Error_HV_F6 = 0x0F6,
        Error_HV_F7 = 0x0F7,
        Error_HV_F8 = 0x0F8,
        Error_HV_F9 = 0x0F9,
        Error_HV_FA = 0x0FA,
        Error_HV_FB = 0x0FB,
        Error_HV_FC = 0x0FC,
        Error_HV_FD = 0x0FD,
    }


    /// <summary>
    /// Set Payment Manager Enumerators
    /// </summary>

    public enum Command : int
    {
        Inhibit = 0,
        Payout = 1,
        Status = 2,
        Escrow = 3,
        Sorting_Device = 4,
        Maintenance = 6,
    }

    public enum Selection : int
    {
        /// <summary>
        /// Inhibit Command Selection
        /// </summary>
        Enable_All_Cash_Items = 0,
        Disable_All_Cash_Items = 1,
        Enable_Cash_Item_With_Specified_Value = 2,
        Disable_Cash_Item_With_Specified_Value = 3,
        Enable_Cash_Item_With_Specified_ID = 4,
        Disable_Cash_Item_With_Specified_ID = 5,

        /// <summary>
        /// Payout Command Selection
        /// </summary>
        Automatic_Payout = 0,
        Payout_Specified_Value_Specified_Device = 1,
        Get_Lowest_Payout_Value_Possible = 2,
        Get_Highest_Payout_Value_Possible = 3,
        Get_Payout_Items_Specified_Device = 4,
        Asynchronous_Payout = 5,
        Get_Empty_Full_Status_Device = 6,
        Enable_Disable_Automatic_Payout = 7,
        Manual_Payout = 8,
        Last_Payout = 9,        //ASIGNAR

        /// <summary>
        /// Status Command Selection
        /// </summary>
        Cash_Item_Value = 0,
        Device_Cash_Item = 1,
        Collector_Cash_Item = 2,
        Availability_Cash_Item = 3,
        Tube_Counter = 4,
        Sorter = 5,

        /// <summary>
        /// Escrow Command Selection
        /// </summary>
        Enable_Disable_Escrow_All_Bills = 0,
        Enable_Disable_Escrow_Specified_Cash_Item = 1,
        Accept_Return_Timeout_Bill_From_Escrow = 3,
        Open_Escrow = 4,
        Status_Escrow = 5,
        Escrow_Bill_Value = 6,      //ASIGNAR

        /// <summary>
        /// Sorter Command Selection
        /// </summary>
        Sorter_Path = 0,
        Set_Sorter_Override = 1,
        Bill_To_Bill_Recycling_Cassette_Value = 2,
        Sorter_Status = 3,
        Sorter_Device = 4,      //ASIGNAR DESDE AQUI
        Get_Sorter_Override = 5,

        /// <summary>
        /// Maintenance Command Selection
        /// </summary>
        Unload_Bills = 0,
        Get_Serial_Number = 1,

    }

    public enum ID_Denomination : int
    {
        ID_Coin_10 = 0,
        ID_Coin_20 = 1,
        ID_Coin_50 = 2, // 2 y 3 
        ID_Coin_100 = 4, // 4 y 5
        ID_Coin_200 = 6,
        ID_Coin_500 = 7, // 7 Y 8
        ID_Coin_1000 = 9,
        ID_Bill_1000 = 10,
        ID_Bill_2000 = 11,
        ID_Bill_5000 = 12,
        ID_Bill_10000 = 13,
        ID_Bill_20000 = 14,
        ID_Bill_50000 = 15,
    }

    public enum Info : int
    {
        /// <summary>
        /// Inhibit Info
        /// </summary>
        All_Cash = 0,
        All_Coins = 1,
        All_Bills = 2,

        /// <summary>
        /// Payout Info
        /// </summary>
        Automatic_Disable = 0,
        Automatic_Enable = 1,
        Pay = 0,

        /// <summary>
        /// Escrow Info 
        /// </summary>
        Disable_Escrow = 0,
        Enable_Escrow = 1,
        Accept_Bill = 1,
        Return_Bill = 2,
        Timeout = 3,
        Accept_Direction = 1,
        Reject_Direction = 2,

        /// <summary>
        /// Not Care Info
        /// </summary>
        Zero = 0,
    }

    /// <summary>
    /// General Result Enumerator
    /// </summary>

    public enum Result : int
    {
        /// <summary>
        /// Open, Close and Stop Payment Manager Results
        /// </summary>
        OK = 0,
        Not_USB_DLL_Found = 0x1000,
        PM_Busy = 0x2005,
        Error = 1,

        /// <summary>
        /// Start Payment Manager OK Results
        /// </summary>
        Coin_Validator_Found = 1,
        Bill_Validator_Found = 2,
        Cashless_System_Found = 4,
        Hoppers_Found = 8,
        Escrow_Found = 16,
        Display_Found = 32,
        ALL_Found_Devices = 64, // 72

        /// <summary>
        /// Start Payment Manager Error Results
        /// </summary>
        Payment_Manager_Unopened = 0x2000,
        Unknown_Protocol_Selected = 0x2001,
        Error_Adapter_Communication_A = 0x2002,
        Error_Adapter_Communication_B = 0x2003,
        No_Communication_With_Payment_Units = 0x2004,
        Payment_Manager_Running = 0x2005,
        Payment_Manager_Not_Start_Thread = 0x2006,

        /// <summary>
        /// Set Payment Manager Error Results
        /// </summary>
        Device_Not_Found = -1,
        Value_Too_Small = -2,
        No_Device_Attached = -3,
        Device_Error = -4,
        Command_Unknowm = -5,
        Payment_Manager_Not_Running = -6,
        Device_Busy = -7,           //ASIGNAR

        /// <summary>
        /// Device Level Mask Results
        /// </summary>
        Normal_Level = 0,
        Empty_Level = 1,
        Full_Level = 2,
    }

    public enum General : int
    {
        Unidad = 1,
        Factor_Denominacion = 100,
        Decimal_Cut_Denom = 2,

        Max_Unload_Bills = 200,
    }

    public enum ID_Part : int
    {
        CashBox = 0,
        Cassette1 = 1,
        Cassette2 = 2,
        Cassette3 = 3,
        Cassette4 = 4,
        Hopper1 = 1,
        Hopper2 = 2,
        Hopper3 = 3,
        Hopper4 = 4,
        Ninguno = 9,
    }

    public enum Device_Type : int
    {
        Cassette = 'B',
        Hopper = 'H',
    }

    public enum Denom_Type : int
    {
        Bill = 'B',
        Coin = 'C',
    }
}
