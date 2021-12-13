using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AutoMapper;
using Common.Services;
using DAL.Entities.Courses;
using DAL.Entities.Identity;
using DAL.Repositories; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.Course.Inputs;
using Model.User.Inputs;
using Model.Vedio;
using SQLitePCL;
using Xabe.FFmpeg;

namespace Services.Services
{
    public class VedioService : IVedioService
    {
        private readonly IGenericRepository<CourseSection> _ICourseSectionRepository;
        private readonly IGenericRepository<CourseVedio> _ICourseVedioRepository;

        private readonly IMapper _mapper;

        public VedioService(IMapper mapper, IGenericRepository<CourseSection> ICourseSectionRepository, IGenericRepository<CourseVedio> ICourseVedioRepository)
        { 
            _mapper = mapper;
            _ICourseSectionRepository = ICourseSectionRepository;
            _ICourseVedioRepository = ICourseVedioRepository;
             
        }



        public ResultService<VedioOutput> NeedToUpload(CourseVedio Vedio, CourseFile input, int Teacherid)
        {
            ResultService<VedioOutput> result = new();
            if (Vedio == null)
                return result.SetCode(ResultStatusCode.NotFound).SetMessege("Vedio Not Found");
            if (Vedio.Section.Course.TeacherId != Teacherid)
                return result.SetCode(ResultStatusCode.Unauthorized).SetMessege("You are not the Owner of this course");
            if (input.File == null)
                return result.SetCode(ResultStatusCode.BadRequest).SetMessege("Please select File.");
            return result;
        }

        public async Task<ResultService<VedioOutput>> Update(VedioUpdateInput Input, int Teacherid)
        {
            try
            {

                ResultService<VedioOutput> result = new();
                var vedio = await _ICourseVedioRepository.GetQuery().Where(a => a.Id == Input.Id).Include(b => b.Section).ThenInclude(c => c.Course).FirstOrDefaultAsync();
                if (vedio == null)
                    return result.SetCode(ResultStatusCode.NotFound).SetMessege("Vedio Not  Found");
                if (vedio.Section.Course.TeacherId != Teacherid)
                    return result.SetCode(ResultStatusCode.Unauthorized).SetMessege("You are not the Owner of this course");
                vedio.VedioTitle = Input.VedioTitle;
                vedio.ShortDescription = Input.ShortDescription;
                if (await _ICourseVedioRepository.UpdateAsync(vedio))
                {
                    return result.SetResult(_mapper.Map<CourseVedio, VedioOutput>(vedio));
                }
                else
                {
                    return result.SetCode(ResultStatusCode.BadRequest).SetMessege("Not updated");
                }
            }
            catch
            {
                return ResultService<VedioOutput>.GetErrorResult();
            }
        }
        public async Task<ResultService<VedioOutput>> Create(VedioInput Input, int Teacherid)
        {
            try
            {

                var result = new ResultService<VedioOutput>();
                var Section = await _ICourseSectionRepository.GetQuery().Where(a => a.Id == Input.SectionId).Include(b => b.Course).FirstOrDefaultAsync();
                if (Section == null)
                {
                    return result.SetCode(ResultStatusCode.NotFound).SetMessege("Section Not  Found");
                }
                if (Section.Course.TeacherId != Teacherid)
                    return result.SetCode(ResultStatusCode.Unauthorized).SetMessege("You are not the Owner of this course");

                var vedio = _mapper.Map<VedioInput, CourseVedio>(Input);
                vedio.AddedDate = DateTime.Now;
                if (await _ICourseVedioRepository.CreateAsync(vedio))
                {
                    vedio = await _ICourseVedioRepository.FindAsync(vedio.Id);
                    result.SetResult(_mapper.Map<CourseVedio, VedioOutput>(vedio));
                    return result.SetMessege("Don");
                }
                else
                {
                    return result.SetCode(ResultStatusCode.BadRequest).SetMessege("Not Added");
                }
            }
            catch
            {
                return ResultService<VedioOutput>.GetErrorResult();
            }
        }

