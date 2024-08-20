using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Interface.Tools
{
    public class MovingAverage
    {
        #region Private Fields

        private readonly int samplesCount;

        private readonly float[] samplesArray;

        private int sampleIndex = 0;

        private float samplesSum = 0;

        #endregion Private Fields

        #region Public Constructors

        public MovingAverage(int samplesCount = 60)
        {
            this.samplesCount = samplesCount;
            this.samplesArray = new float[samplesCount];
        }

        #endregion Public Constructors

        #region Public Properties

        public float Value => samplesSum / samplesCount;

        #endregion Public Properties

        #region Public Methods

        public float Update(float newSample)
        {
            samplesSum -= samplesArray[sampleIndex];
            samplesSum += newSample;
            samplesArray[sampleIndex] = newSample;
            if (++sampleIndex == samplesCount)
                sampleIndex = 0;

            return Value;
        }

        #endregion Public Methods
    }
}