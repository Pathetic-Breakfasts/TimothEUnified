using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveTool : MonoBehaviour
{
    [SerializeField] ToolConfig _config;

    [SerializeField] SpriteRenderer _toolSpriteRenderer;

    public bool UsingTool { get => _usingTool; }
    bool _usingTool;

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

        GameObject closestObj = null;
        float closestDistance = 10000.0f;

        InteractionPoint ip = _interactPoints.GetInteractionPoint(_interactionDirection);
        foreach (GameObject obj in ip.ObjectsInTrigger)
        {
            if (!obj.CompareTag("ResourceNode") && !obj.CompareTag("Farmland")) continue;

            Vector2 rnPos = Camera.main.WorldToScreenPoint(obj.transform.position);

            float dist = Vector2.Distance(rnPos, _mousePosAtClick);
            if (dist < closestDistance)
            {
                closestObj = obj;
                closestDistance = dist;
            }
        }

        if (closestObj != null)
        {
            ResourceNode rn = closestObj.GetComponent<ResourceNode>();

            if (rn)
            {
                if (rn.CanDestroy(_config))
                {
                    rn.TakeHit(_config._toolPower);
                }
            }
            else
            {
                if(_config._type == ToolType.Hoe)
                {
                    FarmableLand fl = closestObj.GetComponent<FarmableLand>();

                    if (fl)
                    {
                        if(!fl.IsOccupied)
                        {
                            fl.IsTilled = true;
                        }
                    }
                }
            }
        }
        _toolSpriteRenderer.gameObject.SetActive(false);
    }

}
