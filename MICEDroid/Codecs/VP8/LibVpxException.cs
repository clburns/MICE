using System;
using System.Runtime.InteropServices;

using Android.Runtime;

namespace MICEDroid.Codecs.VP8
{
    class LibVpxException : Exception
    {
        public LibVpxException(string msg)
            : base(msg)
        { }
    }
}