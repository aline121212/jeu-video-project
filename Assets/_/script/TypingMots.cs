using TMPro;
using UnityEngine;

public class TypingWord : MonoBehaviour
{
    public TMP_Text label;                
    public float startDelay = 3f;          
    public string[] words = { "tomate", "prison", "lol", "arbitraire" , "babouin", "muettes", "obscure", "creux", "enflammé", "malédiction", "atmosphère", "hasard", "confortables", "marines", "intenion", "silhouette" };

    int index, typed;
    string current;
    bool started;
    bool mistake;

    void Start() { label.text = ""; }

  
    public void StartGame()
    {
        if (started) return;
        started = true;
        Invoke(nameof(NextWord), startDelay);
    }

    void Update()
    {
        if (current == null) return;

        foreach (char c in Input.inputString)
        {
            if (char.ToLower(c) == current[typed])
            {
                typed++;
                mistake = false;
                Refresh();

                if (typed >= current.Length)
                {
                    current = null;
                    Invoke(nameof(NextWord), 0.7f);
                    return;
                }
            }
            else
            {
                mistake = true;
                Refresh();

            }
        }
    }

    void NextWord()
    {
        current = words[index % words.Length];
        index++;
        typed = 0;
        Refresh();
    }

    void Refresh()
    {
        string correct = "<color=#0EC21D>"
                       + current.Substring(0, typed)
                       + "</color>";

        string remaining = current.Substring(typed);

        if (mistake && typed < current.Length)
        {
            remaining = "<color=red>"
                      + remaining.Substring(0, 1)
                      + "</color>"
                      + remaining.Substring(1);
        }

        label.text = correct + remaining;
    }
}