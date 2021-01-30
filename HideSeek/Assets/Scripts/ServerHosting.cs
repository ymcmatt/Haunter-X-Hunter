using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Playables;

public class ServerHosting : MonoBehaviourPunCallbacks
{

    PhotonView photonView;
    private int framesCount = 0;
    private int possessionNum = -1;
    private int _idToSend = -1;
    private string hintText;
    private bool _addHint;
    private float timer = 60f;
    private bool stopTimer;

    public GameObject waiting_hider;
    public GameObject waiting_seeker;
    public Slider _hiderCD;
    public Image cdImage;
    public Camera seekerCam;
    public Camera hiderCam;
    public GameObject _optionMenu;
    public Text _paperLeft;
    public GameObject _evictUI;
    public Text hidderPos;
    public TextMeshProUGUI UItext;
    Transform _otherHidderTransform;
    Transform _otherSeekerTransform;
    public AnimationController AC;
    public TextMeshProUGUI _seekerWinStatus;
    public TextMeshProUGUI _hiderWinStatus;
    public GameObject preistWin;
    public GameObject ghostWin;
    public Text _possessChance;
    public Text countdown;
    public PlayableDirector ghostEvicted;
    public PlayableDirector ghostwin_timeline;


    //public Transform thisHidderTransform;
    //public Transform thisSeekerTransform;

    void Start()
    {
        // use photonview to connect to a server with ConnectUsingSettings()
        photonView = GetComponent<PhotonView>();
        PhotonNetwork.LocalPlayer.NickName = "host";
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "usw";
        PhotonNetwork.ConnectUsingSettings();
        cdImage.fillAmount = 1f;
    }

    // overriding methods so our room has max player number of 3 and log messages about their status
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.AutomaticallySyncScene = true;
        string roomName = "GameRoom";
        RoomOptions options = new RoomOptions { MaxPlayers = 3, PlayerTtl = 100000, IsOpen = true };
        Debug.Log("We successfully connected to " + PhotonNetwork.CloudRegion + " server");
        PhotonNetwork.CreateRoom(roomName, options, null);

    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();

