using BenchmarkDotNet.Attributes;

namespace CacheExperiment;

public class Experiment
{
    private const int MaxN = 8_388_608;
    
    [Params(
        2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 
        2048, 4096, 8192, 16_384, 32_768, 65_536, 131_072, 262_144, 524_288, 1_048_576, 
        2_097_152, 4_194_304, MaxN)]
    public int N { get; set; }

    private readonly uint[] _data;
    private readonly int[] _sequentialIndices;
    private readonly int[] _randomIndices;

    public Experiment()
    {
        _data = new uint[MaxN];
        _sequentialIndices = new int[MaxN];
        _randomIndices = new int[MaxN];

        for (var i = 0; i < MaxN; i++)
        {
            _data[i] = (uint)i;
            _sequentialIndices[i] = i;
            _randomIndices[i] = i;
        }

        ShuffleInPlace(_randomIndices);
    }

    private static void ShuffleInPlace<T>(IList<T> arr)
    {
        var random = new Random();
        var n = arr.Count - 1;
        while (n > 1)
        {
            n--;
            var k = random.Next(n + 1);
            (arr[k], arr[n]) = (arr[n], arr[k]);
        }
    }

    [Benchmark]
    public uint SequentialIndexSum()
    {
        uint sum = 0;
        for (var i = 0; i < N; i++)
        {
            var index = _sequentialIndices[i];
            sum += _data[index];
        }

        return sum;
    }
    
    [Benchmark]
    public uint RandomIndexSum()
    {
        uint sum = 0;
        for (var i = 0; i < N; i++)
        {
            var index = _randomIndices[i];
            sum += _data[index];
        }

        return sum;
    }
}