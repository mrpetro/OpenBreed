using OpenBreed.Audio.Interface;
using OpenBreed.Audio.Interface.Managers;
using OpenBreed.Common.Logging;
using OpenTK;
using OpenTK.Audio.OpenAL;
using System;

namespace OpenBreed.Audio.OpenAL.Managers
{
    public class SoundMan : ISoundMan
    {
        #region Private Fields

        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public SoundMan(ILogger logger)
        {
            this.logger = logger;
        }

        #endregion Public Constructors

        #region Public Methods

        public ISound GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ISound Load(string filePath, string id = null)
        {
            throw new NotImplementedException();
        }

        public void UnloadAll()
        {
            throw new NotImplementedException();
        }

        public void PlaySound(int id)
        {
            //Initialize
            var device = Alc.OpenDevice(null);
            var context = Alc.CreateContext(device, (int[])null);

            Alc.MakeContextCurrent(context);

            var version = AL.Get(ALGetString.Version);
            var vendor = AL.Get(ALGetString.Vendor);
            var renderer = AL.Get(ALGetString.Renderer);
            Console.WriteLine(version);
            Console.WriteLine(vendor);
            Console.WriteLine(renderer);
            Console.ReadKey();

            //Process
            int buffers, source;
            AL.GenBuffers(1, out buffers);
            AL.GenSources(1, out source);

            int sampleFreq = 44100;
            double dt = 2 * Math.PI / sampleFreq;
            double amp = 0.5;

            int freq = 440;
            var dataCount = sampleFreq / freq;

            var sinData = new short[dataCount];
            for (int i = 0; i < sinData.Length; ++i)
            {
                sinData[i] = (short)(amp * short.MaxValue * Math.Sin(i * dt * freq));
            }
            AL.BufferData(buffers, ALFormat.Mono16, sinData, sinData.Length, sampleFreq);
            AL.Source(source, ALSourcei.Buffer, buffers);
            AL.Source(source, ALSourceb.Looping, true);

            AL.SourcePlay(source);
            Console.ReadKey();

            ///Dispose
            if (context != ContextHandle.Zero)
            {
                Alc.MakeContextCurrent(ContextHandle.Zero);
                Alc.DestroyContext(context);
            }
            context = ContextHandle.Zero;

            if (device != IntPtr.Zero)
            {
                Alc.CloseDevice(device);
            }
            device = IntPtr.Zero;
        }

        #endregion Public Methods
    }
}