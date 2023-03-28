using LazySoccer.User.Emblem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardEmblem : MonoBehaviour
{
    [SerializeField] private int _id;
    [SerializeField] private Image _emblem;
    [SerializeField] private Toggle _selectEmblem;

    private ManagerEmblem _managerEmblem;

    private void Awake()
    {
        _managerEmblem = ServiceLocator.GetService<ManagerEmblem>();
    }
    private void Start()
    {
        _selectEmblem.onValueChanged.AddListener(ActiveEmblem);
        _managerEmblem.onActionID += ChangeActive;
    }

    public void Init(int id, Sprite sprite, bool active)
    {
        _id = id;
        _emblem.sprite = sprite;
        _selectEmblem.isOn = active;
    }
    public void ActiveEmblem(bool active)
    {
        if (active)
        {
            _managerEmblem.IdActiveEmblem = _id;
        }
    }
    private void ChangeActive(int id)
    {
        if (id != _id)
            _selectEmblem.isOn = false;
    }
}
