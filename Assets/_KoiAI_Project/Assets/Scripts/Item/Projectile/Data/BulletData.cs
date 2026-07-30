using KoiAI.Item;
using KoiAI.Pool;
using UnityEngine;

namespace KoiAI
{
    [CreateAssetMenu(fileName = "new BulletData", menuName = "ProjectileData/BulletData")]
    public class BulletData : ProjectileData
    {
        [SerializeField]
        private BulletController _bulletController;
        [SerializeField]
        private PoolSize _bulletPoolSize;
        [SerializeField]
        private int _maxOverlapCount;

        public BulletController BulletController => _bulletController;
        public PoolSize BulletPoolSize => _bulletPoolSize;
        public int MaxOverlapCount => _maxOverlapCount;
    }
}
