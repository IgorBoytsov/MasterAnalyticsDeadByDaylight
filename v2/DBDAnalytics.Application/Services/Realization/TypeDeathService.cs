using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.Application.UseCases.Abstraction.TypeDeathCase;

namespace DBDAnalytics.Application.Services.Realization
{
    public class TypeDeathService(ICreateTypeDeathUseCase createTypeDeathUseCase,
                                  IDeleteTypeDeathUseCase deleteTypeDeathUseCase,
                                  IGetTypeDeathUseCase getTypeDeathUseCase,
                                  IUpdateTypeDeathUseCase updateTypeDeathUseCase) : ITypeDeathService
    {
        private readonly ICreateTypeDeathUseCase _createTypeDeathUseCase = createTypeDeathUseCase;
        private readonly IDeleteTypeDeathUseCase _deleteTypeDeathUseCase = deleteTypeDeathUseCase;
        private readonly IGetTypeDeathUseCase _getTypeDeathUseCase = getTypeDeathUseCase;
        private readonly IUpdateTypeDeathUseCase _updateTypeDeathUseCase = updateTypeDeathUseCase;

        public async Task<(TypeDeathDTO? TypeDeathDTO, string Message)> CreateAsync(string typeDeathName, string typeDeathDescription)
        {
            return await _createTypeDeathUseCase.CreateAsync(typeDeathName, typeDeathDescription);
        }

        public async Task<(bool IsDeleted, string Message)> DeleteAsync(int idTypeDeath)
        {
            return await _deleteTypeDeathUseCase.DeleteAsync(idTypeDeath);
        }

        public List<TypeDeathDTO> GetAll()
        {
            return _getTypeDeathUseCase.GetAll();
        }

        public async Task<List<TypeDeathDTO>> GetAllAsync()
        {
            return await _getTypeDeathUseCase.GetAllTypeDeathAsync();
        }

        public async Task<TypeDeathDTO> GetAsync(int idTypeDeath)
        {
            return await _getTypeDeathUseCase.GetTypeDeathAsync(idTypeDeath);
        }

        public async Task<(TypeDeathDTO? TypeDeathDTO, string? Message)> UpdateAsync(int idTypeDeath, string typeDeathName, string typeDeathDescription)
        {
            return await _updateTypeDeathUseCase.UpdateAsync(idTypeDeath, typeDeathName, typeDeathDescription);
        }

        public async Task<TypeDeathDTO> ForcedUpdateAsync(int idTypeDeath, string typeDeathName, string typeDeathDescription)
        {
            return await _updateTypeDeathUseCase.ForcedUpdateAsync(idTypeDeath, typeDeathName, typeDeathDescription);
        }
    }
}
