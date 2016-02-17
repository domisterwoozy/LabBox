using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math3D.Probability
{
    public class ContinuousDistribution
    {
        private bool meanCached = false;
        private double cachedMean;

        private bool varianceCached = false;
        private double cachedVariance;

        public ImmutableArray<double> Values { get; }

        public int Count => Values.Length;

        public double Mean
        {
            get
            {
                if (!meanCached) cachedMean = Values.Average();
                return cachedMean;
            }
        }

        public double Variance
        {
            get
            {
                if (!varianceCached) cachedVariance = Values.Sum(v => Math.Pow(v - Mean, 2)) / (Count - 1);
                return cachedVariance;
            }
        }

        public ContinuousDistribution(IEnumerable<double> values)
        {
            Values = ImmutableArray.Create(values.ToArray());
        }
    }
}
