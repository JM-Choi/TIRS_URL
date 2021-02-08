#region Imports
using System;
using System.Collections.Generic;
using System.Threading;
using TechFloor;
#endregion

#region Program
namespace TechFloor
{
    #region Enumerations
    public enum ReelDiameters : int
    {
        Unknown,
        ReelDiameter4 = 4,
        ReelDiameter7 = 7,
        ReelDiameter13 = 13,
        ReelDiameter15 = 15
    }

    public enum ReelThicknesses : int
    {
        Unknown,
        ReelThickness4,
        ReelThickness8,
        ReelThickness12,
        ReelThickness16,
        ReelThickness24,
        ReelThickness32,
        ReelThickness44,
        ReelThickness56,
        ReelThickness72,
    }

    public enum MaterialIdentifiers : int
    {
        Article,
        Carrier,
        Lot,
        Supplier,
        ManufacturedDatetime,
        EntryDatetime,
    }

    public enum MaterialTypes : int
    {
        Reel,
        Tray,
        Carrier,
        Magazine,
        Box,
        Bucket
    }

    public enum LoadMaterialTypes : int
    {
        Cart,
        Return
    }

    public enum ReelTowerCommands
    {
        SEND_PICKING_LIST,

        REQUEST_LINK_TEST,
        REQUEST_TOWER_STATE,
        REQUEST_TOWER_STATE_ALL,
        REQUEST_REEL_LOAD_MOVE,
        REQUEST_BARCODEINFO_CONFIRM,
        REQUEST_REEL_LOAD_ASSIGN,
        REQUEST_LOAD_COMPLETE,
        REQUEST_UNLOAD_COMPLETE,
        REQUEST_REEL_UNLOAD_MOVE,
        REQUEST_REEL_UNLOAD_ASSIGN,
        REQUEST_LOAD_RESET,
        REQUEST_UNLOAD_RESET,
        REQUEST_ALL_LOAD_RESET,
        REQUEST_ALL_UNLOAD_RESET,
        REQUEST_ALL_ALARM_RESET,
        REQUEST_ALL_STOP,
        REQUEST_CART_LOAD_FINISH,

        REPLY_LINK_TEST,
        REPLY_TOWER_STATE,
        REPLY_TOWER_STATE_ALL,
        REPLY_REEL_LOAD_MOVE,
        REPLY_BARCODEINFO_CONFIRM,        
        REPLY_REEL_LOAD_ASSIGN,
        REPLY_REEL_UNLOAD_MOVE,
        REPLY_REEL_UNLOAD_ASSIGN,
        REPLY_LOAD_COMPLETE,
        REPLY_UNLOAD_COMPLETE,
        REPLY_LOAD_RESET,
        REPLY_UNLOAD_RESET,
        REPLY_ALL_LOAD_RESET,
        REPLY_ALL_UNLOAD_RESET,
        REPLY_ALL_ALARM_RESET,
        REPLY_ALL_STOP,

        REPORT_SET_ALARM,
        REPORT_CLEAR_ALARM,
    }
    #endregion

    public class MaterialCategoryData
    {

    }

    public class MaterialData
    {
        #region Fields
        #endregion

        #region Properties
        public virtual string TowerId
        {
            get;
            set;
        }

        public virtual string Category
        {
            get;
            set;
        }

        public virtual string Name
        {
            get;
            set;
        }

        public virtual string LotId
        {
            get;
            set;
        }

        public virtual string Supplier
        {
            get;
            set;
        }

        public virtual int Quantity
        {
            get;
            set;
        }

        public virtual string ManufacturedDatetime
        {
            get;
            set;
        }

        public virtual string Text
        {
            get;
            set;
        }

        public virtual string Comment
        {
            get;
            set;
        }

        public virtual LoadMaterialTypes LoadType
        {
            get;
            set;
        }

        public virtual ReelDiameters ReelType
        {
            get;
            set;
        }

        public virtual ReelThicknesses ReelThickness
        {
            get;
            set;
        }

        public virtual DateTime EventDateTime
        {
            get;
            set;
        }

        public virtual int Size
        {
            get;
            set;
        }

        public bool IsValid => (Category.Length == 9 &&!string.IsNullOrEmpty(Category) && !string.IsNullOrEmpty(LotId) && !string.IsNullOrEmpty(ManufacturedDatetime) && !string.IsNullOrEmpty(Name));
        #endregion

        #region Constructors
        public MaterialData() { }

        public MaterialData(MaterialData src)
        {
            CopyFrom(src);
        }

        public MaterialData(string towerid, string articlename, string carriername, string lot, string supplier, string mfg, int size = 7, string data= null, string comment ="", int qty = 0, ReelDiameters reeltype = ReelDiameters.ReelDiameter7, ReelThicknesses reelthick = ReelThicknesses.ReelThickness12, LoadMaterialTypes loadtype = LoadMaterialTypes.Cart)
        {
            SetValues(towerid, articlename, carriername, lot, supplier, mfg, size, data, comment, qty, reeltype, reelthick, loadtype);
            EventDateTime = DateTime.Now;
        }
        #endregion

