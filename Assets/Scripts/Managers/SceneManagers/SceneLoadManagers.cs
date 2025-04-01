
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Managers.SceneManagers
{
    public class SceneLoadManagers : IInitializable
    {
        private string activeSceneName => SceneManager.GetActiveScene().name;

        public void Initialize()
        {
            LoadScene(Scene.Boot);
        }

        public async UniTask LoadScene(Scene scene)
        {
            if (activeSceneName != scene.ToString())
            {
                await Load(scene.ToString());
            }
        }

        public async UniTask ReloadScene()
        {
            await Load(activeSceneName);
        }

        private async UniTask Load(string sceneName)
        {
            var operation = SceneManager.LoadSceneAsync(sceneName);
            operation.allowSceneActivation = false;
            
            while (operation.progress < 0.9f)
            {
                Debug.Log($"load progress {operation.progress}");
                await UniTask.Delay(10); 
            }
            
            await UniTask.Delay(500);
            operation.allowSceneActivation = true;
        }
    }
    
    public enum Scene
    {
        Boot,
        Lobby,
        Game
    }
}