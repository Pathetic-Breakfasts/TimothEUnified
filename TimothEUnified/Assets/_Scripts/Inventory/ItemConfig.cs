using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName ="Assets/Inventory/New Item Config")]
public class ItemConfig : ScriptableObject
{
    public string itemName = "Unnamed";
    public string description = "Empty Description";
    public Sprite icon;
    public bool isStackable = true;
    public int maxInStack = 999;
    public int baseValue = 0;
    public GameObject itemPrefab = null;
    public string UUID = ""; //TODO: Add GUIDs for saving purposes
}
