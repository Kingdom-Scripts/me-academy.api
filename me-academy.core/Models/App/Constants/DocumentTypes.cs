﻿namespace me_academy.core.Models.App.Constants;

public class DocumentTypes
{
    public const string IMAGE = "Image";
    public const string PDF = "PDF";
    public const string WORD_DOCUMENT = "Word Document";
    public const string UNKNWON = "Unknown";
    public static readonly string[] AllowedTypes = { ".jpg", ".jpeg", ".png", ".gif", ".pdf", ".doc", ".docx" };
}