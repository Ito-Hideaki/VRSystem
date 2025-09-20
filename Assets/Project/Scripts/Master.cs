using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Master : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


// DirectorÇ™ì«Ç›çûÇﬁÇ–Ç∆Ç¬Ç–Ç∆Ç¬ÇÃñΩóﬂ
public class Order
{
    public readonly string code;
    public readonly string category;
    string dialogue = "";
    float length = 0;
    string[] allowedCategories = new string[] { "default", "dialogue", "minecart", "ohno_float", "ohno_chase", "wait", };

    string[] AllowedCodes()
    {
        switch(category)
        {
            case "dialogue":
                return new string[] { "add", "set" };
            case "minecart":
            case "ohno_float":
            case "ohno_chase":
                return new string[] { "start", "stop" };
            case "default":
            case "wait":
            default:
                return new string[] { "default" };
        }
    }

    public Order(string category, string code)
    {
        if (!allowedCategories.Contains(category)) throw new Exception();
        this.category = category;
        if (!AllowedCodes().Contains(code)) throw new Exception();
        this.code = code;
    }

    public void SetDialogue(string dialogue)
    {
        if(category != "dialogue") throw new Exception();
        this.dialogue = dialogue;
    }

    public string GetDialogue()
    {
        if (category != "dialogue") throw new Exception();
        return dialogue;
    }

    public void SetLength(float length)
    {
        if (category != "wait") throw new Exception();
        this.length = length;
    }

    public float GetLength()
    {
        if (category != "wait") throw new Exception();
        return this.length;
    }
}