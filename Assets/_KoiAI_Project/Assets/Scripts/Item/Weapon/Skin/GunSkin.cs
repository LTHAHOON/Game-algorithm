using UnityEngine;

namespace KoiAI.Skin
{
    using KoiAI.Audio;

    public class GunSkin : WeaponSkin
    {
        [SerializeField]
        private Transform _firePoint;
        [SerializeField]
        private AudioData _fireAudioData;
        [SerializeField]
        private ParticleSystem _firePT;

        public Transform FirePoint => _firePoint;
        public AudioData FireAudioData => _fireAudioData;
        public ParticleSystem FirePT => _firePT;
    }
}
