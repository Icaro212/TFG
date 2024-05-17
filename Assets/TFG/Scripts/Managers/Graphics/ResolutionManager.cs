using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResolutionManager : MonoBehaviour
{

    [SerializeField] private TMP_Dropdown resolutionDropdownMainMenu;
    [SerializeField] private TMP_Dropdown resolutionDropdownPauseMenu;

    private Resolution[] resolutions;
    private List<Resolution> filteredResolution;

    private float currentRefreshRate;
    private int currentResolutionIndex = 0;

    private void Start()
    {
        resolutions = Screen.resolutions;
        filteredResolution = new List<Resolution>();

        resolutionDropdownMainMenu.ClearOptions();
        resolutionDropdownPauseMenu.ClearOptions();
        currentRefreshRate = Screen.currentResolution.refreshRate;

        for(int i=0; i < resolutions.Length; i++)
        {
            if(resolutions[i].refreshRate == currentRefreshRate)
            {
                filteredResolution.Add(resolutions[i]);
            }
        }

        List<string> options = new List<string>();
        for(int i = 0; i < filteredResolution.Count; i++)
        {
            string resolutionOption = filteredResolution[i].width + "x" + filteredResolution[i].height + " " + filteredResolution[i].refreshRate + " Hz";
            options.Add(resolutionOption);
            if(filteredResolution[i].width == Screen.width && filteredResolution[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdownMainMenu.AddOptions(options);
        resolutionDropdownMainMenu.value = currentResolutionIndex;
        resolutionDropdownMainMenu.RefreshShownValue();

        resolutionDropdownPauseMenu.AddOptions(options);
        resolutionDropdownPauseMenu.value = currentResolutionIndex;
        resolutionDropdownPauseMenu.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = filteredResolution[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, true);
    }
}
