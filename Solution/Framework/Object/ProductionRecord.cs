#region Imports
using System;
using System.IO;
using System.Reflection;
using System.Xml;
using TechFloor.Util;
#endregion

#region Program
namespace TechFloor.Components
{
    public class ProductionRecord 
    {
        #region Fields
        private string _filename= null;
        string _nodeName= null;
        string _TotalLoadCount= null;
        string _TotalUnloadCount= null;
        string _TotalReturnCount= null;
        string _VisionAlignmentErrorCount= null;
        string _VisionDecodeErrorCount= null;
        string _TotalLoadErrorCount= null;
        string _TotalUnloadErrorCount= null;
        string _TotalReturnErrorCount= null;

        public static int _TotalLoadCountInt = 0;
        public static int _TotalUnloadCountInt = 0;
        public static int _TotalReturnCountInt = 0;
        public static int _VisionAlignmentErrorCountInt = 0;
        public static int _VisionDecodeErrorCountInt = 0;
        public static int _TotalLoadErrorCountInt = 0;
        public static int _TotalUnloadErrorCountInt = 0;
        public static int _TotalReturnErrorCountInt = 0;
        #endregion

        #region Constructors
        public ProductionRecord()
        { 
            try
            {
                // Creating xml file to log status report
                string time = DateTime.Now.ToString("yyyy-MM-dd");
                _filename = String.Format($@"{App.Path}ProductionRecord\{time}.xml");

                // If the directory doesn't exist we will create it here
                if (Directory.Exists($@"{App.Path}ProductionRecord") == false)
                    Directory.CreateDirectory(($@"{App.Path}ProductionRecord"));

                // D:\Projects\TechFloor\SMT\ReelTower\bin\x64\Debug\ProductionRecord
                if (File.Exists(_filename))
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
        #endregion

        #region Methods

        public void FlushData(bool updateddate = false)
        {
            try
            {
                if (updateddate)
                {
                    var logFile = System.IO.File.Create(_filename = String.Format($@"{App.Path}ProductionRecord\{DateTime.Now.ToString("yyyy-MM-dd")}.xml"));
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
            if (File.Exists(String.Format($@"{App.Path}\ProductionRecord\{DateTime.Now.ToString("yyyy-MM-dd")}.xml")))
            {
                return false;
            }
            else
            {
                // _filename = String.Format($@"{App.Path}\ProductionRecord\{time}.xml");
                return true;
            }
        }

        // Function for creating xml file
        
        public void WriteXml()
        {
            try
            {
                XmlWriterSettings xmlset = new XmlWriterSettings();
                xmlset.Indent = true;
                XmlWriter xmlwriter = XmlWriter.Create(_filename, xmlset);
                // Document start
                xmlwriter.WriteStartDocument();
                // Job Stats start
                xmlwriter.WriteStartElement("ProductionRecord");
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
            XmlReader reader = null;

            try
            {
                if (File.Exists(_filename))
                {
                    reader = XmlReader.Create(_filename);

                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            if (_nodeName == null || !_nodeName.Equals(reader.Name))
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
                }
            }
            catch (Exception ex)
            {
                Logger.Trace($"{GetType()}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();
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
#endregion