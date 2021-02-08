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
/// @file ReelTowerManager.cs
/// @brief
/// @details
/// @date 2019, 5, 23, 오후 3:14
///////////////////////////////////////////////////////////////////////////////
#endregion

#region Imports
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml;
using Marcus.Solution.TechFloor.Object;
using Marcus.Solution.TechFloor.Util;
using Marcus.Solution.TechFloor.Device.CommunicationIo;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Security;
#endregion

#region Program
namespace Marcus.Solution.TechFloor
{
    public class MaterialPackage
    {
        #region Constants
        protected readonly char[] CONST_MATERIAL_DELIMETERS = new char[] { ';' };
        #endregion

        #region Fields
        private static StringBuilder buffer_ = new StringBuilder();
        protected bool exported                     = false;
        protected int outputPort                    = 0;
        protected int materialCount                 = 0;
        protected string name                       = string.Empty;
        protected string outputPortName             = string.Empty;
        protected DateTime registeredTime           = DateTime.MinValue;
        protected DateTime finishedTime             = DateTime.MinValue;
        protected ReelUnloadReportStates pickListState = ReelUnloadReportStates.Ready;
        protected List<Pair<string, ReelUnloadReportStates>> materials = new List<Pair<string, ReelUnloadReportStates>>();
        #endregion

        #region Properties
        public bool IsExported                      => exported;
        public int OutputPort                       => outputPort;
        public int MaterialCount                    => materialCount;
        public string Name                          => name;
        public string OutputPortName                => outputPortName;
        public DateTime RegisteredTime              => registeredTime;
        public DateTime FinishedTime                => finishedTime;
        public ReelUnloadReportStates PickState
        {
            get => pickListState;
            set => pickListState = value;
        }
        public IReadOnlyList<Pair<string, ReelUnloadReportStates>> Materials      => materials;
        #endregion

        #region Constructors 
        public MaterialPackage(string id, string outport, int count, string items)
        {
            name = id;
            materialCount = count;

            if (!string.IsNullOrEmpty(outputPortName = outport))
                outputPort = Convert.ToInt32(outport[outport.Length - 1].ToString());

            string[] tokens_ = items.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string token_ in tokens_)
            {
                materials.Add(new Pair<string, ReelUnloadReportStates>(token_, ReelUnloadReportStates.Waiting));
            }

            registeredTime = DateTime.Now;
        }

       // public MaterialPackage(string id, string outport, int count, List<Pair<string, string>> items)
        public MaterialPackage(string id, string outport, int count, List<List<string>> items)
        {
            name = id;
            materialCount = count;

            if (!string.IsNullOrEmpty(outputPortName = outport))
                outputPort = Convert.ToInt32(outport[outport.Length - 1].ToString());

            foreach (var item in items)
            {
                materials.Add(new Pair<string, ReelUnloadReportStates>($"{item[0]};{item[1]};{item[2]};{item[3]};{item[4]};{item[5]};", ReelUnloadReportStates.Waiting));
            }

            registeredTime = DateTime.Now;
        }
        #endregion

        #region Public methods
        public void CopyFrom(MaterialPackage src)
        {
            outputPort = src.outputPort;
            materialCount = src.materialCount;
            name = src.name;
            outputPortName = src.OutputPortName;
            registeredTime = src.registeredTime;
            finishedTime = src.finishedTime;
            materials = src.materials;
            pickListState = src.pickListState;
        }

        public TimeSpan Elapsed()
        {
            return (finishedTime != DateTime.MinValue? finishedTime - registeredTime : DateTime.Now - registeredTime);
        }

        
        public void ExportToFile(object data)
        {
            try 
            {
                string filepath = (string)data;

                if (string.IsNullOrEmpty(filepath))
                    return;

                if (File.Exists(filepath))
                    File.Delete(filepath);

                buffer_.Clear();
                buffer_.AppendLine("[PickInfo]");
                buffer_.AppendLine($"ID={name}");
                buffer_.AppendLine($"OutPort={outputPort}");
                buffer_.AppendLine($"Qty={materialCount}");
                buffer_.AppendLine("[PickList]");

                for (int i_ = 1, j_ = 0; j_ < materials.Count; i_++, j_++)
                {
                    string[] items_ = materials[j_].first.Split(CONST_MATERIAL_DELIMETERS, StringSplitOptions.RemoveEmptyEntries);

                    if (items_.Length >= 2 )
                        buffer_.AppendLine($"{i_}={items_[0]};{items_[1]};{items_[2]};{items_[3]};{items_[4]};{items_[5]};");
                }

                using (FileStream fs = new FileStream(filepath, FileMode.CreateNew, FileAccess.Write, FileShare.Read))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.Unicode))
                    {
                        sw.Write(buffer_.ToString());
                        sw.Flush();
                        sw.Close();
                    }

