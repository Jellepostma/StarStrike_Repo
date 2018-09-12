using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GUI_Game : MonoBehaviour {

    public static GUI_Game instance;

    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;

    


    //public string nextLevel = "Level_1";

    void Awake()
    {
        if (instance == null) instance = this;
        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
    }

    // Update is called once per frame
    void Update () { 

        //Check if the left Mouse button is clicked
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Set up the new Pointer Event
            m_PointerEventData = new PointerEventData(m_EventSystem);
            //Set the Pointer Event Position to that of the mouse position
            m_PointerEventData.position = Input.mousePosition;

            //Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            m_Raycaster.Raycast(m_PointerEventData, results);

            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.name == "GoButton")
                {
                    Destroy(GameObject.Find("GameOver").GetComponent<Animator>());
                    iTween.ScaleFrom(result.gameObject, new Vector3(2.5f, 2.5f, 2.5f), 0.5f);
                    StartCoroutine("retry");
                }
                if (result.gameObject.name == "Arrow_Back")
                {
                    Destroy(GameObject.Find("GameOver").GetComponent<Animator>());
                    iTween.ScaleFrom(result.gameObject, new Vector3(1.5f, 1.5f, 1.5f), 0.5f);
                    StartCoroutine("toMenu");
                }

                if (result.gameObject.name.Contains("Weapon_"))
                {
                    iTween.ScaleFrom(result.gameObject, new Vector3(1.5f, 1.5f, 1.5f), 0.5f);
                    removeWeaponIcon(result.gameObject.name);
                }
               
            }

        }
    }

    IEnumerator retry()
    {
        
        Camera.main.GetComponent<CameraEffects>().fadeIn(1);

        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        yield return null;
    }

    IEnumerator toMenu()
    {

        Camera.main.GetComponent<CameraEffects>().fadeIn(1);

        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Menu");
        yield return null;
    }

    public void addWeaponIcon(int slot)
    {
        iTween.ScaleTo(GameObject.Find("Weapon_" + slot.ToString()), new Vector3(1.5f, 1.5f, 1.5f), 0.5f);
    }

    public void removeWeaponIcon(string iconName)
    {
        iTween.ScaleTo(GameObject.Find(iconName), Vector3.zero, 0.5f);
        Debug.Log(iconName.Remove(0, 7));
        GameObject.Find("PlayerRotator").GetComponent<WeaponInventory>().removeWeapon(int.Parse(iconName.Remove(0, 7)));
        
    }
}
