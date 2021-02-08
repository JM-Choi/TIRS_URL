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
/// project ProcessWatcher
/// @author Marcus (isadrastea.kor@gmail.com)
/// @namespace ProcessWatcher
/// @file Config.cs
/// @brief
/// @details
/// @date 2020-3-14 오후 2:11 
///////////////////////////////////////////////////////////////////////////////
#endregion

#region Program
using TechFloor.Object;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
#endregion

#region Program
namespace ProcessWatcher
{
    public class Config
    {
        #region Fields
        protected string externalServiceName = string.Empty;

        protected string externalServiceAddress = string.Empty;

        protected string logPath = "Log\\";

        protected string alarmList = string.Empty;

        protected string lineCode = string.Empty;

        protected string processCode = string.Empty;

        protected string equipmentId = string.Empty;

        protected string listenerAddresss = "127.0.0.1";

        protected string cultureCode = "en-US";

        protected int listenerPort = 49151;

        protected RunModes runMode = RunModes.Simulation;

        protected static Dictionary<string, Pair<string, int>> processes = new Dictionary<string, Pair<string, int>>();
        #endregion

        #region Properties
        public string ExternalServiceName => externalServiceName;

        public string ExternalServiceAddress => externalServiceAddress;

        public string LogPath => logPath;

        public string AlarmList
        {
            get => alarmList;
            set => alarmList = value;
        }

        public string LineCode
        {
            get => lineCode;
            set => lineCode = value;
        }

        public string ProcessCode
        {
            get => processCode;
            set => processCode = value;
        }

        public string EquipmentId
        {
            get => equipmentId;
            set => equipmentId = value;
        }

        public string CultureCode
        {
            get => cultureCode;
            set => cultureCode = value;
        }

        public string ListenerAddress
        {
            get => listenerAddresss;
            set => listenerAddresss = value;
        }

        public int ListenerPort
        {
            get => listenerPort;
            set => listenerPort = value;
        }

        public RunModes RunMode
        {
            get => runMode;
            set => runMode = value;
        }

        public IReadOnlyDictionary<string, Pair<string, int>> Processes => processes;
        #endregion

        #region Public methods
        public void AddWatchProcess(string name, string path, int interval)
        {
            try
            {
                if (processes.ContainsKey(name))
                    processes[name] = new Pair<string, int>(path, interval);
                else
                    processes.Add(name, new Pair<string, int>(path, interval));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        public void RemoveWatchProcess(string name)
        {
            try
            {
                if (processes.ContainsKey(name))
                    processes.Remove(name);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        public List<Pair<string, string>> Tokenizer(string str, char[] delimiters = null)
        {
            List<Pair<string, string>> result_ = null;

            try
            {
                if (!string.IsNullOrEmpty(str))
                {
                    result_ = new List<Pair<string, string>>();
                    string[] tokens_ = str.Split(delimiters == null ? new char[] { '=' } : delimiters, StringSplitOptions.RemoveEmptyEntries);

                    if (tokens_.Length > 0)
                        result_.Add(new Pair<string, string>(tokens_[0], tokens_.Length > 1 ? tokens_[1] : string.Empty));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"FormConfig.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return result_;
        }

        public bool Load(string version)
        {
            bool result_ = false;

            try
            {
                string name_ = string.Empty;
                string path_ = string.Empty;
                int inteval_ = 0;

                foreach (string str_ in Properties.Settings.Default.Target)
                {
                    List<Pair<string, string>> tokens_ = Tokenizer(str_);

                    if (tokens_ != null)
                    {
                        foreach (Pair<string, string> item_ in tokens_)
                        {
                            switch (item_.first.ToLower())
                            {
                                case "name":
                                    name_ = item_.second;
                                    break;
                                case "path":
                                    path_ = item_.second;
                                    break;
                                case "interval":
                                    inteval_ = Convert.ToInt32(item_.second);
                                    break;
                            }
                        }
                    }
                }

                if (!processes.ContainsKey(name_))
                    processes.Add(name_, new Pair<string, int>(path_, inteval_));

                foreach (string str_ in Properties.Settings.Default.ExternalService)
                {
                    List<Pair<string, string>> tokens_ = Tokenizer(str_);

                    if (tokens_ != null)
                    {
                        foreach (Pair<string, string> item_ in tokens_)
                        {
                            switch (item_.first.ToLower())
                            {
                                case "name":
                                    externalServiceName = item_.second;
                                    break;
                                case "address":
                                    externalServiceAddress = item_.second;
                                    break;
                                case "line":
                                    lineCode = item_.second;
                                    break;
                                case "process":
                                    processCode = item_.second;
                                    break;
                                case "id":
                                    equipmentId = item_.second;
                                    break;
                            }
                        }
                    }
                }

                foreach (string str_ in Properties.Settings.Default.Listener)
                {
                    List<Pair<string, string>> tokens_ = Tokenizer(str_);

                    if (tokens_ != null)
                    {
                        foreach (Pair<string, string> item_ in tokens_)
                        {
                            switch (item_.first.ToLower())
                            {
                                case "address":
                                    listenerAddresss = item_.second;
                                    break;
                                case "port":
                                    listenerPort = Convert.ToInt32(item_.second);
                                    break;
                            }
                        }
                    }
                }

                cultureCode = Properties.Settings.Default.CultureCode;
                alarmList = Properties.Settings.Default.AlarmList;
                logPath = Properties.Settings.Default.LogPath;
                runMode = (RunModes)Enum.Parse(typeof(RunModes), Properties.Settings.Default.RunMode);

                if (!string.IsNullOrEmpty(alarmList) && File.Exists(alarmList))
                {
                    Program.AlarmManagerObject.Load(alarmList, cultureCode);
                }

                result_ = processes.Count > 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"FormConfig.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return result_;
        }

        public void Save()
        {
            try
            {
                Properties.Settings.Default.Target.Clear();
                foreach (KeyValuePair<string, Pair<string, int>> item_ in processes)
                {
                    Properties.Settings.Default.Target.Add($"name={item_.Key}");
                    Properties.Settings.Default.Target.Add($"path={item_.Value.first}");
                    Properties.Settings.Default.Target.Add($"interval={item_.Value.second}");
                }

                Properties.Settings.Default.ExternalService.Clear();
                Properties.Settings.Default.ExternalService.Add($"name={externalServiceName}");
                Properties.Settings.Default.ExternalService.Add($"address={externalServiceAddress}");
                Properties.Settings.Default.ExternalService.Add($"line={lineCode}");
                Properties.Settings.Default.ExternalService.Add($"process={processCode}");
                Properties.Settings.Default.ExternalService.Add($"id={equipmentId}");

                Properties.Settings.Default.Listener.Clear();
                Properties.Settings.Default.Listener.Add($"address={listenerAddresss}");
                Properties.Settings.Default.Listener.Add($"port={listenerPort}");

                if (Properties.Settings.Default.CultureCode != cultureCode)
                {
                    Properties.Settings.Default.CultureCode = cultureCode;
                    Program.AlarmManagerObject.Load(alarmList, cultureCode);
                }

                Properties.Settings.Default.RunMode = runMode.ToString();
                Properties.Settings.Default.AlarmList = alarmList;
                Properties.Settings.Default.LogPath = logPath;
                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"FormConfig.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }
        #endregion
    }
}
#endregion