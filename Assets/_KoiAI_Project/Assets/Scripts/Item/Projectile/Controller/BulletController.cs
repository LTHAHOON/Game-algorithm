using KoiAI.Item;
using KoiAI.Skin;
using KoiAI.Utilities;
using UnityEngine;

namespace KoiAI
{
    public class BulletController : MonoBehaviour
    {
        [SerializeField]
        private BulletSkin _bulletSkin;
        [SerializeField]
        private GameObject _hitEffectPrefab;

        private BulletData _bulletData;
        public void Init(BulletData bulletData, LayerMask targetLayerMask)
        {
            _bulletData = bulletData;
        }

        public void OnHit(Vector3[] hitPoints, Collider[] targetColliders, int hitCount)
        {
            if (_hitEffectPrefab)
            {
                for (int i = 0; i < hitPoints.Length; i++)
                {
                    Instantiate(_hitEffectPrefab, hitPoints[i], Quaternion.identity);
                }
            }

            if (!_bulletData)
            {
                return;
            }
            if (hitCount <= 0)
            {
                return;
            }

            for (int i = 0; i < hitCount; i++)
            {
                if (!targetColliders[i])
                {
                    continue;
                }
                if (targetColliders[i].TryGetComponent(out Health.Health health))
                {
                    health.ChangeHealth(-_bulletData.Damage);
                }
            }
        }

        public BulletSkin GetBulletSkin() => _bulletSkin;
    }
}
