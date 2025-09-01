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
                    dialogueManager.SetNewDialogue("�������󂯓�����Ȃ��悤���ˁB������x�����Ă�����B");
                    break;
                case 1:
                    dialogueManager.AddNewDialogue("59�_�B���ꂪ�N�̍ŏI�]�����B");
                    break;
                case 2:
                    dialogueManager.AddNewDialogue("�N�A���܂ŉ����Ă��́H");
                    break;
                default:
                    break;
            }

            da++;
        }
    }
}
