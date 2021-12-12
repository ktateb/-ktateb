using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Common.Services;
using DAL.Entities.Courses;
using DAL.Entities.Identity;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.Course.Inputs;
using Model.User.Inputs;
using Model.Vedio;

namespace Services.Services
{
    public class VedioService : IVedioService
    {
        private readonly IGenericRepository<CourseSection> _ICourseSectionRepository;
        private readonly IGenericRepository<CourseVedio> _ICourseVedioRepository;

        public VedioService(IGenericRepository<CourseSection> ICourseSectionRepository, IGenericRepository<CourseVedio> ICourseVedioRepository)
        {
            _ICourseSectionRepository = ICourseSectionRepository;
            _ICourseVedioRepository = ICourseVedioRepository;
        }

        public ResultService<bool> NeedToUpload(CourseVedio Vedio, CourseFile input, int Teacherid)
        {
            ResultService<bool> result = new();
            if (Vedio == null)
                return result.SetResult(false).SetCode(ResultStatusCode.NotFound).SetMessege("Vedio Not Found");
            if (Vedio.Section.Course.TeacherId != Teacherid)
                return result.SetResult(false).SetCode(ResultStatusCode.Unauthorized).SetMessege("You are not the Owner of this course");
            if (input.File == null)
                return result.SetCode(ResultStatusCode.BadRequest).SetMessege("Please select File.");
            return result;
        }
        public async Task<ResultService<bool>> UploadeImage(int Vedioid, CourseFile input, int Teacherid)
        {
            try
            {
                var Vedio = await _ICourseVedioRepository.GetQuery().Where(a => a.Id == Vedioid).Include(b => b.Section).ThenInclude(c => c.Course).FirstOrDefaultAsync();
                var result = NeedToUpload(Vedio, input, Teacherid);
                if (result.Code != ResultStatusCode.Ok) return result;

                var file = input.File;
                var vedio = await _ICourseVedioRepository.FindAsync(Vedioid);
                var oldimage = vedio.ImgeURL;
                var path = Path.Combine("wwwroot/Course" + Vedio.Section.Course.Id + "/Vediosimage/", "PhotoForVedio" + Vedioid + "_" + file.FileName);
                var stream = new FileStream(path, FileMode.Create);
                await file.CopyToAsync(stream);
                await stream.DisposeAsync();
                var newimage = path[7..];
                vedio.ImgeURL = newimage;
                if (await _ICourseVedioRepository.UpdateAsync(vedio))
                {
                    if (oldimage != null && !oldimage.Equals(newimage) && System.IO.File.Exists(oldimage))
                        System.IO.File.Delete(oldimage);
                    return result.SetResult(true);
                }
                else
                {
                    if (newimage != null && !newimage.Equals(oldimage) && System.IO.File.Exists(newimage))
                        System.IO.File.Delete(newimage);
                    return result.SetResult(false).SetCode(ResultStatusCode.BadRequest).SetMessege("Image Not updated");
                }
            }
            catch
            {
                return ResultService<bool>.GetErrorResult().SetResult(false);
            }

        }

        public async Task<ResultService<bool>> UploadeVedio(int Vedioid, CourseFile input, int Teacherid)
        {
            try
            {
                var Vedio = await _ICourseVedioRepository.GetQuery().Where(a => a.Id == Vedioid).Include(b => b.Section).ThenInclude(c => c.Course).FirstOrDefaultAsync();
                var result = NeedToUpload(Vedio, input, Teacherid);
                if (result.Code != ResultStatusCode.Ok) return result;

                var file = input.File;
                var vedio = await _ICourseVedioRepository.FindAsync(Vedioid);
                var oldvedio = vedio.URL;
                var path = Path.Combine("wwwroot/Course" + Vedio.Section.Course.Id + "/Vedios/", "Vedio" + Vedioid + "_" + file.FileName);
                var stream = new FileStream(path, FileMode.Create);
                await file.CopyToAsync(stream);
                await stream.DisposeAsync();
                var newvedio = path[7..];
                vedio.URL = newvedio;
                if (await _ICourseVedioRepository.UpdateAsync(vedio))
                {
                    if (oldvedio != null && !oldvedio.Equals(newvedio) && System.IO.File.Exists(oldvedio))
                        System.IO.File.Delete(oldvedio);
                    return result.SetResult(true);
                }
                else
                {
                    if (newvedio != null && !newvedio.Equals(oldvedio) && System.IO.File.Exists(newvedio))
                        System.IO.File.Delete(newvedio);
                    return result.SetResult(false).SetCode(ResultStatusCode.BadRequest).SetMessege("vedio Not updated");
                }
            }
            catch
            {
                return ResultService<bool>.GetErrorResult().SetResult(false);
            }
        }
        public async Task<ResultService<CourseVedio>> VedioInfo(int Id, string UserId)
        {
            var result = new ResultService<CourseVedio>();
            var vedio = await _ICourseVedioRepository.GetQuery().Where(c => c.Id == Id).Include(c => c.Section).ThenInclude(d => d.Course).Where(c => c.Section.Course.Students.Where(x => x.UserId.Equals(UserId)).Any()).FirstOrDefaultAsync();
            if (vedio != null)
                return result.SetResult(vedio);
            else return result.SetCode(ResultStatusCode.Unauthorized);
        }
    }
    public interface IVedioService
    {
        public Task<ResultService<bool>> UploadeVedio(int Vedioid, CourseFile input, int Teacherid);
        public Task<ResultService<bool>> UploadeImage(int Vedioid, CourseFile input, int Teacherid);
        public Task<ResultService<CourseVedio>> VedioInfo(int Id, string UserId);

    }
}