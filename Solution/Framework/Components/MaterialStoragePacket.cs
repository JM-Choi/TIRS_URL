#region Imports
using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Timers;
using System.Xml;
using TechFloor.Device.CommunicationIo;
using TechFloor.Service.MYCRONIC.WebService;
#endregion

#region Program
namespace TechFloor.Object
{
    public class MaterialStoragePacket : AbstractClassDisposable
    {
        #region Fields
        protected delegate void Send(MaterialStoragePacket arg);

        protected int limitOfRetry = 3;

        protected System.Timers.Timer watcher = null;
        #endregion

        #region Properties
        public virtual DateTime SentTime
        {
            get;
            set;
        }

        public virtual int Retry
        {
            get;
            set;
        }

        public virtual bool PrefixUsage
        {
            get;
            protected set;
        }

        public virtual ReelTowerCommands Command
        {
            get;
            protected set;
        }

        public virtual object Data
        {
            get;
            protected set;
        }

        public virtual string TowerId
        {
            get;
            set;
        }

        public virtual string State
        {
            get;
            set;
        }

        public virtual string Code
        {
            get;
            set;
        }

        public virtual string Message
        {
            get;
            set;
        }

        public virtual bool IsOverTimeout(int timeout = 1000) => (DateTime.Now - SentTime).TotalMilliseconds > timeout;

        public virtual bool IsOverRetry(int limit) => Retry > limit;

        public virtual bool IsMatched(ReelTowerCommands command, string towerid) => this.Command == command && this.TowerId == towerid;
        #endregion

        #region Events
        public virtual event EventHandler<int> RetrySendMessage;
        #endregion

        #region Constructors
        public MaterialStoragePacket(ReelTowerCommands command, object data = null, string towerid= null, string state= null, string code = "0", string message = "done", bool prefix = true, int retry = 0, int limitofretry = 3, int timeout = 0, EventHandler<int> func = null)
        {
            this.SentTime       = DateTime.Now;
            this.Command        = command;
            this.Data           = data;
            this.TowerId        = towerid;
            this.State          = state;
            this.Code           = string.IsNullOrEmpty(code)? "0" : code;
            this.Message        = message;
            this.PrefixUsage    = prefix;
            this.Retry          = retry;

            if (timeout > 0)
            {
                watcher             = new System.Timers.Timer(timeout);
                watcher.AutoReset   = false;
                watcher.Elapsed     += OnElapsedEventHandler;
                this.limitOfRetry   = limitofretry;
            }

            if (func != null)
                RetrySendMessage += func;
        }

        public MaterialStoragePacket(MaterialStoragePacket src, bool enabled = false)
        {
            CopyFrom(src);
            this.SentTime = DateTime.Now;

            if (enabled)
                EnableWatcher(true);
        }
        #endregion

        #region Protected methods
        protected override void DisposeManagedObjects()
        {
            EnableWatcher(false);
            base.DisposeManagedObjects();
        }

        protected virtual void MakeXmlHeader(ref XmlDocument xml, ref XmlNode root, string messageName)
        {
            string transactionID    = DateTime.Now.ToString("yyyyMMddhhmmssff");
            string timeStamp        = DateTime.Now.ToString("yyyyMMddhhmmss");

            XmlNode nodeHeader = xml.CreateElement("header");
            AddXmlItem(xml, nodeHeader, "messageName", messageName);
            AddXmlItem(xml, nodeHeader, "transactionId", transactionID);
            AddXmlItem(xml, nodeHeader, "timeStamp", timeStamp);
            root.AppendChild(nodeHeader);
        }

        protected virtual void AddXmlItem(XmlDocument xml, XmlNode parentsNode, string key, string value)
        {
            XmlNode node            = xml.CreateElement(key);
            node.InnerText          = value;
            parentsNode.AppendChild(node);
        }

