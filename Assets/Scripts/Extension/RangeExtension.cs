using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Extension
{
    [System.Serializable]
    public class RangeInt
    {
        [field:SerializeField] public int Min { get; private set; }
        [field: SerializeField] public int Max { get; private set; }

        public RangeInt(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public int Random()
        {
            return UnityEngine.Random.Range(Min, Max);
        }
    }

    [System.Serializable]
    public class RangeFloat
    {
        [field: SerializeField] public float Min { get; private set; }
        [field: SerializeField] public float Max { get; private set; }

        public RangeFloat(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public float Random()
        {
            return UnityEngine.Random.Range(Min, Max);
        }
    }
}

