using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TypingWord : MonoBehaviour
{
    public TMP_Text label;
    public float startDelay = 3f;
    public string[] words = { "tomate", "prison", "lol", "arbitraire", "babouin", "muettes", "obscure", "creux", "enflammé", "malédiction", "atmosphère", "hasard", "confortables", "marines", "intenion", "silhouette" };
    public AudioSource speaker;
    public AudioClip[] wordSounds;
    public AudioClip mistakeSound;
    public AudioClip completeSound;
    public string nextScene = "niveau2";
    public bool autoStart;
    public UnityEvent onWrong;
    public UnityEvent onRight;
    public TMP_Text counterText;
    public Animator counterAnimator;
    int score;
    public TMP_Text tomatoText;
    public Animator tomatoAnimator;
    int wordsRight, tomatoes;

    int index;
    string current;
    string input = "";  
    bool started;

    void Start()
    {
        label.text = "";
        if (counterText != null) counterText.text = "0";
        if (autoStart) StartGame();
    }

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
           
            if (c == '\b')
            {
                if (input.Length > 0)
                {
                    input = input.Substring(0, input.Length - 1);
                    Refresh();
                }
                continue;
            }

            if (!char.IsLetter(c)) continue;              
            if (input.Length >= current.Length) continue;  

            char lower = char.ToLower(c);

            if (lower != current[input.Length])
            {
                if (speaker != null && mistakeSound != null)
                    speaker.PlayOneShot(mistakeSound);
                onWrong.Invoke();
                score--;

                wordsRight = 0;
                tomatoes = 0;
                if (tomatoText != null) tomatoText.text = "0";
                if (tomatoAnimator != null) tomatoAnimator.SetTrigger("pop");
            }

            else
            {
                score++;
            }
            ShowScore();

            input += lower;
            Refresh();


            if (input == current)
            {
                wordsRight++;
                if (wordsRight >= 5)
                {
                    tomatoes++;
                    if (tomatoText != null) tomatoText.text = tomatoes.ToString();
                    if (tomatoAnimator != null) tomatoAnimator.SetTrigger("pop");
                }

                onRight.Invoke();
                if (speaker != null && completeSound != null)
                    speaker.PlayOneShot(completeSound);

                current = null;
                Invoke(nameof(NextWord), 0.7f);
                return;
            }
        }

    }

    void ShowScore()
    {
        if (counterText != null) counterText.text = score.ToString();
        if (counterAnimator != null) counterAnimator.SetTrigger("pop");
    }

    void NextWord()
    {
        if (index >= words.Length)
        {
            if (!string.IsNullOrEmpty(nextScene)) SceneManager.LoadScene(nextScene);
            return;
        }
        int i = index;
        current = words[i].Trim().ToLower();
        index++;
        input = "";
        Refresh();

        if (speaker != null && i < wordSounds.Length && wordSounds[i] != null)
            speaker.PlayOneShot(wordSounds[i]);
    }

    void Refresh()
    {
        string text = "";
       
        for (int i = 0; i < input.Length; i++)
        {
            string color = input[i] == current[i] ? "#0EC21D" : "red";
            text += "<color=" + color + ">" + input[i] + "</color>";
        }

        
        text += "<color=#8d98a8>|</color>" + current.Substring(input.Length);

        label.text = text;
    }
}