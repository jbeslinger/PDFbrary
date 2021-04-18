using OVR.OpenVR;
using Paroxe.PdfRenderer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static OVRInput;

public class BookBehavior : MonoBehaviour
{
    private const int MAX_SPINE_NAME = 28;
    private const int RENDER_RESOLUTION = 1024;

    public Renderer cover, backcover, spine, leftPage, rightPage;

    private string m_FileName;
    private string m_BookDir;

    private PDFRenderer m_PDFRenderer = new PDFRenderer();
    private PDFDocument m_Doc;
    private Animator m_Animator;
    private OVRGrabbable m_GrabbableScript;
    private int m_CurrentPage = 1;
    private int m_PageCount;
    private bool m_Open = false;

    public void Create(string dir, string name)
    {
        m_FileName = name;
        m_BookDir = dir;

        string path = m_BookDir + m_FileName;
        m_Doc = new PDFDocument(path);

        if (m_Doc.IsValid)
        {
            m_PageCount = m_Doc.GetPageCount();

            // Get the cover
            Texture2D tex = m_PDFRenderer.RenderPageToTexture(m_Doc.GetPage(0 % m_PageCount), RENDER_RESOLUTION, RENDER_RESOLUTION);
            tex.filterMode = FilterMode.Bilinear;
            tex.anisoLevel = 8;
            cover.material.mainTexture = tex;

            Color bookColor = tex.GetPixel(1, 1);

            // Set the color of the book
            backcover.material.SetColor("_Color", bookColor);
            spine.material.SetColor("_Color", bookColor);

            RenderPages();

            // Set the spine text
            string bookname = m_FileName.Substring(0, m_FileName.Length - 4);
            if (bookname.Length > MAX_SPINE_NAME)
                bookname = bookname.Substring(0, MAX_SPINE_NAME);
            GetComponentInChildren<TextMesh>().text = bookname;
        }
    }

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_GrabbableScript = GetComponent<OVRGrabbable>();
    }

    private void Update()
    {
        // Book is only functional when being held
        if (m_GrabbableScript.grabbed)
        {
            // If Y is pressed and the player is holding the book, open/close the book
            if (OVRInput.GetDown(OVRInput.Button.Four))
            {
                if (m_Open)
                    CloseBook();
                else
                    OpenBook();

                m_Open = !m_Open;
            }

            if (m_Open)
            {
                // If X is pressed and the player is holding the book, turn back
                // If A is pressed and the player is holding the book, turn forward
                if (OVRInput.GetDown(OVRInput.Button.One))
                    TurnToNextPage();
                else if (OVRInput.GetDown(OVRInput.Button.Three))
                    TurnToPreviousPage();
            }
        }
    }

    private void OpenBook()
    {
        m_Animator.SetTrigger("Open");
    }

    private void CloseBook()
    {
        m_Animator.SetTrigger("Close");
        
    }

    private void TurnToNextPage()
    {
        m_CurrentPage = Mathf.Clamp(m_CurrentPage + 2, 1, m_PageCount - 1);
        RenderPages();
    }

    private void TurnToPreviousPage()
    {
        m_CurrentPage = Mathf.Clamp(m_CurrentPage - 2, 1, m_PageCount - 1);
        RenderPages();
    }

    private void RenderPages()
    {
        Texture2D tex = m_PDFRenderer.RenderPageToTexture(m_Doc.GetPage(m_CurrentPage % m_PageCount), RENDER_RESOLUTION, RENDER_RESOLUTION);
        leftPage.material.mainTexture = tex;

        // Get the right page
        tex = m_PDFRenderer.RenderPageToTexture(m_Doc.GetPage((m_CurrentPage + 1) % m_PageCount), RENDER_RESOLUTION, RENDER_RESOLUTION);
        rightPage.material.mainTexture = tex;
    }

}
