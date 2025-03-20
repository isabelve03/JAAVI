using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSelectedCharacter : MonoBehaviour
{
    private void Start()
    {
        GameObject selectedCharacter = CharacterSelectionManager.Instance.SelectedCharacter;
        if (selectedCharacter != null)
        {
            Instantiate(selectedCharacter, Vector3.zero, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("No character selected!");
        }
    }
}
