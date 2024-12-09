using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Song.DataAccess.Repository.IRepository;
using Song.Models;
using Song.Models.ViewModels;
using Song.Utility;
using SongWeb.Services;
using System.Security.Claims;

namespace SongWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly BlobService _blobService;

        public ProductController(IUnitOfWork unitOfWork, 
            IWebHostEnvironment webHostEnvironment,
            BlobService blobService)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _blobService = blobService;
        }

        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return View(objProductList);
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };
            if (id == null || id == 0)
            {
                return View(productVM);
            }
            else
            {
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id, includeProperties: "ProductImages");
                return View(productVM);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(ProductVM productVM, List<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                }

                _unitOfWork.Save();

                if (files != null)
                {
                    foreach (IFormFile file in files)
                    {
                        if (file != null && file.Length > 0)
                        {
                            string fileName = $"{productVM.Product.Id}/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                            
                            using (var stream = file.OpenReadStream())
                            {
                                string imageUrl = await _blobService.UploadAsync(stream, fileName);

                                ProductImage productImage = new()
                                {
                                    ImageUrl = imageUrl,
                                    ProductId = productVM.Product.Id,
                                };

                                if (productVM.Product.ProductImages == null)
                                    productVM.Product.ProductImages = new List<ProductImage>();

                                productVM.Product.ProductImages.Add(productImage);
                            }
                        }
                    }

                    _unitOfWork.Product.Update(productVM.Product);
                    _unitOfWork.Save();
                }

                TempData["success"] = "Product created/updated successfully";
                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(productVM);
            }
        }

        public async Task<IActionResult> DeleteImage(int imageId)
        {
            var imageToBeDeleted = _unitOfWork.ProductImage.Get(u => u.Id == imageId);
            int productId = imageToBeDeleted.ProductId;

            if (imageToBeDeleted != null)
            {
                if (!string.IsNullOrEmpty(imageToBeDeleted.ImageUrl))
                {
                    var oldImageUrl = imageToBeDeleted.ImageUrl;
                    var fileName = Path.GetFileName(new Uri(oldImageUrl).LocalPath);
                    await _blobService.DeleteAsync(fileName);
                }

                _unitOfWork.ProductImage.Remove(imageToBeDeleted);
                _unitOfWork.Save();

                TempData["success"] = "Deleted successfully";
            }

            return RedirectToAction(nameof(Upsert), new { id = productId });
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductList });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            var productToBeDeleted = _unitOfWork.Product.Get(u => u.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            if (productToBeDeleted.ProductImages != null)
            {
                foreach (var image in productToBeDeleted.ProductImages)
                {
                    if (!string.IsNullOrEmpty(image.ImageUrl))
                    {
                        var fileName = Path.GetFileName(new Uri(image.ImageUrl).LocalPath);
                        await _blobService.DeleteAsync(fileName);
                    }
                }
            }

            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion
    }
}
