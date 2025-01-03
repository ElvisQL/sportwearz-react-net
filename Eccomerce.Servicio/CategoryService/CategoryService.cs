using AutoMapper;
using Eccomerce.DTO.Category;
using Eccomerce.DTO.Producto;
using Eccomerce.MODELO;
using Eccomerce.Repositorio.Contratos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Eccomerce.Servicio.CategoryService
{
    public class CategoryService : ICategoryService
    {

        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(IGenericRepository<Category> categoryRepository,IMapper imapper )
        {
            _categoryRepository = categoryRepository;
            _mapper = imapper;
        }

        public async Task<CategoryDTO> Create(CategoryDTO modelo)
        {
            try
            {
                var productDB = _mapper.Map<Category>(modelo);
                var response = await _categoryRepository.Crear(productDB);
                if (response.CategoryId != 0)
                {
                    return _mapper.Map<CategoryDTO>(response);
                }
                else
                {
                    throw new TaskCanceledException("no se pudo crear la categoria");
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
                var consultaLINQ = _categoryRepository.Consultar(u => u.CategoryId == id);
                var response = await consultaLINQ.FirstOrDefaultAsync();
                if (response != null)
                {
                    var responseDelete = await _categoryRepository.Eliminar(response);
                    if (!responseDelete)
                    {
                        throw new TaskCanceledException("No se pudo eliminar la categoria");
                    }
                    return responseDelete;
                }
                else
                {
                    throw new TaskCanceledException("No se encontro la categoria a eliminar");
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<CategoryDTO>> ListarCategorias(string busqueda)
        {
            try
            {
                var consultaLINQ = _categoryRepository.Consultar(u => string.Concat(u.Nombre, u.Descripcion).Contains(busqueda));
                List<CategoryDTO> lista = _mapper.Map<List<CategoryDTO>>(await consultaLINQ.ToListAsync());
                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<CategoryDTO> Read(int id)
        {
            try
            {
                var consultaSQL = _categoryRepository.Consultar(u => u.CategoryId == id);
                var response = await consultaSQL.FirstOrDefaultAsync();
                if (response != null)
                {
                    return _mapper.Map<CategoryDTO>(response);
                }
                else
                {
                    throw new TaskCanceledException("No se encontro la categoria");
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<bool> Update(CategoryDTO modelo)
        {
            try
            {

                var consultaLINQ = _categoryRepository.Consultar(p => p.CategoryId == modelo.CategoryId);
                var response = await consultaLINQ.FirstOrDefaultAsync();
                if (response != null)
                {
                    response.Nombre = modelo.Nombre;
                    response.Descripcion = modelo.Descripcion;

                    var responseCategory = await _categoryRepository.Editar(response);
                    if (!responseCategory)
                    {
                        throw new TaskCanceledException("No se pudo editar la categoria");
                    }
                    return responseCategory;
                }
                else
                {
                    throw new TaskCanceledException("No se encontro la categoria a editar");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
