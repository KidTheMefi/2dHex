using System;
using System.Collections.Generic;
using DiceCubePrototype;
using UnityEngine;

public class DiceCubeFaceViewer : MonoBehaviour
{
    [SerializeField]
    private CubeFaceOnViewer _hexPrefab;

    private List<CubeFaceOnViewer> _cubeFacesView;

    private void Start()
    {
        CubeDiceSignal.GetInstance().MouseEnterDice += OnMouseEnterDice;
        CubeDiceSignal.GetInstance().MouseExitDice += OnMouseExitDice;
        _cubeFacesView = new List<CubeFaceOnViewer>();
        for (int i = 0; i < 6; i++)
        {
            var hex = Instantiate(_hexPrefab, transform);
            hex.transform.localPosition = HexPosition(Vector3.zero, 1.4f, i);
            _cubeFacesView.Add(hex);
        }

        _hexPrefab.gameObject.SetActive(false);
    }
    private void OnMouseExitDice()
    {
        gameObject.SetActive(false);
    }
    private void OnMouseEnterDice(DiceCube diceCube)
    {
        gameObject.SetActive(true);
        gameObject.transform.position = diceCube.transform.position + new Vector3(1, 1f, -1) * 5 /*+ Vector3.down*0.05f*/;
        
        //SetFacesInOrder(diceCube);
        //NonOrderHighlight(diceCube);
    }
    /*
    private void SetFacesInOrder(DiceCube diceCube)
    {
        int index = diceCube.CubeDiceFaces.IndexOf(diceCube.FaceOnTop);
        for (int i = 0; i < diceCube.CubeDiceFaces.Count; i++)
        {
            if (_cubeFacesView.Count > i)
            {
                _cubeFacesView[i].SetSprite(diceCube.CubeDiceFaces[index].GetFaceSprite());
            }
            index++;
            index = index < diceCube.CubeDiceFaces.Count ? index : 0;
        }
        _cubeFacesView[0].ChangeColorSprite(Color.white);
    }

    private void NonOrderHighlight(DiceCube diceCube)
    {
        int index = diceCube.CubeDiceFaces.IndexOf(diceCube.FaceOnTop);
        for (int i = 0; i < diceCube.CubeDiceFaces.Count; i++)
        {
            if (_cubeFacesView.Count > i)
            {
                _cubeFacesView[i].SetSprite(diceCube.CubeDiceFaces[i].GetFaceSprite());

                if (index == i )
                {
                    _cubeFacesView[i].ChangeColorSprite(Color.white);
                }
                else
                {
                    _cubeFacesView[i].ReturnDefaultColor();
                }
            }
        }
        
    }*/

    private void OnDestroy()
    {
        CubeDiceSignal.GetInstance().MouseEnterDice -= OnMouseEnterDice;
        CubeDiceSignal.GetInstance().MouseExitDice -= OnMouseExitDice;
    }

    private Vector3 HexPosition(Vector3 startPosition, float hexDistance, int i)
    {
        float angleDeg = 60 * (i + 2); // 30;
        float angleRadian = Mathf.Deg2Rad * angleDeg;
        return new Vector3(startPosition.x + hexDistance * Mathf.Cos(angleRadian), 0, startPosition.y + hexDistance * Mathf.Sin(angleRadian));
    }
}