using Managers.SoundManager.Base;
using Managers.SoundManager.Enums;
using UnityEngine;
using Zenject;

namespace Managers.SoundManager
{
    public class SoundManagerUtils : MonoBehaviour
    {
        [Inject] private ISoundManager soundManager;

        public void PlayUISfx_Click ()
        {
            soundManager.PlayOneShot(SoundId.UI_ButtonClick, false);
        }

        public void PlayUISfx_Special (bool buyOrCancel)
        {
            soundManager.PlayOneShot(buyOrCancel ? SoundId.UI_ButtonBuy : SoundId.UI_ButtonCancel, false);
        }

        public void PlaySound (string soundId)
        {
            PlaySound(soundId);
        }

        public void PlayOneShot (string soundId)
        {
            soundManager.PlayOneShot(soundId);
        }

        public void PlaySound (SoundId soundId)
        {
            soundManager.PlaySound(soundId);
        }
    }
}