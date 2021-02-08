#region Licenses
///////////////////////////////////////////////////////////////////////////////
/// MIT License
/// 
/// Copyright (c) 2019 Marcus Software Ltd.
/// 
/// Permission is hereby granted, free of charge, to any person obtaining a copy
/// of this software and associated documentation files (the "Software"), to deal
/// in the Software without restriction, including without limitation the rights
/// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
/// copies of the Software, and to permit persons to whom the Software is
/// furnished to do so, subject to the following conditions:
/// 
/// The above copyright notice and this permission notice shall be included in all
/// copies or substantial portions of the Software.
/// 
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
/// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
/// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
/// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
/// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
/// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
/// SOFTWARE.
///////////////////////////////////////////////////////////////////////////////
///          Copyright Joe Coder 2004 - 2006.
/// Distributed under the Boost Software License, Version 1.0.
///    (See accompanying file LICENSE_1_0.txt or copy at
///          https://www.boost.org/LICENSE_1_0.txt)
///////////////////////////////////////////////////////////////////////////////
/// 저작권 (c) 2019 Marcus Software Ltd. (isadrastea.kor@gmail.com)
///
/// 본 라이선스의 적용을 받는 소프트웨어와 동봉된 문서(소프트웨어)를 획득하는 
/// 모든 개인이나 기관은 소프트웨어를 Marcus Software (isadrastea.kor@gmail.com)
/// 에 신고하고, 허용 의사를 서면으로 득하여, 사용, 복제, 전시, 배포, 실행 및
/// 전송할 수 있고, 소프트웨어의 파생 저작물을 생성할 수 있으며, 소프트웨어가
/// 제공된 제3자에게 그러한 행위를 허용할 수 있다. 단, 이 모든 행위는 다음과 
/// 같은 조건에 의해 제한 한다.:
///
/// 소프트웨어의 저작권 고지, 그리고 위의 라이선스 부여와 이 규정과 아래의 부인 
/// 조항을 포함한 이 글의 전문이 소프트웨어를 전체적으로나 부분적으로 복제한 
/// 모든 복제본과 소프트웨어의 모든 파생 저작물 내에 포함되어야 한다. 단, 해당 
/// 복제본이나 파생저작물이 소스 언어 프로세서에 의해 생성된, 컴퓨터로 인식 
/// 가능한 오브젝트 코드의 형식으로만 되어 있는 경우는 제외된다.
///
/// 이 소프트웨어는 상품성, 특정 목적에의 적합성, 소유권, 비침해에 대한 보증을 
/// 포함한, 이에 국한되지는 않는, 모든 종류의 명시적이거나 묵시적인 보증 없이 
///“있는 그대로의 상태”로 제공된다. 저작권자나 소프트웨어의 배포자는 어떤 
/// 경우에도 소프트웨어 자체나 소프트웨어의 취급과 관련하여 발생한 손해나 기타 
/// 책임에 대하여, 계약이나 불법행위 등에 관계 없이 어떠한 책임도 지지 않는다.
///////////////////////////////////////////////////////////////////////////////
/// project ReelTower 
/// @author Marcus (isadrastea.kor@gmail.com)
/// @namespace Marcus.Solution.TechFloor
/// @file RobotSequenceManager.cs
/// @brief
/// @details
/// @date 2019, 5, 21, 오후 1:13
///////////////////////////////////////////////////////////////////////////////
#endregion

#region Imports
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Marcus.Solution.TechFloor.Object;
using System.Reflection;
using System.Net;
using Marcus.Solution.TechFloor.Util;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Security;
#endregion

#region Program
namespace Marcus.Solution.TechFloor
{
    #region Enumerations    
    public enum RobotSequenceCommands : int
    {
        Unknown = -1,
        MoveToHome = 0,                                                 // Home (E;0;) wp0
        CheckReelTypeOfCart,                                            // Reel size check position (E;1000;) wp100
        ApplyReelTypeOfCart,                                            // Set reel size (S;1;7; or S;1;13;)
        ApplyWorkSlot,                                                  // Set work slot (S;2;1; for reel 13 inch slot 1, to S;2;6; for reel 7 inch slot 6.)
        ApplyReelTypeOfReturn,                                          // Set reel size (S;3;7; or S;3;13;)
        MoveToLoadFrontOfTower1,                                        // (E;1001;)
        MoveToLoadFrontOfTower2,                                        // (E;1002;)
        MoveToLoadFrontOfTower3,                                        // (E;1003;)
        MoveToLoadFrontOfTower4,                                        // (E;1004;)
        MoveToLoadFrontReel13OfReturnStage,                             // (E;1005;)
        MoveToLoadFrontReel7OfReturnStage,                              // (E;1006;)
        MoveToUnloadFrontOfTower1,                                      // (E;1101;)
        MoveToUnloadFrontOfTower2,                                      // (E;1102;)
        MoveToUnloadFrontOfTower3,                                      // (E;1103;)
        MoveToUnloadFrontOfTower4,                                      // (E;1104;)
        MoveToUnloadFrontReel13OfReturnStage,                           // (E;1105;)
        MoveToUnloadFrontReel7OfReturnStage,                            // (E;1106;)
        MoveToUnloadFrontOfOutput1,                                     // (E;1107;)
        MoveToUnloadFrontOfOutput2,                                     // (E;1108;)
        MoveToUnloadFrontOfOutput3,                                     // (E;1109;)
        ApproachToUnloadFrontOfOutput1,                                 // (E;1110;)
        ApproachToUnloadFrontOfOutput2,                                 // (E;1111;)
        ApproachToUnloadFrontOfOutput3,                                 // (E;1112;)
        MoveToReelHeightCheckPointAtWorkSlot1OfReel7Cart,               // Work slot 1 of reel 7 inch cart (E;201;1;)
        MoveToReelHeightCheckPointAtWorkSlot2OfReel7Cart,               // Work slot 2 of reel 7 inch cart (E;201;2;)
        MoveToReelHeightCheckPointAtWorkSlot3OfReel7Cart,               // Work slot 3 of reel 7 inch cart (E;201;3;)
        MoveToReelHeightCheckPointAtWorkSlot4OfReel7Cart,               // Work slot 4 of reel 7 inch cart (E;201;4;)
        MoveToReelHeightCheckPointAtWorkSlot5OfReel7Cart,               // Work slot 5 of reel 7 inch cart (E;201;5;)
        MoveToReelHeightCheckPointAtWorkSlot6OfReel7Cart,               // Work slot 6 of reel 7 inch cart (E;201;6;)
        MeasureReelHeightAtWorkSlot1OfReel7Cart,                        // Work slot 1 of reel 7 inch cart (E;202;1;)
        MeasureReelHeightAtWorkSlot2OfReel7Cart,                        // Work slot 2 of reel 7 inch cart (E;202;2;)
        MeasureReelHeightAtWorkSlot3OfReel7Cart,                        // Work slot 3 of reel 7 inch cart (E;202;3;)
        MeasureReelHeightAtWorkSlot4OfReel7Cart,                        // Work slot 4 of reel 7 inch cart (E;202;4;)
        MeasureReelHeightAtWorkSlot5OfReel7Cart,                        // Work slot 5 of reel 7 inch cart (E;202;5;)
        MeasureReelHeightAtWorkSlot6OfReel7Cart,                        // Work slot 6 of reel 7 inch cart (E;202;6;)
        MoveToReelHeightCheckPointAtWorkSlot1OfReel13Cart,              // Work slot 1 of reel 13 inch cart (E;211;1;)
        MoveToReelHeightCheckPointAtWorkSlot2OfReel13Cart,              // Work slot 2 of reel 13 inch cart (E;211;2;)
        MoveToReelHeightCheckPointAtWorkSlot3OfReel13Cart,              // Work slot 3 of reel 13 inch cart (E;211;3;)
        MoveToReelHeightCheckPointAtWorkSlot4OfReel13Cart,              // Work slot 4 of reel 13 inch cart (E;211;4;)
        MeasureReelHeightAtWorkSlot1OfReel13Cart,                       // Work slot 1 of reel 13 inch cart (E;212;1;)
        MeasureReelHeightAtWorkSlot2OfReel13Cart,                       // Work slot 2 of reel 13 inch cart (E;212;2;)
        MeasureReelHeightAtWorkSlot3OfReel13Cart,                       // Work slot 3 of reel 13 inch cart (E;212;3;)
        MeasureReelHeightAtWorkSlot4OfReel13Cart,                       // Work slot 4 of reel 13 inch cart (E;212;4;)
        ApplyAlignmentAndPickupLoadReelFromWorkSlot1OfReel7Cart,        // Apply alignment offset to reel of work slot. (E;203;1;<offset x>;<offset y>;)
        ApplyAlignmentAndPickupLoadReelFromWorkSlot2OfReel7Cart,        // Apply alignment offset to reel of work slot. (E;203;2;<offset x>;<offset y>;)
        ApplyAlignmentAndPickupLoadReelFromWorkSlot3OfReel7Cart,        // Apply alignment offset to reel of work slot. (E;203;3;<offset x>;<offset y>;)
        ApplyAlignmentAndPickupLoadReelFromWorkSlot4OfReel7Cart,        // Apply alignment offset to reel of work slot. (E;203;4;<offset x>;<offset y>;)
        ApplyAlignmentAndPickupLoadReelFromWorkSlot5OfReel7Cart,        // Apply alignment offset to reel of work slot. (E;203;5;<offset x>;<offset y>;)
        ApplyAlignmentAndPickupLoadReelFromWorkSlot6OfReel7Cart,        // Apply alignment offset to reel of work slot. (E;203;6;<offset x>;<offset y>;)
        ApplyAlignmentAndPickupLoadReelFromWorkSlot1OfReel13Cart,       // Apply alignment offset to reel of work slot. (E;213;1;<offset x>;<offset y>;)
        ApplyAlignmentAndPickupLoadReelFromWorkSlot2OfReel13Cart,       // Apply alignment offset to reel of work slot. (E;213;2;<offset x>;<offset y>;)
        ApplyAlignmentAndPickupLoadReelFromWorkSlot3OfReel13Cart,       // Apply alignment offset to reel of work slot. (E;213;3;<offset x>;<offset y>;)
        ApplyAlignmentAndPickupLoadReelFromWorkSlot4OfReel13Cart,       // Apply alignment offset to reel of work slot. (E;213;4;<offset x>;<offset y>;)
        MoveToReel7OfReturnStage,                                       // Reel 7 on return stage (E;220;7;)
        MoveToReel13OfReturnStage,                                      // Reel 13 on return stage (E;220;13;)
        ApproachToReelHeightCheckPointAtReel7OfReturnStage,             // Reel 7 on return stage (E;221;7;)
        ApproachToReelHeightCheckPointAtReel13OfReturnStage,            // Reel 13 on return stage (E;221;13;)
        MeasureReelHeightAtReel7OfReturnStage,                          // Reel 7 on return stage (E;222;7;)
        MeasureReelHeightAtReel13OfReturnStage,                         // Reel 13 on return stage (E;222;13;)
        ApplyAlignmentAndPickupLoadReelFromReel13OfReturnStage,         // Apply alignment offset to reel on return stage. (E;223;7;<offset x>;<offset y>;) 
        ApplyAlignmentAndPickupLoadReelFromReel7OfReturnStage,          // Apply alignment offset to reel on return stage. (E;223;13;<offset x>;<offset y>;)
        MoveBackToFrontOfReturnStage,                                   // Recover failure position (Vision error, barcode read failure, and pick up error) (E;224;)
        PutReelIntoTower1,                                              // (E;301;)
        PutReelIntoTower2,                                              // (E;302;)
        PutReelIntoTower3,                                              // (E;303;)
        PutReelIntoTower4,                                              // (E;304;)
        PutReelIntoReel7OfReturnStage,                                  // (E;401;)
        PutReelIntoReel13OfReturnStage,                                 // (E;402;)
        PutReelIntoOutput1,                                             // (E;501;)
        PutReelIntoOutput2,                                             // (E;502;)
        PutReelIntoOutput3,                                             // (E;503;)
        TakeReelFromTower1,                                             // (E;601;)
        TakeReelFromTower2,                                             // (E;602;)
        TakeReelFromTower3,                                             // (E;603;)
        TakeReelFromTower4,                                             // (E;604;)  
        GoToHomeBeforeReelHeightCheck,                           		// (E;200;)
        GoToHomeAfterPickUpReel,                                      	// (E;204;)
        ResetProcessCount,                                              // (S;4;)
        MoveBackToFrontOfUnloadTower1,                                  // (E;701;)
        MoveBackToFrontOfUnloadTower2,                                  // (E;702;)
        MoveBackToFrontOfUnloadTower3,                                  // (E;703;)
        MoveBackToFrontOfUnloadTower4,                                  // (E;704;)
    }
    #endregion

