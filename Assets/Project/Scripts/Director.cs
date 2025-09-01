using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour
{
    public DialogueManager dialogueManager;
    int da = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(dialogueManager.DialogueDisplayJobEnd())
        {
            switch(da)
            {
                case 0:
                    dialogueManager.SetNewDialogue("現実が受け入れられないようだね。もう一度教えてあげる。");
                    break;
                case 1:
                    dialogueManager.AddNewDialogue("59点。これが君の最終評価だ。");
                    break;
                case 2:
                    dialogueManager.AddNewDialogue("君、今まで何してたの？");
                    break;
                default:
                    break;
            }

            da++;
        }
    }
}
