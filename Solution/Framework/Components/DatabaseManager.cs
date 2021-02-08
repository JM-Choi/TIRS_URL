#region Imports
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
#endregion

#region Program
namespace TechFloor.Components
{
    public class DatabaseManager
    {
        #region Enumerations
        public enum DatabaseTypes
        {
            Access,
            MsSql,
            MySql,
            dBase,
            PostageSql,
            Sqlite,
        }

        public enum DatabaseOleDrivers
        {
            Jet,
            Ace,
            MSOLEDBSQL,
        }
        #endregion

        #region Constants
        protected readonly string CONST_DEFAULT_CRYPTO_SEED = "techfloor@tf0778";
        #endregion

        #region Fields
        protected bool initialized = false;

        protected bool localDb = false;

        protected int dbPort;

        protected string name;

        protected string dbServer;

        protected string dbName;

        protected string dbFile;

        protected string dbUser;

        protected string dbPassword;

        protected string sqllocaldbTool = @"C:\Program Files\Microsoft SQL Server\130\Tools\Binn\sqllocaldb.exe";

        protected string connectionString;

        protected DatabaseTypes dbType = DatabaseTypes.Access;

        protected DatabaseOleDrivers oleDriver = DatabaseOleDrivers.Jet;
        #endregion

        #region Properties
        public bool Initialized => initialized;

        public string ConnectionString => connectionString;

        public string Name => name;

        public string DBServer => dbServer;

        public string DBFile => dbFile;

        public string DBUser => dbUser;
        #endregion

        #region Constructors
        public DatabaseManager(string name, string server, string dbname= null, int port = -1, string user= null, string password= null, DatabaseTypes dbtype = DatabaseTypes.Access, DatabaseOleDrivers oledriver = DatabaseOleDrivers.Jet, bool localdb = false)
        {
            this.dbPort = port;
            this.name = name;
            this.dbServer = server;
            this.dbName = dbname;
            this.dbUser = user;
            this.dbPassword = password;
            this.dbType = dbtype;
            this.oleDriver = oledriver;
            
            if (localDb = localdb)
                CheckLocalDB();

            this.connectionString = CreateConnectionString();
        }
        #endregion

        #region Protected methods
        protected virtual string CreateConnectionString()
        {
            string result_ = string.Empty;

            switch (dbType)
            {
                case DatabaseTypes.Access:
                    {
                        switch (oleDriver)
                        {
                            case DatabaseOleDrivers.Jet:
                                result_ = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={dbServer};User Id={dbUser};Password={dbPassword};";
                                break;
                            case DatabaseOleDrivers.Ace:
                                result_ = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbServer};Persist Security Info=False;"; ;
                                break;
                        }
                    }
                    break;
                case DatabaseTypes.MsSql:
                    {
                        switch (oleDriver)
                        {
                            case DatabaseOleDrivers.MSOLEDBSQL:
                                {
                                    if (dbPort != -1)
                                        result_ = $"Provider=MSOLEDBSQL;Server={dbServer},{dbPort};Database={dbName};UID={dbUser};PWD={dbPassword};";
                                    else
                                    {
                                        if (string.IsNullOrEmpty(dbUser) || string.IsNullOrEmpty(dbPassword))
                                            result_ = $"Provider=MSOLEDBSQL;Server={dbServer};Database={dbName};Integrated Security=SSPI;";
                                        else
                                            result_ = $"Provider=MSOLEDBSQL;Server={dbServer};Database={dbName};UID={dbUser};PWD={dbPassword};";
                                    }
                                }
                                break;
                        }
                    }
                    break;
                case DatabaseTypes.MySql:
                    {
                        result_ = $"Provider=MySQLProv;Data Source={dbServer};User Id={dbUser};Password={dbPassword};";
                    }
                    break;
            }

            return result_;
        }

