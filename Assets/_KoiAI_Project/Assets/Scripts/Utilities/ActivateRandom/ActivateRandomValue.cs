using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace KoiAI.Utilities
{
    [Serializable]
    public class ActivateRandomValue<T>
    {
        [Header("성공 확률 값 (0~100 사이)")]
        [SerializeField]
        private float _chanceValue;
        [SerializeField]
        private T _activateTargetData;


        public T ActivateTargetData => _activateTargetData;
        public float GetRandomValue()
        {
            float randomValue =  Random.Range(_chanceValue, 100f);
            return randomValue;
        }
    }
}
