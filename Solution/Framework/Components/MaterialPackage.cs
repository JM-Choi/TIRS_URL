#region Imports
using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using TechFloor.Object;
using TechFloor.Util;
using System.Linq;
#endregion

#region Program
namespace TechFloor.Components
{
    public class MaterialPackage
    {
        #region Constants
        protected readonly char[] CONST_MATERIAL_DELIMETERS = { ';' };
        #endregion

        #region Fields
        private static StringBuilder buffer_ = new StringBuilder();

        protected bool exported = false;

        protected int completedCount = 0;

        protected int outputPort = 0;

        protected int materialCount = 0;

        protected string name = string.Empty;

        protected string outputPortName = string.Empty;

        protected DateTime registeredTime = DateTime.MinValue;

        protected DateTime finishedTime = DateTime.MinValue;

        protected ReelUnloadReportStates pickListState = ReelUnloadReportStates.Ready;

        protected List<Pair<string, ReelUnloadReportStates>> materials = new List<Pair<string, ReelUnloadReportStates>>();
        #endregion

        #region Properties
        public virtual bool IsEmpty => materials.Count <= 0;

        public virtual bool IsExported => exported;

        public virtual int CompletedCount => completedCount;

        public virtual int OutputPort => outputPort;

        public virtual int MaterialCount => materialCount;

        public virtual string Name => name;

        public virtual string OutputPortName => outputPortName;

        public virtual DateTime RegisteredTime => registeredTime;

        public virtual DateTime FinishedTime => finishedTime;

        public ReelUnloadReportStates PickState
        {
            get => pickListState;
            set => pickListState = value;
        }

        public IReadOnlyList<Pair<string, ReelUnloadReportStates>> Materials => materials;
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

        public MaterialPackage(MaterialPackage src)
        {
            CopyFrom(src);
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
            pickListState = src.pickListState;

            if (materials.Count != src.materials.Count)
            {
                materials.Clear();

                foreach (Pair<string, ReelUnloadReportStates> item_ in src.materials)
                    materials.Add(new Pair<string, ReelUnloadReportStates>(item_.first, item_.second));
            }
            else
            {
                for (int i_ = 0; i_ < src.materials.Count; i_++)
                {
                    materials[i_].first = src.materials[i_].first;
                    materials[i_].second = src.materials[i_].second;
                }
            }
        }

        public TimeSpan Elapsed()
        {
            return (finishedTime != DateTime.MinValue ? finishedTime - registeredTime : DateTime.Now - registeredTime);
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

                    if (items_.Length >= 2)
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

        public bool IsFinished()
        {
            return !(materials.Where(x_ => x_.second != ReelUnloadReportStates.Complete).Any());
        }

        public bool IsExist(string material)
        {
            return materials.Where(x_ => x_.first == material).Any();
        }

        public ReelUnloadReportStates GetMaterial(string material)
        {
            if (materials.Where(x_ => x_.first == material).Any())
                return materials.Find(x_ => x_.first == material).second;
            return ReelUnloadReportStates.Unknown;
        }

        public bool SetMaterial(string material, ReelUnloadReportStates state)
        {
            bool result_ = false;

            if (materials.Where(x_ => x_.first.Contains(material)).Any())
            {
                materials.Find(x_ => x_.first.Contains(material)).second = state;
                completedCount++;
                result_ = true;
            }

            return result_;
        }

        public void Clear()
        {
            outputPort = 0;
            materialCount = 0;
            name = string.Empty;
            outputPortName = string.Empty;
            registeredTime = DateTime.MinValue;
            finishedTime = DateTime.MinValue;
            materials.Clear();
            pickListState = ReelUnloadReportStates.Unknown;
        }
        #endregion
    }
}
#endregion