using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Points : MonoBehaviour
{
    private TextMeshProUGUI point;
    public static int pointCounter;

    void Start()
    {
        point = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        point.text = "Points: " + pointCounter;
    }
}
