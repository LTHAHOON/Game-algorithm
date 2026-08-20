using R3;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace KoiAI.Core
{
    using System;
    using Cysharp.Threading.Tasks;
    using KoiAI.Input;

    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private TimelineAsset _startCutSceneTimeline;
        [SerializeField]
        private PlayableDirector _playableDirector;
        [SerializeField]
        private PlayerInput _playerInput;
        
        private IDisposable _curSceneDisposable;
        private Subject<PlayableDirector> _cutSceneSubject = new(); 
        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            _cutSceneSubject
                .Subscribe(playableDirector =>
                {
                    _curSceneDisposable = Observable.EveryUpdate()
                        .Where(_ => playableDirector && playableDirector.state == PlayState.Paused)
                        .Take(1)
                        .Subscribe(_ => EndCutScene())
                        .AddTo(this);
                })
                .AddTo(this);
            InputService.ReconnectInputAction();
            InputService.SetEnableActionMap(InputActionMapContext.Player);
        }

        private void OnEnable()
        {
            GameStart();
        }
        
        private void GameStart()
        {
            if (!_playableDirector || !_startCutSceneTimeline)
            {
                return;
            }
            _playableDirector.Play();
            PlayCutScene(_startCutSceneTimeline);
        }

        private void PlayCutScene(TimelineAsset timelineAsset)
        {
            if (!_playerInput)
            {
                return;
            }
            _playerInput.enabled = false;
            _playableDirector.Play(timelineAsset);
            _cutSceneSubject.OnNext(_playableDirector);
        }
    
        private void EndCutScene()
        {
            if (!_playerInput)
            {
                return;
            }
            _playerInput.enabled = true;
            _curSceneDisposable?.Dispose();
        }
    
    }   

}
