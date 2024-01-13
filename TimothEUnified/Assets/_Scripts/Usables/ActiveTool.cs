using GameDevTV.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveTool : MonoBehaviour
{
    [SerializeField] SpriteRenderer _toolSpriteRenderer;

    [SerializeField] LayerMask _interactableLayer;

    public bool UsingTool { get => _usingTool; }
    bool _usingTool;

    public bool HasTool { get => _hasTool; }
    bool _hasTool = false;

    public float EnergyConsumption { get => _config.energyConsumption; }

    Animator _animator;
    InventoryItem _config;

    Vector2 _targetPositon;

    //////////////////////////////////////////////////
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        ChangeTool(_config);
    }

    //////////////////////////////////////////////////
    public void ChangeTool(InventoryItem newConfig)
    {
        _config = newConfig;

        if(!_config)
        {
            _hasTool = false;
            _usingTool = false;
            return;
        }

        _hasTool = true;
        _toolSpriteRenderer.sprite = _config.horizontalToolSprite;
        _toolSpriteRenderer.enabled = false;
    }


    //////////////////////////////////////////////////
    public void UseTool(Vector2 targetPos)
    {
        if (!_hasTool)
        {
            return;
        }

        _toolSpriteRenderer.enabled = true;
        _targetPositon = targetPos;
        _usingTool = true;
        _animator.SetBool("UsingTool", true);
    }

    //Called through Unity Animation Events
    //////////////////////////////////////////////////
    public void ToolUseFinished()
    {
        _usingTool = false;
        _animator.SetBool("UsingTool", false);

        if(Vector2.Distance(_targetPositon, transform.position) < 1.75f)
        {
            RaycastHit2D hit = Physics2D.Raycast(_targetPositon, Vector2.zero, 10.0f, _interactableLayer);
            if (hit.collider)
            {
                if(_config.toolType == ToolType.Hoe)
                {
                    FarmableLand farmableLand = hit.collider.GetComponent<FarmableLand>();
                    if (farmableLand && !farmableLand.IsOccupied)
                    {
                        farmableLand.IsTilled = true;
                    }
                }
                else
                {
                    ResourceNode resourceNode = hit.collider.GetComponent<ResourceNode>();
                    if (resourceNode && resourceNode.CanDestroy(_config))
                    {
                        Health health = resourceNode.GetComponent<Health>();
                        health?.TakeDamage(_config.toolPower);
                    }
                }
            }
        }

        _toolSpriteRenderer.enabled = false;
    }
}
