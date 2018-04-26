using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Enums for easy-to-rename buttton names
public enum ButtonControlNames
{
	Start,
	UseAction,
	UseItem,
	Interact
}

public enum GameMode
{
	NormalPlay,
	StartMenu,
	ClosedStartMenuBuffer, // HACK: to make sure we don't start a convo with a close-by npc on start menu "Continue" click
	CombineMode
}

public class MyGameManager : MonoBehaviour
{
    #region variables
    public bool isKeyboardControls = false;
	public static MyGameManager _instance;
	public RuntimePlatform UserPlatform;

	public Dictionary<ButtonControlNames, KeyCode> ButtonControls_Controller;
	public Dictionary<ButtonControlNames, KeyCode> ButtonControls_Keyboard;
	
	public GameMode GameMode = GameMode.NormalPlay;

    //Audio
    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] private AudioClip m_gameOverSound;
    [SerializeField] private AudioClip m_victorySound;

    //Fade to black
    [SerializeField] private GameObject m_blackoutSphere;
    [SerializeField] private float m_fadeToBlackTime;

    [SerializeField] private GameObject m_player;
    #endregion

    void Awake()
	{
        //Singleton
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);

		UserPlatform = Application.platform;

		ButtonControls_Keyboard = new Dictionary<ButtonControlNames, KeyCode>();
		ButtonControls_Keyboard[ButtonControlNames.Start] = KeyCode.Escape;
		ButtonControls_Keyboard[ButtonControlNames.UseAction] = KeyCode.X;
		ButtonControls_Keyboard[ButtonControlNames.UseItem] = KeyCode.Z;
		ButtonControls_Keyboard[ButtonControlNames.Interact] = KeyCode.Space;
		//ButtonControls_Keyboard["LeftBumper"] = KeyCode.L;
		//ButtonControls_Keyboard["RightBumper"] = KeyCode.K;

		ButtonControls_Controller = new Dictionary<ButtonControlNames, KeyCode>();
		SetDefaultControllerValuesBasedOffOS(UserPlatform);
	}

    private void Start()
    {
        //Set sphere alpha to 0
        Color blackoutSphereColor = m_blackoutSphere.GetComponent<Renderer>().material.color;
        blackoutSphereColor.a = 0;
        m_blackoutSphere.GetComponent<Renderer>().material.color = blackoutSphereColor;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    #region Controller and Keyboard Controls

    public void SetDefaultControllerValuesBasedOffOS(RuntimePlatform userOS)
	{
		if (userOS == RuntimePlatform.LinuxPlayer)
		{
			// TODO
		}
		else if (userOS == RuntimePlatform.OSXPlayer)
		{
			// TODO
		}
		else // Windows and everything else
		{
			ButtonControls_Controller[ButtonControlNames.Start] = KeyCode.Joystick1Button7; // Start Button
			ButtonControls_Controller[ButtonControlNames.UseAction] = KeyCode.Joystick1Button1; // B Button
			ButtonControls_Controller[ButtonControlNames.UseItem] = KeyCode.Joystick1Button2; // X Button
			ButtonControls_Controller[ButtonControlNames.Interact] = KeyCode.Joystick1Button0; // A Button
			//ButtonControls_Controller["LeftBumper"] = KeyCode.Joystick1Button4; // Left Bumper
			//ButtonControls_Controller["RigthBumper"] = KeyCode.Joystick1Button5; // Right Bumper
		}
	}

	#endregion

	public void UpdateButtonControl(bool isController, ButtonControlNames btnName)
	{
		// TODO
	}

    public void Victory()
    {
        m_audioSource.clip = m_victorySound;
        m_audioSource.Play();
    }

    public void GameOver()
    {
        m_audioSource.clip = m_gameOverSound;
        m_audioSource.Play();

        FadeToBlack();
    }

    private void FadeToBlack()
    {
        StartCoroutine(FadeToBlackCoroutine());
    }

    private IEnumerator FadeToBlackCoroutine()
    {
        Material sphereMaterial = m_blackoutSphere.GetComponent<Renderer>().material;
        while(sphereMaterial.color.a < 1.0f)
        {
            yield return new WaitForEndOfFrame();

            Color sphereColor = sphereMaterial.color;
            float currentAlpha = sphereColor.a;
            float newAlpha = currentAlpha + (Time.deltaTime / m_fadeToBlackTime);

            //Set the alpha of the sphere
            sphereColor.a = newAlpha;
            sphereMaterial.color = sphereColor;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Find target player location
        GameObject targetTransform = GameObject.Find("TargetPlayerTransform");

        m_player.transform.position = targetTransform.transform.position;
        m_player.transform.rotation = targetTransform.transform.rotation;
    }

}