    public class RobotSequenceManager : SimulatableDevice
    {
        #region Constants
        protected readonly char[] CONST_TOKEN_DELIMITERS = { ' ', ';', '\r', '\n', '\0' };
        #endregion

        #region Fields
        protected bool waitResponse                             = false;
        protected bool failure                                  = false;
        protected bool reelGrip                                 = false;
        protected bool reelDetectSensor                         = false;
        protected bool reelHeightCheck                          = false;
        protected int receivedFlag                              = 0;
        protected int homed                                     = 0;
        protected int workSlot                                  = -1;
        protected int currentWaypoint                           = -1;
        protected int nextWaypoint                              = -1;
        protected int movingState                               = -1;
        protected int jogMovingState                            = -1;
        protected int jogMode                                   = 0;
        protected int robotProgramModel                         = -1;
        protected int previousScenario                          = -1;
        protected int previousCommand                           = -1;
        protected int executeCommand                            = -1;
        protected int returnCode                                = 0;
        protected int visionAlignmentStep                       = 0;
        protected int clientId                                  = 0;
        protected int sentTick                                  = 0;  
        protected int timeoutOfResponse                         = 10000;
        protected int poseZ                                     = 0;
        protected uint sentMessageId                            = 0;
        protected string message                                = string.Empty;
        protected string lastmessage                            = string.Empty;
        protected ReelTypes reelSizeOfCart                      = ReelTypes.Unknown;
        protected ReelTypes reelSizeOfReturn                    = ReelTypes.Unknown;
        protected RobotActionOrder robotActionOrder             = RobotActionOrder.None;
        protected RobotOperationOrderBy loadOperationOrderBy    = RobotOperationOrderBy.None;
        protected RobotActionStates previousRobotActionState    = RobotActionStates.Unknown;
        protected RobotActionStates robotActionState            = RobotActionStates.Unknown;
        protected RobotCommunicationStates robotState           = RobotCommunicationStates.Unknown;
        protected RobotActionFailures robotActionFailure        = RobotActionFailures.None;
        protected CommunicationStates communicationState        = CommunicationStates.None;
        protected AsyncSocketServer server                      = null;
        protected System.Timers.Timer ConnectionWatcher         = null;
        protected Queue<Pair<RobotSequenceCommands, int>> messageQueue = new Queue<Pair<RobotSequenceCommands, int>>();
        protected ConcurrentQueue<RobotCommunicationStates> responseQueue         = new ConcurrentQueue<RobotCommunicationStates>();
        protected Dictionary<int, AsyncSocketClient> clients    = new Dictionary<int, AsyncSocketClient>();        
        protected RobotSequenceCommands lastExecutedCommand    = RobotSequenceCommands.Unknown;
        protected Dictionary<RobotSequenceCommands, Pair<int, int>> robotScenarios_ = new Dictionary<RobotSequenceCommands, Pair<int, int>>()
        {
            { RobotSequenceCommands.MoveToHome,                                                         new Pair<int, int>(0,     0)      },
            { RobotSequenceCommands.CheckReelTypeOfCart,                                                new Pair<int, int>(100,   101)    },
            { RobotSequenceCommands.ApplyReelTypeOfCart,                                                new Pair<int, int>(1,     0)      },
            { RobotSequenceCommands.ApplyWorkSlot,                                                      new Pair<int, int>(2,     -1)     }, // Don't care current way point)
            { RobotSequenceCommands.ApplyReelTypeOfReturn,                                              new Pair<int, int>(3,     -1)     }, // Don't care current way point)
            { RobotSequenceCommands.ResetProcessCount,                                                  new Pair<int, int>(4,     -1)     }, // Don't care current way point)
            { RobotSequenceCommands.MoveToLoadFrontOfTower1,                                            new Pair<int, int>(1001,  10000)  },
            { RobotSequenceCommands.MoveToLoadFrontOfTower2,                                            new Pair<int, int>(1002,  20000)  },
            { RobotSequenceCommands.MoveToLoadFrontOfTower3,                                            new Pair<int, int>(1003,  30000)  },
            { RobotSequenceCommands.MoveToLoadFrontOfTower4,                                            new Pair<int, int>(1004,  40000)  },
            { RobotSequenceCommands.MoveToLoadFrontReel7OfReturnStage,                                  new Pair<int, int>(1005,  81000)  },
            { RobotSequenceCommands.MoveToLoadFrontReel13OfReturnStage,                                 new Pair<int, int>(1006,  82000)  },
            { RobotSequenceCommands.MoveToUnloadFrontOfTower1,                                          new Pair<int, int>(1101,  10000)  },
            { RobotSequenceCommands.MoveToUnloadFrontOfTower2,                                          new Pair<int, int>(1102,  20000)  },
            { RobotSequenceCommands.MoveToUnloadFrontOfTower3,                                          new Pair<int, int>(1103,  30000)  },
            { RobotSequenceCommands.MoveToUnloadFrontOfTower4,                                          new Pair<int, int>(1104,  40000)  },
            { RobotSequenceCommands.MoveToUnloadFrontReel7OfReturnStage,                                new Pair<int, int>(1105,  81000)  },
            { RobotSequenceCommands.MoveToUnloadFrontReel13OfReturnStage,                               new Pair<int, int>(1106,  82000)  },
            { RobotSequenceCommands.MoveToUnloadFrontOfOutput1,                                         new Pair<int, int>(1107,  50000)  },   /////
            { RobotSequenceCommands.MoveToUnloadFrontOfOutput2,                                         new Pair<int, int>(1108,  60000)  },   // same points
            { RobotSequenceCommands.MoveToUnloadFrontOfOutput3,                                         new Pair<int, int>(1109,  70000)  },   ////
            { RobotSequenceCommands.ApproachToUnloadFrontOfOutput1,                                     new Pair<int, int>(1110,  50001)  },
            { RobotSequenceCommands.ApproachToUnloadFrontOfOutput2,                                     new Pair<int, int>(1111,  60001)  },
            { RobotSequenceCommands.ApproachToUnloadFrontOfOutput3,                                     new Pair<int, int>(1112,  70001)  },
            { RobotSequenceCommands.GoToHomeBeforeReelHeightCheck,                                      new Pair<int, int>(200,   0)      },   //added 190417 
            { RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot1OfReel7Cart,                   new Pair<int, int>(201,   91101)  },   //(200,91100)
            { RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot2OfReel7Cart,                   new Pair<int, int>(201,   91201)  },   //(200,91200)
            { RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot3OfReel7Cart,                   new Pair<int, int>(201,   91301)  },   //(200,91300)
            { RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot4OfReel7Cart,                   new Pair<int, int>(201,   91401)  },   //(200,91400)
            { RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot5OfReel7Cart,                   new Pair<int, int>(201,   91501)  },   //(200,91500)
            { RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot6OfReel7Cart,                   new Pair<int, int>(201,   91601)  },   //(200,91600)
            { RobotSequenceCommands.MeasureReelHeightAtWorkSlot1OfReel7Cart,                            new Pair<int, int>(202,   91103)  },   //(201,91102)
            { RobotSequenceCommands.MeasureReelHeightAtWorkSlot2OfReel7Cart,                            new Pair<int, int>(202,   91203)  },   //(201,91202)
            { RobotSequenceCommands.MeasureReelHeightAtWorkSlot3OfReel7Cart,                            new Pair<int, int>(202,   91303)  },   //(201,91302)
            { RobotSequenceCommands.MeasureReelHeightAtWorkSlot4OfReel7Cart,                            new Pair<int, int>(202,   91403)  },   //(201,91402)
            { RobotSequenceCommands.MeasureReelHeightAtWorkSlot5OfReel7Cart,                            new Pair<int, int>(202,   91503)  },   //(201,91502)
            { RobotSequenceCommands.MeasureReelHeightAtWorkSlot6OfReel7Cart,                            new Pair<int, int>(202,   91603)  },   //(201,91602)
            { RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot1OfReel13Cart,                  new Pair<int, int>(211,   92101)  },   //(210,92100)
            { RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot2OfReel13Cart,                  new Pair<int, int>(211,   92201)  },   //(210,92200)
            { RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot3OfReel13Cart,                  new Pair<int, int>(211,   92301)  },   //(210,92300)
            { RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot4OfReel13Cart,                  new Pair<int, int>(211,   92401)  },   //(210,92400)
            { RobotSequenceCommands.MeasureReelHeightAtWorkSlot1OfReel13Cart,                           new Pair<int, int>(212,   92103)  },   //(211,92102)
            { RobotSequenceCommands.MeasureReelHeightAtWorkSlot2OfReel13Cart,                           new Pair<int, int>(212,   92203)  },   //(211,92202)
            { RobotSequenceCommands.MeasureReelHeightAtWorkSlot3OfReel13Cart,                           new Pair<int, int>(212,   92303)  },   //(211,92302)
            { RobotSequenceCommands.MeasureReelHeightAtWorkSlot4OfReel13Cart,                           new Pair<int, int>(212,   92403)  },   //(211,92402)
            { RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot1OfReel7Cart,            new Pair<int, int>(203,   91108)  },   //(202,91103)// making temp waypoint   91103    9900 is another name for home. 
            { RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot2OfReel7Cart,            new Pair<int, int>(203,   91208)  },   //(202,91203)
            { RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot3OfReel7Cart,            new Pair<int, int>(203,   91308)  },   //(202,91303)
            { RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot4OfReel7Cart,            new Pair<int, int>(203,   91408)  },   //(202,91403)
            { RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot5OfReel7Cart,            new Pair<int, int>(203,   91508)  },   //(202,91503)
            { RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot6OfReel7Cart,            new Pair<int, int>(203,   91608)  },   //(202,91603)
            { RobotSequenceCommands.GoToHomeAfterPickUpReel,                                            new Pair<int, int>(204,   0)      },   //
            { RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot1OfReel13Cart,           new Pair<int, int>(213,   92107)  },   //(212,92103)
            { RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot2OfReel13Cart,           new Pair<int, int>(213,   92207)  },   //(212,92203)
            { RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot3OfReel13Cart,           new Pair<int, int>(213,   92307)  },   //(212,92303)
            { RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot4OfReel13Cart,           new Pair<int, int>(213,   92407)  },   //(212,92403)
            { RobotSequenceCommands.MoveToReel7OfReturnStage,                                           new Pair<int, int>(220,   81000)  },
            { RobotSequenceCommands.MoveToReel13OfReturnStage,                                          new Pair<int, int>(220,   82000)  },
            { RobotSequenceCommands.ApproachToReelHeightCheckPointAtReel7OfReturnStage,                 new Pair<int, int>(221,   81001)  },
            { RobotSequenceCommands.ApproachToReelHeightCheckPointAtReel13OfReturnStage,                new Pair<int, int>(221,   82001)  },
            { RobotSequenceCommands.MeasureReelHeightAtReel7OfReturnStage,                              new Pair<int, int>(222,   81002)  },
            { RobotSequenceCommands.MeasureReelHeightAtReel13OfReturnStage,                             new Pair<int, int>(222,   82002)  },
            { RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromReel7OfReturnStage,              new Pair<int, int>(223,   81000)  },
            { RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromReel13OfReturnStage,             new Pair<int, int>(223,   82000)  },
            { RobotSequenceCommands.MoveBackToFrontOfReturnStage,                                       new Pair<int, int>(224,   81000)  },
            { RobotSequenceCommands.PutReelIntoTower1,                                                  new Pair<int, int>(301,   10000)  },
            { RobotSequenceCommands.PutReelIntoTower2,                                                  new Pair<int, int>(302,   20000)  },
            { RobotSequenceCommands.PutReelIntoTower3,                                                  new Pair<int, int>(303,   30000)  },
            { RobotSequenceCommands.PutReelIntoTower4,                                                  new Pair<int, int>(304,   40000)  },
            { RobotSequenceCommands.PutReelIntoReel7OfReturnStage,                                      new Pair<int, int>(401,   81000)  },
            { RobotSequenceCommands.PutReelIntoReel13OfReturnStage,                                     new Pair<int, int>(402,   82000)  },
            { RobotSequenceCommands.PutReelIntoOutput1,                                                 new Pair<int, int>(501,   50000)  }, //50000 (Safe) -> Move to Put into (50001) -> 50000
            { RobotSequenceCommands.PutReelIntoOutput2,                                                 new Pair<int, int>(502,   60000)  },
            { RobotSequenceCommands.PutReelIntoOutput3,                                                 new Pair<int, int>(503,   70000)  },
            { RobotSequenceCommands.TakeReelFromTower1,                                                 new Pair<int, int>(601,   10000)  },  // 10000 means it will do the pickup and come back at this point after finishing this sequence.
            { RobotSequenceCommands.TakeReelFromTower2,                                                 new Pair<int, int>(602,   20000)  },
            { RobotSequenceCommands.TakeReelFromTower3,                                                 new Pair<int, int>(603,   30000)  },
            { RobotSequenceCommands.TakeReelFromTower4,                                                 new Pair<int, int>(604,   40000)  },
            { RobotSequenceCommands.MoveBackToFrontOfUnloadTower1,                                      new Pair<int, int>(701,   10000)  },
            { RobotSequenceCommands.MoveBackToFrontOfUnloadTower2,                                      new Pair<int, int>(702,   20000)  },
            { RobotSequenceCommands.MoveBackToFrontOfUnloadTower3,                                      new Pair<int, int>(703,   30000)  },
            { RobotSequenceCommands.MoveBackToFrontOfUnloadTower4,                                      new Pair<int, int>(704,   40000)  },
        };
        #endregion

