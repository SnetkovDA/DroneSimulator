using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSceneController : MonoBehaviour
{
    const Single defDroneSpeed = 20.0f;
    const Single defCellDiameter = 1.0f;
    const Single defMaxWaitTime = 10.7f;

    public Slider slDroneSpeed;
    public Slider slCellDiameter;
    public Slider slMaxWaitTime;

    public Text tDroneSpeed;
    public Text tCellDiameter;
    public Text tMaxWaitTime;

    private void Awake()
    {
        slDroneSpeed.value = PlayerPrefs.GetFloat("DroneSpeed", defDroneSpeed);
        slDroneSpeed.onValueChanged.AddListener(DSValueChange);
        tDroneSpeed.text = Math.Round(slDroneSpeed.value, 2).ToString();

        slCellDiameter.value = PlayerPrefs.GetFloat("NodeRadius", defCellDiameter);
        slCellDiameter.onValueChanged.AddListener(CDValueChange);
        tCellDiameter.text = Math.Round(slCellDiameter.value, 2).ToString();

        slMaxWaitTime.value = PlayerPrefs.GetFloat("MaxWait", 10.7f);
        slMaxWaitTime.onValueChanged.AddListener(MWTValueChange);
        tMaxWaitTime.text = Math.Round(slMaxWaitTime.value, 2).ToString();
    }

    public void DSValueChange(Single value)
    {
        tDroneSpeed.text = Math.Round(value, 2).ToString();
    }

    public void CDValueChange(Single value)
    {
        tCellDiameter.text = Math.Round(value, 2).ToString();
    }

    public void MWTValueChange(Single value)
    {
        tMaxWaitTime.text = Math.Round(value, 2).ToString();
    }

    public void SetToDefault()
    {
        slDroneSpeed.value = defDroneSpeed;
        slCellDiameter.value = defCellDiameter;
        slMaxWaitTime.value = defMaxWaitTime;
    }

    public void StartSimulation()
    {
        PlayerPrefs.SetFloat("DroneSpeed", slDroneSpeed.value);
        PlayerPrefs.SetFloat("NodeRadius", slCellDiameter.value);
        PlayerPrefs.SetFloat("MaxWait", slMaxWaitTime.value);
        SceneManager.LoadScene(1);
    }

}
