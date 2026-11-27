using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Put this on the PLAYER (same object as LeadController).
// It builds the HUD by itself when the game starts.
public class PlayerHUD : MonoBehaviour
{
    public GlobalTimer timer;
    [SerializeField]
    AnimalRecoveryTracker animalRecoveryTracker;

    [Header("Optional: drag your Pixel Operator font asset here")]
    public TMP_FontAsset pixelFont;

    [Header("PS1 Colors")]
    public Color textColor = new Color(0.85f, 0.82f, 0.74f);   // bone white
    public Color normalColor = new Color(0.85f, 0.82f, 0.74f, 0.6f);
    public Color aimingColor = new Color(0.62f, 0.72f, 0.42f); // sickly green

    LeadController leadController;

    // HUD pieces (created in BuildHUD)
    Image crosshair;
    TextMeshProUGUI leadPrompt;
    TextMeshProUGUI timerText;
    TextMeshProUGUI animalCountText;
    Image itemIcon;
    TextMeshProUGUI itemNameText;

    void Awake()
    {
        leadController = GetComponent<LeadController>();
        animalRecoveryTracker.ProgressChanged += SetAnimalCount;
        BuildHUD();
    }

    void Start()
    {
        // Placeholder values
        timerText.text = "--:--";
        leadPrompt.text = "";
        animalCountText.text = "ANIMALS -/-";
        SetEquippedItem("None", null);
    }

    void Update()
    {
        UpdateCrosshair();
        UpdateLeadPrompt();
        SetTimer(timer.GetTargetTime() - timer.GetElapsedTime());
    }

    void BuildHUD()
    {
        // Canvas
        GameObject canvasObject = new GameObject("HUD");
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.pixelPerfect = true;
        canvas.sortingOrder = 0; // knockout eyelids (sort order 10) draw on top

        // Low-res 640x360 layout so everything scales up in clean pixel steps
        CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(640, 360);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 1f;

        Transform hud = canvasObject.transform;

        // Anchors: (0.5, 0.5) = center, (0.5, 1) = top-center, (0, 1) = top-left,   (1, 0) = bottom-right
        crosshair = CreateImage(hud, "Crosshair", new Vector2(0.5f, 0.5f), new Vector2(0, 0), new Vector2(2, 2));
        leadPrompt = CreateText(hud, "LeadPrompt", new Vector2(0.5f, 0.5f), new Vector2(0, -24), new Vector2(300, 20), 16, TextAlignmentOptions.Center);
        timerText = CreateText(hud, "TimerText", new Vector2(0.5f, 1f), new Vector2(0, -20), new Vector2(120, 32), 32, TextAlignmentOptions.Center);
        animalCountText = CreateText(hud, "AnimalCountText", new Vector2(0f, 1f), new Vector2(90, -18), new Vector2(160, 20), 16, TextAlignmentOptions.Left);
        itemIcon = CreateImage(hud, "ItemIcon", new Vector2(1f, 0f), new Vector2(-40, 52), new Vector2(32, 32));
        itemNameText = CreateText(hud, "ItemNameText", new Vector2(1f, 0f), new Vector2(-40, 20), new Vector2(120, 20), 16, TextAlignmentOptions.Center);
    }

    // Makes a UI image (used for the crosshair and item icon)
    Image CreateImage(Transform parent, string name, Vector2 anchor, Vector2 position, Vector2 size)
    {
        GameObject obj = new GameObject(name, typeof(RectTransform));
        obj.transform.SetParent(parent, false);

        Image image = obj.AddComponent<Image>();
        image.raycastTarget = false;

        SetPosition(image.rectTransform, anchor, position, size);
        return image;
    }

    // Makes a UI text object
    TextMeshProUGUI CreateText(Transform parent, string name, Vector2 anchor, Vector2 position, Vector2 size, float fontSize, TextAlignmentOptions alignment)
    {
        GameObject obj = new GameObject(name, typeof(RectTransform));
        obj.transform.SetParent(parent, false);

        TextMeshProUGUI text = obj.AddComponent<TextMeshProUGUI>();
        if (pixelFont != null)
        {
            text.font = pixelFont;
        }
        text.fontSize = fontSize;
        text.alignment = alignment;
        text.color = textColor;
        text.raycastTarget = false;

        SetPosition(text.rectTransform, anchor, position, size);
        return text;
    }

    // Places a UI element on the screen
    void SetPosition(RectTransform rect, Vector2 anchor, Vector2 position, Vector2 size)
    {
        rect.anchorMin = anchor;
        rect.anchorMax = anchor;
        rect.anchoredPosition = position;
        rect.sizeDelta = size;
    }

    // Crosshair turns green when aiming at something the player can lead
    void UpdateCrosshair()
    {
        if (leadController.AimingAtAnimal)
        {
            crosshair.color = aimingColor;
        }
        else
        {
            crosshair.color = normalColor;
        }
    }

    // Tells the player how to lead or let go of an animal
    void UpdateLeadPrompt()
    {
        if (leadController.IsLeading)
        {
            leadPrompt.text = "RELEASE LEFT CLICK TO LET GO";
        }
        else if (leadController.AimingAtAnimal)
        {
            leadPrompt.text = "HOLD LEFT CLICK TO LEAD";
        }
        else
        {
            leadPrompt.text = "";
        }
    }

    public void SetTimer(float timeLeft)
    {
        if (timeLeft < 0)
        {
            timeLeft = 0;
        }

        int minutes = Mathf.FloorToInt(timeLeft / 60f);
        int seconds = Mathf.FloorToInt(timeLeft % 60f);
        timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    // Call this whenever an animal is recovered
    // Example: SetAnimalCount(3, 10) shows "ANIMALS 3/10"
    public void SetAnimalCount(int recovered, int total)
    {
        animalCountText.text = "ANIMALS " + recovered + "/" + total;
    }

    // Gabriel's item system calls this when the player switches items
    public void SetEquippedItem(string itemName, Sprite icon)
    {
        itemNameText.text = itemName.ToUpper();

        if (icon != null)
        {
            itemIcon.sprite = icon;
            itemIcon.enabled = true;
        }
        else
        {
            itemIcon.enabled = false;
        }
    }
}
