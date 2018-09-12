using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffects : MonoBehaviour {

	private UnityEngine.UI.Image fade;

    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    public Transform camTransform;

    // How long the object should shake for.
    public float shakeDuration = 0f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

    public static CameraEffects instance;

    Vector3 originalPos;

    void Awake(){
        if (instance == null) instance = this;
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
        fade = GameObject.Find ("BlackFade").GetComponent<UnityEngine.UI.Image>();
        fade.color = Color.black;
        //fadeOut (1);
	}

	public void fadeIn(float time){
		if(!fade) fade = GameObject.Find ("BlackFade").GetComponent<UnityEngine.UI.Image>();
        fade.color = Color.black;
        fade.canvasRenderer.SetAlpha(0);
        fade.CrossFadeAlpha (1.0f, time, false);
	}

	public void fadeOut(float time){
		if(!fade) fade = GameObject.Find ("BlackFade").GetComponent<UnityEngine.UI.Image>();
        fade.color = Color.black;
        fade.canvasRenderer.SetAlpha(1);
        fade.CrossFadeAlpha (0f, time, false);
	}

    public void fadeColor(float time, Color color)
    {
        if (!fade) fade = GameObject.Find("BlackFade").GetComponent<UnityEngine.UI.Image>();
        fade.color = color;
        fade.canvasRenderer.SetAlpha(1);
        fade.CrossFadeAlpha(0f, time, true);
    }

    public void ShakeCamera(float duration, float intenstiy)
    {
        originalPos = camTransform.localPosition;
        shakeDuration = duration;
        shakeAmount = intenstiy;
    }

    void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else if(shakeDuration < 0)
        {
            shakeDuration = 0f;
            camTransform.localPosition = originalPos;
        }
    }



}
