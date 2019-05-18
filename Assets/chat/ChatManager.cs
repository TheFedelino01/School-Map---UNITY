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
    public GameObject MessagePrefab;
    public Transform MessagesList;
    public ScrollRect ScrollRect;
    public bool ChatAperta { get; set; }
    public string playerName { get; set; }

    public class MessageList : SyncListStruct<Message> { }
    private MessageList messages;

    private Coroutine coroutineChiusuraPopup;

    void Awake()
    {
        if (instance == null)
            //if not, set instance to this
            instance = this;
        //If instance already exists and it's not this:
        else if (instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(this.gameObject);
        ChatAperta = false;
        messages = new MessageList();
    }

    // Start is called before the first frame update
    void Start()
    {
        RpcUpdateMessages();    //quando si connette scarico tutti i messaggi
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.partitaAvviata)
        {
            if (Input.GetKeyDown(KeyCode.T) && !ChatAperta)
            {
                if (coroutineChiusuraPopup != null)
                    StopCoroutine(coroutineChiusuraPopup);
                mostraChat();
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (ChatAperta)
                {
                    if (inputField.text != "")
                    {
                        Debug.Log("DEVO INVIARE IL MESSAGGIO: " + inputField.text);
                        Message m = new Message(playerName, inputField.text);
                        CmdSendMessage(m);
                        inputField.text = "";
                    }
                    //ImpostaFocus(inputField);
                    ChatAperta = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    mostraMessaggi();   //mostro solo i messaggi togliendo l'inputText
                    coroutineChiusuraPopup = this.EseguiAspettando(5, () =>
                      {
                          nascondiMessaggi();
                      });
                }
                else
                    mostraChat();
            }
        }
        else if (ChatAperta)
            nascondiChat();
    }


    private void ImpostaFocus(InputField comp)
    {
        comp.ActivateInputField();
        comp.Select();
    }


    public void mostraChat()
    {
        if (coroutineChiusuraPopup != null)
            StopCoroutine(coroutineChiusuraPopup);
        Cursor.lockState = CursorLockMode.None;
        chatPanel.SetActive(true);
        inputField.gameObject.SetActive(true);
        animInputField.Play("In");
        animChatRoom.Play("Chat Panel In");
        ChatAperta = true;
        ImpostaFocus(inputField);
    }

    public void nascondiChat()
    {
        animInputField.Play("Out");
        animChatRoom.Play("Chat Panel Out");
        this.EseguiAspettando(0.25f, () =>
        {
            chatPanel.SetActive(false);
        });
        ChatAperta = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    [Command]
    private void CmdSendMessage(Message m)
    {
        ChatServer.RegisterMessage(ref m);
        messages.Add(m);
        //RpcUpdateMessages(m);
        RpcReciveMessage(m);
    }

    /**
     * Scarica tutti i messaggi
     */
    //[ClientRpc]
    private void RpcUpdateMessages()
    {
        if (MessagesList.childCount < messages.Count)
            for (int i = 0; i < messages.Count; i++)
            {
                if (MessagesList.Find(messages[i].idMessaggio.ToString()) == null)
                {
                    GameObject m = Instantiate(MessagePrefab, MessagesList);
                    m.transform.name = messages[i].idMessaggio.ToString();
                    m.transform.Find("Message").GetComponent<Text>().text = messages[i].text;
                    m.transform.Find("Player Name").GetComponent<Text>().text = messages[i].nomePlayer;
                }
            }
    }

    /**
     * Scarica il messaggio passato come parametro
     */
    [ClientRpc]
    private void RpcReciveMessage(Message mess)
    {
        GameObject m = Instantiate(MessagePrefab, MessagesList);
        m.transform.name = mess.idMessaggio.ToString();
        m.transform.Find("Message").GetComponent<Text>().text = mess.text;
        m.transform.Find("Player Name").GetComponent<Text>().text = mess.nomePlayer;

        this.EseguiAspettando(1, () =>
        {
            if (mess.idMessaggio <= 5)
                ScrollRect.verticalNormalizedPosition = 1;
            else
                ScrollRect.verticalNormalizedPosition = 0;
        });

        if (coroutineChiusuraPopup != null)
            StopCoroutine(coroutineChiusuraPopup);
        //sui client che non hanno inviato il messaggio apro un popup che si chiude dopo 5 secondi
        if (playerName != mess.nomePlayer && !ChatAperta)
        {
            mostraMessaggi();
            Debug.Log("Nascondo i messaggi tra 5 secondi... ");
            coroutineChiusuraPopup = this.EseguiAspettando(5, () =>
              {
                  nascondiMessaggi();
                  Debug.Log("Messaggi nascosti");
              });
        }
    }

    private void mostraMessaggi()
    {
        chatPanel.SetActive(true);
        animChatRoom.Play("Chat Panel In");
        inputField.gameObject.SetActive(false);
    }

    private void nascondiMessaggi()
    {
        if (!ChatAperta)
        {
            animChatRoom.Play("Chat Panel Out");
            this.EseguiAspettando(0.25f, () =>
            {
                inputField.gameObject.SetActive(true);
                chatPanel.SetActive(false);
            });
        }
    }
}


public struct Message
{
    public int idMessaggio;
    public readonly string nomePlayer;
    public readonly string text;

    public Message(string nome, string text)
    {
        idMessaggio = -1;
        nomePlayer = nome;
        this.text = text;
    }

    public void SetId(int id)
    {
        idMessaggio = id;
    }
}