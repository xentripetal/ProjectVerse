using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Verse
{
    public class SceneManagerGo : MonoBehaviour
    {

        public void ChangeScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
