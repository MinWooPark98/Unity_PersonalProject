using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanel : MonoBehaviour
{
    public GameObject characterButtonGroup;
    public Button characterButton;
    public CharacterDescPanel characterDescPanel;

    void Start()
    {
        var characterTable = DataTableMgr.GetTable<CharacterData>();
        var ids = characterTable.GetAllIds();
        foreach (var id in ids)
        {
            var data = characterTable.Get(id);
            var button = Instantiate(characterButton);
            button.image.sprite = Resources.Load<Sprite>(data.sprite);
            button.onClick.AddListener(() => LobbySceneManager.instance.ActivateCharacterDescPanel());
            button.onClick.AddListener(() => characterDescPanel.Set(id));
            button.onClick.AddListener(() => LobbySceneManager.instance.ShowIndexCharacter((int)data.character));
            button.GetComponentInChildren<TextMeshProUGUI>().text = data.name;
            button.transform.parent = characterButtonGroup.transform;
        }
    }
}
