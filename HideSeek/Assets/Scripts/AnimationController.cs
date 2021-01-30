using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Playables;

public class AnimationController : MonoBehaviour
{
    public GameObject fridge;
    public GameObject chair;
    public GameObject clock;
    public GameObject sewingMachine;
    public GameObject ceilingFan;
    public AudioSource casetteAS;

    // fish tank
    public GameObject water;
    public float waterShowLength;
    private bool waterShowing = false;
    // tv
    public GameObject tvOffScreen;
    public GameObject tv;
    public float tvTurnOnLength;
    private bool tvOn = false;
    // picture
    public VideoPlayer picVideo;
    public float picVideoLength;
    public GameObject picLight;
    public GameObject hangPic;
    // celling light
    public PlayableDirector flashLightTimeLine;
    public AudioSource LightAS;
    // ghost door
    public GameObject normalDoor;
    public GameObject ghostRoom;
    public float ghostRoomTime;
    // wardrobe
    public Animator wordrobeAnim;
    public GameObject walldrobe;
    // candle
    public GameObject candleLight;
    public GameObject candle;
    public float candleLightTime;

    public GameObject Taoist_Fu;
    public GameObject cassetteRadio;
    public GameObject Fu_position;

    public PlayableDirector spellAnim;
    public AudioSource talismanAS;

    private float timeCount;

    // Start is called before the first frame update
    void Start()
    {
        water.SetActive(false);
        normalDoor.SetActive(true);
        ghostRoom.SetActive(false);
        candleLight.SetActive(false);
    }

    private void Update()
    {
        if (casetteAS.time > 5)
        {
            casetteAS.Stop();
        }
    }

    public void PlayAnimation(int id)
    {
        print("id!!!");
        switch (id)
        {
            case 0:
                // Fridge
                fridge.GetComponent<Animator>().SetTrigger("hasInteracted");
                fridge.GetComponent<AudioSource>().Play();
                break;
            case 1:
                //  Chair
                chair.GetComponent<Animator>().SetTrigger("hasInteracted");
                chair.GetComponent<AudioSource>().Play();
                break;
            case 2:
                // Clock
                print("clock interat");
                clock.GetComponent<Animator>().SetTrigger("hasInteracted");
                clock.GetComponent<AudioSource>().Play();
                break;
            case 3:
                // Sewing Machine
                sewingMachine.GetComponent<Animator>().SetTrigger("hasInteracted");
                sewingMachine.GetComponent<AudioSource>().Play();
                break;
            case 4:
                // TV
                if (!tvOn)
                {
                    tvOn = true;
                    tv.GetComponent<AudioSource>().Play();
                    tvOffScreen.SetActive(false);
                    // stop playing tv after some time
                    Invoke("turnOffTV", tvTurnOnLength);
                }
                break;
            case 5:
                // Cassette Radio
                // cassetteRadio.GetComponent<AudioSource>().Play();
                casetteAS.Play();
                timeCount = casetteAS.time;
                break;
            case 6:
                // ceiling fan
                ceilingFan.GetComponent<Animator>().SetTrigger("hasInteracted");
                ceilingFan.GetComponent<AudioSource>().Play();
                break;
            case 7:
                // fish tank
                if (!waterShowing)
                {
                    waterShowing = true;
                    water.SetActive(true);
                    // water and fish disappear after some time
                    Invoke("removeWater", waterShowLength);
                }
                water.GetComponent<AudioSource>().Play();
                break;
            case 8:
                // painting
                hangPic.GetComponent<AudioSource>().Play();
                picVideo.Play();
                picLight.SetActive(true);
                Invoke("stopPicVideo", picVideoLength);
                break;
            case 9:
                // light
                LightAS.Play();
                flashLightTimeLine.Play();            
                break;
            case 10:
                // ghost door
                normalDoor.SetActive(false);
                ghostRoom.SetActive(true);
                Invoke("resumeDoor", ghostRoomTime);
                break;
            case 11:
                // wardrobe
                wordrobeAnim.SetTrigger("play");
                walldrobe.GetComponent<AudioSource>().Play();
                break;
            case 12:
                candleLight.SetActive(true);
                candle.GetComponent<AudioSource>().Play();
                Invoke("stopCandleLight", candleLightTime);
                break;
        }

    }
    

    // overloading method to apply yellow paper
    public void PlayAnimation(int id, bool applyPaper)
    {
        print("apply fu");
        if (id >= 0)
        {
            Instantiate(Taoist_Fu, Fu_position.transform.GetChild(id).gameObject.transform.position, Fu_position.transform.GetChild(id).gameObject.transform.rotation);
            if (spellAnim != null)
                spellAnim.Play();

            talismanAS.Play();
        }
        
    }

    private void turnOffTV()
    {
        tvOffScreen.SetActive(true);
        tvOn = false;
    }

    private void removeWater()
    {
        water.SetActive(false);
        waterShowing = false;
    }

    private void stopPicVideo()
    {
        picVideo.Stop();
        picLight.SetActive(false);
    }

    private void resumeDoor(){
        normalDoor.SetActive(true);
        ghostRoom.SetActive(false);
    }

    private void stopCandleLight()
    {
        candleLight.SetActive(false);
    }
}
