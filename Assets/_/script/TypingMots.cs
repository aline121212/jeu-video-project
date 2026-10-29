using TMPro;
using UnityEngine;

public class TypingWord : MonoBehaviour
{
    public TMP_Text label;                 // the text above the mic
    public float startDelay = 3f;          // seconds after the click (lets the camera pan)
    public string[] words = { "tomate", "mollusque", "glace", "couleur" , "bambouin" };

    int index, typed;
    string current;
    bool started;
    bool mistake;

    void Start() { label.text = ""; }

    // The button calls this
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