Attribute VB_Name = "Module1"
Option Explicit
'// Device ID definition//
Public Const COMI_CP101 = &HC101
Public Const COMI_CP201 = &HC201
Public Const COMI_CP301 = &HC301
Public Const COMI_CP401 = &HC401
Public Const COMI_CP501 = &HC501
Public Const COMI_SD101 = &HB101
Public Const COMI_SD102 = &HB102
Public Const COMI_SD103 = &HB103
Public Const COMI_SD201 = &HB201
Public Const COMI_SD301 = &HB301
Public Const COMI_SD401 = &HB401
Public Const COMI_SD501 = &HB501
Public Const COMI_LX101 = &HA101
Public Const COMI_LX102 = &HA102
Public Const COMI_LX103 = &HA103
Public Const COMI_LX201 = &HA201
Public Const COMI_LX202 = &HA202
Public Const COMI_LX203 = &HA203
Public Const COMI_LX301 = &HA301
Public Const COMI_LX401 = &HA401
Public Const COMI_LX501 = &HA501
Public Const COMI_LX502 = &HA502
Public Const COMI_LX504 = &HA504
Public Const COMI_LX508 = &HA508

'// Define scan method //
Public Const TRS_SINGLE = 1
Public Const TRS_BLOCK = 2

'// Define DIO Usage - only for COMI_LX_401 Board//
Public Const DI_ONLY = 0
Public Const DI_DO = 1
Public Const DO_DI = 2
Public Const DO_ONLY = 3

'// define counter control values //
Public Const COUNTER_LATCH = 0        '// counter latch operation       //
Public Const READ_LOAD_MSB = 1        '// read/load MSB only            //
Public Const READ_LOAD_LSB = 2      '// read/load LSB only            //
Public Const READ_LOAD_WORD = 3      '// read/load LSB first, then MSB //

Public Const BCD = 1         '// BCD counter                //
Public Const BINARY = 0               '// Binary counter             //

Public Const CMODE0 = 0               '// Interrupt on terminal count   //
Public Const CMODE1 = 1               '// Programmable one shot         //
Public Const CMODE2 = 2               '// Rate generator                //
Public Const CMODE3 = 3               '// Square wave rate generator    //
Public Const CMODE4 = 4               '// Software triggered strobe     //
Public Const CMODE5 = 5               '// Hardware triggered strobe     //

'// Encoder Counter mode //
Public Const ENCODER_1X = 0   '// Normal encoder counter mode  //
Public Const ENCODER_2X = 1   '// Encoder counter which counts both of rising & falling edge of A  //

Public Const PG_INFINITE = 0  '// PULSE GEN. Infinite mode  //

Public Const VT_SHORT = 0
Public Const VT_FLOAT = 1
Public Const VT_DOUBLE = 2

'// BY ZKS //
Public Const AI_SINGLE = 1
Public Const AI_DIFF = 0
