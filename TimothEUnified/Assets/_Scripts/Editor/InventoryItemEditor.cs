using GameDevTV.Inventories;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

[CanEditMultipleObjects]
[CustomEditor(typeof(InventoryItem))]
public class InventoryItemEditor : Editor
{

    SerializedObject so;

    //Generic Item
    SerializedProperty propItemName;
    SerializedProperty propItemDescription;
    SerializedProperty propItemType;
    SerializedProperty propItemIcon;
    SerializedProperty propItemIsStackable;
    SerializedProperty propItemValue;

    //Seed Item
    SerializedProperty propSeedSpriteArray;
    SerializedProperty propSeedDaysToGrow;
    SerializedProperty propSeedCorrectSeason;
    SerializedProperty propSeedGrownCropItem;

    //Weapon Item
    SerializedProperty propWeaponDamage;
    SerializedProperty propWeaponAttackRange;
    SerializedProperty propWeaponAttackSpeed;
    SerializedProperty propWeaponHeavyAttackDamageBoost;
    SerializedProperty propWeaponLightAttackSwingRate;
    SerializedProperty propWeaponHeavyAttackSwingRate;
    SerializedProperty propWeaponSwingDistance;
    SerializedProperty propWeaponSprite;
    SerializedProperty propWeaponIsRanged;
    SerializedProperty propWeaponProjectilePrefab;

    //Tool Item
    SerializedProperty propToolType;
    SerializedProperty propToolPower;
    SerializedProperty propToolEnergyConsumption;
    SerializedProperty propToolHorizontalSprite;
    SerializedProperty propToolVerticalSprite;

    //Armor Item
    SerializedProperty propArmorType;
    SerializedProperty propArmorDefence;
    SerializedProperty propArmorSprites;
    SerializedProperty propArmorReplaceSprite;

    //////////////////////////////////////////////////
    private void OnEnable()
    {
        so = serializedObject;

        //Generic Item
        propItemName = so.FindProperty("displayName");
        propItemDescription = so.FindProperty("description");
        propItemType = so.FindProperty("itemType");
        propItemIcon = so.FindProperty("icon");
        propItemIsStackable = so.FindProperty("isStackable");
        propItemValue = so.FindProperty("itemValue");

        //Seed Item
        propSeedSpriteArray = so.FindProperty("growthSpriteArray");
        propSeedDaysToGrow = so.FindProperty("daysToGrow");
        propSeedCorrectSeason = so.FindProperty("correctSeason");
        propSeedGrownCropItem = so.FindProperty("grownCropItem");

        //Weapon Item
        propWeaponDamage = so.FindProperty("damage");
        propWeaponAttackRange = so.FindProperty("attackRange");
        propWeaponAttackSpeed = so.FindProperty("attackSpeed");
        propWeaponHeavyAttackDamageBoost = so.FindProperty("heavyAttackDamageBoost");
        propWeaponLightAttackSwingRate = so.FindProperty("lightAttackSwingRate");
        propWeaponHeavyAttackSwingRate = so.FindProperty("heavyAttackSwingRate");
        propWeaponSwingDistance = so.FindProperty("weaponSwingDistance");
        propWeaponSprite = so.FindProperty("weaponSprite");
        propWeaponIsRanged = so.FindProperty("isRanged");
        propWeaponProjectilePrefab = so.FindProperty("projectileConfig");

        //Tool Item
        propToolType = so.FindProperty("toolType");
        propToolPower = so.FindProperty("toolPower");
        propToolEnergyConsumption = so.FindProperty("energyConsumption");
        propToolHorizontalSprite = so.FindProperty("horizontalToolSprite");
        propToolVerticalSprite = so.FindProperty("verticalToolSprite");

        //Armor Item
        propArmorType = so.FindProperty("armorType");
        propArmorDefence = so.FindProperty("armorDefence");
        propArmorSprites = so.FindProperty("armorSprites");
        propArmorReplaceSprite = so.FindProperty("armorReplaceSprite");
    }

