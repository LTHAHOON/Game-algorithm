using KoiAI.Item;
using R3;
using System.Collections.Generic;

namespace KoiAI.Utilities
{
    public class WeaponControlRegistry
    {
        private static readonly Dictionary<ulong, WeaponControllerBase> _dicWeaponControl = new();
        public static void ResgisterWeaponControl(WeaponControllerBase weaponControllerBase)
        {
            WeaponData weaponData = weaponControllerBase.GetWeaponData();
            ulong weaponId = weaponData.ItemId;
            _dicWeaponControl.TryAdd(weaponId, weaponControllerBase);

            weaponControllerBase.WeaponUnRegisterObservable.Subscribe(weaponData =>
            {
               UnRegisterWeaponControl(weaponData);
            }).RegisterTo(weaponControllerBase.destroyCancellationToken);
        }

        public static void UnRegisterWeaponControl(WeaponData weaponData)
        {
            _dicWeaponControl.Remove(weaponData.ItemId);
        }

        public static WeaponControllerBase GetWeaponController(WeaponData weaponData)
        {
            if (_dicWeaponControl.TryGetValue(weaponData.ItemId, out WeaponControllerBase weaponControllerBase))
            {
                return weaponControllerBase;
            }
            return null;
        }

        public static void Clear()
        {
            _dicWeaponControl.Clear();
        }
    }
}
