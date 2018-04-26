using UnityEngine;
using UnityEngine.UI;

public class NotificationHandler : MonoBehaviour {

    //---------------------------------------------------------------------------------------------
    // Singleton
    //---------------------------------------------------------------------------------------------
    private static NotificationHandler s_instance;
    public static NotificationHandler instance
    {
        get { return s_instance; }
    }

    //---------------------------------------------------------------------------------------------
    // Variables
    //---------------------------------------------------------------------------------------------
    [SerializeField] private Text m_notificationText;

    //---------------------------------------------------------------------------------------------
    // MonoBehaviors
    //---------------------------------------------------------------------------------------------
    private void Awake()
    {
        if (s_instance == null)
            s_instance = this;
        else
            Destroy(gameObject);

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        s_instance = null;
    }

    //---------------------------------------------------------------------------------------------
    // Functions
    //---------------------------------------------------------------------------------------------
    public void Notify(string message)
    {
        gameObject.SetActive(true);
        m_notificationText.text = message;
    }

    public void CloseNotification()
    {
        gameObject.SetActive(false);
    }
}