        protected virtual void AddXmlOfBarcode(ref XmlDocument xml, ref XmlNode body, MaterialData barcode)
        {
            XmlNode node = xml.CreateElement("BARCODEINFO");
            AddXmlItem(xml, node, "SID", barcode.Category);
            AddXmlItem(xml, node, "LOTID", barcode.LotId);
            AddXmlItem(xml, node, "SUPPLIER", barcode.Supplier);
            AddXmlItem(xml, node, "QTY", barcode.Quantity.ToString());
            AddXmlItem(xml, node, "MFG", barcode.ManufacturedDatetime);
            AddXmlItem(xml, node, "LOADTYPE", barcode.LoadType.ToString().ToUpper());

            switch (barcode.ReelThickness)
            {
                case ReelThicknesses.ReelThickness4:
                    AddXmlItem(xml, node, "SIZE", "7");
                    break;
                default:
                    {
                        switch (barcode.ReelType)
                        {
                            default:
                            case ReelDiameters.ReelDiameter4:
                                AddXmlItem(xml, node, "SIZE", "4");
                                break;
                            case ReelDiameters.ReelDiameter7:
                                AddXmlItem(xml, node, "SIZE", "7");
                                break;
                            case ReelDiameters.ReelDiameter13:
                                AddXmlItem(xml, node, "SIZE", "13");
                                break;
                            case ReelDiameters.ReelDiameter15:
                                AddXmlItem(xml, node, "SIZE", "15");
                                break;
                        }
                    }
                    break;
            }
            body.AppendChild(node);
        }

        protected virtual void OnElapsedEventHandler(object sender, ElapsedEventArgs e)
        {
            if (!IsOverRetry(limitOfRetry))
                FireRetrySentMessage();
        }
        #endregion

