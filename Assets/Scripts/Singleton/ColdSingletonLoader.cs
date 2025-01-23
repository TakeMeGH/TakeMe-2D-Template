using UnityEngine;

namespace TKM
{
    public class ColdSingletonLoader : MonoBehaviour
    {
        [SerializeField] GameObject _managers;
        void Awake()
        {
            if (FindAnyObjectByType<SingletonManagers>() == null)
            {
                Instantiate(_managers);
            }
        }
    }
}
