using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueDisplay : MonoBehaviour
{
    #region Inspector members

    public GameObject dialoguePanel;
    public TMP_Text nameText;
    public TMP_Text dialogueText;

    public GameObject dialogueOptionButtonPrefab;

    public float characterOutputDelay;

    #endregion

    public string monologue;
    private Queue<char> monologueCharacters = new Queue<char>();

    public List<string> options;

    private float characterOutputTimer = 0;

    public delegate void OutputCompleteDelegate();
    private OutputCompleteDelegate outputCompleteCallback;

    public static DialogueDisplay instance;
    private void Awake()
    {
        // Singleton
        if (instance == null) instance = this;
        else Destroy(this);
    }

    private void Update()
    {
        // Handle timer
        characterOutputTimer += Time.deltaTime;

        if (Keyboard.current != null && Keyboard.current.vKey.wasPressedThisFrame)
        {
            setDialogue("This is a debug output line", new List<string>{ "option 1", "option 2", "option 3", "option 4" }, onOutputComplete);
        }
    }

    private void onOutputComplete()
    {
        Debug.Log("debug output finished");
    }

    private void FixedUpdate()
    {
        // Output text
        if (monologueCharacters.Count > 0)
        {
            if (characterOutputTimer >= characterOutputDelay)
            {
                // Dequeue and output next character
                char nextChar = monologueCharacters.Dequeue();
                dialogueText.text += nextChar;

                // Decrement timer
                characterOutputTimer -= characterOutputDelay;
            }
        }
        // Sentence already completed
        else
        {
            // Handle callback if we JUST finished output
            if (outputCompleteCallback != null)
            {
                outputCompleteCallback();
                outputCompleteCallback = null;
            }

            // Keep timer at 0
            characterOutputTimer = 0;
        }

        /*
        // Handle alpha
        dialogueAlpha = Mathf.MoveTowards(dialogueAlpha, targetAlpha, fadeSpeed);
        nameText.color = new Color(255, 255, 255, dialogueAlpha);
        dialogueText.color = new Color(255, 255, 255, dialogueAlpha);
        dialogueBox.color = new Color(0, 0, 0, dialogueAlpha * 0.6f);
        */
    }

    #region Dialogue setters

    // All-in-one function
    public void setDialogue(string monologue, List<string> options, OutputCompleteDelegate outputCompleteCallback = null)
    {
        this.monologue = monologue;
        this.options = options;
        this.outputCompleteCallback = outputCompleteCallback;
        updateMonologueCharactersQueue();
        updateDialogueOptions();
    }

    public void setMonologue(string monologue)
    {
        this.monologue = monologue;
        updateMonologueCharactersQueue();
    }

    public void setOptions(List<string> options)
    {
        this.options = options;
        updateDialogueOptions();
    }

    public void setOutputCompleteDelegate(OutputCompleteDelegate outputCompleteCallback = null)
    {
        this.outputCompleteCallback = outputCompleteCallback;
    }

    #endregion

    private void updateMonologueCharactersQueue()
    {
        // Clear current text
        monologueCharacters.Clear();
        dialogueText.text = "";
        // Enqueue new text into character queue
        for (int i = 0; i < monologue.Length; i++) monologueCharacters.Enqueue(monologue[i]);
    }

    private void updateDialogueOptions()
    {
        // Clear existing objects
        EventManager.instance.destroyDialogueOptionButtonsEvent.Invoke();
        // Create new option button objects
        for (int i = 0; i < options.Count; i++)
        {
            createOptionButtonPrefab(i, options[i]);
        }
    }

    private void createOptionButtonPrefab(int optionIndex, string optionText)
    {
        // Initialize new button
        GameObject newOptionPrefab = Instantiate(dialogueOptionButtonPrefab, dialoguePanel.transform);
        newOptionPrefab.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 135 + optionIndex * -90);
        newOptionPrefab.GetComponentInChildren<TMP_Text>().text = optionText;
        newOptionPrefab.GetComponent<Button>().onClick.AddListener(() => onDialogueOptionButtonPressed(optionIndex));
    }

    private void onDialogueOptionButtonPressed(int optionIndex)
    {
        // DO SOMETHING BASED ON optionIndex

        // Clear current options
        options.Clear();

        EventManager.instance.destroyDialogueOptionButtonsEvent.Invoke();
    }
}
