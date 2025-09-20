using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.VisualScripting;

public class Director : MonoBehaviour
{
    [SerializeField] DialogueManager dialogueManager;
    [SerializeField] Minecart minecart;
    [SerializeField] DrOhnoScript drOhnoScript;

    List<Order> orderQueue;
    int currentOrderIndex = 100000000;
    float timePassedSinceCurrentOrder = 0;
    enum DirectorJob
    {
        None,
        Momentary,
        DialogueDisplay,
        OhnoFloat,
        Wait,
    }
    DirectorJob currentJob = DirectorJob.None;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Order order;
        orderQueue = new List<Order>();

        order = new Order("dialogue", "set");
        order.SetDialogue("ふははははついにタン・イーを手に入れたぞ（棒）");
        orderQueue.Add(order);

        order = new Order("dialogue", "set");
        order.SetDialogue("これがあればXXXやYYYさらにはZZZまで私の思うままだ（棒）");
        orderQueue.Add(order);

        order = new Order("dialogue", "set");
        order.SetDialogue("だがこの事実を知っているお前たちは邪魔だ（棒）");
        orderQueue.Add(order);

        orderQueue.Add(new Order("ohno_float", "start"));

        order = new Order("dialogue", "set");
        order.SetDialogue("見よこれがタン・イーの力（棒）");
        orderQueue.Add(order);

        orderQueue.Add(new Order("minecart", "start"));

        order = new Order("wait", "default");
        order.SetLength(1f);
        orderQueue.Add(order);

        order = new Order("dialogue", "set");
        order.SetDialogue("逃げるつもりか無駄なことを（棒）");
        orderQueue.Add(order);

        orderQueue.Add(new Order("ohno_chase", "start"));

        order = new Order("wait", "default");
        order.SetLength(1f);
        orderQueue.Add(order);

        order = new Order("dialogue", "set");
        order.SetDialogue("この世界から消え去るがいい（棒）");
        orderQueue.Add(order);

        order = new Order("wait", "default");
        order.SetLength(5f);
        orderQueue.Add(order);

        order = new Order("dialogue", "set");
        order.SetDialogue("ぐはああ（棒）");
        orderQueue.Add(order);

        orderQueue.Add(new Order("ohno_chase", "stop"));

        orderQueue.Add(new Order("ohno_float", "stop"));

        order = new Order("wait", "default");
        order.SetLength(1f);
        orderQueue.Add(order);

        orderQueue.Add(new Order("minecart", "stop"));

        currentOrderIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //progress the time
        timePassedSinceCurrentOrder += Time.deltaTime;

        //end a current job when it have finished
        if(currentJob != DirectorJob.None && PreviousOrderJobDone())
        {
            currentJob = DirectorJob.None;
            currentOrderIndex++;
        }

        //start a new current job
        if(currentJob == DirectorJob.None)
        {
            timePassedSinceCurrentOrder = 0;

            if (currentOrderIndex >= orderQueue.Count) return;
            
            ExecuteOrder(orderQueue[currentOrderIndex]);
        }
    }

    void ExecuteOrder(Order order)
    {
        switch(order.category)
        {
            case "dialogue":
                if (order.code == "add")
                {
                    dialogueManager.AddNewDialogue(order.GetDialogue());
                }
                if (order.code == "set")
                {
                    dialogueManager.SetNewDialogue(order.GetDialogue());
                }
                currentJob = DirectorJob.DialogueDisplay;
                break;
            case "minecart":
                if (order.code == "start")
                {
                    minecart.StartMove();
                }
                if (order.code == "stop")
                {
                    minecart.StopMove();
                }
                currentJob = DirectorJob.Momentary;
                break;
            case "ohno_float":
                if (order.code == "start")
                {
                    drOhnoScript.StartFloat();
                }
                if (order.code == "stop")
                {
                    drOhnoScript.StopFloat();
                }
                currentJob = DirectorJob.OhnoFloat;
                break;
            case "ohno_chase":
                if (order.code == "start")
                {
                    drOhnoScript.StartChase();
                }
                if (order.code == "stop")
                {
                    drOhnoScript.StopChase();
                }
                currentJob = DirectorJob.Momentary;
                break;
            case "wait":
                currentJob = DirectorJob.Wait;
                break;
            default:
                break;
        }
    }

    bool PreviousOrderJobDone()
    {
        switch(currentJob)
        {
            case DirectorJob.DialogueDisplay:
                return dialogueManager.DialogueDisplayJobEnd();
            case DirectorJob.OhnoFloat:
                return drOhnoScript.FloatingJobEnd();
            case DirectorJob.Wait:
                return orderQueue[currentOrderIndex].GetLength() <= this.timePassedSinceCurrentOrder;
            case DirectorJob.Momentary:
            case DirectorJob.None:
            default:
                return true;
        }
    }
}