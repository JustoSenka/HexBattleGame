using System;

namespace Assets
{
    public interface ISkillSelectionManager
    {
        Skill SelectedSkill { get; }

        event Action<Skill> SkillClicked;
        event Action<Skill> SkillSelectionChanged;
        event Action<Skill, HexCell> SkillPerformed;

        void ClickAndSelectSkill(Skill skill);
        void SelectSkill(Skill skill);
    }
}
