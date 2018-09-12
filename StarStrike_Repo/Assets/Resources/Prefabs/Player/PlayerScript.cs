using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {

    public bool started = false;
    public bool canMove = false;
    public Transform LookTransform;
    public WeaponInventory inventory;
    public float speed = 6.0f;
    public float maxVelocityChange = 10.0f;
    public float fuelDepletion = 0.05f;
    public float fuelRate = 0.005f;

    public float fuel = 100.0f;

    public List<MaskableGraphic> colorComponents;

    RectTransform moveStick;
    private Rigidbody rb;
    private Vector2 touchPos, endpos;
    public Color playerColor;

    public float Fuel
    {
        get
        {
            return fuel;
        }

        set
        {
            fuel = Mathf.Clamp(value, 0, 100);
        }
    }

    // Set references and initialize player
    void Start () {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3();
        moveStick = GameManager.instance.references["MoveStick_Sub"].GetComponent<RectTransform>();
        //touchPos = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        touchPos = GameManager.instance.references["MoveStick_Main"].transform.position;
        inventory = GetComponent<WeaponInventory>();
        SetColor(Color.cyan);
        started = true;
    }

    /// <summary>
    /// Shifts the player color hue with the amount provided
    /// </summary>
    /// <param name="amount">Amount of hue shift</param>
    void HueShift(float amount)
    {
        Color temp = playerColor;
        float h, s, v;
        Color.RGBToHSV(temp, out h, out s, out v);
        h += amount;
        SetColor(Color.HSVToRGB(h, s, v));
    }

    /// <summary>
    /// Sets various parts of the interface to the color provided
    /// </summary>
    /// <param name="color">Color to change to</param>
    public void SetColor(Color color)
    {
        foreach (MaskableGraphic m in colorComponents)
        {
            m.color = color;
        }
        playerColor = color;
    }

    /// <summary>
    /// Gets the angle between two Vector2 provided
    /// </summary>
    /// <param name="A">First Vector2 in the angle calculation</param>
    /// <param name="B">Second Vector2 in the angle calculation</param>
    public float GetAngle(Vector2 A, Vector2 B)
    {

        var Delta = B - A;
        var angleRadians = Mathf.Atan2(Delta.y, Delta.x);
        var angleDegrees = angleRadians * Mathf.Rad2Deg;

        if (angleDegrees < 0)
            angleDegrees += 360;

        return angleDegrees;
    }

    // Update is called once per frame
    void Update()
    {
        if (!started) return;

        if (canMove)
        {
            fuel -= fuelDepletion;
            
            if (Input.GetMouseButton(0))
            {
            
                endpos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                float angle = GetAngle(touchPos, endpos) - 90;
                LookTransform.localRotation = Quaternion.AngleAxis(-angle, Vector3.up);
                Vector3 forward = Vector3.Cross(transform.up, LookTransform.forward).normalized;
                
                rb.AddTorque((forward * Mathf.Clamp(Vector2.Distance(touchPos, endpos) / 100, 0, maxVelocityChange)) * speed, ForceMode.Impulse);
                moveStick.localPosition = -(touchPos - endpos) / 5;
                

                for (int i = 0; i < inventory.getSize(); i++)
                {
                    Fuel -= inventory.weapons[i].GetComponent<Rigidbody>().velocity.magnitude * inventory.weapons[i].GetComponent<WeaponProperties>().mass * fuelRate;
                }
            } else
            {
                
                moveStick.localPosition = Vector3.Lerp(moveStick.localPosition, Vector3.zero, Time.deltaTime * 10);
            }
        }

        if (Fuel <= 25.0f)
        {
            if (playerColor != Color.red) SetColor(Color.red);
            if (Fuel <= 0)
            {
                started = false;
                GameManager.instance.GameOver();
            }
        } else {
            if (playerColor != Color.cyan) SetColor(Color.cyan);
        }

        if (Mathf.FloorToInt(Fuel % 5) == 0)
        {
            GameManager.instance.references["FuelGauge_L"].GetComponent<Image>().fillAmount = GameManager.instance.references["FuelGauge_R"].GetComponent<Image>().fillAmount = Fuel / 100;
        }
    }
}
