using System;
using System.Runtime.InteropServices;

namespace Capture.Hook
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CopyData
    {
        public int height;

        public int width;

        public int lastRendered;

        public int format;

        public int pitch;

        public Guid textureId;
    }
}
