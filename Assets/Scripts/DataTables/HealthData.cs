using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthData : ICSVParsing
{
    public string id { get; set; }
    public string name { get; private set; }
    public int maxHp { get; private set; }
    public int growthHp { get; private set; }

    public void Parse(Dictionary<string, string> line)
    {
        id = line["ID"];
        name = line["Name"];    // 빼도 될 거 같긴 함 (csv파일에서 보고 구분하기 위함)
        maxHp = Convert.ToInt32(line["MaxHp"]);
        if (!string.IsNullOrEmpty(line["GrowthHp"]))
            growthHp = Convert.ToInt32(line["GrowthHp"]);
    }
}
