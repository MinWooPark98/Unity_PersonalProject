using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterDescPanel : MonoBehaviour
{
    private Characters character;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI characterDesc;
    public TextMeshProUGUI maxHp;
    public TextMeshProUGUI difficulty;
    public TextMeshProUGUI basicAttack;
    public TextMeshProUGUI skillAttack;
    public AttackInfoPanel attackInfoPanel;

    public void Set(string id)
    {
        var data = DataTableMgr.GetTable<CharacterData>().Get(id);
        character = data.character;
        characterName.text = data.name;
        characterDesc.text = data.desc;
        maxHp.text = DataTableMgr.GetTable<HealthData>().Get(id).maxHp.ToString();
        difficulty.text = data.difficulty;
        basicAttack.text = data.basicAttackName;
        skillAttack.text = data.skillAttackName;
        attackInfoPanel.Set(id);
    }

    public void SelectCharacter()
    {
        LobbySceneManager.instance.SetCharacter(character);
        LobbySceneManager.instance.Home();
    }
}
