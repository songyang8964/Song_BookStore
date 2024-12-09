using Azure.Storage.Blobs;
using Song.DataAccess.Data;
using Song.Models;
using Microsoft.EntityFrameworkCore;

namespace SongWeb.Services
{
    public class ImageMigrationService
    {
        private readonly BlobService _blobService;
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageMigrationService(BlobService blobService, ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _blobService = blobService;
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task MigrateImagesAsync()
        {
            var products = await _db.Products.Include(p => p.ProductImages).ToListAsync();
            var wwwrootPath = _webHostEnvironment.WebRootPath;

            foreach (var product in products)
            {
                if (product.ProductImages != null && product.ProductImages.Any())
                {
                    foreach (var image in product.ProductImages)
                    {
                        if (!string.IsNullOrEmpty(image.ImageUrl))
                        {
                            // 获取本地图片的完整路径
                            var localImagePath = Path.Combine(wwwrootPath, image.ImageUrl.TrimStart('/'));
                            
                            if (File.Exists(localImagePath))
                            {
                                // 生成Blob存储的文件名
                                var fileName = Path.GetFileName(localImagePath);
                                var blobFileName = $"{product.Id}/{fileName}";

                                // 上传到Blob存储
                                using (var fileStream = File.OpenRead(localImagePath))
                                {
                                    var blobUrl = await _blobService.UploadAsync(fileStream, blobFileName);
                                    
                                    // 更新数据库中的URL
                                    image.ImageUrl = blobUrl;
                                }
                            }
                        }
                    }
                }
            }

            await _db.SaveChangesAsync();
        }
    }
}
