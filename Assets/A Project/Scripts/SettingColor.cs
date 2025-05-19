using UnityEngine;

public class SettingColor : MonoBehaviour
{
    public Color newColor = Color.green;

    void Start()
    {
        GetComponent<Renderer>().material.color = newColor;
    }
}
