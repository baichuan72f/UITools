using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HFrameExample
{
    public class SingletonExample : MonoBehaviour
    {
        public class GameManager : Singleton<GameManager>
        {
            private GameManager() { }
            public void Log()
            {
                Debug.Log("GameManager Log");
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            GameManager.Instance.Log();

            GameManager obj1 = GameManager.Instance;
            GameManager obj2 = GameManager.Instance;
            Debug.Log(obj1.GetHashCode() == obj2.GetHashCode());

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
