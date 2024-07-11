using System;

using System.Runtime.InteropServices;

namespace OpenBreed.Audio.LibOpenMpt
{
    internal class Import
    {
        #region Private Fields

        private const string TARGET = "libopenmpt.dll";

        #endregion Private Fields

        #region Public Methods

        [DllImport(TARGET, EntryPoint = "openmpt_module_create_from_memory2")]
        public static extern IntPtr ModuleCreateFromMemory2(
            IntPtr fileData, UIntPtr fileSize, IntPtr logFunc, IntPtr logUser,
            IntPtr errFunc, IntPtr errUser, IntPtr error, IntPtr errorMessage, IntPtr ctls);

        [DllImport(TARGET, EntryPoint = "openmpt_module_destroy")]
        public static extern void ModuleDestroy(IntPtr mod);

        [DllImport(TARGET, EntryPoint = "openmpt_module_error_get_last_message")]
        public static extern IntPtr ErrorGetLastMessage(IntPtr mod);

        [DllImport(TARGET, EntryPoint = "openmpt_module_set_repeat_count")]
        public static extern Int32 ModuleSetRepeatCount(IntPtr mod, Int32 repeatCount);

        [DllImport(TARGET, EntryPoint = "openmpt_module_read_interleaved_stereo")]
        public static extern Int32 ModuleReadInterleavedStereo(IntPtr mod, Int32 sampleRate, UIntPtr count, IntPtr interleavedStereo);

        [DllImport(TARGET, EntryPoint = "openmpt_module_get_duration_seconds")]
        public static extern double ModuleGetDurationSeconds(IntPtr mod);

        [DllImport(TARGET, EntryPoint = "openmpt_module_get_position_seconds")]
        public static extern double ModuleGetPositionSeconds(IntPtr mod);


        #endregion Public Methods
    }
}