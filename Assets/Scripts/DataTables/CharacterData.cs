using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : ICSVParsing
{
    public string id { get; set; }
    public Characters character { get; private set; }
    public string name { get; private set; }
    public string desc { get; private set; }
    public string sprite { get; private set; }
    public string difficulty { get; private set; }
    public string basicAttackName { get; private set; }
    public string basicAttackDesc { get; private set; }
    public string skillAttackName { get; private set; }
    public string skillAttackDesc { get; private set; }

    public void Parse(Dictionary<string, string> line)
    {
        id = line["ID"];
        character = (Characters)int.Parse(id);
        name = line["Name"];
        sprite = line["Sprite"];
        difficulty = line["Difficulty"];
        desc = line["Desc"];
        basicAttackName = line["BasicAttackName"];
        basicAttackDesc = line["BasicAttackDesc"];
        skillAttackName = line["SkillAttackName"];
        skillAttackDesc = line["SkillAttackDesc"];
    }
}
