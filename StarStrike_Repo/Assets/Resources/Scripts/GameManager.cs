using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public Dictionary<string, GameObject> references = new Dictionary<string, GameObject>();

    public int score = 0;
    public int highScore = 0;
    public Sprite[] scoreImages;

    [HideInInspector] public GameObject player;
    [HideInInspector] public PlayerScript playerController;
    [HideInInspector] public string currentPlanet;

    private void Awake()
    {
        if (!instance) instance = this;
        currentPlanet = SceneManager.GetActiveScene().name;
        highScore = PlayerPrefs.GetInt(currentPlanet + "_Highscore");        
    }

    // Use this for initialization
    void Start () {
        player = references["Player"];
        playerController = player.transform.parent.GetComponent<PlayerScript>();
        StartCoroutine("StartSequence");
    }

    IEnumerator StartSequence()
    {
        CameraEffects.instance.fadeOut(1f);
        iTween.MoveFrom(Camera.main.gameObject, iTween.Hash("y", 43, "time", 5, "islocal", true, "easetype", iTween.EaseType.linear));
        yield return new WaitForSeconds(1f);
        references["TractorBH"].GetComponent<Animator>().Play("OpenBH");
        references["UFO"].GetComponent<Animator>().Play("OpenUFO");
        player.transform.parent.GetComponent<WeaponInventory>().addWeapon("Orb");
        references["G_Game"].GetComponent<Animator>().CrossFadeInFixedTime("GameAnim", 0);
        references["TractorBH"].GetComponent<Animator>().Play("CloseBH");
        references["UFO"].GetComponent<Animator>().Play("CloseUFO");
        references["ReadyGo"].GetComponent<Animator>().Play("ReadySlap");
        yield return new WaitForSeconds(3f);
        player.transform.parent.GetComponent<PlayerScript>().canMove = true;

        MonoBehaviour[] spawners = GameObject.Find("Spawners").transform.GetComponentsInChildren<MonoBehaviour>();
        foreach (MonoBehaviour g in spawners)
        {
            g.enabled = true;
        }
        yield break;
       
    }

    public void AddScore(int amount)
    {
        Image ss = references["ScoreStar"].GetComponent<Image>();
        Text st = references["ScoreText"].GetComponent<Text>();
        Text hst = references["HighscoreText"].GetComponent<Text>();

        score += amount;

        st.text = score + "";

        if (score > highScore)
        {
            if (ss.sprite != scoreImages[1])
            {
                ss.sprite = scoreImages[1];
                ss.color = st.color = Color.yellow;
                hst.GetComponentInChildren<ParticleSystem>().Emit(20);
                hst.color = Color.yellow;
                iTween.ScaleFrom(ss.gameObject, new Vector3(2, 2, 2), 0.3f);
                iTween.ScaleFrom(hst.gameObject, new Vector3(2, 2, 2), 0.4f);
                hst.CrossFadeAlpha(0, 2, false);
                
            }
            highScore = score;
        }
    }

    public void GameOver()
    {
        Animator ani_G = references["G_Game"].GetComponent<Animator>();
        ani_G.enabled = true;
        ani_G.SetFloat("Direction", -1.0f);
        ani_G.Play("GameAnim");
        Destroy(references["Wobblepoint"].GetComponent<Animator>());
        Camera.main.transform.parent = player.transform;
        iTween.MoveTo(Camera.main.gameObject, iTween.Hash("position", new Vector3(0, 10, -8), "time", 1, "islocal", true));
        iTween.RotateTo(Camera.main.gameObject, iTween.Hash("rotation", new Vector3(50, 0, 0), "time", 1, "islocal", true));
        references["TractorBH"].GetComponent<Animator>().Play("GameOver");
        references["TractorBH"].transform.parent = player.transform;
    }

    public void ShowGOScreen()
    {
        references["GameOver"].GetComponent<Animator>().Play("GUI_GO");
        references["GameOver"].GetComponent<GUI_GOcontroller>().targetScore = score;
    }

   
}
