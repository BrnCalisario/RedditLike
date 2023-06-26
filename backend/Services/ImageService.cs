namespace Reddit.Services;

using Repositories;
using Model;

public class ImageService
{
    private IRepository<ImageDatum> repository;
    public ImageService(IRepository<ImageDatum> repo)
    { }

    public async Task<int> saveImage(IFormFile file)
    {
        
        Console.WriteLine("oi");

        using MemoryStream ms = new MemoryStream();

        await file.CopyToAsync(ms);
        var data = ms.GetBuffer();

        var img = new ImageDatum();
        img.Photo = data;
        await this.repository.Add(img);

        var code = img.Id;
        return code;
    }
}