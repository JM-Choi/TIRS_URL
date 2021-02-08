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
/// project ServiceManager
/// @author Marcus (isadrastea.kor@gmail.com)
/// @namespace ServiceManager
/// @file ProcessChecker.cs
/// @brief
/// @details
/// @date 2020-3-14 오전 1:01 
///////////////////////////////////////////////////////////////////////////////
#endregion

#region Imports
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
#endregion

#region Program
namespace ServiceManager
{
    public static class ProcessChecker
    {
        #region Imports
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string strClassName, string strWindowName);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowEx(IntPtr parentWindow, IntPtr previousChildWindow, string windowClass, string windowTitle);

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowThreadProcessId(IntPtr window, out int process);

        [DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, EntryPoint = "SendMessage")]
        private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
        #endregion

        #region Constants
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_CLOSE = 0xF060;
        public const uint WM_CLOSE = 0x10;
        #endregion

        #region Public methods
        public static void CloseMainWindow(IntPtr hwnd)
        {
            try
            {
                if (hwnd != IntPtr.Zero)
                    // SendMessage(hwnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                    SendMessage(hwnd, WM_SYSCOMMAND, (IntPtr)SC_CLOSE, IntPtr.Zero);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ProcessChecker.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
            }
        }

        private static IntPtr[] GetProcessWindows(int process)
        {
            IntPtr[] ret_ = (new IntPtr[256]);
            int count_ = 0;
            IntPtr hwnd_ = IntPtr.Zero;

            do
            {
                hwnd_ = FindWindowEx(IntPtr.Zero, hwnd_, null, null);
                int iProcess_;
                GetWindowThreadProcessId(hwnd_, out iProcess_);
                if (iProcess_ == process) ret_[count_++] = hwnd_;
            } while (hwnd_ != IntPtr.Zero);

            System.Array.Resize(ref ret_, count_);
            return ret_;
        }

        public static bool IsProcessRunning(string proc, bool killall = false)
        {
            Process[] processes_ = Process.GetProcessesByName(proc);

            if (processes_.Length >= 1)
            {
                foreach (Process proc_ in processes_)
                {
                    if (killall)
                        proc_.Kill();
                }
                return true;
            }

            return false;
        }

        public static bool IsProcessRunning(string proc, string title, bool killall = false)
        {
            Process[] processes_ = Process.GetProcessesByName(proc);

            foreach (Process ps_ in processes_)
            {
                IntPtr hwnd_ = ps_.MainWindowHandle;
                if (hwnd_.ToInt32() == 0)
                {

                    hwnd_ = FindWindow(null, title);

                    int id_ = -1;
                    GetWindowThreadProcessId(hwnd_, out id_);

                    if (id_ == ps_.Id)
                    {
                        SendMessage(hwnd_, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                        return true;
                    }

                    IntPtr[] items_ = GetProcessWindows(ps_.Id);
                    foreach (IntPtr item_ in items_)
                        SendMessage(item_, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                }
            }

            return false;
        }

        // const int SW_HIDE = 0;
        // const int SW_SHOWNORMAL = 1;
        // const int SW_SHOWMINIMIZED = 2;
        // const int SW_SHOWMAXIMIZED = 3;
        // const int SW_SHOWNOACTIVATE = 4;
        // const int SW_RESTORE = 9;
        // const int SW_SHOWDEFAULT = 10;
        public static bool IsProcessRunning(string proc)
        {
            Process[] processes_ = Process.GetProcessesByName(proc);

            if (processes_.Length >= 1)
                return true;

            return false;
        }

        public static bool IsProcessRunning(string proc, ref IntPtr hwnd, ref string title)
        {
            Process[] processes_ = Process.GetProcessesByName(proc);

            if (processes_.Length >= 1)
            {
                hwnd = processes_[0].MainWindowHandle;
                title = processes_[0].MainWindowTitle;
                return true;
            }

            return false;
        }

        public static bool IsProcessRunning(string proc, int procid, bool showproc = true)
        {
            const int restore_ = 9;

            Process[] processes_ = Process.GetProcessesByName(proc);

            if (processes_.Length > 1)
            {
                for (var i = 0; i < processes_.Length; i++)
                {
                    if (processes_[i].Id != procid && showproc)
                    {
                        IntPtr hWnd = processes_[i].MainWindowHandle;

                        if (IsIconic(hWnd))
                            ShowWindowAsync(hWnd, restore_);

                        SetForegroundWindow(hWnd);
                        break;
                    }
                }

                return true;
            }

            return false;
        }
        #endregion
    }
}
#endregion