#region Imports
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

#region Program
namespace TechFloor.Object
{
	public class RegistryExt
	{
		#region Fields
		private bool showError = false;

		private string subKey = "SOFTWARE\\";

		private RegistryKey baseRegistryKey = Registry.LocalMachine;
		#endregion

		#region Properties
		public bool ShowError
		{
			get => showError;
			set => showError = value;
		}

		public string SubKey
		{
			get => subKey;
			set => subKey = value;
		}
		
		public RegistryKey BaseRegistryKey
		{
			get => baseRegistryKey;
			set => baseRegistryKey = value;
		}
		#endregion

		#region Constructors
		public RegistryExt(string key)
		{
			if (!string.IsNullOrEmpty(key))
				this.subKey = key;
		}
		#endregion

		#region Public methods
		public string Read(string key)
		{
			RegistryKey rk_ = baseRegistryKey;
			using (RegistryKey sk_ = rk_.OpenSubKey(subKey))
			{
				if (sk_ == null)
					return null;
				else
				{
					try
					{
						return (string)sk_.GetValue(key);
					}
					catch (Exception ex)
					{
						Debug.WriteLine($"Reading registry {key}: Exception={ex.Message}");
					}
				}
			}

			return null;
		}

        public object ReadObject(string key)
        {
            RegistryKey rk_ = baseRegistryKey;
            using (RegistryKey sk_ = rk_.OpenSubKey(subKey))
            {
                if (sk_ == null)
                    return null;
                else
                {
                    try
                    {
                        return sk_.GetValue(key);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Reading registry {key}: Exception={ex.Message}");
                    }
                }
            }

            return null;
        }

        public bool Write(string key, object val)
		{
			try
			{
				RegistryKey rk_ = baseRegistryKey;
				using (RegistryKey sk_ = rk_.OpenSubKey(subKey))
				{
					sk_.SetValue(key, val);
					return true;
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Writing registry {key}: Exception={ex.Message}");
			}

			return false;
		}

		public bool IsExistSubKey(string key)
		{
			RegistryKey rk_ = baseRegistryKey;
			using (RegistryKey sk_ = rk_.OpenSubKey(subKey))
			{
				if (sk_ != null)
					return true;
			}

			return false;
		}

		public bool CreateSubKey(string key)
		{
			try
			{
				RegistryKey rk_ = baseRegistryKey;
				using (RegistryKey sk_ = rk_.OpenSubKey(subKey))
				{
					if (sk_ == null)
					{
						if (sk_.CreateSubKey(key, RegistryKeyPermissionCheck.ReadWriteSubTree) != null)
							return true;
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Deleting SubKey {subKey}: Exception={ex.Message}");
			}

			return false;
		}

		public bool DeleteKey(string key)
		{
			try
			{
				RegistryKey rk_ = baseRegistryKey;
				using (RegistryKey sk_ = rk_.OpenSubKey(subKey))
				{
					if (sk_ == null)
						return true;
					else
						sk_.DeleteValue(key);

					return true;
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Deleting SubKey {subKey}: Exception={ex.Message}");
			}

			return false;
		}

		public bool DeleteSubKeyTree()
		{
			try
			{
				RegistryKey rk_ = baseRegistryKey;
				using (RegistryKey sk_ = rk_.OpenSubKey(subKey))
				{
					if (sk_ != null)
						rk_.DeleteSubKeyTree(subKey);

					return true;
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Deleting SubKey {subKey}: Exception={ex.Message}");				
			}

			return false;
		}

		public int SubKeyCount()
		{
			try
			{
				RegistryKey rk_ = baseRegistryKey;
				using (RegistryKey sk_ = rk_.OpenSubKey(subKey))
				{
					if (sk_ != null)
						return sk_.SubKeyCount;
					else
						return 0;
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Retriving subkeys of {subKey}: Exception={ex.Message}");
				return 0;
			}
		}

		public int ValueCount()
		{
			try
			{
				RegistryKey rk_ = baseRegistryKey;
				using (RegistryKey sk_ = rk_.OpenSubKey(subKey))
				{
					if (sk_ != null)
						return sk_.ValueCount;
					else
						return 0;
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Retriving keys of {subKey}: Exception={ex.Message}");
				return 0;
			}
		}
		#endregion
	}
}
#endregion