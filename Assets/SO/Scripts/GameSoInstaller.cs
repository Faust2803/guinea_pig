﻿using Managers.SoundManager.Data;
using UnityEngine;
using Zenject;

namespace SO.Scripts
{
    [CreateAssetMenu (fileName = "GameSoInstaller", menuName = "Create Game SO Installer")]
    public class GameSoInstaller : ScriptableObjectInstaller<SoInstaller>
    {
        [SerializeField] public CharacterConfig _characterConfig;
        [SerializeField] public EnvironmentConfig _environmentConfig;
        [SerializeField] public BooletConfig _booletConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(_characterConfig).IfNotBound();
            Container.BindInstance(_environmentConfig).IfNotBound();
            Container.BindInstance(_booletConfig).IfNotBound();
        }
    }
}