                    fs.Close();
                }

                exported = true;
            }
            catch (Exception ex)
            {
                Logger.Trace($"{this.GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        
        public static void ExportDataToFile(string filepath, string data)
        {
            try
            {
                if (File.Exists(filepath))
                    File.Delete(filepath);

                buffer_.Clear();
                buffer_.AppendLine("[Workinfo]");  
                buffer_.AppendLine($"Work={data}");

                using (FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.Unicode))
                    {
                        sw.WriteAsync(buffer_.ToString());
                        sw.Flush();
                        sw.Close();
                    }

                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.Trace($"ExportDataToFile.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        
        public static void ExportStateToFile(string filepath, ReelUnloadReportStates state)
        {
            try
            {
                if (File.Exists(filepath))
                    File.Delete(filepath);

                buffer_.Clear();
                buffer_.AppendLine("[UR]");
                buffer_.AppendLine($"Status={Convert.ToInt32(state)}");

                using (FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.Unicode))
                    {
                        Debug.WriteLine($"{DateTime.Now.ToString("yyyy-mm-dd HH:MM:ss.fff")}] UR_Status.ini>>>> {buffer_}");
                        sw.WriteAsync(buffer_.ToString());
                        sw.Flush();
                        sw.Close();
                    }

                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.Trace($"ExportStateToFile.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }
        #endregion
    }

    public class MaterialPackageManager
    {
        #region Fields
        protected string exportPath = string.Empty;
        protected ReelUnloadReportStates reportState = ReelUnloadReportStates.Unknown;
        protected List<MaterialPackage> materialPackages = new List<MaterialPackage>();
        #endregion

        #region Properties
        public IReadOnlyList<MaterialPackage> Materials => materialPackages;
        public ReelUnloadReportStates ReportState => reportState;
        #endregion

        #region Events
        public event EventHandler<MaterialPackage> AddedMaterialPackage;
        public event EventHandler<string> RemovedMaterialPackage;
        public event EventHandler RemovedAllMaterialPackages;
        #endregion

        #region Constructors
        protected MaterialPackageManager() {}
        #endregion

        #region Protected methods
        #endregion

        #region Public methods
        public virtual void SetExportPath(string path = @"D:\System\")
        {
            if (!Directory.Exists(path))      // Update 20190701
                Directory.CreateDirectory((path));

            exportPath = path;
        }

        public virtual void FireAddedMaterialPackage(MaterialPackage pkg)
        {
            if (pkg != null)
                AddedMaterialPackage?.Invoke(this, pkg);
        }

        public virtual void FireRemovedMaterialPackage(string name)
        {
            if (!string.IsNullOrEmpty(name))
                RemovedMaterialPackage?.Invoke(this, name);
        }

        public virtual void FireRemoveAllMaterialPackages()
        {
            RemovedAllMaterialPackages?.Invoke(this, EventArgs.Empty);
        }

        
        public virtual void ExportMaterialPackage(MaterialPackage pkg)
        {
            try
            {
                if (pkg == null)
                    return;

                // new Thread(new ParameterizedThreadStart(pkg.ExportToFile)).Start(exportPath + "UR_PickInfo.ini");
                pkg.ExportToFile(exportPath + "UR_PickInfo.ini");
            }
            catch (Exception ex)
            {
                Logger.Trace($"{this.GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        
        public virtual void ExportUnloadMaterial(string data)
        {
            try
            {
                if (string.IsNullOrEmpty(data))
                    return;

                MaterialPackage.ExportDataToFile(exportPath + "UR_WorkInfo.ini", data);
            }
            catch (Exception ex)
            {
                Logger.Trace($"{this.GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        
        public virtual void ExportUnloadState(ReelUnloadReportStates state)
        {
            try
            {
                if (reportState != state)
                {
                    MaterialPackage.ExportStateToFile(exportPath + "UR_Status.ini", reportState = state);
                }
            }
            catch (Exception ex)
            {
                Logger.Trace($"{this.GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        
        public virtual void DeleteMaterialPackage(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                    return;

                if (File.Exists(exportPath + string.Format("UR_PickInfo_{0}.ini", name)))
                    File.Delete(exportPath + string.Format("UR_PickInfo_{0}.ini", name));
            }
            catch (Exception ex)
            {
                Logger.Trace($"{this.GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        
        public virtual void ClearMaterialPackages()
        {
            try
            {
                string[] files_ = Directory.GetFiles(exportPath);

                foreach (string fname_ in files_)
                {
                    if (fname_.Contains("UR_PickInfo_"))
                        File.Delete(fname_);
                }
            }
            catch (Exception ex)
            {
                Logger.Trace($"{this.GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        
        public virtual int AddMaterialPackage(MaterialPackage pkg, bool notify = false, bool export = false)
        {
            try
            {
                if (pkg == null)
                    return -1;

                materialPackages.Clear();
                materialPackages.Add(pkg);

                if (notify)
                    FireAddedMaterialPackage(pkg);

                if (export)
                    ExportMaterialPackage(pkg);

                return materialPackages.Count;
            }
            catch (Exception ex)
            {
                Logger.Trace($"{this.GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                return -1;
            }
        }

        
        public virtual void ExportLatestReceivedMaterialPackage()
        {
            try 
            {
                foreach (MaterialPackage pkg in materialPackages)
                {
                    if (!pkg.IsExported)
                    {
                        ExportMaterialPackage(pkg);
                        ExportUnloadState(ReelUnloadReportStates.Ready);
                        Logger.Trace($"UR Status : {reportState}"); 
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Trace($"{this.GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        
        public virtual void UpdateMaterialPackage(string material, ReelUnloadReportStates state)
        {
            try
            {
                if (string.IsNullOrEmpty(material))
                    return;

                if (state == ReelUnloadReportStates.Run)
                    ExportUnloadMaterial(material);

                ExportUnloadState(state);
            }
            catch (Exception ex)
            {
                Logger.Trace($"{this.GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        
        public virtual bool RemoveMaterialPackage(string name, bool notify = false, bool remove = true)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                    return false;
                MaterialPackage p = materialPackages.Find(x => x.Name == name);
                if (p != null)
                {
                    if (notify)
                        FireRemovedMaterialPackage(name);
                    if (remove)
                        DeleteMaterialPackage(name);
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.Trace($"{this.GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                return false;
            }
        }

        
        public virtual bool RemoveAllMaterialPackages(bool notify = false, bool remove = true)
        {
            try
            {
                materialPackages.Clear();
                if (notify)
                    FireRemoveAllMaterialPackages();
                if (remove)
                    ClearMaterialPackages();
                return true;
            }
            catch (Exception ex)
            {
                Logger.Trace($"{this.GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                return false;
            }
        }
        #endregion
    }

    public class ReelTowerMessage
    {
        #region Fields
        public readonly int SentTick;
        public ReelTowerCommands Command = ReelTowerCommands.REPLY_LINK_TEST;
        public string Data = string.Empty;
        #endregion

        #region Constructors
        public ReelTowerMessage(ReelTowerCommands command, string data = null)
        {
            Command = command;
            Data = data;
            SentTick = App.TickCount;
        }
        #endregion
    }

    public class ReelTowerObject
    {
        #region Fields
        public int ClientId = 0;
        public AsyncSocketClient Socket = null;
        #endregion

        #region Constructors
        public ReelTowerObject(AsyncSocketClient sock, int clientid)
        {
            Socket = sock;
            ClientId = clientid;
        }
        #endregion

        #region Public methods
        public void Send(string message)
        {
            Socket.Send(Encoding.Default.GetBytes(message));
        }
        #endregion
    }

    public class ReelTowerManager : SimulatableDevice
    {
        #region Constants
        protected const string CONST_TOWER_IDS                      = "T0101;T0102;T0103;T0104;";
        protected readonly char[] CONST_UID_LIST_DELIMITER          = { '|' };
        #endregion

        #region Fields
        protected bool stop                                         = false;
        protected bool requiredLoadCancelIfFailure                  = false;
        protected bool serviceout                                   = false;
        protected int receivedStateResponseFlag                     = 0;
        protected int statePollingTick                              = 0;
        protected int clientId                                      = 0;
        protected int lastLoadedTower                               = 0;
        protected int receivedTowerIdOfState                        = -1;
        protected int receivedTowerIdOfResponse                     = -1;
        protected int timeoutOfResponse                             = 3000;
        protected string currentLoadTowerId                         = string.Empty;
        protected string currentUnloadTowerId                       = string.Empty;
        protected string lastRequestedLoadTowerId                   = string.Empty;
        protected string lastReceivedMessage                        = string.Empty;
        protected string lastCommunicationStateLog                  = string.Empty;
        protected XmlDocument receivedXml                           = new XmlDocument();
        protected MaterialStorageMessage receivedData               = new MaterialStorageMessage();
        protected MaterialData receivedBarcode                      = new MaterialData();
        protected Pair<int, string> queuedMessage                   = new Pair<int, string>(0, string.Empty);
        protected Thread threadReelTowerStateWatcher                = null;
        protected MaterialStorageState towerResponse                = new MaterialStorageState();
        protected Dictionary<int, ReelTowerObject> clients          = new Dictionary<int, ReelTowerObject>();
        protected ConcurrentQueue<ReelTowerMessage> messageQueue    = new ConcurrentQueue<ReelTowerMessage>();
        protected ConcurrentQueue<Pair<int, string>> receivedMessages  = new ConcurrentQueue<Pair<int, string>>();
        protected List<int> loadAvailableTowers                     = new List<int>();
        //protected List<MaterialPackage> materialPackages            = new List<MaterialPackage>();
        protected List<MaterialStorageState> unloadRequests         = new List<MaterialStorageState>();
        protected List<MaterialStorageState> towerStates            = new List<MaterialStorageState>();
        protected Dictionary<string, ReelTowerCommands> reelTowerCommands = new Dictionary<string, ReelTowerCommands>()
        {
            { "REQUEST_TOWER_STATE",            TechFloor.ReelTowerCommands.REQUEST_TOWER_STATE },
            { "REQUEST_TOWER_STATE_ALL",        TechFloor.ReelTowerCommands.REQUEST_TOWER_STATE_ALL },
            { "REQUEST_BARCODEINFO_CONFIRM",    TechFloor.ReelTowerCommands.REQUEST_BARCODEINFO_CONFIRM },
            { "REQUEST_REEL_LOAD_MOVE",         TechFloor.ReelTowerCommands.REQUEST_REEL_LOAD_MOVE },
            { "REQUEST_REEL_LOAD_ASSIGN",       TechFloor.ReelTowerCommands.REQUEST_REEL_LOAD_ASSIGN },
            { "REQUEST_LOAD_COMPLETE",          TechFloor.ReelTowerCommands.REQUEST_LOAD_COMPLETE },
            { "REQUEST_UNLOAD_COMPLETE",        TechFloor.ReelTowerCommands.REQUEST_UNLOAD_COMPLETE },
            { "REQUEST_LINK_TEST",              TechFloor.ReelTowerCommands.REQUEST_LINK_TEST },
            { "REQUEST_REEL_UNLOAD_MOVE",       TechFloor.ReelTowerCommands.REQUEST_REEL_UNLOAD_MOVE },
            { "REQUEST_REEL_UNLOAD_ASSIGN",     TechFloor.ReelTowerCommands.REQUEST_REEL_UNLOAD_ASSIGN },
            { "REQUEST_LOAD_RESET",             TechFloor.ReelTowerCommands.REQUEST_LOAD_RESET },
            { "REQUEST_UNLOAD_RESET",           TechFloor.ReelTowerCommands.REQUEST_UNLOAD_RESET },
            { "REPLY_TOWER_STATE",              TechFloor.ReelTowerCommands.REPLY_TOWER_STATE },
            { "REPLY_TOWER_STATE_ALL",          TechFloor.ReelTowerCommands.REPLY_TOWER_STATE_ALL },
            { "REPLY_BARCODEINFO_CONFIRM",      TechFloor.ReelTowerCommands.REPLY_BARCODEINFO_CONFIRM },
            { "REPLY_REEL_LOAD_MOVE",           TechFloor.ReelTowerCommands.REPLY_REEL_LOAD_MOVE },
            { "REPLY_REEL_LOAD_ASSIGN",         TechFloor.ReelTowerCommands.REPLY_REEL_LOAD_ASSIGN },
            { "REPLY_REEL_UNLOAD_MOVE",         TechFloor.ReelTowerCommands.REPLY_REEL_UNLOAD_MOVE },
            { "REPLY_REEL_UNLOAD_ASSIGN",       TechFloor.ReelTowerCommands.REPLY_REEL_UNLOAD_ASSIGN },
            { "REPLY_LOAD_COMPLETE",            TechFloor.ReelTowerCommands.REPLY_LOAD_COMPLETE },
            { "REPLY_UNLOAD_COMPLETE",          TechFloor.ReelTowerCommands.REPLY_UNLOAD_COMPLETE },
            { "REPLY_LINK_TEST",                TechFloor.ReelTowerCommands.REPLY_LINK_TEST },
            { "REPLY_LOAD_RESET",               TechFloor.ReelTowerCommands.REPLY_LOAD_RESET },
            { "REPLY_UNLOAD_RESET",             TechFloor.ReelTowerCommands.REPLY_UNLOAD_RESET },
            { "SEND_PICKING_LIST",              TechFloor.ReelTowerCommands.SEND_PICKING_LIST },
            { "REQUEST_ALL_LOAD_RESET",         TechFloor.ReelTowerCommands.REQUEST_ALL_LOAD_RESET },
            { "REQUEST_ALL_UNLOAD_RESET",       TechFloor.ReelTowerCommands.REQUEST_ALL_UNLOAD_RESET },
            { "REQUEST_ALL_ALARM_RESET",        TechFloor.ReelTowerCommands.REQUEST_ALL_ALARM_RESET },

        };
        protected AsyncSocketServer server                                      = null;
        protected CommunicationStates communicationState                        = CommunicationStates.None;
        protected System.Timers.Timer responseWatcher                           = null;
        #endregion

        #region Properties
        public int TimeoutOfTowerResponse                                       => timeoutOfResponse;
        public bool IsRunning                                                   => !stop;
        public bool IsServiceNow                                                => (server == null? false : server.IsRunning && clients.Count > 0);
        public bool IsServerRunning                                             => (server == null? false : server.IsRunning);
        public bool IsConnected                                                 => clients.Count > 0;        
        public CommunicationStates CommunicationState                           => communicationState;
        public IReadOnlyList<MaterialStorageState> TowerStates                  => towerStates;
        public MaterialStorageState TowerResponses                              => towerResponse;
        public IReadOnlyDictionary<string, ReelTowerCommands> ReelTowerCommandList => reelTowerCommands;
        #endregion

        #region Events
        public event EventHandler<CommunicationStates> CommunicationStateChanged;
        public event EventHandler<string> ReportRuntimeLog;
        public event EventHandler<string> ReportException;
        public event EventHandler<MaterialPackage> ReceivedMaterialPackage;
        #endregion

        #region Costructors
        public ReelTowerManager()
        {
            towerStates.Add(new MaterialStorageState(1, Config.ReelTowerId1));
            towerStates.Add(new MaterialStorageState(2, Config.ReelTowerId2));
            towerStates.Add(new MaterialStorageState(3, Config.ReelTowerId3));
            towerStates.Add(new MaterialStorageState(4, Config.ReelTowerId4));
        }
        #endregion

        #region Protected methods
        #region Dispose methods
        protected override void DisposeManagedObjects()
        {
            if (responseWatcher != null)
                responseWatcher.Stop();

            CommunicationStateChanged -= (App.MainForm as FormMain).OnChangedReelTowerCommunicationState;
            ReportRuntimeLog -= (App.MainForm as FormMain).UpdateReelTowerRuntimeLog;

            if (threadReelTowerStateWatcher != null)
                threadReelTowerStateWatcher.Join();

            ClearMessageQueue();

            foreach (var obj in towerStates)
                obj.Dispose();

            towerResponse.Dispose();

            clients.Clear();
            //Singleton<MaterialPackageManager>.Instance.RemoveAllMaterialPackages();
            base.DisposeManagedObjects();
        }
        #endregion

        #region Received message pump methods
        protected void LoadProperties()
        {
            timeoutOfResponse = Config.TimeoutOfReelTowerResponse;
        }

        protected void Run()
        {
            Logger.Trace($"Started thread={ClassName}.{MethodBase.GetCurrentMethod().Name}");

            while (!stop)
            {
                if (serviceout || (App.ShutdownEvent as ManualResetEvent).WaitOne(10))
                {
                    Logger.Trace($"Stopped thread={MethodBase.GetCurrentMethod().Name}");
                    stop = true;
                }
                else
                {
                    if (receivedMessages.Count > 0)
                    {
                        ParseReceivedMessage();
                    }
                }
            }
        }
        #endregion

        #region Communication state change event methods
        protected void FireCommunicationStateChanged(CommunicationStates state)
        {
            CommunicationStateChanged?.Invoke(this, communicationState = state);
        }
        #endregion

        #region Service event methods
        protected void OnServiceStarted(object sender, EventArgs e)
        {
            FireCommunicationStateChanged(CommunicationStates.Listening);
            FireUpdateRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}");

            responseWatcher = new System.Timers.Timer();
            responseWatcher.Interval = 100;
            responseWatcher.AutoReset = true;            
            responseWatcher.Elapsed += OnTickWatcher;
        }

        protected void OnServiceStopped(object sender, EventArgs e)
        {
            FireCommunicationStateChanged(CommunicationStates.Disconnected);
            FireUpdateRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}");
        }
        #endregion

        #region Client communication event methods
        protected void OnAcceptClientConnection(object sender, AsyncSocketAcceptEventArgs e)
        {
            foreach (var client in clients)
            {
                IPEndPoint s1 = client.Value.Socket.Sock.RemoteEndPoint as IPEndPoint;
                IPEndPoint s2 = e.Worker.RemoteEndPoint as IPEndPoint;

                if (s1.Address.ToString().Contains(s2.Address.ToString()))
                    client.Value.Socket.Disconnect();
            }

            FireCommunicationStateChanged(CommunicationStates.Accepted);
            FireUpdateRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}={++clientId}");

            Thread.Sleep(100);
            AsyncSocketClient newClient = new AsyncSocketClient(clientId, e.Worker, OnClientConnect, OnClientSent, OnClientReceived);
            newClient.OnDisconnected    += new AsyncSocketDisconnectedEventHandler(OnClientDisconnect);
            newClient.OnError           += new AsyncSocketErrorEventHandler(OnClientError);

            lock (clients)
                clients.Add(clientId, new ReelTowerObject(newClient, clientId));
        }

        
        protected void OnClientError(object sender, AsyncSocketErrorEventArgs e)
        {
            try
            {
                FireCommunicationStateChanged(CommunicationStates.Error);
                FireUpdateRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}={e.AsyncSocketException.Message}");

                ClearMessageQueue();
                ClearReelTowerResponse();
                receivedStateResponseFlag = 0;

                lock (clients)
                {
                    foreach (var obj in clients)
                        clients[obj.Key].Socket.Disconnect();

                    clients.Clear();
                }
            }
            catch (Exception ex)
            {
                Logger.Trace(ex.Message);
            }
        }

        protected void OnClientConnect(object sender, AsyncSocketConnectionEventArgs e)
        {
            lastReceivedMessage = string.Empty;
            receivedStateResponseFlag = 0;
            FireCommunicationStateChanged(CommunicationStates.Connected);
            FireUpdateRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}={e.ID}");
        }

        protected void OnClientDisconnect(object sender, AsyncSocketConnectionEventArgs e)
        {
            lock (clients)
            {
                if (clients.ContainsKey(e.ID))
                    clients.Remove(e.ID);
            }

            if (clients.Count <= 0)
                FireCommunicationStateChanged(CommunicationStates.Disconnected);

            FireUpdateRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}={e.ID}");
            lastReceivedMessage = string.Empty;
            receivedStateResponseFlag = 0;
        }

        protected void OnTickWatcher(object sender, EventArgs e)
        {
            responseWatcher.Stop();
            if (IsConnected)
            {
                if (towerResponse.IsWaitResponse)
                {
                    int timeout_ = timeoutOfResponse;
                    
                    switch (towerResponse.Command)
                    {
                        case ReelTowerCommands.SEND_PICKING_LIST:
                        case ReelTowerCommands.REQUEST_LINK_TEST:
                        case ReelTowerCommands.REQUEST_TOWER_STATE:
                        case ReelTowerCommands.REQUEST_TOWER_STATE_ALL:
                        case ReelTowerCommands.REQUEST_BARCODEINFO_CONFIRM:
                        case ReelTowerCommands.REQUEST_REEL_LOAD_MOVE:
                        case ReelTowerCommands.REQUEST_REEL_LOAD_ASSIGN:
                        case ReelTowerCommands.REQUEST_LOAD_COMPLETE:
                        case ReelTowerCommands.REQUEST_UNLOAD_COMPLETE:
                        case ReelTowerCommands.REQUEST_REEL_UNLOAD_MOVE:
                        case ReelTowerCommands.REQUEST_REEL_UNLOAD_ASSIGN:
                        case ReelTowerCommands.REQUEST_LOAD_RESET:
                        case ReelTowerCommands.REQUEST_ALL_ALARM_RESET:
                        case ReelTowerCommands.REQUEST_ALL_LOAD_RESET:
                        case ReelTowerCommands.REQUEST_ALL_UNLOAD_RESET:
                        case ReelTowerCommands.REQUEST_UNLOAD_RESET:
                        case ReelTowerCommands.REPLY_LINK_TEST:
                        case ReelTowerCommands.REPLY_TOWER_STATE:
                        case ReelTowerCommands.REPLY_TOWER_STATE_ALL:
                        case ReelTowerCommands.REPLY_BARCODEINFO_CONFIRM:
                        case ReelTowerCommands.REPLY_REEL_LOAD_MOVE:
                        case ReelTowerCommands.REPLY_REEL_LOAD_ASSIGN:
                        case ReelTowerCommands.REPLY_REEL_UNLOAD_MOVE:
                        case ReelTowerCommands.REPLY_REEL_UNLOAD_ASSIGN:
                        case ReelTowerCommands.REPLY_LOAD_COMPLETE:
                        case ReelTowerCommands.REPLY_UNLOAD_COMPLETE:
                        case ReelTowerCommands.REPLY_LOAD_RESET:
                        case ReelTowerCommands.REPLY_UNLOAD_RESET:
                            timeout_ = timeoutOfResponse;
                            break;
                    }

                    if (IsResponseTimeout(timeout_))
                        ClearReelTowerResponse(true);
                }
            }
            responseWatcher.Start();
        }

        protected void OnClientReceived(object sender, AsyncSocketReceiveEventArgs e)
        {
            receivedMessages.Enqueue(new Pair<int, string>(e.ID, new string(Encoding.Default.GetChars(e.ReceiveData))));
        }

        protected void OnClientSent(object sender, AsyncSocketSendEventArgs e)
        {
        }
        #endregion

        #region Parse received message
        protected bool ParseReelTowerState(string data, ref MaterialStorageState.StorageOperationStates state)
        {
            bool result_ = true;

            switch (data.ToLower())
            {
                case "unknown":
                    state = MaterialStorageState.StorageOperationStates.Unknown;
                    break;
                case "run":
                    state = MaterialStorageState.StorageOperationStates.Run;
                    break;
                case "down":
                    state = MaterialStorageState.StorageOperationStates.Down;
                    break;
                case "error":
                    state = MaterialStorageState.StorageOperationStates.Error;
                    break;
                case "load":
                    state = MaterialStorageState.StorageOperationStates.Load;
                    break;
                case "unload":
                    state = MaterialStorageState.StorageOperationStates.Unload;
                    break;
                case "wait":
                    state = MaterialStorageState.StorageOperationStates.Wait;
                    break;
                case "full":
                    state = MaterialStorageState.StorageOperationStates.Full;
                    break;
                case "idle":
                    state = MaterialStorageState.StorageOperationStates.Idle;
                    break;
                default:
                    result_ = false;
                    break;
            }

            return result_;
        }

        
        protected void ParseReceivedMessage()
        {
            int tagBegin_                       = 0;
            int tagEnd_                         = 0;
            int tagLength_                      = 0;
            int outputStage_                    = 0;
            int clientId_                       = 0;
            int receivedTowerIndex_             = -1;
            int pickcount_                      = 0;
            string receiveMessage_              = string.Empty;
            string messageType_                 = string.Empty;
            string messageTagBegin_             = "<messageName>";
            string messageTagEnd_               = "</messageName>";
            string pickid_                      = string.Empty;
            string targetloc_                   = string.Empty;
            string receivedId_                  = string.Empty;
            List<int> indices_                  = new List<int>();
            List<List<string>> uids_            = new List<List<string>>();
            Dictionary<int, Pair<string, string>> receivedStates_ = new Dictionary<int, Pair<string, string>>();
            ReelTowerCommands messageCommand_   = TechFloor.ReelTowerCommands.REQUEST_TOWER_STATE;
            MaterialStorageState.StorageOperationStates state_ = MaterialStorageState.StorageOperationStates.Unknown;

            if (!receivedMessages.TryDequeue(out queuedMessage))
                return;

            clientId_ = queuedMessage.first;
            receiveMessage_ = queuedMessage.second;
            receiveMessage_ = receiveMessage_.Replace("\0", string.Empty);

            if (receiveMessage_[0] == (char)AsciiControlCharacters.Stx && receiveMessage_[receiveMessage_.Length - 1] == (char)AsciiControlCharacters.Etx)
                receiveMessage_ = receiveMessage_.Substring(1, receiveMessage_.Length - 2);
            else
                return;

            try
            {
                tagBegin_       = receiveMessage_.IndexOf(messageTagBegin_);
                tagEnd_         = receiveMessage_.IndexOf(messageTagEnd_);
                tagLength_      = messageTagBegin_.Length;
                messageType_    = receiveMessage_.Substring(tagBegin_ + tagLength_, tagEnd_ - tagBegin_ - tagLength_);

                if (reelTowerCommands.ContainsKey(messageType_))
                {
                    if ((messageCommand_ = reelTowerCommands[messageType_]) == TechFloor.ReelTowerCommands.REQUEST_LINK_TEST)
                    {
                        ReplyPing();
                    }
                    else
                    {
                        receivedData.Clear();
                        receivedXml.LoadXml(receiveMessage_);
                        XmlNode root    = receivedXml.SelectSingleNode("message");
                        XmlNode header  = root.SelectSingleNode("//header");
                        XmlNode body    = root.SelectSingleNode("//body");
                        XmlNode tail    = root.SelectSingleNode("//return");

                        if (header != null)
                        {   // Parse header of message
                            receivedData.Name           = header.SelectSingleNode("messageName").InnerText;
                            receivedData.TransactionID  = header.SelectSingleNode("transactionId").InnerText;
                            receivedData.TimeStamp      = header.SelectSingleNode("timeStamp").InnerText;
                        }
                        else
                            return;

                        if (body != null)
                        {   // Parse body of message
                            foreach (XmlNode element in body.ChildNodes)
                            {
                                switch (element.Name)
                                {
                                    case "MATERIALTOWER":
                                        {   // State of all towers
                                            foreach (XmlNode child in element.ChildNodes)
                                            {
                                                switch (child.Name)
                                                {
                                                    case "ID":
                                                        {
                                                            switch (receivedTowerIndex_ = Convert.ToInt32(child.InnerText[child.InnerText.Length - 1].ToString()))
                                                            {
                                                                case 1:
                                                                case 2:
                                                                case 3:
                                                                case 4:
                                                                    {
                                                                        receivedId_ = child.InnerText;
                                                                        receivedTowerIndex_ -= 1;

                                                                        if (!receivedStates_.ContainsKey(receivedTowerIndex_))
                                                                        {
                                                                            receivedStates_.Add(receivedTowerIndex_, new Pair<string, string>(receivedId_, string.Empty));
                                                                        }
                                                                        else
                                                                        {
                                                                            receivedStates_[receivedTowerIndex_].first = receivedId_;
                                                                            receivedStates_[receivedTowerIndex_].second = string.Empty;
                                                                        }
                                                                    }
                                                                    break;
                                                                default:
                                                                    return;
                                                            }
                                                        }
                                                        break;
                                                    case "STATE":
                                                        {
                                                            if (!string.IsNullOrEmpty(receivedId_) && receivedTowerIndex_ >= 0)
                                                            {
                                                                if (receivedStates_.ContainsKey(receivedTowerIndex_) && receivedStates_[receivedTowerIndex_].first == receivedId_)
                                                                {
                                                                    receivedStates_[receivedTowerIndex_].second = child.InnerText;
                                                                }
                                                            }
                                                        }
                                                        break;
                                                }
                                            }
                                        }
                                        break;
                                    case "MATERIALTOWERID":
                                        {   // Single tower state report
                                            Logger.Trace($"{ClassName}.{MethodBase.GetCurrentMethod().Name}: Received_MaterialTowerId> {element.InnerText}");

                                            if (string.IsNullOrEmpty(receivedData.TowerId = element.InnerText))
                                                return;

                                            if (receivedData.TowerId.Contains(";"))
                                            {
                                                string[] ids_ = receivedData.TowerId.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                                                foreach (string id_ in ids_)
                                                {
                                                    switch (receivedTowerIndex_ = Convert.ToInt32(id_[id_.Length - 1].ToString()))
                                                    {
                                                        case 1:
                                                        case 2:
                                                        case 3:
                                                        case 4:
                                                            {
                                                                receivedId_ = id_;
                                                                receivedTowerIndex_ -= 1;

                                                                if (!receivedStates_.ContainsKey(receivedTowerIndex_))
                                                                {
                                                                    receivedStates_.Add(receivedTowerIndex_, new Pair<string, string>(receivedId_, string.Empty));
                                                                }
                                                                else
                                                                {
                                                                    receivedStates_[receivedTowerIndex_].first = receivedId_;
                                                                    receivedStates_[receivedTowerIndex_].second = string.Empty;
                                                                }
                                                            }
                                                            break;
                                                        default:
                                                            return;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                switch (receivedTowerIndex_ = Convert.ToInt32(receivedData.TowerId[receivedData.TowerId.Length - 1].ToString()))
                                                {
                                                    case 1:
                                                    case 2:
                                                    case 3:
                                                    case 4:
                                                        {
                                                            receivedId_ = receivedData.TowerId;
                                                            receivedTowerIndex_ -= 1;
                                                        }
                                                        break;
                                                    default:
                                                        return;
                                                }
                                            }
                                        }
                                        break;
                                    case "MATERIALTOWERSTATE":
                                        {   // Single tower state report
                                            if (string.IsNullOrEmpty(receivedData.TowerState = element.InnerText))
                                                return;

                                            if (receivedData.TowerState.Contains(";"))
                                            {
                                                string[] states_ = receivedData.TowerState.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                                                for (int i_ = 0; i_ < states_.Length; i_++)
                                                {
                                                    receivedStates_[i_].second = states_[i_];
                                                }
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(receivedId_) && receivedTowerIndex_ >= 0)
                                                {
                                                    if (!receivedStates_.ContainsKey(receivedTowerIndex_))
                                                        receivedStates_.Add(receivedTowerIndex_, new Pair<string, string>(receivedId_, receivedData.TowerState));
                                                }
                                                else
                                                    return;
                                            }   
                                        }
                                        break;
                                    case "UID":
                                        {
                                            receivedData.Data.Rid = element.InnerText;
                                            receivedData.Data.Text = element.InnerText;
                                        }
                                        break;
                                    case "LOADSTATE":
                                        {
                                            receivedData.LoadState = element.InnerText;
                                        }
                                        break;
                                    case "STAGE":
                                        {
                                            if (!string.IsNullOrEmpty(receivedData.OutputStage = element.InnerText))
                                            {   // S0101,S0102,S0103
                                                switch (outputStage_ = Convert.ToInt32(receivedData.OutputStage[receivedData.OutputStage.Length - 1].ToString()))
                                                {
                                                    case 1:
                                                    case 2:
                                                    case 3:
                                                        break;
                                                    default:
                                                        return;
                                                }
                                            }
                                            else
                                                return;
                                        }
                                        break;
                                    case "BARCODEINFO":
                                        {
                                            foreach (XmlNode child in element.ChildNodes)
                                            {
                                                switch (child.Name)
                                                {
                                                    case "SID":
                                                        receivedData.Data.Sid = child.InnerText;
                                                        break;
                                                    case "LOTID":
                                                        receivedData.Data.LotId = child.InnerText;
                                                        break;
                                                    case "BATCHID":
                                                        receivedData.Data.Maker = child.InnerText;
                                                        break;
                                                    case "QTY":
                                                        receivedData.Data.Qty = child.InnerText;
                                                        break;
                                                    case "MFG":
                                                        receivedData.Data.Mfg = child.InnerText;
                                                        break;
                                                }
                                            }
                                        }
                                        break;
                                    case "PICK_ID":
                                        {
                                            pickid_ = element.InnerText;
                                        }
                                        break;
                                    case "TARGET_LOCATION":
                                        {
                                            targetloc_ = element.InnerText;
                                        }
                                        break;
                                    case "COUNT":
                                        {
                                            pickcount_ = int.Parse(element.InnerText);
                                        }
                                        break;
                                    case "UID_LIST":
                                        {
                                            string[] items_ = element.InnerText.Split(CONST_UID_LIST_DELIMITER, StringSplitOptions.RemoveEmptyEntries);

                                            foreach (string temp_ in items_)
                                            {
                                                string[] vals_ = temp_.Split(';');
                                                List<string> temp = new List<string>();
                                                for(int i = 0; i < vals_.Length; i++)
                                                {
                                                    temp.Add(vals_[i]);
                                                }
                                                uids_.Add(temp);
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                        else
                            return;

                        if (tail != null)
                        {   // Parse tail of message
                            foreach (XmlNode element in tail.ChildNodes)
                            {
                                switch (element.Name)
                                {
                                    case "returnCode":
                                        receivedData.ReturnCode = element.InnerText;
                                        break;
                                    case "returnMessage":
                                        receivedData.ReturnMessage = element.InnerText;
                                        break;
                                }
                            }
                        }
                        else
                            return;

                        switch (messageCommand_)
                        {
                            case TechFloor.ReelTowerCommands.SEND_PICKING_LIST:
                                {   // Forced assignment to process material package from tower.
                                    receivedTowerIndex_ = 0;
                                }
                                break;
                            case TechFloor.ReelTowerCommands.REQUEST_LOAD_COMPLETE:
                                break;
                            case TechFloor.ReelTowerCommands.REQUEST_UNLOAD_COMPLETE:
                                {
                                    if (receivedTowerIndex_ < 0)
                                        return;
                                }
                                break;
                            case TechFloor.ReelTowerCommands.REPLY_TOWER_STATE:
                                {   // Process response of single tower state report from tower interface.                                    
                                    if (receivedStates_.Count == 1 && receivedTowerIndex_ >= 0 && !string.IsNullOrEmpty(receivedData.TowerState))
                                    {
                                        if (ParseReelTowerState(receivedData.TowerState, ref state_))
                                        {
                                            lock (towerStates)
                                            {
                                                if (receivedTowerIndex_ >= 0 && !string.IsNullOrEmpty(receivedId_) && towerStates[receivedTowerIndex_].Name == receivedId_)
                                                    towerStates[receivedTowerIndex_].State = state_;
                                            }

                                            Interlocked.Exchange(ref receivedStateResponseFlag, 2);
                                        }
                                        else
                                            return;
                                    }
                                    else if (receivedStates_.Count > 1)
                                    {
                                        if (receivedStates_.Count > 0)
                                        {
                                            foreach (var obj in receivedStates_)
                                            {
                                                if (ParseReelTowerState(obj.Value.second, ref state_))
                                                {
                                                    lock (towerStates)
                                                    {
                                                        if (obj.Key >= 0 && !string.IsNullOrEmpty(obj.Value.first) && towerStates[obj.Key].Name == obj.Value.first)
                                                        {
                                                            towerStates[obj.Key].State = state_;
                                                        }
                                                    }

                                                    Interlocked.Exchange(ref receivedStateResponseFlag, 2);
                                                }
                                                else
                                                    return;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Logger.Trace("Received state of tower report is not processed properly.");
                                        return;
                                    }
                                }
                                break;
                            case TechFloor.ReelTowerCommands.REPLY_TOWER_STATE_ALL:
                                {   // Process response of all tower state report from tower interface.
                                    if (receivedStates_.Count > 0)
                                    {
                                        foreach (var obj in receivedStates_)
                                        {
                                            if (ParseReelTowerState(obj.Value.second, ref state_))
                                            {
                                                lock (towerStates)
                                                {
                                                    if (obj.Key - 1 >= 0 && !string.IsNullOrEmpty(obj.Value.first) && towerStates[obj.Key - 1].Name == obj.Value.first)
                                                    {
                                                        towerStates[obj.Key - 1].State = state_;
                                                    }
                                                }

                                                Interlocked.Exchange(ref receivedStateResponseFlag, 2);
                                            }
                                            else
                                                return;
                                        }
                                    }
                                    else
                                    {
                                        Logger.Trace("Received state of tower report is not processed properly.");
                                        return;
                                    }
                                }
                                break;
                            case TechFloor.ReelTowerCommands.REPLY_REEL_LOAD_MOVE:
                            case TechFloor.ReelTowerCommands.REPLY_BARCODEINFO_CONFIRM:
                            case TechFloor.ReelTowerCommands.REPLY_REEL_LOAD_ASSIGN:
                                {
                                    if (towerResponse.IsWaitResponse)
                                    {
                                        switch (towerResponse.Command)
                                        {
                                            case TechFloor.ReelTowerCommands.REQUEST_REEL_LOAD_MOVE:
                                            case TechFloor.ReelTowerCommands.REQUEST_BARCODEINFO_CONFIRM:
                                            case TechFloor.ReelTowerCommands.REQUEST_REEL_LOAD_ASSIGN:
                                                {
                                                    lock (towerResponse)
                                                    {
                                                        towerResponse.UpdateResponse(messageCommand_,
                                                            receivedData,
                                                            outputStage_);
                                                    }
                                                }
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        if (messageCommand_ == TechFloor.ReelTowerCommands.REQUEST_REEL_LOAD_ASSIGN)
                                            towerResponse.UpdateResponse(messageCommand_,
                                                receivedData,
                                                outputStage_);
                                    }
                                }
                                break;
                            case TechFloor.ReelTowerCommands.REPLY_REEL_UNLOAD_MOVE:
                            case TechFloor.ReelTowerCommands.REPLY_REEL_UNLOAD_ASSIGN:
                                {
                                    if (towerResponse.IsWaitResponse)
                                    {
                                        switch (towerResponse.Command)
                                        {
                                            case TechFloor.ReelTowerCommands.REQUEST_REEL_UNLOAD_MOVE:
                                            case TechFloor.ReelTowerCommands.REQUEST_REEL_UNLOAD_ASSIGN:
                                                {
                                                    lock (towerResponse)
                                                    {
                                                        towerResponse.UpdateResponse(messageCommand_,
                                                            receivedData,
                                                            outputStage_);
                                                    }
                                                }
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        if (messageCommand_ == TechFloor.ReelTowerCommands.REPLY_REEL_UNLOAD_ASSIGN)
                                            towerResponse.UpdateResponse(messageCommand_,
                                                receivedData,
                                                outputStage_);
                                    }
                                }
                                break;
                            case TechFloor.ReelTowerCommands.REPLY_LOAD_RESET:
                            case TechFloor.ReelTowerCommands.REPLY_UNLOAD_RESET:
                                {
                                    if (towerResponse.IsWaitResponse)
                                    {
                                        switch (towerResponse.Command)
                                        {
                                            case TechFloor.ReelTowerCommands.REQUEST_LOAD_RESET:
                                            case TechFloor.ReelTowerCommands.REQUEST_UNLOAD_RESET:
                                                {
                                                    lock (towerResponse)
                                                    {
                                                        towerResponse.UpdateResponse(messageCommand_,
                                                            receivedData,
                                                            outputStage_);
                                                    }
                                                }
                                                break;
                                        }
                                    }
                                }
                                break;
                        }

                        if ((messageCommand_ != ReelTowerCommands.REPLY_TOWER_STATE &&
                            messageCommand_ != ReelTowerCommands.REPLY_TOWER_STATE_ALL) ||
                            lastReceivedMessage != receivedData.TowerState)
                        {
                            responseWatcher.Stop();
                            lastReceivedMessage = receivedData.TowerState;
                            FireUpdateRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}={receiveMessage_}");
                        }

                        if (receivedTowerIndex_ >= 0)
                        {
                            switch (messageCommand_)
                            {
                                case ReelTowerCommands.REQUEST_LOAD_COMPLETE:
                                    {
                                        SendTowerMessage(TechFloor.ReelTowerCommands.REPLY_LOAD_COMPLETE, receivedData.Data, towerStates[receivedTowerIndex_].Name);
                                    }
                                    break;
                                case ReelTowerCommands.REQUEST_UNLOAD_COMPLETE:
                                    {
                                        bool result_ = true;

                                        // Add a request to unload reel from tower
                                        lock (unloadRequests)
                                        {
                                            if (receivedTowerIndex_ >= 0 && receivedData.Data != null && !string.IsNullOrEmpty(receivedId_) && towerStates[receivedTowerIndex_].Name == receivedId_)
                                            {
                                                if (unloadRequests.Find(x => x.Data.Rid == receivedData.Data.Rid) == null)
                                                {
                                                    unloadRequests.Add(new MaterialStorageState(receivedTowerIndex_ + 1, receivedId_, receivedData, outputStage_));
                                                    Logger.Trace($"Added a new material (TOWER{receivedTowerIndex_ + 1},{receivedData.Data.Rid})");
                                                    Debug.WriteLine($"Added a new material (TOWER{receivedTowerIndex_ + 1},{receivedData.Data.Rid})");
                                                    result_ = true;
                                                }
                                                else if (receivedData.Data == null)
                                                {
                                                    Logger.Trace($"{ClassName}.{MethodBase.GetCurrentMethod().Name}=receivedData.Data is empty");
                                                }
                                                else if ((unloadRequests.Find(x => x.Data.Rid == receivedData.Data.Rid) != null))
                                                {
                                                    Logger.Trace($"{ClassName}.{MethodBase.GetCurrentMethod().Name}=Duplicated RID");
                                                }
                                            }
                                            else if (towerStates[receivedTowerIndex_].Name != receivedId_)
                                            {
                                                Logger.Trace($"{ClassName}.{MethodBase.GetCurrentMethod().Name}=Tower States don't Match");
                                            }
                                            else if (string.IsNullOrEmpty(receivedId_))
                                            {
                                                Logger.Trace($"{ClassName}.{MethodBase.GetCurrentMethod().Name}=Received Tower ID is empty");
                                            }
                                            else if (receivedTowerIndex_ < 0)
                                            {
                                                Logger.Trace($"{ClassName}.{MethodBase.GetCurrentMethod().Name}=Received Tower ID is less than zero");
                                            }    
                                        }

                                        if (result_)
                                            SendTowerMessage(TechFloor.ReelTowerCommands.REPLY_UNLOAD_COMPLETE, receivedData.Data, towerStates[receivedTowerIndex_].Name);
                                    }
                                    break;
                                case ReelTowerCommands.SEND_PICKING_LIST:
                                    {
                                        if (!string.IsNullOrEmpty(pickid_) &&
                                            !string.IsNullOrEmpty(targetloc_) &&
                                            pickcount_ > 0 &&
                                            uids_.Count == pickcount_)
                                        {
                                            FireReceivedMaterialPackager(new MaterialPackage(pickid_, targetloc_, pickcount_, uids_));
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FireReportException($"{ClassName}.{MethodBase.GetCurrentMethod().Name}={ex.Message}");
            }
        }
        #endregion

        #region Send message
        protected void SendSocketData(string data)
        {
            foreach (var client in clients)
                client.Value.Send(data);
        }
        #endregion

        #region Message assembly methods
        protected void MakeXmlHeader(ref XmlDocument xml,
            ref XmlNode root,
            string messageName)
        {
            string transactionID    = DateTime.Now.ToString("yyyyMMddhhmmssff");
            string timeStamp        = DateTime.Now.ToString("yyyyMMddhhmmss");

            XmlNode nodeHeader      = xml.CreateElement("header");
            AddXmlItem(xml, nodeHeader, "messageName", messageName);
            AddXmlItem(xml, nodeHeader, "transactionId", transactionID);
            AddXmlItem(xml, nodeHeader, "timeStamp", timeStamp);
            root.AppendChild(nodeHeader);
        }

        protected void AddXmlItem(XmlDocument xml,
            XmlNode parentsNode,
            string key,
            string value)
        {
            XmlNode node    = xml.CreateElement(key);
            node.InnerText  = value;
            parentsNode.AppendChild(node);
        }

        protected void AddXmlOfBarcode(ref XmlDocument xml,
            ref XmlNode body,
            MaterialData barcode)
        {
            XmlNode node = xml.CreateElement("BARCODEINFO");
            AddXmlItem(xml, node, "SID", barcode.Sid);
            AddXmlItem(xml, node, "LOTID", barcode.LotId);
            AddXmlItem(xml, node, "SUPPLIER", barcode.Maker);
            AddXmlItem(xml, node, "QTY", barcode.Qty);
            AddXmlItem(xml, node, "MFG", barcode.Mfg);
            AddXmlItem(xml, node, "LOADTYPE", barcode.LoadType.ToString().ToUpper());
            AddXmlItem(xml, node, "SIZE", barcode.ReelType == ReelTypes.ReelDiameter7? "7" : "13");
            body.AppendChild(node);
        }
        #endregion

        #region Fire received material package
        protected void FireReceivedMaterialPackager(MaterialPackage pkg)
        {
            if (pkg != null)
                ReceivedMaterialPackage?.Invoke(this, pkg);
        }
        #endregion
        #endregion

        #region Public methods
        #region Element create methods
        public void Create(int portno)
        {
            serviceout = false;
            LoadProperties();
            StartServer(portno);

            if (receivedData.Data == null)
                receivedData.CreateData();

            CommunicationStateChanged   += (App.MainForm as FormMain).OnChangedReelTowerCommunicationState;
            ReportRuntimeLog            += (App.MainForm as FormMain).UpdateReelTowerRuntimeLog;

            FireCommunicationStateChanged(CommunicationStates.Listening);
            FireUpdateRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}");
            (threadReelTowerStateWatcher = new Thread(new ThreadStart(Run)))?.Start();
        }

        public void Destroy()
        {
            serviceout = true;
            stop = false;
            threadReelTowerStateWatcher.Join();
            StopServer();
            CommunicationStateChanged -= (App.MainForm as FormMain).OnChangedReelTowerCommunicationState;
            ReportRuntimeLog -= (App.MainForm as FormMain).UpdateReelTowerRuntimeLog;
        }
        #endregion

        #region Server service control methods
        public void StartServer(int portno = 0)
        {
            if (server == null)
            {
                server = new AsyncSocketServer(portno);
                server.OnAccepted += new AsyncSocketAcceptedEventHandler(OnAcceptClientConnection);
                server.OnError += new AsyncSocketErrorEventHandler(OnClientError);
                server.OnServiceStarted += OnServiceStarted;
                server.OnServiceStopped += OnServiceStopped;
            }

            server.Start();
        }

        public void StopServer(bool forced = false)
        {
            server.Stop(forced);
            server.OnAccepted -= new AsyncSocketAcceptedEventHandler(OnAcceptClientConnection);
            server.OnError -= new AsyncSocketErrorEventHandler(OnClientError);
            server.OnServiceStarted -= OnServiceStarted;
            server.OnServiceStopped -= OnServiceStopped;            
            server = null;

            if (towerResponse.IsWaitResponse)
            {
                lock (towerResponse)
                {
                    if (towerResponse.Command == ReelTowerCommands.REQUEST_REEL_LOAD_MOVE)
                    {
                        towerResponse.Clear();
                        towerResponse.Reset();
                    }
                }
            }
        }
        #endregion

        #region Event notification method
        public void FireCommunicationStateChanged()
        {
            CommunicationStateChanged?.Invoke(this, CommunicationState);
        }

        public void FireUpdateRuntimeLog(string text = null)
        {
            ReportRuntimeLog?.Invoke(this, text);
        }

        public void FireReportException(string text = null)
        {
            ReportException?.Invoke(this, text);
        }
        #endregion

        #region Unload request check methods
        public bool HasUnloadRequest()
        {
            return (unloadRequests.Find(x => x.IsAssignedToUnload) != null);
        }

        public int GetUnloadRequest(ref MaterialStorageState obj)
        {   // Take the oldest reel assignment information to unload
            if (unloadRequests.Count > 0)
            {
                obj.CopyFrom(unloadRequests[0]);
                return obj.Index;
            }
            return -1;
        }

        // Not use
        public int GetOldestUnloadRequest(ref MaterialStorageState obj)
        {   // Take the oldest reel assignment information to unload
            int oldest_ = -1;
            TimeSpan age_ = TimeSpan.Zero;

            for (int i = 0; i < towerStates.Count; i++)
            {
                MaterialStorageState item = towerStates[i];

                if (item.IsAssignedToUnload && DateTime.Now - item.RequestTime >= age_)
                {
                    age_ = DateTime.Now - item.RequestTime;
                    oldest_ = i;
                }
            }

            if (oldest_ >= 0)
            {
                obj.CopyFrom(towerStates[oldest_]);
                return obj.Index;
            }

            return oldest_;
        }
        #endregion

        #region Clear reel tower state methods after robot handling
        public void ClearAllReelTowerStates()
        {
            lock (unloadRequests)
            {
                foreach (var obj in unloadRequests)
                    obj.Dispose();

                unloadRequests.Clear();
            }
        }

        public void ClearReelTowerStates(string towerid, string uid)
        {
            if (string.IsNullOrEmpty(towerid))
            {
                foreach (var obj in towerStates)
                    obj.Clear();
            }
            else
            {
                lock (unloadRequests)
                {
                    foreach (var obj in unloadRequests)
                    {
                        if (obj.Name == towerid && obj.Uid == uid)
                        {
                            obj.Dispose();
                            unloadRequests.Remove(obj);
                            break;
                        }
                    }
                }
            }
        }

        // Not use
        public void ClearReelTowerStates(string towerid = null)
        {
            if (string.IsNullOrEmpty(towerid))
            {
                foreach (var obj in towerStates)
                    obj.Clear();
            }
            else
            {
                lock (towerStates)
                {
                    foreach (var obj in towerStates)
                    {
                        if (obj.Name == towerid)
                        {
                            obj.Clear();
                            break;
                        }
                    }
                }
            }
        }

        public void ClearMessageQueue()
        {
            Pair<int, string> meesage_ = new Pair<int, string>(0, string.Empty);

            while (!receivedMessages.IsEmpty)
                receivedMessages.TryDequeue(out meesage_);
        }
        #endregion

        #region Clear response message state
        public void ClearReelTowerResponse(bool resetflag = false)
        {
            lock (towerResponse)
            {
                towerResponse.Clear();
                if (resetflag)
                    towerResponse.Reset();
            }
        }
        #endregion

        #region Loadable state check methods
        public bool IsPossibleLoadReel(ref MaterialStorageState obj)
        {
            bool result_ = false;

            if (HasUnloadRequest())
                return false;

            lock (towerStates)
            {   // Rotate load tower index
                if (lastLoadedTower == 4)
                    lastLoadedTower = 0;

                loadAvailableTowers.Clear();

                foreach (MaterialStorageState towerState_ in towerStates)
                {
                    if (towerState_.State == MaterialStorageState.StorageOperationStates.Idle)
                    {
                        if (towerState_.Index > lastLoadedTower && towerState_.Index >= lastLoadedTower + 1)
                        {
                            lastLoadedTower = towerState_.Index;
                            obj.CopyFrom(towerStates[lastLoadedTower - 1]);
                            result_ = true;
                            break;
                        }

                        loadAvailableTowers.Add(towerState_.Index);
                    }
                }

                if (!result_ && loadAvailableTowers.Count > 0)
                {
                    loadAvailableTowers.Sort();
                    lastLoadedTower = loadAvailableTowers[0];
                    obj.CopyFrom(towerStates[lastLoadedTower - 1]);
                    result_ = true;
                }

                // Initialize
                foreach (MaterialStorageState state in towerStates)
                    state.InitData();
            }

            return result_;
        }
        #endregion

        #region Response check methods
        public bool IsReceivedStateResponse(ref bool timedout)
        {
            bool result_ = false;
            timedout = false;

            if (receivedStateResponseFlag >= 2)
            {
                Interlocked.Exchange(ref receivedStateResponseFlag, 0);
                result_ = true;
            }
            else if (TimeSpan.FromMilliseconds(App.TickCount - statePollingTick).TotalMilliseconds >= timeoutOfResponse)
            {
                Interlocked.Exchange(ref receivedStateResponseFlag, 0);
                timedout = true;
            }

            return result_;
        }

        public bool IsReceivedResponse()
        {
            return (towerResponse.IsReceived);
        }

        public bool IsFailure()
        {
            return (towerResponse.IsFailure);
        }

        public bool IsResponseTimeout(int timeout = 10000)
        {
            return (towerResponse.IsResponseTimeout(timeout));
        }

        public void ResetResponse()
        {
            towerResponse.Clear();
        }

        public void IgnoreResponse()
        {
            ClearReelTowerResponse();
            ResetResponse();
        }

        
        public bool GetReceivedResponse(ref string rc)
        {
            bool result_ = false;

            try
            {
                if (towerResponse.IsReceived)
                {
                    if (!string.IsNullOrEmpty(towerResponse.ReturnCode))
                    {
                        rc = towerResponse.ReturnCode;
                        result_ = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Trace(ex.Message);
            }

            return result_;
        }

        public bool GetReceivedResponse(ref MaterialStorageState obj)
        {
            if (towerResponse.IsReceived)
            {
                obj.CopyFrom(towerResponse);
                ClearReelTowerResponse();
                return true;
            }

            return false;
        }

        
        public bool IsValidMaterialData(MaterialStorageState obj, MaterialStorageState response, MaterialData src)
        {
            bool result_ = false;

            try
            {
                if (obj.Name == response.Name)
                {
                    if (response.ReturnCode == "0" &&
                        (response.Sid == src.Sid ||
                        response.LotId == src.LotId))
                        result_ = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Trace(ex.Message);
            }

            return result_;
        }

        
        public bool IsValidMaterialData(MaterialStorageState obj, MaterialData src)
        {
            bool result_ = false;

            try
            {
                if (towerResponse.Name == obj.Name)
                {
                    if (towerResponse.ReturnCode == "0" &&
                        (towerResponse.Sid == src.Sid ||
                        towerResponse.LotId == src.LotId))
                        result_ = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Trace(ex.Message);
            }

            return result_;
        }
        #endregion

        #region Send message methods
        public void ReplyPing(string towerid = null)
        {
            SendTowerMessage(TechFloor.ReelTowerCommands.REPLY_LINK_TEST);
        }

        public bool QueryStates(int towerindex = 0, bool prefix = false)
        {   // Check which reel tower is available to load a reel.
            if (HasUnloadRequest())
                return false;
            else
            {
                string towerid_ = string.Empty;
                switch (towerindex)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                        towerid_ = Config.ReelTowerIds[towerindex];
                        break;
                }

                return SendTowerMessage(TechFloor.ReelTowerCommands.REQUEST_TOWER_STATE,
                    null,
                    string.IsNullOrEmpty(towerid_) ? CONST_TOWER_IDS : towerid_,
                    prefix);
            }
        }

        public bool QueryStatesAll(bool prefix = false)
        {   // Check which reel tower is available to load a reel.
            if (HasUnloadRequest())
                return false;
            else
            {
                string towerid_ = string.Empty;

                foreach (string id_ in Config.ReelTowerIds.Values)
                    towerid_ += string.Format($"{id_};");

                // CHECKED: 20190403
                // Temporary remove last delimiter
                towerid_ = towerid_.Remove(towerid_.Length - 1, 1);
                return SendTowerMessage(TechFloor.ReelTowerCommands.REQUEST_TOWER_STATE, null, towerid_, prefix);
                // return SendTowerMessage(TechFloor.ReelTowerCommands.REQUEST_TOWER_STATE_ALL, null, null, prefix);
            }
        }

        public bool RequestReelUnloadAssign(string towerid, MaterialData barcode)
        {   // Send reel unload assign to reel tower
            if (SendTowerMessage(TechFloor.ReelTowerCommands.REQUEST_REEL_UNLOAD_ASSIGN,
                barcode,
                towerid))
            {
                // Clear reel unload tower state
                ClearReelTowerStates(towerid);
                return true;
            }

            return false;
        }

        public bool SendTowerMessage(ReelTowerCommands command,
            MaterialData barcode = null,
            string towerid = null,
            bool prefix = true,
            string code = "0",
            string message = "done")
        {
            if (communicationState != CommunicationStates.Connected || (towerResponse.IsWaitResponse && command < TechFloor.ReelTowerCommands.REQUEST_REEL_LOAD_MOVE))
            {
                if (lastCommunicationStateLog != $"{ClassName}.{MethodBase.GetCurrentMethod().Name}=Send Tower Message returned false, Waitresponse={towerResponse.IsWaitResponse}, command = {command}, CommunicationStates = {communicationState}")
                    FireUpdateRuntimeLog(lastCommunicationStateLog = $"{ClassName}.{MethodBase.GetCurrentMethod().Name}=Send Tower Message returned false, Waitresponse={towerResponse.IsWaitResponse}, command = {command}, CommunicationStates = {communicationState}");
                return false;
            }

            bool printout_      = true;
            bool waitresponse_  = true;
            XmlDocument xml     = new XmlDocument();
            XmlNode header      = xml.CreateElement("message");
            XmlNode body        = xml.CreateElement("body");
            XmlNode tail        = xml.CreateElement("return");

            switch (command)
            {
                case TechFloor.ReelTowerCommands.REPLY_LINK_TEST:
                    {
                        waitresponse_ = false;
                    }
                    break;
                case TechFloor.ReelTowerCommands.REQUEST_TOWER_STATE:
                    {
                        printout_ = waitresponse_ = false;
                        AddXmlItem(xml, body, "MATERIALTOWERID", towerid);
                    }
                    break;
                case TechFloor.ReelTowerCommands.REQUEST_TOWER_STATE_ALL:
                    {
                        printout_ = waitresponse_ = false;
                    }
                    break;
                case TechFloor.ReelTowerCommands.REQUEST_LOAD_COMPLETE:
                    {
                        AddXmlItem(xml, body, "MATERIALTOWERID", towerid);
                        AddXmlItem(xml, body, "UID", barcode.Rid);
                    }
                    break;
                case TechFloor.ReelTowerCommands.REQUEST_BARCODEINFO_CONFIRM:
                    {   // 투입되는 Reel의 Unit ID는 아래와 같은 조합으로 매번 생성하여 Reel Reel Tower에 넘겨준다
                        // barcode.UID = barcode.Sid + barcode.Maker + DateTime.Now.ToString("hhmmss");
                        AddXmlItem(xml, body, "MATERIALTOWERID", towerid);
                        AddXmlItem(xml, body, "UID", barcode.Rid);
                        AddXmlOfBarcode(ref xml, ref body, barcode);
                    }
                    break;
                case TechFloor.ReelTowerCommands.REQUEST_REEL_LOAD_MOVE:
                    {
                        AddXmlItem(xml, body, "MATERIALTOWERID", towerid);
                        AddXmlItem(xml, body, "LOADSTATE", "LOAD");
                        AddXmlItem(xml, body, "UID", barcode.Rid);
                    }
                    break;
                case TechFloor.ReelTowerCommands.REQUEST_REEL_UNLOAD_MOVE:
                    {
                        AddXmlItem(xml, body, "MATERIALTOWERID", towerid);
                        AddXmlItem(xml, body, "LOADSTATE", "UNLOAD");
                        AddXmlItem(xml, body, "UID", barcode.Rid);
                    }
                    break;
                case TechFloor.ReelTowerCommands.REQUEST_REEL_LOAD_ASSIGN:
                case TechFloor.ReelTowerCommands.REQUEST_REEL_UNLOAD_ASSIGN:
                    {
                        waitresponse_ = false;
                        AddXmlItem(xml, body, "MATERIALTOWERID", towerid);
                        AddXmlItem(xml, body, "UID", barcode.Rid);
                    }
                    break;
                case TechFloor.ReelTowerCommands.REPLY_LOAD_COMPLETE:
                case TechFloor.ReelTowerCommands.REPLY_UNLOAD_COMPLETE:
                    {
                        AddXmlItem(xml, body, "MATERIALTOWERID", towerid);
                        AddXmlItem(xml, body, "UID", barcode.Rid);
                        AddXmlItem(xml, tail, "returnCode", code);
                        AddXmlItem(xml, tail, "returnMessage", message);
                        waitresponse_ = false;
                    }
                    break;
                case TechFloor.ReelTowerCommands.REQUEST_LOAD_RESET:
                    {
                        AddXmlItem(xml, body, "MATERIALTOWERID", towerid);
                    }
                    break;
                case TechFloor.ReelTowerCommands.REQUEST_UNLOAD_RESET:
                    {
                        AddXmlItem(xml, body, "MATERIALTOWERID", towerid);
                    }
                    break;
                case TechFloor.ReelTowerCommands.REQUEST_ALL_LOAD_RESET:
                    {
                        waitresponse_ = false;
                    }
                    break;
                case TechFloor.ReelTowerCommands.REQUEST_ALL_UNLOAD_RESET:
                    {
                        waitresponse_ = false;
                    }
                    break;
                case TechFloor.ReelTowerCommands.REQUEST_ALL_ALARM_RESET:
                    {
                        waitresponse_ = false;
                    }
                    break;
                default:
                    return false;
            }

            MakeXmlHeader(ref xml, ref header, command.ToString());
            header.AppendChild(body);
            header.AppendChild(tail);
            xml.AppendChild(header);

            if (prefix)
                SendSocketData(string.Concat((char)AsciiControlCharacters.Stx, xml.OuterXml, (char)AsciiControlCharacters.Etx));
            else
                SendSocketData(xml.OuterXml);

            if (printout_)
                FireUpdateRuntimeLog($"{ClassName}.{MethodBase.GetCurrentMethod().Name}={xml.OuterXml}");

            if (waitresponse_)
            {
                ClearReelTowerResponse();
                towerResponse.SetCommand(command, waitresponse_);

                switch (command)
                {
                    case ReelTowerCommands.REQUEST_REEL_LOAD_MOVE:
                        {
                            requiredLoadCancelIfFailure = true;
                            lastRequestedLoadTowerId = towerid;
                        }
                        break;
                    // case ReelTowerCommands.REQUEST_LOAD_RESET:
                    // case ReelTowerCommands.REQUEST_ALL_LOAD_RESET:
                    // case ReelTowerCommands.REQUEST_REEL_UNLOAD_ASSIGN:
                    // case ReelTowerCommands.REQUEST_REEL_UNLOAD_MOVE:
                    //     {
                    //         requiredLoadCancelIfFailure = false;
                    //         lastRequestedLoadTowerId = string.Empty;
                     //     }
                    //     break;
                }

                if (!responseWatcher.Enabled)
                    responseWatcher.Start();
            }
            else
            {
                switch (command)
                {
                    case ReelTowerCommands.REQUEST_TOWER_STATE:
                    case ReelTowerCommands.REQUEST_TOWER_STATE_ALL:
                        {
                            statePollingTick = App.TickCount;
                            Interlocked.Exchange(ref receivedStateResponseFlag, 1);
                        }
                        break;
                }
            }

            return true;
        }
        #endregion

        #region Automatic reel load reset
        public virtual void SetReelLoadReset()
        {
            if (requiredLoadCancelIfFailure)
            {
                responseWatcher.Stop();

                if (!string.IsNullOrEmpty(lastRequestedLoadTowerId))
                {
                    ClearReelTowerResponse(true);
                    SendReelLoadReset();
                }

                requiredLoadCancelIfFailure = false;
                lastRequestedLoadTowerId = string.Empty;
            }
        }

        public virtual void SendReelLoadReset()
        {
            SendTowerMessage(ReelTowerCommands.REQUEST_ALL_LOAD_RESET, null, lastRequestedLoadTowerId);
        }
        #endregion

        public void RestartReelTowerManager(int portno)
        {
            Destroy();
            Create(portno);
        }

        public void ResetResponseTimeout()
        {
            ClearMessageQueue();
            ClearReelTowerResponse(true);
        }
        #endregion
    }
}
#endregion