using System;
using _Scripts.Skill_System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

/// <summary>
/// 表示一个天赋按钮的UI组件，处理技能按钮的点击事件和图标显示
/// </summary>
[Serializable]
public class UITalentButton
{
    private Button _button;
    private ScriptableSkill _skill;
    private bool _isUnlocked = false;

    /// <summary>
    /// 当技能按钮被点击时触发的事件
    /// </summary>
    public static UnityAction<ScriptableSkill> OnSkillButtonClicked;

    /// <summary>
    /// 初始化UITalentButton实例
    /// </summary>
    /// <param name="assignedButton">分配给此组件的按钮UI元素</param>
    /// <param name="assignedSkill">与此按钮关联的可脚本化技能对象</param>
    public UITalentButton(Button assignedButton, ScriptableSkill assignedSkill)
    {
        _button = assignedButton;
        _button.clicked += OnClick;
        _skill = assignedSkill;
        // 设置按钮背景图片为技能图标（如果存在）
        if (assignedSkill.skillIcon) _button.style.backgroundImage = new StyleBackground(assignedSkill.skillIcon);
    }

    /// <summary>
    /// 处理按钮点击事件，触发技能按钮点击回调
    /// </summary>
    private void OnClick()
    {
        OnSkillButtonClicked?.Invoke(_skill);
    }
}