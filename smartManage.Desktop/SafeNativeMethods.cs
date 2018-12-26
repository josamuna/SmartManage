using System;
using System.Runtime.InteropServices;
using System.Security;

namespace smartManage.Desktop
{
    [SuppressUnmanagedCodeSecurity]
    public static class SafeNativeMethods
    {
        /// <summary>
        /// Permet la reduction de l'utilisation de la mémoire vive
        /// </summary>
        /// <param name="hProcess">Pointeur</param>
        /// <param name="dwMinimumWorkingSetSize">Entier</param>
        /// <param name="dwMaximumWorkingSetSize">Entier</param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        internal static extern bool SetProcessWorkingSetSize(IntPtr hProcess, UIntPtr dwMinimumWorkingSetSize, UIntPtr dwMaximumWorkingSetSize);
    }
}
