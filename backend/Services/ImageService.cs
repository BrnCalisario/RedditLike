using Reddit.Model;
using Reddit.Repositories;

public interface IImageService 
{
    Task<int> SaveImg(IFormFile file);
}

public class ImageService : IImageService
{
    private IRepository<ImageDatum> imageRepository;

    public ImageService(IRepository<ImageDatum> repo)
       => this.imageRepository = repo;
    

    public async Task<int> SaveImg(IFormFile file)
    {
        using MemoryStream ms = new MemoryStream();

        await file.CopyToAsync(ms);
        var data = ms.GetBuffer();

        var img = new ImageDatum();
        img.Photo = data;

        await this.imageRepository.Add(img);

        return img.Id;
    }
}