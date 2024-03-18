using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.UIElements;
using Ink.Runtime;
using UnityEngine.UI;
//using Ink.Parsed;

public class DialogueManager : MonoBehaviour
{
    public TextAsset inkJSON;
    public int numCoinsNeeded;
    public GameObject textBox;
    public GameObject customButton;
    public GameObject optionPanel;
    public bool isTalking = false;
    public TextMeshProUGUI nametag;
    public TextMeshProUGUI textComponent;
    static Story _story;
    static Choice choiceSelected;

    public float textSpeed;

    private Dictionary<int, string> indexMap = new Dictionary<int, string>();
    // Start is called before the first frame update
    void Start()
    {
        _story = new Story(inkJSON.text);
        choiceSelected = null;
        
        nametag.text = "";
        textComponent.text = "Press <Space> to begin dialogue...";
        optionPanel.SetActive(false);
        //StartDialogue();
        textBox.SetActive(false);
        indexMap.Add(0, "first_interaction");
        indexMap.Add(1, "other_interaction");
        _story.variablesState["num_coins_needed"] = numCoinsNeeded;
    }

    // Update is called once per frame
    void Update()
    {
        if(isTalking)
        {
            textBox.SetActive(true);
            if(Input.GetKeyDown(KeyCode.Space))
            {
                if(_story.canContinue && _story.currentChoices.Count == 0)
                {
                    Debug.Log("Continuing...");
                    NextLine();
                    if(_story.currentChoices.Count != 0)
                    {
                        StartCoroutine(ShowChoices());
                    }
                } 
                else if(_story.canContinue && _story.currentChoices.Count != 0)
                {
                    //StartCoroutine(ShowChoices());
                } 
                else
                {
                    StopAllCoroutines();
                    FinishDialogue();
                }
            }
        }
    }
    public void setIndex(int i)
    {
        _story.ChoosePathString(indexMap[i]);
    }
    public void setCoins(int c)
    {
        _story.variablesState["coins"] = c;
    }
    private void FinishDialogue()
    {
        textBox.SetActive(false);
        isTalking = false;
    }

    void NextLine()
    {
        string currentLine = _story.Continue();
        StopAllCoroutines();
        StartCoroutine(TypeLine(currentLine));
    }

    IEnumerator TypeLine(string line)
    {
        textComponent.text = string.Empty;
        foreach (char letter in line.ToCharArray())
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    IEnumerator ShowChoices()
    {
        List<Choice> _choices = _story.currentChoices;

        for(int i = 0; i < _choices.Count; i++)
        {
            GameObject button = Instantiate(customButton, optionPanel.transform);
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _choices[i].text;
            button.AddComponent<Selectable>();
            button.GetComponent<Selectable>().element = _choices[i];
            button.GetComponent<Button>().onClick.AddListener(() => button.GetComponent<Selectable>().Decide());
            
        }
        optionPanel.SetActive(true);
        yield return new WaitUntil(() => {return choiceSelected != null;});

        AdvanceFromDecision();
    }

    public static void SetDecision(object element)
    {
        choiceSelected = (Choice)element;
        _story.ChooseChoiceIndex(choiceSelected.index);
    }

    void AdvanceFromDecision()
    {
        optionPanel.SetActive(false);
        for(int i = 0; i < optionPanel.transform.childCount; i++)
        {
            Destroy(optionPanel.transform.GetChild(i).gameObject);
        }
        choiceSelected = null;

        NextLine();
    }

}
