using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class coinCounter : MonoBehaviour
{
    public MouseHandler MouseController;
    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<TextMeshProUGUI>().text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        GetComponentInChildren<TextMeshProUGUI>().text = MouseController.numCoins.ToString();
    }
}
