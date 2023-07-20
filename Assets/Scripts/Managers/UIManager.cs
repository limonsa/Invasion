using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{

    [SerializeField] Player player;
    [SerializeField] private TMP_Text txtHealth;
    [SerializeField] private TMP_Text txtScore;
    [SerializeField] private TMP_Text txtHighScore;
    [SerializeField] private TMP_Text txtNukes;
    [SerializeField] private Sprite nukeSprite;

    private GameObject nukesContainer;
    private Stack<SpriteRenderer> stkNukes;

    private void Start()
    {
        player.health.OnHealthUpdate += UpdateHealth; //subscribe to action
        stkNukes = new Stack<SpriteRenderer>();
        nukesContainer = GameObject.Find("NukesContainer");
    }

    /// <summary>
    /// unsubscribe from action of updating health
    /// </summary>
    private void OnDisable()
    {
        player.health.OnHealthUpdate -= UpdateHealth; // unsubscribe from action
    }

    /// <summary>
    /// Shows in the UI the current health
    /// </summary>
    /// <param name="currentHealth"></param>
    public void UpdateHealth(float currentHealth)
    {
        txtHealth.SetText(currentHealth.ToString());
    }

    /// <summary>
    /// Shows in the UI the current score
    /// </summary>
    public void UpdateScore()
    {
        //Update Score
        txtScore.SetText(GameManager.GetInstance().scoreManager.GetScore().ToString());
    }

    /// <summary>
    /// Shows in the UI the high score
    /// </summary>
    public void UpdateHighScore()
    {
        //Update HighScore
        txtHighScore.SetText(GameManager.GetInstance().scoreManager.GetHighScore().ToString());
    }

    /// <summary>
    /// When colliding with the player a nuke is picked up
    /// and the UI is updated showing another nuke ready to be used
    /// </summary>
    /// <param name="nuke"></param>
    public void LoadNuke(SpriteRenderer nuke)
    {
        int i = stkNukes.Count + 1;
        Vector2 pos = nukesContainer.gameObject.transform.position;
        pos.x += i * 2f;
        Debug.Log($"UI MANAGER SAYS >>> LoadNuke will spawn the SpriteRendered = {nuke.ToString()} in {pos.ToString()}");
        stkNukes.Push(Instantiate(nuke, pos, Quaternion.identity));
        txtNukes.text = CountAvailableNukes().ToString();
    }

    /// <summary>
    /// Count the amount of nukes the user has picked up and can use to explote
    /// </summary>
    /// <returns>int</returns>
    public int CountAvailableNukes()
    {
        if (stkNukes == null)
        {
            return 0;
        }
        else
        {
            return stkNukes.Count;
        }
    }

    /// <summary>
    /// Reduces by one the amount of nukes the user has picked up and can use to explote
    /// </summary>
    /// <returns>bool showing if was able to use the nuke</returns>
    public bool UseNuke()
    {
        if (stkNukes == null)
        {
            return false;
        }
        else
        {
            //Remove from UI the used Nuke
            Destroy(stkNukes.Pop());
            txtNukes.text = CountAvailableNukes().ToString();
            return true;
        }
    }
}