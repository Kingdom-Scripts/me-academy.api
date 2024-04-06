using Mapster;
using me_academy.core.Interfaces;
using me_academy.core.Models.App;
using me_academy.core.Models.App.Constants;
using me_academy.core.Models.Configurations;
using me_academy.core.Models.Input.Auth;
using me_academy.core.Models.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace me_academy.core.Services;

public class FileService : IFileService
{
    private readonly FileSettings _fileSettings;
    private readonly IHostEnvironment _hostEnvironment;
    private readonly MeAcademyContext _context;
    private readonly UserSession _userSession;

    public FileService(IOptions<AppConfig> appConfig, IHostEnvironment hostEnvironment, MeAcademyContext context,
        UserSession userSession)
    {
        if (appConfig == null) throw new ArgumentNullException(nameof(appConfig));
        _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _userSession = userSession ?? throw new ArgumentNullException(nameof(userSession));

        _fileSettings = appConfig.Value.FileSettings;

        // set up tinify
        TinifyAPI.Tinify.Key = appConfig.Value.TinifyKey;
    }

    public async Task<Result<DocumentView>> UploadFile(string folder, IFormFile file)
    {
        var result = await Upload(folder, file);
        if (!result.Success)
            return new ErrorResult<DocumentView>(result.Title, result.Message);

        await _context.Documents.AddAsync(result.Content);

        int saved = await _context.SaveChangesAsync();

        return saved > 0
            ? new SuccessResult<DocumentView>(result.Content.Adapt<DocumentView>())
            : new ErrorResult<DocumentView>("Saving file failed");
    }

    public async Task<Result<Document>> UploadFileInternal(string folder, IFormFile file) => await Upload(folder, file);

    public FileStreamResult? GetFile(string folder, string fileName)
    {
        string filePath = Path.Combine(_hostEnvironment.ContentRootPath, _fileSettings.BaseFolder, folder, fileName);
        if (!File.Exists(filePath))
            return null;

        var stream = new FileStream(filePath, FileMode.Open);
        return new FileStreamResult(stream, "application/octet-stream");
    }

    public async Task<Result> DeleteFile(int documentId)
    {
        var result = await Delete(documentId);
        if (!result.Success)
            return new ErrorResult(result.Title, result.Message);

        int saved = await _context.SaveChangesAsync();
        return saved > 0
            ? new SuccessResult(result.Message)
            : new ErrorResult("Failed to delete file");
    }

    public async Task<Result> DeleteFileInternal (int documentId) => await Delete(documentId);

    private async Task<Result<Document>> Upload(string folder, IFormFile file)
    {
        string ext = Path.GetExtension(file.FileName).ToLowerInvariant();

        string folderPath =
            Path.Combine(_hostEnvironment.ContentRootPath, _fileSettings.BaseFolder, folder);

        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);


        string fileType = GetDocumentType(ext);
        string fileUploadName = $"{Guid.NewGuid()}{ext}";
        string filePath = Path.Combine(folderPath, fileUploadName);
        if (filePath != DocumentTypeEnum.IMAGE)
        {
            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }
        else
        {
            await SaveImageAsync(filePath, file);
        }

        // save file info to database
        var document = new Document
        {
            Name = file.FileName,
            Type = fileType,
            Url = $"{folder}/{fileUploadName}",
            ThumbnailUrl = fileType == DocumentTypeEnum.IMAGE
                ? $"{folder}/_thumbnail/{fileUploadName}"
                : $"{folder}/_thumbnail/{fileType}.png",
            CreatedById = _userSession.UserId
        };

        return new SuccessResult<Document>(document);
    }

    private static async Task SaveImageAsync(string filePath, IFormFile image)
    {
        await using (var stream = new MemoryStream())
        {
            await image.CopyToAsync(stream);
            stream.Seek(0, SeekOrigin.Begin);

            // Compress the image using Tinify
            var source = await TinifyAPI.Tinify.FromBuffer(stream.ToArray());

            // get thumbnail
            byte[]? thumbnail = await source
                .Preserve("copyright", "creation")
                .Resize(new
                {
                    method = "fit",
                    width = 150,
                    height = 150
                }).ToBuffer();

            // compress original
            byte[]? optimized = await source
                .Preserve("copyright", "creation")
                .ToBuffer();

            // save files
            await File.WriteAllBytesAsync(filePath, optimized);
            await File.WriteAllBytesAsync(filePath + "_thumbnail", thumbnail);
        }
    }

    private async Task<Result> Delete(int documentId)
    {
        var document = await _context.Documents.FindAsync(documentId);
        if (document is null)
            return new ErrorResult("Document not found");

        string filePath = Path.Combine(_hostEnvironment.ContentRootPath, _fileSettings.BaseFolder, document.Url);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        else
        {
            return new ErrorResult("File does not exist.");
        }

        _context.Documents.Remove(document);

        return new SuccessResult("File deleted successfully.");
    }

    private static string GetDocumentType(string extension)
    {
        if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
        {
            return DocumentTypeEnum.IMAGE;
        }
        else if (extension == ".pdf")
        {
            return DocumentTypeEnum.PDF;
        }
        else if (extension == ".doc" || extension == ".docx")
        {
            return DocumentTypeEnum.WORD_DOCUMENT;
        }
        else
        {
            return DocumentTypeEnum.UNKNWON;
        }
    }
}