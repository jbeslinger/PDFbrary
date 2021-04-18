using Paroxe.PdfRenderer;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PDFRendererBehavior : MonoBehaviour
{
    public int page = 0;

#if UNITY_ANDROID
    private string m_FileName = "sample.pdf";
    private string m_FilePath = "Android/Data/LibraryBooks";
#endif
#if UNITY_STANDALONE
    private string m_Path = @"C:\Users\jbeslinger\source\repos\PDFbrary\Assets\sample.pdf";
#endif

    void Start()
    {
#if UNITY_ANDROID
        string root = Application.persistentDataPath.Substring(0, Application.persistentDataPath.IndexOf("Android", StringComparison.Ordinal));
        string path = Path.Combine(Path.Combine(root, m_FilePath), m_FileName);
#endif

        PDFDocument pdfDocument = new PDFDocument(m_Path);

        if (pdfDocument.IsValid)
        {
            int pageCount = pdfDocument.GetPageCount();

            PDFRenderer renderer = new PDFRenderer();
            Texture2D tex = renderer.RenderPageToTexture(pdfDocument.GetPage(page % pageCount), 1024, 1024);

            tex.filterMode = FilterMode.Bilinear;
            tex.anisoLevel = 8;

            GetComponent<MeshRenderer>().material.mainTexture = tex;
        }
    }

}