        #region Properties
        public bool IsReceived                                  => (waitResponse && receivedFlag == 1);
        public bool IsConnected                                 => clients.Count > 0;
        public bool IsFailure                                   => (returnCode < 0 || failure);
        public bool IsHomed                                     => (homed > 0);
        public bool IsActualHomeFailure                         => (homed <= 0 && lastExecutedCommand != RobotSequenceCommands.MoveToHome);
        public bool IsMoving                                    => (movingState > 0 || jogMovingState > 0);
        public bool IsAtSafePosition                            => (currentWaypoint % 100 == 0);
        public bool ReelGrip                                    => reelGrip;
        public bool ReelDetector                                => reelDetectSensor;
        public bool HasAReel                                    => reelGrip && reelDetectSensor;
        public bool IsCheckedReelHeight                         => reelHeightCheck;
        public bool IsPossibleStop                              => IsConnected && IsHomed && currentWaypoint == nextWaypoint && !IsMoving;
        public bool IsWaitForOrder                              => IsConnected && IsHomed && currentWaypoint == nextWaypoint && !IsMoving && !IsFailure;
        public int WorkSlot                                     => workSlot;
        public int JogMode                                      => jogMode;
        public int CurrentWaypoint                              => currentWaypoint;
        public int NextWaypoint                                 => nextWaypoint;
        public int VisionAlignmentStep                          => visionAlignmentStep;
        public int PreviousCase                                 => previousScenario;
        public int PoseZ                                        => poseZ;
        public ReelTypes ReelTypeOfCart                         => reelSizeOfCart;
        public ReelTypes ReelTypeOfReturn                       => reelSizeOfReturn;
        public RobotActionOrder RobotActionOrder
        {
            get => robotActionOrder;
            set
            {
                if (robotActionOrder != value)
                    robotActionOrder = value;
            }                              
        }
        public RobotOperationOrderBy LoadOperationOrderBy
        {
            get => loadOperationOrderBy;
            set
            {
                if (loadOperationOrderBy != value)
                    loadOperationOrderBy = value;
            }
        }
        public RobotActionStates PreviousActionState            => previousRobotActionState;
        public RobotActionStates ActionState                    => robotActionState;
        public RobotCommunicationStates State                   => robotState;
        public RobotActionFailures FailureCode                  => robotActionFailure;
        public RobotSequenceCommands LastExecutedCommand        => lastExecutedCommand;
        public CommunicationStates CommunicationState           => communicationState;
        public bool IsRobotAlreadyAtHome                        => (IsConnected && !IsFailure && !IsMoving && IsHomed && currentWaypoint == nextWaypoint && currentWaypoint == 0);
        public bool IsRobotAtHome                               => (IsConnected && IsReceived && !IsFailure && !IsMoving && IsHomed && currentWaypoint == nextWaypoint && currentWaypoint == 0);
        public bool IsRobotAtPosition(int waypoint)             => (IsConnected && IsReceived && !IsFailure && !IsMoving && IsHomed && waypoint >= 0 && waypoint == currentWaypoint && waypoint == nextWaypoint);        
        public bool IsReadyToUnloadReel
        {
            get
            {
                bool safepos = false;
                IsRobotAtWayPointByCommand(lastExecutedCommand, ref safepos);
                return safepos && !IsMoving && !reelDetectSensor && !IsFailure;
            }
        }
        #endregion

        #region Events
        public event EventHandler<CommunicationStates> CommunicationStateChanged;
        public event EventHandler<string> ReportRuntimeLog;
        #endregion

        #region Protected methods
        protected override void DisposeManagedObjects()
        {
            base.DisposeManagedObjects();

            if (ConnectionWatcher != null)
                ConnectionWatcher.Stop();
        }

        protected void OnTickWatcher(object sender, EventArgs e)
        {
            ConnectionWatcher.Stop();
            if (IsConnected)
            {
                if (messageQueue.Count > 0)
                {
                    int timeout_ = timeoutOfResponse;
                    Pair<RobotSequenceCommands, int> sentMessage_ = messageQueue.Peek();

                    switch (sentMessage_.first)
                    {
                        case RobotSequenceCommands.MoveToHome:
                            timeout_ *= 6; // 60 sec
                            break;                        
                        case RobotSequenceCommands.ApplyReelTypeOfCart:
                        case RobotSequenceCommands.ApplyReelTypeOfReturn:
                        case RobotSequenceCommands.ApplyWorkSlot:
                        case RobotSequenceCommands.ResetProcessCount:
                        case RobotSequenceCommands.MoveBackToFrontOfUnloadTower1:
                        case RobotSequenceCommands.MoveBackToFrontOfUnloadTower2:
                        case RobotSequenceCommands.MoveBackToFrontOfUnloadTower3:
                        case RobotSequenceCommands.MoveBackToFrontOfUnloadTower4:
                            break;// 10 sec
                        case RobotSequenceCommands.CheckReelTypeOfCart:
                        case RobotSequenceCommands.MoveToLoadFrontOfTower1:                                       
                        case RobotSequenceCommands.MoveToLoadFrontOfTower2:                                       
                        case RobotSequenceCommands.MoveToLoadFrontOfTower3:                                       
                        case RobotSequenceCommands.MoveToLoadFrontOfTower4:                                       
                        case RobotSequenceCommands.MoveToLoadFrontReel13OfReturnStage:                            
                        case RobotSequenceCommands.MoveToLoadFrontReel7OfReturnStage:                             
                        case RobotSequenceCommands.MoveToUnloadFrontOfTower1:                                     
                        case RobotSequenceCommands.MoveToUnloadFrontOfTower2:                                     
                        case RobotSequenceCommands.MoveToUnloadFrontOfTower3:                                     
                        case RobotSequenceCommands.MoveToUnloadFrontOfTower4:                                     
                        case RobotSequenceCommands.MoveToUnloadFrontReel13OfReturnStage:                          
                        case RobotSequenceCommands.MoveToUnloadFrontReel7OfReturnStage:                           
                        case RobotSequenceCommands.MoveToUnloadFrontOfOutput1:                                    
                        case RobotSequenceCommands.MoveToUnloadFrontOfOutput2:                                    
                        case RobotSequenceCommands.MoveToUnloadFrontOfOutput3:                                    
                        case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot1OfReel7Cart:                        
                        case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot2OfReel7Cart:                        
                        case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot3OfReel7Cart:                        
                        case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot4OfReel7Cart:                        
                        case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot5OfReel7Cart:                        
                        case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot6OfReel7Cart:                        
                        case RobotSequenceCommands.MeasureReelHeightAtWorkSlot1OfReel7Cart:                       
                        case RobotSequenceCommands.MeasureReelHeightAtWorkSlot2OfReel7Cart:                       
                        case RobotSequenceCommands.MeasureReelHeightAtWorkSlot3OfReel7Cart:                       
                        case RobotSequenceCommands.MeasureReelHeightAtWorkSlot4OfReel7Cart:                       
                        case RobotSequenceCommands.MeasureReelHeightAtWorkSlot5OfReel7Cart:                       
                        case RobotSequenceCommands.MeasureReelHeightAtWorkSlot6OfReel7Cart:                       
                        case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot1OfReel13Cart:                       
                        case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot2OfReel13Cart:                       
                        case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot3OfReel13Cart:                       
                        case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot4OfReel13Cart:                       
                        case RobotSequenceCommands.MeasureReelHeightAtWorkSlot1OfReel13Cart:                      
                        case RobotSequenceCommands.MeasureReelHeightAtWorkSlot2OfReel13Cart:                      
                        case RobotSequenceCommands.MeasureReelHeightAtWorkSlot3OfReel13Cart:                      
                        case RobotSequenceCommands.MeasureReelHeightAtWorkSlot4OfReel13Cart:                      
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot1OfReel7Cart:       
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot2OfReel7Cart:       
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot3OfReel7Cart:       
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot4OfReel7Cart:       
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot5OfReel7Cart:       
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot6OfReel7Cart:       
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot1OfReel13Cart:      
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot2OfReel13Cart:      
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot3OfReel13Cart:      
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot4OfReel13Cart:      
                        case RobotSequenceCommands.ApproachToReelHeightCheckPointAtReel7OfReturnStage:                          
                        case RobotSequenceCommands.ApproachToReelHeightCheckPointAtReel13OfReturnStage:                         
                        case RobotSequenceCommands.MeasureReelHeightAtReel7OfReturnStage:                         
                        case RobotSequenceCommands.MeasureReelHeightAtReel13OfReturnStage:                        
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromReel13OfReturnStage:        
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromReel7OfReturnStage:         
                        case RobotSequenceCommands.PutReelIntoTower1:                                             
                        case RobotSequenceCommands.PutReelIntoTower2:                                             
                        case RobotSequenceCommands.PutReelIntoTower3:                                             
                        case RobotSequenceCommands.PutReelIntoTower4:                                             
                        case RobotSequenceCommands.PutReelIntoReel13OfReturnStage:                                
                        case RobotSequenceCommands.PutReelIntoReel7OfReturnStage:                                 
                        case RobotSequenceCommands.PutReelIntoOutput1:                                            
                        case RobotSequenceCommands.PutReelIntoOutput2:                                            
                        case RobotSequenceCommands.PutReelIntoOutput3:                                            
                        case RobotSequenceCommands.TakeReelFromTower1:                                            
                        case RobotSequenceCommands.TakeReelFromTower2:                                            
                        case RobotSequenceCommands.TakeReelFromTower3:                                            
                        case RobotSequenceCommands.TakeReelFromTower4:
                            timeout_ *= 3; // 30 sec
                            break;
                    }

                    IsResponseTimeout(timeout_);
                }
            }
            ConnectionWatcher.Start();
        }

        protected void RemoveClient(int id)
        {
            foreach (var client in clients)
            {
                if (id == client.Key)
                {
                    clients.Remove(id);
                    break;
                }
            }
        }

        protected void FireCommunicationStateChanged(CommunicationStates state)
        {
            CommunicationStateChanged?.Invoke(this, communicationState = state);
        }

        protected void OnServerAccept(object sender, AsyncSocketAcceptEventArgs e)
        {
            foreach (var client in clients)
            {
                IPEndPoint s1 = client.Value.Sock.RemoteEndPoint as IPEndPoint;
                IPEndPoint s2 = e.Worker.RemoteEndPoint as IPEndPoint;

                if (s1.Address.ToString().Contains(s2.Address.ToString()))
                    client.Value.Sock.Disconnect(true);
            }

            FireCommunicationStateChanged(CommunicationStates.Accepted);
            FireUpdateRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}={++clientId}");

            Thread.Sleep(100);
            AsyncSocketClient newClient = new AsyncSocketClient(clientId, e.Worker, OnClientConnected, OnClientSent, OnClientReceived);
            newClient.OnDisconnected    += new AsyncSocketDisconnectedEventHandler(OnClientDisconnected);
            newClient.OnError           += new AsyncSocketErrorEventHandler(OnClientError);

            lock (clients)
                clients.Add(clientId, newClient);
        }

        
        protected void OnClientError(object sender, AsyncSocketErrorEventArgs e)
        {   // When the server got socket error, it call disconnect then the socket handle is removed in disconnection callback.
            try
            {
                FlushMessageQueue();
                FireCommunicationStateChanged(CommunicationStates.Error);
                FireUpdateRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}({e.ID}){e.AsyncSocketException.Message}");

                lock (clients)
                {
                    foreach (var obj in clients)
                        clients[obj.Key].Sock.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                Logger.Trace(ex.Message);
                RemoveClient(e.ID);
            }
        }

