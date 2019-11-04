using Assets;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "SkillDatabaseData", menuName = "ScriptableObjects/SkillDatabaseData", order = 2)]
public class SkillDatabaseData : SaveableScriptableObject
{
    public override object JsonObjectToSerialize => Skills;

    public List<Skill> Skills = new List<Skill>();

    public void ClearAllData()
    {
        Skills.Clear();
    }
}
