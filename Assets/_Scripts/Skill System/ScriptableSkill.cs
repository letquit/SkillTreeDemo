using System;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace _Scripts.Skill_System
{
    /// <summary>
    /// 可脚本化技能类，用于定义游戏中的技能数据
    /// </summary>
    [CreateAssetMenu(fileName = "New Skill", menuName = "Skill System/New Skill", order = 0)]
    public class ScriptableSkill : ScriptableObject
    {
        /// <summary>
        /// 技能升级数据列表
        /// </summary>
        public List<UpgradeData> UpgradeData = new List<UpgradeData>();
        
        /// <summary>
        /// 标识技能是否为能力型技能
        /// </summary>
        public bool isAbility;
        
        /// <summary>
        /// 技能名称
        /// </summary>
        public string skillName;
        
        /// <summary>
        /// 标识是否覆盖自动生成的描述
        /// </summary>
        public bool overwriteDescription;
        
        /// <summary>
        /// 技能描述文本区域
        /// </summary>
        [TextArea(1, 4)] public string skillDescription;
        
        /// <summary>
        /// 技能图标
        /// </summary>
        public Sprite skillIcon;
        
        /// <summary>
        /// 技能前置条件列表
        /// </summary>
        public List<ScriptableSkill> skillPrerequisites = new List<ScriptableSkill>();
        
        /// <summary>
        /// 技能等级
        /// </summary>
        public int skillTier;
        
        /// <summary>
        /// 技能消耗
        /// </summary>
        public int cost;

        /// <summary>
        /// 验证函数，在编辑器中修改属性时自动调用
        /// </summary>
        private void OnValidate()
        {
            skillName = name;
            if (UpgradeData.Count == 0) return;
            if (overwriteDescription) return;

            GenerateDescription();
        }

        /// <summary>
        /// 生成技能描述文本
        /// </summary>
        private void GenerateDescription()
        {
            // 为能力型技能生成描述
            if (isAbility)
            {
                switch (UpgradeData[0].StatType)
                {
                    case StatTypes.DoubleJump:
                        skillDescription = $"{skillName} grants the Double Jump ability.";
                        break;
                    case StatTypes.Dash:
                        skillDescription = $"{skillName} grants the Dash ability.";
                        break;
                    case StatTypes.Teleport:
                        skillDescription = $"{skillName} grants the Teleport ability.";
                        break;
                }
            }
            // 为属性提升型技能生成描述
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"{skillName} increases ");
                for (int i = 0; i < UpgradeData.Count; i++)
                { 
                    sb.Append(UpgradeData[i].StatType.ToString());
                    sb.Append(" by ");
                    sb.Append(UpgradeData[i].SkillIncreaseAmount.ToString());
                    sb.Append(UpgradeData[i].IsPercentage ? "%" : " points(s)");
                    if (i < UpgradeData.Count - 2) sb.Append(" and ");
                    else sb.Append(i < UpgradeData.Count - 1 ? ", " : ".");
                }
                
                skillDescription = sb.ToString();
            }
        }
    }

    /// <summary>
    /// 技能升级数据类，用于存储技能升级的相关信息
    /// </summary>
    [Serializable]
    public class UpgradeData
    {
        /// <summary>
        /// 属性类型
        /// </summary>
        public StatTypes StatType;
        
        /// <summary>
        /// 技能提升数值
        /// </summary>
        public int SkillIncreaseAmount;
        
        /// <summary>
        /// 标识提升数值是否为百分比
        /// </summary>
        public bool IsPercentage;
    }
    
    /// <summary>
    /// 属性类型枚举，定义游戏中可用的属性类型
    /// </summary>
    public enum StatTypes
    {
        Strength,
        Dexterity,
        Intelligence,
        Wisdom,
        Charisma,
        Constitution,
        DoubleJump,
        Dash,
        Teleport
    }
}