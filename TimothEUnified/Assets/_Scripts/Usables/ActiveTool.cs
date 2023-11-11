using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveTool : MonoBehaviour
{
    [SerializeField] ToolConfig _config;

    [SerializeField] SpriteRenderer _toolSpriteRenderer;

    [SerializeField] LayerMask _farmableLand;

    public bool UsingTool { get => _usingTool; }
    bool _usingTool;

    public float EnergyConsumption { get => _config._energyConsumption; }

    Animator _animator;

    InteractionPointManager _interactPoints;

    InteractDirection _interactionDirection;

    Vector2 _mousePosAtClick;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _interactPoints = GetComponentInChildren<InteractionPointManager>();
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
        _interactionDirection = interactDirection;
        _usingTool = true;
        _animator.SetBool("UsingTool", true);
    }

    public void ToolUseFinished()
    {
        _usingTool = false;
        _animator.SetBool("UsingTool", false);

        InteractionPoint ip = _interactPoints.GetInteractionPoint(_interactionDirection);

        //Current tool is a hoe. Checks for closest farmland in trigger
        if (_config._type == ToolType.Hoe)
        {
            //Instead of this, raycast from the mouse position and see if it was on a farm tile within range
            if (Vector2.Distance(_mousePosAtClick, transform.position) < 1.75f)
            {
                RaycastHit2D h = Physics2D.Raycast(_mousePosAtClick, Vector2.zero, 10.0f, _farmableLand);
                if (h.collider != null)
                {
                    FarmableLand fl = h.collider.GetComponent<FarmableLand>();
                    if(fl != null && !fl.IsOccupied)
                    {
                        fl.IsTilled = true;
                    }
                }
            }
        }
        //Current tool is not a hoe. Checks for resource nodes instead of farmland
        else
        {
            GameObject closestResourceNode = GetClosestObjectInInteractionPointWithTag(ip, GameTagManager._resourceNodeTag);
            if (closestResourceNode)
            {
                ResourceNode rn = closestResourceNode.GetComponent<ResourceNode>();
                if (rn)
                {
                    if (rn.CanDestroy(_config))
                    {
                        rn.TakeHit(_config._toolPower);
                    }
                }
            }
        }

        _toolSpriteRenderer.gameObject.SetActive(false);
    }

    private GameObject GetClosestObjectInInteractionPointWithTag(InteractionPoint ip, string tag)
    {
        GameObject closestObj = null;
        float closestDistance = 1000000.0f;

        foreach (GameObject obj in ip.ObjectsInTrigger)
        {
            if (!obj.CompareTag(tag)) continue;

            Vector2 rnPos = Camera.main.WorldToScreenPoint(obj.transform.position);

            float dist = Vector2.Distance(rnPos, _mousePosAtClick);
            if (dist < closestDistance)
            {
                closestObj = obj;
                closestDistance = dist;
            }
        }

        return closestObj;
    }

}
