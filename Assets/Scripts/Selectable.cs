using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Selectable : MonoBehaviour
{
    public object element;
    public void Decide()
    {
        DialogueManager.SetDecision(element);
        //Debug.Log("Decided: " + element.ToString());
    }
}