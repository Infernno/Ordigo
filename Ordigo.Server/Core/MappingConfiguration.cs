using AutoMapper;
using Ordigo.Server.Core.Data.Entities;
using Ordigo.Server.Core.Requests;

namespace Ordigo.Server.Core
{
    // ReSharper disable once UnusedMember.Global
    public class MappingConfiguration : Profile
    {
        public MappingConfiguration()
        {
            CreateMap<SignUpRequest, Account>();

            CreateMap<TextNoteRequest, TextNote>();
        }
    }
}
