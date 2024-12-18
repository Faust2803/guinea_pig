
using UnityEngine.SceneManagement;
using Zenject;

namespace Managers.SceneManagers
{
    public class SceneLoadManagers : IInitializable
    {
        private string activeSceneName => SceneManager.GetActiveScene().name;

        public void Initialize()
        {
            LoadScene(Scene.Lobby);
        }

        public void LoadScene(Scene scene)
        {
            if (activeSceneName != scene.ToString())
            {
                SceneManager.LoadScene(scene.ToString());
            }
        }

        public void ReloadScene()
        {
            SceneManager.LoadScene(activeSceneName);
        }
    }
    
    public enum Scene
    {
        Boot,
        Lobby,
        Game
    }
}