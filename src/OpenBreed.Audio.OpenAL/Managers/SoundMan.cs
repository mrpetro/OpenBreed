using OpenBreed.Audio.Interface.Managers;
using OpenBreed.Common.Logging;
using OpenTK;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OpenBreed.Audio.OpenAL.Managers
{
    public class SoundMan : ISoundMan, IDisposable
    {
        #region Private Fields

        private readonly Dictionary<int, int> alBuffers = new Dictionary<int, int>();
        private readonly Dictionary<string, int> sampleNames = new Dictionary<string, int>();
        private readonly Dictionary<int, int> alSources = new Dictionary<int, int>();
        private readonly ILogger logger;
        private AudioContext audioContext;

        private bool disposedValue;

        #endregion Private Fields

        #region Public Constructors

        public SoundMan(ILogger logger)
        {
            audioContext = new AudioContext();
            audioContext.MakeCurrent();
            this.logger = logger;

            ReportOpenAL();
        }

        #endregion Public Constructors

        #region Public Methods

        public int CreateSoundSource(float posX, float posY, float posZ)
        {
            int alSource;
            AL.GenSources(1, out alSource);

            var pos = new Vector3(posX, posY, posZ);

            AL.Source(alSource, ALSource3f.Position, ref pos);

            alSources.Add(alSources.Count, alSource);
            return alSources.Count - 1;
        }

        public int CreateSoundSource()
        {
            int alSource;
            AL.GenSources(1, out alSource);

            alSources.Add(alSources.Count, alSource);
            return alSources.Count - 1;
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public int LoadSample(string sampleName, string sampleFilePath, int sampleFreq)
        {
            var sampleData = File.ReadAllBytes(sampleFilePath);
            return LoadSample(sampleName, sampleData, sampleFreq);
        }

        public int LoadSample(string sampleName, byte[] sampleData, int sampleFreq)
        {
            int sampleBuffer;
            AL.GenBuffers(1, out sampleBuffer);
            AL.BufferData(sampleBuffer, ALFormat.Mono8, sampleData, sampleData.Length, sampleFreq);

            alBuffers.Add(alBuffers.Count, sampleBuffer);
            sampleNames.Add(sampleName, alBuffers.Count - 1);

            return alBuffers.Count - 1;
        }

        public void PlaySample(int sampleId)
        {
            if (sampleId == -1)
                return;

            var alBuffer = GetSampleBufferId(sampleId);
            var alSource = GetFirstIdleSource();

            if (alSource == -1)
            {
                Console.WriteLine("No idle source available for playing.");
                return;
            }

            Console.WriteLine($"Playing sample '{sampleId}' at source '{alSource}'");

            AL.Source(alSource, ALSourcei.Buffer, alBuffer);
            AL.Source(alSource, ALSourceb.Looping, false);

            //AL.SourceQueueBuffer(alSource, alBuffer);
            AL.SourcePlay(alSource);

            //var state = AL.GetSourceState(alSource);

            //while (state == ALSourceState.Playing)
            //{
            //    state = AL.GetSourceState(alSource);
            //}

            //Task.Run(() => AL.SourcePlay(alSource));
        }

        public void PlaySampleAtSource(int sampleId, int sourceId)
        {
            Console.WriteLine($"Playing sample '{sampleId}' at source '{sourceId}'");

            var alBuffer = GetSampleBufferId(sampleId);
            var alSource = GetSoundSourceId(sourceId);

            AL.Source(alSource, ALSourcei.Buffer, alBuffer);
            AL.Source(alSource, ALSourceb.Looping, false);

            //AL.SourceQueueBuffer(alSource, alBuffer);
            AL.SourcePlay(alSource);

            var state = AL.GetSourceState(alSource);

            while (state == ALSourceState.Playing)
            {
                state = AL.GetSourceState(alSource);
            }

            //Task.Run(() => AL.SourcePlay(alSource));
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    audioContext.Dispose();
                    audioContext = null;
                }

                disposedValue = true;
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private void ReportOpenAL()
        {
            var version = AL.Get(ALGetString.Version);
            var vendor = AL.Get(ALGetString.Vendor);
            var renderer = AL.Get(ALGetString.Renderer);

            var reportBuilder = new StringBuilder();
            reportBuilder.AppendLine("Open AL info:");
            reportBuilder.AppendLine(version);
            reportBuilder.AppendLine(vendor);
            reportBuilder.AppendLine(renderer);
            logger.Info(reportBuilder.ToString());
        }

        private int GetFirstIdleSource()
        {
            foreach (var alSource in alSources.Values)
            {
                var state = AL.GetSourceState(alSource);

                if (state != ALSourceState.Playing)
                    return alSource;
            }

            return -1;
        }

        private int GetSampleBufferId(int sampleId)
        {
            if (alBuffers.TryGetValue(sampleId, out int sampleBufferId))
                return sampleBufferId;
            else
                throw new InvalidOperationException($"Unable to find OpenAL buffer ID for sample '{sampleId}'");
        }

        private int GetSoundSourceId(int soundSourceId)
        {
            if (alSources.TryGetValue(soundSourceId, out int sampleBufferId))
                return sampleBufferId;
            else
                throw new InvalidOperationException($"Unable to find OpenAL source ID for sound source '{soundSourceId}'");
        }

        public int GetByName(string sampleName)
        {
            if (sampleNames.TryGetValue(sampleName, out int result))
                return result;
            else
                return -1;
        }

        #endregion Private Methods
    }
}