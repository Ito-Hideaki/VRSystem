using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Director : MonoBehaviour
{
    [SerializeField] DialogueManager dialogueManager;
    [SerializeField] Minecart minecart;

    List<Order> orderQueue;
    int currentOrderIndex = 100000000;
    enum DirectorJob
    {
        None,
        Minecart,
        DialogueDisplay
    }
    DirectorJob currentJob = DirectorJob.None;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CheckOutlet();

        Order order;
        orderQueue = new List<Order>();

        orderQueue.Add(new Order("minecart", "start"));

        order = new Order("dialogue", "set");
        order.SetDialogue("どこへ行こうと言うのかね！");
        orderQueue.Add(order);

        order = new Order("dialogue", "set");
        order.SetDialogue("ははは！");
        orderQueue.Add(order);

        order = new Order("dialogue", "add");
        order.SetDialogue("見ろ！");
        orderQueue.Add(order);

        order = new Order("dialogue", "add");
        order.SetDialogue("人がゴミのようだ！");
        orderQueue.Add(order);

        order = new Order("dialogue", "set");
        order.SetDialogue("３分間待ってやる");
        orderQueue.Add(order);

        order = new Order("dialogue", "set");
        order.SetDialogue("目がー！　目が－！");
        orderQueue.Add(order);

        orderQueue.Add(new Order("minecart", "stop"));

        currentOrderIndex = 0;
    }

    void CheckOutlet()
    {
        if(
            dialogueManager == null
            || minecart == null)
        {
            throw new Exception();
        } 
    }

    // Update is called once per frame
    void Update()
    {
        if(currentJob != DirectorJob.None && PreviousOrderJobDone())
        {
            currentJob = DirectorJob.None;
            currentOrderIndex++;
        }

        if(currentJob == DirectorJob.None)
        {
            if (currentOrderIndex >= orderQueue.Count) return;

            ExecuteOrder(orderQueue[currentOrderIndex]);
        }
    }

    void ExecuteOrder(Order order)
    {
        if(order.category == "dialogue")
        {
            if(order.code == "add")
            {
                dialogueManager.AddNewDialogue(order.GetDialogue());
            }
            if(order.code == "set")
            {
                dialogueManager.SetNewDialogue(order.GetDialogue());
            }
            currentJob = DirectorJob.DialogueDisplay;
        }
        if(order.category == "minecart")
        {
            if(order.code == "start")
            {
                minecart.StartMove();
            }
            if(order.code == "stop")
            {
                minecart.StopMove();
            }
            currentJob = DirectorJob.Minecart;
        }
    }

    bool PreviousOrderJobDone()
    {
        switch(currentJob)
        {
            case DirectorJob.DialogueDisplay:
                return dialogueManager.DialogueDisplayJobEnd();
            case DirectorJob.None:
            case DirectorJob.Minecart:
            default:
                return true;
        }
    }
}