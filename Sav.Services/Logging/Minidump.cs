using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Savchin.Services.Logging
{
    public static class MiniDump
    {

        // Taken almost verbatim from http://blog.kalmbach-software.de/2008/12/13/writing-minidumps-in-c/

        [Flags]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
        public enum Options : uint
        {
            // From dbghelp.h:
            Normal = 0x00000000,
            WithDataSegments = 0x00000001,
            WithFullMemory = 0x00000002,
            WithHandleData = 0x00000004,
            FilterMemory = 0x00000008,
            ScanMemory = 0x00000010,
            WithUnloadedModules = 0x00000020,
            WithIndirectlyReferencedMemory = 0x00000040,
            FilterModulePaths = 0x00000080,
            WithProcessThreadData = 0x00000100,
            WithPrivateReadWriteMemory = 0x00000200,
            WithoutOptionalData = 0x00000400,
            WithFullMemoryInfo = 0x00000800,
            WithThreadInfo = 0x00001000,
            WithCodeSegments = 0x00002000,
            WithoutAuxiliaryState = 0x00004000,
            WithFullAuxiliaryState = 0x00008000,
            WithPrivateWriteCopyMemory = 0x00010000,
            IgnoreInaccessibleMemory = 0x00020000,
            ValidTypeFlags = 0x0003ffff
        };

        public enum ExceptionInfo
        {
            None,
            Present
        }

        //typedef struct _MINIDUMP_EXCEPTION_INFORMATION {
        //    DWORD ThreadId;
        //    PEXCEPTION_POINTERS ExceptionPointers;
        //    BOOL ClientPointers;
        //} MINIDUMP_EXCEPTION_INFORMATION, *PMINIDUMP_EXCEPTION_INFORMATION;
        [StructLayout(LayoutKind.Sequential, Pack = 4)]  // Pack=4 is important! So it works also for x64!
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1049:TypesThatOwnNativeResourcesShouldBeDisposable")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes")]
        private struct MiniDumpExceptionInformation
        {
            public uint ThreadId;
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources")]
            public IntPtr ExceptionPointers;
            [MarshalAs(UnmanagedType.Bool)]
            public bool ClientPointers;
        }


        //BOOL
        //WINAPI
        //MiniDumpWriteDump(
        //    __in HANDLE hProcess,
        //    __in DWORD ProcessId,
        //    __in HANDLE hFile,
        //    __in MINIDUMP_TYPE DumpType,
        //    __in_opt PMINIDUMP_EXCEPTION_INFORMATION ExceptionParam,
        //    __in_opt PMINIDUMP_USER_STREAM_INFORMATION UserStreamParam,
        //    __in_opt PMINIDUMP_CALLBACK_INFORMATION CallbackParam
        //    );
        // Overload requiring MiniDumpExceptionInformation
        [DllImport("dbghelp.dll", EntryPoint = "MiniDumpWriteDump", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool MiniDumpWriteDump(IntPtr hProcess, uint processId, SafeHandle hFile, uint dumpType, ref MiniDumpExceptionInformation expParam, IntPtr userStreamParam, IntPtr callbackParam);


        // Overload supporting MiniDumpExceptionInformation == NULL
        [DllImport("dbghelp.dll", EntryPoint = "MiniDumpWriteDump", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool MiniDumpWriteDump(IntPtr hProcess, uint processId, SafeHandle hFile, uint dumpType, IntPtr expParam, IntPtr userStreamParam, IntPtr callbackParam);


        [DllImport("kernel32.dll", EntryPoint = "GetCurrentThreadId", ExactSpelling = true)]
        private static extern uint GetCurrentThreadId();



        /// <summary>
        /// Writes the specified file handle.
        /// </summary>
        /// <param name="fileHandle">The file handle.</param>
        /// <param name="options">The options.</param>
        /// <param name="exceptionInfo">The exception info.</param>
        /// <returns></returns>
        public static bool Write(SafeHandle fileHandle, Options options, ExceptionInfo exceptionInfo)
        {
            Process currentProcess = Process.GetCurrentProcess();
            IntPtr currentProcessHandle = currentProcess.Handle;
            uint currentProcessId = (uint)currentProcess.Id;
            MiniDumpExceptionInformation exp;
            exp.ThreadId = GetCurrentThreadId();
            exp.ClientPointers = false;
            exp.ExceptionPointers = IntPtr.Zero;

            if (exceptionInfo == ExceptionInfo.Present)
            {
                exp.ExceptionPointers = Marshal.GetExceptionPointers();
            }

            bool bRet = false;

            bRet = exp.ExceptionPointers == IntPtr.Zero ?
                MiniDumpWriteDump(currentProcessHandle, currentProcessId, fileHandle, (uint)options, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero) :
                MiniDumpWriteDump(currentProcessHandle, currentProcessId, fileHandle, (uint)options, ref exp, IntPtr.Zero, IntPtr.Zero);

            return bRet;

        }



        /// <summary>
        /// Writes the specified file handle.
        /// </summary>
        /// <param name="fileHandle">The file handle.</param>
        /// <param name="dumpType">Type of the dump.</param>
        /// <returns></returns>
        public static bool Write(SafeHandle fileHandle, Options dumpType)
        {
            return Write(fileHandle, dumpType, ExceptionInfo.None);
        }

        /// <summary>
        /// Writes the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="dumpType">Type of the dump.</param>
        /// <param name="exceptionInfo">The exception info.</param>
        /// <returns></returns>
        public static bool Write(string fileName, Options dumpType, ExceptionInfo exceptionInfo)
        {
            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Write))
            {
                return Write(fs.SafeFileHandle, dumpType, exceptionInfo);
            }
        }

        /// <summary>
        /// Attaches the error listener.
        /// </summary>
        public static void AttachErrorListener()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;
        }
        /// <summary>
        /// Detaches the error listener.
        /// </summary>
        public static void DetachErrorListener()
        {
            AppDomain.CurrentDomain.UnhandledException -= CurrentDomainUnhandledException;
        }

        private static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                Write(string.Format("error{0}.dmp", DateTime.Now.ToString("yyyyMMddHHmmssFF")), Options.WithFullMemoryInfo, ExceptionInfo.Present);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                AppDomain.CurrentDomain.UnhandledException -= CurrentDomainUnhandledException;
            }
        }
    }
}
