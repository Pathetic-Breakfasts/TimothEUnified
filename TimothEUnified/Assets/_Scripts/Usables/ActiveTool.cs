using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveTool : MonoBehaviour
{
    [SerializeField] ToolConfig _config;

    [SerializeField] SpriteRenderer _toolSpriteRenderer;

    [SerializeField] LayerMask _interactableLayer;

    public bool UsingTool { get => _usingTool; }
    bool _usingTool;

    public float EnergyConsumption { get => _config._energyConsumption; }

    Animator _animator;

    Vector2 _mousePosAtClick;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        ChangeTool(_config);
    }

    public void ChangeTool(ToolConfig newConfig)
    {
        _config = newConfig;
        _toolSpriteRenderer.sprite = _config._horizontalSprite;
        _toolSpriteRenderer.gameObject.SetActive(false);
    }

    public void UseTool(InteractDirection interactDirection, Vector2 mousePos)
    {
        _toolSpriteRenderer.gameObject.SetActive(true);
        _mousePosAtClick = mousePos;
        _usingTool = true;
        _animator.SetBool("UsingTool", true);
    }

    public void ToolUseFinished()
    {
        _usingTool = false;
        _animator.SetBool("UsingTool", false);

        if(Vector2.Distance(_mousePosAtClick, transform.position) < 1.75f)
        {
            RaycastHit2D hit = Physics2D.Raycast(_mousePosAtClick, Vector2.zero, 10.0f, _interactableLayer);
            if (hit.collider != null)
            {
                if(_config._type == ToolType.Hoe)
                {
                    FarmableLand farmableLand = hit.collider.GetComponent<FarmableLand>();
                    if (farmableLand != null && !farmableLand.IsOccupied)
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
                        health?.TakeDamage(_config._toolPower);
                    }
                }
            }
        }

        _toolSpriteRenderer.gameObject.SetActive(false);
    }
}
