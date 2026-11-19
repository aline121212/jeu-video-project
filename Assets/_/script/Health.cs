using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Health points: the value, its rules (between 0 and the maximum), its display and its events.
/// The CES calls Remove(int) or Add(int) with an Invoke Events action.
/// </summary>
public class Health : MonoBehaviour
{
    [Header("Value")]
    [Tooltip("Maximum health points")]
    public int maximum = 100;

    [Tooltip("Health points at start")]
    public int startValue = 100;
    [Header("Tomatoes (optional)")]
    [Tooltip("tomate-hp-1, -2, -3 in order")]
    public UnityEngine.UI.Image[] tomatoes;
    public Sprite newTomato;
    public Sprite oldTomato;

    [Header("Display (one, both or none)")]
    [Tooltip("Canvas text")]
    public TMP_Text text;

    [Tooltip("{0} = health, {1} = maximum")]
    public string format = "{0} / {1}";

    [Tooltip("Rectangle or 9-slice image whose width follows health. Set Pivot X to 0 so it shrinks to the left.")]
    public RectTransform bar;

    [Tooltip("Speed at which the bar slides to the new value (points per second). 0 = instant.")]
    public float barSpeed = 100f;

    [Header("Events")]
    public UnityEvent onDamaged;
    public UnityEvent onHealed;
    public UnityEvent onDeath;

    int value;
    float barValue;
    float fullWidth;

    public int Value => value;
    public bool IsDead => value <= 0;

    void Start()
    {
        value = Mathf.Clamp(startValue, 0, maximum);
        if (bar != null) fullWidth = bar.sizeDelta.x;
        barValue = value;
        Display();
    }

    void Update()
    {
        // The bar slides to the value instead of jumping
        if (bar != null && !Mathf.Approximately(barValue, value))
        {
            barValue = barSpeed > 0
                ? Mathf.MoveTowards(barValue, value, barSpeed * Time.deltaTime)
                : value;
            DisplayBar();
        }
    }

    // --- Public methods: visible in Invoke Events and On Click ---

    public void Remove(int points) => Set(value - points);
    public void Add(int points) => Set(value + points);
    public void Fill() => Set(maximum);
    public void ResetValue() => Set(startValue);
    public void LoadScene(string sceneName) => SceneManager.LoadScene(sceneName);
    public void ReloadScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    [ContextMenu("Test Remove 1")]
    void TestRemove() => Remove(1);

    // --- Rules ---

    void Set(int newValue)
    {
        newValue = Mathf.Clamp(newValue, 0, maximum);
        int oldValue = value;
        value = newValue;
        Display();

        if (value < oldValue) onDamaged.Invoke();
        if (value > oldValue) onHealed.Invoke();
        if (value == 0 && oldValue > 0) onDeath.Invoke();
    }

    // --- Display ---

    void Display()
    {
        if (text != null) text.text = string.Format(format, value, maximum);
        DisplayTomatoes();   // <- add this
        if (bar != null && barSpeed <= 0) { barValue = value; DisplayBar(); }
    }

    void DisplayBar()
    {
        float ratio = maximum > 0 ? barValue / maximum : 0;
        bar.sizeDelta = new Vector2(fullWidth * ratio, bar.sizeDelta.y);
        // A 9-slice image narrower than its corners gets distorted: hide it at 0
        bar.gameObject.SetActive(barValue > 0.01f);
    }

    void DisplayTomatoes()
    {
        if (tomatoes == null) return;
        for (int i = 0; i < tomatoes.Length; i++)
        {
            if (tomatoes[i] == null) continue;
            tomatoes[i].sprite = (i < value) ? newTomato : oldTomato;
        }
    }
}
