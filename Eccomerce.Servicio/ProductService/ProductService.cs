using AutoMapper;
using Eccomerce.DTO.Producto;
using Eccomerce.DTO.User;
using Eccomerce.MODELO;
using Eccomerce.Repositorio.Contratos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eccomerce.Servicio.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IGenericRepository<Product> productRepository, IMapper mapper)
        {

            _mapper = mapper;
            _productRepository = productRepository;
        }

        public async Task<List<ProductoDTO>> Catalogo(
            List<int> marcaIds,
            List<int> categoriaIds,
            decimal precioMin = 0,
            decimal precioMax = 0,
            string busqueda = "")
        {
            try
            {
                var consulta = _productRepository.Consultar();

                // Filtros dinámicos
                if (!string.IsNullOrEmpty(busqueda))
                    consulta = consulta.Where(p => p.ProductName.Contains(busqueda));

                if (marcaIds.Any())
                    consulta = consulta.Where(p => marcaIds.Contains(p.Brand.BrandId)); // Filtrar por ID

                if (categoriaIds.Any())
                    consulta = consulta.Where(p => p.Categories.Any(c => categoriaIds.Contains(c.CategoryId)));

                if (precioMin > 0)
                    consulta = consulta.Where(p => p.Price >= precioMin);

                if (precioMax > 0)
                    consulta = consulta.Where(p => p.Price <= precioMax);

                consulta = consulta
                    .Include(c => c.Categories)
                    .Include(p => p.Brand)
                    ;


                // Ejecutar la consulta
                var productos = await consulta.ToListAsync();

                return _mapper.Map<List<ProductoDTO>>(productos);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error en catálogo avanzado", ex);
            }
        }

        public async Task<ProductoDTO> Create(ProductoDTO modelo)
        {
            try
            {
                var product = new Product
                {
                    ProductName = modelo.ProductName,
                    Description = modelo.Description,
                    Price = modelo.Price,
                    Stock = modelo.Stock,
                    ImageUrl = modelo.ImageURL,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    BrandId = modelo.BrandId
                };


                var categories = await _productRepository.GetCategoriesByIdsAsync(modelo.CategoriesIds);

                if (!categories.Any())
                {
                    throw new ArgumentException("No valid categories found.");
                }

                product.Categories = categories;


                var response = await _productRepository.Crear(product);
                if (response.ProductId != 0)
                {
                    return _mapper.Map<ProductoDTO>(response);
                }
                else
                {
                    throw new TaskCanceledException("no se pudo crear el producto");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<bool> Delete(int id)
        {
            try
            {
                var consultaLINQ = _productRepository.Consultar(u => u.ProductId == id);
                var response = await consultaLINQ.FirstOrDefaultAsync();
                if (response != null)
                {
                    var responseDelete = await _productRepository.Eliminar(response);
                    if (!responseDelete)
                    {
                        throw new TaskCanceledException("No se pudo eliminar el producto");
                    }
                    return responseDelete;
                }
                else
                {
                    throw new TaskCanceledException("No se encontro el producto a eliminar");
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<ProductoDTO>> Listar(string busqueda)
        {
            try
            {
                var consultaLINQ = _productRepository.Consultar(p => p.ProductName.ToLower().Contains(busqueda.ToLower()));

                consultaLINQ = consultaLINQ
                    .Include(c => c.Categories)
                    .Include(p => p.Brand);

                List<ProductoDTO> lista = _mapper.Map<List<ProductoDTO>>(await consultaLINQ.ToListAsync());
                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<ProductoDTO> Read(int id)
        {
            try
            {
                var consultaLINQ = _productRepository.Consultar(u => u.ProductId == id);
                consultaLINQ = consultaLINQ
                    .Include(c => c.Categories)
                    .Include(p => p.Brand);

                var response = await consultaLINQ.FirstOrDefaultAsync();
                if (response != null)
                {
                    return _mapper.Map<ProductoDTO>(response);
                }
                else
                {
                    throw new TaskCanceledException("No se encontro el producto");
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<bool> Update(int productId,ProductoDTO modelo)
        {
            try
            {

                var product = await _productRepository.Consultar(c => c.ProductId == productId)
                    .Include(p => p.Categories)
                    .FirstOrDefaultAsync();
               if (product == null)
                {
                    throw new TaskCanceledException("No se encontro el producto a actualizar");
                }
                product.ProductName = modelo.ProductName;
                product.Description = modelo.Description;
                product.Price = modelo.Price;
                product.Stock = modelo.Stock;
                product.ImageUrl = modelo.ImageURL;
                product.UpdatedAt = DateTime.UtcNow;
                product.BrandId = modelo.BrandId;
                var categories = await _productRepository.GetCategoriesByIdsAsync(modelo.CategoriesIds);
                if (!categories.Any())
                {
                    throw new ArgumentException("No valid categories found.");
                }
                product.Categories = categories;
                var response = await _productRepository.Editar(product);
                if (response)
                {
                    return response;
                }
                else
                {
                    throw new TaskCanceledException("No se pudo actualizar el producto");
                }
            }
            catch (Exception ex)
            {
                throw ;
            }
        }
    }
}
