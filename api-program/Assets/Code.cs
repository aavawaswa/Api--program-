using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Code : MonoBehaviour
{

    public string URL;
    public string URL2;
    public List<RawImage> Images;


    public void yoquese()
    {
        StartCoroutine(GetUsers());
    }

    IEnumerator GetUsers() 
    {
        UnityWebRequest request = UnityWebRequest.Get(URL);
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

            foreach (int i in MyUser.deck)
            {
                StartCoroutine(GetCharacter(i));
            }

        }
    }

    IEnumerator GetCharacter(int i) 
    {
        UnityWebRequest request = UnityWebRequest.Get(URL2 + "/" + i);
        yield return request.SendWebRequest();

        if (request.isNetworkError) 
        {
            Debug.Log("F");
        }
        else 
        {
            //Debug.Log(request.downloadHandler.text);
            Character character = JsonUtility.FromJson<Character>(request.downloadHandler.text);
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
            Images[i-1].texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
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
