using UnityEngine;

public class TimerController : MonoBehaviour
{
    private bool _start, _signal;

    private float _timer;
    
    void Update()
    {
        if (_start && !_signal)
        {
            _timer -= Time.deltaTime;

            if (_timer <= 0)
            {
                _signal = true;
            }
        }
    }

    public void StartTimer(float time)
    {
        if (_start != true)
        {
            _timer = time;
            _start = true;
        }
    }

    public bool ReturnTimerSignal()
    {
        return _signal;
    }

    public float ReturnTimerValue()
    {
        return _timer;
    }

    public void RestartTimer()
    {
        _start = false;
        _signal = false;
    }
}
