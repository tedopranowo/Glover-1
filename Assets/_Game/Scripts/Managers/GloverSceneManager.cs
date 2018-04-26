using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GloverSceneManager : MonoBehaviour
{
    //[SerializeField] private string m_calibrationScene;
    //private string m_lastScene;
    [SerializeField] private string m_nextScene;

    //private void Start()
    //{
    //    string currentScene = SceneManager.GetActiveScene().name;

    //    // Go to calibration scene if it hasn't been calibrated yet
    //    if (!MyGameManager._instance.isCalibrated && currentScene != m_calibrationScene)
    //    {
    //        m_lastScene = currentScene;

    //        LoadScene(m_calibrationScene);
    //    }
    //}

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(m_nextScene);
    }

    //public void LoadLastScene()
    //{
    //    LoadScene(m_lastScene);
    //}


    //private IEnumerator WaitThenLoadScene(int seconds, string sceneName)
    //{
    //    yield return new WaitForSeconds(seconds);
    //    LoadScene(sceneName);
    //}
}
