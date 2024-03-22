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
    public int numCoinsNeeded;
    public GameObject textBox;
    public GameObject customButton;
    public GameObject optionPanel;
    public bool isTalking = false;
    public TextMeshProUGUI nametag;
    public TextMeshProUGUI textComponent;
    static Story _story;
    List<string> tags;
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
        indexMap.Add(0, "start");
        indexMap.Add(1, "suburb");
        indexMap.Add(2, "suburb_fix");
        indexMap.Add(1, "stripmall");
        indexMap.Add(5, "mid_density");
        indexMap.Add(6, "mid_density_fix");
    }

    // Update is called once per frame
    void Update()
    {
        // only display the UI if the player is talking to an NPC
        if(isTalking)
        {
            textBox.SetActive(true);
            if(Input.GetKeyDown(KeyCode.Space))
            {   
                // check if the story can continue (if there are more lines to read)
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
    private void FinishDialogue()
    {
        textBox.SetActive(false);
        isTalking = false;
    }

    void NextLine()
    {
        string currentLine = _story.Continue();
        ParseTags();
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

    void ParseTags()
    {
        // get the tags from the current line
        tags = _story.currentTags;
        //
        if(tags.Count > 0)
        {
            foreach(string tag in tags)
            {
                // tags are structured like this:
                // # prefix:value  (e.g. #name:Bob -- note no space after the colon)
                // use C# string split to split the string
                string [] parsedTag = tag.Split(':');
                // get the prefix and value
                string prefix = parsedTag[0];
                string value = parsedTag[1];
                // use a switch statement to check the prefix
                switch(prefix.ToLower())
                {
                    case "name":
                        // set the name tag to the value
                        nametag.text = value;
                        break;
                }
            }
        }
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
