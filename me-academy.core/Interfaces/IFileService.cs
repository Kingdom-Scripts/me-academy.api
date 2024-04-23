
using me_academy.core.Models.App;
using me_academy.core.Models.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace me_academy.core.Interfaces;

public interface IFileService
{
    Task<Result<DocumentView>> UploadFile(string folder, IFormFile file);
    Task<Result<Document>> UploadFileInternal(string folder, IFormFile file);
    FileStreamResult? GetFile(string folder, string fileName);
    Task<Result> DeleteFile(int documentId);
    Task<Result> DeleteFileInternal(int documentId);
}