        Debug.Log("Created Room");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        Debug.Log("Joined Room");
        Debug.Log(PhotonNetwork.CurrentRoom);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log(message);
        Debug.Log("Failed to Create Room");
    }

    // 
    void Update()
    {        
        _idToSend = HintToPaper.idToSend;
        hintText = HintToPaper.hintName;
        _addHint = HintToPaper.addHint;
        if (Input.GetButtonDown("UpdateUI"))
        {
            countdown.enabled = true;
        }
        if (countdown.enabled && !stopTimer) // when update count down UI is pressed and no stopTimer (so no win status sent by hider and seeker)
        {
            timer -= Time.deltaTime;
            countdown.text = "Before Incense burnout: " + timer.ToString("n2");
            photonView.RPC("UpdateUI", RpcTarget.Others, countdown.text);   // send count down time to hider and seeker
            if (timer <= 0)
            {
                photonView.RPC("SendWinStatus", RpcTarget.Others, 0);       // when time runs out, hider win
                countdown.text = "";
                ghostwin_timeline.Play();   // ghostwin animation
                Invoke("set2", 10f);   // time runs out, ghost win
            }
        }
        if (PhotonNetwork.CountOfPlayers > 0)
        {
            foreach(Player player in PhotonNetwork.PlayerList)
            {
                if (player.NickName == "hidder")
                {
                    waiting_hider.SetActive(false);                 // when hider join, remove waiting_for_hider img
                }
                if (player.NickName == "seeker")
                {
                    waiting_seeker.SetActive(false);                // when hider join, remove waiting_for_hider img
                }
            }
            if (_idToSend != -1)                     // if the hint paper is updated, send the hint to hider and seeker client
            {
                photonView.RPC("UpdatePaper", RpcTarget.Others, _idToSend, hintText, _addHint);
                HintToPaper.idToSend = -1;          // reset the hint paper back to None
            }
        }
        switch (possessionNum)          // based on possession number sent by hider, dispaly where hider is on screen
        {
            case 0:
                hidderPos.text = "Hider is in Fridge";
                break;
            case 1:
                hidderPos.text = "Hider is in Chair";
                break;
            case 2:
                hidderPos.text = "Hider is in Clock";
                break;
            case 3:
                hidderPos.text = "Hider is in Sewing machine";
                break;
            case 4:
                hidderPos.text = "Hider is in TV";
                break;
            case 5:
                hidderPos.text = "Hider is in Cassette radio";
                break;
            case 6:
                hidderPos.text = "Hider is in Ceiling fan";
                break;
            case 7:
                hidderPos.text = "Hider is in Fish Tank";
                break;
            case 8:
                hidderPos.text = "Hider is in Hanging Picture";
                break;
            case 9:
                hidderPos.text = "Hider is in Ceiling Light";
                break;
            case 10:
                hidderPos.text = "Hider is in Ghost Door";
                break;
            case 11:
                hidderPos.text = "Hider is in Walldrobe";
                break;
            case 12:
                hidderPos.text = "Hider is in Candle Light";
                break;
        }
    }

    [PunRPC]
    void SendFu(int evictID)
    {
        AC.PlayAnimation(evictID, true);
    }

    [PunRPC]
    void enableOption(bool hasOptionMenu)
    {
        print(hasOptionMenu);
        if (hasOptionMenu)
        {
            _optionMenu.SetActive(true);
        }
        else
        {
            _optionMenu.SetActive(false);
        }
    }

    [PunRPC]
    void SendWinStatus(int hasWon)
    {
        _seekerWinStatus.gameObject.SetActive(true);
        _hiderWinStatus.gameObject.SetActive(true);
        stopTimer = true;
        if (hasWon == 1)
        {
            _seekerWinStatus.text = "You Win!";
            _hiderWinStatus.text = "You Lose :(";
            ghostEvicted.Play();
            Invoke("set1",10f);
  
        }
        else
        {
            _hiderWinStatus.text = "You Win!";
            _seekerWinStatus.text = "You Lose :(";
            ghostwin_timeline.Play();
            Invoke("set2", 10f);
        }
    }

    [PunRPC]
    void SendSeekerUI(bool hasEvictUI, int paperLeft)
    {
        _paperLeft.text = "Paper left to evict: " + paperLeft.ToString();
        if (hasEvictUI)
        {
            _evictUI.SetActive(true);
        }
        else
        {
            _evictUI.SetActive(false);
        }
    }

    [PunRPC]
    void SendSeekerCam(Vector3 seekerCamPos, Quaternion seekerCamRotation)
    {
        // Debug.Log("send seeker cam");
        seekerCam.transform.position = seekerCamPos;
        seekerCam.transform.rotation = seekerCamRotation;
    }

    [PunRPC]
    void SendHiderCam(Vector3 hiderCamPos, Quaternion hiderCamRotation)
    {
        // Debug.Log("send hider cam");
        hiderCam.transform.position = hiderCamPos;
        hiderCam.transform.rotation = hiderCamRotation;
    }


    [PunRPC]
    void SendPossessionNum(int n, int possessChance)
    {
        //if (n != -1)
        //{
        //    waiting_seeker.SetActive(false);
        //}
        // Debug.Log("hidder" + possessionNum.ToString());
        possessionNum = n;
        _possessChance.text = possessChance.ToString();
    }

    [PunRPC]
    void SendInteractionCD(float cd)
    {
        if (cd != 1)
        {
            cdImage.fillAmount = cd;
            //_hiderCD.value = cd;
        }
        else
        {
            cdImage.fillAmount = cd;
           // _hiderCD.value = 0;
        }
    }

    [PunRPC]
    void SendInteractionID(int id, float interactionCD)
    {
        print("playAnim");
        AC.PlayAnimation(id);
        print(interactionCD);
        //_hiderCD.value = interactionCD;
    }


    public IEnumerator delay()
    {
        yield return new WaitForSeconds(3f);
    }

    private void set1()
    {
        preistWin.SetActive(true);
    }

    private void set2()
    {
        ghostWin.SetActive(true);
    }


}
