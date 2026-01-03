using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Scripts.Skill_System
{
    /// <summary>
    /// 技能库脚本对象，用于管理和存储技能数据
    /// </summary>
    [CreateAssetMenu(fileName = "New Skill Library", menuName = "Skill System/New Skill Library", order = 0)]
    public class ScriptableSkillLibrary : ScriptableObject
    {
        /// <summary>
        /// 技能库列表，存储所有可用的技能
        /// </summary>
        public List<ScriptableSkill> skillLibrary;

        /// <summary>
        /// 根据技能等级获取对应的所有技能
        /// </summary>
        /// <param name="tier">技能等级</param>
        /// <returns>指定等级的所有技能列表</returns>
        public List<ScriptableSkill> GetSkillsOfTier(int tier)
        {
            return skillLibrary.Where(skill => skill.skillTier == tier).ToList();
        }
    }
}