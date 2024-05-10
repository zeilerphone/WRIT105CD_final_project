using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;
//using Ink.Parsed;

public class DialogueManager : MonoBehaviour
{
    public TextAsset inkJSON;
    public GameObject textBox;
    public GameObject customButton;
    public GameObject optionPanel;
    public bool isTalking = false;
    public TextMeshProUGUI nametag;
    public TextMeshProUGUI textComponent;
    static Story _story;
    //List<string> tags;
    static Choice choiceSelected;

    public float textSpeed;

    private Dictionary<int, string> indexMap = new Dictionary<int, string>();
    private bool isTyping = false;
    private bool typeInterrupt = false;
    private bool firstLine = true;
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
        indexMap.Add(0, "start");
        indexMap.Add(1, "suburb");
        indexMap.Add(2, "suburb_fix");
        indexMap.Add(3, "stripmall");
        indexMap.Add(4, "improvement1");
        indexMap.Add(5, "improvement2");
    }

    // Update is called once per frame
    void Update()
    {
        // only display the UI if the player is talking to an NPC
        if(isTalking)
        {   
            if(firstLine) {
                firstLine = false;
                NextLine();
            }
            textBox.SetActive(true);
            if(Input.GetKeyDown(KeyCode.Space))
            {   
                if(!isTyping){
                    // check if the story can continue (if there are more lines to read)
                    if(_story.canContinue && _story.currentChoices.Count == 0)
                    {
                        Debug.Log("Continuing...");
                        NextLine();
                    } 
                    else if(_story.currentChoices.Count == 0)
                    {
                        StopAllCoroutines();
                        FinishDialogue();
                    } else {
                        Debug.Log("Choices Exist");
                    }
                } else {
                    typeInterrupt = true;
                }
            }
        } else  {
            return;
        }
    }
    public void setIndex(int i)
    {
        _story.ChoosePathString(indexMap[i]);
    }
    public string getStoryKnot()
    {
        return _story.variablesState["current_knot"].ToString();
    }
    public int getIndex()
    {
        return (int)_story.variablesState["index"];
    }
    public void setStoryKnot(string knot)
    {
        _story.ChoosePathString(knot);
    }
    private void FinishDialogue()
    {
        textBox.SetActive(false);
        isTalking = false;
        firstLine = true;
    }

    void NextLine()
    {
        string currentLine = _story.Continue();
        Debug.Log("Current Line: " + currentLine);
        StopAllCoroutines();
        StartCoroutine(TypeLine(currentLine));
        if(_story.currentChoices.Count != 0)
        {
            StartCoroutine(ShowChoices());
        }
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        textComponent.text = string.Empty;
        foreach (char letter in line.ToCharArray())
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(textSpeed);
            if(typeInterrupt) {
                textComponent.text = line;
                isTyping = false;
                typeInterrupt = false;
                break;
            }
        }
        isTyping = false;
    }

    IEnumerator ShowChoices()
    {
        List<Choice> _choices = _story.currentChoices;
        Debug.Log("Choices: " + _choices.Count);
        for(int i = 0; i < _choices.Count; i++)
        {
            GameObject button = Instantiate(customButton, optionPanel.transform);
            Debug.Log("Creating button: " + _choices[i].text);
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _choices[i].text;
            button.AddComponent<Selectable>();
            button.GetComponent<Selectable>().element = _choices[i];
            button.GetComponent<Button>().onClick.AddListener(() => {button.GetComponent<Selectable>().Decide(); });
        }
        optionPanel.SetActive(true);
        Debug.Log("Waiting for choice...");
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
