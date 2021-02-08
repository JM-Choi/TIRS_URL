#region Imports
using TechFloor.Object;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
#endregion

#region Program
namespace TechFloor.Service.MYCRONIC.WebService
{
    public class ProvideMaterialData : MaterialData
    {
        #region Enumerations
        public enum States
        {
            Unknown = -1,
            Stored,
            Ready,
            Providing,
            Completed
        }
        #endregion

        #region Properties
        public string TowerName
        {
            get;
            set;
        }

        public string Depot
        {
            get;
            set;
        }

        public States State
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public ProvideMaterialData(string towerid= null, string category= null, string name= null, string manufacturer= null, string lotid= null, int count = 0, string depot= null, string mfg= null, States state = States.Ready)
        {
            TowerId     = towerid;
            Category    = category;
            Name        = name;
            Supplier    = manufacturer;
            LotId       = lotid;
            Quantity    = count;
            Depot       = depot;
            ManufacturedDatetime = mfg;
            State       = state;
        }

        public ProvideMaterialData(params string[] data)
        {
            State = States.Ready;

            try
            {
                for (int i_ = 0; i_ < data.Length; i_++)
                {
                    switch (i_)
                    {
                        case 0:
                            {
                                Category = data[i_].ToString();
                            }
                            break;
                        case 1:
                            {
                                Name = data[i_].ToString();
                            }
                            break;
                        case 2:
                            {
                                Supplier = data[i_].ToString();
                            }
                            break;
                        case 3:
                            {
                                if (App.IsDigit(data[i_].ToString()))
                                    Quantity = Convert.ToInt32(data[i_]);
                            }
                            break;
                        case 4:
                            {
                                Depot = data[i_].ToString();

                                if (!string.IsNullOrEmpty(Depot) && Depot[0] == 'T' && Depot.Length >= 7)
                                    TowerId = Depot.Substring(1, 6);
                            }
                            break;
                        case 5:
                            {
                                if (App.IsDigit(data[i_].ToString()))
                                    State = (ProvideMaterialData.States)Convert.ToInt32(data[i_].ToString());
                                else
                                    State = (ProvideMaterialData.States)Enum.Parse(typeof(ProvideMaterialData.States), data[i_].ToString());
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        public ProvideMaterialData(ProvideMaterialData src)
        {
            CopyFrom(src);
        }

        public ProvideMaterialData(MaterialData src)
        {
            CopyFrom(src);
        }
        #endregion

        #region Public methods
        public void Update(ProvideMaterialData src)
        {
            TowerId     = src.TowerId;
            Category    = src.Category;
            Name        = src.Name;
            Quantity    = src.Quantity;
            State       = src.State;
            LotId       = src.LotId;
            ManufacturedDatetime = src.ManufacturedDatetime;

            if (!string.IsNullOrEmpty(src.Supplier))
                Supplier = src.Supplier;

            if (!string.IsNullOrEmpty(src.Depot))
                Depot = src.Depot;
        }

        public void CopyFrom(ProvideMaterialData src)
        {
            TowerId     = src.TowerId;
            Category    = src.Category;
            Name        = src.Name;
            Supplier    = src.Supplier;
            Quantity    = src.Quantity;
            Depot       = src.Depot;
            State       = src.State;
            LotId       = src.LotId;
            ManufacturedDatetime = src.ManufacturedDatetime;
        }
        #endregion
    }

    public class ProvideJobListData
    {
        #region Enumerations
        public enum States
        {
            Unknown,
            Created,
            Ready,
            Providing,
            Canceled,
            Completed,
            Failed,
            Deleted
        }
        #endregion

        #region Fields
        protected List<ProvideMaterialData> materials = new List<ProvideMaterialData>();
        #endregion

        #region Properties
        public string Name
        {
            get;
            set;
        }

        public string User
        {
            get;
            set;
        }

        public int Outport
        {
            get;
            set;
        }

        public int Reels
        {
            get;
            set;
        }

        public States State
        {
            get;
            set;
        }

        public IReadOnlyList<ProvideMaterialData> Materials => materials;

        public DateTime RegisteredDateTime
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public ProvideJobListData(string name, string user, int outport, int reels, States state = States.Created)
        {
            this.Name        = name;
            this.User        = user;
            this.Outport     = outport;
            this.Reels       = reels;
            this.State       = state;
            this.RegisteredDateTime = DateTime.Now;
        }

        public ProvideJobListData(string name, string user, int outport, int reels, States state, IReadOnlyList<ProvideMaterialData> data)
        {
            this.Name        = name;
            this.User        = user;
            this.Outport     = outport;
            this.Reels       = reels;
            this.State       = state;
            this.RegisteredDateTime = DateTime.Now;

            foreach (ProvideMaterialData item_ in data)
                materials.Add(new ProvideMaterialData(item_));
        }

        public ProvideJobListData(string name, string user, int outport, int reels, States state, DataSet data)
        {
            this.Name        = name;
            this.User        = user;
            this.Outport     = outport;
            this.Reels       = reels;
            this.State       = state;
            this.RegisteredDateTime = DateTime.Now;

            SetMaterialData(data);
        }

        public ProvideJobListData(ProvideJobListData src)
        {
            CopyFrom(src);
        }
        #endregion

        #region Public methods
        public void SetMaterialData(DataSet data)
        {
            try
            {
                if (data != null)
                {
                    DataTable tb_ = data.Tables[0];
                    List<string> values_ = new List<string>();

                    foreach (DataRow row_ in tb_.Rows)
                        materials.Add(new ProvideMaterialData(Array.ConvertAll(row_.ItemArray, x_ => x_.ToString())));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        public ProvideMaterialData GetMaterialData(string articlename, string carriername)
        {
            return materials.Find(x_ => x_.Category == articlename && x_.Name == carriername);
        }

        public void RemoveMaterialData(List<string> items)
        {
            if (items != null)
            {
                foreach (string item_ in items)
                {
                    ProvideMaterialData data_ = materials.Find(x_ => x_.Name == item_);

                    if (data_ != null)
                        materials.Remove(data_);
                }
            }
        }

        public void RemoveMaterialData(ProvideMaterialData item)
        {
            if (item != null)
                materials.Remove(item);
        }

        public void UpdateMaterials(List<ProvideMaterialData> items)
        {
            foreach (ProvideMaterialData item_ in items)
            {
                ProvideMaterialData element_ = materials.Find(x_ => x_.Name == item_.Name);

                if (element_ != null)
                    element_.Update(item_);
            }
        }

        public void CopyFrom(ProvideJobListData src)
        {
            materials.Clear();

            this.Name        = src.Name;
            this.User        = src.User;
            this.Outport     = src.Outport;
            this.Reels       = src.Reels;
            this.State       = src.State;
            this.RegisteredDateTime = src.RegisteredDateTime;

            foreach (ProvideMaterialData item_ in src.Materials)
                materials.Add(new ProvideMaterialData(item_));
        }

        public void SetMaterialState(string carriername, ProvideMaterialData.States state)
        {
            ProvideMaterialData item_ = materials.Find(x_ => x_.Name == carriername);

            if (item_ != null)
                item_.State = state;
        }

        public void CleanUpMaterials(ref List<string> carriers, Dictionary<string, FourField<string, string, int, ProvideMaterialData>> unloaded)
        {
            if (unloaded == null)
                return;

            List<int> completed_ = new List<int>();

            try
            {
                for (int i_ = 0; i_ < materials.Count; i_++)
                {
                    List<ProvideMaterialData> items_ = unloaded.Where(x_ => x_.Value.second == materials[i_].Name).Select(y_ => y_.Value.fourth).ToList();

                    if (items_.Count > 0)
                    {
                        completed_.Add(i_);
                        carriers.Add(materials[i_].Name);
                    }
                    else
                    {
                        switch (materials[i_].State)
                        {
                            case ProvideMaterialData.States.Unknown:
                                break;
                            case ProvideMaterialData.States.Stored:
                            case ProvideMaterialData.States.Ready:
                                {   // Reserved job (Stored)
                                }
                                break;
                            case ProvideMaterialData.States.Providing:
                                {   // Current providing job (Already on terminal)
                                    carriers.Add(materials[i_].Name);
                                }
                                break;
                            case ProvideMaterialData.States.Completed:
                                {   // Already moved out from terminal
                                    completed_.Add(i_);
                                    carriers.Add(materials[i_].Name); // Double check
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
            finally
            {
                foreach (int index_ in completed_)
                    materials.RemoveAt(index_);
            }
        }
        #endregion
    }
}
#endregion