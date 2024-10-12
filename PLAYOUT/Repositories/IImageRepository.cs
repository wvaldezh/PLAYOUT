namespace PLAYOUT.Repositories
{
    public interface IImageRepository

    {
        Task<String> UploadAsync(IFormFile file);
    }
}