        #region Public methods
        public virtual string CreatePacket()
        {
            MaterialData barcode_   = null;
            XmlDocument xml         = new XmlDocument();
            XmlNode header          = xml.CreateElement("message");
            XmlNode body            = xml.CreateElement("body");
            XmlNode tail            = xml.CreateElement("return");

            switch (Command)
            {
                case ReelTowerCommands.REQUEST_LINK_TEST:
                case ReelTowerCommands.REQUEST_LOAD_RESET:
                case ReelTowerCommands.REQUEST_UNLOAD_RESET:
                case ReelTowerCommands.REQUEST_ALL_LOAD_RESET:
                case ReelTowerCommands.REQUEST_ALL_UNLOAD_RESET:
                case ReelTowerCommands.REQUEST_ALL_ALARM_RESET:
                    break;
                case ReelTowerCommands.SEND_PICKING_LIST:
                    {
                        if (Data != null)
                        {
                            int count_      = 0;
                            string items_   = string.Empty;
                            Pair<string, ProvideJobListData> job_ = Data as Pair<string, ProvideJobListData>;

                            foreach (ProvideMaterialData item_ in job_.second.Materials)
                            {
                                items_ += $"{item_.Name};{item_.Category};{item_.LotId};{item_.Supplier};{item_.ManufacturedDatetime};{item_.Quantity}|";
                                count_++;
                            }

                            AddXmlItem(xml, body, "PICK_ID", string.IsNullOrEmpty(job_.second.User)? job_.first : $"{job_.first};{job_.second.User}");
                            AddXmlItem(xml, body, "TARGET_LOCATION", $"Output_No_{job_.second.Outport}");
                            AddXmlItem(xml, body, "COUNT", (count_ == job_.second.Reels ? job_.second.Reels.ToString() : count_.ToString()));
                            AddXmlItem(xml, body, "UID_LIST", items_);
                        }
                    }
                    break;
                case ReelTowerCommands.REQUEST_UNLOAD_COMPLETE:
                    {
                        if (Data != null)
                        {
                            barcode_ = Data as ProvideMaterialData;
                            AddXmlItem(xml, body, "MATERIALTOWERID", TowerId);
                            AddXmlItem(xml, body, "UID", barcode_.Name);
                            AddXmlItem(xml, body, "STAGE", barcode_.Text);
                            AddXmlItem(xml, tail, "returnCode", Code);
                            AddXmlItem(xml, tail, "returnMessage", Message);
                        }
                    }
                    break;
                case ReelTowerCommands.REPLY_TOWER_STATE:
                    {
                        AddXmlItem(xml, body, "MATERIALTOWERID", TowerId);
                        AddXmlItem(xml, body, "MATERIALTOWERSTATE", State);
                        AddXmlItem(xml, tail, "returnCode", Code);
                        AddXmlItem(xml, tail, "returnMessage", Message);
                    }
                    break;
                case ReelTowerCommands.REPLY_REEL_LOAD_MOVE:
                    {
                        AddXmlItem(xml, body, "MATERIALTOWERID", TowerId);
                        AddXmlItem(xml, body, "LOADSTATE", State);

                        if (Data != null)
                            barcode_ = Data as MaterialData;

                        AddXmlItem(xml, body, "UID", (Data == null ? string.Empty : barcode_.Name));
                        AddXmlItem(xml, tail, "returnCode", Code);
                        AddXmlItem(xml, tail, "returnMessage", Message);
                    }
                    break;
                case ReelTowerCommands.REPLY_BARCODEINFO_CONFIRM:
                    {
                        AddXmlItem(xml, body, "MATERIALTOWERID", TowerId);

                        if (Data != null)
                        {
                            barcode_ = Data as MaterialData;
                            AddXmlItem(xml, body, "UID", barcode_.Name);
                            AddXmlOfBarcode(ref xml, ref body, barcode_);
                        }
                        else
                            Code = "1";

                        AddXmlItem(xml, tail, "returnCode", Code);
                        
                        if (barcode_.ReelThickness == ReelThicknesses.Unknown)
                            AddXmlItem(xml, tail, "returnMessage", Message);
                        else
                            AddXmlItem(xml, tail, "returnMessage", barcode_.ReelThickness.ToString());
                    }
                    break;
                case ReelTowerCommands.REPLY_REEL_UNLOAD_MOVE:
                    {
                        if (Data != null)
                        {
                            barcode_ = Data as MaterialData;
                            AddXmlItem(xml, body, "MATERIALTOWERID", TowerId);
                            AddXmlItem(xml, body, "LOADSTATE", "UNLOAD");
                            AddXmlItem(xml, body, "UID", barcode_.Name);
                            AddXmlItem(xml, tail, "returnCode", Code);
                            AddXmlItem(xml, tail, "returnMessage", Message);
                        }
                    }
                    break;
                case ReelTowerCommands.REPLY_REEL_UNLOAD_ASSIGN:
                    {
                        if (Data != null)
                        {
                            barcode_ = Data as MaterialData;
                            AddXmlItem(xml, body, "MATERIALTOWERID", TowerId);
                            AddXmlItem(xml, body, "UID", barcode_.Name);
                            AddXmlItem(xml, tail, "returnCode", Code);
                            AddXmlItem(xml, tail, "returnMessage", Message);
                        }
                    }
                    break;
                case ReelTowerCommands.REPLY_LOAD_COMPLETE:
                case ReelTowerCommands.REPLY_UNLOAD_COMPLETE:
                    {
                        if (Data != null)
                        {
                            barcode_ = Data as MaterialData;
                            AddXmlItem(xml, body, "MATERIALTOWERID", TowerId);
                            AddXmlItem(xml, body, "UID", barcode_.Name);
                            AddXmlItem(xml, tail, "returnCode", Code);
                            AddXmlItem(xml, tail, "returnMessage", Message);
                        }
                    }
                    break;
                default:
                    return string.Empty;
            }

            MakeXmlHeader(ref xml, ref header, Command.ToString());
            header.AppendChild(body);
            header.AppendChild(tail);
            xml.AppendChild(header);
            return (PrefixUsage ? string.Concat((char)AsciiControlCharacters.Stx, xml.OuterXml, (char)AsciiControlCharacters.Etx) : xml.OuterXml);
        }

        public virtual void FireRetrySentMessage()
        {
            RetrySendMessage?.Invoke(this, Retry);
        }

        public virtual void EnableWatcher(bool state)
        {
            if (watcher != null)
            {
                if (!(watcher.Enabled = state))
                {
                    watcher.Dispose();
                    watcher = null;
                }
            }
        }

        public virtual void CopyFrom(MaterialStoragePacket src)
        {
            this.SentTime       = src.SentTime;
            this.Command        = src.Command;
            this.Data           = src.Data;
            this.TowerId        = src.TowerId;
            this.State          = src.State;
            this.Code           = src.Code;
            this.Message        = src.Message;
            this.PrefixUsage    = src.PrefixUsage;
            this.Retry          = src.Retry;

            if (src.watcher != null)
            {
                watcher             = new System.Timers.Timer(src.watcher.Interval);
                watcher.AutoReset   = false;
                watcher.Elapsed     += OnElapsedEventHandler;
                this.limitOfRetry   = src.limitOfRetry;
            }

            if (src.RetrySendMessage != null)
                this.RetrySendMessage += src.RetrySendMessage;
        }
        #endregion
    }
}
#endregion