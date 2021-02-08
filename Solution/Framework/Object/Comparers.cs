using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
#region Imports
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
#endregion

#region Program
#pragma warning disable CS1058, CS0618
namespace TechFloor.Object
{
    public class StringLogicalComparer
    {
        protected static readonly char[] CONST_IPADDRESS_DELIMITER_IPV4 = { '.' };
        protected static readonly char[] CONST_IPADDRESS_DELIMITER_IPV6 = { ':' };

        public static int Compare(string s1, string s2)
        {
            if ((s1 == null) && (s2 == null)) return 0;
            else if (s1 == null) return -1;
            else if (s2 == null) return 1;

            if ((s1.Equals(string.Empty) && (s2.Equals(string.Empty)))) return 0;
            else if (s1.Equals(string.Empty)) return -1;
            else if (s2.Equals(string.Empty)) return -1;

            bool sp1_ = Char.IsLetterOrDigit(s1, 0);
            bool sp2 = Char.IsLetterOrDigit(s2, 0);
            if (sp1_ && !sp2) return 1;
            if (!sp1_ && sp2) return -1;

            int i1_ = 0, i2_ = 0, r_ = 0;

            while (true)
            {
                bool c1_ = Char.IsDigit(s1, i1_);
                bool c2_ = Char.IsDigit(s2, i2_);

                if (!c1_ && !c2_)
                {
                    bool letter1_ = Char.IsLetter(s1, i1_);
                    bool letter2_ = Char.IsLetter(s2, i2_);

                    if ((letter1_ && letter2_) || (!letter1_ && !letter2_))
                    {
                        if (letter1_ && letter2_)
                            r_ = Char.ToLower(s1[i1_]).CompareTo(Char.ToLower(s2[i2_]));
                        else
                            r_ = s1[i1_].CompareTo(s2[i2_]);
                        if (r_ != 0) return r_;
                    }
                    else if (!letter1_ && letter2_) return -1;
                    else if (letter1_ && !letter2_) return 1;
                }
                else if (c1_ && c2_)
                {
                    r_ = CompareNum(s1, ref i1_, s2, ref i2_);
                    if (r_ != 0) return r_;
                }
                else if (c1_)
                {
                    return -1;
                }
                else if (c2_)
                {
                    return 1;
                }
                i1_++;
                i2_++;
                if ((i1_ >= s1.Length) && (i2_ >= s2.Length))
                {
                    return 0;
                }
                else if (i1_ >= s1.Length)
                {
                    return -1;
                }
                else if (i2_ >= s2.Length)
                {
                    return -1;
                }
            }
        }

        private static int CompareNum(string s1, ref int i1, string s2, ref int i2)
        {
            int nzStart1 = i1, nzStart2 = i2;
            int end1 = i1, end2 = i2;

            ScanNumEnd(s1, i1, ref end1, ref nzStart1);
            ScanNumEnd(s2, i2, ref end2, ref nzStart2);
            int start1 = i1; i1 = end1 - 1;
            int start2 = i2; i2 = end2 - 1;

            int nzLength1 = end1 - nzStart1;
            int nzLength2 = end2 - nzStart2;

            if (nzLength1 < nzLength2) return -1;
            else if (nzLength1 > nzLength2) return 1;

            for (int j1 = nzStart1, j2 = nzStart2; j1 <= i1; j1++, j2++)
            {
                int r = s1[j1].CompareTo(s2[j2]);
                if (r != 0) return r;
            }
            int length1 = end1 - start1;
            int length2 = end2 - start2;
            if (length1 == length2) return 0;
            if (length1 > length2) return -1;
            return 1;
        }

        private static void ScanNumEnd(string s, int start, ref int end, ref int nzStart)
        {
            nzStart = start;
            end = start;
            bool countZeros = true;
            while (Char.IsDigit(s, end))
            {
                if (countZeros && s[end].Equals('0'))
                {
                    nzStart++;
                }
                else countZeros = false;
                end++;
                if (end >= s.Length) break;
            }
        }

        public static bool IsDigit(string str)
        {
            foreach (char ch_ in str)
            {
                if (!Char.IsDigit(ch_))
                    return false;
            }

            return true;
        }

        public static bool IsIpAddress(string address, ProtocolType protocol = ProtocolType.IPv4)
        {
            switch (protocol)
            {
                case ProtocolType.IPv4:
                    return (address.Split(CONST_IPADDRESS_DELIMITER_IPV4, StringSplitOptions.RemoveEmptyEntries).Length == 4);
                case ProtocolType.IPv6:                    
                    return (address.Split(CONST_IPADDRESS_DELIMITER_IPV6, StringSplitOptions.None).Length == 6);
                default:
                    return false;
            }
        }

        public static bool IsHostAddress(string host)
        {
            bool result_ = false;
            IPHostEntry info_;

            try
            {
                info_ = Dns.Resolve(host);

                if (info_ != null && info_.AddressList.Length > 0)
                    result_ = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"StringLogicalComparer.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }

            return result_;
        }

        public static IPAddress ResolveHostAddress(string host)
        {
            IPHostEntry info_ = Dns.Resolve(host);

            if (info_ != null && info_.AddressList.Length > 0)
                return info_.AddressList.First();

            return null;
        }
    }

    public class NumericComparer : IComparer
    {
        public NumericComparer()
        { }

        public int Compare(object x, object y)
        {
            if ((x is string) && (y is string))
                return StringLogicalComparer.Compare((string)x, (string)y);
            return -1;
        }
    }

    public class NumericComparerByRegex : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            var regex = new Regex("^(d+)");
            var xRegexResult = regex.Match(x);
            var yRegexResult = regex.Match(y);

            if (xRegexResult.Success && yRegexResult.Success)
                return int.Parse(xRegexResult.Groups[1].Value).CompareTo(int.Parse(yRegexResult.Groups[1].Value));

            return x.CompareTo(y);
        }
    }

    public class LogIndexComparerByFileInfo : IComparer
    {
        public int Compare(object x, object y)
        {
            FileInfo fi1 = (FileInfo)x;
            FileInfo fi2 = (FileInfo)y;
            return StringLogicalComparer.Compare(fi1.FullName, fi2.FullName);
        }
    }
}
#endregion