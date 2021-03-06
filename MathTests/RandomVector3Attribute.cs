﻿using Math3D;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using System.Collections;

namespace MathTests
{
    public class RandomVector3Attribute : DataAttribute, IParameterDataSource
    {
        private readonly int count;
        private readonly double minComp;
        private readonly double maxComp;

        // the amount you have to multiply a random number b/w 0.0 and 1.0 to get a random number within the specified range
        private readonly double factor;

        public RandomVector3Attribute(int count) : this(-1.0, 1.0, count)
        {
        }

        public RandomVector3Attribute(double minComp, double maxComp, int count)
        {
            this.count = count;
            this.minComp = minComp;
            this.maxComp = maxComp;
            factor = (maxComp - minComp) + minComp;
        }

        public IEnumerable GetData(IParameterInfo parameter)
        {
            // use randomizer instead of random so that the consistant seed is used  
            Randomizer rand = Randomizer.GetRandomizer(parameter.ParameterInfo); 
            for (int i = 0; i < count; i++)
            {
                double x = rand.NextDouble(minComp, maxComp);
                double y = rand.NextDouble(minComp, maxComp);
                double z = rand.NextDouble(minComp, maxComp);
                yield return new Vector3(x, y, z);
            }
        }

    }
}
