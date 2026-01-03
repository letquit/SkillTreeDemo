using System;
using _Scripts.Skill_System;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    private PlayerSkillManager _playerSkillManager;
    public PlayerSkillManager PlayerSkillManager => _playerSkillManager;
    
    private UIDocument _uiDocument;
    public UIDocument UIDocument => _uiDocument;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        _playerSkillManager = FindAnyObjectByType<PlayerSkillManager>();
    }
}
