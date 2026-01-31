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

    public float characterOutputDelay;

    public GameObject dialogueOptionButtonPrefab;
    public Image optionsPanelDroplets;

    public float optionsPanelAlphaMoveSpeed;

    public Slider expressionSlider;

    #endregion

    public string monologue;
    private Queue<char> monologueCharacters = new Queue<char>();

    public List<string> options;
    private bool optionsShown = true;
    private bool optionSelected = true;

    private float characterOutputTimer = 0;

    public delegate void OutputCompleteDelegate();
    private OutputCompleteDelegate outputCompleteCallback;

    private float optionsPanelTargetAlpha = 0;

    // Input
    private bool digit1KeyDown = false;
    private bool digit2KeyDown = false;
    private bool digit3KeyDown = false;
    private bool digit4KeyDown = false;

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

        // Handle input
        if (Keyboard.current != null && Keyboard.current.digit1Key.wasPressedThisFrame) digit1KeyDown = true;
        if (Keyboard.current != null && Keyboard.current.digit2Key.wasPressedThisFrame) digit2KeyDown = true;
        if (Keyboard.current != null && Keyboard.current.digit3Key.wasPressedThisFrame) digit3KeyDown = true;
        if (Keyboard.current != null && Keyboard.current.digit4Key.wasPressedThisFrame) digit4KeyDown = true;

        if (Keyboard.current != null && Keyboard.current.vKey.wasPressedThisFrame)
        {
            string debugText = "The Fall of Constantinople, also known as the Conquest of Constantinople, was the capture of the capital of the Byzantine Empire by the Ottoman Empire. The city was captured on 29 May 1453 as part of the culmination of a 53-day siege which had begun on 6 April.";
            debugText = "Hello world!";
            setDialogue("Fall of Constantinople", debugText, new List<string>{ "Option 1", "Option 2", "Option 3", "Option 4" }, onDebugOutputComplete);
        }
    }

    private void onDebugOutputComplete()
    {
        Debug.Log("debug output finished");
    }

    private void FixedUpdate()
    {
        // Handle input
        if (digit1KeyDown) onDialogueOptionButtonPressed(1);
        if (digit2KeyDown) onDialogueOptionButtonPressed(2);
        if (digit3KeyDown) onDialogueOptionButtonPressed(3);
        if (digit4KeyDown) onDialogueOptionButtonPressed(4);

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

            if (!optionsShown)
            {
                updateDialogueOptions();
                optionsShown = true;
            }

            // Keep timer at 0
            characterOutputTimer = 0;
        }

        // Handle alpha
        float optionsPanelAlpha = Mathf.MoveTowards(optionsPanelDroplets.color.a, optionsPanelTargetAlpha, optionsPanelAlphaMoveSpeed);
        optionsPanelDroplets.color = new Color(1, 1, 1, optionsPanelAlpha);

        // Reset key downs
        digit1KeyDown = false;
        digit2KeyDown = false;
        digit3KeyDown = false;
        digit4KeyDown = false;
    }

    public float getExpressionSliderValue()
    {
        return expressionSlider.value;
    }

    #region Dialogue setters

    // All-in-one function
    public void setDialogue(string name, string monologue, List<string> options, OutputCompleteDelegate outputCompleteCallback = null)
    {
        nameText.text = name;
        this.monologue = monologue;
        this.options = options;
        this.outputCompleteCallback = outputCompleteCallback;
        updateMonologueCharactersQueue();
        clearDialogueOptions();
    }

    public void setName(string name)
    {
        nameText.text = name;
    }

    public void setMonologue(string monologue)
    {
        this.monologue = monologue;
        updateMonologueCharactersQueue();
    }

    public void setOptions(List<string> options)
    {
        this.options = options;
        clearDialogueOptions();
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

    private void clearDialogueOptions()
    {
        // Clear existing objects
        optionsShown = false;
        EventManager.instance.destroyDialogueOptionButtonsEvent.Invoke();
        optionsPanelTargetAlpha = 0;
    }

    private void updateDialogueOptions()
    {
        // Create new option button objects
        for (int i = 0; i < options.Count; i++)
        {
            createOptionButtonPrefab(i, options[i]);
        }
        optionsPanelTargetAlpha = 0.3f;
        optionSelected = false;
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
        if (optionSelected) return; // Prevent player from clicking options multiple times
        optionSelected = true;

        Debug.Log("option selected: " + optionIndex);

        // DO SOMETHING BASED ON optionIndex

        // Clear current options
        options.Clear();

        EventManager.instance.destroyDialogueOptionButtonsEvent.Invoke();
        optionsPanelTargetAlpha = 0;
    }
}
