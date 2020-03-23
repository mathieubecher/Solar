using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPS : MonoBehaviour
{
    private Text _text;

    private GameManager _manager;

    private List<float> _fps;
    // Start is called before the first frame update
    void Start()
    {
        _manager = FindObjectOfType<GameManager>();
        _text = GetComponent<Text>();
        _fps = new List<float>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_manager.debug)
        {
            _fps.Add(Time.deltaTime);
            if (_fps.Count > 200) _fps.RemoveAt(0);
            float sum = 0;
            for (int i = 0; i < _fps.Count; ++i)
            {
                sum += _fps[i];
            }

            _text.text = "FPS : " + Mathf.Floor(1 / (sum / _fps.Count));
            _text.enabled = true;
        }
        else _text.enabled = false;
    }
}
