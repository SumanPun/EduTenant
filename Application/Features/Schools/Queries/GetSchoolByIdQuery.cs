﻿using Application.Models.Wrapper;
using Domain.Entities;
using Mapster;
using MediatR;

namespace Application.Features.Schools.Queries
{
    public class GetSchoolByIdQuery : IRequest<IResponseWrapper>
    {
        public int SchoolId { get; set; }
    }

    public class GetSchoolByIdQueryHandler(ISchoolService schoolService) : IRequestHandler<GetSchoolByIdQuery, IResponseWrapper>
    {
        private readonly ISchoolService _schoolService = schoolService;

        public async Task<IResponseWrapper> Handle(GetSchoolByIdQuery request, CancellationToken cancellationToken)
        {
            var schoolInDb = (await _schoolService.GetSchoolByIdAsync(request.SchoolId)).Adapt<SchoolResponse>();
            if(schoolInDb is not null)
            {
                return await ResponseWrapper<SchoolResponse>.SuccessAsync(data: schoolInDb);
            }
            return await ResponseWrapper<SchoolResponse>.FailAsync(message: "School does not exist.");
        }
    }
}
