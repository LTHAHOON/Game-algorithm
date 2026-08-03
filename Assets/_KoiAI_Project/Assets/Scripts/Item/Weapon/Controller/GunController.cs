
using R3;
using UnityEngine;
using static KoiAI.Player.PlayerSFXAudioFeature;

namespace KoiAI.Item
{
    using KoiAI.Pool;
    using KoiAI.Skin;
    using KoiAI.Utilities;
    using KoiAI.Audio;

    public class GunController : WeaponControllerBase
    {
        [SerializeField]
        private GunData _gunData;
        [SerializeField]
        private LayerMask _targetLayerMask;

        private GunSkin _gunSkin;
        private Vector3 _hitPoint;
        private Pool<BulletItem> _pool;

        private RaycastHit[] _hits;
        private readonly ReactiveProperty<int> _curBallCount = new(0);
        private AudioSFXTarget _attackAuidoTarget;
        private int _curBallLoadCount;
        private int _remainingBallCount = 0;
        private int _remainingBallLoadCount = 0;
        private float _curLoadTime = 0f;
        private float _targetLoadTime = 0f;
        private bool _isFireLoading = false;
        private bool _isAiming = false;

        private void Update()
        {
            if (!_gunSkin)
            {
                return;
            }
            if (IsFireLoading())
            {
                #region 발사체 장전 로직
                if (_targetLoadTime <= 0f)
                {
                    _curBallLoadCount = 0;
                    _remainingBallLoadCount = 0;
                    _isFireLoading = false;
                    return;
                }
                _curLoadTime += Time.deltaTime;
                int count = Mathf.RoundToInt(_curLoadTime / _targetLoadTime * _remainingBallCount);
                _curBallLoadCount = _curBallCount.CurrentValue + count;
                _remainingBallLoadCount = Mathf.Clamp(_remainingBallCount - count, 0, _remainingBallCount);

                if (_curLoadTime >= _targetLoadTime || _curBallLoadCount >= _gunData.LoadMaxCount)
                {
                    _curBallCount.Value = _curBallLoadCount;
                    _remainingBallCount = _remainingBallLoadCount;
                    _curLoadTime = 0f;
                    _targetLoadTime = 0f;
                }
                #endregion
            }
            if (IsAiming())
            {
            }
        }

        public override void Init(WeaponBase wepaonItem)
        {
            _hits = new RaycastHit[_gunData.MaxHitCount];
            _curBallCount
                .Pairwise()
                .Where(pair => pair.Current < pair.Previous)
                .Subscribe(_ =>
                {
                    if (_gunSkin.FirePT)
                    {
                        _gunSkin.FirePT.Play();
                    }
                    if (_gunSkin.FireAudioData)
                    {
                        var owner = wepaonItem.ItemOwner;
                        if (_attackAuidoTarget == null)
                        {
                            _attackAuidoTarget = owner.GetAudioSFXTarget(PlayerSFXAuidoProperty.Attack);
                        }
                        AudioManager.Instance.PlaySFX(_attackAuidoTarget, _gunSkin.FireAudioData, _gunSkin.FirePoint.position);
                    }
                }).AddTo(this);

            BulletData bulletData = _gunData.BulletData;
            ulong id = gameObject.GetEntityULongID();
            BulletItem projectilePrefab = (BulletItem)bulletData.ItemPrefab;
            PoolManager.Instance.AddPool<BulletItem>(id, projectilePrefab, bulletData.BulletPoolSize, PoolName.Projectile);
            PoolManager.Instance.TryGetPool<BulletItem>(id, out _pool);
            BulletItem[] bullets = _pool.GetAllInstanceArray();
            for (int i = 0; i < bullets.Length; i++)
            {
                //발사체 스킨 생성
                bullets[i].SetupController(_targetLayerMask);
            }
            InitSkin();
        }

        public override bool Activate()
        {
            if (_isFireLoading)
            {
                #region 장전 중일 경우 장전된 만큼 설정하고 초기화하고 True 리턴하기
                _curLoadTime = 0f;
                _targetLoadTime = 0;
                _curBallCount.Value = _curBallLoadCount;
                _remainingBallCount = _remainingBallLoadCount;
                _curBallLoadCount = 0;
                _remainingBallLoadCount = 0;
                _isFireLoading = false;
                return true;
                #endregion
            }
            if (HasNotCannonBall() && !_gunData.IsInfiniteLoad)
            {
                return false;
            }

            if (!_gunData.IsInfiniteLoad)
            {
                --_curBallCount.Value;
            }

            BulletItem bulletItem = _pool.Pop();
            if (!bulletItem)
            {
                return false;
            }
            if (bulletItem.IsEmptyController())
            {
                bulletItem.SetupController(_targetLayerMask);
            }

            int count = Physics.SphereCastNonAlloc(transform.position, 0.1f,transform.forward, _hits, 500f, _targetLayerMask);
            if (count > 0)
            {
                bulletItem.BulletController.OnHit(_hits, count);
            }

            return true;
        }
        public override void SetAim(Vector2 aim) { }

        public override void StartAiming(float startPitchAngle = 0, float startYawAngle = 0)
        {
            _isAiming = true;
            
        }

        public override void EndAiming()
        {
            _isAiming = false;
        }

        public void OnLoadCannonBall(BulletData bulletData)
        {
            if (_curBallCount.CurrentValue >= _gunData.LoadMaxCount)
            {
                _remainingBallCount += bulletData.ProjectileCount;
                return;
            }
            _targetLoadTime += _gunData.LoadTime;
            _remainingBallCount += bulletData.ProjectileCount;
            _isFireLoading = true;
        }

        public void OnReLoadCannonBall()
        {
            //중복 장전 차단
            if (_isFireLoading)
            {
                return;
            }
            //탄이 꽉 차있거나 남은 탄이 없을 경우 리턴
            if (_curBallCount.CurrentValue >= _gunData.LoadMaxCount || _remainingBallCount <= 0)
            {
                return;
            }
            _targetLoadTime = _gunData.LoadTime * _remainingBallCount / _gunData.LoadMaxCount;
            _isFireLoading = true;
        }

        protected override void InitSkin()
        {
            if (_gunSkin)
            {
                Destroy(_gunSkin);
            }
            var skin = GetSkin();
            if (skin is GunSkin gunSkin)
            {
                if (IsSkinPrefab())
                {
                    _gunSkin = Instantiate(gunSkin, transform);
                }
                else
                {
                    _gunSkin = gunSkin;
                }
            }

        }

        public override void ChangeSkin()
        {
            if (_gunSkin)
            {
                Destroy(_gunSkin);
            }

            var skinPrefab = GetSkin();
            if (skinPrefab != null)
            {
                if (skinPrefab is GunSkin gunSkinPrefab)
                {
                    _gunSkin = Instantiate(gunSkinPrefab, transform);
                }
            }
        }

        public bool IsFireLoading() => _isFireLoading;
        private bool HasNotCannonBall() => _pool == null || _curBallCount.CurrentValue <= 0;
        public bool IsAiming() => _isAiming;
        public GunData GunData => _gunData;
        public int CurBallCount => _curBallCount.CurrentValue;
        public int RemainingBallCount => _remainingBallCount;
        public int CurBallLoadCount => _curBallLoadCount;
        public int RemainingBallLoadCount => _remainingBallLoadCount;
    }
}
