using Business.Abstract;
using Business.BusinessAspect.Autofac.Secured;
using Core.Extensions;
using Core.Helpers.Business;
using Core.Helpers.Results.Abstract;
using Core.Helpers.Results.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EF;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IResult = Core.Helpers.Results.Abstract.IResult;

namespace Business.Concrete
{
    public class ReportManager : IReportService
    {
        private readonly IAddPhotoHelperService _addPhotoHelperService;
        private readonly IReportDal _reportDal;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ReportManager(IReportDal reportDal, IAddPhotoHelperService addPhotoHelperService, IHttpContextAccessor httpContextAccessor) 
        { 
            _reportDal = reportDal;
            _addPhotoHelperService = addPhotoHelperService;
            _httpContextAccessor = httpContextAccessor;
        }
        public IResult AddReport(ReportDto reportDto)
        {
            var guid = Guid.NewGuid() + "-" + reportDto.Photo.FileName;
            _addPhotoHelperService.AddImage(reportDto.Photo, guid);
            Report report = new() 
            { 
                ReporetedBy = reportDto.ReportedBy,
                ReportedUser = reportDto.ReportedUser,
                PhotoUrl = "/images/" + guid
            };
            _reportDal.Add(report);
            return new SuccessResult("User is succefully reported");
        }
        public IDataResult<List<Report>> GetAllReports()
        {
            var result = _reportDal.GetAll(r => r.IsDeleted == false);
            if (result.Count > 0)
            {
                return new SuccessDataResult<List<Report>>(result, "All reports succesfully fetched");
            }
            else
            {
                return new ErrorDataResult<List<Report>>(result, "There is no reports in database");
            }
        }
        [SecuredOperation("Admin,delete.report")]
        public IResult DeleteReport(int Id)
        {
            bool deleteReportClaim = _httpContextAccessor.HttpContext.User.ClaimRoles().Contains("delete.report");
            var deleteReport = _reportDal.Get(r => r.Id == Id && r.IsDeleted == false);
            if (deleteReport != null && deleteReportClaim)
            {
                deleteReport.IsDeleted = true;
                _reportDal.Delete(deleteReport);
                return new SuccessResult("report has been deleted successfully");
            }
            else
            {
                return new ErrorResult("report is not found or You cannot delete");
            }
        }
    }
}
