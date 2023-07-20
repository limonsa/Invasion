using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    [SerializeField] private float countdownValue = 5;
    [SerializeField] private GameObject timerFilling;
    [SerializeField] private GameObject timerBackground;
    [SerializeField] private Transform playerTransform;

    private float currCountdownValue;
    private Vector3 fullFilling;
    private Player player;

    //Action to manage order to power up gun speed
    public static UnityAction PowerGunDown;

    public IEnumerator StartCountdown()
    {
        float step = (float)(timerFilling.transform.localScale.y / countdownValue);
        currCountdownValue = countdownValue;

        //Powers up the gun to shoot bullets at a high rate
        player.PowerUpGun();
        while (currCountdownValue > 0)
        {
            //Debug.LogWarning($"TIMER MANAGER SAYS >>> Countdown: {currCountdownValue} will reduce {timerFilling.transform.localScale} by {step}" );
            yield return new WaitForSeconds(1.0f);
            currCountdownValue--;
            timerFilling.transform.localScale += (Vector3.down * step);
        }
        //Powers down the gun to shoot bullets at a regular rate
        player.PowerDownGun();
        PowerGunDown?.Invoke();
        timerFilling.SetActive(false);
        timerBackground.SetActive(false);
    }


    // Start is called before the first frame update
    void SpawnTimer()
    {
        //Debug.LogWarning("TIMER MANAGER SAYS >>> SpawTimer() called");
        timerFilling.SetActive(true);
        timerBackground.SetActive(true);
        ResetTimer();
        StartCoroutine(StartCountdown());

    }

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        //Debug.LogWarning("TIMER MANAGER.Start SAYS >>> Inititializing timer ");
        gameObject.transform.position = playerTransform.position + Vector3.left;
        fullFilling = timerFilling.transform.localScale;
        PowerUp.PowerUpGun += SpawnTimer;
        timerFilling.SetActive(false);
        timerBackground.SetActive(false);
    }

    public void ResetTimer()
    {
        timerFilling.transform.localScale = fullFilling;
    }
}
