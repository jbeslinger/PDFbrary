using Paroxe.PdfRenderer;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PDFRendererBehavior : MonoBehaviour
{
    public int page = 0;

#if UNITY_ANDROID
    private string m_Path = @"\sdcard\LibraryBooks\sample.pdf";
#endif
#if UNITY_STANDALONE
    private string m_Path = @"C:\Users\jbeslinger\source\repos\PDFbrary\Assets\sample.pdf";
#endif

    void Start()
    {
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
