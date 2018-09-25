using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dfo.Sample.Core
{
    public class DfoRandomUtils : IRandom, IDisposable
    {
        private readonly double[] _values;
        private readonly IRandom _oldRandom;

        private int _idx = 0;

        public DfoRandomUtils(params double[] values)
        {
            _values = values;
            _oldRandom = SafeRandom.TestGenerator?.Value;

            SafeRandom.SetTestGenerator(this);
        }

        public int Next()
        {
            return (int)NextDouble();
        }

        public int Next(int maxValue)
        {
            return (int)NextDouble();
        }

        public int Next(int minValue, int maxValue)
        {
            return (int)NextDouble();
        }

        public double NextDouble()
        {
            if (_idx == _values.Length)
            {
                throw new IndexOutOfRangeException();
            }

            var result = _values[_idx];
            _idx++;

            return result;
        }

        public double NextDouble(double minValue, double maxValue)
        {
            return NextDouble();
        }

        public void Dispose()
        {
            if (_oldRandom != null)
            {
                SafeRandom.SetTestGenerator(_oldRandom);
            }
            else
            {
                SafeRandom.ClearTestGenerator();
            }
        }
    }

    internal class UniformRandom : IRandom
    {
        private static readonly RNGCryptoServiceProvider _global = new RNGCryptoServiceProvider();

        private readonly Random _rnd;

        public UniformRandom()
        {
            byte[] buffer = new byte[4];
            _global.GetBytes(buffer);
            _rnd = new Random(BitConverter.ToInt32(buffer, 0));
        }

        public int Next()
        {
            return _rnd.Next();
        }

        public int Next(int maxValue)
        {
            return _rnd.Next(maxValue);
        }

        public int Next(int minValue, int maxValue)
        {
            return _rnd.Next(minValue, maxValue);
        }

        public double NextDouble()
        {
            return _rnd.NextDouble();
        }

        public double NextDouble(double minValue, double maxValue)
        {
            var r = _rnd.NextDouble() * (maxValue - minValue);
            return minValue + r;
        }
    }

    public static class SafeRandom
    {
        private static readonly ThreadLocal<IRandom> _random = new ThreadLocal<IRandom>(() => new UniformRandom());

        public static IRandom Generator => TestGenerator.Value ?? _random.Value;

        public static ThreadLocal<IRandom> TestGenerator { get; private set; } = new ThreadLocal<IRandom>();

        public static void SetTestGenerator(IRandom random)
        {
            TestGenerator.Value = random;
        }

        public static void ClearTestGenerator()
        {
            TestGenerator.Value = null;
        }
    }

    public interface IRandom
    {
        int Next();

        int Next(int maxValue);

        int Next(int minValue, int maxValue);

        double NextDouble();

        double NextDouble(double minValue, double maxValue);
    }
}
