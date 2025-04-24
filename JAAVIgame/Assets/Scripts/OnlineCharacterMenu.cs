using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlineCharacterMenu : MonoBehaviour
{
    [SerializeField] private Button StartButton;
    [SerializeField] private Button KnightButton;
    [SerializeField] private Button WarlockButton;
    [SerializeField] private Button NecromancerButton;
    private CharacterSelectionManager _characterSelectionManager;

    private void Awake()
    {
        _characterSelectionManager = FindObjectOfType<CharacterSelectionManager>();
        if (_characterSelectionManager == null)
        {
            Debug.LogError("Could not find Character Selection Manager");
            return;
        }

        StartButton.onClick.RemoveAllListeners();
        KnightButton.onClick.RemoveAllListeners();
        WarlockButton.onClick.RemoveAllListeners();
        NecromancerButton.onClick.RemoveAllListeners();

        StartButton.onClick.AddListener(() => _characterSelectionManager.StartGame());
        KnightButton.onClick.AddListener(() => _characterSelectionManager.SelectCharacter(KnightButton.gameObject));
        WarlockButton.onClick.AddListener(() => _characterSelectionManager.SelectCharacter(WarlockButton.gameObject));
        NecromancerButton.onClick.AddListener(() => _characterSelectionManager.SelectCharacter(NecromancerButton.gameObject));
    }
}
