using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MenuUIHandeler : MonoBehaviour
{
    public enum State { Menu, LevelSelect, PlanetSelect };
    public State currentState = State.Menu;

    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;
    private GameObject ufo;

    // Menu
    UnityEngine.UI.Image title;
    ParticleSystem starparts;
    UnityEngine.UI.Text pressText;

    // Levelselect
    UnityEngine.UI.Text planetName, hsText, uText;
    UnityEngine.UI.Image stroke;
    private Animator lsAnimator;

    private GameObject player;
    public GameObject[] planets;
    public GameObject currentPlanet;
    private Camera effectCam;

    private Transform playerRotator;
    private Transform topPos;

    private MenuCam menuCam;

    private bool onMap = false;
    private float titleShine;

    public bool displayPressText = false;

    void Awake()
    {
        title = GameObject.Find("Title").GetComponent<UnityEngine.UI.Image>();
        starparts = GameObject.Find("StarParts_M").GetComponent<ParticleSystem>();
        pressText = GameObject.Find("Press to start").GetComponent<UnityEngine.UI.Text>();
        stroke = GameObject.Find("Stroke").GetComponent<UnityEngine.UI.Image>();
        planetName = GameObject.Find("Planetname").GetComponent<UnityEngine.UI.Text>();
        hsText = GameObject.Find("Highscore_text").GetComponent<UnityEngine.UI.Text>();
        uText = GameObject.Find("Unlocked_text").GetComponent<UnityEngine.UI.Text>();
        lsAnimator = GameObject.Find("G_Levelselect").GetComponent<Animator>();
        m_Raycaster = GetComponent<GraphicRaycaster>();
        m_EventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        menuCam = GameObject.Find("CamRotator").GetComponent<MenuCam>();
        effectCam = GameObject.Find("EffectCam").GetComponent<Camera>();
        playerRotator = GameObject.Find("PlayerRotator").transform;
        topPos = GameObject.Find("Cam_Top").transform;
        player = GameObject.Find("Player");

        if (!PlayerPrefs.HasKey("CurrentPlanet"))
        {
            PlayerPrefs.SetInt("CurrentPlanet", 1);
        }

        currentPlanet = GameObject.Find("Level_" + PlayerPrefs.GetInt("CurrentPlanet"));

        //TEMP
        PlayerPrefs.SetInt("Level_1_Highscore", 50);
        PlayerPrefs.SetInt("Level_2_Highscore", 50);
        PlayerPrefs.SetInt("Level_3_Highscore", 50);

        PlayerPrefs.SetInt("Level_1_Unlocked", 1);
        PlayerPrefs.SetInt("Level_2_Unlocked", 1);
        PlayerPrefs.SetInt("Level_3_Unlocked", 1);

        // Align camera to current planet
        menuCam.gameObject.transform.position = playerRotator.transform.position = currentPlanet.transform.position;
        menuCam.gameObject.transform.localScale = playerRotator.transform.localScale = currentPlanet.transform.localScale;
        menuCam.gameObject.transform.parent = playerRotator.transform.parent = currentPlanet.transform;

        iTween.RotateTo(menuCam.gameObject, iTween.Hash("rotation", playerRotator.rotation.eulerAngles, "time", 2, "easetype", iTween.EaseType.easeInCirc, "oncomplete", "StartMenu", "oncompletetarget", this.gameObject));

        Camera.main.GetComponent<CameraEffects>().fadeOut(1);
        OpenMenu();
    }

    void StartMenu()
    {
        displayPressText = true;
        foreach (GameObject p in planets)
        {
            p.GetComponents<RotateAround>()[0].enabled = true;
            p.GetComponents<RotateAround>()[1].enabled = true;
        }
    }

    //Move to planet selection
    IEnumerator ToMap()
    {
        onMap = true;
        CloseMenu();
        Invoke("PlayerToCam", 0.5f);
        stroke.CrossFadeAlpha(1, 1, false);
        menuCam.DetachPlayerCam();
        menuCam.gameObject.transform.parent = null;
        menuCam.gameObject.transform.position = Vector3.zero;
        menuCam.gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
        iTween.MoveTo(Camera.main.gameObject, topPos.position, 1);
        iTween.RotateTo(Camera.main.gameObject, iTween.Hash("rotation", topPos.rotation.eulerAngles, "time", 1, "oncomplete", "SetMapCam", "oncompletetarget", this.gameObject));
        yield return new WaitForSeconds(0.5f);
        menuCam.AttachPlayerCam(menuCam.gameObject.transform);
        menuCam.SetFreeRotate(true);
    }

    // Align camera to planet
    public void CamToPlanet(GameObject planet)
    {
        menuCam.AttachPlayerCam(planet.transform.parent);
        iTween.MoveTo(Camera.main.gameObject, iTween.Hash("position", planet.transform.localPosition + new Vector3(0, 0, -30), "time", 1, "islocal", true));
        iTween.RotateTo(Camera.main.gameObject, iTween.Hash("rotation", Vector3.zero, "time", 1, "islocal", true));
    }

    // Move player to camera
    void PlayerToCam()
    {
        player.transform.parent = Camera.main.gameObject.transform;
        player.transform.localRotation = Quaternion.identity;
        player.transform.localPosition = new Vector3(0, -9.9f, 0f);
        iTween.MoveAdd(player, iTween.Hash("z", 11.0f, "time", 1.5f, "islocal", true));
    }

    // Start game sequence
    IEnumerator ToGame()
    {
        stroke.CrossFadeAlpha(0, 1, false);
        ClosePlanetSelect();
        GameObject bh = Instantiate(Resources.Load("Prefabs/Spawners/SpawnItems/SP_Blackhole"), player.transform.position, Quaternion.identity) as GameObject;
        
        bh.transform.parent = player.transform.parent;
        bh.transform.localPosition = new Vector3(0, 0, 30);
        GameObject p = Instantiate(Resources.Load("Prefabs/SpeedLights"), bh.transform) as GameObject;
        p.transform.position = bh.transform.position;
        p.transform.rotation = Quaternion.LookRotation(player.transform.position - p.transform.position);
        iTween.ValueTo(gameObject, iTween.Hash("time", 1.5f, "from", Camera.main.fieldOfView, "to", 150f, "onupdate", "SetFOV", "easetype", iTween.EaseType.easeInOutExpo));
        iTween.MoveAdd(player, iTween.Hash("z", 5000f, "time", 1.5f, "delay", 1.5f, "islocal", true, "easetype", iTween.EaseType.easeInOutExpo));
        yield return new WaitForSeconds(1.5f);
        CameraEffects.instance.fadeIn(1.5f);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(currentPlanet.name);
    }

    void SetFOV(float fov)
    {
        effectCam.fieldOfView = fov;
    }

    public void OpenMenu()
    {
        if (!title) title = GameObject.Find("Title").GetComponent<UnityEngine.UI.Image>();
        iTween.ScaleTo(title.gameObject, iTween.Hash("easetype", iTween.EaseType.easeOutElastic, "delay", 2f, "scale", new Vector3(1, 1, 0), "time", 1f));
        Invoke("EmitStars", 2f);
    }

    public void CloseMenu()
    {
        displayPressText = false;
        title.CrossFadeAlpha(0, 0.5f, false);
    }

    public void OpenPlanetSelect()
    {
        PlanetInfo planet = currentPlanet.GetComponent<PlanetInfo>();
        foreach (UnityEngine.UI.Image i in lsAnimator.gameObject.GetComponentsInChildren<UnityEngine.UI.Image>())
        {
            i.color = planet.planetColor;
        }
        foreach (UnityEngine.UI.Text t in lsAnimator.gameObject.GetComponentsInChildren<UnityEngine.UI.Text>())
        {
            t.color = planet.planetColor;
        }
        stroke.CrossFadeAlpha(255, 1, false);
        planetName.text = planet.planetName;
        hsText.text = "HIGHSCORE: " + PlayerPrefs.GetInt(planet.name + "_Highscore").ToString();
        uText.text = ((PlayerPrefs.GetInt(planet.name + "_Unlocked") == 1) ? "Locked" : "Unlocked");
        lsAnimator.CrossFadeInFixedTime("LevelSelectAnim", 0);
    }

    public void CloseLevelSelect()
    {
    }

    public void OpenLevelSelect()
    {
    }

    public void ClosePlanetSelect()
    {
        lsAnimator.CrossFadeInFixedTime("LevelSelectAnim Reverse", 0);
    }

    public void EmitStars()
    {
        starparts.Emit(40);
        StartCoroutine(FadeText());
        StartCoroutine(ShineText());
    }

    IEnumerator ShineText()
    {
        while (title.material.color.a > 0)
        {
            iTween.ValueTo(gameObject, iTween.Hash("from", 0, "to", 1, "time", 0.7f, "onupdate", "UpdateTextShine"));
            float waitTime = Random.Range(2f, 5f);
            yield return new WaitForSeconds(waitTime);
        }
    }

    void UpdateTextShine(float val)
    {
        title.material.SetFloat("_ShineLocation", val);
    }

    IEnumerator FadeText()
    {
        yield return new WaitForSeconds(1f);
        while (displayPressText)
        {
            pressText.CrossFadeAlpha(255, 1f, false);
            yield return new WaitForSeconds(1f);
            pressText.CrossFadeAlpha(0, 1f, false);
            yield return new WaitForSeconds(1f);
        }
    }

    // Set current gamestate
    public void SetState(State newState)
    {
        Invoke("Close" + currentState, 0f);
        Invoke("Open" + newState, 0f);
        currentState = newState;
    }


    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {

            m_PointerEventData = new PointerEventData(m_EventSystem);
            m_PointerEventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            m_Raycaster.Raycast(m_PointerEventData, results);

            foreach (RaycastResult result in results)
            {
                if (result.gameObject.name == "GoButton")
                    StartCoroutine("ToGame");
                if (result.gameObject.name == "Arrow_Back")
                {
                    SetState(State.LevelSelect);
                    StartCoroutine("ToMap");
                }
            }
            switch (currentState)
            {
                case State.Menu:
                    if (displayPressText)
                    {
                        SetState(State.LevelSelect);
                        StartCoroutine("ToMap");
                    }
                    break;
                case State.LevelSelect:
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.name == "Dustnet")
                        {
                            menuCam.SetFreeRotate(false);
                            CamToPlanet(hit.collider.gameObject);
                            currentPlanet = hit.collider.transform.parent.gameObject;
                            SetState(State.PlanetSelect);
                        }
                    }
                    break;
            }
        }
    }
}