using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inzynierka.Data.DbModels;

namespace Inzynierka.Data.HelpModels
{
    public class Interval
    {
        private Dictionary<string, int> SoundTypes { get; set; }
        private Dictionary<string, int> IntervalMarkups { get; set; }
        public string Name { get; set; }
        public int Distance { get; set; }
        public int OctavesDistance { get; set; }
        public int IndexOfFirstSound { get; set; }
        public int IndexOfSecondSound { get; set; }

        public int Key { get; set; }

        public Interval()
        {
            IntervalMarkups = CreateIntervalMarkups();
            SoundTypes = CreateSoundTypes();
        }
        public Interval(string Name, int Distance, int OctavesDistance, int IndexOfFirstSound, int IndexOfSecondSound, int Key)
        {
            this.Name = Name;
            this.Distance = Distance;
            this.OctavesDistance = OctavesDistance;
            this.IndexOfFirstSound = IndexOfFirstSound;
            this.IndexOfSecondSound = IndexOfSecondSound;
            this.Key = Key;
        }

        private Interval calculateInterval(string firstSoundName, string secondSoundName,
            int firstOctaveSymbol, int secondOctaveSymbol, int intervalDifference, int IndexOfFirstSound,
                int IndexOfSecondSound, int Key)
        {
            if (firstSoundName == secondSoundName && firstOctaveSymbol == secondOctaveSymbol)
            {
                return new Interval(IntervalMarkups.First().Key,
                    IntervalMarkups.First().Value, 0,
                    IndexOfFirstSound, IndexOfSecondSound, Key);
            }

            int octaveDifference = firstOctaveSymbol > secondOctaveSymbol ? (firstOctaveSymbol - secondOctaveSymbol) :
                        (secondOctaveSymbol - firstOctaveSymbol);

            if (firstSoundName == secondSoundName && firstOctaveSymbol != secondOctaveSymbol)
            {
                return new Interval(IntervalMarkups.Last().Key, IntervalMarkups.Last().Value, octaveDifference,
                    IndexOfFirstSound, IndexOfSecondSound, Key);
            }

            int indexOfInterval = -1;

            for (int i = 1; i < IntervalMarkups.Count - 1; i++)
            {
                if (intervalDifference > IntervalMarkups.ElementAt(i - 1).Value &&
                    intervalDifference <= IntervalMarkups.ElementAt(i).Value)
                {
                    indexOfInterval = i;
                    break;
                }
            }

            return new Interval(IntervalMarkups.ElementAt(indexOfInterval).Key,
                intervalDifference, octaveDifference, IndexOfFirstSound, IndexOfSecondSound, Key);
        }

        public List<Interval> prepareIntervals(int numberOfIntervalsToCreate, List<Sound> sounds)
        {
            var intervals = new List<Interval>();
            Random rnd = new Random();

            int firstOctaveSymbol, secondOctaveSymbol, firstRandom, secondRandom, countOfSounds = sounds.Count;
            string firstSoundName, secondSoundName;

            int positionOfFirstSoundInArray, positionOfSecondSoundInArray, intervalDifference;

            for (int i = 0; i < numberOfIntervalsToCreate; i++)
            {
                firstRandom = rnd.Next(0, countOfSounds);
                secondRandom = rnd.Next(0, countOfSounds);

                firstSoundName = sounds.ElementAt(firstRandom).Name;
                secondSoundName = sounds.ElementAt(secondRandom).Name;

                firstOctaveSymbol = Convert.ToInt32(sounds.ElementAt(firstRandom).OctaveSymbol);
                secondOctaveSymbol = Convert.ToInt32(sounds.ElementAt(secondRandom).OctaveSymbol);

                positionOfFirstSoundInArray = SoundTypes.SingleOrDefault(x => x.Key == firstSoundName).Value;
                positionOfSecondSoundInArray = SoundTypes.SingleOrDefault(x => x.Key == secondSoundName).Value;

                intervalDifference = positionOfFirstSoundInArray > positionOfSecondSoundInArray ?
                    (positionOfFirstSoundInArray - positionOfSecondSoundInArray) : (positionOfSecondSoundInArray - positionOfFirstSoundInArray);

                var interval = calculateInterval(firstSoundName, secondSoundName, firstOctaveSymbol,
                    secondOctaveSymbol, intervalDifference, firstRandom, secondRandom, i);
                intervals.Add(interval);
            }

            return intervals;
        }


        private Dictionary<string, int> CreateIntervalMarkups()
        {
            var IntervalMarkups = new Dictionary<string, int>();
            IntervalMarkups.Add("Pryma", 0);
            IntervalMarkups.Add("Sekunda", 2);
            IntervalMarkups.Add("Tercja", 4);
            IntervalMarkups.Add("Kwarta", 5);
            IntervalMarkups.Add("Tryton", 6);
            IntervalMarkups.Add("Kwinta", 7);
            IntervalMarkups.Add("Seksta", 9);
            IntervalMarkups.Add("Septyma", 11);
            IntervalMarkups.Add("Oktawa", 12);

            return IntervalMarkups;
        }

        private Dictionary<string, int> CreateSoundTypes()
        {
            var SoundTypes = new Dictionary<string, int>();
            SoundTypes.Add("A",1);
            SoundTypes.Add("Ais", 2);
            SoundTypes.Add("B", 3);
            SoundTypes.Add("C", 4);
            SoundTypes.Add("Cis", 5);
            SoundTypes.Add("D", 6);
            SoundTypes.Add("Dis", 7);
            SoundTypes.Add("E", 8);
            SoundTypes.Add("F", 9);
            SoundTypes.Add("Fis", 10);
            SoundTypes.Add("G", 11);
            SoundTypes.Add("Gis", 12);

            return SoundTypes;
        }
    }
}
