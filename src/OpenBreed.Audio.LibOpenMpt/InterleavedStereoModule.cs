using System;
using System.IO;
using System.Runtime.InteropServices;

namespace OpenBreed.Audio.LibOpenMpt
{
    public class InterleavedStereoModule
    {
        #region Private Fields

        /// <summary>
        /// For stereo it's 2
        /// </summary>
        private const int BUFFER_FACTOR = 2;

        private readonly IntPtr modPtr;

        private IntPtr readBufferPtr;

        private long bufferSize;

        #endregion Private Fields

        #region Public Constructors

        public InterleavedStereoModule(string path) : this(File.ReadAllBytes(path))
        {
        }

        public InterleavedStereoModule(byte[] data)
        {
            var dataPtr = Marshal.AllocHGlobal(data.Length * sizeof(byte));

            try
            {
                Marshal.Copy(data, 0, dataPtr, data.Length);

                modPtr = Import.ModuleCreateFromMemory2(dataPtr,
                                                        (UIntPtr)data.LongLength,
                                                        IntPtr.Zero,
                                                        IntPtr.Zero,
                                                        IntPtr.Zero,
                                                        IntPtr.Zero,
                                                        IntPtr.Zero,
                                                        IntPtr.Zero,
                                                        IntPtr.Zero);
            }
            finally
            {
                Marshal.FreeHGlobal(dataPtr);
            }

            if (modPtr == IntPtr.Zero)
                throw new InvalidOperationException("Unable to load module");

            ReadBufferResize(0);
        }

        #endregion Public Constructors

        #region Private Destructors

        ~InterleavedStereoModule()
        {
            ReadBufferFree();

            Import.ModuleDestroy(modPtr);
        }

        #endregion Private Destructors

        #region Public Methods

        public double GetDurationSeconds()
        {
            return Import.ModuleGetDurationSeconds(modPtr);
        }

        public double GetCurrentPositionSeconds()
        {
            return Import.ModuleGetPositionSeconds(modPtr);
        }

        public bool SetRepeatCount(int count)
        {
            int success = Import.ModuleSetRepeatCount(modPtr, count);
            return success != 0;
        }

        public int Read(int sampleRate, long count, short[] data)
        {
            if (count > data.LongLength)
            {
                throw new InvalidOperationException(
                       $"data of size {data.LongLength} is not large enough for output of size {count}"
                    );
            }
            if (count > bufferSize)
                ReadBufferResize((int)count);

            var actualNumberFrames = Import.ModuleReadInterleavedStereo(modPtr,
                                                                         sampleRate,
                                                                         (UIntPtr)count,
                                                                         readBufferPtr);

            ReadBufferCopy(data, actualNumberFrames);

            return actualNumberFrames;
        }

        public String ErrorGetLastMessage()
        {
            IntPtr ptr = Import.ErrorGetLastMessage(modPtr);
            return Marshal.PtrToStringAnsi(ptr);
        }

        #endregion Public Methods

        #region Private Methods

        private void ReadBufferCopy(short[] targetArray, int count)
        {
            Marshal.Copy(readBufferPtr, targetArray, 0, count * BUFFER_FACTOR);
        }

        private void ReadBufferAlloc(int size)
        {
            readBufferPtr = Marshal.AllocHGlobal(size * sizeof(short) * BUFFER_FACTOR);
        }

        private void ReadBufferResize(int size)
        {
            ReadBufferFree();
            ReadBufferAlloc(size * sizeof(short) * BUFFER_FACTOR);
            bufferSize = size;
        }

        private void ReadBufferFree()
        {
            if (readBufferPtr != IntPtr.Zero)
                Marshal.FreeHGlobal(readBufferPtr);
        }

        #endregion Private Methods
    }
}