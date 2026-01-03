using System;
using System.Collections.Generic;
using _Scripts.Skill_System;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// UI管理器，负责管理游戏中的UI文档和玩家技能管理器的引用
/// </summary>
public class UIManager : MonoBehaviour
{
    [SerializeField] private ScriptableSkillLibrary skillLibrary;
    public ScriptableSkillLibrary SkillLibrary => skillLibrary;
    [SerializeField] private VisualTreeAsset uiTalentButton;
    
    /// <summary>
    /// 玩家技能管理器实例
    /// </summary>
    private PlayerSkillManager _playerSkillManager;
    
    /// <summary>
    /// 获取玩家技能管理器实例的只读属性
    /// </summary>
    public PlayerSkillManager PlayerSkillManager => _playerSkillManager;
    
    /// <summary>
    /// UI文档组件实例
    /// </summary>
    private UIDocument _uiDocument;
    
    /// <summary>
    /// 获取UI文档组件实例的只读属性
    /// </summary>
    public UIDocument UIDocument => _uiDocument;

    private VisualElement _skillTopRow, _skillMiddleRow, _skillBottomRow;
    [SerializeField] private List<UITalentButton> _talentButtons;

    /// <summary>
    /// 初始化UI管理器，在Awake阶段获取必要的组件引用
    /// </summary>
    private void Awake()
    {
        // 获取当前游戏对象上的UIDocument组件
        _uiDocument = GetComponent<UIDocument>();
        // 查找场景中的PlayerSkillManager实例
        _playerSkillManager = FindAnyObjectByType<PlayerSkillManager>();
    }

    private void Start()
    {
        CreateSkillButtons();
    }

    /// <summary>
    /// 创建技能按钮，根据技能库中的技能等级在对应的UI行中生成按钮
    /// </summary>
    private void CreateSkillButtons()
    {
        var root = _uiDocument.rootVisualElement;
        _skillTopRow = root.Q<VisualElement>("Skill_RowOne");
        _skillMiddleRow = root.Q<VisualElement>("Skill_RowTwo");
        _skillBottomRow = root.Q<VisualElement>("Skill_RowThree");
        
        SpawnButtons(_skillTopRow, skillLibrary.GetSkillsOfTier(1));
        SpawnButtons(_skillMiddleRow, skillLibrary.GetSkillsOfTier(2));
        SpawnButtons(_skillBottomRow, skillLibrary.GetSkillsOfTier(3));
    }

    /// <summary>
    /// 在指定的父元素中生成技能按钮
    /// </summary>
    /// <param name="parent">父UI元素，按钮将被添加到此元素中</param>
    /// <param name="skills">要生成按钮的技能列表</param>
    private void SpawnButtons(VisualElement parent, List<ScriptableSkill> skills)
    {
        foreach (var skill in skills)
        {
            Button clonedButton = uiTalentButton.CloneTree().Q<Button>();
            _talentButtons.Add(new UITalentButton(clonedButton, skill));
            parent.Add(clonedButton);
        }
    }
}