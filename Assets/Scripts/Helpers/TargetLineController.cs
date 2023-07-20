using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLineController : MonoBehaviour
{
    private LineRenderer _line;
    private Transform[] _points;

    private void Awake() {
        _line = gameObject.GetComponent<LineRenderer>();
    }

    public void SetUpLine(Transform[] points) {
        _line.positionCount = points.Length;
        _points = points;
    }

    private void Update() {
        for(int i = 0; i<_points.Length; i++) {
            _line.SetPosition(i, _points[i].position);
        }
    }
}
