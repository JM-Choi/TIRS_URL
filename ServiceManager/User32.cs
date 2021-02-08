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
/// @file User32.cs
/// @brief
/// @details
/// @date 2020-3-14 오전 12:52 
///////////////////////////////////////////////////////////////////////////////
#endregion

#region Imports
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;
#endregion

#region Program
namespace ServiceManager
{
    #region Enumerations
    [FlagsAttribute]
    public enum WindowStationAccessRight : uint
    {
        /// <summary>All possible access rights for the window station.</summary>
        WINSTA_ALL_ACCESS = 0x37F,

        /// <summary>Required to use the clipboard.</summary>
        WINSTA_ACCESSCLIPBOARD = 0x0004,

        /// <summary>Required to manipulate global atoms.</summary>
        WINSTA_ACCESSGLOBALATOMS = 0x0020,

        /// <summary>Required to create new desktop objects on the window station.</summary>
        WINSTA_CREATEDESKTOP = 0x0008,

        /// <summary>Required to enumerate existing desktop objects.</summary>
        WINSTA_ENUMDESKTOPS = 0x0001,

        /// <summary>Required for the window station to be enumerated.</summary>
        WINSTA_ENUMERATE = 0x0100,

        /// <summary>Required to successfully call the ExitWindows or ExitWindowsEx function. Window stations can be shared by users and this access type can prevent other users of a window station from logging off the window station owner.</summary>
        WINSTA_EXITWINDOWS = 0x0040,

        /// <summary>Required to read the attributes of a window station object. This attribute includes color settings and other global window station properties.</summary>
        WINSTA_READATTRIBUTES = 0x0002,

        /// <summary>Required to access screen contents.</summary>
        WINSTA_READSCREEN = 0x0200,

        /// <summary>Required to modify the attributes of a window station object. The attributes include color settings and other global window station properties.</summary>
        WINSTA_WRITEATTRIBUTES = 0x0010,
    }

    public enum OpenDesktopFlag : uint
    {
        /// <summary>
        /// Default value
        /// </summary>
        DF_NONE = 0x0000,

        /// <summary>
        /// Allows processes running in other accounts on the desktop to set hooks in this process.
        /// </summary>
        DF_ALLOWOTHERACCOUNTHOOK = 0x0001,
    }

    [FlagsAttribute]
    public enum DesktopAccessRight  : uint
    {
        /// <summary>Required to create a menu on the desktop. </summary>
        DESKTOP_CREATEMENU = 0x0004,

        /// <summary>Required to create a window on the desktop. </summary>
        DESKTOP_CREATEWINDOW = 0x0002,

        /// <summary>Required for the desktop to be enumerated. </summary>
        DESKTOP_ENUMERATE = 0x0040,

        /// <summary>Required to establish any of the window hooks. </summary>
        DESKTOP_HOOKCONTROL = 0x0008,

        /// <summary>Required to perform journal playback on a desktop. </summary>
        DESKTOP_JOURNALPLAYBACK = 0x0020,

        /// <summary>Required to perform journal recording on a desktop. </summary>
        DESKTOP_JOURNALRECORD = 0x0010,

        /// <summary>Required to read objects on the desktop. </summary>
        DESKTOP_READOBJECTS = 0x0001,

        /// <summary>Required to activate the desktop using the SwitchDesktop function. </summary>
        DESKTOP_SWITCHDESKTOP = 0x0100,

        /// <summary>Required to write objects on the desktop. </summary>
        DESKTOP_WRITEOBJECTS = 0x0080,
    }
    #endregion

