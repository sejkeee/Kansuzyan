using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interface : MonoBehaviour
{
    [SerializeField] private GameController controller;

    [SerializeField] private Image healthBar;
    [SerializeField] private Text healthText;
    [SerializeField] private Text gold;
    [SerializeField] private Text wave;


    [SerializeField] private Image soundOn;
    [SerializeField] private Image soundOff;
    [SerializeField] private Button soundButton;
    [SerializeField] private AudioSource[] audioSource = new AudioSource[2];

    private bool isSoundOn = true;



    private void Update()
    {
        healthBar.fillAmount = controller.nowEnemyHealth / controller.nowEnemyHealthCalculated;
        healthText.text = $"{controller.nowEnemyHealth} / {controller.nowEnemyHealthCalculated}";
        gold.text = controller.gold.ToString();
        wave.text = controller.wave.ToString();
    }

    public void OnSoundClick()
    {
        if(isSoundOn)
        {
            soundOff.gameObject.SetActive(true);
            soundOn.gameObject.SetActive(false);
            audioSource[0].mute = true;
            audioSource[1].mute = true;
        }
        else
        {
            soundOff.gameObject.SetActive(false);
            soundOn.gameObject.SetActive(true);
            audioSource[0].mute = false;
            audioSource[1].mute = false;
        }
        isSoundOn = !isSoundOn;
    }
}
