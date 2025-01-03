using AutoMapper;
using Eccomerce.DTO.Category;
using Eccomerce.MODELO;
using Eccomerce.Repositorio.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eccomerce.DTO.Brand;
using Microsoft.EntityFrameworkCore;

namespace Eccomerce.Servicio.BrandService
{
    public class BrandService : IBrandService
    {
        private readonly IGenericRepository<Brand> _brandRepository;
        private readonly IMapper _mapper;

        public BrandService(IGenericRepository<Brand> brandRepository, IMapper mapper)
        {
            _brandRepository=brandRepository;
            _mapper=mapper;
        }

        public async Task<BrandDTO> Create(BrandDTO modelo)
        {
            try
            {
                var brandDB = _mapper.Map<Brand>(modelo);
                var response = await _brandRepository.Crear(brandDB);
                if (response.BrandId != 0)
                {
                    return _mapper.Map<BrandDTO>(response);
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
                var consultaLINQ = _brandRepository.Consultar(u => u.BrandId == id);
                var response = await consultaLINQ.FirstOrDefaultAsync();
                if (response != null)
                {
                    var responseDelete = await _brandRepository.Eliminar(response);
                    if (!responseDelete)
                    {
                        throw new TaskCanceledException("No se pudo eliminar la marca");
                    }
                    return responseDelete;
                }
                else
                {
                    throw new TaskCanceledException("No se encontro la marca a eliminar");
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<BrandDTO>> ListarMarcas(string busqueda)
        {
            try
            {
                var consultaLINQ = _brandRepository.Consultar(u => string.Concat(u.BrandName, u.Description).Contains(busqueda));
                List<BrandDTO> lista = _mapper.Map<List<BrandDTO>>(await consultaLINQ.ToListAsync());
                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<BrandDTO> Read(int id)
        {
            try
            {
                var consultaSQL = _brandRepository.Consultar(u => u.BrandId == id);
                var response = await consultaSQL.FirstOrDefaultAsync();
                if (response != null)
                {
                    return _mapper.Map<BrandDTO>(response);
                }
                else
                {
                    throw new TaskCanceledException("No se encontro la marca");
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<bool> Update(BrandDTO modelo)
        {
            try
            {

                var consultaLINQ = _brandRepository.Consultar(p => p.BrandId == modelo.BrandId);
                var response = await consultaLINQ.FirstOrDefaultAsync();
                if (response != null)
                {
                    response.BrandName = modelo.BrandName;
                    response.Description = modelo.Description;

                    var responseCategory = await _brandRepository.Editar(response);
                    if (!responseCategory)
                    {
                        throw new TaskCanceledException("No se pudo editar la marca");
                    }
                    return responseCategory;
                }
                else
                {
                    throw new TaskCanceledException("No se encontro la marca a editar");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

