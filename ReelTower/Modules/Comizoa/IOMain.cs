// -------------------------------------------------------------------------------------------------------------------------------------------------------------------
//  Standard Platform for Machine Contorl by JM Choi, Jun.2017
//  Development tool has been using Microsoft Visual C# 2017, .NET Framework Ver 4.7
//  This program has been developed by JM Choi, and all legal rights are reserved by JM Choi. 
//  Unauthorized use or distribution is a legal violation.  
//
//  IODefine.cs
// -------------------------------------------------------------------------------------------------------------------------------------------------------------------


using System;
using System.IO;
using System.Xml;
using System.Threading;

namespace IO.Common
{
    class IOMain
    {
        public const int _Off = 0;
        public const int _On = 1;

        public const int _Back = 0;
        public const int _Front = 1;

        public const int _Close = 0;
        public const int _Open = 1;
        
        public const int _Stop = 0;
        public const int _Start = 1;
        public const int _Idle = 2;
        public const int _Warning = 3;
        public const int _Error = 4;

        public const int _Down = 0;
        public const int _Up = 1;

        public enum ContactType
        {
            NO = 0,
            NC = 1,
        }

        // -------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //    <Input/Output 에대한 키워드 정의>
        //    1. 각 키워드는 프로그램에서 IN/OUT 번호를 구별하는 Key값으로 사용이 됨,  
        //    2. IO 추가시 아래에 키워드만 추가하면 화면 표시는 자동으로 됨
        //    3. 키워드의 "_" 를 제거하여 화면에 표시되는 default IO 이름으로도 사용
        //    4. 키워드 순서 및 뒤의 번호는 default IO 번호로 사용됨 (XML 파일에서 변경 가능함) 
        //    5. IO 속성 (IO번지, 표시이름, Contact Type (NO, NC)는 SystemDefine.INPUT_OUTPUT_FILE_NAME 에서 지정한 XML 파일에서 불러옴
        //    6. 여기에 키워드를 추가하면 프로그램 실행시 XML 파일과 합성하여 다시 저장 함 
        //    7. XML 파일을 새로 만들고 싶으면 기존 XML 파일을 삭제하고 프로그램 실행하면 Default 값으로 새로 만들어짐
        // -------------------------------------------------------------------------------------------------------------------------------------------------------------------
        public enum IN
        {
            BEGIN = -1,

            START_SW                        = 0,
            STOP_SW                         = 1,
            RESET_SW                        = 2,
            LIGHT_SW                        = 3,
            EMG_SW_FRONT                    = 4,
            EMG_SW_SIDE                     = 5,
            EMG_SW_REAR                     = 6,
            SERVO_POWER_ON                  = 7,
            SIDE_RESET_SW                   = 8,
            REAR_RESET_SW                   = 9,
            R_SIDE_DOOR1                    = 10,
            FRONT_RIGHT_DOOR                = 11, 
            // 이 앞에 계속 IO 추가  하면 됨 (새로 추가된 IO의 속성은 (key, name, contact (NO/NC)) default 값이 들어간다  
            // (이후에 프로그램 실행 시 자동으로 생성되는 XML 파일에서 수정가능)
            END,                         
        }                            
                                     
        public enum OUT              
        {                            
            BEGIN = -1,              

            START_SW_LAMP                   =   0,
            STOP_SW_LAMP                    =   1,
            RESET_SW_LAMP                   =   2,
            LIGHT_SW                        =   3,
            SV_POWER_ON                     =   4,
            SERVO_SW_ENABLE                 =   5,
            SIDE_RESET_SW_LAMP              =   6,
            REAR_RESET_SW_LAMP              =   7,
            TOWER_LAMP_RED                  =   8,
            TOWER_LAMP_YEW                  =   9,
            TOWER_LAMP_GRN                  =   10, 

            // 이 앞에 계속 IO 추가 하면 됨 (새로 추가된 IO의 속성은 (key, name, contact (NO/NC)) default 값이 들어간다  
            // (이후에 프로그램 실행시 자동으로 생성되는 XML 파일에서 수정가능)
            END,
        }


        //Digital I/O Count
        public static int MAX_INPUT = Enum.GetValues(typeof(IN)).Length - 2;   // BEGIN, END 제외한 모든 Input 
        public static int MAX_OUTPUT = Enum.GetValues(typeof(OUT)).Length - 2;   // BEGIN, END 제외한 모든 Output

        // 각 Input/Output 번호의 속성을 지정함  (이 속성 값들은 SystemDefine.INPUT_OUTPUT_FILE_NAME 에 지정한 XML 파일에 저장되며 파일에서 직접 수정 가능함)
        public class IOProperty
        {
            public int ioNo { get; set; }                   // 실제 IO 번지 
            public string key { get; set; }                 // 프로그램 코드에서 사용할 키워드 (enum IN, enum OUT 에 정의)
            public string description { get; set; }         // 화면에 표시할 이름
            public ContactType contactType { get; set; }    // 접점 Type ( NO = Normal Open type (A접점),  NC = Normal Close type (B접점)


            public IOProperty(int gioNo = 0, string gkey = "IN_0", string gdescription = "Input 0", ContactType gcontactType = ContactType.NO)
            {
                ioNo = gioNo;
                key = gkey;
                description = gdescription;
                contactType = gcontactType;
            }

        }

        public static IOProperty[] inputProperty = new IOProperty[MAX_INPUT];  //  속성 값은 enum IN, enum OUT 에 들어 있는 키워드와 번호로 매칭되어 참조 (from 0..)
        public static IOProperty[] outputProperty = new IOProperty[MAX_OUTPUT];
                 
    }
} 