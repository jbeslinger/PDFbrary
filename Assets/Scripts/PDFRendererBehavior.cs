using Paroxe.PdfRenderer;
using System.Collections.Generic;
using UnityEngine;

public class PDFRendererBehavior : MonoBehaviour
{
    public int page = 0;
    public string filePath = "";

    void Start()
    {
        PDFDocument pdfDocument = new PDFDocument(filePath);

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
