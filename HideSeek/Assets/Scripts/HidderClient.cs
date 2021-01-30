using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Playables;

public class HidderClient : MonoBehaviourPunCallbacks
{
    public Text coundown;
    public Camera hiderCam;
    public Text UItext;
    public Text VictoryUI;
    public GameObject preistWin;
    public GameObject ghostWin;
    public AnimationController AC;
    public GameObject before_game;
    public HintToPaper hintToPaper;
    public PlayableDirector ghostEvicted;
    public PlayableDirector ghostwin;
    public GameObject hider_exist_error;

    PhotonView photonView;
    private float framesCount;
    private int possessionNum = -1;
    private int interactionID = -1;
    private bool canJoin = false;
    private bool hasInstantiated;
    private bool hasInteracted;
    private bool hasOptionMenu;
    private bool hasPossessed;
    private float _interactionCD;
    private bool connected;
    private int _possessChance;
    private int seconds;
    private bool hider_exist;

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.CountOfPlayers > 0)
        {
            print("!!!!!!!");
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if (player.NickName == "hidder")
                {
                    print("there is already a hider in game");
                    hider_exist_error.SetActive(true);
                    hider_exist = true;
                }
            }
        }
        else
        {
            photonView = GetComponent<PhotonView>();
            PhotonNetwork.LocalPlayer.NickName = "hidder";
            PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "usw";
            PhotonNetwork.ConnectUsingSettings();

        }
    }

    // Update is called once per frame
    void Update()
    {
        // hint will change every 10 secs
        framesCount += Time.deltaTime;
        seconds = (int)(framesCount % 60f);
        if ((seconds / 10) % 2 == 0)
        {
            before_game.transform.GetChild(0).gameObject.SetActive(true);
            before_game.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            before_game.transform.GetChild(0).gameObject.SetActive(false);
            before_game.transform.GetChild(1).gameObject.SetActive(true);
        }
        // networking logic
        if (!connected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        _possessChance = PossessController.possessChance;
        possessionNum = PossessController.possessID;
        interactionID = PossessController.interactID;
        hasInteracted = PossessController.hasInteracted;
        hasOptionMenu = PossessController.hasOptionMenu;
        _interactionCD = PossessController.sliderVal;
        hasPossessed = PossessController.hasPossessed;
        if (GameObject.Find("Hider(Clone)"))
        {
            hasInstantiated = true;
            photonView.RPC("SendPossessionNum", RpcTarget.Others, possessionNum, _possessChance);
            photonView.RPC("SendHiderCam", RpcTarget.Others, hiderCam.transform.position, hiderCam.transform.rotation);
            photonView.RPC("enableOption", RpcTarget.Others, hasOptionMenu);
            photonView.RPC("SendInteractionCD", RpcTarget.Others, _interactionCD);
            // PossessController.hasOptionMenu = false;

            if (hasInteracted)
            {
                print("hasInteracted");
                photonView.RPC("SendInteractionID", RpcTarget.Others, interactionID, _interactionCD);
                PossessController.hasInteracted = false;
            }
        }
        //Vector3 position = new Vector3(8, -1, 12);
        Vector3 position = new Vector3(4, 4f, 10);
        Quaternion rotation = Quaternion.Euler(0f, 180, 0f);
        if (PhotonNetwork.CountOfPlayers > 0 && !hasInstantiated && canJoin)
        {
            try
            {
                PhotonNetwork.Instantiate("Hider", position, rotation, 0);
            }
            catch
            {

            }
        }
        
    }


    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        Debug.Log(PhotonNetwork.CurrentRoom);
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        canJoin = false;
        base.OnJoinRandomFailed(returnCode, message);
    }

    public override void OnJoinedRoom()
    {
        canJoin = true;
        base.OnJoinedRoom();
        before_game.SetActive(false);
        connected = true;
    }

    [PunRPC]
    void UpdatePaper(int id, string text, bool addHint)
    {
        print(id);
        if (id != -1)
        {
            print(addHint);
            hintToPaper.OnClickPaperFloat(id);
            if (addHint)
            {
                UItext.text += " " + text;
            }
            else
            {
                UItext.text = "Hint: ";
            }
        }
    }

    [PunRPC]
    void SendFu(int evictID)
    {
        AC.PlayAnimation(evictID, true);
        PossessController._evictID = evictID;
    }

    [PunRPC]
    void UpdateUI(string timer)
    {
        print("updateUI");
        coundown.enabled = true;
        coundown.text = timer;
    }

    [PunRPC]
    void SendWinStatus(int hasWon)
    {
        print("sendWinStatus");
        if (hasWon == 1)
        {
            // VictoryUI.text = "You Lose :(";
            ghostEvicted.Play();
            Invoke("set1", 10f);
            //preistWin.SetActive(true);
        }
        else
        {
            print("ghost win");
            // VictoryUI.text = "You Win !!";
            ghostwin.Play();
            Invoke("set2", 10f);
            //ghostWin.SetActive(true);
        }
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
