using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Inzynierka.Data.DbModels;
using Inzynierka.Data.Dtos;
using Inzynierka.Data.HelpModels;
using System.IO.Compression;
using Inzynierka.Data.ViewModels;
using Inzynierka.Repository.Interfaces;
using Inzynierka.Services.Interfaces;
using System.Linq;

namespace Inzynierka.Services.Services
{
    public class SoundService:ISoundService
    {
        private readonly IRepository<Sound> _soundRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfigurationManager _configurationManager;
        private readonly string pathToSaveZippedSounds = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "sounds.zip");
        private readonly string pathToSaveZippedIntervals = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "intervals.zip");

        private readonly Interval mainInterval = new Interval();

        private readonly int MinimumNumberOfSoundsAndChords = 10;
        private readonly int MinimumNumberOfSoundsInMixedType = 25;
        private readonly int MinimumNumberOfSoundsToGetIntervals = 12;

        private readonly List<SoundsFolderSettings> SoundTypes = new List<SoundsFolderSettings>()
        {
            new SoundsFolderSettings(10, "sound"),
            new SoundsFolderSettings(20, "chord")
        };
        public SoundService(IRepository<User> userRepository, IRepository<Sound> soundRepository, IMapper mapper, IConfigurationManager configurationManager)
        {
            _userRepository = userRepository;
            _soundRepository = soundRepository;
            _mapper = mapper;
            _configurationManager = configurationManager;

        }

        private void CreateDirIfDoesntExist(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private FoldersInfo countFilesInFoldersByFolderNameArray()
        {
            Dictionary<string, int> NumberOfFilesByCategory = new Dictionary<string, int>();

            foreach (var type in SoundTypes)
            {
                int numberOfSounds = _soundRepository.GetAllBy(x => x.Category == type.SoundType).Count();
                NumberOfFilesByCategory.Add(type.SoundType, numberOfSounds);
            }
            return new FoldersInfo(NumberOfFilesByCategory, 5);
        }
        private bool isTypeValid(string type)
        {
            return SoundTypes.Exists(x => x.SoundType == type);
        }

        private string createIntervalZipFileName(string currentFileName, 
            string intervalName, string octavesDistance, string key, string loopIdentifier)
        {
            return currentFileName + "_" + intervalName + "_" + octavesDistance + "_" + key + "_" + loopIdentifier + ".mp3";
        }

        public async Task<ResultDto<GetZippedSoundsDto>> DownloadZippedIntervalsByType(string type, int numberOfIntervalsToCreate)
        {
            var result = new ResultDto<GetZippedSoundsDto>();

            if (!isTypeValid(type) || numberOfIntervalsToCreate <= 0)
            {
                result.Errors.Add("Podane parametry są niezgodne z wytycznymi");
                return result;
            }

            var soundInfosFromDB = await Task.Run(() =>
                _soundRepository.GetAllBy(x => x.Category == type).ToList()  
            ); 

            if(soundInfosFromDB.Count < MinimumNumberOfSoundsToGetIntervals)
            {
                result.Errors.Add("Liczba dźwięków do stworzenia interwałów jest zbyt mała");
                return result;
            }

            var intervals = await Task.Run(() =>
                mainInterval.prepareIntervals(numberOfIntervalsToCreate, soundInfosFromDB)
            );

            if (File.Exists(pathToSaveZippedIntervals))
            {
                File.Delete(pathToSaveZippedIntervals);
            }

            using (ZipArchive zip = ZipFile.Open(pathToSaveZippedIntervals, ZipArchiveMode.Create))
            {
                int acc = 0;
                foreach (var interval in intervals)
                {
                    string firstSoundNameInZip = createIntervalZipFileName(soundInfosFromDB.ElementAt(interval.IndexOfFirstSound).Name,
                        interval.Name, interval.OctavesDistance.ToString(), interval.Key.ToString(),
                        acc.ToString());
                    acc++;
                    string secondSoundNameInZip = createIntervalZipFileName(soundInfosFromDB.ElementAt(interval.IndexOfSecondSound).Name,
                        interval.Name, interval.OctavesDistance.ToString(), interval.Key.ToString(),
                        acc.ToString());
                    acc++;

                    string pathToGetFirstFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "sounds",
                        soundInfosFromDB.ElementAt(interval.IndexOfFirstSound).FullName);

                    string pathToGetSecondFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "sounds",
                        soundInfosFromDB.ElementAt(interval.IndexOfSecondSound).FullName);

                    zip.CreateEntryFromFile(pathToGetFirstFile, firstSoundNameInZip);
                    zip.CreateEntryFromFile(pathToGetSecondFile, secondSoundNameInZip);

                }
                result.SuccessResult = new GetZippedSoundsDto() { Path = pathToSaveZippedIntervals };
            }
            return result;
        }

        public async Task<ResultDto<GetSoundsByCategoryDto>> GetSoundsAndChords()
        {
            var result = new ResultDto<GetSoundsByCategoryDto>();

            string pathToGetFiles = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "sounds");

            if (!Directory.Exists(pathToGetFiles))
            {
                result.Errors.Add("Brak folderu z próbkami dźwiękowymi. Prawdopodobnie nie dodano jeszcze żadnych");

                return result;
            }
            var files = await Task.Run(() => _soundRepository.GetAll().ToList());
            int numberOfFiles = files.Count();
            Random rnd = new Random();
            List<string> listOfSoundNames = new List<string>();

            for (int i = 0; i < MinimumNumberOfSoundsInMixedType; i++)
            {
                int randomizedIndex = rnd.Next(0, numberOfFiles);
                listOfSoundNames.Add(files.ElementAt(randomizedIndex).FullName);
            }

            result.SuccessResult = new GetSoundsByCategoryDto() { SoundNames = listOfSoundNames };

            return result;
        }

        public async Task<ResultDto<GetSoundsByCategoryDto>> GetSoundNamesByCategory(string category)
        {
            var result = new ResultDto<GetSoundsByCategoryDto>();

            if (!isTypeValid(category))
            {
                result.Errors.Add("Rodzaj trybu jest niezgodny ze zdefiniowanymi typami");

                return result;
            }

            var soundsByType = await Task.Run(() => _soundRepository.GetAllBy(x => x.Category == category).ToList());

            if (soundsByType.Count < MinimumNumberOfSoundsAndChords)
            {
                result.Errors.Add("Ta gra nie może być rozpoczęta ponieważ system nie posiada wystarczającej ilości próbek dźwiękowych dla tej kategorii");
                return result;
            }
         
            Random rnd = new Random();

            var soundsNames = new List<string>();

            int numberOfFilesToTake = SoundTypes.Single(x => x.SoundType == category).NumberOfFiles;

            for (int i = 0; i < numberOfFilesToTake; i++)
            {
                int randomizedIndex = rnd.Next(0, soundsByType.Count());
                var name = soundsByType.ElementAt(randomizedIndex).FullName;
                soundsNames.Add(soundsByType.ElementAt(randomizedIndex).FullName);
            }

            result.SuccessResult = new GetSoundsByCategoryDto() { SoundNames = soundsNames };

            return result;

        }
       
        public async Task<ResultDto<UploadSoundDto>> UploadSounds(SoundViewModel viewModel, int userId)
        {
            var result = new ResultDto<UploadSoundDto>();

            if (viewModel.Sound.Length == 0)
            {
                result.Errors.Add("Nie przesłano pliku");

                return result;
            }

            if (viewModel.Sound.ContentType != "audio/mpeg")
            {
                result.Errors.Add("Przesyłany plik nie jest plikiem formatu audio/mpeg");

                return result;
            }

            var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            CreateDirIfDoesntExist(rootPath);

            var soundFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "sounds");
            CreateDirIfDoesntExist(soundFolderPath);

            var files = await Task.Run(() => new DirectoryInfo(soundFolderPath).GetFiles());

            string soundPosition = viewModel.SoundPosition == null ? "" : "_" + viewModel.SoundPosition.ToString();

            string combinedSoundName = viewModel.Name + "_" + viewModel.Category + "_" + viewModel.GuitarString.ToString() + soundPosition + ".mp3";

            foreach (var file in files)
            {
                if(file.Name == combinedSoundName)
                {
                    result.Errors.Add("Ta kategoria posiada już ten dźwięk");

                    return result;
                }
            }

            var pathToSaveSound = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "sounds", combinedSoundName);

            using (var stream = new FileStream(pathToSaveSound, FileMode.Create))
            {
                await viewModel.Sound.CopyToAsync(stream);
            }

            var sound = _mapper.Map<Sound>(viewModel);
            sound.UserId = userId;
            sound.FullName = combinedSoundName;

            int isInserted = await _soundRepository.Insert(sound);

            if (isInserted == 0)
            {
                result.Errors.Add("Wystapił błąd podczas dodawania pliku " + sound.Name);

                return result;
            }

            return result;
        }

    }
}
