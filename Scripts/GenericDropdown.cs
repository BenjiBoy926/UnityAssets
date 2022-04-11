using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenericDropdown : MonoBehaviour
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Object to enable/disable for the dropdown")]
    private GameObject dropdown;
    [SerializeField]
    [Tooltip("List of buttons that can enable the dropdown")]
    private List<Button> dropdownEnableButtons;
    [SerializeField]
    [Tooltip("List of buttons that can disable the dropdown")]
    private List<Button> dropdownDisableButtons;
    #endregion

    #region Private Fields
    private UIBlocker currentUIBlocker;
    #endregion

    #region Public Methods
    public void AddDropdownEnableButton(Button button)
    {
        dropdownEnableButtons.Add(button);
        if (enabled) button.onClick.AddListener(EnableDropdown);
    }
    public void AddDropdownDisableButton(Button button)
    {
        dropdownDisableButtons.Add(button);
        if (enabled) button.onClick.AddListener(DisableDropdown);
    }
    public void EnableDropdown()
    {
        // Enable the dropdown
        dropdown.SetActive(true);

        // Make sure the dropdown sorts above all other UI
        Canvas canvas = dropdown.GetOrAddComponent<Canvas>();
        canvas.overrideSorting = true;
        canvas.sortingOrder = 30000;

        // Make sure the dropdown has a graphic raycaster
        dropdown.GetOrAddComponent<GraphicRaycaster>();

        // Create a UI blocker
        currentUIBlocker = UIBlocker.Create(transform);
        currentUIBlocker.OnClick += DisableDropdown;
    }
    public void DisableDropdown()
    {
        // Disable the dropdown
        dropdown.SetActive(false);

        // If there is still an active blocker than destroy it
        if (currentUIBlocker) Destroy(currentUIBlocker.gameObject);
    }
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        dropdown.SetActive(false);
    }
    private void OnEnable()
    {
        foreach (Button button in dropdownEnableButtons)
        {
            button.onClick.AddListener(EnableDropdown);
        }
        foreach (Button button in dropdownDisableButtons)
        {
            button.onClick.AddListener(DisableDropdown);
        }
    }
    private void OnDisable()
    {
        foreach (Button button in dropdownEnableButtons)
        {
            button.onClick.RemoveListener(EnableDropdown);
        }
        foreach (Button button in dropdownDisableButtons)
        {
            button.onClick.RemoveListener(DisableDropdown);
        }
    }
    #endregion
}