        protected bool CheckLocalDB()
        {
            bool result_ = false;
            RegistryKey reg_ = Registry.LocalMachine;
            reg_ = reg_.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server Local DB\Installed Versions", true);

            if (reg_ != null)
            {
                string[] keys_ = reg_.GetSubKeyNames();

                if (keys_.Length > 0)
                {
                    reg_ = reg_.OpenSubKey($@"{keys_[0]}", true);

                    if (reg_ != null)
                    {
                        object val_ = reg_.GetValue("InstanceAPIPath");
                        string key_ = keys_[0].Replace(".", "");
                        string[] tokens_ = Convert.ToString(val_).Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                        string path_ = string.Empty;

                        foreach (string token_ in tokens_)
                        {
                            if (token_ == key_)
                            {
                                path_ += key_ + @"\Tools\Binn\sqllocaldb.exe";
                                break;
                            }
                            else
                            {
                                path_ += (token_ + @"\");
                            }
                        }

                        if (File.Exists(path_))
                        {
                            sqllocaldbTool = path_;
                            result_ = true;
                        }
                    }
                }
            }

            return result_;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "<보류 중>")]
        protected bool CheckDB()
        {
            bool result_ = false;

            using (OleDbConnection conn_ = new OleDbConnection($"Provider=MSOLEDBSQL;Server={dbServer};Initial Catalog=master;Integrated Security=SSPI;"))
            {
                string sql_ = "SELECT * FROM master.dbo.sysdatabases WHERE name ='" + dbName + "'";
                OleDbCommand cmd_ = new OleDbCommand(sql_);
                cmd_.Connection = conn_;

                try
                {
                    conn_.Open();
                    OleDbDataReader reader_ = cmd_.ExecuteReader();
                    result_ = reader_.HasRows;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                }
                finally
                {
                    conn_.Close();
                }
            }

            return result_;
        }

