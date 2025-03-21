using Application.Models.Wrapper;
using Application.Pipelines;
using MediatR;

namespace Application.Features.Schools.Commands
{
    public class UpdateSchoolCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public UpdateSchoolRequest UpdateSchoolRequest { get; set; }
    }

    public class UpdateSchoolCommandHandler(ISchoolService schoolService) : IRequestHandler<UpdateSchoolCommand, IResponseWrapper>
    {
        private readonly ISchoolService _schoolService = schoolService;

        public async Task<IResponseWrapper> Handle(UpdateSchoolCommand request, CancellationToken cancellationToken)
        {
            var schoolInDb = await _schoolService.GetSchoolByIdAsync(request.UpdateSchoolRequest.Id);

            schoolInDb.Name = request.UpdateSchoolRequest.Name;
            schoolInDb.EstablishedOn = request.UpdateSchoolRequest.EstablishedOn;

            var schoolId = await _schoolService.UpdateSchoolAsync(schoolInDb);

            return await ResponseWrapper<int>.SuccessAsync(data: schoolId, message: "School Updated Successfully.");

        }
    }
}
