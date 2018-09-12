using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Inzynierka.Data.DbModels;
using Inzynierka.Data.Dtos;
using Inzynierka.Data.HelpModels;
using Inzynierka.Repository.Interfaces;
using Inzynierka.Services.Interfaces;

namespace Inzynierka.Services.Services
{
    public class MotiveService:IMotiveService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Motive> _motiveRepository;
        private readonly IRepository<SharedMotives> _sharedMotivesRepository;
        private readonly IRepository<User> _userRepository;
        public MotiveService(IRepository<Motive> motiveRepository, IRepository<SharedMotives> sharedMotivesRepository, 
            IRepository<User> userRepository, IMapper mapper)
        {
            _mapper = mapper;
            _motiveRepository = motiveRepository;
            _sharedMotivesRepository = sharedMotivesRepository;
            _userRepository = userRepository;
        }

        public async Task<ResultDto<MotiveDto>> AddMotive(MotiveViewModel viewModel, int userId)
        {
            var result = new ResultDto<MotiveDto>();

            bool isMotiveExist = _motiveRepository.Exist(x => x.UserId == userId && (x.Name == viewModel.Name || 
                (x.MainColor == viewModel.MainColor && x.FontColor == viewModel.FontColor)));

            if (isMotiveExist)
            {
                result.Errors.Add("W twojej kolekcji motywów istnieje już motyw o podanych parametrach");
                return result;
            }

            var motive = _mapper.Map<MotiveViewModel, Motive>(viewModel);
            motive.UserId = userId;

            var insertedMotive = await _motiveRepository.InsertAndReturnObject(motive);

            if(insertedMotive == null)
            {
                result.Errors.Add("Wystąpił błąd podczas dodawania motywu");
                return result;
            }

            result.SuccessResult = _mapper.Map<Motive, MotiveDto>(motive);

            return result;
        }

        public async Task<ResultDto<MotiveDto>> EditMotive(MotiveViewModel viewModel, int motiveId, int UserId)
        {
            var result = new ResultDto<MotiveDto>();

            var motive = await Task.Run(() => _motiveRepository.GetBy(x => x.Id == motiveId && x.UserId == UserId));
            
            if(motive == null)
            {
                result.Errors.Add("Motyw o podanych parametrach nie został odnaleziony");
                return result;
            }

            motive.MainColor = viewModel.MainColor;
            motive.FontColor = viewModel.FontColor;
            motive.Name = viewModel.Name;

            var isUpdated = await Task.Run(() => _motiveRepository.Update(motive));

            if(isUpdated == 0)
                result.Errors.Add("Wystąpił problem podczas edycji motywu");

            return result;
            
        }
        public async Task<ResultDto<MotiveDto>> DeleteMotive(int motiveId, int UserId)
        {
            var result = new ResultDto<MotiveDto>();

            int isMotiveDeleted = await Task.Run(() => _motiveRepository.Delete(x => x.Id == motiveId && x.UserId == UserId));

            if(isMotiveDeleted == 0)
                result.Errors.Add("Wystąpił błąd podczas usuwania motywu");

            return result;

        }

        public async Task<ResultDto<MotiveDto>> ShareMotive(int userToShareId, int userWhichSharingId, int motiveId)
        {
            var result = new ResultDto<MotiveDto>();

            if(userToShareId == userWhichSharingId)
            {
                result.Errors.Add("Nie można udostępnić motywu samemu sobie");
                return result;
            }

            bool isUserToShareExist = _userRepository.Exist(x => x.Id == userToShareId && x.IsAcceptedRegister);

            if (!isUserToShareExist)
            {
                result.Errors.Add("Użytkownik dla którego ma zostać udostępniony motyw, nie istnieje");
                return result;
            }

            bool isMotiveExist = _motiveRepository.Exist(x => x.Id == motiveId && 
                x.UserId == userWhichSharingId);

            if (!isMotiveExist)
            {
                result.Errors.Add("Motyw o podanych parametrach nie istnieje");
                return result;
            }

            bool isMotiveAlreadyShared = await Task.Run(() => _sharedMotivesRepository.Exist(x => x.MotiveId == motiveId &&
                x.UserId == userToShareId));

            if (isMotiveAlreadyShared)
            {
                result.Errors.Add("Motyw o podancyh parametrach został już udostępniony temu użytkownikowi");
                return result;
            }

            var newSharedMotive = new SharedMotives();
            newSharedMotive.MotiveId = motiveId;
            newSharedMotive.UserId = userToShareId;

            int isInserted = await _sharedMotivesRepository.Insert(newSharedMotive);

            if(isInserted == 0)
            {
                result.Errors.Add("Wystąpił problem podczas udostępniania motywu");
            }

            return result;
        }

        // Share motywow/ pobieranie share motywow
    }
}
