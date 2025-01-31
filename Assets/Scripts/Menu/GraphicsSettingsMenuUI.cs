using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsSettingsMenuUI : MenuUI
{
    public static GraphicsSettingsMenuUI instance { get; private set; }
    [Header("UI References")]
    [SerializeField] TMP_Dropdown qualityDropdown; // Assign in Inspector
    [SerializeField] TMP_Dropdown resolutionDropdown; // Assign in Inspector
    [SerializeField] Button applyButton;           // Button to apply settings
    [SerializeField] Button backButton;
    System.Collections.Generic.List<Vector2Int> customResolutions;
    private void Awake()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one GraphicsSettingsMenuUI.");
            Destroy(gameObject);
            return;
        }
        instance = this;
        Debug.Log("GraphicsSettingsMenuUI Created.");
        customResolutions = new System.Collections.Generic.List<Vector2Int>
        {
            new Vector2Int(360, 669),
            new Vector2Int(720, 1339),
            new Vector2Int(1080, 2008),
            new Vector2Int(1440, 2678),
            new Vector2Int(2160, 4017)
        };

        backButton.onClick.AddListener(() =>
        {
            AudioManager.instance.PlayClickSound();
            Hide(true);
        });

        // Initialize settings based on current values
        InitializeUI();

        // Add listeners programmatically
        qualityDropdown.onValueChanged.AddListener(delegate { SetQualityLevel(qualityDropdown.value); });
        resolutionDropdown.onValueChanged.AddListener(delegate { SetResolution(resolutionDropdown.value); });

        // Apply button listener
        applyButton.onClick.AddListener(ApplySettings);
    }

    void Start()
    {
        Hide(false);
    }

    void InitializeUI()
    {
        // Load saved settings or use defaults if not available
        int savedQualityLevel = PlayerPrefs.GetInt("QualityLevel", QualitySettings.GetQualityLevel());
        bool savedShadowsEnabled = PlayerPrefs.GetInt("ShadowsEnabled", 1) == 1; // Default: Shadows Enabled
        int savedResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", 2); // Default to first custom resolution if not saved

        // Apply saved quality level
        QualitySettings.SetQualityLevel(savedQualityLevel, true);

        // Apply saved shadows setting
        QualitySettings.shadows = savedShadowsEnabled ? ShadowQuality.All : ShadowQuality.Disable;

        // Apply saved resolution setting
        if (savedResolutionIndex >= 0 && savedResolutionIndex < customResolutions.Count)
        {
            var savedResolution = customResolutions[savedResolutionIndex];
            Screen.SetResolution(savedResolution.x, savedResolution.y, Screen.fullScreen);
            Debug.Log($"Loaded and applied saved resolution: {savedResolution.x}x{savedResolution.y}");
        }
        else
        {
            Debug.LogWarning("Saved resolution index is out of range. Defaulting to first resolution.");
            Screen.SetResolution(customResolutions[0].x, customResolutions[0].y, Screen.fullScreen);
        }

        // Update Quality Dropdown with custom options
        qualityDropdown.ClearOptions();
        var qualityOptions = new System.Collections.Generic.List<string> { "LOW", "MEDIUM", "HIGH" };
        qualityDropdown.AddOptions(qualityOptions);
        qualityDropdown.value = savedQualityLevel;

        // Clear and Populate Resolution Dropdown
        resolutionDropdown.ClearOptions();
        var resolutionOptions = new System.Collections.Generic.List<string>();

        foreach (var res in customResolutions)
        {
            resolutionOptions.Add($"{res.x}x{res.y}");
        }

        resolutionDropdown.AddOptions(resolutionOptions);

        // Set Dropdown to Saved Value
        resolutionDropdown.value = savedResolutionIndex;
    }


    public void SetQualityLevel(int index)
    {
        AudioManager.instance.PlayClickSound();
        QualitySettings.SetQualityLevel(index, true);
        Debug.Log($"Quality Level set to: {index}");
    }

    public void SetShadows(bool enabled)
    {
        AudioManager.instance.PlayClickSound();
        QualitySettings.shadows = enabled ? ShadowQuality.All : ShadowQuality.Disable;
        Debug.Log($"Shadows Enabled: {enabled}");
    }

    public void SetResolution(int index)
    {
        AudioManager.instance.PlayClickSound();
        if (index >= 0 && index < customResolutions.Count)
        {
            var resolution = customResolutions[index];
            Screen.SetResolution(resolution.x, resolution.y, Screen.fullScreen);
            Debug.Log($"Resolution set to: {resolution.x}x{resolution.y}");
        }
        else
        {
            Debug.LogError("Invalid resolution index!");
        }
    }

    public void ApplySettings()
    {
        AudioManager.instance.PlayClickSound();
        // Save settings
        PlayerPrefs.SetInt("QualityLevel", qualityDropdown.value);
        PlayerPrefs.SetInt("ResolutionIndex", resolutionDropdown.value);
        PlayerPrefs.Save();

        Debug.Log("Graphics settings applied and saved.");
    }
}
