using Managers.SoundManager.Base;
using Managers.SoundManager.Enums;
using UnityEngine;
using Zenject;

namespace Managers.SoundManager.Test
{
    public class TestSound : MonoBehaviour, I3DSoundPoint
    {
        [Inject] private ISoundManager _soundManager;
        [SerializeField] private Transform _point;
        [SerializeField] private bool _withSmooth;
        
        private void Update()
        {
            //Ambient test
            //if (Input.GetKeyDown(KeyCode.Q))
            //{
            //    var effect = new SoundSmoothEffect(1f);
            //    if(_withSmooth)
            //        _soundManager.PlaySound(SoundId.SparksElectr_, effect, effect, true, false, true);
            //    else
            //        _soundManager.PlaySound(SoundId.SparksElectr_, true, false);
            //}
            
            //if (Input.GetKeyDown(KeyCode.W))
            //{
            //    _soundManager.PauseSound(SoundId.SparksElectr_);
            //}
            
            //if (Input.GetKeyDown(KeyCode.E))
            //{
            //    _soundManager.StopSound(SoundId.SparksElectr_);
            //}
            
            ////One shit test
            
            //if (Input.GetKeyDown(KeyCode.A))
            //{
            //    _soundManager.PlayOneShot(SoundId.HitIntoPlayer);
            //}
            
            //if (Input.GetKeyDown(KeyCode.S))
            //{
            //    _soundManager.PlayOneShot(SoundId.PigeonSpaceSuitAttack, false);
            //}
            
            //if (Input.GetKeyDown(KeyCode.D))
            //{
            //    _soundManager.PlayOneShot(SoundId.UFO);
            //}
            
            ////Second theme
            
            //if (Input.GetKeyDown(KeyCode.Z))
            //{
            //    var effect = new SoundSmoothEffect(1f);
            //    if(_withSmooth)
            //        _soundManager.PlaySound(SoundId.Blackhole, effect, effect, true);
            //    else
            //        _soundManager.PlaySound(SoundId.Blackhole, true);
            //}
            
            //if (Input.GetKeyDown(KeyCode.X))
            //{
            //    _soundManager.PauseSound(SoundId.Blackhole);
            //}
            
            //if (Input.GetKeyDown(KeyCode.C))
            //{
            //    _soundManager.StopSound(SoundId.Blackhole);
            //}
            
            ////3d
            //if (Input.GetKeyDown(KeyCode.F))
            //{
            //    _player = _soundManager.GetSoundPlayer(SoundId.HitIntoPlayer);
            //    _player.SetLoop(true)
            //           .FollowAt(this)
            //           .Play();
            //}

            //if (Input.GetKeyDown(KeyCode.G))
            //{
            //    _player?.Pause();
            //}

            //if (Input.GetKeyDown(KeyCode.H))
            //{
            //    _player?.Play();
            //}

            //if (Input.GetKeyDown(KeyCode.J))
            //{
            //    _player?.Stop();
            //}
        }

        private ISoundSourcePlayer _player;
        public Transform Target => _point;
        public bool IsAlive => _point != null;
    }
}