using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ChatManager : NetworkBehaviour
{
    private static ChatManager instance;
    public static ChatManager Instance { get => instance; }

    public GameObject chatPanel;
    public Animator animChatRoom;
    public Animator animTopPanel;
    public Animator animInputField;
    public InputField inputField;
    public bool ChatAperta { get; set; }
    public class MessageList : SyncListStruct<Message> { }

    private MessageList messages;

    void Awake()
    {
        if (instance == null)
            //if not, set instance to this
            instance = this;
        //If instance already exists and it's not this:
        else if (instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(this.gameObject);

        messages = new MessageList();
        ChatAperta = false;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.partitaAvviata)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (ChatAperta)
                    if (inputField.text == "")
                        nascondiChat();
                    else
                    {
                        Debug.LogError("DEVO INVIARE IL MESSAGGIO: " + inputField.text);
                        inputField.text = "";
                        ImpostaFocus(inputField);
                    }
                else
                    mostraChat();
            }
        }
        else
            nascondiChat();
    }


    private void ImpostaFocus(InputField comp)
    {
        comp.ActivateInputField();
        comp.Select();
    }


    public void mostraChat()
    {
        Cursor.lockState = CursorLockMode.None;
        chatPanel.SetActive(true);
        animInputField.Play("In");
        animChatRoom.Play("Chat Panel In");
        ChatAperta = true;
        ImpostaFocus(inputField);
    }

    public void nascondiChat()
    {
        animInputField.Play("Out");
        animChatRoom.Play("Chat Panel Out");
        StartCoroutine(disattivaFinastraChat());
        ChatAperta = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private IEnumerator disattivaFinastraChat()
    {
        yield return new WaitForSeconds(0.25f);
        chatPanel.SetActive(false);
    }
}


public struct Message
{
    string nomePlayer;
    string text;
}