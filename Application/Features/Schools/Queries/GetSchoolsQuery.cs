using Application.Models.Wrapper;
using Domain.Entities;
using Mapster;
using MediatR;

namespace Application.Features.Schools.Queries
{
    public class GetSchoolsQuery : IRequest<IResponseWrapper>
    {

    }

    public class GetSchoolsQueryHandler(ISchoolService schoolService) : IRequestHandler<GetSchoolsQuery, IResponseWrapper>
    {
        private readonly ISchoolService _schoolService = schoolService;

        public async Task<IResponseWrapper> Handle(GetSchoolsQuery request, CancellationToken cancellationToken)
        {
            var schools = (await _schoolService.GetSchoolsAsync()).Adapt<List<SchoolResponse>>();
            if (schools.Count > 0)
            {
                return await ResponseWrapper<List<SchoolResponse>>.SuccessAsync(data: schools);
            }
            return await ResponseWrapper<SchoolResponse>.FailAsync(message: "No schools were found.");

        }
    }
}
