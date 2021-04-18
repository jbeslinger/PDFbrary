using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BookshelfBehavior : MonoBehaviour
{
    public Transform spawnPoints;
    public GameObject bookPrefab;

#if UNITY_ANDROID
    private string m_BookDir;
#endif
#if UNITY_STANDALONE
    private string m_BookDir = @"N:\Not Illegal Books\Books\!Textbooks\";
#endif

    private void Start()
    {
#if UNITY_ANDROID
        m_BookDir = Application.persistentDataPath + "/LibraryBooks/";
#endif

        if (!Directory.Exists(m_BookDir))
        {
            Directory.CreateDirectory(m_BookDir);
        }

        DirectoryInfo dir = new DirectoryInfo(m_BookDir);
        FileInfo[] info = dir.GetFiles("*.pdf");
        for (int i = 0; i < info.Length; i++)
        {
            FileInfo file = info[i];
            Transform spawn = spawnPoints.GetChild(i);
            GameObject newBook = Instantiate(bookPrefab, spawn.position, spawn.rotation);
            newBook.GetComponent<BookBehavior>().Create(m_BookDir, file.Name);
        }
    }

}
