using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;

public class WaveUI : MonoBehaviour
{
    [SerializeField] Transform _startWavePannel;
    [SerializeField] TextMeshProUGUI _startWaveText;
    [SerializeField] Transform _endWavePannel;

    public async void ShowStartWave(int wave)
    {
        _startWaveText.text = $"Wave {wave}";
        _startWavePannel.gameObject.SetActive(true);
        await Task.Delay(5000);
        _startWavePannel.gameObject.SetActive(false);
    }

    public async void ShowEndWave()
    {
        _endWavePannel.gameObject.SetActive(true);
        await Task.Delay(5000);
        _endWavePannel.gameObject.SetActive(false);
    }
}