        protected bool DoProcess(string activity, string arg, ref string result)
        {
            bool result_ = false;

            try
            {
                string cmd_ = "\"" + sqllocaldbTool + "\"";
                ProcessStartInfo info = new ProcessStartInfo(cmd_, " " + activity + (string.IsNullOrEmpty(arg)? string.Empty : " " + arg));
                Process proc_ = new Process();
                proc_.StartInfo = info;
                proc_.StartInfo.UseShellExecute = false;
                proc_.StartInfo.RedirectStandardOutput = true;
                proc_.StartInfo.RedirectStandardError = true;
                proc_.Start();
                proc_.WaitForExit();

                StreamReader reader_ = proc_.StandardOutput;
                result = reader_.ReadToEnd();
                reader_.Close();

                if (string.IsNullOrEmpty(result))
                {
                    reader_ = proc_.StandardError;
                    result = reader_.ReadToEnd();
                    reader_.Close();
                }
                else
                    result_ = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return result_;
        }
        #endregion

        #region Public methods
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "<보류 중>")]
        public bool CreateDB(string path)
        {
            bool result_ = false;
            string con_ = string.Empty;
            string sql_ = string.Empty;

            switch (dbType)
            {
                case DatabaseTypes.Access:
                    {
                        try
                        {
                            sql_ = $"Provider=Microsoft.Jet.OLEDB.4.0;Jet OLEDB:Engine Type=5;Data Source={dbServer}";
                            Type objClassType_ = Type.GetTypeFromProgID("ADOX.Catalog");

                            if (objClassType_ != null)
                            {
                                object obj_ = Activator.CreateInstance(objClassType_);
                                obj_.GetType().InvokeMember("Create", System.Reflection.BindingFlags.InvokeMethod, null, obj_, new object[] { sql_ });
                                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj_);
                                obj_ = null;
                                result_ = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                        }
                    }
                    break;
                case DatabaseTypes.MsSql:
                    {
                        con_ = $"Provider=MSOLEDBSQL;Server={dbServer};Integrated Security=SSPI;";
                        sql_ = $@"CREATE DATABASE [{dbName}]
                            ON PRIMARY (NAME={dbName}_data, FILENAME = '{path}{dbName}_data.mdf')
                            LOG ON (NAME={dbName}_log, FILENAME = '{path}{dbName}_log.ldf')";

                        using (OleDbConnection conn_ = new OleDbConnection(con_ + (localDb ? string.Empty : "Database=master;")))
                        {
                            OleDbCommand cmd_ = new OleDbCommand(sql_);
                            cmd_.Connection = conn_;

                            try
                            {
                                conn_.Open();
                                cmd_.ExecuteNonQuery();
                                result_ = true;
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                            }
                            finally
                            {
                                conn_.Close();
                            }
                        }
                    }
                    break;
            }

            return result_;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "<보류 중>")]
        public bool CreateTable(string tbname)
        {
            bool result_ = false;
            string sql_ = $@"
                    CREATE TABLE {tbname} (
                        UserId INT PRIMARY KEY IDENTITY(1,1),
                        GroupId INT DEFAULT 0 NOT NULL,
                        Name VARCHAR(30) NOT NULL UNIQUE,
                        Password VARBINARY(256) NOT NULL,
                        Fullname VARCHAR(256),
                        Remark VARCHAR(2048));
                    INSERT INTO {tbname} (
                        GroupId,
                        Name,
                        Password,
                        Fullname,
                        Remark)
                    VALUES (
                        3,
                        'Administrator',
                        EncryptByPassPhrase(N'{CONST_DEFAULT_CRYPTO_SEED}', N'007'),
                        'TechFloor account',
                        'https://techfloor.co.kr/');";

            using (OleDbConnection conn_ = new OleDbConnection(connectionString))
            {
                OleDbCommand cmd_ = new OleDbCommand(sql_);
                cmd_.Connection = conn_;

                try
                {
                    conn_.Open();

                    if (cmd_.ExecuteNonQuery() > 0)
                        result_ = true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                }
                finally
                {
                    conn_.Close();
                }
            }

            return result_;
        }

        public bool CreateLocalDBInstance(string instance, bool start = true)
        {
            string result_ = string.Empty;
            return DoProcess("create", instance + (start ? " -s" : string.Empty), ref result_);
        }

        public bool StartLocalDBInstance(string instance)
        {
            string result_ = string.Empty;
            return DoProcess("start", instance, ref result_);
        }

        public bool StopLocalDBInstance(string instance)
        {
            string result_ = string.Empty;
            return DoProcess("stop", instance, ref result_);
        }

        public bool DeleteLocalDBInstance(string instance)
        {
            string result_ = string.Empty;
            return DoProcess("delete", instance, ref result_);
        }

        public bool GetLocalDBInstanceInfo(string instance)
        {
            string result_ = string.Empty;
            return DoProcess("info", instance, ref result_);
        }

        public bool GetAllLocalDB(string instance)
        {
            string result_ = string.Empty;

            if (DoProcess("info", string.Empty, ref result_))
                return result_.Contains(instance);

            return false;
        }

        public bool IsExistDB(bool create = true)
        {
            bool rc_ = false;
            string result_ = string.Empty;

            switch (dbType)
            {
                case DatabaseTypes.Access:
                    {
                        return File.Exists(dbServer);
                    }
                case DatabaseTypes.MsSql:
                    {
                        if (localDb)
                        {
                            string[] tokens_ = dbServer.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                            string key_ = tokens_[tokens_.Length - 1];
                            rc_ = GetAllLocalDB(key_);

                            if (!rc_ && create)
                            {
                                CreateLocalDBInstance(key_);
                                return GetAllLocalDB(key_);
                            }

                            return rc_;
                        }
                        else
                        {
                            return CheckDB();
                        }
                    }
                default:
                    return false;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "<보류 중>")]
        public bool IsExistsTable(string tbname)
        {
            bool result_ = false;

            switch (dbType)
            {
                case DatabaseTypes.Access:
                    {
                        using (OleDbConnection conn_ = new OleDbConnection(connectionString))
                        {
                            try
                            {
                                conn_.Open();
                                result_ = conn_.GetSchema("Tables", new string[4] { null, null, tbname, "TABLE" }).Rows.Count > 0;
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                            }
                            finally
                            {
                                conn_.Close();
                            }
                        }
                    }
                    break;
                case DatabaseTypes.MsSql:
                    {
                        using (OleDbConnection conn_ = new OleDbConnection($"Provider=MSOLEDBSQL;Server={dbServer};Database={dbName};Integrated Security=SSPI;"))
                        {
                            OleDbCommand cmd_ = new OleDbCommand($"SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = '{tbname}'");
                            cmd_.Connection = conn_;

                            try
                            {
                                conn_.Open();
                                OleDbDataReader reader_ = cmd_.ExecuteReader();
                                result_ = reader_.HasRows;
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                            }
                            finally
                            {
                                conn_.Close();
                            }
                        }
                    }
                    break;
            }

            return result_;
        }

        public virtual XElement ToXml(string nodename= null)
        {
            XElement result_ = new XElement(string.IsNullOrEmpty(nodename) ? GetType().Name : nodename,
                new XAttribute("name", name),
                new XAttribute("server", dbServer));

            if (!string.IsNullOrEmpty(dbName))
                result_.Add(new XAttribute("dbname", dbName));

            if (!string.IsNullOrEmpty(dbFile))
                result_.Add(new XAttribute("dbfile", dbFile));

            if (!string.IsNullOrEmpty(dbUser) && !string.IsNullOrEmpty(dbPassword))
                result_.Add(new XAttribute("user", dbUser),
                    new XAttribute("password", dbPassword));

            result_.Add(new XAttribute("driver", oleDriver),
                new XAttribute("dbtype", dbType));

            return result_;
        }

        public virtual bool Init(string path, string tbname)
        {
            initialized = false;

            try
            {
                if (!IsExistDB())
                    CreateDB(path);

                if (!(initialized = IsExistsTable(tbname)))
                    initialized = CreateTable(tbname);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return initialized;
        }

        public virtual bool GetAccessDBPrivder(ref string provider)
        {
            OleDbEnumerator enum_ = new OleDbEnumerator();
            DataTable tb_ = enum_.GetElements();
            List<string> prov_ = new List<string>();

            foreach (DataRow row_ in tb_.Rows)
                prov_.Add(row_[0].ToString());
            
            tb_.Dispose();
            prov_.Sort((p, n) => n.CompareTo(p));

            provider = prov_.Find(m => m.StartsWith("Microsoft.ACE.OLEDB"));
            
            if (string.IsNullOrEmpty(provider))
                provider = prov_.Find(m => m.StartsWith("Microsoft.Jet.OLEDB"));

            return string.IsNullOrEmpty(provider);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "<보류 중>")]
        public virtual DataSet DecryptSelect(string pre, string post)
        {
            DataSet result_ = null;

            if (!string.IsNullOrEmpty(pre) && !string.IsNullOrEmpty(post))
            {
                using (OleDbConnection conn_ = new OleDbConnection(connectionString))
                {
                    try
                    {
                        string query_ = $"{pre}N'{CONST_DEFAULT_CRYPTO_SEED}'{post}";
                        conn_.Open();
                        OleDbDataAdapter adpt_ = adpt_ = new OleDbDataAdapter(query_, conn_);
                        result_ = new DataSet();
                        adpt_.Fill(result_);
                        adpt_.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                    }
                    finally
                    {
                        conn_.Close();
                    }
                }
            }

            return result_;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "<보류 중>")]
        public virtual DataSet Select(string query)
        {
            DataSet result_ = null;

            if (!string.IsNullOrEmpty(query))
            {
                using (OleDbConnection conn_ = new OleDbConnection(connectionString))
                {
                    try
                    {
                        conn_.Open();
                        OleDbDataAdapter adpt_ = new OleDbDataAdapter(query, conn_);
                        result_ = new DataSet();
                        adpt_.Fill(result_);
                        adpt_.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                    }
                    finally
                    {       
                        conn_.Close();
                    }
                }
            }

            return result_;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "<보류 중>")]
        public virtual bool Insert(string query)
        {
            bool result_ = false;

            if (!string.IsNullOrEmpty(query))
            {
                using (OleDbConnection conn_ = new OleDbConnection(connectionString))
                {
                    try
                    {
                        conn_.Open();
                        OleDbDataAdapter adpt_ = new OleDbDataAdapter();
                        adpt_.InsertCommand = new OleDbCommand(query, conn_);
                        result_ = adpt_.InsertCommand.ExecuteNonQuery() > 0;
                        adpt_.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                    }
                    finally
                    {
                        conn_.Close();
                    }
                }
            }

            return result_;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "<보류 중>")]
        public virtual bool Delete(string query)
        {
            bool result_ = false;

            if (!string.IsNullOrEmpty(query))
            {
                using (OleDbConnection conn_ = new OleDbConnection(connectionString))
                {
                    try
                    {
                        conn_.Open();
                        OleDbDataAdapter adpt_ = new OleDbDataAdapter();
                        adpt_.DeleteCommand = new OleDbCommand(query, conn_);
                        result_ = adpt_.DeleteCommand.ExecuteNonQuery() > 0;
                        adpt_.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                    }
                    finally
                    {
                        conn_.Close();
                    }
                }
            }

            return result_;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "<보류 중>")]
        public virtual bool Update(string query)
        {
            bool result_ = false;

            if (!string.IsNullOrEmpty(query))
            {
                using (OleDbConnection conn_ = new OleDbConnection(connectionString))
                {
                    try
                    {
                        conn_.Open();
                        OleDbDataAdapter adpt_ = new OleDbDataAdapter();
                        adpt_.UpdateCommand = new OleDbCommand(query, conn_);
                        result_ = adpt_.UpdateCommand.ExecuteNonQuery() > 0;
                        adpt_.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                    }
                    finally
                    {
                        conn_.Close();
                    }
                }
            }

            return result_;
        }
        #endregion
    }
}
#endregion