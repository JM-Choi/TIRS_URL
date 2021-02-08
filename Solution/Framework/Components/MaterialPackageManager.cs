#region Imports
using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using TechFloor.Util;
using System.Threading.Tasks;
using System.Diagnostics;
using TechFloor.Object;
#endregion

#region Program
namespace TechFloor.Components
{
    public class MaterialPackageManager
    {
        #region Fields
        protected string exportPath = string.Empty;

        protected ReelUnloadReportStates reportState = ReelUnloadReportStates.Unknown;

        protected List<MaterialPackage> materialPackages = new List<MaterialPackage>();
        #endregion

        #region Properties
        public virtual IReadOnlyList<MaterialPackage> Materials => materialPackages;

        public ReelUnloadReportStates ReportState => reportState;
        #endregion

        #region Events
        public event EventHandler<MaterialPackage> AddedMaterialPackage;

        public event EventHandler<MaterialPackage> RemovedMaterialPackage;

        public event EventHandler RemovedAllMaterialPackages;
        #endregion

        #region Constructors
        protected MaterialPackageManager() { }
        #endregion

        #region Protected methods
        #endregion

        #region Public methods
        public virtual void SetExportPath(string path = @"D:\System\")
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory((path));

            exportPath = path;
        }

        public virtual void FireAddedMaterialPackage(MaterialPackage pkg)
        {
            if (pkg != null)
                AddedMaterialPackage?.Invoke(this, pkg);
        }

        public virtual void FireRemovedMaterialPackage(MaterialPackage pkg)
        {
            if (pkg != null)
                RemovedMaterialPackage?.Invoke(this, new MaterialPackage(pkg));
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

                lock (materialPackages)
                {
                    if (materialPackages.Count > 0)
                        FireRemovedMaterialPackage(materialPackages[0]);

                    materialPackages.Clear();
                    materialPackages.Add(pkg);
                }

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
                        // ExportUnloadState(ReelUnloadReportStates.Ready);
                        // Logger.Trace($"UR Status : {reportState}"); 
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
            finally
            {
                if (state == ReelUnloadReportStates.Complete)
                {
                    foreach (MaterialPackage item_ in materialPackages)
                    {
                        if (item_.IsExist(item_.Name))
                        {
                            item_.SetMaterial(item_.Name, state);

                            if (item_.IsFinished())
                            {
                                new TaskFactory().StartNew(new Action<object>((x_) =>
                                {
                                    if (x_ != null)
                                        RemoveMaterialPackage(x_ as string, true, false);
                                }), item_.Name);
                            }

                            break;
                        }
                    }
                }
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
                        FireRemovedMaterialPackage(p);

                    if (remove)
                        DeleteMaterialPackage(name);

                    lock (materialPackages)
                        materialPackages.Remove(p);
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
                lock (materialPackages)
                {
                    if (materialPackages.Count > 0)
                        FireRemovedMaterialPackage(materialPackages[0]);

                    materialPackages.Clear();
                }

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

        public virtual MaterialPackage GetFirstMaterialPickList()
        {
            if (materialPackages.Count > 0)
                return materialPackages[0];
            return null;
        }
        #endregion
    }
}
#endregion