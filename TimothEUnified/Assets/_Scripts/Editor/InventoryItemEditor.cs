using GameDevTV.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

[CustomEditor(typeof(InventoryItem))]
public class InventoryItemEditor : Editor
{

    SerializedObject so;

    //Generic Item
    SerializedProperty propItemName;
    SerializedProperty propItemDescription;
    SerializedProperty propItemType;
    SerializedProperty propItemIcon;
    SerializedProperty propItemPickup;
    SerializedProperty propItemIsStackable;

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

    //Holdable Item

    private void OnEnable()
    {
        so = serializedObject;

        //Generic Item
        propItemName = so.FindProperty("displayName");
        propItemDescription = so.FindProperty("description");
        propItemType = so.FindProperty("itemType");
        propItemIcon = so.FindProperty("icon");
        propItemPickup = so.FindProperty("pickup");
        propItemIsStackable = so.FindProperty("isStackable");


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
        propWeaponProjectilePrefab = so.FindProperty("projectilePrefab");

        //Tool Item
        propToolType = so.FindProperty("toolType");
        propToolPower = so.FindProperty("toolPower");
        propToolEnergyConsumption = so.FindProperty("energyConsumption");
        propToolHorizontalSprite = so.FindProperty("horizontalToolSprite");
        propToolVerticalSprite = so.FindProperty("verticalToolSprite");
    }

    private void OnDisable()
    {
        
    }


    public override void OnInspectorGUI()
    {
        so.Update();

        EditorGUILayout.PropertyField(propItemName);
        EditorGUILayout.PropertyField(propItemDescription);
        EditorGUILayout.PropertyField(propItemType);
        EditorGUILayout.PropertyField(propItemIcon);
        EditorGUILayout.PropertyField(propItemPickup);
        EditorGUILayout.PropertyField(propItemIsStackable);

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

                break;
            case ItemType.ARMOR:
                break;
        }

        so.ApplyModifiedProperties();
    }

}
