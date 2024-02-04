using Elasticsearch.API.Dtos;
using Elasticsearch.API.Repositories;
using System.Collections.Immutable;
using System.Net;

namespace Elasticsearch.API.Services
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository;
        private readonly ILogger<ProductService> _logger;

        public ProductService(ProductRepository productRepository, ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }
        public async Task<ResponseDto<ProductDto>> SaveAsync(ProductCreateDto request)
        {
            var response = await _productRepository.SaveAsync(request.CreateProduct());

            if (response == null)
            {
                return ResponseDto<ProductDto>.Fail(
                    new List<string> { "Beklenmedik bir hata oluştu. " },
                    HttpStatusCode.InternalServerError);
            }

            return ResponseDto<ProductDto>.Success(response.CreateDto(),
             HttpStatusCode.Created);
        }
        public async Task<ResponseDto<ImmutableList<ProductDto>>> GetAllAsync()
        {
            var response = await _productRepository.GetAllAsync();
            var products = response.Select(x => x.CreateDto()).ToImmutableList();

            return ResponseDto<ImmutableList<ProductDto>>.Success(products, HttpStatusCode.OK);
        }
        public async Task<ResponseDto<ProductDto>> GetByIdAsync(string id)
        {
            var response = await _productRepository.GetByIdAsync(id);

            if (response == null)
                return ResponseDto<ProductDto>.Fail("Ürün bulunamadı", HttpStatusCode.NotFound);

            return ResponseDto<ProductDto>.Success(response.CreateDto(), HttpStatusCode.OK);
        }
        public async Task<ResponseDto<bool>> UpdateAsync(ProductUpdateDto productUpdateDto)
        {
            var isSuccess = await _productRepository.UpdateAsync(productUpdateDto);

            if (!isSuccess)
            {
                return ResponseDto<bool>.Fail("Update sırasında bir hata oluştu.", HttpStatusCode.InternalServerError);
            }

            return ResponseDto<bool>.Success(true, HttpStatusCode.NoContent);
        }
        public async Task<ResponseDto<bool>> DeleteAsync(string id)
        {
            var response = await _productRepository.DeleteAsync(id);

            if (!response.IsValid && response.Result == Nest.Result.NotFound)
            {
                return ResponseDto<bool>.Fail("Silinmeye çalışılan data bulunamamıştır.", HttpStatusCode.NotFound);
            }

            if (!response.IsValid)
            {
                // for developer
                _logger.LogError(response.OriginalException, response.ServerError.Error.ToString());

                // for user
                return ResponseDto<bool>.Fail("Silme sırasında bir hata oluştu.", HttpStatusCode.InternalServerError);
            }

            return ResponseDto<bool>.Success(true, HttpStatusCode.NoContent);
        }
    }
}
