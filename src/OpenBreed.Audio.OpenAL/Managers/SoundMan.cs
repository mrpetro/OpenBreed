using Microsoft.Extensions.Logging;
using OpenBreed.Audio.Abstractions.Managers;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using OpenBreed.Core.Abstractions.Events;
using OpenBreed.Core.Abstractions.Managers;
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

        private readonly Dictionary<int, SoundSample> alSamples = new Dictionary<int, SoundSample>();
        private readonly Dictionary<int, SoundStream> alStreams = new Dictionary<int, SoundStream>();
        private readonly Dictionary<string, int> sampleNames = new Dictionary<string, int>();
        private readonly Dictionary<string, int> streamNames = new Dictionary<string, int>();
        private readonly List<SoundSource> alSources = new List<SoundSource>();

        private readonly ILogger logger;
        private readonly IEventsMan eventsMan;
        private readonly List<SoundSource> streamSources = new List<SoundSource>();

        private readonly ALDevice alDevice;
        private ALContext alContext;

        private bool disposedValue;

        #endregion Private Fields

        #region Public Constructors

        public SoundMan(ILogger logger, IEventsMan eventsMan)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.eventsMan = eventsMan ?? throw new ArgumentNullException(nameof(eventsMan));

            alDevice = ALC.OpenDevice(null);
            alContext = ALC.CreateContext(alDevice, new ALContextAttributes());

            ALC.MakeContextCurrent(alContext);
            ReportOpenAL();

            this.eventsMan.Subscribe<WindowUpdateEvent>((e) => OnUpdate(e.Dt));
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
            var alBufferId = AL.GenBuffer();

            AL.BufferData(alBufferId, ALFormat.Mono8, ref sampleData[0], sampleData.Length, sampleFreq);

            return RegisterSample(sampleName, alBufferId);
        }

        public int LoadSample(string sampleName, short[] sampleData, int sampleFreq)
        {
            var alBufferId = AL.GenBuffer();
            AL.BufferData(alBufferId, ALFormat.Stereo16, ref sampleData[0], sampleData.Length * 2, sampleFreq);

            return RegisterSample(sampleName, alBufferId);
        }

        public int CreateStream(string streamName, SoundStreamReader reader)
        {
            var buffers = AL.GenBuffers(4);
            var sampleStream = new SoundStream(buffers, reader)
            {
                Id = alStreams.Count
            };

            alStreams.Add(sampleStream.Id, sampleStream);
            streamNames.Add(streamName, sampleStream.Id);

            return sampleStream.Id;
        }

        public void PlayStream(int streamId)
        {
            if (streamId == -1)
            {
                return;
            }

            if (!TryGetStream(streamId, out var stream))
            {
                throw new InvalidOperationException($"Unable to find sound stream with Id '{streamId}'.");
            }

            var soundSource = GetFirstIdleSource();

            if (soundSource is null)
            {
                Console.WriteLine("No idle source available for playing.");
                return;
            }

            stream.PlayAtSource(soundSource);
            AL.SourceStop(soundSource.ALSourceId);
            AL.SourcePlay(soundSource.ALSourceId);

            if (streamSources.Contains(soundSource))
                return;

            streamSources.Add(soundSource);
        }

        public void PlaySample(int sampleId)
        {
            if (sampleId == -1)
            {
                return;
            }

            if (!TryGetSample(sampleId, out var sample))
            {
                throw new InvalidOperationException($"Unable to find sample with Id'{sampleId}'.");
            }

            var soundSource = GetFirstIdleSource();

            if (soundSource is null)
            {
                Console.WriteLine("No idle source available for playing.");
                return;
            }

            var alSource = soundSource.ALSourceId;

            Console.WriteLine($"Playing sample '{sample.Name}' at source '{alSource}'");

            AL.Source(alSource, ALSourcei.Buffer, sample.AlBufferId);
            AL.Source(alSource, ALSourceb.Looping, false);

            AL.SourcePlay(alSource);
        }

        public void PlaySampleAtSource(int sampleId, int sourceId)
        {
            if (sampleId == -1)
            {
                return;
            }

            if (!TryGetSample(sampleId, out var sample))
            {
                throw new InvalidOperationException($"Unable to find sample with Id'{sampleId}'.");
            }

            Console.WriteLine($"Playing sample '{sample.Name}' at source '{sourceId}'");

            var soundSource = GetSoundSource(sourceId);

            var alSource = soundSource.ALSourceId;

            AL.Source(alSource, ALSourcei.Buffer, sample.AlBufferId);
            AL.Source(alSource, ALSourceb.Looping, false);

            //AL.SourceQueueBuffer(alSource, alBuffer);
            AL.SourcePlay(alSource);

            var state = ALTools.GetSourceState(alSource);

            while (state == ALSourceState.Playing)
            {
                state = ALTools.GetSourceState(alSource);
            }

            //Task.Run(() => AL.SourcePlay(alSource));
        }

        public int GetDuration(int sampleId)
        {
            if (sampleId == -1)
            {
                return 0;
            }

            if (!TryGetSample(sampleId, out var sample))
            {
                throw new InvalidOperationException($"Unable to find sample with Id'{sampleId}'.");
            }

            var alBufferId = sample.AlBufferId;

            AL.GetBuffer(alBufferId, ALGetBufferi.Size, out int sizeInBytes);
            AL.GetBuffer(alBufferId, ALGetBufferi.Channels, out int channels);
            AL.GetBuffer(alBufferId, ALGetBufferi.Bits, out int bits);

            var lengthInSamples = sizeInBytes * 8 / (channels * bits);

            AL.GetBuffer(alBufferId, ALGetBufferi.Frequency, out int frequency);

            var durationInSeconds = (float)lengthInSamples / (float)frequency;

            return (int)(durationInSeconds * 1000.0f);
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

        private int RegisterSample(string sampleName, int alBufferId)
        {
            var sample = new SoundSample
            {
                Id = alSamples.Count,
                AlBufferId = alBufferId,
                Name = sampleName
            };

            alSamples.Add(sample.Id, sample);
            sampleNames.Add(sampleName, sample.Id);

            return sample.Id;
        }

        private void OnUpdate(float dt)
        {
            foreach (var soundSource in streamSources)
            {
                UpdateBuffers(soundSource);
            }
        }

        private void UpdateBuffers(SoundSource soundSource)
        {
            var alSource = soundSource.ALSourceId;

            AL.GetSource(alSource, ALGetSourcei.BuffersProcessed, out int buffersProcessed);

            if (buffersProcessed <= 0)
                return;

            var state = ALTools.GetSourceState(alSource);

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

            logger.LogInformation(reportBuilder.ToString());
        }

        private SoundSource GetFirstIdleSource()
        {
            foreach (var soundSource in alSources)
            {
                if (soundSource.CurrentStream != null)
                    continue;

                var alSource = soundSource.ALSourceId;

                var state = ALTools.GetSourceState(alSource);

                if (state != ALSourceState.Playing)
                    return soundSource;
            }

            return null;
        }

        private bool TryGetSample(int sampleId, out SoundSample sample)
        {
            return alSamples.TryGetValue(sampleId, out sample);
        }

        private bool TryGetStream(int sampleStreamId, out SoundStream stream)
        {
            return alStreams.TryGetValue(sampleStreamId, out stream);
        }

        #endregion Private Methods
    }
}