using Application.Models.Wrapper;
using Domain.Entities;
using Mapster;
using MediatR;

namespace Application.Features.Schools.Queries
{
    public class GetSchoolByNameQuery : IRequest<IResponseWrapper>
    {
        public string SchoolName { get; set; }
    }

    public class GetSchoolByNameQueryHandler(ISchoolService schoolService) : IRequestHandler<GetSchoolByNameQuery, IResponseWrapper>
    {
        private readonly ISchoolService _schoolService = schoolService;

        public async Task<IResponseWrapper> Handle(GetSchoolByNameQuery request, CancellationToken cancellationToken)
        {
            var schoolInDb = (await _schoolService.GetSchoolByNameAsync(request.SchoolName)).Adapt<SchoolResponse>();

            if (schoolInDb is not null)
            {
                return await ResponseWrapper<SchoolResponse>.SuccessAsync(data: schoolInDb);
            }
            return await ResponseWrapper<SchoolResponse>.FailAsync(message: "School does not exist.");

        }
    }
}
