using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ToolType
{
    Hoe,
    Pickaxe,
    Axe
}

[CreateAssetMenu(menuName ="Configs/Tool Config")]
public class ToolConfig : ScriptableObject
{
    public ToolType _type;
    public float _toolPower = 50.0f;

    public Sprite _horizontalSprite;
    public Sprite _verticalSprite;

}
