using UnityEngine;

namespace KoiAI.Item
{
    [CreateAssetMenu(fileName = "new GunData", menuName = "WeaponData/GunData")]
    public class GunData : WeaponData
    {
        [SerializeField]
        private LayerMask _layerMaskForAim;
        [SerializeField]
        private BulletData _bulletData;
        [SerializeField]
        private bool _isInfiniteLoad = false;
        [SerializeField]
        private int _maxHitCount = 1;
        [SerializeField]
        private int _loadMaxCount = 20;
        [SerializeField]
        private float _launchSpeed = 12f;
        [SerializeField]
        private float _loadTime = 1f;
        [SerializeField]
        private float _maxPitchAngle = 90f;
        [SerializeField]
        private float _minPitchAngle = 0f;
        [SerializeField]
        private float _maxYawAngle = 90f;
        [SerializeField]
        private float _minYawAngle = -90f;

        public int MaxHitCount => _maxHitCount;
        public bool IsInfiniteLoad => _isInfiniteLoad;
        public float MaxYawAngle => _maxYawAngle;
        public float MinYawAngle => _minYawAngle;
        public int LoadMaxCount => _loadMaxCount;
        public float LaunchSpeed => _launchSpeed;
        public BulletData BulletData => _bulletData;
        public float LoadTime => _loadTime;
        public float MaxPitchAngle => _maxPitchAngle;
        public float MinPitchAngle => _minPitchAngle;
    }
}
