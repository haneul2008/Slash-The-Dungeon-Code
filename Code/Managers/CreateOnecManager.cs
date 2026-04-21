using UnityEngine;

namespace HN.Code.Managers
{
    public class CreateOnceManager : MonoBehaviour
    {
        public static CreateOnceManager Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}