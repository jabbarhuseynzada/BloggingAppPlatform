using Core.Helpers.Results.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IReportService
    {
        public IResult AddReport(ReportDto reportDto);
        public IDataResult<List<Report>> GetAllReports();
        public IResult DeleteReport(int Id);
    }
}