    #region Imports    
    public static class User32
    {
        /// <summary>
        /// Retrieves a handle to the current window station for the calling process.
        /// </summary>
        /// <returns>If the function succeeds, the return value is a handle to 
        /// the window station. If the function fails, the return value is NULL.</returns>
        [DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr GetProcessWindowStation();

        /// <summary>
        /// Assigns the specified window station to the calling process. 
        /// This enables the process to access objects in the window station 
        /// such as desktops, the clipboard, and global atoms. All subsequent 
        /// operations on the window station use the access rights granted 
        /// to hWinSta.
        /// </summary>
        /// <param name="hWinSta">[in] Handle to the window station to be 
        /// assigned to the calling process</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero. </returns>
        [DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr SetProcessWindowStation(IntPtr hWinstation);

        /// <summary>
        /// Retrieves a handle to the desktop assigned to the specified thread.
        /// </summary>
        /// <param name="dwThread">[in] Handle to the thread for which to return 
        /// the desktop handle.</param>
        /// <returns>If the function succeeds, the return value is a handle to 
        /// the desktop associated with the specified thread. You do not need 
        /// to call the CloseDesktop function to close the returned handle.
        /// If the function fails, the return value is NULL.</returns>      
        [DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr GetThreadDesktop(uint dwThread);

        /// <summary>
        /// Assigns the specified desktop to the calling thread. All subsequent 
        /// operations on the desktop use the access rights granted to the desktop.
        /// </summary>
        /// <param name="hDesktop">[in] Handle to the desktop to be assigned to 
        /// the calling thread.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero. </returns>
        [DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool SetThreadDesktop(IntPtr hDesktop);

        /// <summary>
        /// Opens the specified window station.
        /// </summary>
        /// <param name="lpszWinSta">Pointer to a null-terminated string specifying 
        /// the name of the window station to be opened. Window station names are 
        /// case-insensitive. This window station must belong to the current session.
        /// </param>
        /// <param name="fInherit">[in] If this value is TRUE, processes created 
        /// by this process will inherit the handle. Otherwise, the processes 
        /// do not inherit this handle. 
        /// </param>
        /// <param name="dwDesiredAccess">[in] Access to the window station</param>
        /// <returns>If the function succeeds, the return value is the handle 
        /// to the specified window station. If the function fails, the return 
        /// value is NULL.</returns>
        [DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr OpenWindowStation(string lpszWinStation, 
            bool bInherit, WindowStationAccessRight dwDesireAccess);

        /// <summary>
        /// Closes an open window station handle.
        /// </summary>
        /// <param name="hWinSta">[in] Handle to the window station to 
        /// be closed.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero. </returns>
        [DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr CloseWindowStation(IntPtr hWinStation);

        /// <summary>
        /// Opens the specified desktop object.
        /// </summary>
        /// <param name="lpszDesktop">[in] Pointer to null-terminated string 
        /// specifying the name of the desktop to be opened. Desktop names are 
        /// case-insensitive. This desktop must belong to the current window 
        /// station.</param>
        /// <param name="dwFlags">[in] This parameter can be zero or 
        /// DF_ALLOWOTHERACCOUNTHOOK=0x0001</param>
        /// <param name="fInherit">[in] If this value is TRUE, processes created 
        /// by this process will inherit the handle. Otherwise, the processes 
        /// do not inherit this handle. </param>
        /// <param name="dwDesiredAccess">[in] Access to the desktop. For a list 
        /// of access rights</param>
        /// <returns>If the function succeeds, the return value is a handle to 
        /// the opened desktop. When you are finished using the handle, call the 
        /// CloseDesktop function to close it. If the function fails, the return 
        /// value is NULL.
        /// </returns>
        [DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr OpenDesktop(string lpszDesktop, 
            OpenDesktopFlag dwFlag, bool bInherit, DesktopAccessRight dwDesiredAccess);

        /// <summary>
        /// Closes an open handle to a desktop object.
        /// </summary>
        /// <param name="hDesktop">[in] Handle to the desktop to be closed.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero. </returns>
        [DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr CloseDesktop(IntPtr hDesktop);

        /// <summary>
        /// The GetDesktopWindow function returns a handle to the desktop window. 
        /// The desktop window covers the entire screen. The desktop window is 
        /// the area on top of which other windows are painted. 
        /// </summary>
        /// <returns>The return value is a handle to the desktop window. </returns>
        [DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr GetDesktopWindow();
    }
    #endregion
}
#endregion