        #region Public methods
        public void SetName(string carriername, bool reset = true)
        {
            if (reset)
                Clear();

            Name = carriername;
        }

        public void SetValues(string towerid, string articlename, string carriername, string lot, string supplier, string mfg, int size = 7, string data= null, string comment= null, int qty = 0, ReelDiameters reeltype = ReelDiameters.ReelDiameter7, ReelThicknesses reelthick = ReelThicknesses.ReelThickness12, LoadMaterialTypes loadtype = LoadMaterialTypes.Cart)
        {
            TowerId = towerid;
            Name = carriername;
            Category = articlename;
            LotId = lot;
            Supplier = supplier;
            Quantity = qty;
            ManufacturedDatetime = mfg;
            Text = data;
            Comment = comment;
            LoadType = loadtype;
            ReelType = reeltype;
            ReelThickness = reelthick;
            Size = size;
        }

        public void SetValues(string articlename, string carriername, string lot, string supplier, string mfg, string data= null, string comment= null, int qty = 0, ReelDiameters reeltype = ReelDiameters.ReelDiameter7, ReelThicknesses reelthick = ReelThicknesses.ReelThickness12, LoadMaterialTypes loadtype = LoadMaterialTypes.Cart)
        {
            Name = carriername;
            Category = articlename;
            LotId = lot;
            Supplier = supplier;
            Quantity = qty;
            ManufacturedDatetime = mfg;
            Text = data;
            Comment = comment;
            LoadType = loadtype;
            ReelType = reeltype;
            ReelThickness = reelthick;
        }

        public void SetLoadType(LoadMaterialTypes loadtype)
        {
            LoadType = loadtype;
        }

        public void SetReelType(ReelDiameters reeltype)
        {
            ReelType = reeltype;
        }

        public void SetReelInformation(LoadMaterialTypes loadtype, ReelDiameters reeltype, ReelThicknesses reelthick = ReelThicknesses.ReelThickness16)
        {
            LoadType = loadtype;
            ReelType = reeltype;
            ReelThickness = reelthick;
        }

        public void Clear()
        {
            TowerId = string.Empty;
            Category = string.Empty;
            LotId = string.Empty;
            Supplier = string.Empty;
            Quantity = 0;
            Name = string.Empty;
            ManufacturedDatetime = string.Empty;
            Text = string.Empty;
            Comment = string.Empty;
            LoadType = LoadMaterialTypes.Cart;
        }

        public void CopyFrom(MaterialData src)
        {
            TowerId = src.TowerId;
            Category = src.Category;
            LotId = src.LotId;
            Supplier = src.Supplier;
            Quantity = src.Quantity;
            Name = src.Name;
            ManufacturedDatetime = src.ManufacturedDatetime;
            Text = src.Text;
            Comment = src.Comment;
            LoadType = src.LoadType;
            ReelType = src.ReelType;
            ReelThickness = src.ReelThickness;
            EventDateTime = src.EventDateTime;
            Size = src.Size;
        }
        #endregion
    }

    public struct MaterialStorageMessage
    {
        #region Fields
        public string Name;
        public string TransactionID;
        public string TimeStamp;
        public string TowerId;
        public string TowerState;
        public string LoadState;
        public string ReturnCode;
        public string ReturnMessage;
        public string OutputStage;
        public string ErrorMessage;
        public MaterialData Data;
        #endregion

        public void CreateData()
        {
            Data = new MaterialData();
        }

        public void CopyFrom(MaterialStorageMessage src)
        {
            Name            = src.Name;
            TransactionID   = src.TransactionID;
            TimeStamp       = src.TimeStamp;
            TowerId         = src.TowerId;
            TowerState      = src.TowerState;
            LoadState       = src.LoadState;
            ReturnCode      = src.ReturnCode;
            ReturnMessage   = src.ReturnMessage;
            OutputStage     = src.OutputStage;
            ErrorMessage    = src.ErrorMessage;

            if (src.Data != null)
            {
                if (Data == null)
                    CreateData();

                Data.CopyFrom(src.Data);
            }
        }

        public void Clear()
        {
            Name            = string.Empty;
            TransactionID   = string.Empty;
            TimeStamp       = string.Empty;
            TowerId         = string.Empty;
            TowerState      = string.Empty;
            LoadState       = string.Empty;
            ReturnCode      = string.Empty;
            ReturnMessage   = string.Empty;
            OutputStage     = string.Empty;
            ErrorMessage    = string.Empty;

            if (Data != null)
                Data.Clear();
        }
    }
}
#endregion