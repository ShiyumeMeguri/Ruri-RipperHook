using AssetRipper.IO.Files.Exceptions;
using System.Reflection;

namespace Ruri.RipperHook;

public static class ARIntelnalReflection
{
    public static readonly MethodInfo ThrowNoBytesWrittenMethod = typeof(DecompressionFailedException).GetMethod("ThrowNoBytesWritten", BindingFlags.Static | BindingFlags.NonPublic);
    public static readonly MethodInfo ThrowIncorrectNumberBytesWrittenMethod = typeof(DecompressionFailedException).GetMethod("ThrowIncorrectNumberBytesWritten", BindingFlags.Static | BindingFlags.NonPublic);
}
