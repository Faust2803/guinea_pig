using Managers.SoundManager.Base;
using Managers.SoundManager.SoundHandlers;
using UnityEngine;

namespace Managers.SoundManager
{
    public class SoundSourceFactory : ISoundSourceHandlerFactory
    {
        private int _count;
        
        public ISoundSourceHandler Create(ISoundData data, Transform parent, bool isExternal, bool is3D)
        {
            Transform body = null;

            if (isExternal)
            {
                body = new GameObject($"SoundSource-{_count++}").transform;
                body.SetParent(parent);
                body.localPosition = Vector3.zero;
            }
            else
                body = parent;

            var source = body.gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            
            return new SoundSourceHandler(source, data, body, is3D);
        }
    }
}