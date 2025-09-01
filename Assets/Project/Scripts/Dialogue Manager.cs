using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dialogueText;
    string characterName;
    string dialogue = "";
    float timePassedSinceLastCharacter = 0;
    const float timePerCharacter = 0.15f;
    int currentDialogueLength = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timePassedSinceLastCharacter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(!this.DialogueDisplayJobEnd())
        {
            this.timePassedSinceLastCharacter += Time.deltaTime;
            if (this.timePassedSinceLastCharacter > timePerCharacter)
            {
                this.timePassedSinceLastCharacter -= timePerCharacter;
                this.currentDialogueLength++;
                this.dialogueText.SetText(this.dialogue.Substring(0, this.currentDialogueLength));
            }
        }
    }

    public void SetNewDialogue(string dialogue)
    {
        this.dialogue = dialogue;
        this.currentDialogueLength = 0;
    }

    public void AddNewDialogue(string dialogue)
    {
        this.dialogue += "\r\n" + dialogue;
        this.timePassedSinceLastCharacter = -1;
    }

    public bool DialogueDisplayJobEnd()
    {
        return this.currentDialogueLength == this.dialogue.Length;
    }
}
