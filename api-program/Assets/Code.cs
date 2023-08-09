using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class Code : MonoBehaviour
{

    public string URL;
    public string URL2;
    public List<RawImage> Images;
    public List<TextMeshProUGUI> textos_Imsgenes;
    public TextMeshProUGUI userTXT;
    public int id;


    public void yoquese()
    {
        StartCoroutine(GetUsers());
    }

    IEnumerator GetUsers() 
    {
        UnityWebRequest request = UnityWebRequest.Get(URL + id);
        yield return request.SendWebRequest();

        if (request.isNetworkError)
        {
            Debug.Log("F");
        }
        else
        {
            //Debug.Log(request.downloadHandler.text);
            User MyUser = JsonUtility.FromJson<User>(request.downloadHandler.text);
            //Debug.Log(MyUser.id);
            //Debug.Log(MyUser.username);
            userTXT.text = MyUser.username;

            for (int i = 0; i < MyUser.deck.Count; i++)
            {
                StartCoroutine(GetCharacter(i,MyUser.deck[i]));
            }

        }
    }

    IEnumerator GetCharacter(int i, int CharacterID) 
    {
        UnityWebRequest request = UnityWebRequest.Get(URL2 + "/" + CharacterID);
        yield return request.SendWebRequest();

        if (request.isNetworkError) 
        {
            Debug.Log("F2");
        }
        else 
        {
            //Debug.Log(request.downloadHandler.text);
            Character character = JsonUtility.FromJson<Character>(request.downloadHandler.text);
            textos_Imsgenes[i].text = character.name;
            StartCoroutine(GetImage(character.image,i));
           
            //Character.name
        }
    }

    IEnumerator GetImage(string url, int i)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.isNetworkError)
        {
            Debug.Log("gas");
        }else if (!request.isHttpError) 
        {
            Images[i].texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        }
    }
}



public class User
{
    public int id;
    public string username;
    public bool state;
    public List<int> deck;
}

public class Character 
{
    public string name;
    public string image;
}
