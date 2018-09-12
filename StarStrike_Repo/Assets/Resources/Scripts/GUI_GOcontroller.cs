using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_GOcontroller : MonoBehaviour {

    // Use this for initialization
    private float timer = 0;
    public int targetScore, score = 0;
    public float scoreDuration = 5f;
    public Image scoreStar;
    public Sprite hStar;
    public bool hs = false;
    private int oldHS;
    
    public void countScore()
    {
        if (targetScore == 0) return;
        GetComponent<Animator>().enabled = false;
        oldHS = PlayerPrefs.GetInt(GameManager.instance.currentPlanet + "_Highscore");
        StartCoroutine("CountTo");
    }

    IEnumerator CountTo()
    {
        if (score <= targetScore)
        {
            for (int i = 0; i <= targetScore; i++)
            {
                score = i;
                if(score > oldHS && !hs)
                {
                    scoreStar.sprite = hStar;
                    scoreStar.color = GameManager.instance.references["FinalScore"].GetComponent<Text>().color = Color.yellow;
                    iTween.ScaleFrom(scoreStar.gameObject, new Vector3(4, 4, 4), 1f);
                    scoreStar.GetComponentInChildren<ParticleSystem>().Emit(20);
                    hs = true;
                }
                GameManager.instance.references["FinalScore"].GetComponent<Text>().text = score.ToString();
                yield return new WaitForSeconds(scoreDuration / targetScore);
            }
            
        }
        PlayerPrefs.SetInt(GameManager.instance.currentPlanet + "_Highscore", score);
        GetComponent<Animator>().enabled = true;
        yield return null;
    }
}
