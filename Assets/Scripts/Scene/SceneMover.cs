using UnityEngine;
using UnityEngine.SceneManagement;

namespace TKM
{
    public class SceneMover : MonoBehaviour
    {
        public void LoadNextScene(string nextScene)
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}
