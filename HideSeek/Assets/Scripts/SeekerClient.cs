using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class SeekerClient : MonoBehaviourPunCallbacks
{

    public Camera seekerCam;
    public Text UItext;
    public GameObject before_game;
    public Text coundown;
    public GameObject preistWin;
    public GameObject ghostWin;
    public GameObject seeker_exist_error;

    PhotonView photonView;
    private float framesCount;
    private bool _hasEvictUI;
    private int _paperLeft;
    private int _evictID = -1;
    public static int possessionNum = -1;
    private bool hasInstantiated;
    private bool canJoin = false;
    private int _hasWon;
    private bool connected;
    public AnimationController AC;
    public HintToPaper hintToPaper;
    private int seconds;
    private bool seeker_exist;


    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        PhotonNetwork.LocalPlayer.NickName = "seeker";
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "usw";
        PhotonNetwork.ConnectUsingSettings();
        
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
        _evictID = SeekerUI._evictID;
        _hasEvictUI = SeekerUI.hasEvictUI;
        _paperLeft = SeekerUI.selctionleft;
        _hasWon = SeekerUI.hasWon;
        if (GameObject.Find("Seeker(Clone)"))
        {
            hasInstantiated = true;
            photonView.RPC("SendSeekerCam", RpcTarget.Others, seekerCam.transform.position, seekerCam.transform.rotation);
            photonView.RPC("SendSeekerUI", RpcTarget.Others, _hasEvictUI, _paperLeft);
            if (_hasWon != 0)
            {
                photonView.RPC("SendWinStatus", RpcTarget.Others, _hasWon);
            }
            if (_evictID != -1)
            {
                photonView.RPC("SendFu", RpcTarget.Others, _evictID);
                SeekerUI._evictID = -1;
            }
        }
        // Vector3 position = new Vector3(8, -3.8f, -17);
        Vector3 position = new Vector3(6, 0, 14);
        Quaternion rotation = Quaternion.Euler(0f, 180, 0f);
        if (PhotonNetwork.CountOfPlayers > 0 && !hasInstantiated && canJoin)
        {
            try
            {
                PhotonNetwork.Instantiate("Seeker", position, rotation, 0);
            }
            catch
            {

            }
        }
        
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
    void UpdateUI(string timer)
    {
        print("updateUI");
        coundown.enabled = true;
        coundown.text = timer;
    }

    [PunRPC]
    void SendPossessionNum(int n, int possessChance)
    {
        Debug.Log(possessionNum);
        possessionNum = n;
        if (possessionNum != -1)
        {
            before_game.SetActive(false);
        }
    }

    [PunRPC]
    void SendInteractionID(int id, float _interactionCD)
    {
        print("playAnim");
        AC.PlayAnimation(id);
    }

    [PunRPC]
    void SendWinStatus(int hasWon)
    {
        if (hasWon == 1)
        {
            // VictoryUI.text = "You Lose :(";
            preistWin.SetActive(true);
        }
        else
        {
            // VictoryUI.text = "You Win !!";
            ghostWin.SetActive(true);
        }
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

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
        connected = true;
    }
}
