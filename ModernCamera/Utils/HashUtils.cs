namespace ModernCamera.Utils
{
    public static class HashUtils
    {
        private const uint FNVOffset32 = 2166136261;
        private const uint FNVPrime32 = 16777619;

        private static ulong FNVPrime64 = 1099511628211;
        private static ulong FNVOffset64 = 14695981039346656037;

        public static uint Hash32(string value)
        {
            uint result = FNVOffset32;

            foreach (var c in value)
            {
                result = FNVPrime32 * (result ^ c);
            }

            return result;
        }

        public static ulong Hash64(string value)
        {
            ulong result = FNVOffset64;

            foreach (var c in value)
            {
                result = FNVPrime64 * (result ^ c);
            }

            return result;
        }
    }
}
