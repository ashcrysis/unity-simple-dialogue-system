using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI speakerNameComponent;
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public string[] speakers;
    public float textSpeed;
    private int index;
    private AudioSource audio;
    private static bool buttonclicked = false;
    List<char> listPause = new List<char> { ',', '.','?' };
    void Start()
    {
        speakerNameComponent.text = speakers[index];
        textComponent.text = string.Empty;
        audio = GetComponent<AudioSource>();
        StartDialogue();
    }

    void Update()
    {
      
        if (buttonclicked)
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();

                textComponent.text = lines[index];
         
            }
        buttonclicked = false;
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

IEnumerator TypeLine()
{
    string line = lines[index];
    textComponent.text = string.Empty;

    int tagStart = 0;
    int tagEnd = 0;
    bool containsColourTag = false;

    for (int i = 0; i < line.Length; i++)
    {
        if (i % 2 == 0 && !listPause.Contains(line[i])){

        audio.Play();
        }
        if (line[i] == '.'){
            audio.Play();
            yield return new WaitForSeconds(0.2f);
        }
        if (line[i] == '<')
        {
            tagEnd = line.IndexOf('>', i);

            if (tagEnd != -1)
            {
                string tag = line.Substring(i, tagEnd - i + 1);
                textComponent.text += tag;
                // Check if it's a color tag
                if (tag.ToLower().Contains("color"))
                {
                    // Check if it's a red, yellow, or orange color tag
                    if (tag.ToLower().Contains("red") || tag.ToLower().Contains("yellow") || tag.ToLower().Contains("orange"))
                    {
                        containsColourTag = true;
                    }
                    else
                    {
                        containsColourTag = false;
                    }
                }

                i = tagEnd;

                yield return null; // Wait for one frame before processing the next character.
            }
        }
        else
        {
            if (containsColourTag)
            {
                textComponent.text += line[i];
            }
            else
            {
                textComponent.text += "<color=white>" + line[i] + "</color>";
            }

            yield return new WaitForSeconds(textSpeed);
        }
        if (listPause.Contains(line[i])){
            yield return new WaitForSeconds(0.4f);
        }
    }
textComponent.text = lines[index];
}

public void OnButtonClick(){
    ButtonClick();
    Debug.Log("Has Clicked");
}
void ButtonClick(){
    Debug.Log(buttonclicked);
    buttonclicked = true;
}


   void NextLine()
{
    if (index < lines.Length - 1)
    {
        index++;
        speakerNameComponent.text = speakers[index]; // Update with the current speaker's name
        textComponent.text = string.Empty;
        StartCoroutine(TypeLine());
    }
    else
    {
        gameObject.SetActive(false);
    }
}

}
