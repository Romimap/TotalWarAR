using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelViewer : MonoBehaviour {
    public static List<PanelViewer> s_panelViewers = new List<PanelViewer>();

    Vector2 _defaultPosition;

    Vector2 _from;
    Vector2 _to;
    float _time;
    RectTransform _rectTransform;

    public Vector2 open;
    public Vector2 close;

    float EaseOutQuart (float x) {
        return 1 - Mathf.Pow(1 - x, 4);
    }

    void Start() {
        s_panelViewers.Add(this);
        _rectTransform = GetComponent<RectTransform>();
        _defaultPosition = _rectTransform.position;
        _from = _defaultPosition;
        _to = _defaultPosition; 
    }

    void Update() {
        _time += Time.deltaTime * 2;
        _time = Mathf.Clamp01(_time);

        float t = EaseOutQuart(_time);
        _rectTransform.position = _to * t + _from * (1 - t);
    }

    public void Show () {
        HideAll();
        
        _from = _rectTransform.position;
        _to = _defaultPosition + open;
        _time = 0;
    }
    public void ResetAll () {
        foreach (PanelViewer p in s_panelViewers) {
            p.Reset();
        }
    }

    public static void OpenAnimationAll () {
        foreach (PanelViewer p in s_panelViewers) {
            p.OpenAnimation();
        }
    }

    public void OpenAnimation () {
        Hide();
        _time = 1;
        _rectTransform.position = _to;
        Reset();
    }

    private void Reset () {
        _from = _rectTransform.position;
        _to = _defaultPosition;
        _time = 0;
    }

    public static void HideAll () {
        foreach (PanelViewer p in s_panelViewers) {
            p.Hide();
        }
    }

    private void Hide () {
        _from = _rectTransform.position;
        _to = _defaultPosition + close;
        _time = 0;
    }
}