        public async Task<ResultService<VedioOutput>> UploadeImageAsync(int Vedioid, CourseFile input, int Teacherid)
        {
            try
            {
                var Vedio = await _ICourseVedioRepository.GetQuery().Where(a => a.Id == Vedioid).Include(b => b.Section).ThenInclude(c => c.Course).FirstOrDefaultAsync();
                var result = NeedToUpload(Vedio, input, Teacherid);
                if (result.Code != ResultStatusCode.Ok) return result;
                var file = input.File;
                var oldimage = Vedio.ImgeURL;
                if (!Directory.Exists("wwwroot/Course" + Vedio.Section.Course.Id + "/Vediosimage/"))
                {
                    Directory.CreateDirectory("wwwroot/Course" + Vedio.Section.Course.Id + "/Vediosimage/");
                }
                var path = Path.Combine("wwwroot/Course" + Vedio.Section.Course.Id + "/Vediosimage/", "PhotoForVedio" + Vedioid + "_" + file.FileName);
                var stream = new FileStream(path, FileMode.Create);
                await file.CopyToAsync(stream);
                await stream.DisposeAsync();
                var newimage = path[7..];
                Vedio.ImgeURL = newimage;
                if (await _ICourseVedioRepository.UpdateAsync(Vedio))
                {
                    if (oldimage != null && !oldimage.Equals(newimage) && System.IO.File.Exists(oldimage))
                        System.IO.File.Delete(oldimage);
                    return result.SetResult(_mapper.Map<CourseVedio, VedioOutput>(Vedio));
                }
                else
                {
                    if (newimage != null && !newimage.Equals(oldimage) && System.IO.File.Exists(newimage))
                        System.IO.File.Delete(newimage);
                    return result.SetCode(ResultStatusCode.BadRequest).SetMessege("Image Not updated");
                }
            }
            catch
            {
                return ResultService<VedioOutput>.GetErrorResult();
            }

        }

        public async Task<ResultService<VedioOutput>> UploadeVedioAsync(int Vedioid, CourseFile input, int Teacherid)
        {
            try
            {
                var Vedio = await _ICourseVedioRepository.GetQuery().Where(a => a.Id == Vedioid).Include(b => b.Section).ThenInclude(c => c.Course).FirstOrDefaultAsync();
                var result = NeedToUpload(Vedio, input, Teacherid);
                if (result.Code != ResultStatusCode.Ok) return result;

                var file = input.File;
                var vedio = await _ICourseVedioRepository.FindAsync(Vedioid);
                var oldvedio = vedio.URL;
                if (!Directory.Exists("wwwroot/Course" + Vedio.Section.Course.Id + "/Vedios/"))
                {
                    Directory.CreateDirectory("wwwroot/Course" + Vedio.Section.Course.Id + "/Vedios/");
                }
                var path = Path.Combine("wwwroot/Course" + Vedio.Section.Course.Id + "/Vedios/", "Vedio" + Vedioid + "_" + file.FileName);
                var stream = new FileStream(path, FileMode.Create);
                await file.CopyToAsync(stream);
                await stream.DisposeAsync();
                var newvedio = path[7..];
                vedio.URL = newvedio;  
                IMediaInfo mediaInfo = await FFmpeg.GetMediaInfo(path); 
                vedio.TimeInSeconds = Convert.ToInt32(Math.Floor(mediaInfo.VideoStreams.First().Duration.TotalSeconds));

                if (await _ICourseVedioRepository.UpdateAsync(vedio))
                {
                    if (oldvedio != null && !oldvedio.Equals(newvedio) && System.IO.File.Exists(oldvedio))
                        System.IO.File.Delete(oldvedio);
                    return result.SetResult(_mapper.Map<CourseVedio, VedioOutput>(Vedio));
                }
                else
                {
                    if (newvedio != null && !newvedio.Equals(oldvedio) && System.IO.File.Exists(newvedio))
                        System.IO.File.Delete(newvedio);
                    return result.SetCode(ResultStatusCode.BadRequest).SetMessege("vedio Not updated");
                }
            }
            catch(Exception e)
            {
                
                var x=e.Message;
                return ResultService<VedioOutput>.GetErrorResult();
            }
        }
        public async Task<ResultService<VedioOutput>> VedioInfo(int Id, string UserId)
        {
            try
            {

                var result = new ResultService<VedioOutput>();
                var vedio = await _ICourseVedioRepository.GetQuery().Where(c => c.Id == Id).Include(c => c.Section).ThenInclude(d => d.Course).ThenInclude(e => e.Teacher).Where(c => c.Section.Course.Teacher.UserId.Equals(UserId) || c.Section.Course.Students.Where(x => x.UserId.Equals(UserId)).Any()).FirstOrDefaultAsync();
                if (vedio != null)
                    return result.SetResult(_mapper.Map<CourseVedio, VedioOutput>(vedio));
                else return result.SetCode(ResultStatusCode.Unauthorized).SetMessege("You do not have this course");
            }
            catch
            {
                return ResultService<VedioOutput>.GetErrorResult();
            }
        }
    }
    public interface IVedioService
    {
        public Task<ResultService<VedioOutput>> Create(VedioInput Input, int Teacherid);
        public Task<ResultService<VedioOutput>> Update(VedioUpdateInput Input, int Teacherid);
        public Task<ResultService<VedioOutput>> UploadeVedioAsync(int Vedioid, CourseFile input, int Teacherid);
        public Task<ResultService<VedioOutput>> UploadeImageAsync(int Vedioid, CourseFile input, int Teacherid);

        public Task<ResultService<VedioOutput>> VedioInfo(int Id, string UserId);

    }
}