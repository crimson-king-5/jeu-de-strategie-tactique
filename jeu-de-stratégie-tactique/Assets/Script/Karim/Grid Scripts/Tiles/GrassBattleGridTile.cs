using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassBattleGridTile : BattleGridTile
{
    [SerializeField] private Color _baseColor, _offsetColor;

    //public void Init(int x, int y)
    //{
    //    var isOffset = (x + y) % 2 == 1; 
    //    _renderer.color = isOffset ? _offsetColor : _baseColor;
    //}
}
