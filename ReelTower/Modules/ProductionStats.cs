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
/// @namespace Marcus.Solution.TechFloor.Components
/// @file ProductionStats.cs
/// @brief
/// @details
/// @date 2019, 5, 21, 오후 1:13
///////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using Marcus.Solution.TechFloor.Object;
using Marcus.Solution.TechFloor.Util;
using System.Runtime.ExceptionServices;
using System.Security;

namespace Marcus.Solution.TechFloor.Components
{
    public class ProductionStats 
    {
        #region Fields
        private string _filename = "";
        string _nodeName = "";
        string _TotalLoadCount = "";
        string _TotalUnloadCount = "";
        string _TotalReturnCount = "";
        string _VisionAlignmentErrorCount = "";
        string _VisionDecodeErrorCount = "";
        string _TotalLoadErrorCount = "";
        string _TotalUnloadErrorCount = "";
        string _TotalReturnErrorCount = "";

        public static int _TotalLoadCountInt = 0;
        public static int _TotalUnloadCountInt = 0;
        public static int _TotalReturnCountInt = 0;
        public static int _VisionAlignmentErrorCountInt = 0;
        public static int _VisionDecodeErrorCountInt = 0;
        public static int _TotalLoadErrorCountInt = 0;
        public static int _TotalUnloadErrorCountInt = 0;
        public static int _TotalReturnErrorCountInt = 0;
        #endregion

        public ProductionStats()
        { 
            try
            {
                // Creating xml file to log status report
                string time = DateTime.Now.ToString("yyyy-MM-dd");
                _filename = String.Format($@"{App.Path}\ProductionStats\{time}.xml");

                // If the directory doesn't exist we will create it here
                if (System.IO.Directory.Exists($@"{App.Path}\ProductionStats") == false)
                    System.IO.Directory.CreateDirectory(($@"{App.Path}\ProductionStats"));

                if (System.IO.File.Exists(_filename))
                {   // reading xml data if it has already been created otherwise it will read all values as null
                    ReadXml();
                    // initializing variables    
                    InitializeVariables();
                }
                else
                {
                    var logFile = System.IO.File.Create(_filename);
                    logFile.Close();                            // Closing the file here otherwise it will throw an exception
                    WriteXml();                                // We need to initialize the file since it will be empty after being created 
                }
            }
            catch (Exception ex)
            {
                Logger.Trace($"{GetType()}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        #region Methods
        
        public void FlushData(bool updateddate = false)
        {
            try
            {
                if (updateddate)
                {
                    var logFile = System.IO.File.Create(_filename = String.Format($@"{App.Path}\ProductionStats\{DateTime.Now.ToString("yyyy-MM-dd")}.xml"));
                    logFile.Close();                            // Closing the file here otherwise it will throw an exception
                    WriteXml();                                // We need to initialize the file since it will be empty after being created 
                }
                else
                {
                    WriteXml();
                }
            }
            catch (Exception ex)
            {
                Logger.Trace($"{GetType()}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        public bool IsChangedDate()
        {
            if (File.Exists(String.Format($@"{App.Path}\ProductionStats\{DateTime.Now.ToString("yyyy-MM-dd")}.xml")))
            {
                return false;
            }
            else
            {
                // _filename = String.Format($@"{App.Path}\ProductionStats\{time}.xml");
                return true;
            }
        }

        // Function for creating xml file
        
        public void WriteXml()
        {
            try
            {
                XmlWriter xmlwriter = XmlWriter.Create(_filename);
                //xmlwriter.Formatting = Formatting.Indented;
                // Document start
                xmlwriter.WriteStartDocument();
                // Job Stats start
                xmlwriter.WriteStartElement("ProductionStats");
                // Total job count start
                xmlwriter.WriteStartElement("JobCount");
                xmlwriter.WriteElementString("TotalLoadCount", _TotalLoadCountInt.ToString());
                xmlwriter.WriteElementString("TotalUnloadCount", _TotalUnloadCountInt.ToString());
                xmlwriter.WriteElementString("TotalReturnCount", _TotalReturnCountInt.ToString());
                //Total job count end
                xmlwriter.WriteEndElement();
                // Total Error Count start
                xmlwriter.WriteStartElement("TotalErrorCount");
                xmlwriter.WriteElementString("VisionAlignmentError", _VisionAlignmentErrorCountInt.ToString());
                xmlwriter.WriteElementString("VisionDecodeError", _VisionDecodeErrorCountInt.ToString());
                xmlwriter.WriteElementString("TotalLoadError", _TotalLoadErrorCountInt.ToString());
                xmlwriter.WriteElementString("TotalUnloadError", _TotalUnloadErrorCountInt.ToString());
                xmlwriter.WriteElementString("TotalReturnError", _TotalReturnErrorCountInt.ToString());
                 //Total Error count end
                xmlwriter.WriteEndElement();
                // Job status end
                xmlwriter.WriteEndElement();
                // Document end
                xmlwriter.WriteEndDocument();
                xmlwriter.Flush();
                xmlwriter.Close();
            }
            catch (Exception ex)
            {
                Logger.Trace($"{GetType()}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        
        public void ReadXml()
        {
            try
            {
                XmlReader reader = XmlReader.Create(_filename);

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (!_nodeName.Equals(reader.Name))
                        {
                            _nodeName = reader.Name;
                        }
                    }

                    if (reader.NodeType == XmlNodeType.Text)
                    {
                        switch (_nodeName)
                        {
                            case "TotalLoadCount":
                                _TotalLoadCount = reader.Value;
                                break;
                            case "TotalUnloadCount":
                                _TotalUnloadCount = reader.Value;
                                break;
                            case "TotalReturnCount":
                                _TotalReturnCount = reader.Value;
                                break;
                            case "VisionAlignmentError":
                                _VisionAlignmentErrorCount = reader.Value;
                                break;
                            case "VisionDecodeError":
                                _VisionDecodeErrorCount = reader.Value;
                                break;
                            case "TotalLoadError":
                                _TotalLoadErrorCount = reader.Value;
                                break;
                            case "TotalUnloadError":
                                _TotalUnloadErrorCount = reader.Value;
                                break;
                            case "TotalReturnError":
                                _TotalReturnErrorCount = reader.Value;
                                break;
                        }
                    }
                }

                reader.Dispose();
            }
            catch (Exception ex)
            {
                Logger.Trace($"{GetType()}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        
        public void InitializeVariables()
        {
            try
            {
                _TotalLoadCountInt = Convert.ToInt32(_TotalLoadCount);
                _TotalUnloadCountInt = Convert.ToInt32(_TotalUnloadCount);
                _TotalReturnCountInt = Convert.ToInt32(_TotalReturnCount);
                _VisionAlignmentErrorCountInt = Convert.ToInt32(_VisionAlignmentErrorCount);
                _VisionDecodeErrorCountInt = Convert.ToInt32(_VisionDecodeErrorCount);
                _TotalLoadErrorCountInt = Convert.ToInt32(_TotalLoadErrorCount);
                _TotalUnloadErrorCountInt = Convert.ToInt32(_TotalUnloadErrorCount);
                _TotalReturnErrorCountInt = Convert.ToInt32(_TotalReturnErrorCount);
            }
            catch (Exception ex)
            {
                Logger.Trace($"{GetType()}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }
        #endregion
    }
}
