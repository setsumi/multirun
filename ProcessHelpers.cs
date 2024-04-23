using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace multirun
{
    internal static class ProcessHelpers
    {
        public static List<Process> GetDescendantProcesses(int parentProcessId)
        {
            var descendants = new List<Process>();
            if (parentProcessId > 0)
            {
                var allProcesses = Process.GetProcesses();
                var children = allProcesses.Where(p => p.Parent()?.Id == parentProcessId);
                foreach (var child in children)
                {
                    descendants.Add(child);
                    // Recursively find all descendants of the child process
                    descendants.AddRange(GetDescendantProcesses(child.Id));
                }
            }
            return descendants;
        }

        // Process object extension method
        public static Process Parent(this Process process)
        {
            IntPtr handle;
            try { handle = process.Handle; }
            catch { return null; }
            return GetParentProcess(handle);
        }

        public static Process GetParentProcess()
        {
            return GetParentProcess(Process.GetCurrentProcess().Handle);
        }

        public static Process GetParentProcess(int id)
        {
            Process process;
            try { process = Process.GetProcessById(id); }
            catch (ArgumentException) { return null; }
            return GetParentProcess(process.Handle);
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct PROCESS_BASIC_INFORMATION
        {
            internal IntPtr Reserved1;
            internal IntPtr PebBaseAddress;
            internal IntPtr Reserved2_0;
            internal IntPtr Reserved2_1;
            internal IntPtr UniqueProcessId;
            internal IntPtr InheritedFromUniqueProcessId;
        }

        [DllImport("ntdll.dll")]
        private static extern int NtQueryInformationProcess(
            IntPtr processHandle,
            int processInformationClass,
            ref PROCESS_BASIC_INFORMATION processInformation,
            int processInformationLength,
            out int returnLength
        );

        public static Process GetParentProcess(IntPtr handle)
        {
            PROCESS_BASIC_INFORMATION pbi = new PROCESS_BASIC_INFORMATION();
            int returnLength;
            int status = NtQueryInformationProcess(handle, 0, ref pbi, Marshal.SizeOf(pbi), out returnLength);
            if (status != 0)
                throw new Win32Exception(status);

            try
            {
                return Process.GetProcessById(pbi.InheritedFromUniqueProcessId.ToInt32());
            }
            catch (ArgumentException)
            {
                return null;
            }
        }
    }
}
