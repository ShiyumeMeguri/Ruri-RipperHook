using System.Runtime.InteropServices;

namespace Ruri.RipperHook.UnityMihoyo;
public class Acl
{
    public static void DecompressAll(byte[] data, out float[] values, out float[] times)
    {
        var decompressedClip = new DecompressedClip();
        DecompressAll(data, ref decompressedClip);

        values = new float[decompressedClip.ValuesCount];
        Marshal.Copy(decompressedClip.Values, values, 0, decompressedClip.ValuesCount);

        times = new float[decompressedClip.TimesCount];
        Marshal.Copy(decompressedClip.Times, times, 0, decompressedClip.TimesCount);

        Dispose(ref decompressedClip);
    }

    [DllImport("acl", CallingConvention = CallingConvention.Cdecl)]
    private static extern void DecompressAll(byte[] data, ref DecompressedClip decompressedClip);

    [DllImport("acl", CallingConvention = CallingConvention.Cdecl)]
    private static extern void Dispose(ref DecompressedClip decompressedClip);

    public struct DecompressedClip
    {
        public IntPtr Values;
        public int ValuesCount;
        public IntPtr Times;
        public int TimesCount;
    }
}