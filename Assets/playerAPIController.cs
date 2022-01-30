using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;

public class playerAPIController : MonoBehaviour
{

    public int player1Score = 0;
    public int player2Score = 0;
    public Player player1;
    public Player player2;

    public Text scoreOne;
    public Text scoreTwo;
    

    public Transform spawnLocation1;
    public Transform spawnLocation2;
 

    public GameObject saber;
    public GameObject staff;
    public GameObject axe;
    public GameObject knife;
    public GameObject mace;

    public class Player
    {
        public int id;
        public string name;
        public string attack;
    }

    public string textVal = "";
    private readonly string baseUrl = "http://127.0.0.1:3000/api/attack/";

    public float period = 0.0f;
    Boolean drop = true;
    Boolean start = false;
    Boolean player1Ready = false;
    Boolean player2Ready = false;


    void Update()
    {
        if (start)
        {

            if (period > 2)
            {
                if (player1Ready && player2Ready)
                {
                    if (drop) OnTimer();
                    drop = false;
                    if (period > 5)
                    {
                        Debug.Log("5 seconds passed");
                        period = 0;
                        compareAttack(player1.attack, player2.attack);
                        drop = true;
                        Debug.Log(player1Score);
                        Debug.Log(player2Score);
                        scoreOne.text = player1Score.ToString();
                        scoreTwo.text = player2Score.ToString();

                    }
                }
                else
                {
                    StartCoroutine(CheckSelection(baseUrl + "1"));
                    StartCoroutine(CheckSelection(baseUrl + "2"));
                    period = 0;
                }   

            }
        }

        period += UnityEngine.Time.deltaTime;
    }

    void compareAttack(string attack1, string attack2)
    {
        if (attack1 == attack2) return;
        if (attack1 == "saber")
        {
            if (attack2 == "mace")
            {
                player2Score++;
                return;
            }
            if (attack2 == "staff")
            {
                player1Score++;
                return;
            }
            if (attack2 == "knife")
            {
                player1Score++;
                return;
            }
            if (attack2 == "axe")
            {
                player2Score++;
                return;
            }

        }
        if (attack1 == "mace")
        {
            if (attack2 == "saber")
            {
                player1Score++;
                return;
            }
            if (attack2 == "staff")
            {
                player2Score++;
                return;
            }
            if (attack2 == "knife")
            {
                player2Score++;
                return;
            }
            if (attack2 == "axe")
            {
                player1Score++;
                return;
            }
        }
        if (attack1 == "knife")
        {
            if (attack2 == "mace")
            {
                player1Score++;
                return;
            }
            if (attack2 == "staff")
            {
                player2Score++;
                return;
            }
            if (attack2 == "saber")
            {
                player2Score++;
                return;
            }
            if (attack2 == "axe")
            {
                player1Score++;
                return;
            }

        }
        if (attack1 == "staff")
        {
            if (attack2 == "mace")
            {
                player2Score++;
                return;
            }
            if (attack2 == "saber")
            {
                player1Score++;
                return;
            }
            if (attack2 == "knife")
            {
                player1Score++;
                return;
            }
            if (attack2 == "axe")
            {
                player2Score++;
                return;
            }

        }
        if (attack1 == "axe")
        {
            if (attack2 == "mace")
            {
                player2Score++;
                return;
            }
            if (attack2 == "staff")
            {
                player1Score++;
                return;
            }
            if (attack2 == "knife")
            {
                player1Score++;
                return;
            }
            if (attack2 == "saber")
            {
                player2Score++;
                return;
            }

        }
    }
    public void OnButtonClick()
    {
        start = true;
    }

    public void OnTimer()
    {
        Debug.Log("See if click works");
        StartCoroutine(GetRequest(baseUrl + "1"));
        StartCoroutine(GetRequest(baseUrl + "2"));

    }

    void DropItem(Player player)
    {
        string index = player.attack;
        GameObject objectToSpawn = axe;
        if (index == "saber") objectToSpawn = saber;
        if (index == "staff") objectToSpawn = staff;
        if (index == "axe") objectToSpawn = axe;
        if (index == "knife") objectToSpawn = knife;
        if (index == "mace") objectToSpawn = mace;

        if (player.name == "Player 1")
        {
            Destroy(Instantiate(objectToSpawn, spawnLocation1.transform.position, objectToSpawn.transform.rotation), 3);
            player1 = player;
        }
        else
        {
            Destroy(Instantiate(objectToSpawn, spawnLocation2.transform.position, objectToSpawn.transform.rotation), 3);
            player2 = player;
        }
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    string jsonCut = webRequest.downloadHandler.text;
                    Player player = JsonUtility.FromJson<Player>(jsonCut);

                    DropItem(player);

                    break;
            }
        }
    }


    IEnumerator CheckSelection(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    string jsonCut = webRequest.downloadHandler.text;
                    Player player = JsonUtility.FromJson<Player>(jsonCut);
                    if(player.attack != "none")
                    {
                        if (player.name == "Player 1") player1Ready = true;
                        if (player.name == "Player 2") player2Ready = true;
                    }
                    
                    break;
            }
        }
    }

}