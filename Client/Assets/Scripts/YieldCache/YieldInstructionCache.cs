using System.Collections.Generic;
using UnityEngine;

namespace Flux.Yield
{
    internal static class YieldInstructionCache
    {
        class FloatComparer : IEqualityComparer<float>
        {
            bool IEqualityComparer<float>.Equals(float x, float y) { return Mathf.Approximately(x, y); }
            int IEqualityComparer<float>.GetHashCode(float value) { return value.GetHashCode(); }
        }

        public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
        public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();
        private static readonly Dictionary<float, WaitForSeconds> s_MapTimeSeconds = new Dictionary<float, WaitForSeconds>(new FloatComparer());
        private static readonly Dictionary<float, WaitForSecondsRealtime> s_MapRealTimeSeconds = new Dictionary<float, WaitForSecondsRealtime>(new FloatComparer());
        public static WaitForSeconds WaitForSeconds(float seconds)
        {
            WaitForSeconds wfs;
            if (!s_MapTimeSeconds.TryGetValue(seconds, out wfs))
                s_MapTimeSeconds.Add(seconds, wfs = new WaitForSeconds(seconds));
            return wfs;
        }

        public static WaitForSecondsRealtime WaitForSecondsRealtime(float seconds)
        {
            WaitForSecondsRealtime wfsr;
            if (!s_MapRealTimeSeconds.TryGetValue(seconds, out wfsr))
                s_MapRealTimeSeconds.Add(seconds, wfsr = new WaitForSecondsRealtime(seconds));
            return wfsr;
        }
    }    
}