        protected void OnClientConnected(object sender, AsyncSocketConnectionEventArgs e)
        {   // Reset last received message buffer
            lastmessage = string.Empty;
            FireCommunicationStateChanged(CommunicationStates.Connected);
            FireUpdateRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}={e.ID}");
        }

        protected void OnClientDisconnected(object sender, AsyncSocketConnectionEventArgs e)
        {
            FlushMessageQueue();
            FireCommunicationStateChanged(CommunicationStates.Disconnected);
            FireUpdateRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}={e.ID}");
            RemoveClient(e.ID);

            // It have to call dashboard server to check program running.
            // Reset last received message buffer
            lastmessage = string.Empty;
        }

        
        protected void ProcessStateMessage(string[] arguments)
        {
            try
            {
                // State message format:
                // r;prevstate=-1;opstate=-1;homed=-1;currentwp=-1;nextwp=-1;moving=0;jogmoving=0;jogmode=0;model=-1;prevcase=-1;prevcmd=-1;hostcmd=-1;returncode=0;
                // prevstate: previous operation state
                // opstate: operation state
                // homed: robot initialization state (homing)
                // currentwp: current waypoint
                // nextwp: next waypoint
                // reelgrip: reel gripper on / off
                // reeldetect: reel detect sensor on / off
                // moving: robot moving state
                // jogmoving: robot jog moving state
                // jogmode: robot jog mode
                // model: robot program waypoints model
                // prevcase: previous executed scenario
                // prevcmd: previous executed host command
                // hostcmd: current executed host command
                // returncode: executed host command result
                // posez: robot axis z height

                for (int i = 1; i < arguments.Length; i++)
                {
                    string[] items_ = arguments[i].Split('=');

                    if (items_.Length >= 2)
                    {
                        switch (items_[0])
                        {
                            case "prevstate":
                                {
                                    switch ((RobotActionStates)int.Parse(items_[1]))
                                    {
                                        default:
                                        case RobotActionStates.Unknown:
                                            {   // Required homing
                                                previousRobotActionState = RobotActionStates.Unknown;
                                            }
                                            break;
                                        case RobotActionStates.Stop:
                                            {
                                                previousRobotActionState = RobotActionStates.Stop;
                                            }
                                            break;
                                        case RobotActionStates.Load:
                                            {
                                                previousRobotActionState = RobotActionStates.Load;
                                            }
                                            break;
                                        case RobotActionStates.Loading:
                                            {
                                                previousRobotActionState = RobotActionStates.Loading;
                                            }
                                            break;
                                        case RobotActionStates.LoadCompleted:
                                            {
                                                previousRobotActionState = RobotActionStates.LoadCompleted;
                                            }
                                            break;
                                        case RobotActionStates.Unload:
                                            {
                                                previousRobotActionState = RobotActionStates.Unload;
                                            }
                                            break;
                                        case RobotActionStates.Unloading:
                                            {
                                                previousRobotActionState = RobotActionStates.Unloading;
                                            }
                                            break;
                                        case RobotActionStates.UnloadCompleted:
                                            {
                                                previousRobotActionState = RobotActionStates.UnloadCompleted;
                                            }
                                            break;
                                    }
                                }
                                break;
                            case "opstate":
                                {
                                    switch ((RobotActionStates)int.Parse(items_[1]))
                                    {
                                        default:
                                        case RobotActionStates.Unknown:
                                            {   // Required homing
                                                // Power on state
                                                robotActionState = RobotActionStates.Unknown;
                                            }
                                            break;
                                        case RobotActionStates.Stop:
                                            {   // Homed
                                                // After check reel size of cart
                                                // After set reel size of cart
                                                // After set reel size of return
                                                // After set work slot of cart
                                                // After measure reel height
                                                // After vision check to prepare adjust alignment of reel
                                                // After barcode confirm to load a reel
                                                // After received REPLY_REEL_LOAD_MOVE with confirm return code
                                                // After received REQUEST_REEL_UNLOAD_MOVE
                                                // After move to return stage to measure reel height
                                                robotActionState = RobotActionStates.Stop;
                                            }
                                            break;
                                        case RobotActionStates.Load:
                                            {
                                                // After received REPLY_BARCODEINFO_CONFIRM
                                                // After picked up a reel to load
                                                // And has a reel on gripper
                                                robotActionState = RobotActionStates.Load;
                                            }
                                            break;
                                        case RobotActionStates.Loading:
                                            {
                                                // Move to front of tower with a load reel
                                                robotActionState = RobotActionStates.Loading;
                                            }
                                            break;
                                        case RobotActionStates.LoadCompleted:
                                            {
                                                // After send REQUEST_REEL_LOAD_ASSIGN
                                                // And doesn't have a reel on gripper
                                                robotActionState = RobotActionStates.LoadCompleted;
                                            }
                                            break;
                                        case RobotActionStates.Unload:
                                            {
                                                // After reply REPLY_REEL_UNLOAD_MOVE
                                                // And doesn't have a reel on gripper
                                                // Move to front of tower
                                                robotActionState = RobotActionStates.Unload;
                                            }
                                            break;
                                        case RobotActionStates.Unloading:
                                            {
                                                // After take out a reel from tower with a reel on gripper
                                                // Stop a front of a tower
                                                // Move to output stage with an unload reel
                                                robotActionState = RobotActionStates.Unloading;
                                            }
                                            break;
                                        case RobotActionStates.UnloadCompleted:
                                            {
                                                // After drop an unload reel to output stage
                                                // Move up to safe position of output stage top
                                                robotActionState = RobotActionStates.UnloadCompleted;
                                            }
                                            break;
                                    }
                                }
                                break;
                            case "homed":
                                {
                                    homed = int.Parse(items_[1]);
                                }
                                break;
                            case "currentwp":
                                {
                                    currentWaypoint = int.Parse(items_[1]);
                                }
                                break;
                            case "nextwp":
                                {
                                    nextWaypoint = int.Parse(items_[1]);
                                }
                                break;
                            case "reelgrip":
                                {
                                    reelGrip = bool.Parse(items_[1]);
                                }
                                break;
                            case "reeldetect":
                                {
                                    reelDetectSensor = bool.Parse(items_[1]);
                                }
                                break;
                            case "reelheightcheck":
                                {
                                    reelHeightCheck = bool.Parse(items_[1]);
                                }
                                break;
                            case "visionalignmentstep":
                                {
                                    visionAlignmentStep = int.Parse(items_[1]);
                                }
                                break;
                            case "moving":
                                {
                                    movingState = int.Parse(items_[1]);
                                }
                                break;
                            case "jogmoving":
                                {
                                    jogMovingState = int.Parse(items_[1]);
                                }
                                break;
                            case "jogmode":
                                {
                                    jogMode = int.Parse(items_[1]);
                                }
                                break;
                            case "model":
                                {
                                    robotProgramModel = int.Parse(items_[1]);
                                }
                                break;
                            case "prevcase":
                                {
                                    previousScenario = int.Parse(items_[1]);
                                }
                                break;
                            case "prevcmd":
                                {
                                    previousCommand = int.Parse(items_[1]);
                                }
                                break;
                            case "hostcmd":
                                {
                                    executeCommand = int.Parse(items_[1]);
                                }
                                break;
                            case "returncode":
                                {
                                    returnCode = int.Parse(items_[1]);
                                }
                                break;
                            case "workslot":
                                {
                                    workSlot = int.Parse(items_[1]);
                                }
                                break;
                            case "cartreelsize":
                                {
                                    reelSizeOfCart = (ReelTypes)Enum.Parse(typeof(ReelTypes), items_[1]);
                                }
                                break;
                            case "returnreelsize":
                                {
                                    reelSizeOfReturn = (ReelTypes)Enum.Parse(typeof(ReelTypes), items_[1]);
                                }
                                break;
                            case "posez":
                                {
                                    poseZ = int.Parse(items_[1]);
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                MessageBox.Show($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        protected void ProcessResponseMessage(string[] arguments)
        {

        }

        
        protected void OnClientReceived(object sender, AsyncSocketReceiveEventArgs e)
        {
            try
            {
                string message_ = string.Empty;

                if (string.IsNullOrEmpty(message_ = (new string(Encoding.Default.GetChars(e.ReceiveData))).Replace("\0", string.Empty)))
                    return;

                if (lastmessage != message_)
                {
                    string[] tokens_ = message_.Split(CONST_TOKEN_DELIMITERS, StringSplitOptions.RemoveEmptyEntries);

                    switch (tokens_[0])
                    {
                        case "r":
                            {   // Parse and process data of state message
                                ProcessStateMessage(tokens_);
                            }
                            break;
                        case "R":
                            {   // Parse and process query response
                                ProcessResponseMessage(tokens_);
                            }
                            break;
                    }

                    Pair<RobotSequenceCommands, int> sentMessage_ = null;
                    GetSentMessage(ref sentMessage_);
                    SetConversationFlag();
                    FireUpdateRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}={lastmessage = message_}");
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                MessageBox.Show($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        protected void OnClientSent(object sender, AsyncSocketSendEventArgs e)
        {
        }

        protected bool Send(string message, bool waitbit = false)
        {
            int sentCount_ = 0;
            FireUpdateRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}({sentMessageId++}:{waitResponse = waitbit})={message}");

            foreach (var client in clients.Values)
            {
                if (client.Sock.Connected)
                {
                    if (client.Send(Encoding.Default.GetBytes(message)))
                        sentCount_++;
                }
            }

            return (sentCount_ > 0);
        }

        protected void FlushMessageQueue()
        {
            Pair<RobotSequenceCommands, int> message_ = null;
            if (messageQueue.Count > 0)
            {
                lock (messageQueue)
                {
                    while (messageQueue.Count > 0)
                        message_ = messageQueue.Dequeue();
                }
            }
        }
        #endregion

        #region Public methods
        #region Server service control methods
        public void StartServer(int portno = 0)
        {
            if (server == null)
            {
                server = new AsyncSocketServer(portno);
                server.OnAccepted += new AsyncSocketAcceptedEventHandler(OnServerAccept);
                server.OnError += new AsyncSocketErrorEventHandler(OnClientError);
            }

            server.Start();
        }

        public void StopServer(bool forced = false)
        {
            server.Stop(forced);
            server.OnAccepted -= new AsyncSocketAcceptedEventHandler(OnServerAccept);
            server.OnError -= new AsyncSocketErrorEventHandler(OnClientError);
            server = null;

            FlushMessageQueue();
        }
        #endregion

        public bool GetResponse(ref RobotCommunicationStates response)
        {
            return responseQueue.TryDequeue(out response);
        }

        public void Reset()
        {
            SetConversationFlag(false);
            returnCode = 0;
            failure = false;
        }

        public void SetConversationFlag(bool state = true)
        {
            if (state)
                Interlocked.Exchange(ref receivedFlag, 1);
            else
            {
                Interlocked.Exchange(ref receivedFlag, 0);
                returnCode = 0;
            }
        }

        public void FireUpdateRuntimeLog(string text = null)
        {
            ReportRuntimeLog?.Invoke(this, text);
        }

        public void Create(int portno)
        {
            CommunicationStateChanged   += (App.MainForm as FormMain).UpdateRobotSequenceCommunicationState;
            ReportRuntimeLog            += (App.MainForm as FormMain).OnReportRobotRuntimeLog;
            StartServer(portno);

            ConnectionWatcher = new System.Timers.Timer();
            ConnectionWatcher.Interval = 1000;
            ConnectionWatcher.Elapsed += OnTickWatcher;
            ConnectionWatcher.Start();

            FireCommunicationStateChanged(CommunicationStates.Listening);
            FireUpdateRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}");
        }

        public void Destroy()
        {
            ConnectionWatcher.Stop();
            StopServer();
            CommunicationStateChanged -= (App.MainForm as FormMain).UpdateRobotSequenceCommunicationState;
            ReportRuntimeLog -= (App.MainForm as FormMain).OnReportRobotRuntimeLog;
        }

        public bool GetSentMessage(ref Pair<RobotSequenceCommands, int> message)
        {
            if (messageQueue.Count > 0)
            {
                lock (messageQueue)
                {
                    message = messageQueue.Dequeue();
                    return true;
                }
            }

            return false;
        }

        
        public bool IsResponseTimeout(int timeout = 1000)
        {
            try
            {
                if (messageQueue.Count > 0)
                {
                    Pair<RobotSequenceCommands, int> sentMessage_ = messageQueue.Peek();

                    if (TimeSpan.FromMilliseconds(App.TickCount - sentMessage_.second).TotalMilliseconds > timeout)
                    {   // Remove message from queue.
                        GetSentMessage(ref sentMessage_);
                        // failure = true;
                        FireUpdateRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}: Response Timeout ({sentMessage_})");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Trace($"{ClassName}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return failure;
        }

        
        public bool SendCommand(RobotSequenceCommands command, string arg = null, string delimiter = "\n")
        {
            bool waitbit_ = true;
            string commandText_ = "E;";

            if (messageQueue.Count > 0 || !robotScenarios_.ContainsKey(command))
            {
                return false;
            }

            int commandCode_ = robotScenarios_[command].first;

            try
            {
                switch (command)
                {
                    case RobotSequenceCommands.ApplyReelTypeOfCart:
                    case RobotSequenceCommands.ApplyReelTypeOfReturn:
                    case RobotSequenceCommands.ApplyWorkSlot:
                    case RobotSequenceCommands.ResetProcessCount:
                        {   // Set variable
                            commandText_ = string.IsNullOrEmpty(arg) ? $"S;{commandCode_};{delimiter}" : $"S;{commandCode_};{arg};{delimiter}";
                        }
                        break;
                    case RobotSequenceCommands.MoveToHome:
                    case RobotSequenceCommands.CheckReelTypeOfCart:
                    case RobotSequenceCommands.MoveToLoadFrontOfTower1:
                    case RobotSequenceCommands.MoveToLoadFrontOfTower2:
                    case RobotSequenceCommands.MoveToLoadFrontOfTower3:
                    case RobotSequenceCommands.MoveToLoadFrontOfTower4:
                    case RobotSequenceCommands.MoveToLoadFrontReel13OfReturnStage:
                    case RobotSequenceCommands.MoveToLoadFrontReel7OfReturnStage:
                    case RobotSequenceCommands.MoveToUnloadFrontOfTower1:
                    case RobotSequenceCommands.MoveToUnloadFrontOfTower2:
                    case RobotSequenceCommands.MoveToUnloadFrontOfTower3:
                    case RobotSequenceCommands.MoveToUnloadFrontOfTower4:
                    case RobotSequenceCommands.MoveToUnloadFrontReel13OfReturnStage:
                    case RobotSequenceCommands.MoveToUnloadFrontReel7OfReturnStage:
                    case RobotSequenceCommands.MoveToUnloadFrontOfOutput1:
                    case RobotSequenceCommands.MoveToUnloadFrontOfOutput2:
                    case RobotSequenceCommands.MoveToUnloadFrontOfOutput3:
                    case RobotSequenceCommands.ApproachToUnloadFrontOfOutput1:
                    case RobotSequenceCommands.ApproachToUnloadFrontOfOutput2:
                    case RobotSequenceCommands.ApproachToUnloadFrontOfOutput3:
                    case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot1OfReel7Cart:
                    case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot2OfReel7Cart:
                    case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot3OfReel7Cart:
                    case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot4OfReel7Cart:
                    case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot5OfReel7Cart:
                    case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot6OfReel7Cart:
                    case RobotSequenceCommands.MeasureReelHeightAtWorkSlot1OfReel7Cart:
                    case RobotSequenceCommands.MeasureReelHeightAtWorkSlot2OfReel7Cart:
                    case RobotSequenceCommands.MeasureReelHeightAtWorkSlot3OfReel7Cart:
                    case RobotSequenceCommands.MeasureReelHeightAtWorkSlot4OfReel7Cart:
                    case RobotSequenceCommands.MeasureReelHeightAtWorkSlot5OfReel7Cart:
                    case RobotSequenceCommands.MeasureReelHeightAtWorkSlot6OfReel7Cart:
                    case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot1OfReel13Cart:
                    case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot2OfReel13Cart:
                    case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot3OfReel13Cart:
                    case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot4OfReel13Cart:
                    case RobotSequenceCommands.MeasureReelHeightAtWorkSlot1OfReel13Cart:
                    case RobotSequenceCommands.MeasureReelHeightAtWorkSlot2OfReel13Cart:
                    case RobotSequenceCommands.MeasureReelHeightAtWorkSlot3OfReel13Cart:
                    case RobotSequenceCommands.MeasureReelHeightAtWorkSlot4OfReel13Cart:
                    case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot1OfReel7Cart:
                    case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot2OfReel7Cart:
                    case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot3OfReel7Cart:
                    case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot4OfReel7Cart:
                    case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot5OfReel7Cart:
                    case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot6OfReel7Cart:
                    case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot1OfReel13Cart:
                    case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot2OfReel13Cart:
                    case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot3OfReel13Cart:
                    case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot4OfReel13Cart:
                    case RobotSequenceCommands.MoveToReel7OfReturnStage:
                    case RobotSequenceCommands.MoveToReel13OfReturnStage:
                    case RobotSequenceCommands.ApproachToReelHeightCheckPointAtReel7OfReturnStage:
                    case RobotSequenceCommands.ApproachToReelHeightCheckPointAtReel13OfReturnStage:
                    case RobotSequenceCommands.MeasureReelHeightAtReel7OfReturnStage:
                    case RobotSequenceCommands.MeasureReelHeightAtReel13OfReturnStage:
                    case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromReel13OfReturnStage:
                    case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromReel7OfReturnStage:
                    case RobotSequenceCommands.MoveBackToFrontOfReturnStage:
                    case RobotSequenceCommands.PutReelIntoTower1:
                    case RobotSequenceCommands.PutReelIntoTower2:
                    case RobotSequenceCommands.PutReelIntoTower3:
                    case RobotSequenceCommands.PutReelIntoTower4:
                    case RobotSequenceCommands.PutReelIntoReel7OfReturnStage:
                    case RobotSequenceCommands.PutReelIntoReel13OfReturnStage:
                    case RobotSequenceCommands.PutReelIntoOutput1:
                    case RobotSequenceCommands.PutReelIntoOutput2:
                    case RobotSequenceCommands.PutReelIntoOutput3:
                    case RobotSequenceCommands.TakeReelFromTower1:
                    case RobotSequenceCommands.TakeReelFromTower2:
                    case RobotSequenceCommands.TakeReelFromTower3:
                    case RobotSequenceCommands.TakeReelFromTower4:
                    case RobotSequenceCommands.GoToHomeAfterPickUpReel:
                    case RobotSequenceCommands.GoToHomeBeforeReelHeightCheck:
                    case RobotSequenceCommands.MoveBackToFrontOfUnloadTower1:
                    case RobotSequenceCommands.MoveBackToFrontOfUnloadTower2:
                    case RobotSequenceCommands.MoveBackToFrontOfUnloadTower3:
                    case RobotSequenceCommands.MoveBackToFrontOfUnloadTower4:
                        {   // Execute commands
                            robotActionFailure = RobotActionFailures.None;
                            commandText_ = string.IsNullOrEmpty(arg) ? $"E;{commandCode_};{delimiter}" : $"E;{commandCode_};{arg};{delimiter}";
                        }
                        break;
                }

                if (!string.IsNullOrEmpty(commandText_) && Send(commandText_, waitbit_))
                {
                    messageQueue.Enqueue(new Pair<RobotSequenceCommands, int>(lastExecutedCommand = command, App.TickCount));
                    return true;
                }
                else
                {
                    switch (command)
                    {   // Recover unload pick up failure.
                        case RobotSequenceCommands.MoveBackToFrontOfUnloadTower1:
                        case RobotSequenceCommands.MoveBackToFrontOfUnloadTower2:
                        case RobotSequenceCommands.MoveBackToFrontOfUnloadTower3:
                        case RobotSequenceCommands.MoveBackToFrontOfUnloadTower4:
                            {   // To avoid unwanted position check.
                                lastExecutedCommand = command;
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Trace($"{ClassName}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return false;
        }

        public void SendResponse(string message, bool waitbit = false)
        {
            FireUpdateRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}({sentMessageId++}:{waitResponse = false})={message}");

            foreach (var client in clients.Values)
            {
                if (client.Sock.Connected)
                    client.Send(Encoding.Default.GetBytes(message));
            }
        }

        public void RestartRobotSequenceManager(int portno)
        {
            Destroy();
            Create(portno);
        }
        #endregion

        #region Check robot position and command execution result
        // This function is checking if the robot executed last sent command or not.
        public virtual RobotSequenceCommands GetRobotExecutedCommandByScenario(ref bool finished)
        {
            RobotSequenceCommands command_ = RobotSequenceCommands.Unknown;

            if (homed == 1)
            {
                if (robotScenarios_.ContainsKey(lastExecutedCommand))
                {
                    if (robotScenarios_[lastExecutedCommand].first == previousScenario)
                    {
                        finished = (robotScenarios_[lastExecutedCommand].second < 0 || (robotScenarios_[lastExecutedCommand].second == currentWaypoint && currentWaypoint == nextWaypoint) && movingState == 0);
                        
                        switch (previousScenario)
                        {
                            case 0:
                            case 1:
                            case 2:
                            case 3:
                            case 4:
                                break;
                            case 100:
                                {
                                    switch (robotScenarios_[lastExecutedCommand].second)
                                    {
                                        case 101:
                                            finished &= (lastExecutedCommand == RobotSequenceCommands.CheckReelTypeOfCart || lastExecutedCommand == RobotSequenceCommands.ApplyReelTypeOfCart);
                                            break;
                                    }
                                }
                                break;
                            case 200:
                            case 204:
                                {
                                    switch (robotScenarios_[lastExecutedCommand].second)
                                    {   // Checking if last executed command matches our case number
                                        case 0:
                                            finished &= (lastExecutedCommand == RobotSequenceCommands.GoToHomeBeforeReelHeightCheck || lastExecutedCommand == RobotSequenceCommands.GoToHomeAfterPickUpReel);
                                            break;
                                        default:
                                            finished = false;
                                            break;
                                    }
                                }
                                break;
                            case 201:
                                {
                                    switch (robotScenarios_[lastExecutedCommand].second)
                                    {
                                        case 91101:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot1OfReel7Cart;
                                            break;
                                        case 91201:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot2OfReel7Cart;
                                            break;
                                        case 91301:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot3OfReel7Cart;
                                            break;
                                        case 91401:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot4OfReel7Cart;
                                            break;
                                        case 91501:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot5OfReel7Cart;
                                            break;
                                        case 91601:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot6OfReel7Cart;
                                            break;
                                        default:
                                            finished = false;
                                            break;
                                    }
                                }
                                break;
                            case 202:
                                {
                                    switch (robotScenarios_[lastExecutedCommand].second)
                                    {
                                        case 91103:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.MeasureReelHeightAtWorkSlot1OfReel7Cart;
                                            break;
                                        case 91203:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.MeasureReelHeightAtWorkSlot2OfReel7Cart;
                                            break;
                                        case 91303:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.MeasureReelHeightAtWorkSlot3OfReel7Cart;
                                            break;
                                        case 91403:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.MeasureReelHeightAtWorkSlot4OfReel7Cart;
                                            break;
                                        case 91503:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.MeasureReelHeightAtWorkSlot5OfReel7Cart;
                                            break;
                                        case 91603:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.MeasureReelHeightAtWorkSlot6OfReel7Cart;
                                            break;
                                        default:
                                            finished = false;
                                            break;
                                    }
                                }
                                break;
                            case 203:
                                {
                                    switch (robotScenarios_[lastExecutedCommand].second)
                                    {
                                        case 91108:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot1OfReel7Cart;                                            
                                            break;
                                        case 91208:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot2OfReel7Cart;
                                            break;
                                        case 91308:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot3OfReel7Cart;
                                            break;
                                        case 91408:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot4OfReel7Cart;
                                            break;
                                        case 91508:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot5OfReel7Cart;
                                            break;
                                        case 91608:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot6OfReel7Cart;
                                            break;
                                        default:
                                            finished = false;
                                            break;
                                    }
                                }
                                break;
                            case 211:
                                {
                                    switch (robotScenarios_[lastExecutedCommand].second)
                                    {
                                        case 92101:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot1OfReel13Cart;
                                            break;
                                        case 92201:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot2OfReel13Cart;
                                            break;
                                        case 92301:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot3OfReel13Cart;
                                            break;
                                        case 92401:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot4OfReel13Cart;
                                            break;
                                        default:
                                            finished = false;
                                            break;
                                    }
                                }
                                break;
                            case 212:
                                {
                                    switch (robotScenarios_[lastExecutedCommand].second)
                                    {
                                        case 92103:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.MeasureReelHeightAtWorkSlot1OfReel13Cart;
                                            break;
                                        case 92203:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.MeasureReelHeightAtWorkSlot2OfReel13Cart;
                                            break;
                                        case 92303:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.MeasureReelHeightAtWorkSlot3OfReel13Cart;
                                            break;
                                        case 92403:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.MeasureReelHeightAtWorkSlot4OfReel13Cart;
                                            break;
                                        default:
                                            finished = false;
                                            break;
                                    }
                                }
                                break;
                            case 213:
                                {
                                    switch (robotScenarios_[lastExecutedCommand].second)
                                    {
                                        case 92107:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot1OfReel13Cart;
                                            break;
                                        case 92207:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot2OfReel13Cart;
                                            break;
                                        case 92307:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot3OfReel13Cart;
                                            break;
                                        case 92407:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot4OfReel13Cart;
                                            break;
                                        default:
                                            finished = false;
                                            break;
                                    }
                                }
                                break;
                            case 220:
                                {
                                    switch (robotScenarios_[lastExecutedCommand].second)
                                    {
                                        case 81000:
                                        case 82000:
                                            finished |= (lastExecutedCommand == RobotSequenceCommands.MoveToReel7OfReturnStage || lastExecutedCommand == RobotSequenceCommands.MoveToReel13OfReturnStage);
                                            break;
                                        default:
                                            finished = false;
                                            break;
                                    }
                                }
                                break;
                            case 221:
                                {
                                    switch (robotScenarios_[lastExecutedCommand].second)
                                    {
                                        case 81001:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.ApproachToReelHeightCheckPointAtReel7OfReturnStage;
                                            break;
                                        case 82001:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.ApproachToReelHeightCheckPointAtReel13OfReturnStage;
                                            break;
                                        default:
                                            finished = false;
                                            break;
                                    }
                                }
                                break;
                            case 222:
                                {
                                    switch (robotScenarios_[lastExecutedCommand].second)
                                    {
                                        case 81002:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.MeasureReelHeightAtReel7OfReturnStage;
                                            break;
                                        case 82002:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.MeasureReelHeightAtReel13OfReturnStage;
                                            break;
                                        default:
                                            finished = false;
                                            break;
                                    }
                                }
                                break;
                            case 223:
                                {
                                    switch (robotScenarios_[lastExecutedCommand].second)
                                    {
                                        case 81000:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromReel7OfReturnStage;
                                            break;
                                        case 82000:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromReel13OfReturnStage;
                                            break;
                                        default:
                                            finished = false;
                                            break;
                                    }
                                }
                                break;
                            case 301:
                                {
                                    switch (robotScenarios_[lastExecutedCommand].second)
                                    {
                                        case 10000:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.PutReelIntoTower1;
                                            break;
                                        default:
                                            finished = false;
                                            break;
                                    }
                                }
                                break;
                            case 302:
                                {
                                    switch (robotScenarios_[lastExecutedCommand].second)
                                    {
                                        case 20000:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.PutReelIntoTower2;
                                            break;
                                        default:
                                            finished = false;
                                            break;
                                    }
                                }
                                break;
                            case 303:
                                {
                                    switch (robotScenarios_[lastExecutedCommand].second)
                                    {
                                        case 30000:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.PutReelIntoTower3;
                                            break;
                                        default:
                                            finished = false;
                                            break;
                                    }
                                }
                                break;
                            case 304:
                                {
                                    switch (robotScenarios_[lastExecutedCommand].second)
                                    {
                                        case 40000:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.PutReelIntoTower4;
                                            break;
                                        default:
                                            finished = false;
                                            break;
                                    }
                                }
                                break;
                            case 401:
                            case 402:
                                break;
                            // Way point ambiguity     
                            // Put Reel into Unload Output Stage 1
                            case 501:
                                {
                                    switch (robotScenarios_[lastExecutedCommand].second)
                                    {
                                        case 50000:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.PutReelIntoOutput1;
                                            break;
                                        default:
                                            finished = false;
                                            break;
                                    }
                                }
                                break;
                            // Put Reel into Unload Output Stage 2
                            case 502:
                                {
                                    switch (robotScenarios_[lastExecutedCommand].second)
                                    {
                                        case 60000:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.PutReelIntoOutput2;
                                            break;
                                        default:
                                            finished = false;
                                            break;
                                    }
                                }
                                break;
                            // Put Reel into Unload Output Stage 3
                            case 503:
                                {
                                    switch (robotScenarios_[lastExecutedCommand].second)
                                    {
                                        case 70000:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.PutReelIntoOutput3;
                                            break;
                                        default:
                                            finished = false;
                                            break;
                                    }
                                }
                                break;
                            // Take out reel from tower 1
                            case 601:
                                {
                                    switch (robotScenarios_[lastExecutedCommand].second)
                                    {
                                        case 10000:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.TakeReelFromTower1;
                                            break;
                                        default:
                                            finished = false;
                                            break;
                                    }
                                }
                                break;
                            // Take out reel from tower 2
                            case 602:
                                {
                                    switch (robotScenarios_[lastExecutedCommand].second)
                                    {
                                        case 20000:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.TakeReelFromTower2;
                                            break;
                                        default:
                                            finished = false;
                                            break;
                                    }
                                }
                                break;
                            // Take out reel from tower 3
                            case 603:
                                {
                                    switch (robotScenarios_[lastExecutedCommand].second)
                                    {
                                        case 30000:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.TakeReelFromTower3;
                                            break;
                                        default:
                                            finished = false;
                                            break;
                                    }
                                }
                                break;
                            // Take out reel from tower 4
                            case 604:
                                {
                                    switch (robotScenarios_[lastExecutedCommand].second)
                                    {
                                        case 40000:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.TakeReelFromTower4;
                                            break;
                                        default:
                                            finished = false;
                                            break;
                                    }
                                }
                                break;
                            // Move back to unload tower 1
                            case 701:
                                {
                                    switch (robotScenarios_[lastExecutedCommand].second)
                                    {
                                        case 10000:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.MoveBackToFrontOfUnloadTower1;
                                            break;
                                        default:
                                            finished = false;
                                            break;
                                    }
                                }
                                break;
                            // Move back to unload tower 2
                            case 702:
                                {
                                    switch (robotScenarios_[lastExecutedCommand].second)
                                    {
                                        case 20000:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.MoveBackToFrontOfUnloadTower2;
                                            break;
                                        default:
                                            finished = false;
                                            break;
                                    }
                                }
                                break;
                            // Move back to unload tower 3
                            case 703:
                                {
                                    switch (robotScenarios_[lastExecutedCommand].second)
                                    {
                                        case 30000:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.MoveBackToFrontOfUnloadTower3;
                                            break;
                                        default:
                                            finished = false;
                                            break;
                                    }
                                }
                                break;
                            // Move back to unload tower 4
                            case 704:
                                {
                                    switch (robotScenarios_[lastExecutedCommand].second)
                                    {
                                        case 40000:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.MoveBackToFrontOfUnloadTower4;
                                            break;
                                        default:
                                            finished = false;
                                            break;
                                    }
                                }
                                break;
                            // Move to front of Load Tower 1
                            case 1001:
                                {
                                    switch (robotScenarios_[lastExecutedCommand].second)
                                    {
                                        case 10000:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.MoveToLoadFrontOfTower1;
                                            break;
                                        default:
                                            finished = false;
                                            break;
                                    }
                                }
                                break;
                            // Move to front of Load Tower 2 
                            case 1002:
                                {
                                    switch (robotScenarios_[lastExecutedCommand].second)
                                    {
                                        case 20000:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.MoveToLoadFrontOfTower2;
                                            break;
                                        default:
                                            finished = false;
                                            break;
                                    }
                                }
                                break;
                            // Move to front of Load Tower 3
                            case 1003:
                                {
                                    switch (robotScenarios_[lastExecutedCommand].second)
                                    {
                                        case 30000:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.MoveToLoadFrontOfTower3;
                                            break;
                                        default:
                                            finished = false;
                                            break;
                                    }
                                }
                                break;
                            // Move to front of Load Tower 4
                            case 1004:
                                {
                                    switch (robotScenarios_[lastExecutedCommand].second)
                                    {
                                        case 40000:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.MoveToLoadFrontOfTower4;
                                            break;
                                        default:
                                            finished = false;
                                            break;
                                    }
                                }
                                break;
                            case 1005:
                            case 1006:
                                break;
                            // Move to front of Unload Tower 1
                            case 1101:
                                {
                                    switch (robotScenarios_[lastExecutedCommand].second)
                                    {
                                        case 10000:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.MoveToUnloadFrontOfTower1;
                                            break;
                                        default:
                                            finished = false;
                                            break;
                                    }
                                }
                                break;
                            // Move to front of Unload Tower 2 
                            case 1102:
                                {
                                    switch (robotScenarios_[lastExecutedCommand].second)
                                    {
                                        case 20000:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.MoveToUnloadFrontOfTower2;
                                            break;
                                        default:
                                            finished = false;
                                            break;
                                    }
                                }
                                break;
                            // Move to front of Unload Tower 3
                            case 1103:
                                {
                                    switch (robotScenarios_[lastExecutedCommand].second)
                                    {
                                        case 30000:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.MoveToUnloadFrontOfTower3;
                                            break;
                                        default:
                                            finished = false;
                                            break;
                                    }
                                }
                                break;
                            // Move to front of Unload Tower 4
                            case 1104:
                                {
                                    switch (robotScenarios_[lastExecutedCommand].second)
                                    {
                                        case 40000:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.MoveToUnloadFrontOfTower4;
                                            break;
                                        default:
                                            finished = false;
                                            break;
                                    }
                                }
                                break;
                            case 1105:
                            case 1106:
                                break;
                            // Move to front of Unload output stage 1
                            case 1107:
                                {
                                    switch (robotScenarios_[lastExecutedCommand].second)
                                    {
                                        case 50000:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.MoveToUnloadFrontOfOutput1;
                                            break;
                                        default:
                                            finished = false;
                                            break;
                                    }
                                }
                                break;

                            // Move to front of Unload output stage 2
                            case 1108:
                                {
                                    switch (robotScenarios_[lastExecutedCommand].second)
                                    {
                                        case 60000:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.MoveToUnloadFrontOfOutput2;
                                            break;
                                        default:
                                            finished = false;
                                            break;
                                    }
                                }
                                break;

                            // Move to front of Unload output stage 3
                            case 1109:
                                {
                                    switch (robotScenarios_[lastExecutedCommand].second)
                                    {
                                        case 70000:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.MoveToUnloadFrontOfOutput3;
                                            break;
                                        default:
                                            finished = false;
                                            break;
                                    }
                                }
                                break;

                            case 1110:
                                {
                                    switch (robotScenarios_[lastExecutedCommand].second)
                                    {
                                        case 50001:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.ApproachToUnloadFrontOfOutput1;
                                            break;
                                        default:
                                            finished = false;
                                            break;
                                    }
                                }
                                break;

                            case 1111:
                                {
                                    switch (robotScenarios_[lastExecutedCommand].second)
                                    {
                                        case 60001:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.ApproachToUnloadFrontOfOutput2;
                                            break;
                                        default:
                                            finished = false;
                                            break;
                                    }
                                }
                                break;

                            case 1112:
                                {
                                    switch (robotScenarios_[lastExecutedCommand].second)
                                    {
                                        case 70001:
                                            finished &= lastExecutedCommand == RobotSequenceCommands.ApproachToUnloadFrontOfOutput3;
                                            break;
                                        default:
                                            finished = false;
                                            break;
                                    }
                                }
                                break;

                        }

                        if (finished)
                            command_ = lastExecutedCommand;
                    }
                    else
                    {
                        switch (robotScenarios_[lastExecutedCommand].first)
                        {
                            case 0:
                                {
                                    switch (robotScenarios_[lastExecutedCommand].second)
                                    {
                                        case 0:
                                            {
                                                if (finished |= (lastExecutedCommand == RobotSequenceCommands.MoveToHome || lastExecutedCommand == RobotSequenceCommands.GoToHomeBeforeReelHeightCheck || lastExecutedCommand == RobotSequenceCommands.GoToHomeAfterPickUpReel))
                                                    command_ = RobotSequenceCommands.MoveToHome;
                                            }
                                            break;
                                        case 200:
                                        case 204:
                                            finished &= (lastExecutedCommand == RobotSequenceCommands.GoToHomeBeforeReelHeightCheck || lastExecutedCommand == RobotSequenceCommands.GoToHomeAfterPickUpReel);
                                            break;
                                        default:
                                            finished = false;
                                            break;
                                    }
                                }
                                break;
                            case 200:
                            case 204:
                                {
                                    switch (robotScenarios_[lastExecutedCommand].second)
                                    {   // Checking if last executed command matches our case number
                                        case 0:
                                            finished &= (lastExecutedCommand == RobotSequenceCommands.GoToHomeBeforeReelHeightCheck || lastExecutedCommand == RobotSequenceCommands.GoToHomeAfterPickUpReel);
                                            break;
                                        default:
                                            finished = false;
                                            break;
                                    }
                                }
                                break;
                            default:
                                finished = false;
                                break;
                        }
                    }
                }
            }

            return command_;
        }

        public virtual bool IsRobotAtSafeWayPoint()
        {
            bool result_ = IsPossibleStop;
            int waypoint_ = -1;

            if (robotScenarios_.ContainsKey(lastExecutedCommand))
            {
                if ((waypoint_ = robotScenarios_[lastExecutedCommand].second) < 0)
                {
                    result_ &= (movingState == 0 && homed == 1 && !failure);
                }
                else
                {
                    switch (lastExecutedCommand)
                    {
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot1OfReel7Cart:
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot2OfReel7Cart:
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot3OfReel7Cart:
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot4OfReel7Cart:
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot5OfReel7Cart:
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot6OfReel7Cart:
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot1OfReel13Cart:
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot2OfReel13Cart:
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot3OfReel13Cart:
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot4OfReel13Cart:
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromReel7OfReturnStage:
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromReel13OfReturnStage:
                        case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot1OfReel7Cart:
                        case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot2OfReel7Cart:
                        case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot3OfReel7Cart:
                        case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot4OfReel7Cart:
                        case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot5OfReel7Cart:
                        case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot6OfReel7Cart:
                        case RobotSequenceCommands.ApproachToUnloadFrontOfOutput1:
                        case RobotSequenceCommands.ApproachToUnloadFrontOfOutput2:
                        case RobotSequenceCommands.ApproachToUnloadFrontOfOutput3:
                            {
                                result_ = false;

                                if (result_)
                                    robotActionFailure = (reelGrip && reelDetectSensor ? RobotActionFailures.None : RobotActionFailures.PickupFailureToLoadReel);
                            }
                            break;
                        case RobotSequenceCommands.TakeReelFromTower1:
                        case RobotSequenceCommands.TakeReelFromTower2:
                        case RobotSequenceCommands.TakeReelFromTower3:
                        case RobotSequenceCommands.TakeReelFromTower4:
                            {
                                result_ = false;

                                if (result_)
                                    robotActionFailure = (reelGrip && reelDetectSensor ? RobotActionFailures.None : RobotActionFailures.PickupFailureToUnloadReel);
                            }
                            break;
                        default:
                            result_ &= ((waypoint_ % 100) == 0);
                            break;
                    }
                }
            }

            return result_;
        }

        public virtual bool IsRobotAtWayPointByCommand(ref bool safepoint)
        {
            bool result_ = false;
            int waypoint_ = -1;

            if (robotScenarios_.ContainsKey(lastExecutedCommand))
            {
                if ((waypoint_ = robotScenarios_[lastExecutedCommand].second) < 0)
                {
                    result_ = (movingState == 0 && homed == 1 && !failure);
                    safepoint = ((currentWaypoint % 100) == 0) && currentWaypoint == nextWaypoint && result_;
                }
                else
                {
                    if (waypoint_ == currentWaypoint && waypoint_ == nextWaypoint &&
                        movingState == 0 && homed == 1 && !failure)
                        result_ = true;

                    switch (lastExecutedCommand)
                    {
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot1OfReel7Cart:
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot2OfReel7Cart:
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot3OfReel7Cart:
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot4OfReel7Cart:
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot5OfReel7Cart:
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot6OfReel7Cart:
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot1OfReel13Cart:
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot2OfReel13Cart:
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot3OfReel13Cart:
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot4OfReel13Cart:
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromReel7OfReturnStage:
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromReel13OfReturnStage:
                        case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot1OfReel7Cart:
                        case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot2OfReel7Cart:
                        case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot3OfReel7Cart:
                        case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot4OfReel7Cart:
                        case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot5OfReel7Cart:
                        case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot6OfReel7Cart:
                        case RobotSequenceCommands.ApproachToUnloadFrontOfOutput1:
                        case RobotSequenceCommands.ApproachToUnloadFrontOfOutput2:
                        case RobotSequenceCommands.ApproachToUnloadFrontOfOutput3:
                            {
                                safepoint = result_;

                                if (result_)
                                    robotActionFailure = (reelGrip && reelDetectSensor ? RobotActionFailures.None : RobotActionFailures.PickupFailureToLoadReel);
                            }
                            break;
                        default:
                            safepoint = ((waypoint_ % 100) == 0);
                            break;
                    }
                }
            }

            return result_;
        }

        public virtual bool IsRobotAtWayPointByCommand(RobotSequenceCommands command, ref bool safepoint)
        {
            bool result_ = false;
            int waypoint_ = -1;

            if (robotScenarios_.ContainsKey(command))
            {
                if ((waypoint_ = robotScenarios_[command].second) < 0)
                {
                    result_ = (movingState == 0 && homed == 1 && !failure);
                    safepoint = ((currentWaypoint % 100) == 0) && currentWaypoint == nextWaypoint && result_;
                }
                else
                {
                    if (waypoint_ == currentWaypoint && waypoint_ == nextWaypoint &&
                        movingState == 0 && homed == 1 && receivedFlag == 1 && !failure)
                        result_ = true;

                    switch (lastExecutedCommand)
                    {
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot1OfReel7Cart:
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot2OfReel7Cart:
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot3OfReel7Cart:
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot4OfReel7Cart:
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot5OfReel7Cart:
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot6OfReel7Cart:
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot1OfReel13Cart:
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot2OfReel13Cart:
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot3OfReel13Cart:
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot4OfReel13Cart:
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromReel7OfReturnStage:
                        case RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromReel13OfReturnStage:
                        case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot1OfReel7Cart:
                        case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot2OfReel7Cart:
                        case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot3OfReel7Cart:
                        case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot4OfReel7Cart:
                        case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot5OfReel7Cart:
                        case RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot6OfReel7Cart:
                        case RobotSequenceCommands.ApproachToUnloadFrontOfOutput1:
                        case RobotSequenceCommands.ApproachToUnloadFrontOfOutput2:
                        case RobotSequenceCommands.ApproachToUnloadFrontOfOutput3:
                            {
                                safepoint = result_;

                                if (result_)
                                    robotActionFailure = (reelGrip && reelDetectSensor ? RobotActionFailures.None : RobotActionFailures.PickupFailureToLoadReel);
                            }
                            break;
                        case RobotSequenceCommands.TakeReelFromTower1:
                        case RobotSequenceCommands.TakeReelFromTower2:
                        case RobotSequenceCommands.TakeReelFromTower3:
                        case RobotSequenceCommands.TakeReelFromTower4:
                            {
                                safepoint = result_;

                                if (result_)
                                    robotActionFailure = (reelGrip && reelDetectSensor ? RobotActionFailures.None : RobotActionFailures.PickupFailureToUnloadReel);
                            }
                            break;
                        default:
                            safepoint = ((waypoint_ % 100) == 0);
                            break;
                    }
                }
            }

            // if (robotActionOrder == RobotActionOrder.UnloadReelFromTower)
            //     Debug.WriteLine($"Debug> {GetType().Name}.{MethodBase.GetCurrentMethod().Name}: {command}/{safepoint}");
            return result_;
        }

        public virtual bool IsRobotAtWorkSlotOfCart()
        {
            if (reelSizeOfCart != ReelTypes.Unknown && workSlot > 0 && workSlot <= 6)
                return IsRobotAtWorkSlotOfCart(reelSizeOfCart, workSlot);

            return false;
        }

        public virtual bool IsRobotAtWorkSlotOfCart(ReelTypes reeltype, int workslot)
        {
            bool result_ = false;
            bool safeposition_ = false;
            RobotSequenceCommands command_ = RobotSequenceCommands.Unknown;
            switch (reeltype)
            {
                case ReelTypes.ReelDiameter7:
                    {
                        switch (workslot)
                        {
                            case 1:
                                command_ = RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot1OfReel7Cart;
                                break;
                            case 2:
                                command_ = RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot2OfReel7Cart;
                                break;
                            case 3:
                                command_ = RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot3OfReel7Cart;
                                break;
                            case 4:
                                command_ = RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot4OfReel7Cart;
                                break;
                            case 5:
                                command_ = RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot5OfReel7Cart;
                                break;
                            case 6:
                                command_ = RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot6OfReel7Cart;
                                break;
                            default:
                                return result_;
                        }
                    }
                    break;
                case ReelTypes.ReelDiameter13:
                    {
                        switch (workslot)
                        {
                            case 1:
                                command_ = RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot1OfReel13Cart;
                                break;
                            case 2:
                                command_ = RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot2OfReel13Cart;
                                break;
                            case 3:
                                command_ = RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot3OfReel13Cart;
                                break;
                            case 4:
                                command_ = RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot4OfReel13Cart;
                                break;
                            default:
                                return result_;
                        }
                    }
                    break;
            }

            return (result_ = IsRobotAtWayPointByCommand(command_, ref safeposition_));
        }

        public virtual bool MoveToWorkSlotOfCart(ReelTypes reeltype, int workslot)
        {
            bool result_ = false;
            RobotSequenceCommands command_ = RobotSequenceCommands.Unknown;
            switch (reeltype)
            {
                case ReelTypes.ReelDiameter7:
                    {
                        switch (workslot)
                        {
                            case 1:
                                command_ = RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot1OfReel7Cart;
                                break;
                            case 2:
                                command_ = RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot2OfReel7Cart;
                                break;
                            case 3:
                                command_ = RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot3OfReel7Cart;
                                break;
                            case 4:
                                command_ = RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot4OfReel7Cart;
                                break;
                            case 5:
                                command_ = RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot5OfReel7Cart;
                                break;
                            case 6:
                                command_ = RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot6OfReel7Cart;
                                break;
                            default:
                                return result_;
                        }
                    }
                    break;
                case ReelTypes.ReelDiameter13:
                    {
                        switch (workslot)
                        {
                            case 1:
                                command_ = RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot1OfReel13Cart;
                                break;
                            case 2:
                                command_ = RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot2OfReel13Cart;
                                break;
                            case 3:
                                command_ = RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot3OfReel13Cart;
                                break;
                            case 4:
                                command_ = RobotSequenceCommands.MoveToReelHeightCheckPointAtWorkSlot4OfReel13Cart;
                                break;
                            default:
                                return result_;
                        }
                    }
                    break;
            }

            return (result_ = SendCommand(command_, $"{workslot}"));
        }

        public virtual bool MeasureReelHeightOfWorkSlotOfCart(ReelTypes reeltype, int workslot)
        {
            bool result_ = false;
            RobotSequenceCommands command_ = RobotSequenceCommands.Unknown;
            switch (reeltype)
            {
                case ReelTypes.ReelDiameter7:
                    {
                        switch (workslot)
                        {
                            case 1:
                                command_ = RobotSequenceCommands.MeasureReelHeightAtWorkSlot1OfReel7Cart;
                                break;
                            case 2:
                                command_ = RobotSequenceCommands.MeasureReelHeightAtWorkSlot2OfReel7Cart;
                                break;
                            case 3:
                                command_ = RobotSequenceCommands.MeasureReelHeightAtWorkSlot3OfReel7Cart;
                                break;
                            case 4:
                                command_ = RobotSequenceCommands.MeasureReelHeightAtWorkSlot4OfReel7Cart;
                                break;
                            case 5:
                                command_ = RobotSequenceCommands.MeasureReelHeightAtWorkSlot5OfReel7Cart;
                                break;
                            case 6:
                                command_ = RobotSequenceCommands.MeasureReelHeightAtWorkSlot6OfReel7Cart;
                                break;
                            default:
                                return result_;
                        }
                    }
                    break;
                case ReelTypes.ReelDiameter13:
                    {
                        switch (workslot)
                        {
                            case 1:
                                command_ = RobotSequenceCommands.MeasureReelHeightAtWorkSlot1OfReel13Cart;
                                break;
                            case 2:
                                command_ = RobotSequenceCommands.MeasureReelHeightAtWorkSlot2OfReel13Cart;
                                break;
                            case 3:
                                command_ = RobotSequenceCommands.MeasureReelHeightAtWorkSlot3OfReel13Cart;
                                break;
                            case 4:
                                command_ = RobotSequenceCommands.MeasureReelHeightAtWorkSlot4OfReel13Cart;
                                break;
                            default:
                                return result_;
                        }
                    }
                    break;
            }

            return (result_ = SendCommand(command_, $"{workslot}"));
        }

        public virtual bool PickupReelFromCart(ReelTypes reeltype, int workslot, string offsetx, string offsety)
        {
            bool result_ = false;
            RobotSequenceCommands command_ = RobotSequenceCommands.Unknown;

            switch (reeltype)
            {
                case ReelTypes.ReelDiameter7:
                    {
                        switch (workslot)
                        {
                            case 1:
                                command_ = RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot1OfReel7Cart;
                                break;
                            case 2:
                                command_ = RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot2OfReel7Cart;
                                break;
                            case 3:
                                command_ = RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot3OfReel7Cart;
                                break;
                            case 4:
                                command_ = RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot4OfReel7Cart;
                                break;
                            case 5:
                                command_ = RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot5OfReel7Cart;
                                break;
                            case 6:
                                command_ = RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot6OfReel7Cart;
                                break;
                            default:
                                return result_;
                        }
                    }
                    break;
                case ReelTypes.ReelDiameter13:
                    {
                        switch (workslot)
                        {
                            case 1:
                                command_ = RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot1OfReel13Cart;
                                break;
                            case 2:
                                command_ = RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot2OfReel13Cart;
                                break;
                            case 3:
                                command_ = RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot3OfReel13Cart;
                                break;
                            case 4:
                                command_ = RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromWorkSlot4OfReel13Cart;
                                break;
                            default:
                                return result_;
                        }
                    }
                    break;
            }
            return (result_ = SendCommand(command_, $"{offsetx};{offsety}"));
        }

        public virtual bool MoveRobotToFrontOfUnloadTower(int tower)
        {
            bool result_ = false;
            RobotSequenceCommands command_ = RobotSequenceCommands.Unknown;
            switch (tower)
            {
                case 1:
                    command_ = RobotSequenceCommands.MoveToUnloadFrontOfTower1;
                    break;
                case 2:
                    command_ = RobotSequenceCommands.MoveToUnloadFrontOfTower2;
                    break;
                case 3:
                    command_ = RobotSequenceCommands.MoveToUnloadFrontOfTower3;
                    break;
                case 4:
                    command_ = RobotSequenceCommands.MoveToUnloadFrontOfTower4;
                    break;
                default:
                    return result_;
            }
            return (result_ = SendCommand(command_));
        }

        public virtual bool MoveRobotToFrontOfUnloadOutputStage()
        {
            bool result_ = false;
            RobotSequenceCommands command_ = RobotSequenceCommands.Unknown;
            command_ = RobotSequenceCommands.MoveToUnloadFrontOfOutput1;     // stage 1 2,3 approach point is same
            return (result_ = SendCommand(command_));
        }

        public virtual bool PutReelIntoOutputStage(int stagenumber)
        {
            bool result_ = false;
            RobotSequenceCommands command_ = RobotSequenceCommands.Unknown;
            switch (stagenumber)
            {
                case 1:
                    command_ = RobotSequenceCommands.PutReelIntoOutput1;
                    break;
                case 2:
                    command_ = RobotSequenceCommands.PutReelIntoOutput2;
                    break;
                case 3:
                    command_ = RobotSequenceCommands.PutReelIntoOutput3;
                    break;
                default:
                    return result_;
            }
            return (result_ = SendCommand(command_));
        }

        public virtual bool ApproachOutputStage(int stagenumber)
        {
            bool result_ = false;
            RobotSequenceCommands command_ = RobotSequenceCommands.Unknown;
            switch (stagenumber)
            {
                case 1:
                    command_ = RobotSequenceCommands.ApproachToUnloadFrontOfOutput1;
                    break;
                case 2:
                    command_ = RobotSequenceCommands.ApproachToUnloadFrontOfOutput2;
                    break;
                case 3:
                    command_ = RobotSequenceCommands.ApproachToUnloadFrontOfOutput3;
                    break;
                default:
                    return result_;
            }
            return (result_ = SendCommand(command_));
        }

        public virtual bool MoveRobotToFrontOfTower(int tower)
        {
            bool result_ = false;
            RobotSequenceCommands command_ = RobotSequenceCommands.Unknown;
            switch (tower)
            {
                case 1:
                    command_ = RobotSequenceCommands.MoveToLoadFrontOfTower1;
                    break;
                case 2:
                    command_ = RobotSequenceCommands.MoveToLoadFrontOfTower2;
                    break;
                case 3:
                    command_ = RobotSequenceCommands.MoveToLoadFrontOfTower3;
                    break;
                case 4:
                    command_ = RobotSequenceCommands.MoveToLoadFrontOfTower4;
                    break;
                default:
                    return result_;
            }
            return (result_ = SendCommand(command_));
        }

        public virtual bool MoveBackToFrontOfUnloadTower(int tower)
        {
            bool result_ = false;
            RobotSequenceCommands command_ = RobotSequenceCommands.Unknown;
            switch (tower)
            {
                case 1:
                    command_ = RobotSequenceCommands.MoveBackToFrontOfUnloadTower1;
                    break;
                case 2:
                    command_ = RobotSequenceCommands.MoveBackToFrontOfUnloadTower2;
                    break;
                case 3:
                    command_ = RobotSequenceCommands.MoveBackToFrontOfUnloadTower3;
                    break;
                case 4:
                    command_ = RobotSequenceCommands.MoveBackToFrontOfUnloadTower4;
                    break;
                default:
                    return result_;
            }
            return (result_ = SendCommand(command_));
        }

        public virtual bool PutReelIntoTowerPort(int tower)
        {
            bool result_ = false;
            RobotSequenceCommands command_ = RobotSequenceCommands.Unknown;
            switch (tower)
            {
                case 1:
                    command_ = RobotSequenceCommands.PutReelIntoTower1;
                    break;
                case 2:
                    command_ = RobotSequenceCommands.PutReelIntoTower2;
                    break;
                case 3:
                    command_ = RobotSequenceCommands.PutReelIntoTower3;
                    break;
                case 4:
                    command_ = RobotSequenceCommands.PutReelIntoTower4;
                    break;
                default:
                    return result_;
            }
            return (result_ = SendCommand(command_));
        }

        public virtual bool TakeReelFromTowerPort(int tower)
        {
            bool result_ = false;
            RobotSequenceCommands command_ = RobotSequenceCommands.Unknown;
            switch (tower)
            {
                case 1:
                    command_ = RobotSequenceCommands.TakeReelFromTower1;
                    break;
                case 2:
                    command_ = RobotSequenceCommands.TakeReelFromTower2;
                    break;
                case 3:
                    command_ = RobotSequenceCommands.TakeReelFromTower3;
                    break;
                case 4:
                    command_ = RobotSequenceCommands.TakeReelFromTower4;
                    break;
                default:
                    return result_;
            }
            return (result_ = SendCommand(command_));
        }

        public virtual bool IsRobotAtFrontOfReturnStage(ReelTypes reeltype)
        {
            bool result_ = false;
            bool safeposition_ = false;
            RobotSequenceCommands command_ = RobotSequenceCommands.Unknown;
            switch (reeltype)
            {
                case ReelTypes.ReelDiameter7:
                    {
                        command_ = RobotSequenceCommands.MoveToReel7OfReturnStage;
                    }
                    break;
                case ReelTypes.ReelDiameter13:
                    {
                        command_ = RobotSequenceCommands.MoveToReel13OfReturnStage;
                    }
                    break;
                default:
                    {
                        switch (reelSizeOfReturn)
                        {
                            case ReelTypes.ReelDiameter7:
                                {
                                    command_ = RobotSequenceCommands.MoveToReel7OfReturnStage;
                                }
                                break;
                            case ReelTypes.ReelDiameter13:
                                {
                                    command_ = RobotSequenceCommands.MoveToReel13OfReturnStage;
                                }
                                break;
                            default:
                                return false;
                        }
                    }
                    break;
            }

            return (result_ = IsRobotAtWayPointByCommand(command_, ref safeposition_));
        }

        public virtual bool IsRobotAtMeasureReelHeightPositionOfReturnStage(ReelTypes reeltype)
        {
            bool result_ = false;
            bool safeposition_ = false;
            RobotSequenceCommands command_ = RobotSequenceCommands.Unknown;
            switch (reeltype)
            {
                case ReelTypes.ReelDiameter7:
                    {
                        command_ = RobotSequenceCommands.ApproachToReelHeightCheckPointAtReel7OfReturnStage;
                    }
                    break;
                case ReelTypes.ReelDiameter13:
                    {
                        command_ = RobotSequenceCommands.ApproachToReelHeightCheckPointAtReel13OfReturnStage;
                    }
                    break;
                default:
                    {
                        switch (reelSizeOfReturn)
                        {
                            case ReelTypes.ReelDiameter7:
                                {
                                    command_ = RobotSequenceCommands.ApproachToReelHeightCheckPointAtReel7OfReturnStage;
                                }
                                break;
                            case ReelTypes.ReelDiameter13:
                                {
                                    command_ = RobotSequenceCommands.ApproachToReelHeightCheckPointAtReel13OfReturnStage;
                                }
                                break;
                            default:
                                return false;
                        }
                    }
                    break;
            }

            return (result_ = IsRobotAtWayPointByCommand(command_, ref safeposition_));
        }

        public virtual bool MoveToFrontOfReturnStage(ReelTypes reeltype)
        {
            bool result_ = false;
            RobotSequenceCommands command_ = RobotSequenceCommands.Unknown;
            switch (reeltype)
            {
                case ReelTypes.ReelDiameter7:
                    command_ = RobotSequenceCommands.MoveToReel7OfReturnStage;
                    break;
                case ReelTypes.ReelDiameter13:
                    command_ = RobotSequenceCommands.MoveToReel13OfReturnStage;
                    break;
                default:
                    {
                        switch (reelSizeOfReturn)
                        {
                            case ReelTypes.ReelDiameter7:
                                {
                                    command_ = RobotSequenceCommands.MoveToReel7OfReturnStage;
                                }
                                break;
                            case ReelTypes.ReelDiameter13:
                                {
                                    command_ = RobotSequenceCommands.MoveToReel13OfReturnStage;
                                }
                                break;
                            default:
                                return false;
                        }
                    }
                    break;
            }
            return (result_ = SendCommand(command_));
        }

        public virtual bool ApproachToReturnStage(ReelTypes reeltype)
        {
            bool result_ = false;
            RobotSequenceCommands command_ = RobotSequenceCommands.Unknown;
            switch (reeltype)
            {
                case ReelTypes.ReelDiameter7:
                    command_ = RobotSequenceCommands.ApproachToReelHeightCheckPointAtReel7OfReturnStage;
                    break;
                case ReelTypes.ReelDiameter13:
                    command_ = RobotSequenceCommands.ApproachToReelHeightCheckPointAtReel13OfReturnStage;
                    break;
                case ReelTypes.Unknown:
                    command_ = RobotSequenceCommands.MoveBackToFrontOfReturnStage;
                    break;
                default:
                    return result_;
            }
            return (result_ = SendCommand(command_));
        }

        public virtual bool MeasureReelHeightOfReturnStage(ReelTypes reeltype)
        {
            bool result_ = false;
            RobotSequenceCommands command_ = RobotSequenceCommands.Unknown;
            switch (reeltype)
            {
                case ReelTypes.ReelDiameter7:
                    command_ = RobotSequenceCommands.MeasureReelHeightAtReel7OfReturnStage;
                    break;
                case ReelTypes.ReelDiameter13:
                    command_ = RobotSequenceCommands.MeasureReelHeightAtReel13OfReturnStage;
                    break;
                case ReelTypes.Unknown:
                    command_ = RobotSequenceCommands.MoveBackToFrontOfReturnStage;
                    break;
                default:
                    return result_;
            }
            return (result_ = SendCommand(command_));
        }

        public virtual bool PickupReelFromReturn(ReelTypes reeltype, string offsetx, string offsety)
        {
            bool result_ = false;
            RobotSequenceCommands command_ = RobotSequenceCommands.Unknown;

            switch (reeltype)
            {
                case ReelTypes.ReelDiameter7:
                    command_ = RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromReel7OfReturnStage;
                    break;
                case ReelTypes.ReelDiameter13:
                    command_ = RobotSequenceCommands.ApplyAlignmentAndPickupLoadReelFromReel13OfReturnStage;
                    break;
            }
            return (result_ = SendCommand(command_, $"{offsetx};{offsety}"));
        }

        public virtual void ResetExecuteCommandToHome()
        {
            lastExecutedCommand = RobotSequenceCommands.MoveToHome;
        }
        #endregion
    }
}
#endregion