    //////////////////////////////////////////////////
    public override void OnInspectorGUI()
    {
        InventoryItem item = (InventoryItem)target;

        so.Update();

        EditorGUILayout.PropertyField(propItemName);
        EditorGUILayout.PropertyField(propItemDescription);
        EditorGUILayout.PropertyField(propItemType);
        EditorGUILayout.PropertyField(propItemIcon);
        EditorGUILayout.PropertyField(propItemIsStackable);
        EditorGUILayout.PropertyField(propItemValue);

        EditorGUILayout.Separator();

        ItemType itemType = (ItemType)propItemType.enumValueIndex;
        switch (itemType)
        {
            case ItemType.HOLDABLE:
                break;
            case ItemType.WEAPON:
                EditorGUILayout.PropertyField(propWeaponDamage);
                EditorGUILayout.PropertyField(propWeaponAttackRange);
                EditorGUILayout.PropertyField(propWeaponAttackSpeed);
                EditorGUILayout.PropertyField(propWeaponHeavyAttackDamageBoost);
                EditorGUILayout.PropertyField(propWeaponHeavyAttackSwingRate);
                EditorGUILayout.PropertyField(propWeaponLightAttackSwingRate);
                EditorGUILayout.PropertyField(propWeaponSwingDistance);
                EditorGUILayout.PropertyField(propWeaponSprite);
                EditorGUILayout.PropertyField(propWeaponIsRanged);

                bool isRanged = propWeaponIsRanged.boolValue;
                if (isRanged)
                {
                    EditorGUILayout.PropertyField(propWeaponProjectilePrefab);
                }

                break;
            case ItemType.TOOL:
                EditorGUILayout.PropertyField(propToolType);
                EditorGUILayout.PropertyField(propToolPower);
                EditorGUILayout.PropertyField(propToolEnergyConsumption);
                EditorGUILayout.PropertyField(propToolHorizontalSprite);
                EditorGUILayout.PropertyField(propToolVerticalSprite);

                break;
            case ItemType.SEED:
                EditorGUILayout.PropertyField(propSeedSpriteArray);
                EditorGUILayout.PropertyField(propSeedDaysToGrow);
                EditorGUILayout.PropertyField(propSeedCorrectSeason);
                EditorGUILayout.PropertyField(propSeedGrownCropItem);

                if(GUILayout.Button("Autofill Seed Growth Sprites/Icons"))
                {
                    //Find all sprites matching our item name
                    string itemName = propItemName.stringValue;
                    propSeedSpriteArray.ClearArray();

                    string[] spriteAssetsGUIDs = AssetDatabase.FindAssets(itemName + " t:texture2d", null);
                    for (int i = 0; i < spriteAssetsGUIDs.Length; i++)
                    {
                        string assetPath = ExtractResourcePath(AssetDatabase.GUIDToAssetPath(spriteAssetsGUIDs[i]));
                        List<Sprite> sprites = Resources.LoadAll<Sprite>(assetPath).ToList();
                        if (sprites != null && sprites.Count > 0)
                        {
                            sprites.RemoveAt(sprites.Count - 1);
                            item.growthSpriteArray = sprites.ToArray();
                            item.icon = sprites.ToArray()[0];
                        }
                    }
                }

                if(GUILayout.Button("Autofill Crop Item"))
                {
                    string itemName = "Crop_" + propItemName.stringValue;
                    string[] itemNameGUIDs = AssetDatabase.FindAssets(itemName, null);
                    if(itemNameGUIDs != null && itemNameGUIDs.Length > 0)
                    {
                        string assetPath = ExtractResourcePath(AssetDatabase.GUIDToAssetPath(itemNameGUIDs[0]));
                        InventoryItem it = Resources.Load<InventoryItem>(assetPath);
                        if (it != null)
                        {
                            item.grownCropItem = it;
                        }
                        else
                        {
                            Debug.LogWarning("Could not load item");
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Crop Item with name not found: " + itemName);
                    }
                }

                break;
            case ItemType.ARMOR:
                EditorGUILayout.PropertyField(propArmorType);
                EditorGUILayout.PropertyField(propArmorDefence);
                EditorGUILayout.PropertyField(propArmorReplaceSprite);

                SerializedProperty sp = propArmorSprites.FindPropertyRelative("directions");
                sp.arraySize = 4;
                string[] dirStrs = { "Up", "Down", "Left", "Right"};
                
                for(int i = 0; i < 4; ++i)
                {
                    EditorGUILayout.PropertyField(sp.GetArrayElementAtIndex(i), new GUIContent(dirStrs[i]));
                }
                break;
        }

        so.ApplyModifiedProperties();
    }

    /// <summary>
    /// Extracts the resource path from a complete asset file path. E.g. "Assets/Textures/Resources/Sprites/Crops" to "Sprites/Crops"
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    //////////////////////////////////////////////////
    private string ExtractResourcePath(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError("Path was null or empty");
            return null;
        }

        const string resourceStr = "Resources/";
        int index = path.IndexOf(resourceStr);
        if (index != -1)
        {
            path = path.Substring(index + resourceStr.Length);
        }

        index = path.IndexOf('.');
        if (index != -1)
        {
            path = path.Substring(0, index);
        }

        return path;
    }
}
