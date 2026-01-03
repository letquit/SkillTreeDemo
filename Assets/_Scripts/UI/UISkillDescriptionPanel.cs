using System;
using _Scripts.Skill_System;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// UI组件，用于显示技能描述面板，包括技能图标、名称、描述、成本、前置条件等信息，并提供购买技能的功能
/// </summary>
public class UISkillDescriptionPanel : MonoBehaviour
{
    private UIManager _uiManager;
    private ScriptableSkill _assignedSkill;
    private VisualElement _skillImage;
    private Label _skillNameLabel, _skillDescriptionLabel, _skillCostLabel, _skillPreReqLabel;
    private Button _purchaseSkillButton;

    /// <summary>
    /// 在对象初始化时调用，获取UIManager组件
    /// </summary>
    private void Awake()
    {
        _uiManager = GetComponent<UIManager>();
    }

    /// <summary>
    /// 在组件启用时订阅技能按钮点击事件
    /// </summary>
    private void OnEnable()
    {
        UITalentButton.OnSkillButtonClicked += PopulateLabelText;
    }
    
    /// <summary>
    /// 在组件禁用时取消订阅技能按钮点击事件并移除购买技能按钮的点击事件
    /// </summary>
    private void OnDisable()
    {
        UITalentButton.OnSkillButtonClicked -= PopulateLabelText;
        if (_purchaseSkillButton != null) _purchaseSkillButton.clicked -= PurchaseSkill;
    }

    /// <summary>
    /// 在游戏对象开始运行时获取UI元素引用并初始化显示第一个技能的信息
    /// </summary>
    private void Start()
    {
        GatherLabelReferences();
        var skill = _uiManager.SkillLibrary.GetSkillsOfTier(1)[0];
        PopulateLabelText(skill);
    }

    /// <summary>
    /// 获取UI文档中的各个UI元素引用并绑定购买技能按钮的点击事件
    /// </summary>
    private void GatherLabelReferences()
    {
        _skillImage = _uiManager.UIDocument.rootVisualElement.Q<VisualElement>("Icon");
        _skillNameLabel = _uiManager.UIDocument.rootVisualElement.Q<Label>("SkillNameLabel");
        _skillDescriptionLabel = _uiManager.UIDocument.rootVisualElement.Q<Label>("SkillDescriptionLabel");
        _skillCostLabel = _uiManager.UIDocument.rootVisualElement.Q<Label>("SkillCostLabel");
        _skillPreReqLabel = _uiManager.UIDocument.rootVisualElement.Q<Label>("PreReqLabel");
        _purchaseSkillButton = _uiManager.UIDocument.rootVisualElement.Q<Button>("BuySkillButton");
        _purchaseSkillButton.clicked += PurchaseSkill;
    }

    /// <summary>
    /// 处理购买技能的逻辑，检查玩家是否能够负担技能成本并解锁技能
    /// </summary>
    private void PurchaseSkill()
    {
        if (_uiManager.PlayerSkillManager.CanAffordSkill(_assignedSkill))
        {
            _uiManager.PlayerSkillManager.UnlockSkill(_assignedSkill);
            PopulateLabelText(_assignedSkill);
        }
    }

    /// <summary>
    /// 根据指定的技能对象填充UI面板中的文本和图像内容
    /// </summary>
    /// <param name="skill">要显示的技能对象</param>
    private void PopulateLabelText(ScriptableSkill skill)
    {
        if (skill is null) return;
        _assignedSkill = skill;
        
        if (_assignedSkill.skillIcon)
            _skillImage.style.backgroundImage = new StyleBackground(_assignedSkill.skillIcon);
        _skillNameLabel.text = _assignedSkill.skillName;
        _skillDescriptionLabel.text = _assignedSkill.skillDescription;
        _skillCostLabel.text = $"COST: {skill.cost}";

        // 构建前置技能列表字符串
        if (_assignedSkill.skillPrerequisites.Count > 0)
        {
            _skillPreReqLabel.text = "PREREQUISITES: ";
            foreach (var preReq in _assignedSkill.skillPrerequisites)
            {
                var lastIndex = _assignedSkill.skillPrerequisites.Count - 1;
                string punctuation = preReq == _assignedSkill.skillPrerequisites[lastIndex] ? "" : ",";
                _skillPreReqLabel.text += $" {preReq.skillName}{punctuation}";
            }
        }
        else
        {
            _skillPreReqLabel.text = "";
        }

        // 根据技能状态设置购买按钮的文本和启用状态
        if (_uiManager.PlayerSkillManager.IsSkillUnlocked(_assignedSkill))
        {
            _purchaseSkillButton.text = "PURCHASED";
            _purchaseSkillButton.SetEnabled(false);
        }
        else if (!_uiManager.PlayerSkillManager.PreReqsMet(_assignedSkill))
        {
            _purchaseSkillButton.text = "PREREQUISITES NOT MET";
            _purchaseSkillButton.SetEnabled(false);
        }
        else if (!_uiManager.PlayerSkillManager.CanAffordSkill(_assignedSkill))
        {
            _purchaseSkillButton.text = "CAN'T AFFORD";
            _purchaseSkillButton.SetEnabled(false);
        }
        else
        {
            _purchaseSkillButton.text = "PURCHASE";
            _purchaseSkillButton.SetEnabled(true);
        }
    }
}