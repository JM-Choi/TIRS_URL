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
/// @file AlarmManager.cs
/// @brief
/// @details
/// @date 2020-3-14 오후 6:10 
///////////////////////////////////////////////////////////////////////////////
#endregion


#region Imports
using TechFloor.Object;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml;
#endregion

#region Program
namespace ProcessWatcher
{
    public class AlarmManager
    {
        #region Fields
        protected string cultureCode;

        protected Dictionary<int, AlarmData> alarmList = new Dictionary<int, AlarmData>();
        #endregion

        #region Properties
        public string CultureCode => cultureCode;

        public IReadOnlyDictionary<int, AlarmData> AlarmList => alarmList;
        #endregion

        #region Constructors
        public AlarmManager(string culturecode = "en-US")
        {
            this.cultureCode = culturecode;
        }
        #endregion

        #region Public methods
        public virtual void SetCulture(string culturecode)
        {
            this.cultureCode = culturecode;
        }

        public virtual bool AddAlarmData(int code, string name, SeverityLevels level, string message, bool enabled, bool report = false, string extra = null, string module = null, string description = null, string cause = null, string remedy = null)
        {
            if (!alarmList.ContainsKey(code))
            {
                alarmList.Add(code, new AlarmData(code, name, level, message, enabled, report, extra, module, description, cause, remedy));
                return true;
            }

            return false;
        }

        public virtual string GetAlarmMessage(int code)
        {
            if (alarmList.ContainsKey(code))
                return alarmList[code].Message;

            return "The alarm code is not defined.";
        }

        public virtual AlarmData GetAlarmData(int code)
        {
            if (alarmList.ContainsKey(code))
                return alarmList[code];

            return null;
        }

        public virtual void Clear()
        {
            alarmList.Clear();
        }

        public virtual void Load(string file, string culturecode)
        {
            try
            {
                Clear();

                if (File.Exists(file))
                {
                    XmlDocument xml = new XmlDocument();
                    xml.Load(file);

                    if (!string.IsNullOrEmpty(culturecode))
                        cultureCode = culturecode;

                    foreach (XmlNode element_ in xml.DocumentElement.ChildNodes)
                    {
                        switch (element_.Name.ToLower())
                        {
                            case "alarm":
                                {
                                    bool enabled_ = false;
                                    bool report_ = false;
                                    int code_ = 0;
                                    string extra_ = string.Empty;
                                    string name_ = string.Empty;
                                    string message_ = string.Empty;
                                    string remedy_ = string.Empty;
                                    SeverityLevels severity_ = SeverityLevels.Low;

                                    foreach (XmlAttribute attr_ in element_.Attributes)
                                    {
                                        switch (attr_.Name.ToLower())
                                        {
                                            case "id":
                                                code_ = Convert.ToInt32(attr_.Value);
                                                break;
                                            case "extra":
                                                extra_ = attr_.Value;
                                                break;
                                            case "name":
                                                name_ = attr_.Value;
                                                break;
                                            case "enabled":
                                                enabled_ = Convert.ToBoolean(attr_.Value);
                                                break;
                                            case "report":
                                                report_ = Convert.ToBoolean(attr_.Value);
                                                break;
                                            case "severity":
                                                severity_ = (SeverityLevels)Enum.Parse(typeof(SeverityLevels), attr_.Value);
                                                break;
                                        }
                                    }

                                    foreach (XmlNode child_ in element_.ChildNodes)
                                    {
                                        if (child_.Attributes["culture"].Value == cultureCode)
                                        {
                                            foreach (XmlNode node_ in child_.ChildNodes)
                                            {
                                                switch (node_.Name.ToLower())
                                                {
                                                    case "message":
                                                        message_ = node_.InnerText;
                                                        break;
                                                    case "remedy":
                                                        remedy_ = node_.InnerText;
                                                        break;
                                                }
                                            }

                                            AddAlarmData(code_, name_, severity_, message_, enabled_, report_, extra_, string.Empty, string.Empty, string.Empty, remedy_);
                                            break;
                                        }
                                    }
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }
        #endregion
    }
}
#endregion
