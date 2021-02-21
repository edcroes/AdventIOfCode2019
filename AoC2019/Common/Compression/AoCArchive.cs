namespace AoC2019.Common.Compression
{
    public class AoCArchive<T>
    {
        public int[] Constructed { get; init; }

        public T[][] Parts { get; init; }
    }
}
