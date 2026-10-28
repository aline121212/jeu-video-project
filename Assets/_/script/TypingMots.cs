using TMPro;
using UnityEngine;

public class TypingWord : MonoBehaviour
{
    public TMP_Text label;                 // the text above the mic
    public float startDelay = 3f;          // seconds after the click (lets the camera pan)
    public string[] words = { "tomate", "maison", "glace", "couleur" };

    int index, typed;
    string current;
    bool started;

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
                Refresh();
                if (typed >= current.Length)
                {
                    current = null;
                    Invoke(nameof(NextWord), 0.7f);
                    return;
                }
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
        label.text = "<color=#4CFF4C>" + current.Substring(0, typed) + "</color>"
                   + current.Substring(typed);
    }
}