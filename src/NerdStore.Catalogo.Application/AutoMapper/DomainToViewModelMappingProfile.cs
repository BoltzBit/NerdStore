using AutoMapper;
using NerdStore.Catalogo.Application.ViewModels;
using NerdStore.Catalogo.Domain;

namespace NerdStore.Catalogo.Application.AutoMapper;

public class DomainToViewModelMappingProfile : Profile
{
    public DomainToViewModelMappingProfile()
    {
        CreateMap<Produto, ProdutoViewModel>()
            .ForMember(pv => pv.Altura,
                output => output
                    .MapFrom(p => p.Dimensoes.Altura))
            .ForMember(pv => pv.Largura,
                output => output
                    .MapFrom(p => p.Dimensoes.Largura))
            .ForMember(pv => pv.Profundidade,
                output => output
                    .MapFrom(p => p.Dimensoes.Profundidade));

        CreateMap<Categoria, CategoriaViewModel>();
    }
}