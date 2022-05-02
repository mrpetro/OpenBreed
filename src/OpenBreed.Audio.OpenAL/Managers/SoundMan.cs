using OpenBreed.Audio.Interface.Managers;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using OpenTK;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBreed.Audio.OpenAL.Managers
{
    public class SoundMan : ISoundMan, IDisposable
    {
        #region Private Fields

        private readonly Dictionary<int, int> alBuffers = new Dictionary<int, int>();
        private readonly Dictionary<int, SampleStream> sampleStreams = new Dictionary<int, SampleStream>();
        private readonly Dictionary<string, int> sampleNames = new Dictionary<string, int>();
        private readonly Dictionary<string, int> streamNames = new Dictionary<string, int>();
        private readonly List<SoundSource> alSources = new List<SoundSource>();

        private readonly ILogger logger;
        private readonly List<SoundSource> streamSources = new List<SoundSource>();

        private readonly ALDevice alDevice;
        private ALContext alContext;

        private bool disposedValue;

        #endregion Private Fields

        #region Public Constructors

        public SoundMan(ILogger logger)
        {
            alDevice = ALC.OpenDevice(null);
            alContext = ALC.CreateContext(alDevice, new ALContextAttributes());
            this.logger = logger;

            ALC.MakeContextCurrent(alContext);
            ReportOpenAL();
        }

        #endregion Public Constructors

        #region Public Methods

        public int CreateSoundSource(float posX, float posY, float posZ)
        {
            var alSource = AL.GenSource();

            var pos = new Vector3(posX, posY, posZ);

            AL.Source(alSource, ALSource3f.Position, ref pos);

            var newSoundSource = new SoundSource(alSources.Count, alSource);
            alSources.Add(newSoundSource);
            return newSoundSource.Id;
        }

        public int CreateSoundSource() => CreateSoundSource(0, 0, 0);

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public int LoadSample(string sampleName, byte[] sampleData, int sampleFreq)
        {
            var sampleBuffer = AL.GenBuffer();

            AL.BufferData(sampleBuffer, ALFormat.Mono8, ref sampleData[0], sampleData.Length, sampleFreq);

            alBuffers.Add(alBuffers.Count, sampleBuffer);
            sampleNames.Add(sampleName, alBuffers.Count - 1);

            return alBuffers.Count - 1;
        }

        public int LoadSample(string sampleName, short[] sampleData, int sampleFreq)
        {
            var sampleBuffer = AL.GenBuffer();
            AL.BufferData(sampleBuffer, ALFormat.Stereo16, ref sampleData[0], sampleData.Length * 2, sampleFreq);

            alBuffers.Add(alBuffers.Count, sampleBuffer);
            sampleNames.Add(sampleName, alBuffers.Count - 1);

            return alBuffers.Count - 1;
        }

        public int CreateStream(string streamName, SoundStreamReader reader)
        {
            var buffers = AL.GenBuffers(4);
            var sampleStream = new SampleStream(buffers, reader);

            sampleStreams.Add(sampleStreams.Count, sampleStream);
            streamNames.Add(streamName, sampleStreams.Count - 1);

            return sampleStreams.Count - 1;
        }

        public void PlayStream(int sampleStreamId)
        {
            if (sampleStreamId == -1)
                return;

            var streamSource = GetSampleStream(sampleStreamId);

            var soundSource = GetFirstIdleSource();

            if (soundSource is null)
            {
                Console.WriteLine("No idle source available for playing.");
                return;
            }

            streamSource.PlayAtSource(soundSource);
            AL.SourceStop(soundSource.ALSourceId);
            AL.SourcePlay(soundSource.ALSourceId);

            if (streamSources.Contains(soundSource))
                return;

            streamSources.Add(soundSource);
        }

        public void Update()
        {
            foreach (var soundSource in streamSources)
            {
                UpdateBuffers(soundSource);
            }
        }

        public void PlaySample(int sampleId)
        {
            if (sampleId == -1)
                return;

            var alBuffer = GetSampleBufferId(sampleId);
            var soundSource = GetFirstIdleSource();

            if (soundSource is null)
            {
                Console.WriteLine("No idle source available for playing.");
                return;
            }

            var alSource = soundSource.ALSourceId;

            Console.WriteLine($"Playing sample '{sampleId}' at source '{alSource}'");

            AL.Source(alSource, ALSourcei.Buffer, alBuffer);
            AL.Source(alSource, ALSourceb.Looping, false);

            AL.SourcePlay(alSource);
        }

        public void PlaySampleAtSource(int sampleId, int sourceId)
        {
            Console.WriteLine($"Playing sample '{sampleId}' at source '{sourceId}'");

            var alBuffer = GetSampleBufferId(sampleId);

            var soundSource = GetSoundSource(sourceId);

            var alSource = soundSource.ALSourceId;

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

        public int GetByName(string sampleName)
        {
            if (sampleNames.TryGetValue(sampleName, out int result))
                return result;
            else
                return -1;
        }

        #endregion Public Methods

        #region Internal Methods

        internal SoundSource GetSoundSource(int sourceId)
        {
            if (sourceId < 0 || sourceId >= alSources.Count)
                throw new InvalidOperationException($"No AL source exists for source '{sourceId}'.");

            return alSources[sourceId];
        }

        #endregion Internal Methods

        #region Protected Methods

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    //audioContext.Dispose();
                    //audioContext = null;
                }

                disposedValue = true;
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private void UpdateBuffers(SoundSource soundSource)
        {
            var alSource = soundSource.ALSourceId;

            AL.GetSource(alSource, ALGetSourcei.BuffersProcessed, out int buffersProcessed);

            if (buffersProcessed <= 0)
                return;

            var state = AL.GetSourceState(alSource);

            while (buffersProcessed-- > 0)
            {
                var bufferArray = new int[1];
                AL.SourceUnqueueBuffers(alSource, 1, bufferArray);

                var dataSize = soundSource.CurrentStream.FillBuffer(bufferArray[0]);

                if (dataSize > 0)
                    AL.SourceQueueBuffers(alSource, 1, bufferArray);
            }

            if (state != ALSourceState.Playing)
            {
                AL.SourceStop(soundSource.ALSourceId);
                AL.SourcePlay(soundSource.ALSourceId);
            }
        }

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

            reportBuilder.AppendLine("Extensions:");
            if (AL.IsExtensionPresent("AL_EXT_float32"))
                reportBuilder.AppendLine("AL_EXT_float32");

            logger.Info(reportBuilder.ToString());
        }

        private SoundSource GetFirstIdleSource()
        {
            foreach (var soundSource in alSources)
            {
                if (soundSource.CurrentStream != null)
                    continue;

                var alSource = soundSource.ALSourceId;

                var state = AL.GetSourceState(alSource);

                if (state != ALSourceState.Playing)
                    return soundSource;
            }

            return null;
        }

        private int GetSampleBufferId(int sampleId)
        {
            if (alBuffers.TryGetValue(sampleId, out int sampleBufferId))
                return sampleBufferId;
            else
                throw new InvalidOperationException($"Unable to find OpenAL buffer ID for sample '{sampleId}'");
        }

        private SampleStream GetSampleStream(int sampleStreamId)
        {
            if (sampleStreams.TryGetValue(sampleStreamId, out SampleStream sampleStream))
                return sampleStream;
            else
                throw new InvalidOperationException($"Unable to find sample stream '{sampleStreamId}'");
        }

        #endregion Private Methods
    }
}