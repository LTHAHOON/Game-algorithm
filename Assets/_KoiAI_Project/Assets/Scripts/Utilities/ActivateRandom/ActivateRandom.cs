using System.Collections.Generic;
using UnityEngine;

namespace KoiAI.Utilities
{
    public static class ActivateRandom
    {
        public static T GetRandomActivateTarget<T>(ActivateRandomGroup<T> activateRandomGroup) where T : Component
        {
            if (activateRandomGroup == null)
            {
                return null;
            }
            T target = GetRandomActivateTarget(activateRandomGroup.ActivateTargets);
            return target;
        }

        public static T GetRandomActivateTarget<T>(List<ActivateRandomValue<T>> activateValues) where T : Component
        {
            if (activateValues == null || activateValues.Count <= 0)
            {
                return null;
            }
            ActivateRandomValue<T> curActivateValue = activateValues[0];

            for (int i = 1; i < activateValues.Count; i++)
            {
                if (curActivateValue == null || activateValues[i] == null)
                {
                    return null;
                }

                float curRandomValue = curActivateValue.GetRandomValue();
                float randomValue = activateValues[i].GetRandomValue();
                if (curRandomValue < randomValue)
                {
                    curActivateValue = activateValues[i];
                }
            }
            return curActivateValue.ActivateTarget;
        }
    }
}
