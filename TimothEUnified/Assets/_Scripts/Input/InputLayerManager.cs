using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputLayerManager : MonoBehaviour
{
    Stack<IInputLayer> _inputLayers;

#if DEBUG
    [SerializeField] bool _enableLogging = false;
#endif

    //////////////////////////////////////////////////
    void Awake()
    {
        _inputLayers = new Stack<IInputLayer>();
    }

    //////////////////////////////////////////////////

    public void UpdateLayers()
    {
        if(_inputLayers.Count > 0)
        {
            _inputLayers.Peek().UpdateLayer();
        }
#if DEBUG
        else if(_enableLogging)
        {
            Debug.LogError("No Input Layers Detected!");
        }
#endif
    }

    //////////////////////////////////////////////////
    public void AddLayer(IInputLayer layer)
    {
        if(!_inputLayers.Contains(layer) )
        {
            layer.Initialize();
            _inputLayers.Push(layer);
#if DEBUG
            if (_enableLogging) Debug.Log("Adding Input Layer: " + layer);
#endif
        }
    }

    //////////////////////////////////////////////////
    public void PopLayer()
    {
#if DEBUG
        if (_enableLogging) Debug.Log("Popping Input Layer: " + _inputLayers.Peek());
#endif
        _inputLayers.Pop();
    }

    //////////////////////////////////////////////////
    public bool ContainsLayer(IInputLayer layer)
    {
        return _inputLayers.Contains(layer);
    }

    //////////////////////////////////////////////////
    public bool IsLayerInFront(IInputLayer layer)
    {
        return _inputLayers.Peek() == layer;
    }
}
