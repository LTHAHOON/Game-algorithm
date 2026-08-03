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

        public void OnHit(RaycastHit[] hits, int hitCount)
        {
            if (_hitEffectPrefab)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    Instantiate(_hitEffectPrefab, hits[i].transform.position, Quaternion.identity);
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
                if (!hits[i].collider)
                {
                    continue;
                }
                if (hits[i].collider.TryGetComponent(out Health.Health health))
                {
                    health.ChangeHealth(-_bulletData.Damage);
                }
            }
        }

        public BulletSkin GetBulletSkin() => _bulletSkin;
    }
}
