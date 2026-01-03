using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.Skill_System
{
    /// <summary>
    /// 玩家技能管理器，负责管理玩家的技能点数、属性值和技能解锁系统
    /// </summary>
    public class PlayerSkillManager : MonoBehaviour
    {
        private int _strength, _dexterity, _intelligence, _wisdom, _charisma, _constitution;
        private int _doubleJump, _dash, _teleport;
        private int _skillPoints;

        public int Strength => _strength;
        public int Dexterity => _dexterity;
        public int Intelligence => _intelligence;
        public int Wisdom => _wisdom;
        public int Charisma => _charisma;
        public int Constitution => _constitution;

        public bool DoubleJump => _doubleJump > 0;
        public bool Dash => _dash > 0;
        public bool Teleport => _teleport > 0;

        public int SkillPoints => _skillPoints;
    
        public UnityAction OnSkillPointsChanged;
        
        private List<ScriptableSkill> _unlockedSkills = new List<ScriptableSkill>();

        /// <summary>
        /// 初始化玩家技能管理器，设置初始属性值和技能点数
        /// </summary>
        private void Awake()
        {
            _strength = 10;
            _dexterity = 10;
            _intelligence = 10;
            _wisdom = 10;
            _charisma = 10;
            _constitution = 10;
            _skillPoints = 20;
        }

        /// <summary>
        /// 增加一个技能点数并触发技能点数变化事件
        /// </summary>
        public void GainSkillPoint()
        {
            _skillPoints++;
            OnSkillPointsChanged?.Invoke();
        }

        /// <summary>
        /// 检查当前技能点数是否足够购买指定技能
        /// </summary>
        /// <param name="skill">要检查的技能</param>
        /// <returns>如果技能点数足够则返回true，否则返回false</returns>
        public bool CanAffordSkill(ScriptableSkill skill)
        {
            return _skillPoints >= skill.cost;
        }
        
        /// <summary>
        /// 解锁指定技能，如果技能点数不足则不执行任何操作
        /// </summary>
        /// <param name="skill">要解锁的技能</param>
        public void UnlockSkill(ScriptableSkill skill)
        {
            if (!CanAffordSkill(skill)) return;
            ModifyStats(skill);
            _unlockedSkills.Add(skill);
            _skillPoints -= skill.cost;
            OnSkillPointsChanged?.Invoke();
        }

        /// <summary>
        /// 根据技能的升级数据修改对应的属性值
        /// </summary>
        /// <param name="skill">包含升级数据的技能</param>
        private void ModifyStats(ScriptableSkill skill)
        {
            foreach (UpgradeData data in skill.UpgradeData)
            {
                switch (data.StatType)
                {
                    case StatTypes.Strength:
                        ModifyStat(ref _strength, data);
                        break;
                    case StatTypes.Dexterity:
                        ModifyStat(ref _dexterity, data);
                        break;
                    case StatTypes.Intelligence:
                        ModifyStat(ref _intelligence, data);
                        break;
                    case StatTypes.Wisdom:
                        ModifyStat(ref _wisdom, data);
                        break;
                    case StatTypes.Charisma:
                        ModifyStat(ref _charisma, data);
                        break;
                    case StatTypes.Constitution:
                        ModifyStat(ref _constitution, data);
                        break;
                    case StatTypes.DoubleJump:
                        ModifyStat(ref _doubleJump, data);
                        break;
                    case StatTypes.Dash:
                        ModifyStat(ref _dash, data);
                        break;
                    case StatTypes.Teleport:
                        ModifyStat(ref _teleport, data);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// 检查指定技能是否已经被解锁
        /// </summary>
        /// <param name="skill">要检查的技能</param>
        /// <returns>如果技能已解锁则返回true，否则返回false</returns>
        public bool IsSkillUnlocked(ScriptableSkill skill)
        {
            return _unlockedSkills.Contains(skill);
        }

        /// <summary>
        /// 检查指定技能的前置条件是否都已满足
        /// </summary>
        /// <param name="skill">要检查前置条件的技能</param>
        /// <returns>如果前置条件都已满足或没有前置条件则返回true，否则返回false</returns>
        public bool PreReqsMet(ScriptableSkill skill)
        {
            return skill.skillPrerequisites.Count == 0 || skill.skillPrerequisites.All(_unlockedSkills.Contains);
        }
        
        /// <summary>
        /// 根据升级数据修改指定属性值，支持百分比和固定数值两种修改方式
        /// </summary>
        /// <param name="stat">要修改的属性引用</param>
        /// <param name="data">包含修改方式和数值的升级数据</param>
        private void ModifyStat(ref int stat, UpgradeData data)
        {
            if (data.IsPercentage) stat += (int)(stat * (data.SkillIncreaseAmount / 100f));
            else stat += data.SkillIncreaseAmount;
        }
    }
}