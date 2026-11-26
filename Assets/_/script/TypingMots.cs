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
    public TMP_Text livesText;
    public string defeatScene = "defaite";
    public int startLives = 3;
    int lives;
    public float streakDuration = 1.5f;
    public float fadeSpeed = 4f;
    float streakTarget;
    public ParticleSystem boom;
    public float boomDelay = 0.3f;


    int index;
    string current;
    string input = "";  
    bool started;

    void Start()
    {
        label.text = "";
        if (counterText != null) counterText.text = "0";
        if (tomatoText != null) tomatoText.text = "";
        lives = startLives;
        ShowLives();
        if (autoStart) StartGame();
        if (tomatoText != null) tomatoText.alpha = 0f;
    }

    public void StartGame()
    {
        if (started) return;
        started = true;
        Invoke(nameof(NextWord), startDelay);
    }

    void Update()
    {
        if (tomatoText != null)
            tomatoText.alpha = Mathf.MoveTowards(tomatoText.alpha, streakTarget, fadeSpeed * Time.deltaTime);
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


                wordsRight = 0;
                ShowStreak();
                lives--;
                ShowLives();
                if (lives <= 0)
                {
                    SceneManager.LoadScene(defeatScene);
                    return;
                }
            }

         

            input += lower;


            Refresh();

            

            if (input == current)
            {
                wordsRight++;
                score++;
                ShowScore();
                lives++;
                ShowLives();
                ShowStreak();

                onRight.Invoke();
                Invoke(nameof(PlayBoom), boomDelay);
                if (speaker != null && completeSound != null)
                    speaker.PlayOneShot(completeSound);

                current = null;
                Invoke(nameof(NextWord), 0.7f);
                return;
            }
        }

    }

    void ShowLives()
    {
        if (livesText != null) livesText.text = " " + lives;
    }

    void ShowStreak()
    {
        if (tomatoText == null) return;

        CancelInvoke(nameof(HideStreak));

        if (wordsRight >= 2)
        {
            tomatoText.text = "x" + wordsRight + " streak";
            tomatoText.alpha = 0f;
            streakTarget = 1f;
            if (tomatoAnimator != null) tomatoAnimator.SetTrigger("pop");
            Invoke(nameof(HideStreak), streakDuration);
        }
        else
        {
            streakTarget = 0f;
        }
    }

    void HideStreak()
    {

        streakTarget = 0f;
    }
    void PlayBoom()
    {
        if (boom != null) boom.Play();
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
        CancelInvoke(nameof(PlayBoom));
        if (boom != null) boom.Stop(true, ParticleSystemStopBehavior.StopEmitting);

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